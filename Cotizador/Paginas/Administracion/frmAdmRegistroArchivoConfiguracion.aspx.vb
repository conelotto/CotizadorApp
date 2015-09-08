Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador

Public Class frmAdmRegistroArchivoConfiguracion
    Inherits System.Web.UI.Page
    Private Property CodigoProducto() As String
    Private Property TipoSeccion() As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            
            If String.IsNullOrEmpty(Request.QueryString("transf")) Then
                Dim urlAdd As String

                If String.IsNullOrEmpty(Request.QueryString("cod")) Then
                    urlAdd = "?transf=SI"
                Else
                    Dim cod As String
                    Dim tipo As String
                    Dim usuario As String
                    Dim descripcion As String
                    cod = AdminSeguridad.Encriptar(Request.QueryString("cod"))
                    tipo = AdminSeguridad.Encriptar(Request.QueryString("tipo"))
                    usuario = AdminSeguridad.Encriptar(Request.QueryString("usu"))
                    descripcion = AdminSeguridad.Encriptar(Request.QueryString("dsc"))

                    urlAdd = "?cod=" + cod + "&tipo=" + tipo + "&usu=" + usuario + "&dsc=" + descripcion + "&transf=SI"
                End If
                Response.Redirect(Request.Path + urlAdd)
            Else

                If Not String.IsNullOrEmpty(Request.QueryString("cod")) Then
                    Me.hdfCodigoBusqueda.Value = AdminSeguridad.DesEncriptar(Request.QueryString("cod"))
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("tipo")) Then
                    Me.hdfTipoBusqueda.Value = AdminSeguridad.DesEncriptar(Request.QueryString("tipo"))
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("usu")) Then
                    Me.hdfUsuarioBusqueda.Value = AdminSeguridad.DesEncriptar(Request.QueryString("usu"))
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("dsc")) Then
                    Me.hdfDescripcionBusqueda.Value = AdminSeguridad.DesEncriptar(Request.QueryString("dsc"))
                End If

                Select Case hdfTipoBusqueda.Value
                    Case "ESPTEC" ' Especificacion Tecnica
                        Me.hdfCodSeccionBusqueda.Value = Modulo.strCodSeccionEspecifTecnica
                        Dim listaSecciones As New List(Of beTablaMaestra)
                        Dim obcTablaMaestra As New bcTablaMaestra
                        Dim obValidacion As New beValidacion
                        obValidacion.flag = 1
                        obcTablaMaestra.ListarSeccionesCotizaciones(Modulo.strConexionSql, obValidacion, listaSecciones)
                        If listaSecciones.Count > 0 Then
                            Dim obeSecion As beTablaMaestra
                            obeSecion = listaSecciones.Where(Function(a) a.IdSeccion = Me.hdfCodSeccionBusqueda.Value).FirstOrDefault
                            If Not IsNothing(obeSecion) Then
                                Me.lblTitulo.Text = obeSecion.Nombre
                            End If
                        End If
                        Exit Select
                    Case ""
                        Exit Select
                    Case Else
                        Exit Select
                End Select
            End If

        End If
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)> 
    Public Shared Function ConsultarRegistro(ByVal cod As String, ByVal tipo As String) As beArchivoConfiguracion
        Dim obeArchivoConfiguracion As New beArchivoConfiguracion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion

        Dim listaArchivoCotizacion As New List(Of beArchivoConfiguracion)

        Select Case tipo.ToUpper
            Case "ESPTEC" ' Especificacion Tecnica
                obeArchivoConfiguracion.Codigo = cod
                obeArchivoConfiguracion.CodigoSeccion = Modulo.strCodSeccionEspecifTecnica

                Exit Select
            Case ""
                Exit Select
            Case Else
                Exit Select
        End Select

        obcArchivoConfiguracion.BuscarCodigoYSeccion(Modulo.strConexionSql, obeArchivoConfiguracion, listaArchivoCotizacion)

        If listaArchivoCotizacion.Count > 0 Then
            obeArchivoConfiguracion = listaArchivoCotizacion.FirstOrDefault
        End If

        Return obeArchivoConfiguracion
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

        obeArchivoConfiguracion.UsuarioCreacion = Usuario
        obeArchivoConfiguracion.UsuarioModificacion = Usuario

        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion

        Try
            Select Case Accion.ToUpper
                Case "N"

                    Dim listaArchivoCotizacion As New List(Of beArchivoConfiguracion)
                    obcArchivoConfiguracion.BuscarCodigoYSeccion(Modulo.strConexionSql, obeArchivoConfiguracion, listaArchivoCotizacion)

                    If listaArchivoCotizacion.Count > 0 Then
                        obeArchivoConfiguracion = listaArchivoCotizacion.Where(Function(a) a.IdSeccionCriterio = obeArchivoConfiguracion.IdSeccionCriterio).FirstOrDefault
                        strValorReturn = "2"
                        Exit Select
                    End If

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
    Public Shared Function GenerarNombreArchivo(ByVal NombreArchivo As String) As String
        Dim NombreGenerado As String = String.Empty
        NombreGenerado = Modulo.GenerarNombreArchivo(NombreArchivo)
        Return NombreGenerado
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
End Class