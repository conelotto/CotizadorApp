<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCotizacionAprobacion.aspx.vb" Inherits="Cotizador.frmCotizacionAprobacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

  <%--JQuery----------------------------------------------------------------------------------%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/grid.locale-en.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/ui.multiselect.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.tablednd.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.contextmenu.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.alphaNumeric.js" type="text/javascript"></script>

    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID ="pnlToolBarOpciones" 
     SkinID ="ToolbarBordeRedondeado"  >
    </asp:RoundedCornersExtender>
     <asp:Panel id="pnlToolBarOpciones" runat ="server" >
        <table width="100%" cellpadding="0" border="0" cellspacing="0" class="ToolbarTitulo">
        <tr valign="middle">
            <td align="left">
                <asp:Label runat="server" ID="lblTitulo" Text="Aprobación de Cotización" CssClass="TextoTitulo"></asp:Label>
            </td>
            <td align="right">
                <div id="submenu">
                    <p>
                    <asp:LinkButton ID="lbnConsultar" runat="server" Text="Consultar" /> | 
                    <asp:LinkButton ID="lbnLimpiar" runat="server" Text="Limpiar" EnableViewState="true"
                        OnClientClick="javascript:Limpiar(); return false;"/> |&nbsp;
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
                                        <asp:label CssClass="Etiqueta" ID="lblCotizacion" runat="server" Text="N° Cotización:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TextoControl" ID="txtCotizacion" Width="120"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:label ID="lblCliente" runat="server" CssClass="Etiqueta" Text="Cliente:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCliente" Width="300" CssClass="TextoControl"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:label ID="lblModelo" runat="server" CssClass="Etiqueta" Text="Modelo:"></asp:label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtModelo" Width="150" CssClass="TextoControl"></asp:TextBox>
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
                                AllowPaging="True" AutoGenerateColumns="False" SkinID="dgvDirectorio">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-BorderStyle="None" HeaderStyle-BorderStyle="None">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbOculto" runat="server" CommandArgument="<%# 0 %>" 
                                                CommandName="Oculto" Width="0px" />
                                            <asp:ImageButton ID="imbEvaluarCotizacion" runat="server" CommandName="Evaluar" AlternateText="Evaluar Cotización"
                                                ImageUrl="~/Images/calDerecha.gif" TabIndex="1" />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle BorderStyle="None" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-BorderStyle="None" HeaderStyle-BorderStyle="None">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbAprobar" runat="server" CommandName="Select" AlternateText="Aprobar Cotización" 
                                                ImageUrl="~/Images/AdjuntoUpdate.gif" OnClientClick="javascript:return confirm('Seguro de aprobar la cotización');" />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle BorderStyle="None" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-BorderStyle="None" HeaderStyle-BorderStyle="None">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbEvaluar" runat="server" CommandName="Evaluar" AlternateText="Evaluar Cotización"
                                                ImageUrl="~/Images/ActionUpdate.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle BorderStyle="None" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-BorderStyle="None" HeaderStyle-BorderStyle="None">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbRechazar" runat="server" CommandName="Delete" AlternateText="Rechazar Cotización" 
                                                ImageUrl="~/Images/AdjuntoDelete.gif" OnClientClick="javascript:return confirm('Seguro de rechazar la cotización');"  />
                                        </ItemTemplate>
                                        <HeaderStyle BorderStyle="None" CssClass="GridHeader" />
                                        <ItemStyle BorderStyle="None" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cotización" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lbnCotizacion">
                                                <%# Container.DataItem("IdCotizacionSAP")%></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="120px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Fecha Validez" DataField="FechaFinalValidez" 
                                        ItemStyle-Width="90px" DataFormatString="{0:d}" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Cliente" DataField="DescripSolicitante" 
                                        ItemStyle-Width="150px">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Aprobador" DataField="Aprobador" 
                                        ItemStyle-Width="100px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Modelo" DataField="IdProductoSAP" 
                                        ItemStyle-Width="90px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" 
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total Lista" DataField="ValorNeto" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="90px" >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Descuento" DataField="ValorImpuesto" 
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" 
                                        ItemStyle-Width="70px"  >
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="No se encontraron registros" CssClass="Texto"></asp:Label>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <%--Cuadro de Dialogo--%>
   <%-- <input id="btnAbrir" type="button" value="Probar..." />--%>
    <div id="idDialogForm" title="Evaluación de Aprobación">    
    </div>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $("#idDialogForm").dialog({
                autoOpen: false,
                height: 600,
                width: 650,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            $('#btnAbrir').click(function () {
//                $("#somediv").load(url).dialog({ modal: true }); 

             });
            

        });
          

        function Limpiar() {
            document.getElementById('MainContent_txtCotizacion').value = '';
            document.getElementById('MainContent_txtModelo').value = '';
            document.getElementById('MainContent_txtCliente').value = '';
            document.getElementById('MainContent_lblItems').innerText = '';
            if (document.getElementById('MainContent_gdvCotizacion') != null) document.getElementById('MainContent_gdvCotizacion').style.display = 'none';
        }
        
        function MostarEvaluacion(){
            $("#idDialogForm").dialog("open");
        }

        
    </script> 
</asp:Content>
