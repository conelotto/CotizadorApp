Imports System.Data
Imports System.Data.OleDb
Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras
Imports System.IO
Imports System.Net.Mail
Imports System
Imports System.Collections.Generic
Imports System.Linq.Dynamic
Imports System.Web.HttpServerUtility
Imports System.Xml
Imports System.Runtime.Serialization.Formatters.Binary
Imports DocumentFormat.OpenXml.Wordprocessing
Imports DocumentFormat.OpenXml.Packaging
Imports System.Reflection

Module Modulo

    Public strConexion As String = ConfigurationManager.AppSettings.Get("Conexion")
    Public strConexionAdryan As String = ConfigurationManager.AppSettings.Get("ConexionAdryan")
    Public strLibreria As String = ConfigurationManager.AppSettings.Get("Libreria")
    Public strConexionSql As String = ConfigurationManager.AppSettings.Get("ConexionSQL")
    'Email
    Public strEmailOrigen As String = ConfigurationManager.AppSettings.Get("EmailOrigen")
    Public strEmailEnvio As String = ConfigurationManager.AppSettings.Get("EmailEnvio")
    Public strEmailAsuntoSolAprobacion As String = ConfigurationManager.AppSettings.Get("EmailAsuntoSolAprobacion")
    Public strEmailMensajeSolAprobacion As String = ConfigurationManager.AppSettings.Get("EmailMensajeSolAprobacion")



    Public strEmailMensajeCotAprobado As String = ConfigurationManager.AppSettings.Get("EmailMensajeCotAprobado")
    Public strEmailAsuntoCotRechazado As String = ConfigurationManager.AppSettings.Get("EmailAsuntoCotRechazado")
    Public strEmailMensajeCotRechazado As String = ConfigurationManager.AppSettings.Get("EmailMensajeCotRechazado")
    Public strEmailAsuntoCotCambio As String = ConfigurationManager.AppSettings.Get("EmailAsuntoCotCambio")
    Public strEmailMensajeCotCambio As String = ConfigurationManager.AppSettings.Get("EmailMensajeCotCambio")

    Public strUbicServidor As String = ConfigurationManager.AppSettings.Get("UbicServidor")
    Public RolEdicionUsuario As String = ConfigurationManager.AppSettings.Get("RolEdicionUsuario")


    'Fin
    Public dstDatos As New DataSet
    Public dstCache As New DataSet
    Public IdUsuario As String

    'URL FTP
    Public Property strUrlFTPArchivo As String = System.Configuration.ConfigurationManager.AppSettings("UrlFtpArchivo")
    Public Property strUrlLocalArchivo As String = System.Configuration.ConfigurationManager.AppSettings("UrlLocalArchivo")
    Public Property strUrlFtpArchivoPlantillas As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoPlantillas")
    Public Property strUrlFtpArchivoEspecificacion As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoEspecificacion")
    Public Property strUrlFtpCotizacionVersion As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpCotizacionVersion")
    Public Property strUrlFtpArchivoTemporal As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoTemporal")
    Public Property strUrlFtpArchivoAnexos As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoAnexos")
    Public Property strUrlFtpArchivoDetallePartes As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoDetallePartes")
    Public Property strUrlFtpArchivoTarifaRS As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoTarifasRS")
    Public Property strUrlFtpArchivoDetallePartesRS As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpArchivoDetallePartesRS")


    'Valores Servicio SAP
    Public Property strUsuarioServSAP As String = System.Configuration.ConfigurationManager.AppSettings("userServicioSap")
    Public Property strPasswordServSAP As String = ConfigurationManager.AppSettings.Get("passServicioSap")

    'Codigos de Secciones
    Public Property strCodSeccionEspecifTecnica As String = ConfigurationManager.AppSettings.Get("CodSeccionEspecifTecnica").ToString()
    Public Property strCodSeccionCartaPresent As String = ConfigurationManager.AppSettings.Get("CodSeccionCartaPresent")

    Public Property strCadenaAleatoria() As String
        Get
            Return HttpContext.Current.Session("cadenaAleatoria")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("cadenaAleatoria") = value
        End Set
    End Property


