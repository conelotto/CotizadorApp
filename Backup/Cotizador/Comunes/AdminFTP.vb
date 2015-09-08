Imports System.Net.FtpWebRequest
Imports System.Net
Imports System.IO
Imports log4net

Public Class AdminFTP

    Dim host, user, pass As String
    Public Sub New()
        Me.host = ""
        Me.user = ConfigurationManager.AppSettings.Get("userFTP")
        Me.pass = ConfigurationManager.AppSettings.Get("passFTP") 
    End Sub
    Public Sub New(ByVal host As String, ByVal user As String, ByVal pass As String)
        Me.host = host
        Me.user = user
        Me.pass = pass
    End Sub

    Public Function eliminarFichero(ByVal fichero As String) As String
        Dim peticionFTP As FtpWebRequest

        ' Creamos una petición FTP con la dirección del fichero a eliminar
        peticionFTP = CType(WebRequest.Create(New Uri(fichero)), FtpWebRequest)

        ' Fijamos el usuario y la contraseña de la petición
        peticionFTP.Credentials = New NetworkCredential(user, pass)

        ' Seleccionamos el comando que vamos a utilizar: Eliminar un fichero
        peticionFTP.Method = WebRequestMethods.Ftp.DeleteFile
        peticionFTP.UsePassive = False

        Try
            Dim respuestaFTP As FtpWebResponse
            respuestaFTP = CType(peticionFTP.GetResponse(), FtpWebResponse)
            respuestaFTP.Close()
            ' Si todo ha ido bien, devolvemos String.Empty
            Return String.Empty
        Catch ex As Exception
            ' Si se produce algún fallo, se devolverá el mensaje del error
            Return ex.Message
        End Try
    End Function

    Public Function existeObjeto(ByVal dir As String) As Boolean
        Dim peticionFTP As FtpWebRequest

        ' Creamos una peticion FTP con la dirección del objeto que queremos saber si existe
        peticionFTP = CType(WebRequest.Create(New Uri(dir)), FtpWebRequest)

        ' Fijamos el usuario y la contraseña de la petición
        peticionFTP.Credentials = New NetworkCredential(user, pass)

        ' Para saber si el objeto existe, solicitamos la fecha de creación del mismo
        peticionFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp

        peticionFTP.UsePassive = False

        Try
            ' Si el objeto existe, se devolverá True
            Dim respuestaFTP As FtpWebResponse
            respuestaFTP = CType(peticionFTP.GetResponse(), FtpWebResponse)
            Return True
        Catch ex As Exception
            ' Si el objeto no existe, se producirá un error y al entrar por el Catch
            ' se devolverá falso
            Return False
        End Try
    End Function

    Public Function creaDirectorio(ByVal dir As String) As String
        Dim peticionFTP As FtpWebRequest

        ' Creamos una peticion FTP con la dirección del directorio que queremos crear
        peticionFTP = CType(WebRequest.Create(New Uri(dir)), FtpWebRequest)

        ' Fijamos el usuario y la contraseña de la petición
        peticionFTP.Credentials = New NetworkCredential(user, pass)

        ' Seleccionamos el comando que vamos a utilizar: Crear un directorio
        peticionFTP.Method = WebRequestMethods.Ftp.MakeDirectory

        Try
            Dim respuesta As FtpWebResponse
            respuesta = CType(peticionFTP.GetResponse(), FtpWebResponse)
            respuesta.Close()
            ' Si todo ha ido bien, se devolverá String.Empty
            Return String.Empty
        Catch ex As Exception
            ' Si se produce algún fallo, se devolverá el mensaje del error
            Return ex.Message
        End Try
    End Function

    Public Function subirFichero(ByVal fichero As String, ByVal destino As String, _
    ByVal dir As String) As String
        Dim infoFichero As New FileInfo(fichero)

        Dim uri As String
        uri = destino

        ' Si no existe el directorio, lo creamos
        If Not existeObjeto(dir) Then
            creaDirectorio(dir)
        End If

        Dim peticionFTP As FtpWebRequest

        ' Creamos una peticion FTP con la dirección del fichero que vamos a subir
        peticionFTP = CType(FtpWebRequest.Create(New Uri(destino)), FtpWebRequest)

        ' Fijamos el usuario y la contraseña de la petición
        peticionFTP.Credentials = New NetworkCredential(user, pass)

        peticionFTP.KeepAlive = False
        peticionFTP.UsePassive = False

        ' Seleccionamos el comando que vamos a utilizar: Subir un fichero
        peticionFTP.Method = WebRequestMethods.Ftp.UploadFile

        ' Especificamos el tipo de transferencia de datos
        peticionFTP.UseBinary = True

        ' Informamos al servidor sobre el tamaño del fichero que vamos a subir
        peticionFTP.ContentLength = infoFichero.Length

        ' Fijamos un buffer de 2KB
        Dim longitudBuffer As Integer
        longitudBuffer = 2048
        Dim lector As Byte() = New Byte(2048) {}

        Dim num As Integer

        ' Abrimos el fichero para subirlo
        Dim fs As FileStream
        fs = infoFichero.OpenRead()

        Try
            Dim escritor As Stream
            escritor = peticionFTP.GetRequestStream()

            ' Leemos 2 KB del fichero en cada iteración
            num = fs.Read(lector, 0, longitudBuffer)

            While (num <> 0)
                ' Escribimos el contenido del flujo de lectura en el 
                ' flujo de escritura del comando FTP
                escritor.Write(lector, 0, num)
                num = fs.Read(lector, 0, longitudBuffer)
            End While

            escritor.Close()
            fs.Close()
            ' Si todo ha ido bien, se devolverá String.Empty
            Return String.Empty
        Catch ex As Exception
            ' Si se produce algún fallo, se devolverá el mensaje del error
            Return ex.Message
        End Try
    End Function

    Public Function ObtenerArchivo(ByVal urlFolderArchivo As String, ByVal nombreArchivo As String) As MemoryStream

        Dim folderFTP As String = urlFolderArchivo
        Dim userFTP As String = ConfigurationManager.AppSettings.Get("userFTP")
        Dim passwordFTP As String = ConfigurationManager.AppSettings.Get("passFTP")

        Dim mm As MemoryStream = New MemoryStream()

        'Dim extencion As String
        'Dim mimeType As String = ""
        Try
            Dim request As FtpWebRequest = FtpWebRequest.Create(New Uri(folderFTP + nombreArchivo))
            SyncLock request
                request.Credentials = New NetworkCredential(userFTP, passwordFTP)
                Using response As WebResponse = request.GetResponse()
                    Using stream As Stream = response.GetResponseStream()
                        'extencion = Path.GetExtension(request.RequestUri.ToString())
                        ' obtener el MemoryStream
                        Dim cl As Long = response.ContentLength
                        Dim bufferSize As Integer = 2048
                        Dim readCount As Integer
                        Dim buffer As Byte() = New Byte(bufferSize) {}

                        readCount = stream.Read(buffer, 0, bufferSize)

                        While readCount > 0
                            mm.Write(buffer, 0, readCount)
                            readCount = stream.Read(buffer, 0, bufferSize)
                        End While
                        stream.Dispose()
                    End Using
                End Using
            End SyncLock
        Catch ex As Exception
            mm = Nothing

        End Try

        Return mm
    End Function

    Public Function SubirArchivoTemporal(ByVal streamArchivo As Stream, ByVal nombreArchivo As String) As String
        Dim folderFTPTemporal As String = Modulo.strUrlFtpArchivoTemporal
        Dim userFTP As String = ConfigurationManager.AppSettings.Get("userFTP")
        Dim passwordFTP As String = ConfigurationManager.AppSettings.Get("passFTP")

        Dim rutaArchivoFTP As String = String.Concat(folderFTPTemporal, nombreArchivo)
        Dim strResultado = String.Empty

        If existeObjeto(rutaArchivoFTP) Then
            eliminarFichero(rutaArchivoFTP)
        End If
        Try

            Dim reqFTP As FtpWebRequest = FtpWebRequest.Create(New Uri(rutaArchivoFTP))
            reqFTP.Credentials = New NetworkCredential(userFTP, passwordFTP)

            reqFTP.KeepAlive = False
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile
            reqFTP.UseBinary = True
            reqFTP.ContentLength = 2048

            Dim buffLength As Integer = streamArchivo.Length
            Dim buff As Byte() = New Byte(buffLength) {}

            Dim contentLen As Integer = 0

            Dim strm As Stream = Nothing

            Try
                strm = reqFTP.GetRequestStream()
                'posicionar la incio para poder leer el stream
                streamArchivo.Position = 0
                'leer
                contentLen = streamArchivo.Read(buff, 0, buffLength)

                While contentLen <> 0
                    strm.Write(buff, 0, contentLen)
                    contentLen = streamArchivo.Read(buff, 0, buffLength)
                End While
                If Not strm Is Nothing Then
                    strm.Close()
                End If
                strResultado = "1"
            Catch ex As Exception
                strResultado = ex.Message.ToString
                If Not strm Is Nothing Then
                    strm.Close()
                End If

            End Try
        Catch ex As Exception
            strResultado = ex.Message.ToString
        End Try
        Return strResultado
    End Function

    Public Function SubirArchivo(ByVal streamArchivo As Stream, ByVal urlDestino As String, ByVal nombreArchivo As String) As String

        Dim folderFTP As String = urlDestino
        Dim userFTP As String = ConfigurationManager.AppSettings.Get("userFTP")
        Dim passwordFTP As String = ConfigurationManager.AppSettings.Get("passFTP")

        Dim rutaArchivoFTP As String = folderFTP + nombreArchivo

        Dim reqFTP As FtpWebRequest = FtpWebRequest.Create(New Uri(rutaArchivoFTP))
        reqFTP.Credentials = New NetworkCredential(userFTP, passwordFTP)

        reqFTP.Method = WebRequestMethods.Ftp.UploadFile
        reqFTP.ContentLength = streamArchivo.Length

        Dim buffLength As Integer = 2048
        Dim buff As Byte() = New Byte(buffLength) {}

        Dim contentLen As Integer

        Using strm As Stream = reqFTP.GetRequestStream()
            Try
                'strm = reqFTP.GetRequestStream()

                'posicionar la incio para poder leer el stream
                streamArchivo.Position = 0
                'leer
                contentLen = streamArchivo.Read(buff, 0, buffLength)
                While contentLen <> 0
                    strm.Write(buff, 0, contentLen)
                    contentLen = streamArchivo.Read(buff, 0, buffLength)
                End While

                If Not strm Is Nothing Then
                    strm.Close()
                End If

                'Return rutaArchivoFTP
            Catch ex As Exception
                rutaArchivoFTP = "0"
                If Not strm Is Nothing Then
                    strm.Close()
                End If
            End Try
        End Using
        Return rutaArchivoFTP
    End Function

    Public Function GuardarArchivo(ByVal streamArchivo As Stream, ByVal urlDestino As String, ByVal nombreArchivo As String, ByRef Resultado As String) As String

        Resultado = "0"

        Dim folderFTP As String = urlDestino
        Dim userFTP As String = ConfigurationManager.AppSettings.Get("userFTP")
        Dim passwordFTP As String = ConfigurationManager.AppSettings.Get("passFTP")

        Dim rutaArchivoFTP As String = folderFTP + nombreArchivo

        Dim reqFTP As FtpWebRequest = FtpWebRequest.Create(New Uri(rutaArchivoFTP))
        reqFTP.Credentials = New NetworkCredential(userFTP, passwordFTP)

        reqFTP.Method = WebRequestMethods.Ftp.UploadFile
        reqFTP.ContentLength = streamArchivo.Length


        Dim buffLength As Integer = 2048
        Dim buff As Byte() = New Byte(buffLength) {}

        Dim contentLen As Integer

        Dim strm As Stream = Nothing
        Try
            strm = reqFTP.GetRequestStream()

            'posicionar la incio para poder leer el stream
            streamArchivo.Position = 0
            'leer
            contentLen = streamArchivo.Read(buff, 0, buffLength)
            While contentLen <> 0
                strm.Write(buff, 0, contentLen)
                contentLen = streamArchivo.Read(buff, 0, buffLength)
            End While

            Resultado = "1"
            If Not strm Is Nothing Then
                strm.Close()
            End If

            'Return rutaArchivoFTP
        Catch ex As Exception
            rutaArchivoFTP = "0"
            Resultado = "0"
            If Not strm Is Nothing Then
                strm.Close()
            End If
        End Try

        Return rutaArchivoFTP
    End Function
    
    Public Function CopiarFichero(ByVal UrlOrigen As String, ByVal urldestino As String, _
                                  ByVal NombreArchivoOrigen As String, ByVal NombreArchivoDestino As String) As String

        Dim strResultado = String.Empty
        UrlOrigen = String.Concat(UrlOrigen, NombreArchivoOrigen)
        urldestino = String.Concat(urldestino, NombreArchivoDestino)

        ' Si no existe el directorio, lo creamos
        'If Not existeObjeto(dirFolder) Then
        '    creaDirectorio(dirFolder)
        'End If

        Dim peticionFTP As FtpWebRequest
        '==== Obtener Stream del Archivo Temporal=====
        Dim request As FtpWebRequest = FtpWebRequest.Create(New Uri(UrlOrigen))
        request.Credentials = New NetworkCredential(user, pass)
        Dim response As WebResponse = request.GetResponse()

        Dim stream As Stream = response.GetResponseStream()
        '============================================
        ' Creamos una peticion FTP con la dirección del fichero que vamos a subir
        peticionFTP = CType(FtpWebRequest.Create(New Uri(urldestino)), FtpWebRequest)

        ' Fijamos el usuario y la contraseña de la petición
        peticionFTP.Credentials = New NetworkCredential(user, pass)

        peticionFTP.KeepAlive = False
        'peticionFTP.UsePassive = False       

        ' Seleccionamos el comando que vamos a utilizar: Subir un fichero
        peticionFTP.Method = WebRequestMethods.Ftp.UploadFile

        ' Especificamos el tipo de transferencia de datos
        peticionFTP.UseBinary = True

        ' Informamos al servidor sobre el tamaño del fichero que vamos a subir
        peticionFTP.ContentLength = 2048  'infoFichero.Length

        ' Fijamos un buffer de 2KB
        Dim longitudBuffer As Integer
        longitudBuffer = 2048
        Dim lector As Byte() = New Byte(2048) {}

        Dim num As Integer

        Try
            Dim escritor As Stream
            escritor = peticionFTP.GetRequestStream()

            ' Leemos 2 KB del fichero en cada iteración
            num = stream.Read(lector, 0, longitudBuffer)

            While (num <> 0)
                ' Escribimos el contenido del flujo de lectura en el 
                ' flujo de escritura del comando FTP
                escritor.Write(lector, 0, num)
                num = stream.Read(lector, 0, longitudBuffer)
            End While

            escritor.Close()
            stream.Close()

            'If existeObjeto(UrlOrigen) Then
            '    eliminarFichero(UrlOrigen)
            'End If
            ' Si todo ha ido bien, se devolverá 1
            strResultado = "1"
        Catch ex As Exception
            ' Si se produce algún fallo, se devolverá el mensaje del error
            strResultado = ex.Message
        End Try
        Return strResultado
    End Function

    'Public Function CopiarFicheroTemp(ByVal urldestino As String, ByVal NombreArchivo As String) As String

    '    Dim strResultado = String.Empty
    '    Dim UrlOrigen As String = Modulo.strUrlFtpArchivoTemporal & "Temp.docx"
    '    urldestino = String.Concat(urldestino, NombreArchivo)

    '    ' Si no existe el directorio, lo creamos
    '    'If Not existeObjeto(dirFolder) Then
    '    '    creaDirectorio(dirFolder)
    '    'End If

    '    Dim peticionFTP As FtpWebRequest
    '    '==== Obtener Stream del Archivo Temporal=====
    '    Dim request As FtpWebRequest = FtpWebRequest.Create(New Uri(UrlOrigen))
    '    request.Credentials = New NetworkCredential(user, pass)
    '    Dim response As WebResponse = request.GetResponse()

    '    Dim stream As Stream = response.GetResponseStream()
    '    '============================================
    '    ' Creamos una peticion FTP con la dirección del fichero que vamos a subir
    '    peticionFTP = CType(FtpWebRequest.Create(New Uri(urldestino)), FtpWebRequest)

    '    ' Fijamos el usuario y la contraseña de la petición
    '    peticionFTP.Credentials = New NetworkCredential(user, pass)

    '    peticionFTP.KeepAlive = False
    '    'peticionFTP.UsePassive = False       

    '    ' Seleccionamos el comando que vamos a utilizar: Subir un fichero
    '    peticionFTP.Method = WebRequestMethods.Ftp.UploadFile

    '    ' Especificamos el tipo de transferencia de datos
    '    peticionFTP.UseBinary = True

    '    ' Informamos al servidor sobre el tamaño del fichero que vamos a subir
    '    peticionFTP.ContentLength = 2048  'infoFichero.Length

    '    ' Fijamos un buffer de 2KB
    '    Dim longitudBuffer As Integer
    '    longitudBuffer = 2048
    '    Dim lector As Byte() = New Byte(2048) {}

    '    Dim num As Integer

    '    Try
    '        Dim escritor As Stream
    '        escritor = peticionFTP.GetRequestStream()

    '        ' Leemos 2 KB del fichero en cada iteración
    '        num = stream.Read(lector, 0, longitudBuffer)

    '        While (num <> 0)
    '            ' Escribimos el contenido del flujo de lectura en el 
    '            ' flujo de escritura del comando FTP
    '            escritor.Write(lector, 0, num)
    '            num = stream.Read(lector, 0, longitudBuffer)
    '        End While

    '        escritor.Close()
    '        stream.Close()

    '        If existeObjeto(UrlOrigen) Then
    '            eliminarFichero(UrlOrigen)
    '        End If
    '        ' Si todo ha ido bien, se devolverá 1
    '        strResultado = "1"
    '    Catch ex As Exception
    '        ' Si se produce algún fallo, se devolverá el mensaje del error
    '        strResultado = ex.Message
    '    End Try
    '    Return strResultado
    'End Function

End Class

