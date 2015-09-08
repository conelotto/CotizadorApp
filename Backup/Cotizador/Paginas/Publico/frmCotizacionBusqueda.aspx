<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCotizacionBusqueda.aspx.vb" Inherits="Cotizador.frmCotizacionBusqueda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"> 
    <%--<asp:LinkButton ID="lbnNuevo" runat="server" Text="Nuevo | " EnableViewState="true"
                        OnClientClick="javascript:Nuevo();; return false;"/>--%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    
     
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:BoundField HeaderText="Modelo" DataField="CodigoModelo" ItemStyle-Width="90px" />--%>
    <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID ="pnlToolBarOpciones" 
     SkinID ="ToolbarBordeRedondeado"  >
    </asp:RoundedCornersExtender>
    <asp:Panel id="pnlToolBarOpciones" runat ="server" >
    <table width="100%" cellpadding="0" border="0" cellspacing="0" class="ToolbarTitulo">
        <tr valign="middle" >
            <td align="left">
                <asp:Label runat="server" ID="lblTitulo"  Text="Búsqueda de Cotización" CssClass="TextoTitulo"></asp:Label>                
            </td>
            <td align="right">
                <div id="submenu">
                    <p>
                    <asp:LinkButton ID="lbnConsultar" runat="server" Text="Consultar | " />
                    <%--<asp:LinkButton ID="lbnNuevo" runat="server" Text="Nuevo | " EnableViewState="true"
                        OnClientClick="javascript:Nuevo();; return false;"/>--%>
                    <asp:LinkButton ID="lbnLimpiar" runat="server" Text="Limpiar | "
                        OnClientClick="javascript:Limpiar(); return false;"/>
                    <asp:LinkButton ID="lbnExportar" runat="server" Text="Exportar |" 
                        OnClientClick="javascript:return ValidarLista();" />&nbsp;
                    </p>
                </div>                
            </td>            
        </tr>
     </table>
     </asp:Panel>
    
    <asp:UpdatePanel runat="server" ID="pnlDetalle">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;height:100%;">
                <tr>
                    <td>
                        <fieldset>
                            <legend class="EtiquetaRequerida">Datos de consulta</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:label CssClass="Texto" ID="lblCotizacion" runat="server" Text="N° Cotización:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TextoControl" ID="txtNumeroCotizacion" 
                                            Width="150"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:label ID="lblCliente" runat="server" CssClass="Texto" Text="Cliente:"></asp:label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox runat="server" ID="txtCliente" Width="380" CssClass="TextoControl"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:label ID="lblEstado" CssClass="Texto" runat="server" Text="Estado:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstado" Width="155px" CssClass="TextoCombo"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:label ID="lblModelo" runat="server" CssClass="Texto" Text="Modelo:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtModelo" Width="150" CssClass="TextoControl"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:label ID="lblVendedor" runat="server" CssClass="Texto" Text="Vendedor:"></asp:label>
                                        
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtVendedor" Width="150" CssClass="TextoControl"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" Text="Verificar Servicio Prime" 
                                            Visible="False" />
                                        <asp:Button ID="btnModeloEspecificacion" runat="server" 
                                            Text="Modelo Especificacion" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend class="EtiquetaRequerida">
                                <asp:Label runat="server" ID="lblItems" CssClass="Campo"></asp:Label>
                            </legend>
                            <asp:GridView runat="server" ID="gdvCotizacion" DataKeyNames="IdCotizacion" 
                                SkinID="dgvDirectorio" AutoGenerateColumns="False" Width="1200px">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbOculto" runat="server" CommandName="Oculto" 
                                                CommandArgument='<%# 0 %>' Width="0px" />
                                            <asp:ImageButton ID="imbEditar" runat="server" 
                                                AlternateText="Modificar Cotización" 
                                                CommandArgument='<%# Container.DataItem("idCotizacion") %>' CommandName="Editar" 
                                                ImageUrl="~/Images/ActionUpdate.gif" TabIndex="1" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbCopiar" runat="server" CommandName="Copiar" AlternateText="Copiar Nueva Cotización" 
                                                ImageUrl="~/Images/ActionCopy.gif" 
                                                CommandArgument='<%# Container.DataItem("idCotizacion") %>' 
                                                Visible="False"/>
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbEliminar" runat="server" CommandName="Eliminar" AlternateText="Eliminar Cotización" 
                                                CommandArgument='<%# Container.DataItem("idCotizacion") %>' ImageUrl="~/Images/AdjuntoDelete.gif"
                                                
                                                
                                                OnClientClick="javascript:return confirm('Seguro de eliminar la cotización');" 
                                                Visible="False"  />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" >
                                        <ItemTemplate>
                                            <asp:Image runat="server" id="imgVisualizar" 
                                                AlternateText="Visualizar Cotización" style="cursor:hand;" 
                                                ImageAlign="AbsMiddle" ToolTip="Visualizar Cotización" 
                                                ImageUrl="~/Images/PrintPDF.gif" Visible="False"/>                                        
                                            <asp:ImageButton ID="imbImprimir" runat="server" 
                                                AlternateText="Imprimir Cotización" 
                                                CommandArgument='<%# Container.DataItem("IdCotizacion") %>' 
                                                CommandName="Imprimir" ImageUrl="~/Images/PrintPDF.gif" TabIndex="1" 
                                                Visible="False" />
                                            <asp:ImageButton ID="imbImprimirCotizacion" runat="server" 
                                                AlternateText="Imprimir" ImageUrl="~/Images/PrintPDF.gif" TabIndex="1" />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cotización" ItemStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdCotizacion" runat="server" 
                                                Text="<%# bind('IdCotizacion') %>" Visible="False"></asp:Label>
                                            <asp:LinkButton runat="server" ID="lbnCotizacion" 
                                                CommandArgument='<%# Container.DataItem("IdCotizacionSAP") %>' CommandName="Ver">
                                                <%# Container.DataItem("IdCotizacionSAP")%></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="110px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Fecha" DataField="FechaFinalValidez" 
                                        ItemStyle-Width="80px" DataFormatString="{0:d}" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Estado" DataField="Estado" 
                                        ItemStyle-Width="100px">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Solicitante" DataField="DescripSolicitante" 
                                        ItemStyle-Width="200px">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Contacto" DataField="Contacto" 
                                        ItemStyle-Width="100px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Cargo" DataField="Cargo" ItemStyle-Width="100px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField HeaderText="Modelo" DataField="CodigoModelo" ItemStyle-Width="90px" />--%>
                                    <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" 
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="55px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="55px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Valor Lista" DataField="ValorBruto" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="80px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Descuento" DataField="ValorDescuento" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="80px"  >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="IGV" DataField="ValorImpuesto" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="80px"  >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Valor Venta" DataField="ValorNeto" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="80px"  >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="No se encontraron registros" CssClass="Texto"></asp:Label>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <asp:HiddenField runat="server" Value="" ID="hidLimpiar" />
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
    <div id="dialog" style ="width:400px">
  

    </div>
