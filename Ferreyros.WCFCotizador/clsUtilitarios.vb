Imports Ferreyros.ClasServicioCotizador
Imports System.Web.Configuration
Imports log4net
Imports Ferreyros.Utiles

Public Class clsUtilitarios

    ''' <summary>
    ''' ENVIO DE EMAIL INTERNO
    ''' </summary>
    ''' <param name="pst_strMensaje"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(clsUtilitarios))

    Private strConexionSQL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConexionCotizador")
    Private strUrlFtpDescarga As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpDescarga")
    Dim eValidacion As New beValidacion
    Dim uConfig As New uConfiguracion

    Public Function EnvioEmail(ByVal pst_strAsunto As String, _
                               ByVal pst_strMensaje As StringBuilder, ByVal pst_TipoMensaje As String) As Boolean

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

            '===============================
            Dim nomColor As String = "black"
            Select Case pst_TipoMensaje
                Case TipoMensaje.ERR
                    nomColor = "Red"
                Case TipoMensaje.SUCCES
                    nomColor = "black"
                Case Else
                    nomColor = "black"
            End Select
            '=================================

            mensajeHtml = mensajeHtml + "<div style ='color:" + nomColor + "'>"

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

    Public Function EnvioEmailCotizador(ByVal oEmail As beEmail) As Boolean

        Dim bolResultado = False


        Dim strSMTPServer As String = ConfigurationManager.AppSettings.Get("SMTPServer")
        Dim strServerPort As String = ConfigurationManager.AppSettings.Get("ServerPort")
        Dim pst_strEmailFrom As String = ConfigurationManager.AppSettings.Get("EmailFrom")

        'Variable con la que enviamos el correo
        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = strSMTPServer
        smtp.Port = strServerPort

        'Variable que almacena los Attachment
        Dim correo As New System.Net.Mail.MailMessage
        Dim Attachment As System.Net.Mail.Attachment

        Call LogPropiedadesEmail(oEmail)

        Try

            'Indicamos el correo al que se le va a enviar el mensaje
            correo.From = New System.Net.Mail.MailAddress(pst_strEmailFrom)

            'Destinatario
            If Not oEmail.EmailResponsableServicio Is Nothing Then
                If oEmail.EmailResponsableServicio.ToString <> String.Empty Then
                    correo.To.Add(oEmail.EmailResponsableServicio)
                    'Copiar Correo a destinatario
                    For Each email As beEmailDato In oEmail.ListaEmailCopia
                        If Not email.EmailDireccion Is Nothing Then
                            If email.EmailDireccion.ToString <> String.Empty Then
                                correo.CC.Add(email.EmailDireccion)
                            End If
                        End If
                    Next
                Else 'responsable vacio
                    'Enviar a todos los de la lista de Email
                    For Each email As beEmailDato In oEmail.ListaEmailCopia
                        If Not email.EmailDireccion Is Nothing Then
                            If email.EmailDireccion.ToString <> String.Empty Then
                                correo.To.Add(email.EmailDireccion)
                            End If
                        End If
                    Next
                End If
            Else
                'Enviar a todos los de la lista de Email
                For Each email As beEmailDato In oEmail.ListaEmailCopia
                    If Not email.EmailDireccion Is Nothing Then
                        If email.EmailDireccion.ToString <> String.Empty Then
                            correo.To.Add(email.EmailDireccion)
                        End If
                    End If
                Next
            End If

            For Each email As beEmailDato In oEmail.ListaEmailCopiaOculta
                If Not email.EmailDireccion Is Nothing Then
                    If email.EmailDireccion.ToString <> String.Empty Then
                        correo.Bcc.Add(email.EmailDireccion)
                    End If
                End If

            Next
            'Asunto
            correo.Subject = "Se ha ganado item CSA para el cliente " & oEmail.RazonSocial

            'Attachment = New MailAttachment(nombrePDF.DiskFileName.ToString)
            'correo.Attachments.Add(Attachment)

            ' Cuerpo
            correo.Body = CuerpoHtmlMensaje(oEmail) ' "Mensaje de error obtenido al Consultar Clientes : " & Chr(13) & Chr(13) & pst_strMensaje

            ' Tipo de Formato
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal

            If correo.To.LongCount > 0 Then
                smtp.Send(correo)
                Call LogDestinatariosEmail(correo)
            Else
                Throw New Exception("No hay Destinatarios")
            End If

            bolResultado = True

        Catch ex As Exception
            Dim textoError As New Text.StringBuilder
            textoError.AppendLine("No se pudo enviar el correo.")
            If Not oEmail.CodigoOportunidadSAP Is Nothing Then
                textoError.AppendLine("Nro Oportunidad :" & oEmail.CodigoOportunidadSAP)
            Else
                textoError.AppendLine("Nro Oportunidad : null")
            End If

            If Not oEmail.NroItem Is Nothing Then
                textoError.AppendLine("Nro Item :" & oEmail.NroItem)
            Else
                textoError.AppendLine("Nro Item : null")
            End If

            If Not oEmail.CodigoCotizacionSap Is Nothing Then
                textoError.AppendLine("Nro Cotización :" & oEmail.CodigoCotizacionSap)
            Else
                textoError.AppendLine("Nro Cotización : null")
            End If
            If Not oEmail.RazonSocial Is Nothing Then
                textoError.AppendLine("Razón Social :" & oEmail.RazonSocial)
            Else
                textoError.AppendLine("Razón Social : null")
            End If

            textoError.AppendLine("Mensaje Error :" + ex.Message)
            textoError.AppendLine("Stack Trace :" + ex.StackTrace)

            Call EnvioEmail("Error en Envío de Correo", textoError, TipoMensaje.ERR)

        End Try

        Return bolResultado

    End Function
    Private Function CuerpoHtmlMensaje(ByVal oEmail As beEmail) As String
        Dim str_mensaje As String = String.Empty
        Dim divAbre As String = "<div style ='width:100%'>"
        Dim divCierre As String = "</div>"
        str_mensaje = "<div>"
        str_mensaje = str_mensaje & divAbre & "Se ha ganado el item nro. " & ConvertirEntero(oEmail.NroItem) & " de la oportunidad CSA " & ConvertirEntero(oEmail.CodigoOportunidadSAP) & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

        str_mensaje = str_mensaje & divAbre & "Código Cliente: " & divCierre
        str_mensaje = str_mensaje & divAbre & oEmail.CodigoCliente & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

        str_mensaje = str_mensaje & divAbre & "Cliente: " & divCierre
        str_mensaje = str_mensaje & divAbre & oEmail.RazonSocial & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

        'Homologar Tipo CSA
        Dim lengclase As Integer = 0
        Dim IdTipoClaseCsa As String = oEmail.TipoCSA
        Dim NombreTipoCSA As String = String.Empty

        lengclase = clsUtilitarios.CodigoTipoCSA.Plan.ToString.Length
        If IdTipoClaseCsa.Length >= lengclase Then
            IdTipoClaseCsa = IdTipoClaseCsa.Substring(0, lengclase)
        End If

        Select Case IdTipoClaseCsa
            Case clsUtilitarios.CodigoTipoCSA.Plan
                NombreTipoCSA = clsUtilitarios.NombreTipoCSA.Planes
                Exit Select
            Case clsUtilitarios.CodigoTipoCSA.Acuerdo
                NombreTipoCSA = clsUtilitarios.NombreTipoCSA.Acuerdos
                Exit Select
        End Select

        str_mensaje = str_mensaje & divAbre & NombreTipoCSA & ": " & divCierre
        str_mensaje = str_mensaje & divAbre & oEmail.NombreProducto & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre


        Dim nCotizacion As New bcCotizacion
        Dim beCotizacionversion As New beCotizacionVersion
        Dim enlaceCotizacion As String = String.Empty
        Dim enlaceDetallePartes As String = String.Empty

        beCotizacionversion.IdCotizacionSap = oEmail.CodigoCotizacionSap
        Dim listaCotizacionVersion As New List(Of beCotizacionVersion)

        Call nCotizacion.BuscarIdCotizacionSap(strConexionSQL, beCotizacionversion, listaCotizacionVersion)
        If listaCotizacionVersion.Count > 0 Then

            enlaceCotizacion = "<a href ='" & strUrlFtpDescarga & "?NombreArchivo=" & listaCotizacionVersion.FirstOrDefault().NombreArchivo & "&Opc=COTIZACION' target ='_blank' >" + listaCotizacionVersion.FirstOrDefault().NombreArchivo + "</a>"

            str_mensaje = str_mensaje & divAbre & "Cotizacion:" & divCierre
            str_mensaje = str_mensaje & divAbre & enlaceCotizacion & divCierre
            str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

            Dim obeCotizacionVersion As beCotizacionVersion = listaCotizacionVersion.Where(Function(c) c.TieneDetalleParte = "1" Or c.TieneDetalleParte = "TRUE").FirstOrDefault
            If Not IsNothing(obeCotizacionVersion) Then

                If obeCotizacionVersion.TieneDetalleParte = "1" Or obeCotizacionVersion.ToString.ToUpper = "TRUE" Then

                    str_mensaje = str_mensaje & divAbre & "Detalle de Partes:" & divCierre
                    enlaceDetallePartes = "<a href ='" & strUrlFtpDescarga & "?NombreArchivo=" & obeCotizacionVersion.IdCotizacionSap & "-v." & obeCotizacionVersion.NumVersion & ".xlsx" & "&Opc=DETALLE_PARTES'" & " target ='_blank' >" + obeCotizacionVersion.IdCotizacionSap & "-v." & obeCotizacionVersion.NumVersion & ".xlsx" + "</a>"
                    str_mensaje = str_mensaje & divAbre & enlaceDetallePartes & divCierre
                    str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

                End If

            End If
        End If


        str_mensaje = str_mensaje & divAbre & "Orden de Compra y aprobación de Crédito: " & divCierre

        For Each link As beLink In oEmail.ListaLink
            Dim enlace As String
            enlace = "<a href ='" + link.Url + "' target ='_blank' >" + link.Nombre + "</a>"
            str_mensaje = str_mensaje & divAbre & enlace & divCierre
        Next

        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre
        str_mensaje = str_mensaje & divAbre & "Favor no responder este correo." & divCierre
        str_mensaje = str_mensaje & divAbre & "<br/>" & divCierre

        str_mensaje = str_mensaje & "</div>"
        Return str_mensaje
    End Function


    Public Function GenerarRandom() As String
        Dim strReturn As String = String.Empty
        strReturn = Now.Date.Year.ToString
        strReturn = String.Concat(strReturn, Now.Date.Month.ToString())
        strReturn = String.Concat(strReturn, Now.Date.Day.ToString())
        strReturn = String.Concat(strReturn, Now.Hour.ToString)
        strReturn = String.Concat(strReturn, Now.Minute.ToString())
        strReturn = String.Concat(strReturn, Now.Second.ToString())
        strReturn = String.Concat(strReturn, Now.Millisecond.ToString())
        Dim Generator As System.Random = New System.Random()
        Dim aleatorio As String
        aleatorio = Generator.Next(100, 999)

        strReturn = String.Concat(strReturn, aleatorio)

        Return strReturn
    End Function
    Public Function ConvertirEntero(ByVal valor As String) As Integer
        Dim IntRetorno As Integer = 0
        Try
            IntRetorno = CInt(valor)
        Catch ex As Exception

        End Try
        Return IntRetorno
    End Function

    Public Structure CodigoTipoCSA
        Public Shared Plan = WebConfigurationManager.AppSettings("CodPlan") ' Desarrollo:CSA0101 , Calidad: CSA_PLN , Produccion: SE_CSA_PLN
        Public Shared Acuerdo = WebConfigurationManager.AppSettings("CodAcuerdo") ' Desarrollo:CSA0102 , Calidad: CSA_ACD , Produccion: SE_CSA_ACD
    End Structure
    Public Structure NombreTipoCSA
        Public Shared Planes = "Plan"
        Public Shared Acuerdos = "Acuerdo"
    End Structure

    Public Structure TipoMensaje
        Public Shared ERR = "ERROR"
        Public Shared SUCCES = "CORRECTO"
    End Structure
