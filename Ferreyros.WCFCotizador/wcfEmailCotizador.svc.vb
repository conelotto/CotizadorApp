Imports Ferreyros.ClasServicioCotizador
Imports System.ServiceModel
Imports log4net
Imports Ferreyros.Utiles

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "wcfEmailCotizador" en el código, en svc y en el archivo de configuración a la vez.
Public Class wcfEmailCotizador
    Implements IwcfEmailCotizador
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(wcfEmailCotizador))
    Private uConfig As uConfiguracion = Nothing
    Private Validacion As beValidacion = Nothing
    Public Sub New()
        log4net.Config.XmlConfigurator.Configure()
    End Sub
    Public Function EnviarEmail(ByVal beEmail As beEmail, ByRef ErrorDescripcion As String) As Boolean Implements IwcfEmailCotizador.EnviarEmail
        Dim bol_Return As Boolean = False
        Dim oUtilitario As New clsUtilitarios
        Validacion = New beValidacion
        uConfig = New uConfiguracion

        Try
            Validacion.cadenaAleatoria = uConfig.fc_cadenaAleatoria
            log.Info(Validacion.cadenaAleatoria + ": Inicio Envio de correo---------------------------------")
            bol_Return = oUtilitario.EnvioEmailCotizador(beEmail)
            log.Info(Validacion.cadenaAleatoria + ": Final Envio de correo-----------------------------------")
        Catch ex As Exception

        End Try

        Return bol_Return
    End Function

End Class
