<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmQuerys.aspx.vb" Inherits="Cotizador.frmAdmQuerys" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/json2.js" type="text/javascript"></script>

    <script type ="text/javascript" language ="javascript" >

        $(document).ready(function () {
            String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ""); };
            var valwidth = $(window).width();

            $('#btnAceptar').button();
            $('#btnCerrar').button();

            $("#jqgQuerys").jqGrid({
                datatype: function () {
                    listarQuerys();
                },
                height: 400,
                width: (valwidth * 0.98),
                colNames: ['IdQuery', 'Descripción', 'Query', 'Campos', 'Condiciones'],
                colModel: [
                            { name: 'IdQuery', index: 'IdQuery', hidden: true },
                            { name: 'Descripcion', index: 'Descripcion', width: 20, sorttype: "string" , align : "center"},
                            { name: 'Query', index: 'Query', width: 60, sorttype: "string" },
                            { name: 'Campos', index: 'Campos', width: 30, sorttype: "string" },
                            { name: 'Condiciones', index: 'Condiciones', width: 30, sorttype: "string" }
                          ],
                rowNum: 500,
//                rowList: [5, 20, 50],
//                pager: '#pagerQuerys',
                sortname: 'IdQuery',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Querys: "
            });
            $('#t_jqgQuerys').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");

            $('a', '#t_jqgQuerys').click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantenimiento('NUEVO');
                        break;
                    case 'lblEditar':
                        AbrirMantenimiento('MODIFICAR');
                        break;
                    case 'lblELiminar':
                        Eliminar();
                        break;
                }
            });

            $("#diagMantenimiento").dialog({
                autoOpen: false,
                height: 350,
                width: 500,
                resizable: false,
                modal: true,
                close: function () {
                }
            });
        });

        function listarQuerys() {
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
                url: location.pathname + "/listarQuerys",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgQuerys").clearGridData();
                        var thegrid = jQuery("#jqgQuerys")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function AbrirMantenimiento(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#hdnEstado').val('');
                    $('#txtDescripcion').val('');
                    $('#strQuery').val('');
                    $('#strWhere').val('');
                    $('#strResult').val('');
                    $('#diagMantenimiento').dialog('open');
                    break;
                case 'MODIFICAR':
                    $('#hdnEstado').val('MOD');
                    var id = $("#jqgQuerys").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione un registro de la tabla.");
                        return;
                    }
                    var fila = $('#jqgQuerys').jqGrid('getRowData', id);
                    var IdQuery = fila.IdQuery;
                    var Query = fila.Query;
                    var Descripcion = fila.Descripcion;
                    var Campos = fila.Campos;
                    var Condiciones = fila.Condiciones;

                    Campos = Campos.replace(/\r\, |\r|\, /g, "\n");
                    Condiciones = Condiciones.replace(/\r\, |\r|\, /g, "\n");
                    $('#txtDescripcion').val(Descripcion);
                    $('#strQuery').val(Query);
                    $('#strWhere').val(Condiciones);
                    $('#strResult').val(Campos);
                    $('#diagMantenimiento').dialog('open');
                    break;
            }
        }

        function Guardar(accion) {
            if ($('#hdnEstado').val() == 'MOD') {
                accion = "MOD"
                var id = $("#jqgQuerys").jqGrid('getGridParam', 'selrow');
                if (!id) {
                    alert("Error. No se pudo obtener el codigo del QUERY a Editar. Intentelo Nuevamente.");
                    $('#diagMantenimiento').dialog('close');
                    return;
                }
                if (!confirm('Al editar los campos de este registro se perderan las referencias guardadas. Si continua, revise despues en config. de Tablas, las tablas/celdas asignadas con este query.')) {
                    return;
                }
                var fila = $('#jqgQuerys').jqGrid('getRowData', id);
                var IdQuery = fila.IdQuery;
            }
            var arreglo = null;
            var parametros = null;

            var descripcion = $('#txtDescripcion').val();
            if ($.trim(descripcion) == '') {
                alert('Ingrese una descripción para este Query');
                return;
            }

            var strQuery = $('#strQuery').val();
            var strWhere = $('#strWhere').val();
            var strResult = $('#strResult').val();

            if ($.trim(strQuery) == '') {
                alert('Todos los Campos son Obligatorios');
                return;
            } else {
                if (validarQuery(strQuery)) {
                    alert('El Query Ingresado NO ESTA PERMITIDO.');
                    return;
                }
            }
            if ($.trim(strWhere) == '') {
                alert('Todos los Campos son Obligatorios');
                return;
            }
            if ($.trim(strResult) == '') {
                alert('Todos los Campos son Obligatorios');
                return;
            }
            
            strQuery = strQuery.replace(/\r\n|\r|\n/g, " ");
            strWhere = strWhere.replace(/\r\n|\r|\n/g, ",");
            strResult = strResult.replace(/\r\n|\r|\n/g, ",");

            arreglo = fc_redimencionarArray(7)
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#hdfLogin').val();
            arreglo[2][0] = 'descripcion';
            arreglo[2][1] = descripcion;
            arreglo[3][0] = 'query';
            arreglo[3][1] = strQuery;
            arreglo[4][0] = 'where';
            arreglo[4][1] = strWhere;
            arreglo[5][0] = 'result';
            arreglo[5][1] = strResult;
            arreglo[6][0] = 'codigo';
            arreglo[6][1] = IdQuery;

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarQuerys',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    var retorno = rpta.d;
                    if (retorno.validacion) {
                        alert('Query guardado correctamente.');
                        jQuery("#jqgQuerys").trigger("reloadGrid");
                        $('#diagMantenimiento').dialog('close');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function Eliminar(idQuery) {

            var id = $("#jqgQuerys").jqGrid('getGridParam', 'selrow');            
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (!confirm('¿Esta seguro que desea eliminar este registro?')) {
                return;
            }
            var fila = $('#jqgQuerys').jqGrid('getRowData', id);
            idQuery = fila.IdQuery;
            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(2);

            arreglo[0][0] = 'idQuery';
            arreglo[0][1] = idQuery;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#hdfLogin').val();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/EliminarQuerys",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    jQuery("#jqgQuerys").trigger("reloadGrid");
                    alert('Registro eliminado correctamente.');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function validarQuery(strQuery) {

            var Array = [];
            var Encontro;
            Array[0] = "DELETE";
            Array[1] = "DROP";
            Array[2] = "ALTER";
            Array[3] = "INSERT";
            Array[4] = "UPDATE";
            strQuery = strQuery.toUpperCase();
            for (i = 0; i < Array.length; i++) {
                Encontro = strQuery.indexOf(Array[i]);
                if (Encontro >= 0) {
                    return true;
                }
            }
            return false;
        }

        function Cerrar() {
            $('#diagMantenimiento').dialog('close');
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento de Querys"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgQuerys"></table>
    <div id="diagMantenimiento" title="Manenimiento de Querys">
        <input id="hdnEstado" type="hidden" value=""/>
        <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width:20%;">
                        Descripción:
                    </td>
                    <td style="width:80%;">
                        <input type="text" id="txtDescripcion" style="width:98%;"/>
                    </td>
                </tr>
                <tr><td></td></tr>
                <tr>
                    <td valign="middle">
                        Ingrese Query:
                    </td>
                    <td>
                        <textarea id="strQuery" rows="5"  style="width:98%;"></textarea>
                    </td>
                </tr>
                <tr><td></td></tr>
                <tr>
                    <td valign="middle">
                        Condicionales (Cotizador):
                    </td>
                    <td>
                        <textarea id="strWhere" rows="3"  style="width:98%;"></textarea>
                    </td>
                </tr>
                <tr><td></td></tr>
                <tr>
                    <td valign="middle">
                        Resultado:
                    </td>
                    <td>
                        <textarea id="strResult" rows="3" style="width:98%;"></textarea>
                    </td>
                </tr>
            </table>
            <div id="divBotonesMantenimiento" 
                style="text-align: center; 
                padding-top: 5px; 
                border-bottom-color: Navy;
                border-width: 5px; width: 100%">
                <input id="btnAceptar" type="button" value="Guardar" onclick="Guardar(); return false;" />
                <input id="btnCerrar" type="button" value="Cancelar" onclick="Cerrar(); return false;" />
            </div>
    </div>
     <%--<div id="pagerQuerys">
     </div>--%>
</asp:Content>