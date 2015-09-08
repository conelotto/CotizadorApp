<%@ Page Title="Tarifas" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmTarifas.aspx.vb" Inherits="Cotizador.frmAdmTarifas" %>

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
    <script src="../../Scripts/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script type="text/javascript">

        function LimpiarUrlArchivo() {
            var NombreInicial = $('#hdfNombreArchivoInicial').val();
            $('#lklNombreArchivo').html(NombreInicial);
            $('#hdfCargarArchivo').val('NO');
            $('#imgCerrar').css('visibility', 'hidden');
        }

        function GenerarNombreArchivo(NombreArchivo) {

            $('#lklNombreArchivo').html("Excel_Tarifa.xls");
            $('#imgCerrar').css('visibility', 'visible');

        }

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

            $("#itxtFamilia").css("width", "260");
            $("#itxtFamilia").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtModeloBase").css("width", "260");
            $("#itxtModeloBase").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxtModelo").css("width", "260");
            $("#itxtModelo").maxlength({ maxCharacters: 100, status: false, showAlert: false });
            $("#itxPrefijo").css("width", "260");
            $("#itxPrefijo").maxlength({ maxCharacters: 3, status: false, showAlert: false });
            $("#iddlPlanMantenimiento").css("width", "260");
            $("#iddlEvento").css("width", "90");
            $("#itxtKitRepuestos").css("width", "90");
            $("#itxtKitRepuestos").numeric({ allow: '.' });
            $("#itxtKitRepuestos").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtFluidos").css("width", "90");
            $("#itxtFluidos").numeric({ allow: '.' });
            $("#itxtFluidos").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtServicioContratado").css("width", "90");
            $("#itxtServicioContratado").numeric({ allow: '.' });
            $("#itxtServicioContratado").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtSOS").css("width", "90");
            $("#itxtSOS").numeric({ allow: '.' });
            $("#itxtSOS").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtTotal").css("width", "90");
            $("#itxtEventoNuevos").css("width", "90");
            $("#itxtEventoNuevos").numeric({ allow: '.' });
            $("#itxtEventoNuevos").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtEventoUsados").css("width", "90");
            $("#itxtEventoUsados").numeric({ allow: '.' });
            $("#itxtEventoUsados").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtTarifaUSD").css("width", "90");
            $("#itxtTarifaUSD").numeric({ allow: '.' });
            $("#itxtTarifaUSD").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtKitRepuestosT").css("width", "90");
            $("#itxtKitRepuestosT").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtFluidosT").css("width", "90");
            $("#itxtFluidosT").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtServicioContratadoT").css("width", "90");
            $("#itxtServicioContratadoT").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtTotalT").css("width", "90");
            $("#itxtTotalT").maxlength({ maxCharacters: 15, status: false, showAlert: false });
            $("#itxtTotal").css('background', '#ffff66');
            $("#itxtKitRepuestosT").css('background', '#ffff66');
            $("#itxtFluidosT").css('background', '#ffff66');
            $("#itxtServicioContratadoT").css('background', '#ffff66');
            $("#itxtTotalT").css('background', '#ffff66');
            // ------------------------------------------------------------------------------------------

            $("#File1").uploadify({
                'swf': '../../Styles/uploadify.swf',
                'uploader': '../frmCargarExcel.aspx',
                'cancelImg': '../../Styles/uploadify-cancel.png',
                'displayData': 'percentage',
                'fileTypeExts': '*.xls;*.xlsx', //'*.*',
                'fileTypeDesc': 'Todos los Archivos',
                'buttonText': 'cargar',
                'auto': true,
                'multi': false,
                'wmode': 'transparent',
                'removeTimeout': 0.5,
                'hideButton': true,
                'method': 'get',
                'height': 15,
                'width': 80,
                'onUploadStart': function (file) {
                    GenerarNombreArchivo(file.name);
                },
                'onUploadSuccess': function (file, data, response) {
                    $('#hdfCargarArchivo').val('SI');
                },
                'onCancel': function (file) {
                    //alert('The file ' + file.name + ' was cancelled.');
                    var NombreArchivoInicial = $('#hdfNombreArchivoInicial').val();
                    $('#lklNombreArchivo').html(NombreArchivoInicial);
                    $('#hdfCargarArchivo').val('NO');
                }
            });

            function nuevo() {
                iniciarDialog();
                editarDialog(true);
                $("#ihddTipo").attr('value', 'N');
                $("#idDialogForm").dialog("open");
            }

            function editar() {

                var id = jQuery("#gvPrefijo").jqGrid('getGridParam', 'selrow');
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

                var id = jQuery("#gvPrefijo").jqGrid('getGridParam', 'selrow');

                if (!id) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + "Por favor seleccione una fila");
                } else {
                    if (!(confirm('¿Seguro desea eliminar la tarifa?'))) {
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

                var conFluidos = null;
                var arreglo = null;
                var parametro = null;
                var tipo = $("#ihddTipo").get(0).value;
                var id = $("#ihdnIdTarifa").get(0).value;
                var familia = $("#itxtFamilia").get(0).value;
                var modeloBase = $("#itxtModeloBase").get(0).value;
                var modelo = $("#itxtModelo").get(0).value;
                var prefijo = $("#itxPrefijo").get(0).value;
                var codigoPlan = $('#iddlPlanMantenimiento :selected').val();
                var plan = $('#iddlPlanMantenimiento :selected').html();
                var evento = $("#iddlEvento").val();
                var kitRepuesto = $("#itxtKitRepuestos").get(0).value;
                var fluidos = $("#itxtFluidos").get(0).value;
                var servicioContratado = $("#itxtServicioContratado").get(0).value;
                var sos = $("#itxtSOS").get(0).value;
                var eventoNuevos = $("#itxtEventoNuevos").get(0).value;
                var eventoUsados = $("#itxtEventoUsados").get(0).value;
                var tarifaUSD = $("#itxtTarifaUSD").get(0).value;

                if ($('input:radio[id=irdnSi]:checked').val()) {
                    conFluidos = 'SI';
                } else {
                    conFluidos = 'NO';
                }

                arreglo = fc_redimencionarArray(18)
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
                arreglo[6][0] = 'codigoPlan';
                arreglo[6][1] = codigoPlan;
                arreglo[7][0] = 'plan';
                arreglo[7][1] = plan;
                arreglo[8][0] = 'evento';
                arreglo[8][1] = evento;
                arreglo[9][0] = 'kitRepuestos';
                arreglo[9][1] = kitRepuesto;
                arreglo[10][0] = 'conFluidos';
                arreglo[10][1] = conFluidos;
                arreglo[11][0] = 'fluidos';
                arreglo[11][1] = fluidos;
                arreglo[12][0] = 'servicioContratado';
                arreglo[12][1] = servicioContratado;
                arreglo[13][0] = 'SOS';
                arreglo[13][1] = sos;
                arreglo[14][0] = 'eventosNueva';
                arreglo[14][1] = eventoNuevos;
                arreglo[15][0] = 'eventosUsada';
                arreglo[15][1] = eventoUsados;
                arreglo[16][0] = 'tarifaUSDxH';
                arreglo[16][1] = tarifaUSD;
                arreglo[17][0] = 'usuario';
                arreglo[17][1] = usuario;

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

                $("#iddlPlanMantenimiento").attr('disabled', rpt);
                $("#iddlEvento").attr('disabled', rpt);
                $("#irdnSi").attr('disabled', rpt);
                $("#irdnNo").attr('disabled', rpt);

                if (!rpt) {
                    $("#btnGrabar").css('display', 'block');
                } else {
                    $("#btnGrabar").css('display', 'none');
                }

            }

            function changeFluidos() {
                if ($('input:radio[id=irdnSi]:checked').val()) {
                    $("#idFluidos").css('display', 'block');
                    $("#idFluidosT").css('display', 'block');
                } else {
                    $("#idFluidos").css('display', 'none');
                    $("#idFluidosT").css('display', 'none');
                }
            }

            function iniciarDialog() {

                $("#ihddTipo").attr('value', '');
                $("#ihdnIdTarifa").attr('value', '');
                $("#itxtFamilia").attr('value', '');
                $("#itxtModeloBase").attr('value', '');
                $("#itxtModelo").attr('value', '');
                $("#itxPrefijo").attr('value', '');
                $("#itxtKitRepuestos").attr('value', '');
                $("#itxtFluidos").attr('value', '');
                $("#itxtServicioContratado").attr('value', '');
                $("#itxtSOS").attr('value', '');
                $("#itxtTotal").attr('value', '');
                $("#itxtEventoNuevos").attr('value', '');
                $("#itxtEventoUsados").attr('value', '');
                $("#itxtTarifaUSD").attr('value', '');
                $("#itxtKitRepuestosT").attr('value', '');
                $("#itxtFluidosT").attr('value', '');
                $("#itxtServicioContratadoT").attr('value', '');
                $("#itxtTotalT").attr('value', '');

                $("#irdnSi").attr('checked', true);
                $("#irdnNo").attr('checked', false);
                $("#idFluidos").css('display', 'block');
                $("#idFluidosT").css('display', 'block');

                $("#iddlPlanMantenimiento").val('');
                $("#iddlEvento").val('');

            }


            function leerDatos(id) {

                var ret = jQuery("#gvPrefijo").jqGrid('getRowData', id);

                $("#ihdnIdTarifa").attr('value', ret.id);
                $("#itxtFamilia").attr('value', ret.familia);
                $("#itxtModeloBase").attr('value', ret.modeloBase);
                $("#itxtModelo").attr('value', ret.modelo);
                $("#itxPrefijo").attr('value', ret.prefijo);
                $("#iddlPlanMantenimiento").val(ret.codigoPlan);
                $("#iddlEvento").val(ret.evento);
                $("#itxtKitRepuestos").attr('value', ret.kitRepuestos);
                $("#itxtFluidos").attr('value', ret.fluidos);
                $("#itxtServicioContratado").attr('value', ret.servicioContratado);
                $("#itxtSOS").attr('value', ret.SOS);
                $("#itxtTotal").attr('value', ret.total);
                $("#itxtEventoNuevos").attr('value', ret.eventosNueva);
                $("#itxtEventoUsados").attr('value', ret.eventosUsada);
                $("#itxtTarifaUSD").attr('value', ret.tarifaUSDxH);
                $("#itxtKitRepuestosT").attr('value', ret.kitRepuestosT);
                $("#itxtFluidosT").attr('value', ret.fluidosT);
                $("#itxtServicioContratadoT").attr('value', ret.servicioContratadoT);
                $("#itxtTotalT").attr('value', ret.totalT);

                if (ret.conFluidos == 'Con Fluidos') {
                    $("#irdnSi").attr('checked', true);
                    $("#irdnNo").attr('checked', false);
                    $("#idFluidos").css('display', 'block');
                    $("#idFluidosT").css('display', 'block');

                } else {
                    $("#irdnNo").attr('checked', true);
                    $("#irdnSi").attr('checked', false);
                    $("#idFluidos").css('display', 'none');
                    $("#idFluidosT").css('display', 'none');
                }

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

            $.ajax({
                type: "POST",
                url: frmName + "/ConsultarPlanes",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data.d, function () {
                        $("#iddlPlanMantenimiento").append($("<option></option>").val(this['id']).html(this['plan']));
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

            $.ajax({
                type: "POST",
                url: frmName + "/ConsultarEventos",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data.d, function () {
                        $("#iddlEvento").append($("<option></option>").val(this['id']).html(this['evento']));
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });



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

                if (fc_IsNullOrEmpty($("#itxPrefijo").get(0))) {
                    jAlert.error(mensaje + "Ingrese el prefijo");
                    $("#itxPrefijo").focus();
                    return false;
                }

                if ($("#iddlPlanMantenimiento").val() == '') {
                    jAlert.error(mensaje + "Ingrese el plan de mantenimiento");
                    $("#iddlPlanMantenimiento").focus();
                    return false;
                }

                if ($("#iddlEvento").val() == '') {
                    jAlert.error(mensaje + "Ingrese el evento");
                    $("#iddlEvento").focus();
                    return false;
                }

                if (fc_IsNullOrEmpty($("#itxtKitRepuestos").get(0))) {
                    jAlert.error(mensaje + "Ingrese kit de repuestos");
                    $("#itxtKitRepuestos").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtKitRepuestos").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese kit de repuestos");
                        $("#itxtKitRepuestos").focus();
                        $("#itxtKitRepuestos").select();
                        return false;
                    }
                }

                if ($('input:radio[id=irdnSi]:checked').val()) {
                    if (fc_IsNullOrEmpty($("#itxtFluidos").get(0))) {
                        jAlert.error(mensaje + "Ingrese los fluidos");
                        $("#itxtFluidos").focus();
                        return false;
                    } else {
                        if (isNaN($("#itxtFluidos").get(0).value)) {
                            jAlert.error(mensaje + "Ingrese los fluidos");
                            $("#itxtFluidos").focus();
                            $("#itxtFluidos").select();
                            return false;
                        }
                    }
                }

                if (fc_IsNullOrEmpty($("#itxtServicioContratado").get(0))) {
                    jAlert.error(mensaje + "Ingrese servicio contratado");
                    $("#itxtServicioContratado").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtServicioContratado").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese servicio contratado");
                        $("#itxtServicioContratado").focus();
                        $("#itxtServicioContratado").select();
                        return false;
                    }
                }

                if (fc_IsNullOrEmpty($("#itxtSOS").get(0))) {
                    jAlert.error(mensaje + "Ingrese SOS");
                    $("#itxtSOS").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtSOS").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese SOS");
                        $("#itxtSOS").focus();
                        $("#itxtSOS").select();
                        return false;
                    }
                }

                if (fc_IsNullOrEmpty($("#itxtEventoNuevos").get(0))) {
                    jAlert.error(mensaje + "Ingrese eventos nuevos");
                    $("#itxtEventoNuevos").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtEventoNuevos").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese eventos nuevos");
                        $("#itxtEventoNuevos").focus();
                        $("#itxtEventoNuevos").select();
                        return false;
                    } else {
                        if (isNaN($("#itxtEventoNuevos").get(0).value > 0)) {
                            jAlert.error(mensaje + "Eventos Nuevos debe ser mayor a cero");
                            $("#itxtEventoNuevos").focus();
                            $("#itxtEventoNuevos").select();
                            return false;
                        }
                    }
                }

                if (fc_IsNullOrEmpty($("#itxtEventoUsados").get(0))) {
                    jAlert.error(mensaje + "Ingrese eventos usados");
                    $("#itxtEventoUsados").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtEventoUsados").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese eventos usados");
                        $("#itxtEventoUsados").focus();
                        $("#itxtEventoUsados").select();
                        return false;
                    }
                }

                if (fc_IsNullOrEmpty($("#itxtTarifaUSD").get(0))) {
                    jAlert.error(mensaje + "Ingrese tarifa USD/H");
                    $("#itxtTarifaUSD").focus();
                    return false;
                } else {
                    if (isNaN($("#itxtTarifaUSD").get(0).value)) {
                        jAlert.error(mensaje + "Ingrese tarifa USD/H");
                        $("#itxtTarifaUSD").focus();
                        $("#itxtTarifaUSD").select();
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

                //                fc_parametrosData

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = 'ruta';
                arreglo[0][1] = t;

                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    url: frmName + "/Importar",
                    data: parametro,
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
                height: 220,
                width: 780,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            /***************** PREFIJO *******************************************************************************/

            function ejecutarConsulta(prefijo, modelo, modeloBase, familia, plan, evento,
                                      kitRepuestos, fluidos, servicioContratado, SOS, total,
                                      eventosNueva, eventosUsada, kitRepuestosT, fluidosT,
                                      servicioContratadoT, totalT, tarifaUSDxH) {

                var arreglo = null;
                var parametro = null;
                var sortColumn = $('#gvPrefijo').getGridParam("sortname");
                var sortOrder = $('#gvPrefijo').getGridParam("sortorder");

                arreglo = fc_redimencionarArray(20);
                arreglo[0][0] = 'prefijo';
                arreglo[0][1] = prefijo;
                arreglo[1][0] = 'modelo';
                arreglo[1][1] = modelo;
                arreglo[2][0] = 'modeloBase';
                arreglo[2][1] = modeloBase;
                arreglo[3][0] = 'familia';
                arreglo[3][1] = familia;
                arreglo[4][0] = 'plan';
                arreglo[4][1] = plan;
                arreglo[5][0] = 'evento';
                arreglo[5][1] = evento
                arreglo[6][0] = 'kitRepuestos';
                arreglo[6][1] = kitRepuestos;
                arreglo[7][0] = 'fluidos';
                arreglo[7][1] = fluidos;
                arreglo[8][0] = 'servicioContratado';
                arreglo[8][1] = servicioContratado;
                arreglo[9][0] = 'SOS';
                arreglo[9][1] = SOS;
                arreglo[10][0] = 'total';
                arreglo[10][1] = total;
                arreglo[11][0] = 'eventosNueva';
                arreglo[11][1] = eventosNueva;
                arreglo[12][0] = 'eventosUsada';
                arreglo[12][1] = eventosUsada;
                arreglo[13][0] = 'kitRepuestosT';
                arreglo[13][1] = kitRepuestosT
                arreglo[14][0] = 'fluidosT';
                arreglo[14][1] = fluidosT;
                arreglo[15][0] = 'servicioContratadoT';
                arreglo[15][1] = servicioContratadoT;
                arreglo[16][0] = 'totalT';
                arreglo[16][1] = totalT;
                arreglo[17][0] = 'tarifaUSDxH';
                arreglo[17][1] = tarifaUSDxH;
                arreglo[18][0] = 'sortColumn';
                arreglo[18][1] = sortColumn;
                arreglo[19][0] = 'sortOrder';
                arreglo[19][1] = sortOrder;
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    url: frmName + "/Consultar",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success")
                            jQuery("#gvPrefijo")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                        else
                            jAlert.alert(JSON.parse(jsondata.responseText).Message);
                    }
                });

            }

            var valwidth = $(window).width();

            jQuery("#gvPrefijo").jqGrid({

                datatype: function (o) {

                    var prefijo, modelo, modeloBase, familia, plan, evento,
                        kitRepuestos, fluidos, servicioContratado, SOS, total,
                        eventosNueva, eventosUsada, kitRepuestosT, fluidosT,
                        servicioContratadoT, totalT, tarifaUSDxH;

                    prefijo = modelo = modeloBase = familia = plan = evento =
                        kitRepuestos = fluidos = servicioContratado = SOS = total =
                        eventosNueva = eventosUsada = kitRepuestosT = fluidosT =
                        servicioContratadoT = totalT = tarifaUSDxH = '';

                    if (o.prefijo != null) prefijo = o.prefijo;
                    if (o.modelo != null) modelo = o.modelo;
                    if (o.modeloBase != null) modeloBase = o.modeloBase;
                    if (o.familia != null) familia = o.familia;
                    if (o.plan != null) plan = o.plan;
                    if (o.evento != null) evento = o.evento;
                    if (o.kitRepuestos != null) kitRepuestos = o.kitRepuestos;
                    if (o.fluidos != null) fluidos = o.fluidos;
                    if (o.servicioContratado != null) servicioContratado = o.servicioContratado;
                    if (o.SOS != null) SOS = o.SOS;
                    if (o.total != null) total = o.total;
                    if (o.eventosNueva != null) eventosNueva = o.eventosNueva;
                    if (o.eventosUsada != null) eventosUsada = o.eventosUsada;
                    if (o.kitRepuestosT != null) kitRepuestosT = o.kitRepuestosT;
                    if (o.fluidosT != null) fluidosT = o.fluidosT;
                    if (o.servicioContratadoT != null) servicioContratadoT = o.servicioContratadoT;
                    if (o.totalT != null) totalT = o.totalT;
                    if (o.tarifaUSDxH != null) tarifaUSDxH = o.tarifaUSDxH;

                    ejecutarConsulta(prefijo, modelo, modeloBase, familia, plan, evento,
                                     kitRepuestos, fluidos, servicioContratado, SOS, total,
                                     eventosNueva, eventosUsada, kitRepuestosT, fluidosT,
                                     servicioContratadoT, totalT, tarifaUSDxH);

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
                width: (valwidth * 0.95),
                colNames: ['id', 'Prefijo', 'Modelo', 'Modelo Base', 'Familia', 'codigoPlan',
                           'Plan', 'Evento', 'conFluidos', 'aceites', 'Kit Repuestos', 'Fluidos',
                           'Servicio Contratado', 'SOS', 'Total', 'Eventos Nueva', 'Eventos Usada',
                           'Kit Repuestos T', 'Fluidos T', 'Servicio Contratado T', 'Total T', 'Tarifa USDxH'
                          ],
                colModel: [
                                    { name: 'id', index: 'id', sorttype: "string", width: 100 },
                                    { name: 'prefijo', index: 'prefijo', sorttype: "string", width: 100 },
                                    { name: 'modelo', index: 'modelo', sorttype: "string", width: 100 },
                                    { name: 'modeloBase', index: 'modeloBase', sorttype: "string", width: 100 },
                                    { name: 'familia', index: 'familia', sorttype: "string", width: 200 },
                                    { name: 'codigoPlan', index: 'codigoPlan', sorttype: "string", width: 100 },
                                    { name: 'plan', index: 'plan', sorttype: "string", width: 300 },
                                    { name: 'evento', index: 'evento', sorttype: "string", width: 100 },
                                    { name: 'conFluidos', index: 'conFluidos', sorttype: "string", width: 100 },
                                    { name: 'aceites', index: 'aceites', sorttype: "string", width: 100 },
                                    { name: 'kitRepuestos', index: 'kitRepuestos', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'fluidos', index: 'fluidos', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'servicioContratado', index: 'servicioContratado', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'SOS', index: 'SOS', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'total', index: 'total', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'eventosNueva', index: 'eventosNueva', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;1:1;2:2;5:5"} },
                                    { name: 'eventosUsada', index: 'eventosUsada', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;1:1;2:2;5:5"} },
                                    { name: 'kitRepuestosT', index: 'kitRepuestosT', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'fluidosT', index: 'fluidosT', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'servicioContratadoT', index: 'servicioContratadoT', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
                                    { name: 'totalT', index: 'totalT', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} },
               		                { name: 'tarifaUSDxH', index: 'tarifaUSDxH', sorttype: "string", width: 140, align: "right", stype: 'select', editoptions: { value: "0:Todos;100:100.00;200:200.00;500:500.00;1000:1000.00"} }
               	          ],
                rowNum: 5000,
                loadtext: 'Cargando datos...',
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: false,
                sortname: "id",
                sortorder: "asc",
                grouping: true,
                caption: "Listado de Tarifas",
                mtype: "POST",
                gridview: true,
                toolbar: [false, "top"],// asignar True para ver el toolbar
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
            jQuery("#gvPrefijo").jqGrid('hideCol', ["id", "codigoPlan", "conFluidos", "aceites"]);
            /* ----------------------------------------------------------------------------------------------------------- */            
            jQuery("#gvPrefijo").jqGrid('filterToolbar');
           /* ----------------------------------------------------------------------------------------------------------- */
//            //ACTIVAR PARA VER LA BARRA DE TITULO DE LA GRILLA
//            $("#t_gvPrefijo").append("<table><tr><td style='vertical-align: middle'><a id='lbnNuevo'>Nuevo</a> | <a id='lbnEditar'>Editar</a> | <a id='lbnEliminar'>Eliminar</a></td></tr></table>");

//            $("a", "#t_gvPrefijo").click(function (event) {
//                var control = event.target.id;
//                switch (control) {
//                    case 'lbnNuevo':
//                        nuevo();
//                        break;
//                    case 'lbnEditar':
//                        editar();
//                        break;
//                    case 'lbnEliminar':
//                        eliminar();
//                        break;
//                }
//            });

            $("#irdnSi").change(function (event) {
                changeFluidos();
            });
            $("#irdnNo").change(function (event) {
                changeFluidos();
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
            $("#itxPrefijo").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxPrefijo").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtKitRepuestos").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtKitRepuestos").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtFluidos").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtFluidos").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtServicioContratado").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtServicioContratado").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOS").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtSOS").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventoNuevos").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventoNuevos").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventoUsados").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtEventoUsados").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtTarifaUSD").keypress(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtTarifaUSD").keydown(function (evento) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return false;
                }
            });
            $("#itxtTotal").keypress(function (evento) {
                return false;
            });
            $("#itxtTotal").keydown(function (evento) {
                return false;
            });
            $("#itxtKitRepuestosT").keypress(function (evento) {
                return false;
            });
            $("#itxtKitRepuestosT").keydown(function (evento) {
                return false;
            });
            $("#itxtFluidosT").keypress(function (evento) {
                return false;
            });
            $("#itxtFluidosT").keydown(function (evento) {
                return false;
            });
            $("#itxtServicioContratadoT").keypress(function (evento) {
                return false;
            });
            $("#itxtServicioContratadoT").keydown(function (evento) {
                return false;
            });
            $("#itxtTotalT").keypress(function (evento) {
                return false;
            });
            $("#itxtTotalT").keydown(function (evento) {
                return false;
            });

        });
        // Redimensionar Grillas
        $(window).bind('resize', function () {
            var valwidth = $(window).width();
            $('#gvPrefijo').setGridWidth(valwidth * 0.95);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frmTarifa" action="#">
    <table width="100%">
        <tr>
            <td>
                <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td class="TextoTitulo">
                            Tarifas
                        </td>
                        <td align="right">
                            <div id="submenu">
                                <p>
                                    <a id="lbnImportar" style="visibility:hidden">Importar</a>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <table id="gvPrefijo">
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="idDialogForm" title="Registro de Tarifa" style="display: none">
        <input id="ihddTipo" type="hidden" />
        <input id="ihdnIdTarifa" type="hidden" />
        <fieldset style="width: 500px">
            <legend style="text-align: left" class="TDTexto">Datos Generales</legend>
            <table width="500px">
                <tr>
                    <td align="left" class="Texto" style="width: 150px">
                        Familia :
                    </td>
                    <td align="left" style="width: 450px">
                        <input id="itxtFamilia" name="itxtFamilia" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Modelo Base :
                    </td>
                    <td align="left">
                        <input id="itxtModeloBase" name="itxtModeloBase" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Modelo :
                    </td>
                    <td align="left">
                        <input id="itxtModelo" name="itxtModelo" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Prefijo :
                    </td>
                    <td align="left">
                        <input id="itxPrefijo" name="itxPrefijo" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Plan de Mantenimiento :
                    </td>
                    <td align="left">
                        <select id="iddlPlanMantenimiento" name="iddlPlanMantenimiento" class="ArialFrm">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Evento :
                    </td>
                    <td align="left">
                        <select id="iddlEvento" name="iddlEvento" class="ArialFrm">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Con Fluidos :
                    </td>
                    <td align="left">
                        <input id="irdnSi" name="iRadio" type="radio" /><label for="irdnSi">Si</label>
                        <input id="irdnNo" name="iRadio" type="radio" /><label for="irdnNo">No</label>
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Kit de Repuestos :
                    </td>
                    <td align="left">
                        <input id="itxtKitRepuestos" name="itxtKitRepuestos" type="text" class="Texto" />
                    </td>
                </tr>
                <tr id="idFluidos">
                    <td class="Texto">
                        Fluidos :
                    </td>
                    <td align="left">
                        <input id="itxtFluidos" name="itxtFluidos" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Servicio Contratado :
                    </td>
                    <td align="left">
                        <input id="itxtServicioContratado" name="itxtServicioContratado" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        SOS :
                    </td>
                    <td align="left">
                        <input id="itxtSOS" name="itxtSOS" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Total :
                    </td>
                    <td align="left">
                        <input id="itxtTotal" name="itxtTotal" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Eventos Nuevos :
                    </td>
                    <td align="left">
                        <input id="itxtEventoNuevos" name="itxtEventoNuevos" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Eventos Usados :
                    </td>
                    <td align="left">
                        <input id="itxtEventoUsados" name="itxtEventoUsados" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Tarifa USD/H :
                    </td>
                    <td align="left">
                        <input id="itxtTarifaUSD" name="itxtTarifaUSD" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Kit de Repuestos T :
                    </td>
                    <td align="left">
                        <input id="itxtKitRepuestosT" name="itxtKitRepuestosT" type="text" class="Texto" />
                    </td>
                </tr>
                <tr id="idFluidosT">
                    <td class="Texto">
                        Fluidos T :
                    </td>
                    <td align="left">
                        <input id="itxtFluidosT" name="itxtFluidosT" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Servicio Contratado T :
                    </td>
                    <td align="left">
                        <input id="itxtServicioContratadoT" name="itxtServicioContratadoT" type="text" class="Texto" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Total T :
                    </td>
                    <td align="left">
                        <input id="itxtTotalT" name="itxtTotalT" type="text" class="Texto" />
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
                        <input id="File1" type="file" style="width: 460px" />
                        <a id="lklNombreArchivo" style="color: #0000FF"></a>
                        <img alt="" src="../../Images/ActionDelete.gif" id="imgCerrar" style="visibility: hidden"
                            onclick="LimpiarUrlArchivo();return false;" />
                        <input type="hidden" id="hdfCargarArchivo" />
                        <input type="hidden" id="hdfNombreArchivoInicial" />
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
