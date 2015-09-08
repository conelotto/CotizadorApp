<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmCampos.aspx.vb" Inherits="Cotizador.frmAdmCampos" %>

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
                colNames: ['Codigo', 'Descripción', 'Tabla', 'Campo'],
                colModel: [
                            { name: 'Codigo', index: 'Codigo', hidden: true },
                            { name: 'Descripcion', index: 'Descripcion', width: 100, sorttype: "string" },
                            { name: 'Tabla', index: 'Tabla', width: 50, sorttype: "string" },
                            { name: 'Campo', index: 'Campo', width: 50, sorttype: "string" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerCampos',
                sortname: 'IdCampos',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Detalle Campos"
            });
            $('#t_jqgCampos').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
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
                height: 200,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            $('#cbTabla').change(function (evento) {

                var id = $('#cbTabla').val();

                if (id == '0') {
                    alert('Seleccione una Tabla.');
                }
                else {
                    //                 var tabla = $('#cbTabla :selected').text();
                    var tabla = $('#cbTabla').val();
                    cargarComboCampo(tabla);
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

            var Descripcion = $.trim($('#txtDescripcion').val());
            var Tabla = $('#cbTabla').val();
            var Campo = $('#cbCampo').val();

            if (Descripcion == '') {
                alert('Ingrese una descripción.');
                return;
            }
            if (Tabla == '0') {
                alert('Seleccione una tabla.');
                return;
            }
            if (Campo == '0') {
                alert('Seleccione un campo.');
                return;
            }

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(6);
            arreglo[0][0] = 'Tabla';
            arreglo[0][1] = Tabla
            arreglo[1][0] = 'Campo';
            arreglo[1][1] = Campo
            arreglo[2][0] = 'Descripcion';
            arreglo[2][1] = Descripcion;
            arreglo[3][0] = 'usuario';
            arreglo[3][1] = $('#hdfLogin').val();
            arreglo[4][0] = 'accion';
            arreglo[4][1] = $('#hdnAccion').val();
            arreglo[5][0] = 'codigo';
            arreglo[5][1] = $('#hdnCodMod').val();

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
                    $('#txtDescripcion').val('');
                    $("#cbCampo option").remove();
                    $("#cbTabla option").remove();
                    cargarComboTabla();
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
                    $("#cbTabla option").remove();
                    cargarComboTabla();
                    //                     var valCod = $('#cbTabla :selected').text();
                    $('#cbTabla').val(fila.Tabla);
                    cargarComboCampo(fila.Tabla, 'MOD');
                    $('#cbCampo').val(fila.Campo);
                    $('#txtDescripcion').val(fila.Descripcion);
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
                var idCampo = fila.Codigo;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdCampo';
                arreglo[0][1] = idCampo;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = $('#hdfLogin').val();
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

        function CerrarCampo() {
            $('#diagMantCampos').dialog('close');
        }

        function cargarComboCampo(Tabla, accion) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(2);
            arreglo[0][0] = 'nombreTabla';
            arreglo[0][1] = Tabla;
            arreglo[1][0] = 'accion';
            arreglo[1][1] = accion;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboCampo",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#cbCampo option').remove();
                    $('#cbCampo').append('<option value="0" selected="selected">Seleccione... </option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun Campo para la tabla seleccionada.');
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

        function cargarComboTabla() {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'nombreCombo';
            arreglo[0][1] = 'nn';

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboTabla",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#cbTabla').append('<option value="0" selected="selected">Seleccione... </option>');
                    $('#cbCampo').append('<option value="0" selected="selected">Seleccione Tabla... </option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ninguna Tabla en el esquema Cotizador.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbTabla').append(varoption);
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
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento de Campos"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgCampos">
    </table>
    <div id="diagMantCampos" title="Registro Campos">
        <div class="divHeadTitle100">
            Nuevo Campo:</div>
        <div class="divContenido100">
            <input id="hdnAccion" type="hidden" />
            <input id="hdnCodMod" type="hidden" />
            <table style="width: 100%; text-align: left">
                <tr>
                    <td>
                        Ingrese Descripcion:
                    </td>
                    <td>
                        <input type="text" id="txtDescripcion" style="width: 85%;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Seleccione Tabla:
                    </td>
                    <td>
                        <select id="cbTabla" style="width: 80%">
                            <option></option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seleccione Campo:
                    </td>
                    <td>
                        <select id="cbCampo" style="width: 80%">
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantenimiento" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptar" type="button" value="Aceptar" onclick="aceptarCampo(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="CerrarCampo(); return false;" />
        </div>
    </div>
    <div id="pagerCampos">
    </div>
</asp:Content>
