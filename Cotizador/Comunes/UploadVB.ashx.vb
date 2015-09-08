Imports System.Web
Imports System.Web.Services
Imports System.IO
Imports System.Net

Public Class UploadVB
    Implements System.Web.IHttpHandler
    Private objEmail As New AdminEmail
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
         
        Dim str_mensajeRecorrido As New StringBuilder

        Try
            str_mensajeRecorrido.AppendLine("INICIO " & Now.ToString() & " : ===============================================")
            Dim postedFile As HttpPostedFile = context.Request.Files("Filedata")
            Dim oAdminFTP As New AdminFTP
            Dim RutaDestino As String = String.Empty
            Select Case context.Request("DESTINO")
                Case "PLANTILLAS"
                    RutaDestino = Modulo.strUrlFtpArchivoPlantillas
                Case "TEMPORAL"
                    RutaDestino = Modulo.strUrlFtpArchivoTemporal
                Case "COTIZACION_VERSION"
                    RutaDestino = Modulo.strUrlFtpCotizacionVersion
            End Select
             
            Dim mm As MemoryStream = New MemoryStream() 
            Dim stream As Stream = postedFile.InputStream
            ' obtener el MemoryStream
            Dim cl As Long = postedFile.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize) {}

            readCount = stream.Read(buffer, 0, bufferSize)

            While readCount > 0
                mm.Write(buffer, 0, readCount)
                readCount = stream.Read(buffer, 0, bufferSize)
            End While
            'stream.Dispose()

            Dim NombreArchivo As String = String.Empty
            Try
                NombreArchivo = context.Request("Archivo").ToString()
            Catch ex As Exception
                NombreArchivo = ""
            End Try

            str_mensajeRecorrido.AppendLine(Now.ToString() & " : Nombre Archivo - " & NombreArchivo)
            If String.IsNullOrEmpty(NombreArchivo) Then
                'SubirArchivoTemporal()
            Else
                If mm.Length > 0 Then
                    str_mensajeRecorrido.AppendLine(Now.ToString() & " : Tamaño archivo - " & mm.Length.ToString())

                    Dim UrlResultado As String = String.Empty
                    Dim ResultSubir As String = "0"

                    'Intentar 10 veces subir el archivo
                    For contador As Integer = 1 To 10
                        If ResultSubir = "0" Then
                            str_mensajeRecorrido.AppendLine(Now.ToString() & " : Inicio llamada metodo SubirArchivo() - " & contador.ToString)
                            UrlResultado = oAdminFTP.GuardarArchivo(mm, RutaDestino, NombreArchivo, ResultSubir)
                            str_mensajeRecorrido.AppendLine(Now.ToString() & " : Fin de llamada metodo SubirArchivo()")
                        Else
                            Exit For
                        End If
                    Next

                    str_mensajeRecorrido.AppendLine(Now.ToString() & " : Resultado Subir - " & ResultSubir)
                    str_mensajeRecorrido.AppendLine(Now.ToString() & " : Url Subir - " & UrlResultado)

                    'context.Response.Write("terminado satisfactoriamente...")
                    'context.Response.StatusCode = 200
                    'context.Response.StatusCode = HttpStatusCode.OK

                End If
            End If
            str_mensajeRecorrido.AppendLine("FIN    " & Now.ToString() & " : ===============================================")

        Catch ex As Exception
            'Escribir en el log el seguimiento de subir archivo
            Try
                'Call EscribirLog(str_mensajeRecorrido)
                'objEmail.EnvioEmail("Error en Subir Archivo", str_mensajeRecorrido)
            Catch ex1 As Exception
            End Try
        End Try

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Sub EscribirLog(ByVal mensaje As StringBuilder)

        ' Log de tareas
        Dim file As FileInfo
        Dim log As System.IO.StreamWriter
        Dim NombUrlLog As String
        Dim Fecha As String
        Fecha = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString
        NombUrlLog = Hosting.HostingEnvironment.ApplicationPhysicalPath & "\ArchivosCotizador\" & "logSubirArchivo_" & Fecha & ".txt" 'Server.MapPath("~\Log") & "\" & "logCuenta_" & Fecha & ".txt"

        Try
            ' RUTA PRINCIPAL
            file = New FileInfo(NombUrlLog)

            If Not file.Exists Then
                ' CREAR SI NO EXISTE
                log = New System.IO.StreamWriter(NombUrlLog)
            Else
                ' YA EXISTE
                log = New System.IO.StreamWriter(NombUrlLog, True)
            End If

            log.WriteLine(mensaje.ToString)
            log.Close()
            log.Dispose()
            GC.SuppressFinalize(log)
        Catch ex As Exception
            'Notificar el error por email
        End Try

    End Sub

     
End Class