#Region "Cache"
    Public Function ObtenerCache(ByRef objCache As System.Web.Caching.Cache, ByVal NombreCache As String, _
                                     ByVal EntidadesMaestrasDbs As List(Of String), ByVal EntidadesMaestras As List(Of String)) As Boolean
        Dim dstDbs As New DataSet
        Dim dstTablaMaestra As New DataSet
        Dim dstCache As New DataSet
        Dim Valido As Boolean
        Dim Campo As String

        Dim leTablaMaestra As New List(Of beTablaMaestra)
        Dim eTablaMaestra As beTablaMaestra
        Dim Lista As New List(Of String)

        If objCache(NombreCache) Is Nothing Then
            Lista.AddRange(EntidadesMaestrasDbs)

            For Each Campo In EntidadesMaestras
                eTablaMaestra = New beTablaMaestra
                eTablaMaestra.Grupo = Campo
                eTablaMaestra.Estado = Estado.Habilitado
                leTablaMaestra.Add(eTablaMaestra)
            Next
        Else
            dstCache = objCache(NombreCache)

            For Each Campo In EntidadesMaestrasDbs
                If Not dstCache.Tables.Contains(Campo) Then
                    Lista.Add(Campo)
                End If
            Next

            For Each Campo In EntidadesMaestras
                If Not dstCache.Tables.Contains(Campo) Then
                    eTablaMaestra = New beTablaMaestra
                    eTablaMaestra.Grupo = Campo
                    eTablaMaestra.Estado = Estado.Habilitado
                    leTablaMaestra.Add(eTablaMaestra)
                End If
            Next
        End If

        If Not Lista.Count.Equals(0) Then
            Dim oDbs As New bcDbs
            If oDbs.BuscarEntidadMaestraDbs(strConexion, strLibreria, EntidadesMaestrasDbs, dstDbs) Then
                For Indice As Int32 = 0 To dstDbs.Tables.Count - 1
                    If Not dstCache.Tables.Contains(dstDbs.Tables(Indice).TableName) Then
                        dstCache.Tables.Add(dstDbs.Tables(Indice).Copy)
                    End If
                Next
            End If
        End If

        If Not leTablaMaestra.Count.Equals(0) Then
            Dim oTablaMaestra As New bcTablaMaestra
            If oTablaMaestra.BuscarGrupo(strConexionSql, leTablaMaestra, dstTablaMaestra) Then
                For Indice As Int32 = 0 To dstTablaMaestra.Tables.Count - 1
                    If Not dstCache.Tables.Contains(dstTablaMaestra.Tables(Indice).TableName) Then
                        dstCache.Tables.Add(dstTablaMaestra.Tables(Indice).Copy)
                    End If
                Next
            End If
        End If

        objCache(NombreCache) = dstCache

        Return Valido
    End Function

    Public Function ObtenerIgv(ByRef dstCache As DataSet) As Decimal
        Dim decIgv As Decimal
        Dim dtbIgv As DataTable = Nothing
        If Not dstCache.Tables.Contains(EntidadTablaMaestra.IGV.ToString) Then
            dtbIgv = ObtenerTablaGrupo(EntidadTablaMaestra.IGV.ToString)
            If dtbIgv IsNot Nothing Then
                dstCache.Tables.Add(dtbIgv)
            End If
        Else
            dtbIgv = dstCache.Tables(EntidadTablaMaestra.IGV.ToString)
        End If
        If dtbIgv IsNot Nothing AndAlso Not dtbIgv.Rows.Count.Equals(0) Then
            Dim dvwIgv As DataView = dtbIgv.DefaultView
            dvwIgv.RowFilter = String.Format("Codigo='{0}'", IGV.c_strIGV)
            If Not dvwIgv.Count.Equals(0) Then
                Decimal.TryParse(dvwIgv(0)("Nombre").ToString, decIgv)
            End If
        End If
        Return decIgv
    End Function

    Public Function ObtenerMensaje(ByRef dstCache As DataSet, ByVal strCodigo As String) As String
        Dim strMensaje As String = String.Empty
        Dim dtbMensaje As DataTable = Nothing
        If Not strCodigo.Equals(String.Empty) Then
            If dstCache Is Nothing Then
                dstCache = New DataSet
            End If
            If Not dstCache.Tables.Contains(EntidadTablaMaestra.Mensaje.ToString) Then
                dtbMensaje = ObtenerTablaGrupo(EntidadTablaMaestra.Mensaje.ToString)
                If dtbMensaje IsNot Nothing Then
                    dstCache.Tables.Add(dtbMensaje)
                End If
            Else
                dtbMensaje = dstCache.Tables(EntidadTablaMaestra.Mensaje.ToString)
            End If
        End If
        If dtbMensaje IsNot Nothing AndAlso Not dtbMensaje.Rows.Count.Equals(0) Then
            Dim dvwMensaje As DataView = dtbMensaje.DefaultView
            dvwMensaje.RowFilter = String.Format("Codigo='{0}'", strCodigo)
            If Not dvwMensaje.Count.Equals(0) Then
                strMensaje = dvwMensaje(0)("Descripcion").ToString
            End If
        End If
        Return strMensaje
    End Function

    Public Function ObtenerTablaGrupo(ByVal Grupo As String) As DataTable
        Dim oTablaMaestra As New bcTablaMaestra
        Dim eTablaMaestra As New beTablaMaestra
        Dim dtbGrupo As DataTable = Nothing
        eTablaMaestra.Grupo = Grupo
        eTablaMaestra.Estado = Estado.Habilitado
        Dim dstIgv As New DataSet
        If oTablaMaestra.BuscarGrupo(strConexionSql, eTablaMaestra, dstIgv) Then
            dtbGrupo = dstIgv.Tables(Grupo).Copy
        End If
        Return dtbGrupo
    End Function
#End Region

