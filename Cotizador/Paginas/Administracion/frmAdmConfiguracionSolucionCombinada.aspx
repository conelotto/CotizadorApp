<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmConfiguracionSolucionCombinada.aspx.vb" Inherits="Cotizador.frmAdmConfiguracionSolucionCombinada" %>

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

            $("#jqgConfiguracion").jqGrid({
                datatype: function () {
                    listarConfiguracion();
                },
                height: 300,
                width: (valwidth * 0.98),
                colNames: ['IdLineas', 'Lineas', 'IdLLaves', 'LLaves', 'Orden'],
                colModel: [
                            { name: 'IdLineas', index: 'IdLineas', hidden: true },
                            { name: 'Lineas', index: 'Lineas', width: 50, sorttype: "string", align: "center" },
                            { name: 'IdLLaves', index: 'IdLLaves', hidden: true },
                            { name: 'LLaves', index: 'LLaves', width: 150, sorttype: "string" },
                            { name: 'Orden', index: 'Orden', hidden: true }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerConfiguracion',
                sortname: 'IdLineas',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Detalle Configuración de Lineas - LLaves"
            });
            $('#t_jqgConfiguracion').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
            //             | <a id='lblEditar'>Agregar Linea</a> | <a id='lblEditar'>Eliminar Linea</a> | <a id='lblEditar'>Agregar LLave</a> | <a id='lblEditar'>Eliminar LLave</a>

            /* ----------------------------------------------------------------------------------------------------------- */
            //            jQuery("#jqgConfiguracion").jqGrid('navGrid', '#pagerjqgConfiguracion', { search: false, refresh: false, edit: false, add: false, del: false });
            /* ----------------------------------------------------------------------------------------------------------- */

            $('a', '#t_jqgConfiguracion').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantenimiento('NUEVO');
                        break;
                    case 'lblEditar':
                        AbrirMantenimiento('MODIFICAR');
                        break;
                    case 'lblELiminar':
                        eliminarCombinacion();
                        break;
                }

            });

            // 2.- Diag Mantenimiento
            $("#diagMantenimiento").dialog({
                autoOpen: false,
                height: 400,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });
        });     // fin de Ready

        function CerrarCombinacion() {
            //$("#jqgCombinacion").clearGridData();
            $('#diagMantenimiento').dialog('close');
        }

        function listarConfiguracion() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgConfiguracion').getGridParam("sortname");
            sortOrder = jQuery('#jqgConfiguracion').getGridParam("sortorder");
            pageSize = jQuery('#jqgConfiguracion').getGridParam("rowNum");
            currentPage = jQuery('#jqgConfiguracion').getGridParam("page");

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
                url: location.pathname + "/listarConfiguracionCombinada",
                data: parametro,  // For empty input data use "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgConfiguracion").clearGridData();
                        var thegrid = jQuery("#jqgConfiguracion")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        function AbrirMantenimiento(Accion) {
            //            $("#jqgCombinacion").clearGridData();
            switch (Accion) {
                case 'NUEVO':
                    llenarComboLineas("NUE");
                    llenarTablaLLaves("NUE", 0, 0);
                    listarLLaves("NUE", 0, 0);
                    document.getElementById('cbLineas').disabled = false;
                    $('#diagMantenimiento').dialog('open');
                    break;
                case 'MODIFICAR':
                    $("#jqgCombinacion").clearGridData();
                    var id = $("#jqgConfiguracion").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione una fila");
                        return;
                    }
                    llenarComboLineas("MOD");
                    var filaConf = $('#jqgConfiguracion').jqGrid('getRowData', id);
                    llenarTablaLLaves("MOD", filaConf.IdLLaves, filaConf.Orden);
                    listarLLaves("MOD", filaConf.IdLLaves, filaConf.Orden);
                    $('#cbLineas').val(filaConf.IdLineas);
                    document.getElementById('cbLineas').disabled = true;
                    $('#diagMantenimiento').dialog('open');
                    //jQuery("#jqgCombinacion").trigger("reloadGrid");
                    break;
            }


        }

        function llenarComboLineas(nombreCombo) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'nombreCombo';
            arreglo[0][1] = nombreCombo;

            parametro = fc_parametrosData(arreglo);
            $("#cbLineas option").remove();

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarCombo",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#cbLineas').append('<option value="0" selected="selected">Seleccione... </option>')
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ninguna Linea disponible.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Linea + '</option>';
                            $('#cbLineas').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }


        function llenarTablaLLaves(mod, idGrid, Orden) {
            $("#jqgCombinacion").jqGrid({
                datatype: function () {
                    listarLLaves(mod, idGrid, Orden);
                },
                height: 180,
                width: 360,
                colNames: ['IdLLave', 'LLave', 'Act', 'Orden'],
                colModel: [
                                { name: 'IdLLave', index: 'IdLLave', hidden: true },
                                { name: 'LLave', index: 'LLave', sorttype: "string" },
                                { name: 'Chk', index: 'Chk', width: 30, align: 'center', editable: 'true', edittype: 'checkbox', editoptions: { value: "True:False" }, formatter: "checkbox", formatoptions: { disabled: false} },
                                { name: 'Dependencia', index: 'Dependencia', align: 'center', width: 30, editable: 'true', edittype: 'text', formatter: generarTextBox, unformat: valorTextBox }
                              ],
                sortname: 'IdLLave',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "LLaves: "                
            });
        }

        function generarTextBox(cellvalue, options, rowObject) {
            return '<input type="text" size="3" value="' + cellvalue + '" maxlength="2" name="txtDependencia" onkeypress="return soloNumeros(event);" />';
        }

        function valorTextBox(cellvalue, options, cell) {
            //alert($('input', cell).attr('value'));
            return $('input', cell).attr('value');
            //return '0';
        }

        function soloNumeros(e) {
            var keynum = window.event ? window.event.keyCode : e.which;
            if ((keynum == 8) || (keynum == 46))
                return true;

            return /\d/.test(String.fromCharCode(keynum));
        }

        function listarLLaves(mod, idLLaves, Orden) {
            $("#jqgCombinacion").clearGridData();
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgCombinacion').getGridParam("sortname");
            sortOrder = jQuery('#jqgCombinacion').getGridParam("sortorder");
            pageSize = jQuery('#jqgCombinacion').getGridParam("rowNum");
            currentPage = jQuery('#jqgCombinacion').getGridParam("page");

            arreglo = fc_redimencionarArray(7);
            arreglo[0][0] = "sortColumn";
            arreglo[0][1] = sortColumn;
            arreglo[1][0] = "sortOrder";
            arreglo[1][1] = sortOrder;
            arreglo[2][0] = "pageSize";
            arreglo[2][1] = pageSize;
            arreglo[3][0] = "currentPage";
            arreglo[3][1] = currentPage;
            arreglo[4][0] = "accion";
            arreglo[4][1] = mod;
            arreglo[5][0] = "idLLaves";
            arreglo[5][1] = idLLaves;
            arreglo[6][0] = "Orden";
            arreglo[6][1] = Orden;

            parametro = fc_parametrosData(arreglo);
            $.ajax({
                url: location.pathname + "/listarLLaves",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        var thegrid = jQuery("#jqgCombinacion")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        function aceptarCombinacion() {

            var IdLinea = $('#cbLineas').val();
            var acumulado = '';
            var dependencias = '';
            // todos los id del grid
            var idTotal = $('#jqgCombinacion').jqGrid('getDataIDs');
            // Recorremos todas las filas 
            for (var i = 0; i < idTotal.length; i++) {
                var fila = $('#jqgCombinacion').jqGrid('getRowData', idTotal[i]);

                var check = fila.Chk;
                if (fila.Chk == 'True') {                    
                    var dep = fila.Dependencia;
                    if ($.trim(dep) == '') {
                        alert('No ah ingresado ninguna dependencia para la llave seleccionada.');
                        return;
                    }
                    var idLLave = fila.IdLLave;
                    acumulado = acumulado + ',' + idLLave;
//                    var filaDep = $('#jqgCombinacion').jqGrid('getRowData', dep);
                    dependencias = dependencias + ',' + dep;
                }
            }
            if (IdLinea == '0') {
                alert('No ah seleccionado ninguna linea. Seleccione una para continuar.');
                return;
            }
            if (acumulado == '') {
                alert('No ah seleccionado ninguna llave. Seleccione una para continuar.');
                return;
            }
            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(4);
            arreglo[0][0] = 'IdLinea';
            arreglo[0][1] = IdLinea
            arreglo[1][0] = 'IdLLaves';
            arreglo[1][1] = acumulado;
            arreglo[2][0] = 'Dependencias';
            arreglo[2][1] = dependencias
            arreglo[3][0] = 'usuario';
            arreglo[3][1] = $('#hdfLogin').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarCombinacionLLaves',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    alert('Combinación guardada correctamente.');
                    $('#diagMantenimiento').dialog('close');
                    jQuery("#jqgConfiguracion").trigger("reloadGrid");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function eliminarCombinacion() {

            var id = $("#jqgConfiguracion").jqGrid('getGridParam', 'selrow');
            if (!id) {
                alert("Por favor seleccione una fila");
                return;
            }
            if (confirm('¿Seguro desea eliminar?')) {
                var parametro = null;
                var arreglo = null;

                var fila = $('#jqgConfiguracion').jqGrid('getRowData', id);
                var Linea = fila.IdLineas;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdLinea';
                arreglo[0][1] = Linea;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = $('#hdfLogin').val();
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/eliminarCombinacion",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        jQuery("#jqgConfiguracion").trigger("reloadGrid");
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


        $(window).bind('resize', function () {
            var valwidth = $(window).width();
            $('#jqgConfiguracion').setGridWidth(valwidth * 0.95);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento Configuración de Lineas - LLaves"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgConfiguracion">
    </table>
    <div id="diagMantenimiento" title="Registro Configuración de Lineas - LLaves">
        <div class="divHeadTitle100">
            Nuevo:</div>
        <div class="divContenido100">
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 80px;">
                        Linea :
                    </td>
                    <td>
                        <select id="cbLineas" style="width: 65%">
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divMantenimiento">
            <table id="jqgCombinacion">
            </table>
        </div>
        <div id="divBotonesMantenimiento" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptar" type="button" value="Aceptar" onclick="aceptarCombinacion(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="CerrarCombinacion(); return false;" />
        </div>
    </div>
    <%--Contenedor de Paginacion--%>
    <div id="pagerConfiguracion">
    </div>
</asp:Content>
