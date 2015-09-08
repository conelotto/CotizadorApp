Imports System
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Configuration
Imports Ferreyros.Utiles

Public Class Site
    Inherits System.Web.UI.MasterPage

    Private miContexto As Ferreyros.Base.Interfase.MiUsuario
    Public clsMain As New main
    Public pu_strFecha As String = ""
    Public pu_strMoneda As String = ""
    Public pu_dblCompra As Double = 0
    Public pu_dblVenta As Double = 0
    ' OBJETO DE MENU
    Private objMenu As Ferreyros.Base.Negocio.Menu
    Private objComp As Ferreyros.Base.Negocio.Compania
    Private oUtiles As Ferreyros.Utiles.uConfiguracion = Nothing

    Private Sub Iniciar()
        oUtiles = New uConfiguracion
        Dim mensaje As String = String.Empty
        oUtiles.extraerAppSettings("MensajeDerechos", mensaje)
        idDerechos.Text = mensaje

        If miContexto IsNot Nothing Then

            If Session("Menu_Site") = "OK" Then
                Return
            End If
            ' USUARIO AUTENTICADO
            Me.lnkUsuario.Text = miContexto.NombreCompleto.ToString.ToString.ToUpper
            Me.hdfLogin.Value = AdminSeguridad.Encriptar(miContexto.login)
            ' TIPO DE CAMBIO
            pu_strFecha = miContexto.TCambio_Fecha
            pu_strMoneda = miContexto.TCambio_Moneda
            pu_dblCompra = miContexto.TCambio_Compra
            pu_dblVenta = miContexto.TCambio_Venta
            ' CARGAR MENU
            Dim colMenu = CType(Session("Menu_Master"), MenuItemCollection)
            For Each Item In colMenu
                Menu.Items.Add(Item)
            Next
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        'OBTENER CONTEXTO DEL CACHE
        If Not Session("MiUsuario") Is Nothing Then
            miContexto = CType(Cache(Session("MiUsuario")), Ferreyros.Base.Interfase.MiUsuario)
        Else
            Response.Redirect("~/Default.aspx", False)
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If Not Page.IsPostBack Then
            Iniciar()
        End If

    End Sub

    Protected Sub lnkUsuario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUsuario.Click

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        Response.Redirect("~/Default.aspx?txhInicio=new", False)

    End Sub

End Class