#Region "---Imprimir Log---"
    Private Sub LogPropiedadesEmail(ByVal oEmail As beEmail)
        eValidacion.cadenaAleatoria = uConfig.fc_cadenaAleatoria
        With oEmail
            log.Info(String.Concat(eValidacion.cadenaAleatoria, ": ", "CodigoCotizacionSap = ", .CodigoCotizacionSap, _
                                                            ",RazonSocial = " + .RazonSocial, _
                                                            ",NroItem = " + .NroItem))

        End With
    End Sub
    Private Sub LogDestinatariosEmail(correo As System.Net.Mail.MailMessage)
        eValidacion.cadenaAleatoria = uConfig.fc_cadenaAleatoria
        Dim Cadena As String = "DESTINATARIOS : "
        For Each ocorreo In correo.To
            Cadena = String.Concat(Cadena, ", ", ocorreo.Address())
        Next
        Cadena = String.Concat(Cadena, " COPIAS : ")
        For Each ocorreo In correo.CC
            Cadena = String.Concat(Cadena, ", ", ocorreo.Address())
        Next
        Cadena = String.Concat(Cadena, " COPIAS OCULTA : ")
        For Each ocorreo In correo.Bcc
            Cadena = String.Concat(Cadena, ", ", ocorreo.Address())
        Next

        log.Info(String.Concat(eValidacion.cadenaAleatoria, ": ", Cadena))
    End Sub
#End Region
End Class
