Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros
Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Imports System.IO

Public Class frmCotizacionBusqueda
    Inherits System.Web.UI.Page

#Region "---------- Declaracion ----------"

    Private miContexto As Base.Interfase.MiUsuario
    Private obcCotizacion As bcCotizacion

#End Region

#Region "------------ Eventos ------------"
    Private Sub frmCotizacionBusqueda_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Dim AccesoValido As Boolean
        'If Session("MiUsuario") IsNot Nothing Then
        '    miContexto = CType(Cache(Session("MiUsuario")), Base.Interfase.MiUsuario)
        '    AccesoValido = miContexto IsNot Nothing
        'End If

        'If Not AccesoValido Then
        '    Response.Redirect("~/Default.aspx", True)
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargaInicial()
            ViewState("Lista") = Nothing
        End If
    End Sub

    Protected Sub lbnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnConsultar.Click
        Call BuscarCotizacion()
    End Sub

    Protected Sub lbnLimpiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnLimpiar.Click

    End Sub

    Protected Sub lbnExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbnExportar.Click
        'If ViewState("Lista") IsNot Nothing AndAlso hidLimpiar.Value.Equals(Estado.Habilitado) Then
        If ViewState("Lista") IsNot Nothing Then
            Dim dtbConsulta As New DataTable
            Dim dtsConsulta As New DataSet

            dtbConsulta = ViewState("Lista")
            hidLimpiar.Value = Estado.Inahibilitado

            Modulo.RemoverColumna(dtbConsulta, "IdCotizacion")
            Modulo.RenombrarColumna(dtbConsulta, "IdCotizacionSAP", "Número")
            Modulo.RenombrarColumna(dtbConsulta, "FechaRegistro", "Fecha Fin Validez")
            Modulo.RenombrarColumna(dtbConsulta, "ValorBruto", "Total Valor Lista")
            Modulo.RenombrarColumna(dtbConsulta, "ValorDescuento", "Descuento")
            Modulo.RenombrarColumna(dtbConsulta, "ValorImpuesto", "IGV")
            Modulo.RenombrarColumna(dtbConsulta, "ValorNeto", "Valor Venta")

            dtsConsulta.Tables.Add(dtbConsulta)

            'Modulo.AddScriptPagina(Page, "OcultarModal", sb.ToString())
            Modulo.Exportar(Response, dtsConsulta, "Cotizacion")
            'ExportarArchivo()
        End If
    End Sub

    'Private Sub ExportarArchivo()

    '    Dim sb As StringBuilder = New StringBuilder()
    '    Dim sw As StringWriter = New StringWriter(sb)
    '    Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
    '    Dim pagina As Page = New Page
    '    Dim form = New HtmlForm
    '    form.Target = "_blank"
    '    pagina.ClientTarget = "_blank"
    '    Me.gdvCotizacion.EnableViewState = False
    '    pagina.EnableEventValidation = False
    '    pagina.DesignerInitialize()
    '    pagina.Controls.Add(form)
    '    form.Controls.Add(gdvCotizacion)
    '    pagina.RenderControl(htw)

    '    pagina.Response.Clear()
    '    pagina.Response.Buffer = True
    '    pagina.Response.ContentType = "application/vnd.ms-excel"
    '    pagina.Response.AddHeader("Content-Disposition", "attachment;filename=data.xls")
    '    pagina.Response.Charset = "UTF-8"

    '    pagina.Response.ContentEncoding = Encoding.Default
    '    pagina.Response.Write(sb.ToString())
    '    pagina.Response.End()



    'End Sub
    Private Sub gdvCotizacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvCotizacion.PageIndexChanging
        If ViewState("Lista") IsNot Nothing Then
            Dim dtbConsulta As New DataTable
            dtbConsulta = ViewState("Lista")
            gdvCotizacion.PageIndex = e.NewPageIndex
            EnlazarGrid(gdvCotizacion, dtbConsulta, False, True)
        End If
    End Sub

    Protected Sub gdvCotizacion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvCotizacion.RowCommand

        Select Case e.CommandName.ToString.Trim.ToUpper()
            Case "VER"
                Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).Parent.Parent, GridViewRow)
                Dim lblIdCotizacion As Label
                '       GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                'int index = row.RowIndex;

                Dim index As Integer = gvRow.RowIndex
                lblIdCotizacion = CType(gvRow.FindControl("lblIdCotizacion"), Label)

                Dim lbnCotizacion As LinkButton
                lbnCotizacion = CType(gvRow.FindControl("lbnCotizacion"), LinkButton)
                'Response.Redirect("frmCotizacionRegistroCsa.aspx?IdCotizacionSap=" + lbnCotizacion.CommandArgument.Trim() + "&Accion=V")
                Response.Redirect("frmCotizacionRegistro.aspx?IdCotizacionSap=" + lbnCotizacion.CommandArgument.Trim() + "&Usuario=RSARMIENTO")
            Case "EDITAR"
                Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).Parent.Parent, GridViewRow)
                Dim lblIdCotizacion As Label

                Dim index As Integer = gvRow.RowIndex
                lblIdCotizacion = CType(gvRow.FindControl("lblIdCotizacion"), Label)
                'Response.Redirect("frmCotizacionRegistro.aspx?Id=" + lblIdCotizacion.Text.Trim() + "&Accion=M")
                Dim lbnCotizacion As LinkButton
                lbnCotizacion = CType(gvRow.FindControl("lbnCotizacion"), LinkButton)
                'Response.Redirect("frmCotizacionRegistroCsa.aspx?IdCotizacionSap=" + lbnCotizacion.CommandArgument.Trim() + "&Accion=M")
                Response.Redirect("frmCotizacionRegistro.aspx?IdCotizacionSap=" + lbnCotizacion.CommandArgument.Trim() + "&Usuario=RSARMIENTO")
            Case "IMPRIMIR"
                Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).Parent.Parent, GridViewRow)
                Dim imbImprimir As ImageButton
                imbImprimir = CType(gvRow.FindControl("imbImprimir"), ImageButton)
                'imbImprimir.OnClientClick = "javascript:showPanel('dialog');return false;"
                imbImprimir.Attributes.Add("onclick", "javascript:showPanel('dialog');return false;")

        End Select
    End Sub
