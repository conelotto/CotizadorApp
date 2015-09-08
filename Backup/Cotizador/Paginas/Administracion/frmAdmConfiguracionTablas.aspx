<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmConfiguracionTablas.aspx.vb" Inherits="Cotizador.frmAdmConfiguracionTablas" %>
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

            $('#btnAceptarTabla').button();
            $('#btnCerrarTabla').button();
            $('#btnAceptarCelda').button();
            $('#btnCerrarCelda').button();

            $("#jqgTablas").jqGrid({
                datatype: function () {
                    listarTablas();
                },
                height: 300,
                width: (valwidth * 0.98),
                colNames: ['IdTabla', 'Descripción', 'Repetir Filas'],
                colModel: [
                            { name: 'IdTabla', index: 'IdTabla', hidden: true },
                            { name: 'Descripcion', index: 'Descripcion', width: 50, sorttype: "string" },
                            { name: 'Repetir', index: 'Repetir', width: 30, sorttype: "string", align:"center" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerTablas',
                sortname: 'IdTabla',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Tablas Configuradas: "
            });
            $('#t_jqgTablas').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
              
            $('a', '#t_jqgTablas').click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantTablas('NUEVO');
                        break;
                    case 'lblEditar':
                        AbrirMantTablas('MODIFICAR');
                        break;
                    case 'lblELiminar':
                        EliminarTablas();
                        break;
                }
            });

            $("#diagMantTablas").dialog({
                autoOpen: false,
                height: 390,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                    var IdTabla = $('#hdnIdTablaNueva').val();
                    if (IdTabla != "0") {
                        EliminarCeldas("PIV", IdTabla);
                        EliminarTablas("PIV", IdTabla);                        
                        $('#hdnIdTablaNueva').val("0");
                    }
                    $('#hdnEstado').val("");
                    $('#hdnIdTablaMod').val("0");
                }
            });

            $("#jqgCeldas").jqGrid({
                datatype: function () {
                    listarCeldas();
                },
                height: 120,
                width: 360,
                colNames: ['IdCelda', 'IdTabla', 'Fila', 'Columna', 'Tipo', 'IdValor', 'Query/Formula'],
                colModel: [
                            { name: 'IdCelda', index: 'IdCelda', hidden: true },
                            { name: 'IdTabla', index: 'IdTabla', hidden: true },
                            { name: 'Fila', index: 'Fila', width: 5, sorttype: "string" },
                            { name: 'Columna', index: 'Columna', width: 7, sorttype: "string" },
                            { name: 'Tipo', index: 'Tipo', width: 5, sorttype: "string" },
                            { name: 'IdValor', index: 'IdValor', hidden: true },
                            { name: 'Valor', index: 'Valor', width: 15, sorttype: "string" }
                          ],
                rowNum: 5,
                rowList: [5, 10, 20],
                pager: '#pagerCeldas',
                sortname: 'IdCelda',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Celdas: "
            });
            $('#t_jqgCeldas').append("<table><tr><td style='vertical-aling: middle'> <a id='lblAgregar'>Agregar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");
            //             <a id='lblEditar'>Editar</a> | 
            $('a', '#t_jqgCeldas').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblAgregar':
                        AbrirMantCeldas('NUEVO');
                        break;
                    case 'lblELiminar':
                        EliminarCeldas();
                        break;
                }
            });

            $("#diagMantCeldas").dialog({
                autoOpen: false,
                height: 235,
                width: 300,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            $('input:radio[name="tipo"]').change(function () {
                if ($(this).val() == 'query') {
                    $("#cbTipo option").remove();
                    $('#lblTipo').text('Query: ');
//                    cargarComboCampo('NUE');
                    cargarComboQuery('NUE');
                }
                if ($(this).val() == 'formula') {
                    $("#cbTipo option").remove();
                    $('#cbTipo').append('<option value="0" selected="selected">Seleccione Formula</option>');
                    $('#lblTipo').text('Formula: ');
                }
            });

        });   //fin ready

        function AbrirMantTablas(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#txtDescripcionTabla').val('');
                    $("#chkRepetir").attr("checked", false);
                    listarCeldas();
                    $('#diagMantTablas').dialog('open');
                    AceptarTablas("PIV");
                    break;
                case 'MODIFICAR':
                    $('#hdnEstado').val('MOD');                    
                    var id = $("#jqgTablas").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione un registro de la tabla.");
                        return;
                    }
                    var fila = $('#jqgTablas').jqGrid('getRowData', id);
                    $('#txtDescripcionTabla').val(fila.Descripcion);
                    if (fila.Repetir == "SI") {
                        $("#chkRepetir").attr("checked", true);
                    }
                    else {
                        $("#chkRepetir").attr("checked", false);
                    }
                    $('#hdnIdTablaMod').val(fila.IdTabla);
                    listarCeldas("MOD", fila.IdTabla);
                    $('#diagMantTablas').dialog('open');
                    break;
            }
        }

        function AbrirMantCeldas(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('input:radio[name=tipo]').filter('[value=query]').attr('checked', true);
                    $('#lblTipo').text('Query: ');
                    LLenarFilasColumnas();
//                    cargarComboCampo('NUE');
                    cargarComboQuery('NUE');
                    $('#diagMantCeldas').dialog('open');
                    break;
            }
        }

        function listarTablas() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgTablas').getGridParam("sortname");
            sortOrder = jQuery('#jqgTablas').getGridParam("sortorder");
            pageSize = jQuery('#jqgTablas').getGridParam("rowNum");
            currentPage = jQuery('#jqgTablas').getGridParam("page");

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
                url: location.pathname + "/listarTablas",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgTablas").clearGridData();
                        var thegrid = jQuery("#jqgTablas")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function listarCeldas(accion, IdTabla) {

            if (accion != 'MOD' && accion != 'PIV') {
                IdTabla = "0";
            }

            arreglo = parametro = null;
            sortColumn = jQuery('#jqgCeldas').getGridParam("sortname");
            sortOrder = jQuery('#jqgCeldas').getGridParam("sortorder");
            pageSize = jQuery('#jqgCeldas').getGridParam("rowNum");
            currentPage = jQuery('#jqgCeldas').getGridParam("page");

            arreglo = fc_redimencionarArray(5);
            arreglo[0][0] = "sortColumn";
            arreglo[0][1] = sortColumn;
            arreglo[1][0] = "sortOrder";
            arreglo[1][1] = sortOrder;
            arreglo[2][0] = "pageSize";
            arreglo[2][1] = pageSize;
            arreglo[3][0] = "currentPage";
            arreglo[3][1] = currentPage;
            arreglo[4][0] = "IdTabla";
            arreglo[4][1] = IdTabla;

            parametro = fc_parametrosData(arreglo);
            $.ajax({
                url: location.pathname + "/listarCeldas",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgCeldas").clearGridData();
                        var thegrid = jQuery("#jqgCeldas")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function AceptarTablas(accion) {
            var idTabla = "";
            if ($('#hdnEstado').val() == 'MOD') {
                idTabla = $('#hdnIdTablaMod').val();
                accion = "MOD"
            }
            else {
                idTabla=$('#hdnIdTablaNueva').val();
            }
            var arreglo = null;
            var parametros = null;
            var descipcion = $('#txtDescripcionTabla').val();
            var repetir;
            $("#chkRepetir").is(':checked') ? repetir="1" : repetir="0";
            if (descipcion == "" && accion != "PIV") {
                alert("Ingrese una Nombre para la Tabla.");
                return;
            }

            arreglo = fc_redimencionarArray(5);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#hdfLogin').val();
            arreglo[2][0] = 'descripcion';
            arreglo[2][1] = descipcion;
            arreglo[3][0] = 'repetir';
            arreglo[3][1] = repetir;
            arreglo[4][0] = 'idTabla';
            arreglo[4][1] = idTabla;

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarTablas',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    if (accion == "PIV") {
                        $('#hdnIdTablaNueva').val(rpta.d);
                    }
                    else {
                        alert('Tabla guardada correctamente.');
                        jQuery("#jqgTablas").trigger("reloadGrid");
                        $('#hdnIdTablaNueva').val("0");
                        $('#diagMantTablas').dialog('close');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function AceptarCeldas() {

            var arreglo = null;
            var parametros = null;
            var idTabla = "";
            var numFila = $('#cbFila').val();
            var numColumna = $('#cbColumna').val();
            var tipo = $('input[name="tipo"]:checked').val();
            var valorTipo = $('#cbTipo').val();

            if (valorTipo == "0") {
                alert("Seleccione un(a) " + tipo);
                return;
            }

            if ($('#hdnEstado').val() == 'MOD') {
                idTabla = $('#hdnIdTablaMod').val();
            }
            else{
                idTabla=$('#hdnIdTablaNueva').val();
            }

            arreglo = fc_redimencionarArray(6);
            arreglo[0][0] = 'IdTabla';
            arreglo[0][1] = idTabla;
            arreglo[1][0] = 'Fila';
            arreglo[1][1] = numFila;
            arreglo[2][0] = 'Columna';
            arreglo[2][1] = numColumna;
            arreglo[3][0] = 'Tipo';
            arreglo[3][1] = tipo;
            arreglo[4][0] = 'ValorTipo';
            arreglo[4][1] = valorTipo;
            arreglo[5][0] = 'usuario';
            arreglo[5][1] = $('#hdfLogin').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarCeldas',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    listarCeldas("MOD", idTabla);
//                    jQuery("#jqgCeldas").trigger("reloadGrid");
                    $('#diagMantCeldas').dialog('close');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function EliminarTablas(accion, IdTabla) {

            if (accion != 'PIV') {
                var id = $("#jqgTablas").jqGrid('getGridParam', 'selrow');
                var fila = $('#jqgTablas').jqGrid('getRowData', id);
                IdTabla = fila.IdTabla;
                if (!id) {
                    alert("Por favor seleccione un registro de la tabla.");
                    return;
                }
                if (!confirm('¿Esta seguro que desea eliminar este registro?')) {
                    return;
                }
            }

            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(3);

            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'Id';
            arreglo[1][1] = IdTabla;
            arreglo[2][0] = 'usuario';
            arreglo[2][1] = $('#hdfLogin').val();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/EliminarTablas",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (accion != 'PIV') {
                        var result = data.d;
                        jQuery("#jqgTablas").trigger("reloadGrid");
                        alert('Registro eliminado correctamente.');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function EliminarCeldas(accion, IdTabla) {
            var IdCelda = ""
            if (accion != 'PIV') {
                var id = $("#jqgCeldas").jqGrid('getGridParam', 'selrow');
                var fila = $('#jqgCeldas').jqGrid('getRowData', id);
                IdTabla = fila.IdTabla;
                IdCelda = fila.IdCelda;
                if (!id) {
                    alert("Por favor seleccione un registro de la tabla.");
                    return;
                }
            }
            else {
                IdTabla = $('#hdnIdTablaNueva').val();
            }

            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(4);

            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'IdTabla';
            arreglo[1][1] = IdTabla;
            arreglo[2][0] = 'IdCelda';
            arreglo[2][1] = IdCelda;
            arreglo[3][0] = 'usuario';
            arreglo[3][1] = $('#hdfLogin').val();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/EliminarCeldas",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (accion != 'PIV') {
                        var result = data.d;
                        //                        jQuery("#jqgCeldas").trigger("reloadGrid");
                        listarCeldas("MOD", IdTabla);
                        alert('Registro eliminado correctamente.');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }
        
        function cargarComboQuery(accion) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboQuery",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#cbTipo option").remove();
                    $('#cbTipo').append('<option value="0" selected="selected">Seleccione</option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun opción para seleccionar.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbTipo').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
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
                    $("#cbTipo option").remove();
                    $('#cbTipo').append('<option value="0" selected="selected">Seleccione Campo </option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun Campo libre de asignación.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbTipo').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }
        function LLenarFilasColumnas() {
            $("#cbFila option").remove();
            $("#cbColumna option").remove();
            for (var i = 1; i < 21; i++) {
                var varoption = '<option value="' + i + '" >' + i + '</option>';
                $('#cbFila').append(varoption);
                $('#cbColumna').append(varoption);
            }
        }

        function CerrarTablas() {
            $('#diagMantTablas').dialog('close');
        }

        function CerrarCeldas() {
            $('#diagMantCeldas').dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Configuración de Tablas"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgTablas"></table>
    <div id="diagMantTablas" title="Registro de Tablas: ">
        <div class="divHeadTitle100">Tablas:</div>
        <div class="divContenido100">
            <input id="hdnIdTablaNueva" type="hidden" value="0"/>
            <input id="hdnIdTablaMod" type="hidden" value="0"/>
            <input id="hdnEstado" type="hidden" value=""/>
            
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width:40%;">
                        Ingrese Nombre Tabla:
                    </td>
                    <td>
                        <input type="text" id="txtDescripcionTabla" style="width: 80%;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="chkRepetir" name="chkRepetir"/>Repetir Filas
                    </td>
                </tr>
            </table>
            <br />
            <table id="jqgCeldas" title="Celdas"></table>
        </div>
        <div id="divBotonesMantTabla" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptarTabla" type="button" value="Aceptar" onclick="AceptarTablas(); return false;" />
            <input id="btnCerrarTabla" type="button" value="Cerrar" onclick="CerrarTablas(); return false;" />
        </div>
        <div id="pagerCeldas">
    </div>
    </div>
    <div id="diagMantCeldas" title="Registro de Celdas: ">
        <div class="divHeadTitle100">Celdas:</div>
        <div class="divContenido100">
            <table width="100%">
                <tr>
                    <td>
                    </td>
                    <td>
                        Fila
                    </td>
                    <td>
                        Columna
                    </td>
                </tr>
                <tr>
                    <td>
                        Ubicación Celda:
                    </td>
                    <td>
                        <select id="cbFila" style="width: 50px">
                            <option></option>
                        </select>
                    </td>
                    <td>
                        <select id="cbColumna" style="width: 50px">
                            <option></option>
                        </select>
                    </td>
                </tr>
                <tr><td>&nbsp</td></tr>
                <tr>
                    <td>
                        <input type="radio" name="tipo" value="query" />Query
                    </td>
                    <td colspan="2">
                        <input type="radio" name="tipo" value="formula" />Formula
                    </td>
                </tr>
                <tr><td>&nbsp</td></tr>
                <tr>
                    <td align="right">
                        <label id="lblTipo" for="Query :"></label>
                    </td>
                    <td colspan="2">
                        <select id="cbTipo" style="width: 150px"  >
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantCeldas" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptarCelda" type="button" value="Grabar" onclick="AceptarCeldas(); return false;" />
            <input id="btnCerrarCelda" type="button" value="Cancelar" onclick="CerrarCeldas(); return false;" />
        </div>
    </div>
    <div id="pagerTablas">
    </div>
</asp:Content>
