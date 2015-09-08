Imports Ferreyros.Utiles

Public Class _Default
    Inherits System.Web.UI.Page

    Private pr_strUsuario As String = ""
    Private pr_strSistema As String = ""
    Private pr_strFormulario As String = ""
    Private pr_strSistemaID As String = ""
    Private pr_strLogin As String = ""
    Private pr_strPassword As String = ""

    ' CONTEXTO DEL USUARIO
    Private miContexto As Ferreyros.Base.Interfase.MiUsuario

    ' OBJETOS DE SEGURIDAD
    Private objUsu As Ferreyros.Base.Negocio.Usuario

    ' DELEGADO DE CONTEXTO
    Shared reason As CacheItemRemovedReason
    Private onRemove As CacheItemRemovedCallback
    Private oUtiles As Ferreyros.Utiles.uConfiguracion = Nothing
    Private objMenu As Ferreyros.Base.Negocio.Menu

    Public Sub RemovedCallback(ByVal k As String, ByVal v As Object, ByVal r As CacheItemRemovedReason)
        reason = r
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        pr_strFormulario = ConfigurationManager.AppSettings.Get("FormularioID").ToString
        pr_strSistemaID = ConfigurationManager.AppSettings.Get("SistemaID").ToString
        pr_strSistema = ConfigurationManager.AppSettings.Get("Sistema").ToString

        If Request("txhInicio") <> Nothing Then
            pr_strUsuario = Request("txhInicio").ToString.Trim
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        oUtiles = New uConfiguracion
        Dim mensaje As String = String.Empty
        oUtiles.extraerAppSettings("MensajeDerechos", mensaje)
        idDerechos.Text = mensaje
        ' INICIO DE CONTEXTO
        miContexto = New Ferreyros.Base.Interfase.MiUsuario(pr_strLogin)

        ' INICIO DE SESSION
        Session("MiUsuario") = Session.SessionID

        ' LIMPIO SESSION
        If Not Cache(Session.SessionID) Is Nothing Then
            Cache.Remove(Session.SessionID)
        End If

        ' INICIO DE INSTANCIA
        objUsu = New Ferreyros.Base.Negocio.Usuario(miContexto)

        ' NOMBRE DEL SISTEMA
        miContexto.SistemaID = pr_strSistemaID
        miContexto.Libreria = ConfigurationManager.AppSettings.Get("Libreria").ToString
        miContexto.FormularioID = ConfigurationManager.AppSettings.Get("FormularioID").ToString

        If Not Page.IsPostBack Then
            ' CARGA DE DATOS DEL USUARIO AUTENTICADO
            If pr_strUsuario.ToString.Trim = "" Then
                pr_strLogin = Request.ServerVariables("LOGON_USER")
                pr_strLogin = pr_strLogin.Substring(pr_strLogin.IndexOf("\") + 1, (pr_strLogin.Length - 1) - pr_strLogin.IndexOf("\")).ToUpper.Trim

                miContexto.login = pr_strLogin '"rsarmiento" 'pr_strLogin ' 
                InicioUsuario()
            Else
                pr_strLogin = pr_strUsuario

                ' INICIO DE CONTEXTO
                miContexto.login = pr_strLogin

                ' INICIO DE INSTANCIA
                objUsu = New Ferreyros.Base.Negocio.Usuario(miContexto)
            End If
        End If

    End Sub

    Private Sub InicioUsuario()
        ' DELEGADO DE MEMORIA CACHE
        onRemove = New CacheItemRemovedCallback(AddressOf Me.RemovedCallback)
        Cache.Remove(Session("MiUsuario"))
        Try
            Dim pstrResul As String = objUsu.InicioContexto("strSeguridad", "strAdryan")

            Select Case pstrResul.ToString.Trim
                Case ""
                    ' EN CACHE DE MEMORIA
                    Cache.Add(Session("MiUsuario"), _
                              miContexto, _
                              Nothing, _
                              Date.Now.AddHours(6), _
                              System.Web.Caching.Cache.NoSlidingExpiration, _
                              CacheItemPriority.High, onRemove)
                    Call formarMenu()
                    'Response.Redirect("Inicio.htm", False)
                    Response.Redirect("Inicio.aspx", False)
                Case "-1"
                    SRT_Exception(pstrResul)
                Case Else
                    txtUsuario.Text = pr_strLogin
                    txtMsg.Text = pstrResul
            End Select
        Catch ex As Exception
            SRT_Exception(ex)
        End Try
    End Sub

    Private Sub formarMenu()

        Dim clsMain As New main
        Try
            objMenu = New Ferreyros.Base.Negocio.Menu
            Dim ds As New DataSet
            Dim doMenu As New Ferreyros.Base.Interfase.dsSeguridad.MenuDataTable
            Dim l_Menu As New List(Of clsMenu)
            Dim eMenu As clsMenu = Nothing
            oUtiles = New Ferreyros.Utiles.uConfiguracion

            doMenu.CodUsuarioColumn.DefaultValue = miContexto.login
            doMenu.CodSistemaColumn.DefaultValue = miContexto.SistemaID
            doMenu.CodFormularioColumn.DefaultValue = miContexto.FormularioID
            doMenu.HabilitadoColumn.DefaultValue = "1"
            doMenu.VisibleColumn.DefaultValue = "1"
            ds = objMenu.ConsultaMenuUsuario("strSeguridad", doMenu)

            For Each Fila In ds.Tables(0).Rows
                eMenu = New clsMenu
                If Not IsDBNull(Fila("IdMenu")) Then eMenu.IdMenu = Fila("IdMenu")
                If Not IsDBNull(Fila("Descripcion")) Then eMenu.Descripcion = Fila("Descripcion")
                If Not IsDBNull(Fila("Informativo")) Then eMenu.Informativo = Fila("Informativo")
                If Not IsDBNull(Fila("Url")) Then eMenu.Url = Fila("Url")
                If Not IsDBNull(Fila("IdPadre")) Then eMenu.IdPadre = Fila("IdPadre")
                If Not IsDBNull(Fila("Habilitado")) Then eMenu.Habilitado = Fila("Habilitado")
                If Not IsDBNull(Fila("Visible")) Then eMenu.Visible = Fila("Visible")
                If Not IsDBNull(Fila("Imagen")) Then eMenu.Imagen = Fila("Imagen")
                If Not IsDBNull(Fila("Destino")) Then eMenu.Destino = Fila("Destino")
                If eMenu.Habilitado.Trim = "1" AndAlso eMenu.Visible.Trim = "1" Then
                    eMenu.IdMenu = eMenu.IdMenu.Trim
                    eMenu.Descripcion = eMenu.Descripcion.Trim
                    eMenu.Informativo = eMenu.Informativo.Trim
                    eMenu.Url = eMenu.Url.Trim
                    eMenu.IdPadre = eMenu.IdPadre.Trim
                    eMenu.Imagen = eMenu.Imagen.Trim
                    eMenu.Destino = eMenu.Destino.Trim
                    l_Menu.Add(eMenu)
                End If
            Next

            Dim colMenu As New MenuItemCollection
            Dim l_Padre = l_Menu.Where(Function(Rpt) Rpt.IdPadre = String.Empty).ToList
            l_Padre = l_Padre.OrderBy(Function(rpt) rpt.IdMenu).ToList()
            'recorrer padres
            For Each RowPadre In l_Padre
                Dim itemPadre As New MenuItem
                itemPadre.Value = RowPadre.IdMenu
                itemPadre.Text = RowPadre.Descripcion
                itemPadre.ToolTip = RowPadre.Informativo
                itemPadre.NavigateUrl = RowPadre.Url
                itemPadre.ImageUrl = RowPadre.Imagen
                'recorrer hijos
                Dim l_Hijo = l_Menu.Where(Function(Rpt) Rpt.IdPadre = itemPadre.Value).ToList
                l_Hijo = l_Hijo.OrderBy(Function(rpt) rpt.IdMenu).ToList()
                For Each RowHijo In l_Hijo
                    Dim itemHijo As New MenuItem
                    itemHijo.Value = RowHijo.IdMenu
                    itemHijo.Text = RowHijo.Descripcion
                    itemHijo.ToolTip = RowHijo.Informativo
                    itemHijo.NavigateUrl = RowHijo.Url
                    itemHijo.ImageUrl = RowHijo.Imagen
                    itemPadre.ChildItems.Add(itemHijo)
                    'recorrer nietos
                    Dim l_Nieto = l_Menu.Where(Function(Rpt) Rpt.IdPadre = itemHijo.Value).ToList
                    l_Nieto = l_Nieto.OrderBy(Function(rpt) rpt.IdMenu).ToList()
                    For Each RowNieto In l_Nieto
                        Dim itemNieto As New MenuItem
                        itemNieto.Value = RowNieto.IdMenu
                        itemNieto.Text = RowNieto.Descripcion
                        itemNieto.ToolTip = RowNieto.Informativo
                        itemNieto.NavigateUrl = RowNieto.Url
                        itemNieto.ImageUrl = RowNieto.Imagen
                        itemHijo.ChildItems.Add(itemNieto)
                    Next
                Next
                colMenu.Add(itemPadre)
            Next
            'almacenar en la session
            Session("Menu_Master") = colMenu
            strCadenaAleatoria = oUtiles.fc_cadenaAleatoria()
        Catch ex As Exception
            clsMain.Msg_Exception(ex)
        End Try

    End Sub

    Private Sub SRT_Exception(ByRef Exp As Exception)
        Session("MsgSource") = Exp.Source
        Session("MsgError") = Exp.Message
        Response.Redirect("~/Error.aspx", False)
    End Sub

    Private Sub SRT_Exception(ByVal mensaje As String)
        Session("MsgSource") = "Error en Carga"
        Session("MsgError") = mensaje
        Response.Redirect("~/Error.aspx", False)
    End Sub

    Protected Sub imgIngresar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgIngresar.Click

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If hddValidacion.Value <> "OK" Then
            Return
        End If

        pr_strLogin = txtUsuario.Text.ToString.Trim
        pr_strPassword = txtPassword.Text.ToString.Trim

        Dim resul As String = ""
        Dim lAutenticacionLDAP As Boolean = objUsu.AutenticarLDAP(pr_strLogin, pr_strPassword)

        miContexto.login = pr_strLogin

        If lAutenticacionLDAP Then
            resul = "0"
        Else
            resul = objUsu.ValidarUsuario("strSeguridad", pr_strLogin, pr_strPassword)
        End If

        Select Case resul
            Case "0"
                InicioUsuario()
            Case "1"
                txtMsg.Text = "Error en clave, intente otra vez!"
                txtUsuario.Focus()
            Case "2"
                txtMsg.Text = "Expiro el nombre usuario."
            Case "3"
                txtMsg.Text = "Clave de usuario expiró."
            Case "4"
                txtMsg.Text = "Usuario no existe."
            Case "5"
                txtMsg.Text = "Cuenta de usuario bloqueado."
        End Select

    End Sub

    Protected Sub imgCerrar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCerrar.Click

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        Cache.Remove(Session.SessionID)
        Dim script As String = String.Empty
        script &= " <script language='javascript'>"
        script &= " opener=null;"
        script &= " window.close();"
        script &= " </script>"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "WClose", script, False)

    End Sub

    Private Class clsMenu

        Private _IdMenu As String
        Public Property IdMenu() As String
            Get
                Return _IdMenu
            End Get
            Set(ByVal value As String)
                _IdMenu = value
            End Set
        End Property
        Private _Descripcion As String
        Public Property Descripcion() As String
            Get
                Return _Descripcion
            End Get
            Set(ByVal value As String)
                _Descripcion = value
            End Set
        End Property
        Private _Informativo As String
        Public Property Informativo() As String
            Get
                Return _Informativo
            End Get
            Set(ByVal value As String)
                _Informativo = value
            End Set
        End Property
        Private _Url As String
        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal value As String)
                _Url = value
            End Set
        End Property
        Private _IdPadre As String
        Public Property IdPadre() As String
            Get
                Return _IdPadre
            End Get
            Set(ByVal value As String)
                _IdPadre = value
            End Set
        End Property
        Private _Habilitado As String
        Public Property Habilitado() As String
            Get
                Return _Habilitado
            End Get
            Set(ByVal value As String)
                _Habilitado = value
            End Set
        End Property
        Private _Visible As String
        Public Property Visible() As String
            Get
                Return _Visible
            End Get
            Set(ByVal value As String)
                _Visible = value
            End Set
        End Property
        Private _Imagen As String
        Public Property Imagen() As String
            Get
                Return _Imagen
            End Get
            Set(ByVal value As String)
                _Imagen = value
            End Set
        End Property
        Private _Destino As String
        Public Property Destino() As String
            Get
                Return _Destino
            End Get
            Set(ByVal value As String)
                _Destino = value
            End Set
        End Property

        Sub New()

            _IdMenu = String.Empty
            _Descripcion = String.Empty
            _Informativo = String.Empty
            _Url = String.Empty
            _IdPadre = String.Empty
            _Habilitado = String.Empty
            _Visible = String.Empty
            _Imagen = String.Empty
            _Destino = String.Empty

        End Sub

    End Class

End Class