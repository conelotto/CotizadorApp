Imports System
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Configuration
Imports Ferreyros.Utiles

Public Class SapSite
    Inherits System.Web.UI.MasterPage
    Private oUtiles As Ferreyros.Utiles.uConfiguracion = Nothing

    Private Sub Iniciar()
        oUtiles = New uConfiguracion
        Dim mensaje As String = String.Empty
        oUtiles.extraerAppSettings("MensajeDerechos", mensaje)
        idDerechos.Text = mensaje
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If Not Page.IsPostBack Then
            Iniciar()
        End If
    End Sub

End Class