#Region "Presentacion"

    Private Sub ActualizarCamposArchivoCotizacion(ByVal packagePath As String, ByVal dstPlantilla As DataSet)
        Dim xmlDocumento As New System.Xml.XmlDocument
        Dim xpnDocumento As System.Xml.XPath.XPathNavigator
        Dim xpnCampo As System.Xml.XPath.XPathNavigator
        Dim xnmDocumento As System.Xml.XmlNamespaceManager
        Dim msPackage As IO.Packaging.PackagePart
        Dim uriPartTarget As Uri
        Dim strCampo As String
        Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"
        Try

            Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(packagePath, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                'Recupera el xml del contenido del documento
                uriPartTarget = New Uri("/word/document.xml", UriKind.Relative)
                msPackage = package.GetPart(uriPartTarget)
                xmlDocumento.Load(msPackage.GetStream)

                'Se crea el navegador
                xpnDocumento = xmlDocumento.CreateNavigator()
                xnmDocumento = New System.Xml.XmlNamespaceManager(xpnDocumento.NameTable)
                xnmDocumento.AddNamespace("w", strUri)

                'Recupera los marcadores del documento
                'For Each nav In navigator.Select("//w:bookmarkStart", manager)
                For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                    strCampo = String.Empty
                    If xpnCampo.MoveToChild("name", strUri) Then
                        If xpnCampo.MoveToAttribute("val", strUri) Then
                            strCampo = xpnCampo.Value
                        End If

                        Dim Item = From Fila In dstPlantilla.Tables(Entidad.PlantillaCampo.ToString) _
                                  Where Fila("CampoPlantilla") = strCampo Select Fila
                        If Not Item.Count.Equals(0) Then
                            'Move to w:instrText
                            If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                'Check FORMTEXT
                                If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                    'Move to t
                                    If xpnCampo.MoveToFollowing("t", strUri) Then
                                        'Set value
                                        xpnCampo.SetValue(Item(0)("CampoTabla").ToString)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                'Actualiza y cierra el documento
                xmlDocumento.Save(msPackage.GetStream(IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))
                msPackage.GetStream.Flush()
                msPackage.GetStream.Close()
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(ex.Message.ToString)
        End Try
    End Sub
#End Region

#Region "Scripts"
    Public Sub AddScriptPagina(ByVal Pagina As Page, ByVal Funcion As String, ByVal Instruccion As String, Optional ByVal Modal As Boolean = False)
        'Dim csm As ClientScriptManager = Pagina.ClientScript
        'csm.RegisterStartupScript(Pagina.GetType(), Funcion, String.Concat("<script language=""javascript"" type=""text/javascript"">", Instruccion, "</script>"))
        'If Not Modal Then
        ScriptManager.RegisterStartupScript(Pagina, Pagina.GetType, Funcion, Instruccion, True)
        'Else
        'Pagina.ClientScript.RegisterStartupScript(Pagina.GetType(), Funcion, Instruccion)
        'End If
    End Sub

    Public Sub ClosePaginaServer(ByVal Pagina As Page)
        AddScriptPagina(Pagina, "Cerrar", "window.close();")
    End Sub

    Public Sub AlertPaginaServer(ByVal Pagina As Page, ByVal Mensaje As String)
        If Mensaje IsNot Nothing Then
            Mensaje = Mensaje.Replace("'", "")
            Mensaje = Mensaje.Replace(vbCrLf, "\n")
            Mensaje = String.Format("alert('{0}');", Mensaje)
        Else
            Mensaje = "Mensaje no establecido"
        End If

        ScriptManager.RegisterStartupScript(Pagina, Pagina.GetType(), "Alerta", Mensaje, True)
    End Sub
#End Region

#Region "Comportamiento"
    Public Sub HabilitarControl(ByVal Control As TextBox, ByVal Estado As Boolean, ByVal Limpiar As Boolean, ByVal AlinearDerecha As Boolean)
        If Not Estado Then
            Control.CssClass = IIf(AlinearDerecha, "CampoBordeLecturaDerecha", "CampoBordeLectura")
            Control.ReadOnly = True
        Else
            Control.CssClass = IIf(AlinearDerecha, "CampoBordeDerecha", "CampoBorde")
            Control.ReadOnly = False
        End If
        If Limpiar Then Control.Text = String.Empty
    End Sub

    Public Sub HabilitarControl(ByVal Control As DropDownList, ByVal Estado As Boolean, ByVal Limpiar As Boolean)
        If Estado Then
            Control.CssClass = "CampoBorde"
            Control.Enabled = True
        Else
            Control.CssClass = "CampoBordeLectura"
            Control.Enabled = False
        End If
        If Limpiar Then Control.SelectedIndex = -1
    End Sub

    Public Sub HabilitarControl(ByVal Control As Object, ByVal Estado As Boolean)
        If Estado Then
            Control.CssClass = "CampoBorde"
            Control.Enabled = True
        Else
            Control.CssClass = "CampoBordeLectura"
            Control.Enabled = False
        End If
    End Sub

    Public Function DataSetValido(ByRef dstComparar As DataSet, ByVal VerificaRegistros As Boolean) As Boolean
        Dim Resultado As Boolean
        If dstComparar IsNot Nothing Then
            Resultado = Not dstComparar.Tables.Count.Equals(0)
            If VerificaRegistros Then
                Resultado = False
                For Each dtbTabla As DataTable In dstComparar.Tables
                    If Not dtbTabla.Rows.Count.Equals(0) Then
                        Resultado = True
                        Exit For
                    End If
                Next
            End If
        End If
        Return Resultado
    End Function

    Public Sub RemoverColumna(ByRef Tabla As DataTable, ByVal Columna As String)
        If Tabla.Columns.Contains(Columna) Then
            Tabla.Columns.Remove(Columna)
        End If
    End Sub

    Public Sub RenombrarColumna(ByRef Tabla As DataTable, ByVal NombreOriginal As String, ByVal NombreActual As String)
        If Tabla.Columns.Contains(NombreOriginal) Then
            Tabla.Columns(NombreOriginal).ColumnName = NombreActual
        End If
    End Sub
    Public Sub AgregarColumna(ByRef Tabla As DataTable, ByVal Columna As String)
        If Not Tabla.Columns.Contains(Columna) Then
            Tabla.Columns.Add(Columna)
        End If
    End Sub
#End Region

#Region "Calculo"
    Public Function CalcularMargen(ByVal ValorVenta As String, ByVal CostoTotal As String) As Decimal
        Dim decMargen As Decimal
        Dim decValorVenta As Decimal
        Dim decCostoTotal As Decimal
        Decimal.TryParse(ValorVenta.Trim, decValorVenta)
        Decimal.TryParse(CostoTotal.Trim, decCostoTotal)
        If Not decValorVenta.Equals(0) Then
            decMargen = (decValorVenta - decCostoTotal) / decValorVenta * 100
        End If
        Return decMargen
    End Function
#End Region

#Region "Grid"
    Public Sub Exportar(ByRef objResponse As System.Web.HttpResponse, ByVal dstResponse As DataSet, ByVal NombreArchivo As String)
        objResponse.AppendHeader("content-disposition", String.Format("attachment; filename={0}.xls", NombreArchivo))
        objResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        objResponse.ContentEncoding = Encoding.GetEncoding("iso-8859-1")
        Using sw As New IO.StringWriter()
            Using hw As New HtmlTextWriter(sw)
                Dim dgdCotizacion As New DataGrid
                dgdCotizacion.DataSource = dstResponse.Tables(0).DefaultView
                dgdCotizacion.DataBind()
                dgdCotizacion.RenderControl(hw)
            End Using
            objResponse.Write(sw.ToString)
        End Using
        objResponse.Flush()
        objResponse.End()
    End Sub

    Public Sub EnlazarGrid(ByRef Grid As GridView, ByVal Data As Object, ByVal SinIndice As Boolean, ByVal SinEditar As Boolean)
        With Grid
            If Not SinIndice Then .SelectedIndex = -1
            If Not SinEditar Then .EditIndex = -1
            .DataSource = Data
            .DataBind()
        End With
    End Sub

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As Image) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), Image)
            Resultado = True
        End If
        Return Resultado
    End Function

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As ImageButton) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), ImageButton)
            Resultado = True
        End If
        Return Resultado
    End Function

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As LinkButton) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), LinkButton)
            Resultado = True
        End If
        Return Resultado
    End Function

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As TextBox) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), TextBox)
            Resultado = True
        End If
        Return Resultado
    End Function

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As ListControl) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), ListControl)
            Resultado = True
        End If
        Return Resultado
    End Function

    Public Function EncontrarControlGrid(ByVal Fila As GridViewRow, ByVal NombreControl As String, ByRef Control As System.Web.UI.WebControls.CheckBox) As Boolean
        Dim Resultado As Boolean
        If Fila.FindControl(NombreControl) IsNot Nothing Then
            Control = CType(Fila.FindControl(NombreControl), System.Web.UI.WebControls.CheckBox)
            Resultado = True
        End If
        Return Resultado
    End Function
