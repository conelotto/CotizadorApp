Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Imports System.Xml
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports log4net

Public Class frmAdmConfiguracionSeccion
    Inherits System.Web.UI.Page
    Private Shared Property Session_NombreArchivo() As Object
        Get
            Return HttpContext.Current.Session("NombreArchivo")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("NombreArchivo") = value
        End Set
    End Property
    Private Shared Property Session_Archivo() As Object
        Get 
            Return HttpContext.Current.Session("Archivo")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("Archivo") = value
        End Set
    End Property
    Public Shared ReadOnly Property UrlCarpetaFTP() As String
        Get
            Return Modulo.strUrlFTPArchivo
        End Get
    End Property
    Private Shared lResponse As JQGridJsonResponse = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Session("archivo") = String.Empty
        End If
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ListarSeccion() As List(Of beTablaMaestra)
        Dim obeValidacion As New beValidacion
        Dim obcTablaMaestra As New bcTablaMaestra
        Dim lista As New List(Of beTablaMaestra)

        obeValidacion.flag = "1"

        obcTablaMaestra.ListarSeccionesCotizaciones(Modulo.strConexionSql, obeValidacion, lista)

        Return lista
    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ListarCriteriosPorSeccion(ByVal IdSeccion As String) As List(Of beTablaMaestra)
        Dim obeValidacion As New beValidacion
        Dim obcTablaMaestra As New bcTablaMaestra
        Dim listaReturn As New List(Of beTablaMaestra)

        obeValidacion.flag = IdSeccion

        obcTablaMaestra.ListarCriteriosPorSeccion(Modulo.strConexionSql, obeValidacion, listaReturn)

        Return listaReturn

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ConfiguracionBuscarPorCriterioPaginado(ByVal sortColumn As String, ByVal sortOrder As String, _
                                             ByVal pageSize As String, ByVal currentPage As String, ByVal Tipo As String, _
                                                          ByVal IdSeccionCriterio As String, ByVal IdSubSeccionCriterio As String, _
                                                          ByVal campoBusqueda As String, ByVal comparacionBusqueda As String, _
                                                          ByVal textoBuscar As String) As JQGridJsonResponse ' List(Of beArchivoConfiguracion)
        Dim obeArchivoConfiguracion As New beArchivoConfiguracion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        Dim listaReturn As New List(Of beArchivoConfiguracion)
        Dim listaData As New List(Of beArchivoConfiguracion)

        obeArchivoConfiguracion.Tipo = Tipo
        obeArchivoConfiguracion.IdSeccionCriterio = IdSeccionCriterio
        obeArchivoConfiguracion.IdSubSeccionCriterio = IdSubSeccionCriterio

        obcArchivoConfiguracion.BuscarPorCriterio(Modulo.strConexionSql, obeArchivoConfiguracion, listaData)
        '===== Busqueda =======================
        If textoBuscar <> "" Then
            Select Case campoBusqueda.ToString.ToUpper()
                Case "CODIGO"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.Codigo.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.Codigo.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.Codigo.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
                    Exit Select
                Case "NOMBRE"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.Nombre.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.Nombre.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.Nombre.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
                    Exit Select
                Case "ARCHIVO"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.Archivo.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.Archivo.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.Archivo.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
                    Exit Select
            End Select
        End If
        '==============================================

        Dim eValidacion = New beValidacion

        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder
        eValidacion.pageSize = pageSize
        eValidacion.currentPage = currentPage

        If textoBuscar = "" Then
            Call ArmarPaginacion(eValidacion, listaData)
        Else
            Call ArmarPaginacion(eValidacion, listaReturn)
        End If

        If textoBuscar = "" Then
            lResponse = New JQGridJsonResponse(eValidacion.pageCount, currentPage, eValidacion.recordCount, listaData)
        Else
            lResponse = New JQGridJsonResponse(eValidacion.pageCount, currentPage, eValidacion.recordCount, listaReturn)
        End If

        Return lResponse

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function BuscarArchivoConfiguracionId(ByVal IdArchivoConfiguracion As String) As beArchivoConfiguracion
        Dim obeArchivoConfiguracion As New beArchivoConfiguracion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion

        obeArchivoConfiguracion.IdArchivoConfiguracion = IdArchivoConfiguracion

        obcArchivoConfiguracion.BuscarId(Modulo.strConexionSql, obeArchivoConfiguracion)

        Return obeArchivoConfiguracion

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GenerarNombreArchivo(ByVal NombreArchivo As String) As String
        Dim NombreGenerado As String = String.Empty
        NombreGenerado = Modulo.GenerarNombreArchivo(NombreArchivo)
        Return NombreGenerado
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarMantenimiento(ByVal Accion As String, _
                                                          ByVal IdArchivoConfiguracion As String, _
                                                          ByVal IdSeccionCriterio As String, _
                                                          ByVal IdSubSeccionCriterio As String, _
                                                          ByVal Tipo As String, _
                                                          ByVal Codigo As String, _
                                                          ByVal Nombre As String, _
                                                          ByVal Archivo As String, _
                                                          ByVal Valor As String, _
                                                          ByVal CargarArchivo As String, _
                                                          ByVal Usuario As String) As String
        Dim strValorReturn As String = String.Empty
        ' valores de Reotrno:
        ' -1: No se puedo subir el Archivo
        '  0: No se pudo Guardar
        '  Numero: Codigo generado o guardado de obeArchivoConfiguracion.IdArchivoConfiguracion

        If Not String.IsNullOrEmpty(Archivo) Then
            If CargarArchivo.ToUpper().Trim = "SI" Then
                Dim strMensajeSubir As String = SubirArchivoDe_Temporal_a_Plantillas(Archivo)
                If Not strMensajeSubir = "1" Then
                    strValorReturn = strMensajeSubir
                    Return strValorReturn
                End If
            End If
        End If

        Dim obeArchivoConfiguracion As New beArchivoConfiguracion
        obeArchivoConfiguracion.IdArchivoConfiguracion = IdArchivoConfiguracion
        obeArchivoConfiguracion.IdSeccionCriterio = IdSeccionCriterio
        obeArchivoConfiguracion.IdSubSeccionCriterio = IdSubSeccionCriterio
        obeArchivoConfiguracion.Tipo = Tipo
        obeArchivoConfiguracion.Codigo = Codigo
        obeArchivoConfiguracion.Nombre = Nombre
        obeArchivoConfiguracion.Archivo = Archivo
        obeArchivoConfiguracion.Valor = Valor
        Usuario = AdminSeguridad.DesEncriptar(Usuario)
        obeArchivoConfiguracion.UsuarioCreacion = Usuario
        obeArchivoConfiguracion.UsuarioModificacion = Usuario

        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion

        Try
            Select Case Accion.ToUpper
                Case "N"
                    If obcArchivoConfiguracion.Insertar(Modulo.strConexionSql, obeArchivoConfiguracion) Then
                        strValorReturn = obeArchivoConfiguracion.IdArchivoConfiguracion
                    Else
                        strValorReturn = "0"
                    End If
                    Exit Select
                Case "M"
                    If obcArchivoConfiguracion.Actualizar(Modulo.strConexionSql, obeArchivoConfiguracion) Then
                        strValorReturn = obeArchivoConfiguracion.IdArchivoConfiguracion
                    Else
                        strValorReturn = "0"
                    End If
                    Exit Select
            End Select
            strValorReturn = "1"
        Catch ex As Exception
            strValorReturn = "0"
        End Try

        Return strValorReturn
    End Function


    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function EliminarArchivoConfiguracion(ByVal id As String, _
                                    ByVal usuario As String) As beValidacion

        Dim obeArchivoConfiguracion As New beArchivoConfiguracion
        Dim eValidacion As New beValidacion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        eValidacion.flag = 3
        eValidacion.usuario = usuario
        obeArchivoConfiguracion.IdArchivoConfiguracion = id

        eValidacion.validacion = obcArchivoConfiguracion.Eliminar(Modulo.strConexionSql, obeArchivoConfiguracion, eValidacion)
        If Not eValidacion.validacion Then
            eValidacion.mensaje = "No se pudo eliminar"
        End If
        Return eValidacion

    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ListarMarcadores() As List(Of beMarcador)

        Dim listaMarcador As New List(Of beMarcador)
        Dim oValidacion As New beValidacion

        Dim obcTablaMaestra As New bcTablaMaestra

        obcTablaMaestra.MarcadorListar(Modulo.strConexionSql, oValidacion, listaMarcador)

        Return listaMarcador
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ListarMarcadorCotizacion(ByVal IdArchivoConfiguracion As String, ByVal NombreArchivo As String) As List(Of beMarcadorCotizacion)

        Dim listaMarcadorDocument As New List(Of String)
        Dim listaMarcadorCotizacion As New List(Of beMarcadorCotizacion)
        Dim listaMarcadorReturn As New List(Of beMarcadorCotizacion)
        Dim obcTablaMaestra As New bcTablaMaestra

        Dim obeMarcadorCotizacion As New beMarcadorCotizacion

        obeMarcadorCotizacion.IdArchivoConfiguracion = IdArchivoConfiguracion

        obcTablaMaestra.MarcadorBuscarIdArchivoConfig(Modulo.strConexionSql, obeMarcadorCotizacion, listaMarcadorCotizacion)
        Dim strUrlArchivo As String = Modulo.strUrlFtpArchivoPlantillas
        listaMarcadorDocument = Modulo.ObtenerCampos(strUrlArchivo, NombreArchivo)

        For Each MarcadorDocumento As String In listaMarcadorDocument
            Dim ebeMarcadorCotizacion As New beMarcadorCotizacion
            ebeMarcadorCotizacion.NombreMarcadorCotizacion = MarcadorDocumento.Trim
            ebeMarcadorCotizacion.IdArchivoConfiguracion = IdArchivoConfiguracion

            Dim beMarcadorCot As New beMarcadorCotizacion()
            beMarcadorCot = Nothing
            If listaMarcadorCotizacion.Count > 0 Then

                Try
                    beMarcadorCot = (From marcot In listaMarcadorCotizacion _
                                                                        Where marcot.NombreMarcadorCotizacion.Trim = MarcadorDocumento.ToString
                                                                        Select marcot).Last
                Catch ex As Exception
                    beMarcadorCot = Nothing
                End Try

            End If

            If Not IsNothing(beMarcadorCot) Then
                ebeMarcadorCotizacion.IdMarcadorCotizacion = beMarcadorCot.IdMarcadorCotizacion
                ebeMarcadorCotizacion.NombreMarcador = beMarcadorCot.NombreMarcador
            Else
                ebeMarcadorCotizacion.IdMarcadorCotizacion = "0"
                ebeMarcadorCotizacion.NombreMarcador = ""
            End If

            listaMarcadorReturn.Add(ebeMarcadorCotizacion)

        Next

        Return listaMarcadorReturn
    End Function
    Private Shared Function SubirArchivoDe_Temporal_a_Plantillas(ByVal NombreArchivo As String) As String
        Dim strResultado As String = ""
        Dim strMesanje As String = String.Empty
        Try
            'Subir el Archivo .- Linea para subir a carpeta FTP
            Dim oAdminFTP As New AdminFTP()
            Dim urldestino As String = Modulo.strUrlFtpArchivoPlantillas
            Dim urlOrigen As String = Modulo.strUrlFtpArchivoTemporal

            strMesanje = oAdminFTP.CopiarFichero(urlOrigen, urldestino, NombreArchivo, NombreArchivo)

            If strMesanje <> "1" Then
                strResultado = String.Concat("Error al subir el Archivo", " - ", strMesanje)
            Else
                strResultado = "1"
            End If


        Catch ex As Exception
            strResultado = "Error al subir el Archivo"
        End Try
        Return strResultado
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarMarcadores(ByVal data As Object) As String
        Dim strReturn As String = String.Empty
        Dim listaMarcadorCotizacion As New List(Of beMarcadorCotizacion)
        listaMarcadorCotizacion = DeserealizarCadena(data)
        Dim obcTablaMaesta As New bcTablaMaestra
        strReturn = obcTablaMaesta.MarcadorMantenimiento(Modulo.strConexionSql, listaMarcadorCotizacion)
        Return strReturn

    End Function
    Public Shared Function DeserealizarCadena(ByVal DataCadena As String) As List(Of beMarcadorCotizacion)
        Dim Lista As New List(Of beMarcadorCotizacion)
        Dim lineas As String()
        lineas = DataCadena.Split("|")
        For Each Rpt In lineas
            Dim subLineas As String()
            subLineas = Rpt.Split(";")
            Dim lResult As New beMarcadorCotizacion

            lResult.IdMarcadorCotizacion = subLineas(0)
            lResult.IdArchivoConfiguracion = subLineas(1)
            lResult.NombreMarcadorCotizacion = subLineas(2)
            lResult.NombreMarcador = subLineas(3)

            Lista.Add(lResult)
        Next

        Return Lista
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function EliminarArchivoTemporal(ByVal NombreArchivo As String) As String
        Dim strReturn As String = "-1"
        Dim oAdminFTP As New AdminFTP()
        Dim urldestino As String = String.Concat(Modulo.strUrlFtpArchivoTemporal, NombreArchivo)

        If oAdminFTP.existeObjeto(urldestino) Then
            oAdminFTP.eliminarFichero(urldestino)
            strReturn = "1"
        End If

        Return strReturn
    End Function

    Public Shared Function JsonDeserialize(Of T)(ByVal jsonString As String) As T
        Dim ser As New DataContractJsonSerializer(GetType(T))
        Dim ms As New MemoryStream(Encoding.UTF8.GetBytes(jsonString))
        Dim obj As T = CType(ser.ReadObject(ms), T)

        Return obj

    End Function

End Class