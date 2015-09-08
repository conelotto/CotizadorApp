<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmLineas.aspx.vb" Inherits="Cotizador.frmAdmLineas" %>

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

            $("#jqgLineas").jqGrid({
                datatype: function () {
                    listarLineas();
                },
                height: 300,
                width: (valwidth * 0.98),
                colNames: ['IdLineas', 'Codigo', 'Descripción'],
                colModel: [
                            { name: 'IdLinea', index:'IdLinea', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 40, sorttype: "string" },
                            { name: 'Descripcion', index: 'Descripcion', width: 50, sorttype: "string" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerLineas',
                sortname: 'IdLineas',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Detalle Lineas"
            });
            $('#t_jqgLineas').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
            //             <a id='lblEditar'>Editar</a> | 
            $('a', '#t_jqgLineas').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantLineas('NUEVO');
                        break;
                    //                     case 'lblEditar':  
                    //                         AbrirMantLineas('MODIFICAR');  
                    //                         break;  
                    case 'lblELiminar':
                        eliminarLineas();
                        break;
                }
            });

            $("#diagMantLineas").dialog({
                autoOpen: false,
                height: 150,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

        });

        function listarLineas() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgLineas').getGridParam("sortname");
            sortOrder = jQuery('#jqgLineas').getGridParam("sortorder");
            pageSize = jQuery('#jqgLineas').getGridParam("rowNum");
            currentPage = jQuery('#jqgLineas').getGridParam("page");

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
                url: location.pathname + "/listarLineas",
                data: parametro,  // For empty input data use "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgLineas").clearGridData();
                        var thegrid = jQuery("#jqgLineas")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function aceptarLinea() {

            var Codigo = $.trim($('#txtCodigo').val());
            var Descripcion = $.trim($('#txtDescripcion').val());

            if (Codigo == '') {
                alert('Complete el campo Código.');
                return;
            }
            if (Descripcion == '') {
                alert('Complete el campo Descripción.');
                return;
            }

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(3);
            arreglo[0][0] = 'Codigo';
            arreglo[0][1] = Codigo
            arreglo[1][0] = 'Descripcion';
            arreglo[1][1] = Descripcion
            arreglo[2][0] = 'usuario';
            arreglo[2][1] = $('#hdfLogin').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarLineas',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    if (rpta.d == "1") {
                        alert('Linea guardada correctamente.');
                        $('#diagMantLineas').dialog('close');
                        jQuery("#jqgLineas").trigger("reloadGrid");
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

        function AbrirMantLineas(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#txtCodigo').val('');
                    $('#txtDescripcion').val('');
                    $('#diagMantLineas').dialog('open');
                    break;
            }


        }

        function eliminarLineas() {
            var id = $("#jqgLineas").jqGrid('getGridParam', 'selrow');
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (confirm('¿Seguro desea eliminar?')) {
                var parametro = null;
                var arreglo = null;

                var fila = $('#jqgLineas').jqGrid('getRowData', id);
                var idLinea = fila.IdLinea;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdLinea';
                arreglo[0][1] = idLinea;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = $('#hdfLogin').val();
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/eliminarLineas",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        jQuery("#jqgLineas").trigger("reloadGrid");
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

        function CerrarLinea() {
            $('#diagMantLineas').dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento de Lineas"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgLineas">
    </table>
    <div id="diagMantLineas" title="Registro Lineas">
        <div class="divHeadTitle100">
            Nueva Linea:</div>
        <div class="divContenido100">
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 100;">
                        Código Linea:
                    </td>
                    <td>
                        <input type="text" id="txtCodigo" style="width: 150;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100;">
                        Descripción Linea:
                    </td>
                    <td>
                        <input type="text" id="txtDescripcion" style="width: 150;" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantenimiento" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptar" type="button" value="Aceptar" onclick="aceptarLinea(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="CerrarLinea(); return false;" />
        </div>
    </div>
    <div id="pagerLineas">
    </div>
</asp:Content>