#End Region

#Region "------------ Metodos ------------"
    Private Sub CargaInicial()

        Dim dtbEstadoCotizacion As New DataTable
        dtbEstadoCotizacion = Modulo.ObtenerTablaGrupo(EntidadTablaMaestra.EstadoCotizacion.ToString())
        If dtbEstadoCotizacion.Rows.Count > 0 Then
            EnlazarCombo(ddlEstado, dtbEstadoCotizacion, "Codigo", "Nombre", True)
        End If

    End Sub
    Private Sub BuscarCotizacion()
        Dim strNumeroCotizacion As String = String.Empty
        Dim IdVendedor As String = String.Empty
        Dim CodigoModelo As String = String.Empty
        Dim IdCliente As String = String.Empty
        Dim IdEstadoCotizacion As String = String.Empty

        Dim beCotizacion As New beCotizacion
        Dim dtbConsulta As New DataTable
        obcCotizacion = New bcCotizacion

        Try

            If Not txtNumeroCotizacion.Text.Trim() = String.Empty Then
                strNumeroCotizacion = txtNumeroCotizacion.Text.Trim()
            End If
            If Not txtCliente.Text.Trim() = String.Empty Then
                IdCliente = CInt(txtCliente.Text.Trim)
            End If
            If Not ddlEstado.SelectedValue.Trim() = String.Empty Then
                IdEstadoCotizacion = ddlEstado.SelectedValue
            End If
            If Not txtModelo.Text.Trim() = String.Empty Then
                CodigoModelo = txtModelo.Text.Trim()
            End If
            If Not txtVendedor.Text.Trim() = String.Empty Then
                IdVendedor = txtVendedor.Text.Trim()
            End If


            beCotizacion.IdCotizacionSap = strNumeroCotizacion
            beCotizacion.IdSolicitante = IdCliente
            beCotizacion.NombreEstado = IdEstadoCotizacion
            beCotizacion.IdPersonaResponsable = IdVendedor

            If obcCotizacion.Busqueda(Modulo.strConexionSql, beCotizacion, dtbConsulta) Then
                ViewState("Lista") = dtbConsulta
                hidLimpiar.Value = Estado.Habilitado
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

    Private Sub gdvCotizacion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCotizacion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imbImprimirCotizacion As ImageButton
            Dim drvFila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim IdCotizacion As String
            IdCotizacion = drvFila("IdCotizacion").ToString()

            imbImprimirCotizacion = CType(e.Row.FindControl("imbImprimirCotizacion"), ImageButton)

            imbImprimirCotizacion.OnClientClick = "showPanel('dialog','" + IdCotizacion + "');return false;"
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ''http://localhost/WCFCotizador/wcfCotizador.svc?wsdl 

        'Dim obcCotizacion As New ServicioWCFCotizador.IwcfCotizadorClient

        ''Cotizacion
        'Dim obeCotizacion As New ServicioWCFCotizador.beCotizacion
        'Dim obeCotizacionContacto As New ServicioWCFCotizador.beCotizacionContacto
        'Dim obeTelefonoContacto As New ServicioWCFCotizador.beTelefonoContacto
        ''Producto
        'Dim obeProductoPrime As New ServicioWCFCotizador.beProductoPrime
        'Dim obeProductoAdicional As New ServicioWCFCotizador.beProductoAdicional
        'Dim obeDetalleRecompra As New ServicioWCFCotizador.beDetalleRecompra
        'obeCotizacion._FechaRegistro = "01102012"
        'obeCotizacion._FechaVigencia = "31102012"
        'obeCotizacion._IdCotizacion = "1"
        ''Cotizacion
        'obeCotizacionContacto._ListabeTelefonoContacto.Aggregate( 

        'obeCotizacionContacto._ListabeTelefonoContacto.Add(obeTelefonoContacto)
        'obeCotizacion._ListabeCotizacionContacto.Add(obeCotizacionContacto)

        ''Producto 
        'obeProductoPrime._ListabeProductoAdicional.(obeProductoAdicional)
        'obeProductoPrime._ListabeProductoAdicional.Add(obeProductoAdicional)
        'obeProductoPrime._ListabeDetalleRecompra.Add(obeDetalleRecompra)

        'obeCotizacion.ListabeProductoPrime.Add(obeProductoPrime)

        'obcCotizacion.InsertarPrime(Modulo.strConexionSql, obeCotizacion)
    End Sub

    Protected Sub btnModeloEspecificacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModeloEspecificacion.Click
        Response.Redirect("../Administracion/frmAdmModeloEspecificacion.aspx")
    End Sub
End Class