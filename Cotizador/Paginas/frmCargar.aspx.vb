Imports System.IO

Public Class frmCargar
    Inherits System.Web.UI.Page

    'Private _NombreArchivo As String
    'Public Property NombreArchivo As String
    '    Get
    '        Return _NombreArchivo
    '    End Get
    '    Set(ByVal value As String)
    '        _NombreArchivo = value
    '    End Set
    'End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Not IsPostBack Then

            'Try
            '    NombreArchivo = Request("Archivo").ToString()

            'Catch ex As Exception
            '    NombreArchivo = String.Empty
            'End Try
            Dim NombreArchivo As String = String.Empty
            Try
                NombreArchivo = Request("Archivo").ToString()
            Catch ex As Exception
                NombreArchivo = ""
            End Try

            If String.IsNullOrEmpty(NombreArchivo) Then
                SubirArchivoTemporal()
            Else
                Call SubirArchivoCotizacionVersion(NombreArchivo)
            End If



        End If
    End Sub

    Private Sub SubirArchivoTemporal()

        Try
            Dim file As HttpPostedFile
            file = Request.Files("FileData")
            'HttpContext.Current.Session("NombreArchivo") = NombreArchivo

            '===== Subir fisicamente a la carpeta temporal === 
            Dim oAdminFTP As New AdminFTP
            oAdminFTP.SubirArchivoTemporal(file.InputStream, "Temp.docx")
            '=================================================
        Catch ex As Exception
        End Try

    End Sub

    Private Sub SubirArchivoCotizacionVersion(ByVal NombreArchivo As String)

        Try
            Dim file As HttpPostedFile
            file = Request.Files("FileData")
            '===============================================
            Dim mm As MemoryStream = New MemoryStream()
            Dim stream As Stream = file.InputStream'Response.GetResponseStream()
            'extencion = Path.GetExtens ion(request.RequestUri.ToString())
            ' obtener el MemoryStream
            Dim cl As Long = file.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize) {}

            readCount = stream.Read(buffer, 0, bufferSize)

            While readCount > 0
                mm.Write(buffer, 0, readCount)
                readCount = stream.Read(buffer, 0, bufferSize)
            End While
            'stream.Dispose()
            '==================================================

            Dim oAdminFTP As New AdminFTP
            Dim RutaDestino As String = Modulo.strUrlFtpCotizacionVersion

            oAdminFTP.SubirArchivo(mm, RutaDestino, NombreArchivo)
            '=================================================
        Catch ex As Exception
        End Try

    End Sub

End Class