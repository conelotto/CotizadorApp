Public Class AdminEmail

    Public Function EnvioEmail(ByVal pst_strAsunto As String, _
                               ByVal pst_strMensaje As StringBuilder) As Boolean

        EnvioEmail = False


        Dim strSMTPServer As String = ConfigurationManager.AppSettings.Get("SMTPServer")
        Dim strServerPort As String = ConfigurationManager.AppSettings.Get("ServerPort")
        Dim pst_strEmailFrom As String = ConfigurationManager.AppSettings.Get("EmailFrom")
        Dim pst_strEmailCc As String = ConfigurationManager.AppSettings.Get("EmailCc")
        Dim pst_strEmailTo As String = ConfigurationManager.AppSettings.Get("EmailTo")

        'Variable con la que enviamos el correo
        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = strSMTPServer
        smtp.Port = strServerPort

        'Variable que almacena los Attachment
        Dim correo As New System.Net.Mail.MailMessage
        Dim Attachment As System.Net.Mail.Attachment

        Try

            'Indicamos el correo al que se le va a enviar el mensaje
            correo.From = New System.Net.Mail.MailAddress(pst_strEmailFrom)
            correo.CC.Add(pst_strEmailCc)
            correo.To.Add(pst_strEmailTo)

            'Asunto
            pst_strAsunto = Environment.MachineName & " - " & pst_strAsunto
            correo.Subject = pst_strAsunto

            'Attachment = New MailAttachment(nombrePDF.DiskFileName.ToString)
            'correo.Attachments.Add(Attachment)
            Dim mensajeHtml As String = String.Empty
            Dim LineasMensaje() As String = Split(pst_strMensaje.ToString, Chr(13))

            mensajeHtml = mensajeHtml + "<div style ='color:Red'>"

            For Each linea As String In LineasMensaje
                mensajeHtml = mensajeHtml + "<p>" + linea + "</p>"
            Next

            mensajeHtml = mensajeHtml + "</div>"
            ' Cuerpo
            correo.Body = mensajeHtml ' "Mensaje de error obtenido al Consultar Clientes : " & Chr(13) & Chr(13) & pst_strMensaje

            ' Tipo de Formato
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal
            smtp.Send(correo)

            EnvioEmail = True

        Catch ex As Exception

            Throw

        End Try

        Return EnvioEmail

    End Function
End Class
