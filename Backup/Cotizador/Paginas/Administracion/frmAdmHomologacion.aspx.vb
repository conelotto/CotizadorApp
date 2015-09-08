Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Imports System.Xml
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports log4net
Public Class frmAdmHomologacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ListarPaginado(ByVal sortColumn As String, ByVal sortOrder As String, _
                                             ByVal pageSize As String, ByVal currentPage As String, _
                                                          ByVal campoBusqueda As String, ByVal comparacionBusqueda As String, _
                                                          ByVal textoBuscar As String) As JQGridJsonResponse ' List(Of beArchivoConfiguracion)
        Dim lResponse As JQGridJsonResponse = Nothing

        Dim obeHomologacion As New beHomologacion
        Dim obcHomologacion As New bcHomologacion
        Dim listaReturn As New List(Of beHomologacion)
        Dim listaData As New List(Of beHomologacion)


        obcHomologacion.Listar(Modulo.strConexionSql, listaData)
        '===== Busqueda =======================
        If textoBuscar <> "" Then
            Select Case campoBusqueda.ToString.ToUpper()
                Case "TABLA"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.Tabla.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.Tabla.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.Tabla.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
                    Exit Select
                Case "DESCRIPCION"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.Descripcion.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.Descripcion.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.Descripcion.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
                    Exit Select
                Case "VALOR SAP"
                    If comparacionBusqueda.ToUpper = CStr("eq").ToUpper Then listaReturn = listaData.Where(Function(c) c.ValorSap.ToUpper = textoBuscar.ToUpper).ToList()
                    If comparacionBusqueda.ToUpper = CStr("bw").ToUpper Then listaReturn = listaData.Where(Function(c) c.ValorSap.ToUpper.StartsWith(textoBuscar.ToUpper)).ToList()
                    If comparacionBusqueda.ToUpper = CStr("cn").ToUpper Then listaReturn = listaData.Where(Function(c) c.ValorSap.ToUpper.Contains(textoBuscar.ToUpper)).ToList()
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
    Public Shared Function GuardarMantenimiento(ByVal Accion As String, _
                                                          ByVal IdHomologacion As String, _
                                                          ByVal Tabla As String, _
                                                          ByVal Descripcion As String, _
                                                          ByVal ValorSap As String, _
                                                          ByVal ValorCotizador As String) As String
        Dim strValorReturn As String = String.Empty
        ' valores de Reotrno:
        ' -1: No se puedo subir el Archivo
        '  0: No se pudo Guardar
        '  Numero: Codigo generado o guardado de obeArchivoConfiguracion.IdArchivoConfiguracion


        Dim obeHomologacion As New beHomologacion
        obeHomologacion.IdHomologacion = IdHomologacion
        obeHomologacion.Tabla = Tabla
        obeHomologacion.Descripcion = Descripcion
        obeHomologacion.ValorSap = ValorSap
        obeHomologacion.ValorCotizador = ValorCotizador 


        Dim ObcHomologacion As New bcHomologacion

        Try
            Select Case Accion.ToUpper
                Case "N"
                    If ObcHomologacion.Insertar(Modulo.strConexionSql, obeHomologacion) Then
                        strValorReturn = obeHomologacion.IdHomologacion
                    Else
                        strValorReturn = "0"
                    End If
                    Exit Select
                Case "M"
                    If ObcHomologacion.Actualizar(Modulo.strConexionSql, obeHomologacion) Then
                        strValorReturn = obeHomologacion.IdHomologacion
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
    Public Shared Function EliminarHomologacion(ByVal id As String, _
                                    ByVal usuario As String) As beValidacion

        Dim obeHomologacion As New beHomologacion
        Dim eValidacion As New beValidacion
        Dim obcHomologacion As New bcHomologacion
        eValidacion.flag = 3
        eValidacion.usuario = usuario
        obeHomologacion.IdHomologacion = id

        eValidacion.validacion = obcHomologacion.Eliminar(Modulo.strConexionSql, obeHomologacion, eValidacion)
        If Not eValidacion.validacion Then
            eValidacion.mensaje = "No se pudo eliminar"
        End If
        Return eValidacion

    End Function
End Class