<script type ="text/javascript" language ="javascript" >

    function Limpiar() {
        document.getElementById('MainContent_txtNumeroCotizacion').value = ''
        document.getElementById('MainContent_txtCliente').value = ''
        document.getElementById('MainContent_ddlEstado').selectedIndex = 0
        document.getElementById('MainContent_txtModelo').value = ''
        document.getElementById('MainContent_txtVendedor').value = ''
        document.getElementById('MainContent_lblItems').innerText = ''  
        if (document.getElementById('MainContent_gdvCotizacion') != null) document.getElementById('MainContent_gdvCotizacion').style.display = 'none';
    }
   
    function ValidarLista() {
        OcultarModal();
        var valido = true;
        var obj= document.getElementById('MainContent_hidLimpiar');
        if (obj != null)
            valido = obj.value == '1';

        return valido;
    }

    function RedirigirImprimir(url) {
        //        var pagina = url 'CO001D01.aspx?Id=' + Id + '&Op=' + Op;
        var pagina = url
        window.showModalDialog(pagina, window, 'dialogHeight:500px;dialogWidth:500px;status:0;center:1;scroll:0;help:0;');
    }

    function showPanel(panelID,Id) {
        $panel = $('#' + panelID);
        var body =""

        $panel.html(body);

        //$panel.dialog();

        $panel.dialog({
                autoOpen:true,
                modal: false,
                width:630,
                height:500
        });
    }

</script>
</asp:Content>

