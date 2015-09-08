Imports System
Imports System.Web
Imports System.IO

Public Class frmAdmCargaNE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hdnASPArchivo.Value = ""
        Label1.Font.Size = 9
        Label1.Font.Bold = True
        Label1.ForeColor = Drawing.Color.Red
        Label1.Text = "Ningún archivo se ha cargado aún."
    End Sub

    Protected Sub btnCargar_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mensaje As String = ""
        Dim NombreArchivo As String = String.Empty
        Try
            If (Archivo.HasFile) Then
                If (checkFileType(Archivo.FileName)) Then
                    Dim oAdminFTP As New AdminFTP
                    Dim RutaDestino As String = Modulo.strUrlFtpArchivoPlantillas

                    Dim file As HttpPostedFile
                    file = Request.Files("Archivo")

                    Dim mm As MemoryStream = New MemoryStream()
                    Dim stream As Stream = file.InputStream

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


                    Try
                        'NombreArchivo = file.FileName
                        NombreArchivo = GenerarNombreArchivo(file.FileName)
                        If mm.Length > 0 Then
                            Dim ResultSubir As String = "0"
                            'Intentar 10 veces subir el archivo
                            For contador As Integer = 1 To 10
                                If ResultSubir = "0" Then
                                    ResultSubir = oAdminFTP.SubirArchivoTemporal(mm, NombreArchivo)
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        hdnASPArchivo.Value = ""
                        Label1.ForeColor = Drawing.Color.Red
                        mensaje = "Error al subir el archivo al servidor FTP."
                    End Try

                    Label1.ForeColor = Drawing.Color.Green
                    mensaje = "Archivo preparado: " + Path.GetFileName(file.FileName)

                Else
                    Label1.ForeColor = Drawing.Color.Red
                    'Response.Write("<script language =Javascript> alert('Formato Invalido');</script>")
                    mensaje = "Formato de archivo incorrecto. Vuelva a intentarlo!"
                End If
            End If
        Catch
            hdnASPArchivo.Value = ""
            Label1.ForeColor = Drawing.Color.Red
            mensaje = "Error: Archivo no se puede subir al servidor."
        End Try
        hdnASPArchivo.value = NombreArchivo
        Label1.Font.Size = 9
        Label1.Font.Bold = True
        Label1.Text = mensaje
    End Sub


    Private Function checkFileType(ByVal fileName As String) As Boolean
        Dim fileExt As String = Path.GetExtension(Archivo.FileName)

        Select Case (fileExt.ToLower())
            Case ".doc"
                Return True
            Case ".docx"
                Return True
            Case ".xls"
                Return True
            Case ".xlsx"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function GenerarNombreArchivo(ByVal NombreArchivo As String) As String
        Dim NombreGenerado As String = String.Empty
        NombreGenerado = Modulo.GenerarNombreArchivo(NombreArchivo)
        Return NombreGenerado
    End Function

End Class