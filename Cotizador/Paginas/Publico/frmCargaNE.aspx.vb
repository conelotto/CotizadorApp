Imports System
Imports System.Web
Imports System.IO
Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador

Public Class frmCargaNE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hdnEstado.Value = "NOK"
        Label1.Font.Size = 9
        Label1.Font.Bold = True
        Label1.ForeColor = Drawing.Color.Red
        Label1.Text = "" ' "Ningún archivo se ha cargado aún."
    End Sub

    Protected Sub btnCargar_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mensaje As String = ""
        Dim NombreArchivo As String = String.Empty
        Try
            If (Archivo.HasFile) Then
                If (checkFileType(Archivo.FileName)) Then
                    Dim oAdminFTP As New AdminFTP
                    Dim RutaDestino As String = Modulo.strUrlFtpCotizacionVersion

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
                    Try
                        NombreArchivo = hdnNombreCotizacionVersion.Value
                        If mm.Length > 0 Then
                            Dim ResultSubir As String = "0"
                            For contador As Integer = 1 To 10
                                If ResultSubir = "0" Then
                                    mensaje = "Carga FTP = NOK."
                                    ResultSubir = oAdminFTP.GuardarArchivo(mm, RutaDestino, NombreArchivo, ResultSubir)
                                Else
                                    mensaje = "Carga FTP = OK."
                                    Exit For
                                End If
                            Next
                        End If
                        If ActualizarCotizacionVersion(hdnIdCotizacionVersion.Value, hdnNombreCotizacionVersion.Value).validacion Then
                            Label1.ForeColor = Drawing.Color.Green
                            mensaje = mensaje + " Actualizado correctamente."
                            hdnEstado.Value = "OK"
                        Else
                            Label1.ForeColor = Drawing.Color.Red
                            mensaje = mensaje + " Error: Archivo NO actualizado."
                            hdnEstado.Value = "NOK"
                        End If
                    Catch ex As Exception
                        Label1.ForeColor = Drawing.Color.Red
                        mensaje = mensaje + "Error: Carga FTP = NOK."
                        hdnEstado.Value = "NOK"
                    End Try
                Else
                    Label1.ForeColor = Drawing.Color.Red
                    mensaje = "Formato de archivo incorrecto(.doc/.docx). Vuelva a intentarlo!"
                    hdnEstado.Value = "NOK"
                End If
            End If
        Catch
            Label1.ForeColor = Drawing.Color.Red
            mensaje = "Error: Archivo no se puede subir al servidor."
            hdnEstado.Value = "NOK"
        End Try
        'hdnASPArchivo.Value = NombreArchivo
        Label1.Font.Size = 9
        Label1.Font.Bold = True
        Label1.Text = "" 'mensaje
    End Sub


    Private Function checkFileType(ByVal fileName As String) As Boolean
        Dim fileExt As String = Path.GetExtension(Archivo.FileName)

        Select Case (fileExt.ToLower())

            Case ".doc"
                Return True
            Case ".docx"
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

    Private Function ActualizarCotizacionVersion(ByVal IdCotizacionVersion As String, ByVal NombreArchivo As String) As beValidacion

        Dim eValidacion As New beValidacion
        Dim obeCotizacionVersion As New beCotizacionVersion
        Dim obcCotizacionVersion As New bcCotizacionVersion

        obeCotizacionVersion.IdCotizacionVersion = IdCotizacionVersion
        obeCotizacionVersion.NombreArchivo = NombreArchivo
        Try
            obcCotizacionVersion.ActualizarArchivo(strConexionSql, obeCotizacionVersion, eValidacion)
        Catch ex As Exception

            eValidacion.mensaje = ex.Message
            eValidacion.validacion = False
        End Try
        Return eValidacion

    End Function
End Class