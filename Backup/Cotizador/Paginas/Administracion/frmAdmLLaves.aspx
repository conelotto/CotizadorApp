<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmLLaves.aspx.vb" Inherits="Cotizador.frmAdmLLaves" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--JQuery----------------------------------------------------------------------------------%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/ui.multiselect.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/grid.common.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/grid.formedit.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.fmatter.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.tablednd.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.contextmenu.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.alphaNumeric.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/grid.grouping.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.alert.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ""); };
            var valwidth = $(window).width();

            $('#btnAceptar').button();
            $('#btnCerrar').button();

            $("#jqgLLaves").jqGrid({
                datatype: function () {
                    listarLLaves();
                },
                height: 300,
                width: (valwidth * 0.98),
                colNames: ['IdLLaves', 'LLaves'],
                colModel: [
                            { name: 'IdLLave', index: 'IdLLave', hidden: true },
                            { name: 'LLaves', index: 'LLaves', width: 50, sorttype: "string" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerLLaves',
                sortname: 'IdLLaves',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Detalle LLaves"
            });
            $('#t_jqgLLaves').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
            //             <a id='lblEditar'>Editar</a> | 
            $('a', '#t_jqgLLaves').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantLLaves('NUEVO');
                        break;
                    //                     case 'lblEditar':   
                    //                         AbrirMantLLaves('MODIFICAR');   
                    //                         break;   
                    case 'lblELiminar':
                        eliminarLLaves();
                        break;
                }
            });

            $("#diagMantLLaves").dialog({
                autoOpen: false,
                height: 150,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

        });

        function listarLLaves() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgLLaves').getGridParam("sortname");
            sortOrder = jQuery('#jqgLLaves').getGridParam("sortorder");
            pageSize = jQuery('#jqgLLaves').getGridParam("rowNum");
            currentPage = jQuery('#jqgLLaves').getGridParam("page");

            arreglo = fc_redimencionarArray(4);
            arreglo[0][0] = "sortColumn";
            arreglo[0][1] = sortColumn;
            arreglo[1][0] = "sortOrder";
            arreglo[1][1] = sortOrder;
            arreglo[2][0] = "pageSize";
            arreglo[2][1] = pageSize;
            arreglo[3][0] = "currentPage";
            arreglo[3][1] = currentPage;

            parametro = fc_parametrosData(arreglo);
            $.ajax({
                url: location.pathname + "/listarLLaves",
                data: parametro,  // For empty input data use "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgLLaves").clearGridData();
                        var thegrid = jQuery("#jqgLLaves")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function aceptarLLave() {

            var LLave = $.trim($('#txtLLave').val());

            if (LLave == '') {
                alert('No ah ingresado ninguna LLave.');
                return;
            }

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(2);
            arreglo[0][0] = 'LLave';
            arreglo[0][1] = LLave
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#hdfLogin').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarLLaves',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    if (rpta.d == "1") {
                        alert('LLave guardada correctamente.');
                        $('#diagMantLLaves').dialog('close');
                        jQuery("#jqgLLaves").trigger("reloadGrid");
                    }
                    else {
                        alert(rpta.d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function AbrirMantLLaves(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#txtLLave').val('');
                    $('#diagMantLLaves').dialog('open');
                    break;
            }


        }

        function eliminarLLaves() {
            var id = $("#jqgLLaves").jqGrid('getGridParam', 'selrow');
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (confirm('¿Seguro desea eliminar?')) {
                var parametro = null;
                var arreglo = null;

                var fila = $('#jqgLLaves').jqGrid('getRowData', id);
                var idLLave = fila.IdLLave;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdLLave';
                arreglo[0][1] = idLLave;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = $('#hdfLogin').val();
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/eliminarLLaves",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        jQuery("#jqgLLaves").trigger("reloadGrid");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }
            else {
                return;
            }
        }

        function CerrarLLave() {
            $('#diagMantLLaves').dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento de LLaves"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgLLaves">
    </table>
    <div id="diagMantLLaves" title="Registro LLaves">
        <div class="divHeadTitle100">
            Nueva LLave:</div>
        <div class="divContenido100">
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 100;">
                        Ingrese LLave:
                    </td>
                    <td>
                        <input type="text" id="txtLLave" style="width: 150;" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantenimiento" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptar" type="button" value="Aceptar" onclick="aceptarLLave(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="CerrarLLave(); return false;" />
        </div>
    </div>
    <div id="pagerLLaves">
    </div>
</asp:Content>
