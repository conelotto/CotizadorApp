Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros
Imports System.Globalization

Public Class frmAdmTablasImpresion
    Inherits System.Web.UI.Page

#Region "---------- Declaracion ----------"

    Private miContexto As Base.Interfase.MiUsuario
    Private Shared lResponse As JQGridJsonResponse = Nothing
    Private Shared eValidacion As beValidacion = Nothing
    Private Shared eTablaMaestra As beTablaMaestra = Nothing
    Private Shared cTablaMaestra As bcTablaMaestra = Nothing
    Private Shared l_Secciones As List(Of beTablaMaestra) = Nothing
    Private Shared l_SeccionCriterio As List(Of beTablaMaestra) = Nothing

    Private Shared Property Session_Secciones() As Object
        Get
            Return HttpContext.Current.Session("Secciones")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("Secciones") = value
        End Set
    End Property
    Private Shared Property Session_Usuario() As Object
        Get
            Return HttpContext.Current.Session("Usuario")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("Usuario") = value
        End Set
    End Property
    Private Shared Property Session_SeccionCriterio() As Object
        Get
            Return HttpContext.Current.Session("SeccionCriterio")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("SeccionCriterio") = value
        End Set
    End Property
    
#End Region

#Region "----------   Eventos   ----------"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If Not IsPostBack Then
            Session_Usuario = CType(Cache(Session("MiUsuario")), Base.Interfase.MiUsuario).login
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        Dim AccesoValido As Boolean
        If Session("MiUsuario") IsNot Nothing Then
            miContexto = CType(Cache(Session("MiUsuario")), Base.Interfase.MiUsuario)
            AccesoValido = miContexto IsNot Nothing
        End If

        If Not AccesoValido Then
            Response.Redirect("~/Default.aspx", True)
        End If

    End Sub

#End Region

#Region "-----------  Metodos  -----------"

    Private Shared Sub mtd_ConsultarTablaImpresion(ByVal Tabla As String)

        eValidacion = New beValidacion
        cTablaMaestra = New bcTablaMaestra
        l_Secciones = New List(Of beTablaMaestra)
        l_SeccionCriterio = New List(Of beTablaMaestra)

        Session_Secciones = Nothing
        eValidacion.flag = Tabla

        If Tabla <> "0" Then
            Call cTablaMaestra.ListarSeccionesCotizaciones(strConexionSql, eValidacion, l_Secciones)
        End If

        Session_SeccionCriterio = l_SeccionCriterio
        Session_Secciones = l_Secciones

    End Sub

#End Region

#Region "----------  Funciones  ----------"



#End Region

#Region "----------  WebMethod  ----------"

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarCriterios() As List(Of beTablaMaestra)

        Call mtd_ConsultarTablaImpresion("2")
        l_Secciones = New List(Of beTablaMaestra)
        l_Secciones = Session_Secciones
        Return l_Secciones

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarTablaImpresion(ByVal Tabla As String) As JQGridJsonResponse

        l_Secciones = New List(Of beTablaMaestra)

        Call mtd_ConsultarTablaImpresion(Tabla)
        l_Secciones = Session_Secciones
        lResponse = New JQGridJsonResponse(1, 1, 1000, l_Secciones)

        Return lResponse

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function MantenimientoSeccion(ByVal TipoOperacion As String, _
                                            ByVal IdSeccion As String, _
                                            ByVal Tipo As String, _
                                            ByVal PosicionInicial As String, _
                                            ByVal Nombre As String, _
                                            ByVal Opcional As String, _
                                            ByVal CambioPosicion As String) As beValidacion
        '----------------------------------------------------------------------'
        eValidacion = New beValidacion
        eTablaMaestra = New beTablaMaestra
        cTablaMaestra = New bcTablaMaestra
        l_SeccionCriterio = New List(Of beTablaMaestra)
        '----------------------------------------------------------------------'
        Try
            eValidacion.flag = TipoOperacion
            eValidacion.usuario = Session_Usuario
            eTablaMaestra.IdSeccion = IdSeccion
            eTablaMaestra.Tipo = Tipo
            eTablaMaestra.PosicionInicial = PosicionInicial
            eTablaMaestra.Nombre = Nombre

            If Tipo = "1" AndAlso TipoOperacion <> "3" Then
                eTablaMaestra.Opcional = CBool(Opcional)
                eTablaMaestra.CambioPosicion = CBool(CambioPosicion)
            Else
                eTablaMaestra.Opcional = Nothing
                eTablaMaestra.CambioPosicion = Nothing
            End If

            If TipoOperacion = "3" Then
                eTablaMaestra.PosicionInicial = 0
            ElseIf TipoOperacion <> "3" AndAlso Tipo = "1" Then
                l_SeccionCriterio = Session_SeccionCriterio
            End If

            If Tipo = "1" Then
                Call cTablaMaestra.MantenimientoSeccion(strConexionSql, eTablaMaestra, l_SeccionCriterio, eValidacion)
            ElseIf Tipo = "2" Then
                Call cTablaMaestra.MantenimientoCriterio(strConexionSql, eTablaMaestra, eValidacion)
            End If

        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try
        '----------------------------------------------------------------------'
        Return eValidacion
        '----------------------------------------------------------------------'
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarSeccionCriterio(ByVal IdSeccion As String) As beValidacion

        eValidacion = New beValidacion
        cTablaMaestra = New bcTablaMaestra
        l_SeccionCriterio = New List(Of beTablaMaestra)

        eValidacion.flag = IdSeccion
        cTablaMaestra.ListarCriteriosPorSeccion(strConexionSql, eValidacion, l_SeccionCriterio)

        Session_SeccionCriterio = l_SeccionCriterio

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function AgregarQuitarSeccionCriterio(ByVal TipoOperacion As String, ByVal IdCriterio As String, ByVal Nombre As String) As beValidacion

        eValidacion = New beValidacion
        eTablaMaestra = New beTablaMaestra
        l_SeccionCriterio = New List(Of beTablaMaestra)
        l_SeccionCriterio = Session_SeccionCriterio

        Try
            If TipoOperacion = "1" Then 'Agregar SeccionCriterio
                'Buscar si existe el criterio -----------------------------------------------------------
                If l_SeccionCriterio.Exists(Function(Rpt) Rpt.IdCriterio = IdCriterio) Then
                    Throw New Exception("ya existe el criterio")
                End If
                '----------------------------------------------------------------------------------------
                eTablaMaestra.IdSeccionCriterio = l_SeccionCriterio.Max(Function(Rpt) Rpt.IdSeccionCriterio) + 1
                eTablaMaestra.IdCriterio = IdCriterio
                eTablaMaestra.Nombre = Nombre
                l_SeccionCriterio.Add(eTablaMaestra)
            End If
            If TipoOperacion = "2" Then 'Quitar SeccionCriterio
                l_SeccionCriterio.RemoveAll(Function(Rpt) Rpt.IdCriterio = IdCriterio)
            End If
            Session_SeccionCriterio = l_SeccionCriterio
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ActualizarSeccionCriterio() As List(Of beTablaMaestra)

        l_SeccionCriterio = New List(Of beTablaMaestra)

        If Not IsNothing(Session_SeccionCriterio) Then
            l_SeccionCriterio = Session_SeccionCriterio
        End If

        Return l_SeccionCriterio

    End Function

#End Region

End Class