#End Region

#Region "ListControl"
    ''' <summary>
    ''' Enlaza el combo a un origen de datos
    ''' </summary>
    ''' <param name="drop">DropDowlist a enlazar</param>
    ''' <param name="Data">Origen de datos</param>
    ''' <param name="Codigo">Valor del ValueField</param>
    ''' <param name="Texto">Valor del textField</param>
    ''' <param name="InsertarVacio">Indicador si se incluye un item vacio</param>
    ''' <remarks>EnlazarCombo</remarks>
    Public Sub EnlazarCombo(ByRef drop As DropDownList, ByVal Data As Object, ByVal Codigo As String, _
                            ByVal Texto As String, ByVal InsertarVacio As Boolean)
        With drop
            .Items.Clear()
            .SelectedIndex = -1
            If Data IsNot Nothing Then
                .DataSource = Data
                .DataValueField = Codigo
                .DataTextField = Texto
                .DataBind()
            End If

            If InsertarVacio Then
                .Items.Insert(0, String.Empty)
            End If
        End With
    End Sub

    Public Sub EnlazarCombo(ByRef drop As DropDownList, ByVal Items As List(Of String), ByVal InsertarVacio As Boolean)
        With drop
            .Items.Clear()
            .SelectedIndex = -1
            For Indice As Int32 = 0 To Items.Count - 1
                .Items.Add(Items(Indice).ToString)
            Next
            If InsertarVacio Then
                .Items.Insert(0, String.Empty)
            End If
        End With
    End Sub

    ''' <summary>
    ''' Establece el origen de datos a una entidad de la TablaMaestra
    ''' </summary>
    ''' <param name="drop">DropDowlist a enlazar</param>
    ''' <param name="Entidad">Nombre de la entidad de la TablaMaestra</param>
    ''' <param name="InsertarVacio">Indicador si se incluye un item vacio</param>
    ''' <param name="oTablaMaestra">Bussines Component que contiene la operacion Buscar</param>
    ''' <remarks>EnlazarCombo</remarks>
    Public Sub EnlazarCombo(ByRef drop As DropDownList, ByVal Entidad As EntidadTablaMaestra, _
                            ByVal InsertarVacio As Boolean, Optional ByRef oTablaMaestra As bcTablaMaestra = Nothing)
        Dim dstTablaMaestra As New DataSet
        Dim eTablaMaestra As New beTablaMaestra

        If oTablaMaestra Is Nothing Then
            oTablaMaestra = New bcTablaMaestra
        End If
        Dim leTablaMaestra As New List(Of beTablaMaestra)
        eTablaMaestra.Grupo = Entidad.ToString
        leTablaMaestra.Add(eTablaMaestra)

        If oTablaMaestra.BuscarGrupo(strConexionSql, leTablaMaestra, dstTablaMaestra) Then
            EnlazarCombo(drop, dstTablaMaestra, "Codigo", "Nombre", InsertarVacio)
        End If
    End Sub

    Public Function GetItemValueInt32(ByRef objlista As ListControl) As Int32
        Dim Resultado As Int32 = 0
        If objlista.SelectedIndex > -1 Then
            If Not objlista.SelectedItem.Value.Equals(String.Empty) Then
                Resultado = objlista.SelectedItem.Value
            End If
        End If
        Return Resultado
    End Function

    Public Function GetItemValueString(ByRef objlista As ListControl, Optional ByVal ObtenerTexto As Boolean = False) As String
        Dim Resultado As String = String.Empty
        If objlista.SelectedIndex > -1 Then
            If Not objlista.Text.Trim.Equals(Resultado) Then
                If Not objlista.SelectedItem.Value.Equals(String.Empty) Then
                    If ObtenerTexto Then
                        Resultado = objlista.SelectedItem.Text
                    Else
                        Resultado = objlista.SelectedItem.Value
                    End If
                End If
            End If
        End If
        Return Resultado
    End Function

    Public Sub SetItemValueString(ByRef objlista As ListControl, ByVal Valor As String, ByVal EsCodigo As Boolean)
        Dim Indice As Int32 = -1
        If Not Valor.Equals(String.Empty) Then
            Dim Item As System.Web.UI.WebControls.ListItem
            If EsCodigo Then
                Item = objlista.Items.FindByValue(Valor)
            Else
                Item = objlista.Items.FindByText(Valor)
            End If
            Indice = objlista.Items.IndexOf(Item)
        End If
        objlista.SelectedIndex = Indice
    End Sub
#End Region

#Region "DBS"
    'Public Function ObtenerModeloPorCodigo(ByVal IdModelo As String) As String
    '    'Dim oDbs As New bcDbs
    '    'Dim Modelo As String = String.Empty
    '    'Modelo = oDbs.ObtenerModelo(strConexion, strLibreria, IdModelo, False)
    '    'Return Modelo
    '    Dim oModelo As New bcModelo
    '    Dim eModelo As New beModelo
    '    Dim Descripcion As String = String.Empty
    '    eModelo.CodigoModelo = IdModelo
    '    If oModelo.BuscarDescripcionPorCodigo(strConexionSql, eModelo) Then
    '        If eModelo.Descripcion IsNot Nothing Then
    '            Descripcion = eModelo.Descripcion
    '        End If
    '    End If
    '    Return Descripcion
    'End Function

    Public Function ObtenerNombrePorCodigoDbs(ByVal dstDbs As DataSet, ByVal EntidadDbs As String, _
                                              ByVal Codigo As String, ByRef Nombre As String) As Boolean
        Dim Valido As Boolean
        Nombre = String.Empty
        If dstDbs.Tables.Contains(EntidadDbs) Then
            Dim Resultado = From Fila In dstCache.Tables(EntidadDbs) Where Fila("CODIGO") = Codigo Select Fila
            If Not Resultado.Count.Equals(0) Then
                Nombre = Resultado(0)("DESCRIPE")
                Valido = True
            End If
        End If
        Return Valido
    End Function
#End Region

#Region "Email"
    Public Function GenerarMailMessage(ByVal strFrom As String, ByVal strTo As String, ByVal strCC As String, ByVal asunto As String, ByVal cuerpo As String) As MailMessage
        Dim message As MailMessage = New MailMessage
        Dim strSubject As StringBuilder = New StringBuilder
        Dim separator As Char() = {";"} 'Separa los correos en caso se desee enviar a mas de un destinario
        Dim ToArray As String() = strTo.Split(separator)
        Dim ccArray As String() = strCC.Split(separator)
        'origen
        message.From = New MailAddress(strFrom)
        'destinario
        For i As Integer = 0 To (ToArray.Length - 1)
            message.To.Add(New MailAddress(ToArray(i).Trim()))
        Next
        'con copia
        If Not String.IsNullOrEmpty(strCC) Then
            For i As Integer = 0 To (ccArray.Length - 1)
                message.CC.Add(New MailAddress(ccArray(i).Trim()))
            Next
        End If
        'Otros
        message.Subject = asunto
        message.Body = cuerpo
        message.IsBodyHtml = True
        message.Priority = System.Net.Mail.MailPriority.Normal
        message.BodyEncoding = Encoding.UTF8
        Return message
    End Function
#End Region

#Region "Paginacion"


    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beCotizacion))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub

    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beProducto))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub

    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beCotizacionContacto))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub

    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beMaquinaria))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub

    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beArchivoConfiguracion))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub

    Public Sub ArmarPaginacion(ByRef vPaginacion As beValidacion, ByRef Listado As List(Of beHomologacion))

        If Not (Listado IsNot Nothing AndAlso Listado.Count > 0) Then
            vPaginacion.currentPage = 1
            vPaginacion.pageCount = 1
            vPaginacion.recordCount = 0
        Else
            If Not (String.IsNullOrEmpty(vPaginacion.sortColumn) OrElse String.IsNullOrEmpty(vPaginacion.sortOrder)) Then
                Dim Q_Listado = Listado.AsQueryable
                Listado = DynamicQueryable.OrderBy(Q_Listado, vPaginacion.sortColumn + " " + vPaginacion.sortOrder).ToList
            End If
            If IsNumeric(vPaginacion.pageSize) AndAlso IsNumeric(vPaginacion.currentPage) Then
                Dim indexPage As Integer = 0
                Dim pageSize As Integer = CInt(vPaginacion.pageSize)
                vPaginacion.recordCount = Listado.Count
                vPaginacion.pageCount = Math.Ceiling(CInt(vPaginacion.recordCount) / CInt(vPaginacion.pageSize))
                If CInt(vPaginacion.currentPage) > CInt(vPaginacion.pageCount) AndAlso CInt(vPaginacion.pageCount) <> 0 Then vPaginacion.currentPage = vPaginacion.pageCount
                indexPage = CInt(vPaginacion.currentPage) - 1
                Dim I_Cotizacion = Listado.Select(Function(Fila, index) New With {Key .RowNumber = index + 1, Fila}).ToList
                Dim P_Cotizacion = From Tabla In I_Cotizacion
                                   Where Tabla.RowNumber >= pageSize * indexPage + 1 AndAlso
                                         Tabla.RowNumber <= pageSize * (indexPage + 1)
                Dim W_Cotizacion = From Tabla In P_Cotizacion
                                   Select Tabla.Fila
                Listado = W_Cotizacion.ToList
            End If
        End If

    End Sub
#End Region

#Region "General"

    Public Function GenerarNombreArchivo(ByVal Nombre As String) As String
        Dim strNombre As String = String.Empty
        Dim Extensions As String = String.Empty
        Dim index As Integer = -1

        strNombre &= DateTime.Now.Year.ToString()
        strNombre &= DateTime.Now.Month.ToString()
        strNombre &= DateTime.Now.Day.ToString()
        strNombre &= DateTime.Now.Hour.ToString()
        strNombre &= DateTime.Now.Second.ToString()

        index = Nombre.LastIndexOf(".")
        If index > 0 Then
            Extensions = Nombre.Substring(index, Nombre.Length - index)
        End If

        strNombre &= Extensions

        Return strNombre

    End Function
    Public Function ExtensionArchivo(ByVal Nombre As String) As String

        Dim Extensions As String = String.Empty
        Dim index As Integer = -1

        index = Nombre.LastIndexOf(".")
        If index > 0 Then
            Extensions = Nombre.Substring(index, Nombre.Length - index)
        End If

        Return Extensions

    End Function

    Public Function ObtenerCampos(ByVal urlFolderArchivo As String, ByVal NombreArchivo As String) As List(Of String)
        Dim Lista As New List(Of String)
        Dim Parte As Packaging.PackagePart
        Dim xmlDocumento As New XmlDocument
        Dim Navegador As XPath.XPathNavigator
        Dim xmlManager As XmlNamespaceManager
        Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"
        Dim ostream As System.IO.MemoryStream

        Dim oAdminFTP As New AdminFTP

        ostream = oAdminFTP.ObtenerArchivo(urlFolderArchivo, NombreArchivo)

        Using package As Packaging.Package = Packaging.Package.Open(ostream)
            'Recupera el xml del contenido del documento
            Dim uriPartTarget As New Uri("/word/document.xml", UriKind.Relative)
            Parte = package.GetPart(uriPartTarget)

            xmlDocumento.Load(Parte.GetStream)

            'Se crea el navegador
            Navegador = xmlDocumento.CreateNavigator()
            xmlManager = New XmlNamespaceManager(Navegador.NameTable)
            xmlManager.AddNamespace("w", strUri)

            ' Recupera los marcadores del documento
            Dim xpnCampo As XPath.XPathNavigator
            For Each xpnCampo In Navegador.Select("//w:ffData", xmlManager)
                If xpnCampo.MoveToChild("name", strUri) Then
                    If xpnCampo.MoveToAttribute("val", strUri) Then
                        Lista.Add(xpnCampo.Value)
                    End If
                End If
            Next

            'Cierra el documento
            Parte.GetStream.Close()
        End Using
        Return Lista
    End Function

    Public Function ObtenerMarcadores(ByVal urlFolderArchivo As String, ByVal NombreArchivo As String) As List(Of String) 'Dictionary(Of String, BookmarkEnd)

        Dim lista As New List(Of String)
        Dim ostream As System.IO.MemoryStream

        Dim oAdminFTP As New AdminFTP

        ostream = oAdminFTP.ObtenerArchivo(urlFolderArchivo, NombreArchivo)

        Dim wordDoc As WordprocessingDocument = WordprocessingDocument.Open(ostream, True)
        Dim l_MarcadoresWord As Dictionary(Of String, BookmarkEnd) = Nothing

        l_MarcadoresWord = ListarMarcadores(wordDoc.MainDocumentPart.Document)

        For Each Rpt In l_MarcadoresWord
            lista.Add(Rpt.Key)
        Next

        Return lista

    End Function

    Private Function ListarMarcadores(ByVal objOpenXmlElement As DocumentFormat.OpenXml.OpenXmlElement, Optional ByVal Results As Dictionary(Of String, BookmarkEnd) = Nothing, Optional ByVal UnMatched As Dictionary(Of String, String) = Nothing) As Dictionary(Of String, BookmarkEnd)

        Results = If(Results, New Dictionary(Of String, BookmarkEnd)())
        UnMatched = If(UnMatched, New Dictionary(Of String, String)())

        For Each child In objOpenXmlElement.Elements()
            If TypeOf child Is BookmarkStart Then
                Dim bStart = TryCast(child, BookmarkStart)
                UnMatched.Add(bStart.Id, bStart.Name)
            End If
            If TypeOf child Is BookmarkEnd Then
                Dim bEnd = TryCast(child, BookmarkEnd)
                For Each orphanName In UnMatched
                    If bEnd.Id = orphanName.Key Then
                        Results.Add(orphanName.Value, bEnd)
                    End If
                Next
            End If
            ListarMarcadores(child, Results, UnMatched)
        Next

        Return Results

    End Function

    Public Function GenerarNombreVersion(ByVal obeCotizacionVersion As beCotizacionVersion) As String
        Dim strResultado As String = String.Empty
        If Not IsNothing(obeCotizacionVersion) Then
            strResultado = obeCotizacionVersion.IdCotizacionSap & "-" & "v." & obeCotizacionVersion.NumVersion & ".docx"
        End If
        Return strResultado
    End Function
    Public Function CopiarClase(Of T)(ByVal clase As T) As T
        Dim bFormatter = New BinaryFormatter()
        Dim stream = New MemoryStream()
        bFormatter.Serialize(stream, clase)
        stream.Seek(0, SeekOrigin.Begin)
        Return CType(bFormatter.Deserialize(stream), T)

    End Function

    Public Function CompararObjetos(ByVal _Objeto1 As Object, ByVal _Objeto2 As Object) As Boolean
        Dim _TipoObjeto1 As String = ""
        Dim _TipoObjeto2 As String = ""

        If Not _Objeto1 Is Nothing Then
            _TipoObjeto1 = _Objeto1.GetType.ToString
        End If

        If Not _Objeto2 Is Nothing Then
            _TipoObjeto2 = _Objeto2.GetType.ToString
        End If

        Dim _Resultado As Boolean = True

        If _TipoObjeto1 = _TipoObjeto2 Then
            Dim Propiedades() As PropertyInfo = _Objeto1.GetType.GetProperties
            Dim Propiedad As PropertyInfo
            Dim _Valor1 As Object
            Dim _Valor2 As Object
            For Each Propiedad In Propiedades
                _Valor1 = Propiedad.GetValue(_Objeto1, Nothing)
                _Valor2 = Propiedad.GetValue(_Objeto2, Nothing)

                If Not IsNothing(_Valor1) Then
                    Dim TipoDato1 As String = _Valor1.GetType.ToString()
                    Dim TipoDato2 As String = _Valor2.GetType.ToString()
                    If TipoDato1 = TipoDato2 Then

                        If TipoDato1.Contains("System.Collections.Generic.List") Then
                            If _Valor1.count > 0 Then
                                Dim subObj1 As Object
                                Dim subObj2 As Object
                                For i As Integer = 0 To _Valor1.count - 1
                                    subObj1 = _Valor1(i)
                                    subObj2 = _Valor2(i)
                                    _Resultado = CompararObjetos(subObj1, subObj2)
                                    If _Resultado = False Then Exit For
                                Next
                                If _Resultado = False Then Exit For
                            End If

                        Else
                            If TipoDato1.Contains("Ferreyros.") Then
                                _Resultado = CompararObjetos(_Valor1, _Valor2)
                                If _Resultado = False Then Exit For
                            Else
                                If _Valor1 <> _Valor2 Then
                                    _Resultado = False
                                    Exit For
                                End If
                            End If

                        End If
                    End If
                End If

            Next
        Else
            _Resultado = False
        End If

        Return _Resultado

    End Function
    Public Function ObtenerMemoryStream(ByVal stream As Stream) As MemoryStream

        Dim mm As MemoryStream = New MemoryStream()

        Try

            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize) {}

            readCount = stream.Read(buffer, 0, bufferSize)

            While readCount > 0
                mm.Write(buffer, 0, readCount)
                readCount = stream.Read(buffer, 0, bufferSize)
            End While
            stream.Dispose()
        Catch ex As Exception
            mm = Nothing

        End Try

        Return mm
    End Function
    Public Function GenerarCadenaUnicaTiempo() As String

        Dim valCadenaUnica As String = String.Empty
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Year.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Month.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Day.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, "_")
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Hour.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Minute.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Second.ToString)
        valCadenaUnica = String.Concat(valCadenaUnica, DateTime.Now.Millisecond.ToString)
        Return valCadenaUnica

    End Function
    Public Function FormatoMoneda(ByVal valMonto As Decimal) As String
        Dim strResultado As String = String.Empty

        Try
            strResultado = String.Format("{0:#,##0.00}", valMonto)
        Catch ex As Exception
            strResultado = String.Empty
        End Try
        If strResultado = "0" Then strResultado = String.Empty

        Return strResultado
    End Function

    Public Function IdRequestCliente(ByVal Url As String)
        Dim strReturn As String = String.Empty
        Dim Parametro As String = String.Empty
        Dim ListaParametro() As String = Nothing
        Parametro = Url

        'Limpiar caracteres
        Parametro = Parametro.Replace("?", "")
        Parametro = Parametro.Replace("{", "")
        Parametro = Parametro.Replace("}", "")

        Parametro = Parametro.Replace("#", "")
        Parametro = Parametro.Replace("%", "")

        Parametro = Parametro.Replace("IdCotizacionSap", "")
        Parametro = Parametro.Replace("Usuario", "")
        Parametro = Parametro.Replace("rdm", "")
        Parametro = Parametro.Replace("=", "")

        ListaParametro = Parametro.Split("&")
        Dim cont As Integer = 0
        For Each valor In ListaParametro
            If cont > 2 Then Exit For 'Obtener solo valores de los 3 primeros parametros
            strReturn = String.Concat(strReturn, valor)
            cont += 1
        Next

        strReturn = strReturn.Trim()
        Return strReturn
    End Function

    Public Function CotizacionesDiferentes(ByVal eCotizacion As beCotizacion, ByVal eCotizacionRespaldo As beCotizacion) As Boolean
        Dim boolReturn As Boolean = False
        Dim cantMaquinaCotizacion As Integer = 0
        Dim cantMaquinaCotizacionRespaldo As Integer = 0
        Dim ListaMaquinaria As New List(Of beMaquinaria)
        Dim ListaMaquinariaRespaldo As New List(Of beMaquinaria)


        For contador As Integer = 0 To eCotizacionRespaldo.ListaProducto.Count - 1
            Dim oProductoRespaldo As New beProducto
            Dim oProducto As New beProducto
            oProductoRespaldo = eCotizacionRespaldo.ListaProducto(contador)
            oProducto = eCotizacion.ListaProducto(contador)
            ListaMaquinariaRespaldo = oProductoRespaldo.ProductoCSA.ListaMaquinaria
            ListaMaquinaria = oProducto.ProductoCSA.ListaMaquinaria

            cantMaquinaCotizacionRespaldo = ListaMaquinariaRespaldo.Count
            cantMaquinaCotizacion = ListaMaquinaria.Count

            If cantMaquinaCotizacion <> cantMaquinaCotizacionRespaldo Then
                boolReturn = True
                Exit For
            Else
                For iterador As Integer = 0 To ListaMaquinariaRespaldo.Count - 1
                    Dim oMaquinariaRespaldo As New beMaquinaria
                    Dim oMaquinaria As New beMaquinaria
                    'Dim boolEsIgual As Boolean = True
                    oMaquinariaRespaldo = ListaMaquinariaRespaldo(iterador)
                    oMaquinaria = ListaMaquinaria(iterador)
                    'Comparacion de maquinarias
                    If SinCambioPropiedadesClases(oMaquinariaRespaldo, oMaquinaria) = False Then
                        boolReturn = True
                        Exit For
                    End If
                Next
            End If
        Next


        Return boolReturn
    End Function
    Public Function SinCambioPropiedadesClases(ByVal _Objeto1 As Object, ByVal _Objeto2 As Object) As Boolean
        Dim _TipoObjeto1 As String = ""
        Dim _TipoObjeto2 As String = ""

        If Not _Objeto1 Is Nothing Then
            _TipoObjeto1 = _Objeto1.GetType.ToString
        End If

        If Not _Objeto2 Is Nothing Then
            _TipoObjeto2 = _Objeto2.GetType.ToString
        End If

        Dim _Resultado As Boolean = True

        If _TipoObjeto1 = _TipoObjeto2 Then
            Dim Propiedades() As PropertyInfo = _Objeto1.GetType.GetProperties
            Dim Propiedad As PropertyInfo
            Dim _Valor1 As Object
            Dim _Valor2 As Object
            For Each Propiedad In Propiedades
                _Valor1 = Propiedad.GetValue(_Objeto1, Nothing)
                _Valor2 = Propiedad.GetValue(_Objeto2, Nothing)

                If Not IsNothing(_Valor1) Then
                    Dim TipoDato1 As String = _Valor1.GetType.ToString()
                    Dim TipoDato2 As String = _Valor2.GetType.ToString()
                    If TipoDato1 = TipoDato2 Then
                        If _Valor1 <> _Valor2 Then
                            _Resultado = False
                            Exit For
                        End If
                    End If
                End If

            Next
        Else
            _Resultado = False
        End If

        Return _Resultado

    End Function
#End Region

End Module