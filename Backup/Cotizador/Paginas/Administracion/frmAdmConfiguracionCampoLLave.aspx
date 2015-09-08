<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmConfiguracionCampoLLave.aspx.vb" Inherits="Cotizador.frmAdmConfiguracionCampoLLave" %>

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

            $("#jqgCampos").jqGrid({
                datatype: function () {
                    listarCampos();
                },
                height: 300,
                width: (valwidth * 0.98),
                colNames: ['Codigo', 'IdLLave', 'LLave', 'IdCampo', 'Campos'],
                colModel: [
                            { name: 'Codigo', index: 'Codigo', hidden: true },
                            { name: 'IdLLave', index: 'IdLLave', hidden: true },
                            { name: 'LLave', index: 'LLave', width: 50, sorttype: "string" },
                            { name: 'IdCampo', index: 'IdCampo', hidden: true },
                            { name: 'Campo', index: 'Campo', width: 85, sorttype: "string" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerCampos',
                sortname: 'IdLLave',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Detalle Configuracíon LLaves - Campos"
            });
            $('#t_jqgCampos').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> |<a id='lblELiminar'>Eliminar</a></td></tr></table>");
            $('a', '#t_jqgCampos').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        $('#hdnAccion').val('NUE');
                        AbrirMantCampos('NUEVO');
                        break;
                    case 'lblEditar':
                        $('#hdnAccion').val('MOD');
                        AbrirMantCampos('MODIFICAR');
                        break;
                    case 'lblELiminar':
                        eliminarCampos();
                        break;
                }
            });

            $("#diagMantCampos").dialog({
                autoOpen: false,
                height: 250,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

        });

        function listarCampos() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgCampos').getGridParam("sortname");
            sortOrder = jQuery('#jqgCampos').getGridParam("sortorder");
            pageSize = jQuery('#jqgCampos').getGridParam("rowNum");
            currentPage = jQuery('#jqgCampos').getGridParam("page");

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
                url: location.pathname + "/listarCampos",
                data: parametro,  // For empty input data use "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgCampos").clearGridData();
                        var thegrid = jQuery("#jqgCampos")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function aceptarCampo() {

            var LLave = $('#cbLLave').val();
            var Campo = $('#cbCampo').val();

            if (LLave == '0') {
                alert('Seleccione una LLave.');
                return;
            }
            if (Campo == '0') {
                alert('Seleccione un campo.');
                return;
            }

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(5);
            arreglo[0][0] = 'IdLLave';
            arreglo[0][1] = LLave
            arreglo[1][0] = 'IdCampo';
            arreglo[1][1] = Campo
            arreglo[2][0] = 'usuario';
            arreglo[2][1] = $('#hdfLogin').val();
            arreglo[3][0] = 'accion';
            arreglo[3][1] = $('#hdnAccion').val();
            arreglo[4][0] = 'codigo';
            arreglo[4][1] = $('#hdnCodMod').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarCampos',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    if (rpta.d == "1") {
                        alert('Campo guardado correctamente.');
                        $('#diagMantCampos').dialog('close');
                        jQuery("#jqgCampos").trigger("reloadGrid");
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

        function AbrirMantCampos(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $("#cbCampo option").remove();
                    $("#cbLLave option").remove();
                    cargarComboCampo('NUE');
                    cargarComboLLave('NUE');
                    $('#diagMantCampos').dialog('open');
                    break;
                case 'MODIFICAR':
                    var id = $("#jqgCampos").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione un registro de la tabla.");
                        return;
                    }
                    var fila = $('#jqgCampos').jqGrid('getRowData', id);
                    $("#cbCampo option").remove();
                    $("#cbLLave option").remove();
                    cargarComboLLave('MOD');
                    //                     var valCod = $('#cbTabla :selected').text();
                    $('#cbLLave').val(fila.IdLLave);
                    cargarComboCampo('MOD');
                    $('#cbCampo').val(fila.IdCampo);
                    $('#hdnCodMod').val(fila.Codigo);
                    $('#diagMantCampos').dialog('open');
                    break;
            }


        }

        function eliminarCampos() {
            var id = $("#jqgCampos").jqGrid('getGridParam', 'selrow');
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (confirm('¿Seguro desea eliminar?')) {
                var parametro = null;
                var arreglo = null;

                var fila = $('#jqgCampos').jqGrid('getRowData', id);
                var id = fila.Codigo;
                arreglo = fc_redimencionarArray(3);
                arreglo[0][0] = 'Id';
                arreglo[0][1] = id;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = $('#hdfLogin').val();
                arreglo[1][0] = 'mantenimiento';
                arreglo[1][1] = 'LOCAL';
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/eliminarCampos",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        jQuery("#jqgCampos").trigger("reloadGrid");
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

        function Cerrar() {
            $('#diagMantCampos').dialog('close');
        }

        function cargarComboCampo(accion) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboCampo",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#cbCampo').append('<option value="0" selected="selected">Seleccione... </option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun Campo libre de asignación.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbCampo').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function cargarComboLLave(accion) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboLLave",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#cbLLave').append('<option value="0" selected="selected">Seleccione... </option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ninguna LLave libre para asignación.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbLLave').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento Configuración LLave - Campo"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgCampos">
    </table>
    <div id="diagMantCampos" title="Registro Configuración">
        <div class="divHeadTitle100">
            Nuevo Campo:</div>
        <div class="divContenido100">
            <input id="hdnAccion" type="hidden" />
            <input id="hdnCodMod" type="hidden" />
            <table style="width: 100%; text-align: left">
                <tr>
                    <td>
                        Seleccione LLave:
                    </td>
                    <td>
                        <select id="cbLLave" style="width: 85%">
                            <option></option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seleccione Campo:
                    </td>
                    <td>
                        <select id="cbCampo" style="width: 85%">
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantenimiento" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptar" type="button" value="Aceptar" onclick="aceptarCampo(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="Cerrar(); return false;" />
        </div>
    </div>
    <div id="pagerCampos">
    </div>
</asp:Content>
