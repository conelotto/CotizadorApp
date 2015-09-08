Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Text
Imports Ferreyros.ClasServicioCotizador
Imports System.Configuration
Imports log4net
Imports Ferreyros.Utiles
Imports System.Web.Services.WebService


Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

' NOTE: You can use the "Rename" command on the context menu to change the class name "wcfCotizador" in code, svc and config file together.
Public Class wcfCotizador
    Implements IwcfCotizador

    Private strConexionSQL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConexionCotizador")
    Private strConexionAS400 As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConexionAS400")
    Private strUrlResult As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlResult")
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(wcfCotizador))
    Private uConfig As uConfiguracion = Nothing
    Private nCotizacion As bcCotizacion = Nothing
    Private Validacion As beValidacion = Nothing


    Public Sub New()
        log4net.Config.XmlConfigurator.Configure()
    End Sub

    Public Function InsertarCotizacion(ByVal Cotizacion As beCotizacion, ByRef UrlResult As String, ByRef ErrorDescripcion As String) As Boolean Implements IwcfCotizador.InsertarCotizacion

        Dim lResult As Boolean = False
        Dim oUtilitario As New clsUtilitarios
        nCotizacion = New bcCotizacion
        Validacion = New beValidacion
        uConfig = New uConfiguracion


        nCotizacion.UrlServicio = Hosting.HostingEnvironment.ApplicationPhysicalPath

        Try
            Validacion.cadenaAleatoria = uConfig.fc_cadenaAleatoria
            log.Info(Validacion.cadenaAleatoria + ": Inicio Envio Cotizacion----------------------------------")
            lResult = nCotizacion.InsertarCotizacion(strConexionSQL, strConexionAS400, Cotizacion, Validacion)
            If lResult Then
                Dim valRand As String = GenerarValorRandom()
                UrlResult = strUrlResult + "?IdCotizacionSap=" + Cotizacion.IdCotizacion + "&Usuario=" + Cotizacion.Usuario
                UrlResult = String.Concat(UrlResult, String.Concat("&rdm=", valRand.ToString()))
                log.Info(Validacion.cadenaAleatoria + ": URL " + UrlResult)
            End If

            ErrorDescripcion = nCotizacion.ErrorDescripcion
        Catch ex As Exception
            ErrorDescripcion = ex.Message.ToString
            log.Error(Validacion.cadenaAleatoria + ": " + ErrorDescripcion)
        Finally
            log.Info(Validacion.cadenaAleatoria + ": Final Envio Cotizacion --------------------------------------")
        End Try

        Return lResult

    End Function
   
    Private Function GenerarRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Private Function GenerarValorRandom() As String
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



End Class
