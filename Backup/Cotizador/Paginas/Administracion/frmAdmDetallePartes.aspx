<%@ Page Title="Detalle Partes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmDetallePartes.aspx.vb" Inherits="Cotizador.frmAdmDetallePartes" %>

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
    <script type="text/javascript">
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        $(document).ready(function () {

            var frmName = location.pathname;
            var mensaje = '<h1>Cotizador</h1><br/>';
            var usuario = '<%= HttpContext.Current.Session("idUsuario").toString() %>';

            // -------------  que se conviertan en botones ----------------------------------------
            $("#btnGrabar").button();
            $("#btnCancelar").button();
            $("#btnGrabar").css("width", "90");
            $("#btnGrabar").css("height", "28");
            $("#btnCancelar").css("width", "90");
            $("#btnCancelar").css("height", "28");

            $("#exbtnGrabar").button();
            $("#exbtnCancelar").button();
            $("#exbtnGrabar").css("width", "90");
            $("#exbtnGrabar").css("height", "28");
            $("#exbtnCancelar").css("width", "90");
            $("#exbtnCancelar").css("height", "28");

            $("#itxtFamilia").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtModeloBase").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtModelo").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtPrefijo").maxlength({ maxCharacters: 3, status: false, showAlert: false });
            $("#itxtServiceCategory").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtRodetail").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtQuantity").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtReplacement").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtUnitPrice").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtExtendedPrice").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtSellEvent").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtEventos").maxlength({ maxCharacters: 10, status: false, showAlert: false });
            $("#itxtSell").maxlength({ maxCharacters: 10, status: false, showAlert: false });

            $("#itxtCompQty").numeric({ allow: '.' });
            $("#itxtFirstInterval").numeric({ allow: '.' });
            $("#itxtNextInterval").numeric({ allow: '.' });
            $("#itxtQuantity").numeric({ allow: '.' });
            $("#itxtReplacement").numeric({ allow: '.' });
            $("#itxtUnitPrice").numeric({ allow: '.' });
            $("#itxtExtendedPrice").numeric({ allow: '.' });
            $("#itxtSellEvent").numeric({ allow: '.' });
            $("#itxtEventos").numeric({ allow: '.' });
            $("#itxtSell").numeric({ allow: '.' });

            $("#itxtFamilia").css('width', '300');
            $("#itxtModeloBase").css('width', '300');
            $("#itxtModelo").css('width', '300');
            $("#itxtPrefijo").css('width', '300');
            $("#itxtServiceCategory").css('width', '300');
            $("#itxtRodetail").css('width', '300');
            $("#itxtJodetail").css('width', '300');
            $("#itxtSOSPartNumber").css('width', '300');
            $("#itxtSOSDescription").css('width', '300');
            $("#itxtCompQty").css('width', '100');
            $("#itxtFirstInterval").css('width', '100');
            $("#itxtNextInterval").css('width', '100');
            $("#itxtQuantity").css('width', '100');
            $("#itxtReplacement").css('width', '100');
            $("#itxtUnitPrice").css('width', '100');
            $("#itxtExtendedPrice").css('width', '100');
            $("#itxtSellEvent").css('width', '100');
            $("#itxtEventos").css('width', '100');
            $("#itxtSell").css('width', '100');

            // ------------------------------------------------------------------------------------------

            function nuevo() {
                iniciarDialog();
                editarDialog(true);
                $("#ihddTipo").attr('value', 'N');
                $("#idDialogForm").dialog("open");
            }

            function editar() {

                var id = jQuery("#gvDetallePartes").jqGrid('getGridParam', 'selrow');
                if (!id) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + "Por favor seleccione una fila");
                } else {
                    iniciarDialog();
                    editarDialog(true);
                    leerDatos(id);
                    $("#ihddTipo").attr('value', 'E');
                    $("#idDialogForm").dialog("open");
                }
            }

            function eliminar() {

                var id = jQuery("#gvDetallePartes").jqGrid('getGridParam', 'selrow');

                if (!id) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + "Por favor seleccione una fila");
                } else {
                    if (!(confirm('¿Seguro desea eliminar el Detalle Partes?'))) {
                        return;
                    }

                    var parametro = null;
                    var arreglo = null;
                    var prefijo = ret.prefijo;

                    arreglo = fc_redimencionarArray(2);
                    arreglo[0][0] = 'id';
                    arreglo[0][1] = id;
                    arreglo[1][0] = 'usuario';
                    arreglo[1][1] = usuario;
                    parametro = fc_parametrosData(arreglo);

                    $.ajax({
                        type: "POST",
                        url: frmName + "/Eliminar",
                        data: parametro,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var result = data.d;
                            if (!result.validacion) {
                                jAlert.error('<h1>Cotizador</h1><br/>' + result.mensaje);
                            } else {
                                jAlert.alert('<h1>Cotizador</h1><br/>' + 'Se elimino correctamente');
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });


                }
            }



            function grabar() {

                var arreglo = null;
                var parametro = null;
                var tipo = $("#ihddTipo").get(0).value;
                var id = $("#ihddIdDetallePartes").get(0).value;
                var familia = $("#itxtFamilia").get(0).value;
                var modeloBase = $("#itxtModeloBase").get(0).value;
                var modelo = $("#itxtModelo").get(0).value;
                var prefijo = $("#itxtPrefijo").get(0).value;
                var serviceCategory = $("#itxtServiceCategory").get(0).value;
                var Rodetail = $("#itxtRodetail").get(0).value;
                var compQty = $("#itxtCompQty").get(0).value;
                var firstInterval = $("#itxtFirstInterval").get(0).value;
                var nextInterval = $("#itxtNextInterval").get(0).value;
                var jodetail = $("#itxtJodetail").get(0).value;
                var SOSPartNumber = $("#itxtSOSPartNumber").get(0).value;
                var SOSDescription = $("#itxtSOSDescription").get(0).value;
                var quantity = $("#itxtQuantity").get(0).value;
                var replacement = $("#itxtReplacement").get(0).value;
                var unitPrice = $("#itxtUnitPrice").get(0).value;
                var extendedPrice = $("#itxtExtendedPrice").get(0).value;
                var sellEvent = $("#itxtSellEvent").get(0).value;
                var eventos = $("#itxtEventos").get(0).value;
                var sell = $("#itxtSell").get(0).value;

                arreglo = fc_redimencionarArray(22)
                arreglo[0][0] = 'tipo';
                arreglo[0][1] = tipo;
                arreglo[1][0] = 'id';
                arreglo[1][1] = id;
                arreglo[2][0] = 'prefijo';
                arreglo[2][1] = prefijo;
                arreglo[3][0] = 'familia';
                arreglo[3][1] = familia;
                arreglo[4][0] = 'modelo';
                arreglo[4][1] = modelo;
                arreglo[5][0] = 'modeloBase';
                arreglo[5][1] = modeloBase;
                arreglo[6][0] = 'serviceCategory';
                arreglo[6][1] = serviceCategory;
                arreglo[7][0] = 'Rodetail';
                arreglo[7][1] = Rodetail;
                arreglo[8][0] = 'compQty';
                arreglo[8][1] = compQty;
                arreglo[9][0] = 'firstInterval';
                arreglo[9][1] = firstInterval;
                arreglo[10][0] = 'nextInterval';
                arreglo[10][1] = nextInterval;
                arreglo[11][0] = 'jodetail';
                arreglo[11][1] = jodetail;
                arreglo[12][0] = 'SOSPartNumber';
                arreglo[12][1] = SOSPartNumber;
                arreglo[13][0] = 'SOSDescription';
                arreglo[13][1] = SOSDescription;
                arreglo[14][0] = 'quantity';
                arreglo[14][1] = quantity;
                arreglo[15][0] = 'replacement';
                arreglo[15][1] = replacement;
                arreglo[16][0] = 'unitPrice';
                arreglo[16][1] = unitPrice;
                arreglo[17][0] = 'extendedPrice';
                arreglo[17][1] = extendedPrice;
                arreglo[18][0] = 'sellEvent';
                arreglo[18][1] = sellEvent;
                arreglo[19][0] = 'eventos';
                arreglo[19][1] = eventos;
                arreglo[20][0] = 'sell';
                arreglo[20][1] = sell;
                arreglo[21][0] = 'usuario';
                arreglo[21][1] = usuario;

                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: frmName + "/Grabar",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            jAlert.error('<h1>Cotizador</h1><br/>' + result.mensaje);
                        } else {
                            $("#btnCancelar").click();
                            jAlert.alert('<h1>Cotizador</h1><br/>' + 'Se grabo correctamente');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function editarDialog(rpt) {

                rpt = !rpt;

                if (!rpt) {
                    $("#btnGrabar").css('display', 'block');
                } else {
                    $("#btnGrabar").css('display', 'none');
                }

            }


            function iniciarDialog() {

                $("#ihddTipo").attr('value', '');
                $("#ihddIdDetallePartes").attr('value', '');
                $("#itxtFamilia").attr('value', '');
                $("#itxtModeloBase").attr('value', '');
                $("#itxtModelo").attr('value', '');
                $("#itxtPrefijo").attr('value', '');
                $("#itxtServiceCategory").attr('value', '');
                $("#itxtRodetail").attr('value', '');
                $("#itxtCompQty").attr('value', '');
                $("#itxtFirstInterval").attr('value', '');
                $("#itxtNextInterval").attr('value', '');
                $("#itxtJodetail").attr('value', '');
                $("#itxtSOSPartNumber").attr('value', '');
                $("#itxtSOSDescription").attr('value', '');
                $("#itxtQuantity").attr('value', '');
                $("#itxtReplacement").attr('value', '');
                $("#itxtUnitPrice").attr('value', '');
                $("#itxtExtendedPrice").attr('value', '');
                $("#itxtSellEvent").attr('value', '');
                $("#itxtEventos").attr('value', '');
                $("#itxtSell").attr('value', '');

            }


            function leerDatos(id) {

                var ret = jQuery("#gvDetallePartes").jqGrid('getRowData', id);

                $("#ihddIdDetallePartes").attr('value', ret.id);

                $("#itxtFamilia").attr('value', ret.familia);
                $("#itxtModeloBase").attr('value', ret.modeloBase);
                $("#itxtModelo").attr('value', ret.modelo);
                $("#itxtPrefijo").attr('value', ret.prefijo);
                $("#itxtServiceCategory").attr('value', ret.serviceCategory);
                $("#itxtRodetail").attr('value', ret.Rodetail);
                $("#itxtCompQty").attr('value', ret.compQty);
                $("#itxtFirstInterval").attr('value', ret.firstInterval);
                $("#itxtNextInterval").attr('value', ret.nextInterval);
                $("#itxtJodetail").attr('value', ret.jodetail);
                $("#itxtSOSPartNumber").attr('value', ret.SOSPartNumber);
                $("#itxtSOSDescription").attr('value', ret.SOSDescription);
                $("#itxtQuantity").attr('value', ret.quantity);
                $("#itxtReplacement").attr('value', ret.replacement);
                $("#itxtUnitPrice").attr('value', ret.unitPrice);
                $("#itxtExtendedPrice").attr('value', ret.extendedPrice);
                $("#itxtSellEvent").attr('value', ret.sellEvent);
                $("#itxtEventos").attr('value', ret.eventos);
                $("#itxtSell").attr('value', ret.sell);

            }

            function visualizar(id) {

                if (!id) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + 'Por favor seleccione una fila');
                } else {
                    iniciarDialog();
                    leerDatos(id);
                    editarDialog(false);
                    $("#ihddTipo").attr('value', 'C');
                    $("#idDialogForm").dialog("open");
                }

            }


            function validarDatos() {

                var tipo = $("#ihddTipo").get(0).value;

                if (!(tipo == 'E' || tipo == 'N')) {
                    jAlert.error(mensaje + "El tipo de operación es incorrecto");
                    return false;
                }

                if (fc_IsNullOrEmpty($("#itxtFamilia").get(0))) {
                    jAlert.error(mensaje + "Ingrese la familia");
                    $("#itxtFamilia").focus();
                    return false;
                }

                if (fc_IsNullOrEmpty($("#itxtModeloBase").get(0))) {
                    jAlert.error(mensaje + "Ingrese el modelo base");
                    $("#itxtModeloBase").focus();
                    return false;
                }

                if (fc_IsNullOrEmpty($("#itxtModelo").get(0))) {
                    jAlert.error(mensaje + "Ingrese el modelo");
                    $("#itxtModelo").focus();
                    return false;
                }

                if (fc_IsNullOrEmpty($("#itxtPrefijo").get(0))) {
                    jAlert.error(mensaje + "Ingrese el prefijo");
                    $("#itxtPrefijo").focus();
                    return false;
                }


                if (!fc_IsNullOrEmpty($("#itxtCompQty").get(0))) {
                    if (isNaN($("#itxtCompQty").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese CompQty numerico");
                        $("#itxtCompQty").focus();
                        $("#itxtCompQty").select();
                        return false;
                    } 
                }

                if (!fc_IsNullOrEmpty($("#itxtFirstInterval").get(0))) {
                    if (isNaN($("#itxtFirstInterval").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese First Interval numerico");
                        $("#itxtFirstInterval").focus();
                        $("#itxtFirstInterval").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtNextInterval").get(0))) {
                    if (isNaN($("#itxtNextInterval").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Next Interval numerico");
                        $("#itxtNextInterval").focus();
                        $("#itxtNextInterval").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtQuantity").get(0))) {
                    if (isNaN($("#itxtQuantity").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Quantity numerico");
                        $("#itxtQuantity").focus();
                        $("#itxtQuantity").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtReplacement").get(0))) {
                    if (isNaN($("#itxtReplacement").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Replacement numerico");
                        $("#itxtReplacement").focus();
                        $("#itxtReplacement").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtUnitPrice").get(0))) {
                    if (isNaN($("#itxtUnitPrice").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Unit Price numerico");
                        $("#itxtUnitPrice").focus();
                        $("#itxtUnitPrice").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtExtendedPrice").get(0))) {
                    if (isNaN($("#itxtExtendedPrice").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Extended Price numerico");
                        $("#itxtExtendedPrice").focus();
                        $("#itxtExtendedPrice").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtSellEvent").get(0))) {
                    if (isNaN($("#itxtSellEvent").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Sell Event numerico");
                        $("#itxtSellEvent").focus();
                        $("#itxtSellEvent").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtEventos").get(0))) {
                    if (isNaN($("#itxtEventos").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Eventos numerico");
                        $("#itxtEventos").focus();
                        $("#itxtEventos").select();
                        return false;
                    }
                }

                if (!fc_IsNullOrEmpty($("#itxtSell").get(0))) {
                    if (isNaN($("#itxtSell").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese Sell numerico");
                        $("#itxtSell").focus();
                        $("#itxtSell").select();
                        return false;
                    }
                }

                return true;

            }

            $("#btnGrabar").click(function (evento) {

                if (!validarDatos()) {
                    return;
                }

                if (confirm('¿Seguro desea grabar?')) {
                    grabar();
                }

            });

            $("#exbtnGrabar").click(function (evento) {

                //                var tt = $("#exFileExportar").each();
                //get file input
                //                var $el = $('#exFileExportar');
                //                //set the next siblings (the span) text to the input value 
                //                $el.next().text($el.val());

                var file = $("#File1");
                var t = file.val();

                $.ajax({
                    url: frmName + "/Importar",
                    data: "{'ruta':''}",
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success")
                            alert(5);
                        else
                            jAlert.alert(JSON.parse(jsondata.responseText).Message);
                    }
                });

            });



            $("#exbtnCancelar").click(function (evento) {
                $("#idDialogExcel").dialog("close");
            });


            $("#lbnImportar").click(function (evento) {
                $("#idDialogExcel").dialog("open");
            });

            $("#btnCancelar").click(function (evento) {
                $("#idDialogForm").dialog("close");
            });

            $("#idDialogForm").dialog({
                autoOpen: false,
                height: 600,
                width: 550,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            $("#idDialogExcel").dialog({
                autoOpen: false,
                height: 180,
                width: 700,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            /***************** PREFIJO *******************************************************************************/

            function ejecutarConsulta(prefijo, modelo, modeloBase, familia, serviceCategory, Rodetail,
                                     compQty, firstInterval, nextInterval, jodetail, SOSPartNumber,
                                     SOSDescription, quantity, replacement, unitPrice,
                                     extendedPrice, sellEvent, eventos, sell) {

                var arreglo = null;
                var parametro = null;
                var sortColumn = $('#gvDetallePartes').getGridParam("sortname");
                var sortOrder = $('#gvDetallePartes').getGridParam("sortorder");

                arreglo = fc_redimencionarArray(21);
                arreglo[0][0] = 'prefijo';
                arreglo[0][1] = prefijo;
                arreglo[1][0] = 'modelo';
                arreglo[1][1] = modelo;
                arreglo[2][0] = 'modeloBase';
                arreglo[2][1] = modeloBase;
                arreglo[3][0] = 'familia';
                arreglo[3][1] = familia;
                arreglo[4][0] = 'serviceCategory';
                arreglo[4][1] = serviceCategory;
                arreglo[5][0] = 'Rodetail';
                arreglo[5][1] = Rodetail
                arreglo[6][0] = 'compQty';
                arreglo[6][1] = compQty;
                arreglo[7][0] = 'firstInterval';
                arreglo[7][1] = firstInterval;
                arreglo[8][0] = 'nextInterval';
                arreglo[8][1] = nextInterval;
                arreglo[9][0] = 'jodetail';
                arreglo[9][1] = jodetail;
                arreglo[10][0] = 'SOSPartNumber';
                arreglo[10][1] = SOSPartNumber;
                arreglo[11][0] = 'SOSDescription';
                arreglo[11][1] = SOSDescription;
                arreglo[12][0] = 'quantity';
                arreglo[12][1] = quantity;
                arreglo[13][0] = 'replacement';
                arreglo[13][1] = replacement
                arreglo[14][0] = 'unitPrice';
                arreglo[14][1] = unitPrice;
                arreglo[15][0] = 'extendedPrice';
                arreglo[15][1] = extendedPrice;
                arreglo[16][0] = 'sellEvent';
                arreglo[16][1] = sellEvent;
                arreglo[17][0] = 'eventos';
                arreglo[17][1] = eventos;
                arreglo[18][0] = 'sell';
                arreglo[18][1] = sell;
                arreglo[19][0] = 'sortColumn';
                arreglo[19][1] = sortColumn;
                arreglo[20][0] = 'sortOrder';
                arreglo[20][1] = sortOrder;
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    url: frmName + "/Consultar",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success")
                            jQuery("#gvDetallePartes")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                        else
                            jAlert.alert(JSON.parse(jsondata.responseText).Message);
                    }
                });

            }

            jQuery("#gvDetallePartes").jqGrid({

                datatype: function (o) {

                    var prefijo, modelo, modeloBase, familia, serviceCategory, Rodetail,
                        compQty, firstInterval, nextInterval, jodetail, SOSPartNumber,
                        SOSDescription, quantity, replacement, unitPrice, extendedPrice,
                        sellEvent, eventos, sell;

                    prefijo = modelo = modeloBase = familia = serviceCategory = Rodetail =
                        compQty = firstInterval = nextInterval = jodetail = SOSPartNumber =
                        SOSDescription = quantity = replacement = unitPrice = extendedPrice =
                        sellEvent = eventos = sell = '';

                    if (o.prefijo != null) prefijo = o.prefijo;
                    if (o.modelo != null) modelo = o.modelo;
                    if (o.modeloBase != null) modeloBase = o.modeloBase;
                    if (o.familia != null) familia = o.familia;
                    if (o.serviceCategory != null) serviceCategory = o.serviceCategory;
                    if (o.Rodetail != null) Rodetail = o.Rodetail;
                    if (o.compQty != null) compQty = o.compQty;
                    if (o.firstInterval != null) firstInterval = o.firstInterval;
                    if (o.nextInterval != null) nextInterval = o.nextInterval;
                    if (o.jodetail != null) jodetail = o.jodetail;
                    if (o.SOSPartNumber != null) SOSPartNumber = o.SOSPartNumber;
                    if (o.SOSDescription != null) SOSDescription = o.SOSDescription;
                    if (o.quantity != null) quantity = o.quantity;
                    if (o.replacement != null) replacement = o.replacement;
                    if (o.unitPrice != null) unitPrice = o.unitPrice;
                    if (o.extendedPrice != null) extendedPrice = o.extendedPrice;
                    if (o.sellEvent != null) sellEvent = o.sellEvent;
                    if (o.eventos != null) eventos = o.eventos;
                    if (o.sell != null) sell = o.sell;

                    ejecutarConsulta(prefijo, modelo, modeloBase, familia, serviceCategory, Rodetail,
                                     compQty, firstInterval, nextInterval, jodetail, SOSPartNumber,
                                     SOSDescription, quantity, replacement, unitPrice,
                                     extendedPrice, sellEvent, eventos, sell);


                },

                jsonReader: {
                    root: "Items",
                    page: "CurrentPage",
                    total: "PageCount",
                    records: "RecordCount",
                    repeatitems: true,
                    cell: "Row",
                    id: "ID"
                },
                height: 550,
                width: 1200,
                colNames: ['id', 'Prefijo', 'Modelo', 'Modelo Base', 'Familia', 'Service Category',
                           'RODETAIL', 'CompQty', 'First Interval', 'Next Interval', 'JODETAIL', 'SOS Part Number',
                           'SOS Description', 'Quantity', 'Replacement', 'UnitPrice', 'ExtendedPrice',
                           'Sell/Event', 'Eventos', 'Sell'
                          ],
                colModel: [
                                    { name: 'id', index: 'id', sorttype: "string", width: 100 },
                                    { name: 'prefijo', index: 'prefijo', sorttype: "string", width: 100 },
                                    { name: 'modelo', index: 'modelo', sorttype: "string", width: 100 },
                                    { name: 'modeloBase', index: 'modeloBase', sorttype: "string", width: 100 },
                                    { name: 'familia', index: 'familia', sorttype: "string", width: 300 },
                                    { name: 'serviceCategory', index: 'serviceCategory', sorttype: "string", width: 300 },
                                    { name: 'Rodetail', index: 'Rodetail', sorttype: "string", width: 300 },
                                    { name: 'compQty', index: 'compQty', sorttype: "float", width: 100, align: "right", stype: 'select', editoptions: { value: "0:Todos;1:1;2:2;5:5"} },
                                    { name: 'firstInterval', index: 'firstInterval', sorttype: "float", width: 100, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'nextInterval', index: 'nextInterval', sorttype: "float", width: 100, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'jodetail', index: 'jodetail', sorttype: "string", width: 300 },
                                    { name: 'SOSPartNumber', index: 'SOSPartNumber', sorttype: "string", width: 300 },
                                    { name: 'SOSDescription', index: 'SOSDescription', sorttype: "string", width: 300 },
                                    { name: 'quantity', index: 'quantity', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;1:1;2:2;5:5"} },
                                    { name: 'replacement', index: 'replacement', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'unitPrice', index: 'unitPrice', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'extendedPrice', index: 'extendedPrice', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'sellEvent', index: 'sellEvent', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'eventos', index: 'eventos', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
               		                { name: 'sell', index: 'sell', sorttype: "float", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} }
               	          ],
                rowNum: 5000,
                loadtext: 'Cargando datos...',
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: false,
                sortname: "id",
                sortorder: "asc",
                grouping: true,
                caption: "Listado de Detalle Partes",
                mtype: "POST",
                gridview: true,
                toolbar: [true, "top"],
                ondblClickRow: function (rowId) { visualizar(rowId); },
                groupingView: {
                    groupField: ['prefijo'],
                    groupColumnShow: [true],
                    groupText: ['<b>{0} - {1} Item(s)</b>'],
                    groupCollapse: true,
                    groupOrder: ['asc'],
                    groupSummary: [false],
                    groupDataSorted: true
                }
            });
            /* ----------------------------------------------------------------------------------------------------------- */
            jQuery("#gvDetallePartes").jqGrid('hideCol', ["id"]);
            /* ----------------------------------------------------------------------------------------------------------- */
            jQuery("#gvDetallePartes").jqGrid('filterToolbar');
            /* ----------------------------------------------------------------------------------------------------------- */
            $("#t_gvDetallePartes").append("<table><tr><td style='vertical-align: middle'><a id='lbnNuevo'>Nuevo</a> | <a id='lbnEditar'>Editar</a> | <a id='lbnEliminar'>Eliminar</a></td></tr></table>");
            $("a", "#t_gvDetallePartes").click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lbnNuevo':
                        nuevo();
                        break;
                    case 'lbnEditar':
                        editar();
                        break;
                    case 'lbnEliminar':
                        eliminar();
                        break;
                }
            });



            /*--------------------------------------------------------*/
            $("#itxtFamilia").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtFamilia").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtModeloBase").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtModeloBase").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtModelo").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtModelo").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtPrefijo").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtPrefijo").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtServiceCategory").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtServiceCategory").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtRodetail").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtRodetail").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtCompQty").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtCompQty").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtFirstInterval").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtFirstInterval").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtNextInterval").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtNextInterval").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtJodetail").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtJodetail").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOSPartNumber").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOSPartNumber").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOSDescription").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOSDescription").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtQuantity").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtQuantity").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtReplacement").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtReplacement").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtUnitPrice").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtUnitPrice").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtExtendedPrice").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtExtendedPrice").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSellEvent").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSellEvent").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventos").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventos").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSell").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSell").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frmDetallePartes" action="#">
    <table width="100%">
        <tr>
            <td>
                <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td class="TextoTitulo">
                            Detalle Partes
                        </td>
                        <td align="right">
                            <div id="submenu">
                                <p>
                                    <a id="lbnImportar">Importar</a>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <table id="gvDetallePartes">
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="idDialogForm" title="Registro de Detalle Partes" style="display: none">
        <input id="ihddTipo" type="hidden" />
        <input id="ihddIdDetallePartes" type="hidden" />
        <fieldset style="width: 500px">
            <legend style="text-align: left" class="TDTexto">Datos Generales</legend>
            <table width="500px">
                <tr>
                    <td align="left" class="Texto" style="width: 150px">
                        Familia :
                    </td>
                    <td align="left" style="width: 450px">
                        <input id="itxtFamilia" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Modelo Base :
                    </td>
                    <td align="left">
                        <input id="itxtModeloBase" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Modelo :
                    </td>
                    <td align="left">
                        <input id="itxtModelo" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Prefijo :
                    </td>
                    <td align="left">
                        <input id="itxtPrefijo" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Service Category :
                    </td>
                    <td align="left">
                        <input id="itxtServiceCategory" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        RODETAIL :
                    </td>
                    <td align="left">
                        <input id="itxtRodetail" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        CompQty :
                    </td>
                    <td align="left">
                        <input id="itxtCompQty" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        First Interval :
                    </td>
                    <td align="left">
                        <input id="itxtFirstInterval" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Next Interval :
                    </td>
                    <td align="left">
                        <input id="itxtNextInterval" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        JODETAIL :
                    </td>
                    <td align="left">
                        <input id="itxtJodetail" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        SOS PartNumber :
                    </td>
                    <td>
                        <input id="itxtSOSPartNumber" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        SOS Description :
                    </td>
                    <td>
                        <input id="itxtSOSDescription" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Quantity :
                    </td>
                    <td>
                        <input id="itxtQuantity" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Replacement :
                    </td>
                    <td>
                        <input id="itxtReplacement" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        UNITPRICE :
                    </td>
                    <td>
                        <input id="itxtUnitPrice" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Extended Price:
                    </td>
                    <td>
                        <input id="itxtExtendedPrice" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Sell/Event :
                    </td>
                    <td>
                        <input id="itxtSellEvent" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Eventos :
                    </td>
                    <td>
                        <input id="itxtEventos" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Sell :
                    </td>
                    <td>
                        <input id="itxtSell" type="text" class="Texto" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="100%">
            <tr style="text-align: right; vertical-align: middle">
                <td align="right">
                    <a id="btnGrabar" title="Grabar">Grabar</a>
                </td>
                <td align="left" style="width: 90px">
                    <a id="btnCancelar" title="Cancelar">Cancelar</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="idDialogExcel" title="Importar Tarifas" style="display: none">
        <fieldset style="width: 650px">
            <legend style="text-align: left" class="TDTexto">IMPORTAR</legend>
            <table width="650px">
                <tr>
                    <td>
                        <label class="Texto">
                            Seleccione el Excel a Importar :
                        </label>
                    </td>
                    <td>
                        <input id="File1" type="file" style="width: 400px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="exchkEncabezado" runat="server" CssClass="Texto" Text='El archivo tiene encabezado de columnas'
                            Checked="True" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="100%">
            <tr style="text-align: right; vertical-align: middle">
                <td align="right">
                    <a id="exbtnGrabar" title="Grabar">Grabar</a>
                </td>
                <td align="left" style="width: 90px">
                    <a id="exbtnCancelar" title="Cancelar">Cancelar</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
