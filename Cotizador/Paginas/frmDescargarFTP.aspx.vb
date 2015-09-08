Imports System.IO

Public Class frmDescargarFTP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim NombreArchivo As String = String.Empty
        Try
            NombreArchivo = Request.Params("NombreArchivo").ToString()
        Catch ex As Exception
            NombreArchivo = String.Empty
        End Try

        Dim Opcion As String = String.Empty
        Try
            Opcion = Request.Params("Opc").ToString()
        Catch ex As Exception
            Opcion = String.Empty
        End Try

        ViewState("NombreArchivo") = NombreArchivo
        ViewState("Opc") = Opcion

        If Not IsPostBack Then
            'Call Descargar()
            Call Descargar(NombreArchivo, Opcion)
        End If
    End Sub
    Private Sub Descargar(ByVal pNombreArchivo As String, ByVal pOpc As String)
        Dim NombreArchivo As String = pNombreArchivo
        Dim Opcion As String = pOpc

        'NombreArchivo = NombreArchivo.Replace("_", ".")
        Dim oAdminFTP As New AdminFTP
        Dim mm As MemoryStream = New MemoryStream()
        Dim strUrlArchivo As String = String.Empty

        Select Case Opcion.ToUpper
            Case "PLANTILLAS"
                strUrlArchivo = Modulo.strUrlFtpArchivoPlantillas
                Exit Select
            Case "ESPECIFICACION"
                strUrlArchivo = Modulo.strUrlFtpArchivoEspecificacion
                Exit Select
            Case "COTIZACION"
                strUrlArchivo = Modulo.strUrlFtpCotizacionVersion
                Exit Select
            Case "DETALLE_PARTES"
                strUrlArchivo = Modulo.strUrlFtpArchivoDetallePartes
                Exit Select
            Case "TEMPORAL"
                strUrlArchivo = Modulo.strUrlFtpArchivoTemporal
                Exit Select
            Case "TARIFASRS"
                strUrlArchivo = Modulo.strUrlFtpArchivoTarifaRS
                Exit Select
            Case "PARTESRS"
                strUrlArchivo = Modulo.strUrlFtpArchivoDetallePartesRS
                Exit Select
            Case Else
                strUrlArchivo = Modulo.strUrlFTPArchivo
                Exit Select

        End Select

        mm = oAdminFTP.ObtenerArchivo(strUrlArchivo, NombreArchivo)

        If Not IsNothing(mm) Then
            Response.HeaderEncoding = UTF8Encoding.UTF8
            Response.AppendHeader("content-disposition", "attachment; filename=" + NombreArchivo)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            Response.BinaryWrite(mm.ToArray())
            Response.Flush()
            Response.End()
        Else
            Response.Redirect("~/Paginas/frmErrorArchivoFTP.aspx")
        End If


    End Sub
    Private Sub Descargar()
        Dim NombreArchivo As String = ViewState("NombreArchivo")
        Dim Opcion As String = ViewState("Opc")

        'NombreArchivo = NombreArchivo.Replace("_", ".")
        Dim oAdminFTP As New AdminFTP
        Dim mm As MemoryStream = New MemoryStream()
        Dim strUrlArchivo As String = String.Empty

        Select Case Opcion.ToUpper
            Case "PLANTILLAS"
                strUrlArchivo = Modulo.strUrlFtpArchivoPlantillas
                Exit Select
            Case "ESPECIFICACION"
                strUrlArchivo = Modulo.strUrlFtpArchivoEspecificacion
                Exit Select
            Case "COTIZACION"
                strUrlArchivo = Modulo.strUrlFtpCotizacionVersion
                Exit Select
            Case "DETALLE_PARTES"
                strUrlArchivo = Modulo.strUrlFtpArchivoDetallePartes
            Case "TARIFASRS"
                strUrlArchivo = Modulo.strUrlFtpArchivoTarifaRS
                Exit Select
            Case "PARTESRS"
                strUrlArchivo = Modulo.strUrlFtpArchivoDetallePartesRS
                Exit Select
            Case Else
                strUrlArchivo = Modulo.strUrlFTPArchivo
                Exit Select

        End Select

        mm = oAdminFTP.ObtenerArchivo(strUrlArchivo, NombreArchivo)

        If Not IsNothing(mm) Then
            Response.HeaderEncoding = UTF8Encoding.UTF8
            Response.AppendHeader("content-disposition", "attachment; filename=" + NombreArchivo)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            Response.BinaryWrite(mm.ToArray())
            Response.Flush()
            Response.End()
        Else
            Response.Redirect("~/Paginas/frmErrorArchivoFTP.aspx")
        End If


    End Sub

    

End Class