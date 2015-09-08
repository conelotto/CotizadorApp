Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Public Class frmCotizacionAprobacion
    Inherits System.Web.UI.Page
    Private obcCotizacion As bcCotizacion
#Region "-- Eventos --"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("Lista") = Nothing
            CargaInicial()
            If Not String.IsNullOrEmpty(Request("Numero")) And ViewState("IdAprobador") IsNot Nothing Then
                txtCotizacion.Text = Convert.ToString(Request("Numero"))
                Buscar()
            End If
        End If

        If Session("CerrarAprobacion") IsNot Nothing Then
            If Convert.ToBoolean(Session("CerrarAprobacion")) Then
                ClosePaginaServer(Page)
            End If
        End If

        If Session("Actualizar") IsNot Nothing Then
            If Convert.ToBoolean(Session("Actualizar")) Then
                Buscar()
                Session("Actualizar") = Nothing
            End If
        End If
    End Sub

    Protected Sub lbnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnConsultar.Click
        Buscar()
    End Sub

    Private Sub gdvCotizacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvCotizacion.PageIndexChanging
        If ViewState("Lista") IsNot Nothing Then
            Dim dtbConsulta As New DataTable
            dtbConsulta = ViewState("Lista")
            gdvCotizacion.PageIndex = e.NewPageIndex
            EnlazarGrid(gdvCotizacion, dtbConsulta, False, True)
        End If
    End Sub
#End Region
#Region "-- Metodos ---"
    Private Sub CargaInicial()

    End Sub

    Private Sub Buscar()
        Dim strNumeroCotizacion As String = String.Empty
        Dim IdVendedor As String = String.Empty
        Dim CodigoModelo As String = String.Empty
        Dim NombreCliente As String = String.Empty
        Dim IdEstadoCotizacion As String = String.Empty
        'Dim beModelo As New beModelo
        Dim beCotizacion As New beCotizacion
        Dim dtbConsulta As New DataTable
        obcCotizacion = New bcCotizacion


        Try

            If Not txtCotizacion.Text.Trim() = String.Empty Then
                strNumeroCotizacion = txtCotizacion.Text.Trim()
            End If
            If Not txtCliente.Text.Trim() = String.Empty Then
                NombreCliente = txtCliente.Text.Trim
            End If
            If Not txtModelo.Text.Trim() = String.Empty Then
                CodigoModelo = txtModelo.Text.Trim()
            End If

            beCotizacion.IdCotizacionSap = strNumeroCotizacion
            beCotizacion.DescripSolicitante = NombreCliente

            If obcCotizacion.BusquedaEnAprobacion(Modulo.strConexionSql, beCotizacion, dtbConsulta) Then
                ViewState("Lista") = dtbConsulta

                lblItems.Text = String.Format("N° Items: {0}", dtbConsulta.Rows.Count)
                EnlazarGrid(gdvCotizacion, dtbConsulta, True, True)
            Else
                AlertPaginaServer(Page, obcCotizacion.ErrorDes)
            End If
        Catch ex As Exception
            AlertPaginaServer(Page, ex.Message.ToString())
        End Try
    End Sub
    
#End Region

End Class