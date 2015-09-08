<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SapSite.Master"
    CodeBehind="frmCotizacionRegistro.aspx.vb" Inherits="Cotizador.frmCotizacionRegistro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
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
    <script src="../../Scripts/jQuery-1.8.0/grid.subgrid.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.alphaNumeric.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/jquery.ui.datepicker-es.js" type="text/javascript"></script>
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jQuery-1.8.0/jquery.alert.js" type="text/javascript"></script>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.timers.js" type="text/javascript"></script>
    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script type="text/javascript">

//    $(window).focus(function(e) {
//        alert('evento focus');
//    });

//    $(window).blur(function(e) {
//        alert('evento blur');
//    });
  
          function cambiarPosicion(Tipo, IdSeccion) {

            var indice = null;
            var getRowData = jQuery("#gvVerLista").jqGrid('getRowData');
            var rowDataPrev, rowData, rowDataNext;

            for (var i = 0; i < getRowData.length; i++) {
                if (getRowData[i].IdSeccion == IdSeccion) {
                    indice = i;
                    break;
                }
            }

            if (indice == null) return;

            rowData = getRowData[indice];

            if (Tipo == 1) {
                rowDataPrev = getRowData[indice - 1];
                jQuery("#gvVerLista").jqGrid('setRowData', indice, rowData);
                jQuery("#gvVerLista").jqGrid('setRowData', indice + 1, rowDataPrev);
            }
            if (Tipo == 2) {
                rowDataNext = getRowData[indice + 1];
                jQuery("#gvVerLista").jqGrid('setRowData', indice + 1, rowDataNext);
                jQuery("#gvVerLista").jqGrid('setRowData', indice + 2, rowData);
            }

            jQuery("#gvVerLista").resetSelection();
        }

        jQuery(document).ajaxStart(jQuery.blockUI).ajaxStop(jQuery.unblockUI);

        var frameSW = false;

        //  Inicio Ready
        jQuery(document).ready(function () {

            document.getElementsByTagName('iframe')[0].onload = function () {
//                if (/(MSIE\ [0-9]{1})/i.test(navigator.userAgent)) {
//                    window.frameSW = true;
//                     $('#hdfFlatArchivoActualizado').val('0');
//                }
                if (window.frameSW) {
                    var childiFrame = document.getElementById("myFrame");
                    var innerDoc = childiFrame.contentDocument || ChildiFrame.contentWindow.document;
                    var yourChildiFrameControl = innerDoc.getElementById("hdnEstado");
                    var value = yourChildiFrameControl.value;
                    if(value == "OK"){
                      $('#hdfFlatArchivoActualizado').val('1');
                      var nombreControl= $('#hdfNombreLinkArchivo').val();
                      var NombreArchivo = $('#lklNombreArchivoActualizar').html();
                      
                      var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreArchivo + '&Opc=COTIZACION';
                      $('#' + nombreControl ).html(NombreArchivo);                            
                      $('#' + nombreControl).attr('href', urlArchivo);
                      $('#' + nombreControl).attr('target', '_blank');
                      $("#diagActualizarArchivo").dialog('close');
                      alert('Se Actualizó correctamente.');
                    }
                      
                }
                else {
                    window.frameSW = true;
                     $('#hdfFlatArchivoActualizado').val('0');
                }
            }

            //0000000000000000000000000000000000000000000000000000
            var TiempoSession = <%= CInt(0.95 *(Session.Timeout * 60000)) %>;
            $(document).everyTime(TiempoSession, function () {
                //  VerificarEstadoSession('0');
                            arreglo = parametro = null;
                            arreglo = fc_redimencionarArray(1);
                            arreglo[0][0] = "session";
                            arreglo[0][1] = $('#MainContent_lblIdSession').html();
                            parametro = fc_parametrosData(arreglo);

                                $.ajax({
                                    type: "POST",
                                    url: location.pathname + "/KeepActiveSession",
                                    data: parametro,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: true,
                                    success: VerificarEstadoSession,
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert(textStatus + ": " + XMLHttpRequest.responseText);
                                    }
                                });

            });

            $("#diagMensaje").dialog({
                autoOpen: false,
                height: 150,
                width: 250,
                resizable: false,
                modal: true,
                closeOnEscape: false ,
                open: function (event, ui) {
                    //hide close button.
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
            //0000000000000000000000000000000000000000000000000000

            jQuery(document).bind("contextmenu", function (e) { return false; });
            jQuery("#tabs").tabs();
            jQuery("ul.tabs").tabs("div.panes > div");

            jQuery("#btnGenerarCotizacion").button();
            jQuery("#btnGenerarCotizacionPrevia").button();

            jQuery("#ibtnAceptarProductoCsa").button();
            jQuery("#ibtnCancelarProductoCsa").button();

            jQuery("#ibtnMaquinariaAceptar").button();
            jQuery("#ibtnMaquinariaCancelar").button();

            jQuery("#ibtnDescargarDocumento").button();

            $('#btnBuscarAceptarMaquina').button();
            $('#btnBuscarCancelarMaquina').button();

            //            $('#btnCancelarArchivoActualizar').button();

            jQuery("#itxtPartVenededor1").numeric();
            jQuery("#itxtPartVenededor2").numeric();
            jQuery("#itxtFecInicioContrato").numeric({ allow: '/' });
            jQuery("#itxtFecEstCierre").numeric({ allow: '/' });
            jQuery("#itxtFecInicioContrato").datepicker();
            jQuery("#itxtFecEstCierre").datepicker();

            jQuery("#itxtNroMaquinas").numeric();
            jQuery("#itxtHorometro").numeric();
            jQuery("#itxtHorasPromMensual").numeric();
            jQuery("#itxtFechaHorometro").numeric({ allow: '/' });
            jQuery("#itxtFechaHorometro").datepicker();

            $('#txtDetalleCantidad').numeric();

            //=== Actualizar Archivo Cotizacion ==================

                $("#FileActualizarArchivo").uploadify({                
                'swf': '../../Styles/uploadify.swf',
                'uploader': '../../Comunes/UploadVB.ashx',
                'cancelImg': '../../Styles/uploadify-cancel.png',
                'displayData': 'percentage',
                'fileTypeExts': '*.docx;*.pdf', //'*.*',
                'fileTypeDesc': 'Todos los Archivos',
                'buttonText': 'Cargar',
                'auto': true,
                'multi': false,
                'wmode': 'transparent',
                'removeTimeout': 0.5,
                'hideButton': true,
                'method': 'get', 
                'height': 20,
                'width': 230,
                'onSelect': function (file) {  
                    $("#diagActualizarArchivo").dialog({ height: 210 });
                    $('#hdfFlatArchivoActualizado').val('0');

//                    var auth ='';
                    var auth ='<%=If(Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing, String.Empty, Request.Cookies(FormsAuthentication.FormsCookieName).Value) %>';
                    var ASPSESSID = "<%=Session.SessionID%>";                    
                    var TIPOACCION='SUBIR_ARCHIVO';
                    var NombreArchivoActualizar='';

//                    NombreArchivoActualizar = $('#lklNombreArchivoActualizar').html();
                   NombreArchivoActualizar =ExtraerNombreArchivo($('#lklNombreArchivoActualizar').html());
                   NombreArchivoActualizar= NombreArchivoActualizar + file.type;

                    $("#FileActualizarArchivo").uploadify('settings','formData' ,{'Archivo' : NombreArchivoActualizar,'ASPSESSID':ASPSESSID,'AUTHID': auth,'TIPOACCION':TIPOACCION,'DESTINO':'COTIZACION_VERSION'});
                }, 
                'onUploadStart': function (file) {                    
                    $("#FileActualizarArchivo").css('opacity', '0'); 
                },
                'onUploadSuccess': function (file, data, response) {
                    var NombreArchivo = $('#lklNombreArchivoActualizar').html();
                    $("#diagActualizarArchivo").dialog({ height: 150 });
                    $("#FileActualizarArchivo").css('opacity', '1');
//                     alert('Se Actualizó correctamente.'); 
                },
                 'onUploadComplete' : function(file) {
                    ActualizarCotizacionVersion(file.type);
                    $("#diagActualizarArchivo").dialog('close');
                },
                'onUploadError' : function(file, errorCode, errorMsg, errorString) {
                    $("#FileActualizarArchivo").css('opacity', '1');

                    alert('El archivo ' + file.name + ' no pudo guardarse: ' + errorString + '. Vuelva intentarlo');
                }
            });
            //==========================================================
            
            var IdCotizacionSap, IdPosicion, IdProductoSap;
            var arreglo, parametro;
            var tipoOperacion, item;
            var sortColumn, sortOrder, pageSize, currentPage;
            var IncluyeFluidos, IncluyeDetallePartes,
                ParticipacionVendedor1, ParticipacionVendedor2,
                FechaInicioContrato, FechaEstimadaCierre;
            

            function presentarCotizacion() {
            arreglo = parametro = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = "session";
            arreglo[0][1] = $('#MainContent_lblIdSession').html();
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarCotizacion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success") {
                            var result = JSON.parse(jsondata.responseText).d;
                            jQuery("#lbnIdCotizacionSap").text(result[0]);
                            jQuery("#lbnEstado").text(result[1]);
                            jQuery("#lbnSolicitante").text(result[2]);
                            jQuery("#lbnPersonaResponsable").text(result[3]);
                            jQuery("#lbnFechaInicioValidez").text(result[4]);
                            jQuery("#lbnFechaFinValidez").text(result[5]);
                            jQuery("#lbnValorNeto").text(result[6]);
                            jQuery("#lbnFechaPrecio").text(result[7]);
                            jQuery("#lbnImporteImpuesto").text(result[8]);
                            jQuery("#lbnTipoCambio").text(result[9]);
                            jQuery("#lbnValorBruto").text(result[10]);
                            jQuery("#lbnFechaEstFacturacion").text(result[11]);

                            jQuery("#hdfvalRolEdicion").val(result[13]);
                            jQuery("#hdfvalRolBloqueo").val(result[14]);

                            AsignarRolesEdicion(jQuery("#hdfvalRolEdicion").val());
                            AsignarRolesBloqueo(jQuery("#hdfvalRolBloqueo").val());
                            ConfigurarCotizarPrevia(result[15]);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            presentarCotizacion();

            // Consultar Informacion de Producto
            function paginacionProducto() {

                arreglo = parametro = null;
                sortColumn = jQuery('#gvProducto').getGridParam("sortname");
                sortOrder = jQuery('#gvProducto').getGridParam("sortorder");
                pageSize = jQuery('#gvProducto').getGridParam("rowNum");
                currentPage = jQuery('#gvProducto').getGridParam("page");

                arreglo = fc_redimencionarArray(5);
                arreglo[0][0] = "sortColumn";
                arreglo[0][1] = sortColumn;
                arreglo[1][0] = "sortOrder";
                arreglo[1][1] = sortOrder;
                arreglo[2][0] = "pageSize";
                arreglo[2][1] = pageSize;
                arreglo[3][0] = "currentPage";
                arreglo[3][1] = currentPage;
                arreglo[4][0] = "session";
                arreglo[4][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarProducto",
                    dataType: "json",
                    data: parametro,
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success")
                            jQuery("#gvProducto")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function leerProductoSeleccionado(result) {

                if (result.TipoProducto == 'Z001') { // ProductoPrime
                    AbrirDetallePrime(result.IdProducto);
                }
                if (result.TipoProducto == 'Z002') { // ProductoCsa

                    jQuery("#ilbnIdProducto").text(result.IdProductoSap);
                    jQuery("#ilbnProducto").text(result.Descripcion);
                    jQuery("#ilbnEstado").text(result.NombreEstado);
                    jQuery("#ilbnValorNeto").text(result.ValorNeto);

                    var ProductoCSA = result.ProductoCSA;

                    $('#hdfClaseCSA').val(ProductoCSA.ClaseCsa);
                    jQuery("#ilbnDuracion").text(ProductoCSA.Duracion);
                    jQuery("#ilbnPlazoRenovacion").text(ProductoCSA.PlazoRenovacion);

                    IncluyeFluidos = ProductoCSA.IncluyeFluidos.toLowerCase();
                    IncluyeDetallePartes = ProductoCSA.IncluyeDetallePartes.toLowerCase();

                    if (IncluyeFluidos == 'true') {
                        jQuery("#irdnFluidosSi").attr('checked', true);
                        jQuery("#irdnFluidosNo").attr('checked', false);
                    } else {
                        jQuery("#irdnFluidosSi").attr('checked', false);
                        jQuery("#irdnFluidosNo").attr('checked', true);
                    }
                    if (IncluyeDetallePartes == 'true') {
                        jQuery("#irdnDetalleSi").attr('checked', true);
                        jQuery("#irdnDetalleNo").attr('checked', false);
                    } else {
                        jQuery("#irdnDetalleSi").attr('checked', false);
                        jQuery("#irdnDetalleNo").attr('checked', true);
                    }

                    jQuery("#itxtFecInicioContrato").attr("value", ProductoCSA.FechaInicioContrato);
                    jQuery("#itxtFecEstCierre").attr("value", ProductoCSA.FechaEstimadaCierre);
                    jQuery("#itxtPartVenededor1").attr("value", ProductoCSA.ParticipacionVendedor1);
                    jQuery("#itxtPartVenededor2").attr("value", ProductoCSA.ParticipacionVendedor2);

                    $('#itxtValorLista').val(result.ValorLista);
                    $('#itxtValorReal').val(result.ValorReal);
                    $('#itxtDescuentoPorcent').val(result.PorcDescuento);
                    $('#itxtDescuentoImporte').val(result.DescuentoImp);
                    $('#itxtFlete').val(result.Flete);
                    $('#itxtValorVentaTotal').val(result.ValorVenta);
                    $('#itxtImpuestoPorcentaje').val(result.PorcImpuesto);
                    $('#itxtPrecioVentaTotal').val(result.PrecioVentaFinal);

                    jQuery('#gvMaquinaria').trigger("reloadGrid");
                    jQuery("#idDialogProductoCsa").dialog("open");

                }

                if (result.TipoProducto == 'Z003') { // ProductoAccesorio
                    AbrirDetalleAccesorio(result.IdProducto);
                }

                if (result.TipoProducto == 'Z004') { // ProductoAlquiler
                    AbrirDetalleAlquiler(result.IdProducto);
                }
                
                if (result.TipoProducto =='Z005'){
                    AbrirDiagDetalleProductoSolucionCombinada(result.IdProducto);
                }

            }

            function aceptarMaquinaria() {

            var valNumeroSerie='';
            var tipAccion='';

            if (jQuery("#iddlNroSerie").val()=='0'){
                 valNumeroSerie = jQuery("#itxtNroSerie").val();
            }
            else{
                valNumeroSerie=jQuery("#iddlNroSerie").val();
            }
                
           tipAccion=jQuery("#ihddTipoOperacion").get(0).value

                var validoMaquina=ValidarMaquinaria(tipAccion,valNumeroSerie);
                if (validoMaquina !=''){
                    alert(validoMaquina);
                    return false;
                }
                

                var maquinaNueva, familia, familiaOt, modeloBase, modeloBaseOt,
                    prefijo,prefijoOt, numeroMaquinas, numeroSerie, numeroSerieOt,
                    horometroInicial, fechaHorometro, horasPromedioMensual, codDepartamento, departamento;

                maquinaNueva = familia = familiaOt = modeloBase =
                modeloBaseOt = prefijo = numeroMaquinas =
                numeroSerie = numeroSerieOt = horometroInicial = fechaHorometro =
                horasPromedioMensual = codDepartamento = departamento = "";

                tipoOperacion = jQuery("#ihddTipoOperacion").get(0).value;
                item = jQuery("#ihdnIdMaquinaria").get(0).value

                if (jQuery('input:radio[id=irdnMaquinaNuevaSi]:checked').val())
                    maquinaNueva = true;
                else
                    maquinaNueva = false;

                familia = jQuery("#iddlFamilia").val();
                familiaOt = jQuery("#itxtFamilia").val();
                modeloBase = jQuery("#iddlModeloBase").val();
                modeloBaseOt = jQuery("#itxtModeloBase").val();
                prefijo = jQuery("#iddlPrefijo").val();
                prefijoOt = $("#itxtPrefijo").val();
                numeroMaquinas = jQuery("#itxtNroMaquinas").val();
                numeroSerie = jQuery("#iddlNroSerie").val();
                numeroSerieOt = jQuery("#itxtNroSerie").val();
                horometroInicial = jQuery("#itxtHorometro").val();
                fechaHorometro = jQuery("#itxtFechaHorometro").val();
                horasPromedioMensual = jQuery("#itxtHorasPromMensual").val();
                codDepartamento = jQuery("#iddlDepartamento").val();
                departamento = jQuery("#iddlDepartamento option:selected").text();

                if (familia == "") {
                    jQuery("#iddlFamilia").focus();
                    return alert("¡ Seleccione familia ...");
                }
                if (familia == "0") {
                    if (familiaOt == "") {
                        jQuery("#itxtFamilia").focus();
                        return alert("¡Ingrese descripción de familia ...");
                    }
                }
                if (modeloBase == "") {
                    jQuery("#iddlModeloBase").focus();
                    return alert("¡ Seleccione modelo base ...");
                }
                if (modeloBase == "0") {
                    if (modeloBaseOt == "") {
                        jQuery("#itxtModeloBase").focus();
                        return alert("¡Ingrese descripción de modelo base ...");
                    }
                }
                if (prefijo == "") {
                    jQuery("#iddlPrefijo").focus();
                    return alert("¡ Seleccione prefijo ...");
                }
                 if (prefijo == "0") {
                    if (prefijoOt == "") {
                        $("#itxtPrefijo").focus();
                        return alert("¡Ingrese descripción de prefijo ...");
                    }
                }
                if (horometroInicial == "") {
                    jQuery("#itxtHorometro").focus();
                    return alert("¡ Ingrese el Horometro ...");
                }
                if (isNaN(horometroInicial)) {
                    jQuery("#itxtHorometro").focus();
                    return alert("¡ Ingrese el Horometro ...");
                }
                if (maquinaNueva) {
                    if (parseInt(horometroInicial) > 250) {
                        jQuery("#itxtHorometro").focus();
                        return alert("¡ Para máquinas nuevas el horometro no puede ser mayor a 250 ...");
                    }
                } else {
                    if (parseInt(horometroInicial) <= 250) {
                        jQuery("#itxtHorometro").focus();
                        return alert("¡ Para máquinas usadas el horometro no puede ser menor o igual a 250 ...");
                    }
                }
                if (isNaN(horasPromedioMensual)) {
                    jQuery("#itxtHorasPromMensual").focus();
                    return alert("¡ Ingrese las horas promedio de uso mensual ...");
                }
                if (codDepartamento == "") {
                    jQuery("#iddlDepartamento").focus();
                    return alert("¡ Seleccione departamento ...");
                }

                arreglo = fc_redimencionarArray(18);
                arreglo[0][0] = "tipoOperacion";
                arreglo[0][1] = tipoOperacion;
                arreglo[1][0] = "item";
                arreglo[1][1] = item;
                arreglo[2][0] = "maquinaNueva";
                arreglo[2][1] = maquinaNueva;
                arreglo[3][0] = "familia";
                arreglo[3][1] = familia;
                arreglo[4][0] = "familiaOt";
                arreglo[4][1] = familiaOt;
                arreglo[5][0] = "modeloBase";
                arreglo[5][1] = modeloBase;
                arreglo[6][0] = "modeloBaseOt";
                arreglo[6][1] = modeloBaseOt;
                arreglo[7][0] = "prefijo";
                arreglo[7][1] = prefijo;                 
                arreglo[8][0] = "prefijoOt";
                arreglo[8][1] =  prefijoOt;
                arreglo[9][0] = "numeroMaquinas";
                arreglo[9][1] =  numeroMaquinas;
                arreglo[10][0] = "numeroSerie";
                arreglo[10][1] = numeroSerie;
                arreglo[11][0] = "numeroSerieOt";
                arreglo[11][1] = numeroSerieOt;
                arreglo[12][0] = "horometroInicial";
                arreglo[12][1] = horometroInicial;
                arreglo[13][0] = "fechaHorometro";
                arreglo[13][1] = fechaHorometro;
                arreglo[14][0] = "horasPromedioMensual";
                arreglo[14][1] = horasPromedioMensual;
                arreglo[15][0] = "codDepartamento";
                arreglo[15][1] = codDepartamento;
                arreglo[16][0] = "departamento";
                arreglo[16][1] = departamento;
                arreglo[17][0] = "session";
                arreglo[17][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/AceptarMaquinaria",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            actualizarDialogProductoCsa();
                            jQuery("#gvMaquinaria").trigger("reloadGrid");
                            jQuery("#ibtnMaquinariaCancelar").click();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            function ValidarMaquinaria(accion,paramNroSerie){
            
            var mensaje= '';
            var Registros =jQuery("#gvMaquinaria").jqGrid('getRowData');
            var indiceRegSel = -1;
            $.each(Registros, function(index, fila){ 
                var nroSerie='';
                nroSerie=fila.numeroSerie;
                
                if(paramNroSerie.toUpperCase() ==nroSerie.toUpperCase() ){
                    
                    if (accion=='2'){
                        indiceRegSel = jQuery("#gvMaquinaria").jqGrid('getGridParam', 'selrow');
                        
                        var codigoRegistro='';
                        var codigoRegistroSel='';

                        rowSeleccionado = jQuery("#gvMaquinaria").jqGrid('getRowData', indiceRegSel); 
                        codigoRegistroSel=rowSeleccionado.codigo
                        codigoRegistro = fila.codigo; 
                         
                        if (codigoRegistroSel != codigoRegistro){// El indice del registro de la grilla empieza de uno..cuando la grilla tiene cabecera
                        mensaje='Ya existe una maquinaria con la serie: ' + paramNroSerie + ' en la posición ' + (parseInt(index) + 1)  ;
                        }
                    }
                    else{
                        mensaje='Ya existe una maquinaria con la serie: ' + paramNroSerie + ' en la posición ' + ( parseInt(index) + 1)  ;
                    }
                    
                    return mensaje;
                }
            }); 

            return mensaje;

            }

            function actualizarDialogProductoCsa() {

                if (jQuery('input:radio[id=irdnFluidosSi]:checked').val())
                    IncluyeFluidos = true;
                else
                    IncluyeFluidos = false;

                if (jQuery('input:radio[id=irdnDetalleSi]:checked').val())
                    IncluyeDetallePartes = true;
                else
                    IncluyeDetallePartes = false;

                FechaInicioContrato = jQuery("#itxtFecInicioContrato").val();
                FechaEstimadaCierre = jQuery("#itxtFecEstCierre").val();
                ParticipacionVendedor1 = jQuery("#itxtPartVenededor1").val();
                ParticipacionVendedor2 = jQuery("#itxtPartVenededor2").val();

                arreglo = parametro = null;

                arreglo = fc_redimencionarArray(7);
                arreglo[0][0] = "IncluyeFluidos";
                arreglo[0][1] = IncluyeFluidos;
                arreglo[1][0] = "IncluyeDetallePartes";
                arreglo[1][1] = IncluyeDetallePartes;
                arreglo[2][0] = "FechaInicioContrato";
                arreglo[2][1] = FechaInicioContrato;
                arreglo[3][0] = "FechaEstimadaCierre";
                arreglo[3][1] = FechaEstimadaCierre;
                arreglo[4][0] = "ParticipacionVendedor1";
                arreglo[4][1] = ParticipacionVendedor1;
                arreglo[5][0] = "ParticipacionVendedor2";
                arreglo[5][1] = ParticipacionVendedor2;
                arreglo[6][0] = "session";
                arreglo[6][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarProductoCsaActualizado",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success") {
                            var result = JSON.parse(jsondata.responseText).d;
                            leerProductoSeleccionado(result);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function abrirDialog(rowid) {
                var fila = jQuery("#gvProducto").jqGrid('getRowData', rowid);
                var IdProducto = fila.IdProducto;
                switch (fila.TipoProducto.toString()) {
                    case 'Z001':
                        AbrirDetallePrime(IdProducto);
                        return;
                    case 'Z002':

                        IdPosicion = fila.IdPosicion;
                        IdProductoSap = fila.IdProductoSap;
                        arreglo = parametro = null;

                        arreglo = fc_redimencionarArray(3)
                        arreglo[0][0] = 'IdPosicion';
                        arreglo[0][1] = IdPosicion;
                        arreglo[1][0] = 'IdProductoSap';
                        arreglo[1][1] = IdProductoSap;
                        arreglo[2][0] = "session";
                        arreglo[2][1] = $('#MainContent_lblIdSession').html();

                        parametro = fc_parametrosData(arreglo);

                        jQuery.ajax({
                            url: location.pathname + "/ConsultarProductoSeleccionado",
                            data: parametro,
                            dataType: "json",
                            type: "post",
                            contentType: "application/json; charset=utf-8",
                            complete: function (jsondata, stat) {
                                if (stat == "success") {
                                    var result = JSON.parse(jsondata.responseText).d;
                                    leerProductoSeleccionado(result);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert(textStatus + ': ' + XMLHttpRequest.responseText);
                            }
                        });
                        return;
                    case "Z003":
                        AbrirDetalleAccesorio(IdProducto);
                        return;
                    case "Z004":
                        AbrirDetalleAlquiler(IdProducto);
                        return;
                     case "Z005":
                        AbrirDiagDetalleProductoSolucionCombinada(IdProducto);
                        return;
                }

            }

            function aceptarProductoCsa() {

                if (jQuery('input:radio[id=irdnFluidosSi]:checked').val())
                    IncluyeFluidos = true;
                else
                    IncluyeFluidos = false;

                if (jQuery('input:radio[id=irdnDetalleSi]:checked').val())
                    IncluyeDetallePartes = true;
                else
                    IncluyeDetallePartes = false;

                FechaInicioContrato = jQuery("#itxtFecInicioContrato").val();
                FechaEstimadaCierre = jQuery("#itxtFecEstCierre").val();
                ParticipacionVendedor1 = jQuery("#itxtPartVenededor1").val();
                ParticipacionVendedor2 = jQuery("#itxtPartVenededor2").val();

                arreglo = parametro = null;

                arreglo = fc_redimencionarArray(7);
                arreglo[0][0] = "IncluyeFluidos";
                arreglo[0][1] = IncluyeFluidos;
                arreglo[1][0] = "IncluyeDetallePartes";
                arreglo[1][1] = IncluyeDetallePartes;
                arreglo[2][0] = "FechaInicioContrato";
                arreglo[2][1] = FechaInicioContrato;
                arreglo[3][0] = "FechaEstimadaCierre";
                arreglo[3][1] = FechaEstimadaCierre;
                arreglo[4][0] = "ParticipacionVendedor1";
                arreglo[4][1] = ParticipacionVendedor1;
                arreglo[5][0] = "ParticipacionVendedor2";
                arreglo[5][1] = ParticipacionVendedor2;
                arreglo[6][0] = "session";
                arreglo[6][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/AceptarProductoCsa",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            presentarCotizacion();
                            jQuery("#gvProducto").trigger("reloadGrid");
                            jQuery("#ibtnCancelarProductoCsa").click();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function inicializarDialogMaquinaria() {
                consultarFamilia();
                consultarDepartamento();
            }

            function nuevaMaquinaria(rowGrid, item) {

                jQuery("#ihddTipoOperacion").attr("value", "1");
                jQuery("#ihdnIdMaquinaria").attr("value", "");

                $("#itxtFamilia").attr("value", "");
                $("#itxtModeloBase").attr("value", "");
                $("#itxtPrefijo").attr("value", "");
                $("#itxtNroSerie").attr("value", "");

                $("#itxtFamilia").css("display", "none");
                $("#itxtModeloBase").css("display", "none");
                $("#itxtPrefijo").css("display", "none");
                $("#itxtNroSerie").css("display", "none");

                //--------------------------------------------------//
                inicializarDialogMaquinaria();
                //--------------------------------------------------//
                jQuery("#idDialogMaquinaria").dialog("open");

            }

            function consultarDepartamento() {
                jQuery("#iddlDepartamento").find('option').remove().end();
                jQuery("#iddlDepartamento").attr('disabled', 'true');
                jQuery.ajax({
                    url: location.pathname + "/ConsultarDepartamento",
                    data: '{}',
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery("#iddlDepartamento").append(jQuery("<option></option>").val('').html(''));
                        jQuery.each(data.d, function () {
                            jQuery("#iddlDepartamento").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                        jQuery('#iddlDepartamento').removeAttr('disabled');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') {
                            jQuery('#iddlDepartamento').val(jQuery("#ihddDepartamento").get(0).value);
                        }
                    }
                });
            }

            function consultarFamilia() {

                jQuery("#iddlFamilia").find('option').remove().end();
                jQuery("#iddlFamilia").attr('disabled', 'true');

                arreglo = parametro = null;
                 arreglo = fc_redimencionarArray(1);
                 arreglo[0][0] = "session";
                arreglo[0][1] = $('#MainContent_lblIdSession').html();
                 parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarFamilia",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            jQuery("#iddlFamilia").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        jQuery('#iddlFamilia').removeAttr('disabled');
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') {
                            jQuery('#iddlFamilia').val(jQuery("#ihddFamilia").get(0).value);
                            jQuery('#iddlFamilia').change();
                        }
                    }
                });
            }

            function leerMaquinaria(rowGrid) {

                jQuery("#ihdnIdMaquinaria").attr("value", rowGrid.item);
                var texto='';
                texto=rowGrid.maquinaNueva.toUpperCase();
                texto='';
                texto=('No').toUpperCase();

                if (rowGrid.maquinaNueva.toUpperCase() ==('No').toUpperCase()) {
                    jQuery("#irdnMaquinaNuevaSi").attr('checked', false);
                    jQuery("#irdnMaquinaNuevaNo").attr('checked', true); 
                } else { 
                    jQuery("#irdnMaquinaNuevaNo").attr('checked', false);
                    jQuery("#irdnMaquinaNuevaSi").attr('checked', true);
                }

               
                if (rowGrid.familiaOt !=""){
                        $("#ihddFamilia").attr("value","0");
                }
                else {
                    $("#ihddFamilia").attr("value", rowGrid.familia);
                }
                                 
                jQuery("#itxtFamilia").attr("value", rowGrid.familiaOt);

               
               
                if (rowGrid.modeloBaseOt !="")
                {
                    $("#ihddModeloBase").attr("value", "0");
                }
                else
                {
                    $("#ihddModeloBase").attr("value", rowGrid.modeloBase);
                }
                 
                jQuery("#itxtModeloBase").attr("value", rowGrid.modeloBaseOt);


                
                if(rowGrid.prefijoOt !="")
                {
                    $("#ihddPrefijo").attr("value", "0");
                }
                else
                {
                    $("#ihddPrefijo").attr("value", rowGrid.prefijo);
                }
                                
                $("#itxtPrefijo").attr("value", rowGrid.prefijoOt);

                jQuery("#itxtNroMaquinas").attr("value", rowGrid.numeroMaquinas);



                if(rowGrid.numeroSerieOt != "")
                {                     
                    $("#ihddNroSerie").attr("value", "0");  
                     // por la demora en cargar del combo de serie, esperar que se carge para poder selecionar un option   
                    SelOptionComboTimer('iddlNroSerie', '0', 'itxtNroSerie', '500') ;                     
                     
                      jQuery("#itxtNroSerie").css("display", "block");
                }
                else
                {
                    $("#ihddNroSerie").val(rowGrid.numeroSerie);
                }
                                 
                jQuery("#itxtNroSerie").attr("value", rowGrid.numeroSerieOt);
                jQuery("#itxtHorometro").attr("value", rowGrid.horometroInicial);
                jQuery("#itxtFechaHorometro").attr("value", rowGrid.fechaHorometro);
                jQuery("#itxtHorasPromMensual").attr("value", rowGrid.horasPromedioMensual);
                jQuery("#ihddDepartamento").attr("value", rowGrid.codDepartamento);

            }

            function editarMaquinaria(rowGrid) {

                jQuery("#ihddTipoOperacion").attr("value", "2");
                //--------------------------------------------------//
                inicializarDialogMaquinaria();
                //--------------------------------------------------//
                leerMaquinaria(rowGrid);                                

                $("#trNroMaquinas").css("display", "none");


                jQuery("#idDialogMaquinaria").dialog("open");

            }

            function eliminarMaquinaria(rowGrid) {

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = "item";
                arreglo[0][1] = rowGrid.item;
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/EliminarMaquinaria",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            actualizarDialogProductoCsa();
                            jQuery("#gvMaquinaria").trigger("reloadGrid");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function CargarMaquinaria(){
                $('#txtBuscarNroCotizacion').val('');
                $("#jqgBuscarListaMaquinaria").clearGridData();
                $("#cbBuscarPosicionProducto option").remove();

                $('#divDialogBuscarMaquinaria').dialog('open');
            }

            $('#lblCerrarVentana').click(function (evento) {
                if (!(confirm('¿Seguro desea Cerrar?'))) {
                    return;
                }
                window.open('', '_parent', '');
                window.close();
            });

            jQuery("#lbnGrabar").click(function (evento) {

                if (!confirm('¿Seguro desea grabar?')) return;

                arreglo = parametro = null;

                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = "session";
                arreglo[0][1] = $('#MainContent_lblIdSession').html();
//                arreglo[1][0] = "idProducto";
//                arreglo[1][1] = $('#txtIdProductoSolComb').val();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/GrabarCotizacion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            alert('Se grabó correctamente');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            });

            jQuery("#irdnFluidosSi").change(function (evento) {
                actualizarDialogProductoCsa();
            });

            jQuery("#irdnFluidosNo").change(function (evento) {
                actualizarDialogProductoCsa();
            });

            jQuery("#ibtnMaquinariaAceptar").click(function (evento) {
                aceptarMaquinaria();
            });

            jQuery("#ibtnAceptarProductoCsa").click(function (evento) {
                aceptarProductoCsa();
            });

            $('#btnBuscarCotizacion').click(function(evento){
                BuscarCotizacionCargar();
            });

            $('#btnBuscarAceptarMaquina').click(function(evento){
                aceptarCargarMaquinaria();
            });

            $('#btnBuscarCancelarMaquina').click(function(evento){
               CerrarDiagCargarMaquinaria(); 
            });

            jQuery('#btnDetalleAceptar').click(function (evento) {
                GuardarDetallePrime();
            });

            jQuery('#btnDetalleCancelar').click(function (evento) {
                CerrarDiagDetallePrime();
            });

            $('#btnAceptarProductoAlquiler').click(function (evento) {
                GuardarDetalleAlquiler();
            });

            $('#btnCancelarProductoAlquiler').click(function (evento) {
                CerrarDiagDetalleAlquiler();
            });

            $('#btnAceptarProductoAccesorio').click(function (evento) {
                GuardarDetalleAccesorio();
            });

            $('#btnCancelarProductoAccesorio').click(function (evento) {
                CerrarDiagDetalleAccesorio();
            });

            $('#btnAceptarProductoSolComb').click(function (evento) {
                GuardarDetalleProductoSolucionCombinada();
            });

            jQuery('#btnCancelarProductoSolComb').click(function (evento) {
                CerrarDiagDetalleProductoSolucionCombinada();
            });

            $('#cbBuscarPosicionProducto').change(function (evento) {
                BuscarMaquinariaCargar();
            });

            //            $('#btnCancelarArchivoActualizar').click(function (evento) {
            //                $("#diagActualizarArchivo").dialog("close");
            //            });

            $('#lklDescargarArchivo').click(function (evento) {
                $("#diagDescargarArchivo").dialog("close");
            });

            jQuery('#ibtnDescargarDocumento').click(function (evento) {

                var indice = null;
                var getRowData = jQuery("#gvVerLista").jqGrid('getRowData');
                var rowDataPrev, rowData, rowDataNext;
                var IdSeccion = '';
                var contenido, Imprimir;
                var Guardar = null;
                for (var i = 0; i < getRowData.length; i++) {
                    contenido = getRowData[i].Imprimir;
                    if (contenido.length > 2) {
                        contenido = $('#ddlImprimir_'+i).find("option:selected").text();
//                        posicion = contenido.indexOf("<OPTION selected>");
//                        contenido = (contenido.substring(posicion + 17)).substring(0, 2);
                    }
                    if (IdSeccion == '') {
                        IdSeccion = getRowData[i].IdSeccion;
                        Imprimir = contenido;
                    } else {
                        IdSeccion = IdSeccion + ',' + getRowData[i].IdSeccion;
                        Imprimir = Imprimir + ',' + contenido;
                    }
                }

                if (jQuery('input:checkbox[id=ichkGuardarLista]:checked').val())
                    Guardar = true;
                else
                    Guardar = false;
                
                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(4);
                arreglo[0][0] = "IdSeccion";
                arreglo[0][1] = IdSeccion;
                arreglo[1][0] = "Imprimir";
                arreglo[1][1] = Imprimir;
                arreglo[2][0] = "Guardar";
                arreglo[2][1] = Guardar;
                arreglo[3][0] = "session";
                arreglo[3][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/DescargarCotizacion",
                    dataType: "json",
                    type: "post",
                    cache: false,
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    data: parametro,
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else {
                            if (result.mensaje != "") {
                                //Actualizar la Grilla con la Nueva version Generada
                                BuscarOfertaValor();
                                var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + result.mensaje + '&Opc=COTIZACION';
                                //window.showModalDialog(urlArchivo, "", "center=yes;dialogWidth=200px;dialogHeight=200px;status:0;help:0")
                                //window.open(urlArchivo);
                                $('#lklDescargarArchivo').attr('href', urlArchivo);
                                $('#lklDescargarArchivo').attr('target', '_blank');
                                $("#diagDescargarArchivo").dialog("open");
                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            });

            jQuery("#idDialogLista").dialog({
                autoOpen: false,
                height: 340,
                width: 600,
                resizable: false,
                modal: true,
                open: function () {
                    consultarVerLista();
                }
            });

            jQuery("#idDialogProductoCsa").dialog({
                autoOpen: false,
                height: 590,
                width: 940,
                resizable: false,
                modal: true,
                close: function () {

                    jQuery("#ilbnIdProducto").text('');
                    jQuery("#ilbnProducto").text('');
                    jQuery("#ilbnEstado").text('');
                    jQuery("#ilbnValorNeto").text('');
                    jQuery("#ilbnDuracion").text('');
                    jQuery("#ilbnPlazoRenovacion").text('');
                    jQuery("#irdnFluidosSi").attr('checked', false);
                    jQuery("#irdnFluidosNo").attr('checked', false);
                    jQuery("#irdnDetalleSi").attr('checked', false);
                    jQuery("#irdnDetalleNo").attr('checked', false);
                    jQuery("#itxtFecInicioContrato").attr("value", '');
                    jQuery("#itxtFecEstCierre").attr("value", '');
                    jQuery("#itxtPartVenededor1").attr("value", '');
                    jQuery("#itxtPartVenededor2").attr("value", '');
                    jQuery("#gvMaquinaria").clearGridData();

                    $('#hdfClaseCSA').val('');
                    $('#itxtValorLista').val('');
                    $('#itxtValorReal').val('');
                    $('#itxtDescuentoPorcent').val('');
                    $('#itxtDescuentoImporte').val('');
                    $('#itxtFlete').val('');
                    $('#itxtValorVentaTotal').val('');
                    $('#itxtImpuestoPorcentaje').val('');
                    $('#itxtPrecioVentaTotal').val('');

                }
            });

            jQuery("#idDialogMaquinaria").dialog({
                autoOpen: false,
                height: 370,
                width: 550,
                resizable: false,
                modal: true,
                close: function () {

                    jQuery("#ihddTipoOperacion").attr("value", '');
                    jQuery("#ihdnIdMaquinaria").attr("value", '');

                    jQuery("#ihddFamilia").attr("value", '');
                    jQuery("#itxtFamilia").attr("value", '');
                    jQuery("#ihddModeloBase").attr("value", '');
                    jQuery("#itxtModeloBase").attr("value", '');
                    jQuery("#ihddPrefijo").attr("value", '');
                    $("#itxtPrefijo").attr("value", '');
                    jQuery("#itxtNroMaquinas").attr("value", '');
                    jQuery("#ihddNroSerie").val('');
                    jQuery("#itxtNroSerie").attr("value", '');
                    jQuery("#itxtHorometro").attr("value", '');
                    jQuery("#itxtFechaHorometro").attr("value", '');
                    jQuery("#itxtHorasPromMensual").attr("value", '');
                    jQuery("#ihddDepartamento").attr("value", '');

                    jQuery("#iddlNroSerie").find('option').remove().end();
                    jQuery("#iddlPrefijo").find('option').remove().end();
                    jQuery("#iddlModeloBase").find('option').remove().end();
                    jQuery("#iddlFamilia").find('option').remove().end();
                    jQuery("#iddlDepartamento").find('option').remove().end();

                    //                    $("#itxtNroMaquinas").css("display", "block");
                    //                    $("#labelNroMaquinas").css("display", "block");
                    $("#trNroMaquinas").css("display", "block");

                }
            });

            jQuery("#divDialogBuscarMaquinaria").dialog({
                autoOpen: false,
                height: 580,
                width: 900,
                resizable: false,
                modal: true,
                open: function () {
//                    consultarVerLista();
                }
            });


            $('#txtDetalleCantidad').keyup(function (evento) {
                CalcularValorNeto();
            });

            jQuery("#ibtnCancelarProductoCsa").click(function (evento) {
                jQuery("#idDialogProductoCsa").dialog("close");
            });

            jQuery("#ibtnMaquinariaCancelar").click(function (evento) {
                jQuery("#idDialogMaquinaria").dialog("close");
            });

            jQuery("#gvVerLista").jqGrid({
                colNames: ['', 'Posición', 'Secciones', 'Imprimir', ''],
                colModel: [
                                        { name: 'IdSeccion', index: 'IdSeccion', hidden: true },
                                        { name: 'PosicionInicial', index: 'PosicionInicial', hidden: true },
                                        { name: 'Nombre', index: 'Nombre', width: 380 },
                                        { name: 'Imprimir', index: 'Imprimir', width: 65, align: "center" },
                                        { name: 'CambioPosicion', index: 'CambioPosicion', width: 60, align: "center" }
                          ],
                height: 210,
                viewrecords: true,
                cmTemplate: { sortable: false },
                rownumbers: true
            });

            $("#diagProductoVersion").dialog({
                autoOpen: false,
                height: 340,
                width: 600,
                resizable: false,
                modal: true,
                open: function () {
                    //consultarProductoVersion();
                }
            });

            $("#diagDescargarArchivo").dialog({
                autoOpen: false,
                height: 100,
                width: 200,
                resizable: false,
                closeOnEscape: false ,
                modal: true
            });

            $("#diagActualizarArchivo").dialog({
                autoOpen: false,
                height: 175,
                width: 330,
                resizable: false,
                modal: true
            });

            $("#jqgProductoVersion").jqGrid({
                colNames: ['Ide', 'Codigo', 'Nombre'],
                colModel: [
                            { name: 'Ide', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 100 },
                            { name: 'Nombre', index: 'Nombre', width: 400 }
                          ],
                height: 250,
                width: 550,
                viewrecords: true,
                cmTemplate: { sortable: false },
                rownumbers: true
            });

            function consultarVerLista() {

            arreglo = parametro = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = "session";
            arreglo[0][1] = $('#MainContent_lblIdSession').html();
            parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarVerLista",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        lista = response.d;
                        jQuery("#gvVerLista").clearGridData();
                        for (var i = 0; i < lista.length; i++) {
                            var Boton = '';
                            var Opcional = lista[i].Opcional;
                            var CambioPosicion = lista[i].CambioPosicion;
                            var Imprimir = lista[i].Imprimir;

                            Opcional = Opcional.toLowerCase();
                            CambioPosicion = CambioPosicion.toLowerCase();

                            if (Opcional == 'true' || Opcional == 'yes' || Opcional == 'si') {
                                if (Imprimir == 'Si')
                                    Imprimir = '<select id="ddlImprimir_' + i + '"><option selected>Si</option><option>No</option></select>';
                                else
                                    Imprimir = '<select id="ddlImprimir_' + i + '"><option>Si</option><option selected>No</option></select>';
                            }

                            if (CambioPosicion == 'true' || CambioPosicion == 'yes' || CambioPosicion == 'si') {
                                Boton = '<img id="imgSubir_' + i + '" src="../../Images/arrow_up.jpg" alt="0" border="1" title="subir" style="cursor: pointer" onclick="cambiarPosicion(1,' + lista[i].IdSeccion + ')"/>';
                                Boton = Boton + '  <img id="imgBajar_' + i + '" src="../../Images/arrow_down.jpg" alt="0" border="1" title="bajar" style="cursor: pointer" onclick="cambiarPosicion(2,' + lista[i].IdSeccion + ')"/>';
                            }

                            var mydata = [
                                {
                                    IdSeccion: lista[i].IdSeccion,
                                    PosicionInicial: lista[i].PosicionInicial,
                                    Nombre: lista[i].Nombre,
                                    Imprimir: Imprimir,
                                    CambioPosicion: Boton
                                }
                            ];
                            jQuery("#gvVerLista").jqGrid('addRowData', i + 1, mydata[0]);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            //Cargar Datos de contacto
            jQuery("#gvContacto").jqGrid({
                datatype: function () {
                    arreglo = parametro = null;
                    sortColumn = jQuery('#gvContacto').getGridParam("sortname");
                    sortOrder = jQuery('#gvContacto').getGridParam("sortorder");
                    arreglo = fc_redimencionarArray(3);
                    arreglo[0][0] = "sortColumn";
                    arreglo[0][1] = sortColumn;
                    arreglo[1][0] = "sortOrder";
                    arreglo[1][1] = sortOrder;
                    arreglo[2][0] = "session";
                    arreglo[2][1] = $('#MainContent_lblIdSession').html();

                    parametro = fc_parametrosData(arreglo);
                    jQuery.ajax({
                        url: location.pathname + "/ConsultarContacto",
                        data: parametro,
                        dataType: "json",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        complete: function (jsondata, stat) {
                            if (stat == "success")
                                jQuery("#gvContacto")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });
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
                colNames: ['', 'Función', 'Nombres', 'Dirección', '', 'Telefono'],
                colModel: [
                                        { name: 'IdCotizacionContacto', index: 'IdCotizacionContacto', hidden: true },
                                        { name: 'Cargo', index: 'Cargo', width: 220 },
                                        { name: 'Nombres', index: 'Nombres', width: 320 },
                                        { name: 'Direccion', index: 'Direccion', width: 340 },
                                        { name: 'Email', index: 'Email', hidden: true },
                                        { name: 'Telefono', index: 'Telefono', width: 120 }
                          ],
                height: 200,
                viewrecords: true,
                rownumbers: true,
                caption: "Lista de Contacto"
            });



            jQuery("#gvProducto").jqGrid({
                datatype: function () {
                    paginacionProducto();
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
                height: 480,
                width: 1100,
                colNames: [
                            'IdProducto', 'Nro Posición', 'ID Producto', 'TipoProducto',
                            'Producto', 'Valor Unitario', 'IdMonedaValorUnitario',
                            'Cantidad', 'Unidad', 'Valor Neto', 'Moneda', 'Status'
                          ],
                colModel: [
                                    { name: 'IdProducto', index: 'IdProducto', hidden: true },
                                    { name: 'IdPosicion', index: 'IdPosicion', align: "center", width: 120 },
                                    { name: 'IdProductoSap', index: 'IdProductoSap', align: "center", width: 120 },
                                    { name: 'TipoProducto', index: 'TipoProducto', hidden: true },
                                    { name: 'Descripcion', index: 'Descripcion', width: 400 },
                                    { name: 'ValorUnitario', index: 'ValorUnitario', width: 120, align: "right",
                                        formatter: 'number',
                                        formatoptions: { decimalPlaces: 2, defaultValue: '0.00', thousandsSeparator: ',', decimalSeparator: '.' },
                                        hidden: true
                                    },
                                    { name: 'IdMonedaValorUnitario', index: 'IdMonedaValorUnitario', hidden: true },
                                    { name: 'Cantidad', index: 'Cantidad', width: 120, align: 'center' },
                                    { name: 'Unidad', index: 'Unidad', width: 100, align: 'center' },
                                    { name: 'ValorNeto', index: 'ValorNeto', width: 120, align: "right",
                                        formatter: 'number',
                                        formatoptions: { decimalPlaces: 2, defaultValue: '0.00', thousandsSeparator: ',', decimalSeparator: '.' }
                                    },
                                    { name: 'IdMonedaValorNeto', index: 'IdMonedaValorNeto', align: "center", width: 100 },
                                    { name: 'NombreEstado', index: 'NombreEstado', width: 160, align: 'center' }
                          ],
                viewrecords: true,
                multiselect: false,
                pager: "#pgProducto",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                rowNum: "10",
                rowList: [10, 20, 30],
                rownumbers: true,
                caption: "Posiciones",
                gridComplete: function () {
                    var jQueryids = jQuery("#gvProducto").jqGrid('getDataIDs');
                    for (var jQueryi = 0; jQueryi < jQueryids.length; jQueryi++) {
                        jQuery("#gvProducto").setCell(jQueryids[jQueryi], "IdPosicion", "", { 'font-weight': 'bold', color: 'blue', 'text-decoration': 'underline', 'cursor': 'pointer' });
                    }
                },
                onCellSelect: function (rowid, index) {
                    if (index == 2) abrirDialog(rowid);
                }
            });
            /* ----------------------------------------------------------------------------------------------------------- */
            jQuery("#gvProducto").jqGrid('navGrid', '#pgProducto', { search: false, refresh: false, edit: false, add: false, del: false });
            /* ----------------------------------------------------------------------------------------------------------- */

            // Carga de Datos de maquinarias
            jQuery("#gvMaquinaria").jqGrid({
                datatype: function () {

                    arreglo = parametro = null;
                    sortColumn = jQuery('#gvMaquinaria').getGridParam("sortname");
                    sortOrder = jQuery('#gvMaquinaria').getGridParam("sortorder");
                    pageSize = jQuery('#gvMaquinaria').getGridParam("rowNum");
                    currentPage = jQuery('#gvMaquinaria').getGridParam("page");

                    arreglo = fc_redimencionarArray(5);
                    arreglo[0][0] = "sortColumn";
                    arreglo[0][1] = sortColumn;
                    arreglo[1][0] = "sortOrder";
                    arreglo[1][1] = sortOrder;
                    arreglo[2][0] = "pageSize";
                    arreglo[2][1] = pageSize;
                    arreglo[3][0] = "currentPage";
                    arreglo[3][1] = currentPage;
                    arreglo[4][0] = "session";
                    arreglo[4][1] = $('#MainContent_lblIdSession').html();

                    parametro = fc_parametrosData(arreglo);

                    jQuery.ajax({
                        url: location.pathname + "/ConsultarMaquinaria",
                        data: parametro,
                        dataType: "json",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        complete: function (jsondata, stat) {
                            if (stat == "success")
                                jQuery("#gvMaquinaria")[0].addJSONData(JSON.parse(jsondata.responseText).d); 
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });
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
                colNames: ['codigo','item', 'Familia', 'Modelo', 'Modelo Base', 'Prefijo',
                            'Tipo', 'Nro Serie', 'Horometro', 'Fecha Horom',
                            'Hrs Prom. Uso', 'Horom. Fin', 'Renov.', 'Renov. Val',
                            'Departamento', 'maquinaNueva', 'renovacion', 'renovacionValida',
                             'familiaOt', 'modeloBaseOt','prefijoOt', 'numeroMaquinas', 
                             'numeroSerieOt','codDepartamento', 'montoItem'],
                colModel: [
                                             { name: 'codigo', index: 'item', hidden: true },
                                             { name: 'item', index: 'item', hidden: true },
                                             { name: 'familia', index: 'familia', width: 300 },
                                             { name: 'modelo', index: 'modelo', width: 90 },
                                             { name: 'modeloBase', index: 'modeloBase', width: 90 },
                                             { name: 'prefijo', index: 'prefijo', width: 90 },
                                             { name: 'condicionMaquinaria', index: 'condicionMaquinaria', width: 60, align: 'center' },
                                             { name: 'numeroSerie', index: 'numeroSerie', width: 90 },
                                             { name: 'horometroInicial', index: 'horometroInicial', width: 90 },
                                             { name: 'fechaHorometro', index: 'fechaHorometro', width: 90 },
                                             { name: 'horasPromedioMensual', index: 'horasPromedioMensual', width: 90 },
                                             { name: 'horometroFinal', index: 'horometroFinal', width: 90 },
                                             { name: 'descripRenovacion', index: 'descripRenovacion', width: 60, align: 'center' },
                                             { name: 'descripRenovacionValida', index: 'descripRenovacionValida', width: 60, align: 'center' },
                                             { name: 'departamento', index: 'departamento', width: 200, width: 160 },
                                             { name: 'maquinaNueva', index: 'maquinaNueva', sorttype: "boolean", formatter: 'checkbox', hidden: true },
                                             { name: 'renovacion', index: 'renovacion', sorttype: "boolean", formatter: 'checkbox', hidden: true },
                                             { name: 'renovacionValida', index: 'renovacionValida', sorttype: "boolean", formatter: 'checkbox', hidden: true },
                                             { name: 'familiaOt', index: 'familiaOt', hidden: true },
                                             { name: 'modeloBaseOt', index: 'modeloBaseOt', hidden: true },
                                             { name: 'prefijoOt', index: 'prefijoOt', hidden: true },
                                             { name: 'numeroMaquinas', index: 'numeroMaquinas', hidden: true },
                                             { name: 'numeroSerieOt', index: 'numeroSerieOt', hidden: true },
                                             { name: 'codDepartamento', index: 'codDepartamento', hidden: true },
                                             { name: 'montoItem', index: 'montoItem', hidden: true }
                          ],
                pager: "#pgMaquinaria",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                rowNum: "5",
                rowList: [5, 10, 15],
                viewrecords: true,
                rownumbers: true,
                toolbar: [true, "top"],
                shrinkToFit: false,
                width: 850,
                height: 120
            });

            jQuery("#gvMaquinaria").jqGrid('navGrid', '#pgMaquinaria', { search: false, refresh: false, edit: false, add: false, del: false });

            var menuMaquinaria="<table><tr><td style='vertical-align: middle'>";
            menuMaquinaria = menuMaquinaria +"<a id='lbnNuevo'>Nuevo</a> |" ;
            menuMaquinaria = menuMaquinaria + "<a id='lbnEditar'>Editar</a> |"
            menuMaquinaria = menuMaquinaria +"<a id='lbnEliminar'>Eliminar</a> |"
            menuMaquinaria = menuMaquinaria +"<a id='lbnCargarMaquina'>Cargar</a> "
             menuMaquinaria = menuMaquinaria + "</td></tr></table>";

            jQuery("#t_gvMaquinaria").append(menuMaquinaria);
            jQuery("a", "#t_gvMaquinaria").click(function (event) {
                var control = event.target.id;
                var idRow, rowGrid;
//                if (control == 'lbnNuevo') {
//                    nuevaMaquinaria();
//                } else {
//                    idRow = jQuery("#gvMaquinaria").jqGrid('getGridParam', 'selrow');
//                    if (!idRow) {
//                        return alert('Por favor seleccione una fila');
//                    }
//                    rowGrid = jQuery("#gvMaquinaria").jqGrid('getRowData', idRow);
//                    if (control == 'lbnEditar') {
//                        editarMaquinaria(rowGrid);
//                    } else {
//                        eliminarMaquinaria(rowGrid);
//                    }
//                }
                if (control == 'lbnEditar' || control == 'lbnEliminar') { 
                    idRow = jQuery("#gvMaquinaria").jqGrid('getGridParam', 'selrow');
                    if (!idRow) {
                        return alert('Por favor seleccione una fila');
                    }
                    rowGrid = jQuery("#gvMaquinaria").jqGrid('getRowData', idRow);                    
                }

                  switch (control) {
                    case 'lbnNuevo':
                       nuevaMaquinaria();
                     break;
                    case 'lbnEditar':
                       editarMaquinaria(rowGrid);
                     break;
                    case 'lbnEliminar':
                       eliminarMaquinaria(rowGrid);
                     break;
                     case 'lbnCargarMaquina':
                     CargarMaquinaria();
                     break;
                  }

            });

            $("#jqgBuscarListaMaquinaria").jqGrid({
                colNames: [' ','IdMaquinaria', 'Familia', 'Modelo', 'Modelo Base','Prefijo','Tipo','Nro Serie','Horometro','Fecha Horom','Hrs Prom.Uso','Horom.Fin','Renov','Renov.Val','Departamento'],
                colModel: [
                            { name: 'sel', width: 40 },
                            { name: 'IdMaquinaria', index: 'IdMaquinaria'},
                            { name: 'Familia', index: 'Familia', width: 300 },
                            { name: 'Modelo', index: 'Modelo', width: 100 },
                            { name: 'ModeloBase', index: 'ModeloBase', width: 100 },
                            { name: 'Prefijo', index: 'Prefijo', width: 100 },
                            { name: 'Tipo', index: 'Tipo', width: 100, hidden: true },
                            { name: 'NroSerie', index: 'NroSerie', width: 100 },
                            { name: 'Horometro', index: 'Horometro', width: 100 },
                            { name: 'FechaHorom', index: 'FechaHorom', width: 100 },
                            { name: 'HrsPromoUso', index: 'HrsPromoUso', width: 100 },
                            { name: 'HoromFin', index: 'HoromFin', width: 100 },
                            { name: 'Renov', index: 'Horometro', width: 100,hidden: true},
                            { name: 'RenovVal', index: 'FechaHorom', width: 100, hidden: true},
                            { name: 'Departamento', index: 'HrsPromoUso', width: 160 }
                          ],
                height: 400,
                width: 870,
                viewrecords: true,
                cmTemplate: { sortable: false },
                rownumbers: true,
                shrinkToFit: false
            });

            jQuery("#iddlFamilia").bind("change", function (e) {

                jQuery("#iddlModeloBase").find('option').remove().end();
                jQuery("#iddlModeloBase").attr('disabled', 'true');

                $("#iddlPrefijo").find('option').remove().end();
//                $("#iddlPrefijo").attr('disabled', 'true');

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = "familia";
                arreglo[0][1] = jQuery("#iddlFamilia").val();
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    url: location.pathname + "/ConsultarModeloBase",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            jQuery("#iddlModeloBase").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        jQuery('#iddlModeloBase').removeAttr('disabled');
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') {
                            jQuery('#iddlModeloBase').val(jQuery("#ihddModeloBase").get(0).value);
                            jQuery("#iddlModeloBase").change();
                        }
                    }
                });

                if (jQuery("#iddlFamilia").val() == "0") {
                    jQuery("#itxtFamilia").css("display", "block");
                    jQuery("#itxtFamilia").focus();
                } else {
                    jQuery("#itxtFamilia").css("display", "none");
                }

            });

            jQuery("#iddlModeloBase").bind("change", function (e) {

                jQuery("#iddlPrefijo").find('option').remove().end();
                jQuery("#iddlPrefijo").attr('disabled', 'true');

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = "familia";
                arreglo[0][1] = jQuery("#iddlFamilia").val();
                arreglo[1][0] = "modeloBase";
                arreglo[1][1] = jQuery("#iddlModeloBase").val();
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarPrefijo",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            jQuery("#iddlPrefijo").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        jQuery('#iddlPrefijo').removeAttr('disabled');
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') {
                            jQuery('#iddlPrefijo').val(jQuery("#ihddPrefijo").get(0).value);
                            jQuery("#iddlPrefijo").change();
                        }
                    }
                });

                if (jQuery("#iddlModeloBase").val() == "0") {
                    jQuery("#itxtModeloBase").css("display", "block");
                    jQuery("#itxtModeloBase").focus();
                } else {
                    jQuery("#itxtModeloBase").css("display", "none");
                }

            });

            jQuery("#iddlPrefijo").bind("change", function (e) {
            
                jQuery("#iddlNroSerie").find('option').remove().end();
                jQuery("#iddlNroSerie").attr('disabled', 'true');

                 
                if (jQuery("#iddlPrefijo").val()=='0')
                {
                    $("#itxtPrefijo").css("display", "block");
                    $("#itxtPrefijo").focus();
                    return;
                } 

                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = "prefijo";
                arreglo[0][1] = jQuery("#iddlPrefijo").val();
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarNroSerie",
                    data: parametro,
                    dataType: "json",
                    type: "post", 
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            jQuery("#iddlNroSerie").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        jQuery('#iddlNroSerie').removeAttr('disabled');
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') { 

                            jQuery('#iddlNroSerie').val(jQuery("#ihddNroSerie").get(0).value); 
                             
                             if (jQuery("#iddlNroSerie").val() == "0") {
                                jQuery("#itxtNroSerie").css("display", "block");
                                jQuery("#itxtNroSerie").focus();
                            } else {
                                jQuery("#itxtNroSerie").css("display", "none");
                             }                       
                        }
                    }
                });

                 

                if ($("#iddlPrefijo").val() == "0") {
                    $("#itxtPrefijo").css("display", "block");
                    $("#itxtPrefijo").focus();
                } else {
                    $("#itxtPrefijo").css("display", "none");
                }
                 
                jQuery("#iddlNroSerie").change();

            });

            jQuery("#itxtPrefijo").bind("blur", function (e) {
             
                jQuery("#iddlNroSerie").find('option').remove().end();
                jQuery("#iddlNroSerie").attr('disabled', 'true');
                 

                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = "prefijo";
                arreglo[0][1] = jQuery("#itxtPrefijo").val();
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarNroSerie",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            jQuery("#iddlNroSerie").append(jQuery("<option></option>").val(this['codigo']).html(this['descripcion']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    },
                    complete: function () {
                        jQuery('#iddlNroSerie').removeAttr('disabled');
                        if (jQuery("#ihddTipoOperacion").get(0).value == '2') { 

                            jQuery('#iddlNroSerie').val(jQuery("#ihddNroSerie").get(0).value); 
                             
                             if (jQuery("#iddlNroSerie").val() == "0") {
                                jQuery("#itxtNroSerie").css("display", "block");
                                jQuery("#itxtNroSerie").focus();
                            } else {
                                jQuery("#itxtNroSerie").css("display", "none");
                             }                       
                        }
                    }
                });

            });

            jQuery("#iddlNroSerie").bind("change", function (e) {
             
                if (jQuery("#iddlNroSerie").val() == "0") {
                    jQuery("#itxtNroSerie").css("display", "block");
                    jQuery("#itxtNroSerie").focus();
                } else {
                    jQuery("#itxtNroSerie").css("display", "none");
                }

            });


            jQuery('#btnGenerarCotizacion').bind("click", function (e) {
                jQuery("#idDialogLista").dialog("open");
            });

            jQuery('#btnGenerarCotizacionPrevia').bind('click', function(){            
                GenerarCotizacionPrevia();
            }); 


            jQuery('#gvOfertaValor').jqGrid({
                datatype: 'local',
                height: 460,
                colNames: ['Ide', 'NumCotizacionOculto', 'Nro Cotización', 'Producto(s)', '', 'Version', 'Fecha', 'Valor Negociado', 'Moneda', 'Archivo', 'Detalle Partes'],
                colModel: [
                        { name: 'Ide', hidden: true },
                        { name: 'NumCotizacionOculto', hidden: true },
                        { name: 'NroCotizacion', width: 100, sorttype: 'string', align: "center" },
                        { name: 'Productos', width: 320, sorttype: 'string' },
                        { name: 'VerLista', width: 60, sorttype: 'string', align: "center" },
                        { name: 'Version', width: 60, sorttype: 'string', align: "center" },
                        { name: 'Fecha', width: 90, sorttype: 'string', align: "center" },
                        { name: 'ValorNegociado', width: 120, align: "right",
                            formatter: 'number',
                            formatoptions: { decimalPlaces: 2, defaultValue: '0.00', thousandsSeparator: ',', decimalSeparator: '.' }
                        },
                        { name: 'Moneda', width: 80, sorttype: 'string', align: "center" },
                        { name: 'Archivo', width: 270, align: 'center' },
                        { name: 'DetallePartes', width: 170, align: 'center' }
                      ],
                rowNum: 10,
                loadtext: 'Cargando datos...',
                emptyrecords: 'No hay resultados',
                rownumbers: true,
                viewrecords: true,
                caption: "Cotizaciones",
                cmTemplate: { sortable: false },
                afterInsertRow: function (rowid, rowdata, rowelem) {
                    ConfigControlSubir('ControlGrillaSubir' + (rowid - 1), (rowid - 1));
                },
                gridComplete: function () {
                }
            });


            function BuscarOfertaValor() {
            
            arreglo = parametro = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = "session";
            arreglo[0][1] = $('#MainContent_lblIdSession').html();
            parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/ConsultarOfertaValor",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        //===Destruir los controles Uploadify de la grilla ==
                        //                        var arrayId = $('#gvOfertaValor').getDataIDs();
                        //                        var cantidRegistro = arrayId.length.toString();
                        //                        for (var i = 0; i < cantidRegistro; i++) {
                        //                            var ControlSubir = 'ControlSubir' + i;
                        //                            $('#' + ControlSubir).uploadify('destroy');
                        //                        }
                        //==========================================
                        jQuery("#gvOfertaValor").clearGridData();
                        var lista = response.d;
                        if (lista.length == 0) {
                        } else {
                            for (var i = 0; i < lista.length; i++) {
                                var NumeroCotizacion = "";

                                // Validar valor null para evitar el bloqueo de pantalla
                                if (lista[i].DescripcionProducto==null)
                                {
                                    lista[i].DescripcionProducto='';
                                }

                                NumeroCotizacion = Number(lista[i].IdCotizacionSap.toString());
                                var urlDescarga = '../frmDescargarFTP.aspx?NombreArchivo=' + lista[i].NombreArchivo + '&Opc=COTIZACION';
                                //                                var textoNroCotizacion = parseInt(NumeroCotizacion)
                                NumeroCotizacion = NumeroCotizacion + '-' + Number(lista[i].NumVersion);
                                var urlArchivo = '<a href ="' + urlDescarga + '" target="_blank" >' + NumeroCotizacion.toString() + '</a>'

                                var DescripProducto = "";
                                var VerLista = '<a href="#"  onclick ="consultarProductoVersion(' + lista[i].IdCotizacionVersion + '); return false;" >Ver Lista</a>';

                                DescripProducto = lista[i].DescripcionProducto.toString();

                                var ControlSubir = '<input id="ControlGrillaSubir' + i + '" type="button" class="classControlGrillaSubir" value="Actualizar" style ="float:left"/>';
                                var ControlArchivo = '<a id="lklArchivoGrillaActualizar' + i + '" href ="' + urlDescarga + '" target="_blank" style ="float:left">' + lista[i].NombreArchivo + '</a>';
                                var ActualizarArchivo = '<div style="height:20px;"><div style ="float:left;padding-right :10px;">' + ControlSubir + '</div><div>' + ControlArchivo + '</div></div> ';

                                //----Construir Nombre de Detalle partes excel-------------------------                           

                                var VerDetallePartes = '';
                                var NombreArchivoDetalleParte = '';
                                if (lista[i].NombreArchivo.length > 0) {
                                    var UltimaPosicion = lista[i].NombreArchivo.lastIndexOf('.');
                                    if (UltimaPosicion > 0) {
                                        NombreArchivoDetalleParte = lista[i].NombreArchivo.substring(0, UltimaPosicion);
                                    }
                                }
                                if (NombreArchivoDetalleParte != '') {
                                    NombreArchivoDetalleParte = NombreArchivoDetalleParte + '.xlsx'
                                }

                                var UrlDescargaDetallePartes = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreArchivoDetalleParte + '&Opc=DETALLE_PARTES'; ;

                                if (lista[i].TieneDetallePartes != null && lista[i].TieneDetallePartes != '' && lista[i].TieneDetallePartes != '0') {
                                    VerDetallePartes = '<a id="lklArchivoDetallePartes' + i + '" href ="' + UrlDescargaDetallePartes + '" target="_blank" style ="float:left">' + NombreArchivoDetalleParte + '</a>';
                                }
                                //------------------------------------------------------------

                                var mydata = [
                                { Ide: lista[i].IdCotizacionVersion,
                                    NumCotizacionOculto: lista[i].IdCotizacionSap.toString(),
                                    NroCotizacion: urlArchivo,
                                    Productos: DescripProducto,
                                    VerLista: VerLista,
                                    Version: lista[i].NumVersion,
                                    Fecha: lista[i].Fecha,
                                    ValorNegociado: lista[i].ValorNegociado,
                                    Moneda: lista[i].Moneda,
                                    Archivo: ActualizarArchivo,
                                    DetallePartes: VerDetallePartes
                                }
                            ];
                                jQuery("#gvOfertaValor").jqGrid('addRowData', i + 1, mydata[0]);
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

                // para asinar los roles de bloqueo segun el rol del usuario
                AsignarRolesBloqueo(jQuery("#hdfvalRolBloqueo").val());
            }

            function ConfigControlSubir(NombreControl, Indice) {

                if ($('#' + NombreControl).length) {

                    //$('#' + NombreControl).button();
                    $('#' + NombreControl).bind("click", function (e) {
                        window.frameSW = false;
                        var NombreArchivo = $('#lklArchivoGrillaActualizar' + Indice).html();
                        var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreArchivo + '&Opc=COTIZACION';
                        $("#lklNombreArchivoActualizar").html(NombreArchivo);

                        $('#hdfNombreLinkArchivo').val('lklArchivoGrillaActualizar'+Indice);
                        var rowGrid = jQuery("#gvOfertaValor").jqGrid('getRowData', (Indice+1));
                        $('#hdfIdCotizacionVersion').val(rowGrid.Ide);
                        
                        //Asignar la url para descargar el archivo
                        $('#lklNombreArchivoActualizar').attr('href', urlArchivo);
                        $('#lklNombreArchivoActualizar').attr('target', '_blank');
                        $("#diagActualizarArchivo").dialog("open");
                        
                        if (navigator.appName == "Microsoft Internet Explorer") {
                //                                window.document.getElementById('myFrame').contentWindow.location.reload(true);
                            window.document.getElementById("myFrame").src = window.document.getElementById("myFrame").src;
                        }
                    });
                }
            }

            
            // Inicio:Buscar Oferta de valor
            BuscarOfertaValor();

            jQuery(".classVerLista").click(function (event) {
                $("#diagProductoVersion").dialog("open");

            });

            // Detalle Prime
            //===================================================================

            jQuery('#btnDetalleAceptar').button();
            jQuery('#btnDetalleCancelar').button();

            jQuery("#diagDetalleProductoPrime").dialog({
                autoOpen: false,
                height: 670,
                width: 700,
                resizable: false,
                modal: true,
                close: function () { }
            });

            // Detalle de Producto Solucion combinada
            jQuery('#btnAceptarProductoSolComb').button();
            jQuery('#btnCancelarProductoSolComb').button();

            jQuery("#diagDetalleProductoSolucionCombinada").dialog({
                autoOpen: false,
                height: 'auto',//320,
                width: 700,
                resizable: false,
                modal: true,
                close: function () { }
            });


            jQuery('#jqgridDetalleAdicional').jqGrid({
                datatype: 'local',
                height: 100,
                width: 500,
                colNames: ['Ide', 'Codigo', 'Descripción', 'Cantidad', 'Unidad de Medida', 'Eliminado','Incluir.Esp.Técnica'],
                colModel: [
                        { name: 'Ide', hidden: true },
                        { name: 'NroCodigo', width: 80, sorttype: 'string' },
                        { name: 'Descripcion', width: 200, sorttype: 'string' },
                        { name: 'Cantidad', width: 50, sorttype: 'string' },
                        { name: 'UnidadMedida', width: 100, sorttype: 'string' },
                        { name: 'Eliminado', width: 100, sorttype: 'string', hidden: true },
                        { name: 'IncluirEspTec', width: 50, sorttype: 'string', align: "center" }
                      ],
                rowNum: 10,
                rowList: [10, 20, 30],
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: 'desc'
            });

             jQuery('#jqgridDetalleAccesorio').jqGrid({
                datatype: 'local',
                height: 100,
                width: 500,
                colNames: ['Ide', 'Codigo', 'Descripción', 'Cantidad', 'Unidad de Medida', 'Eliminado','Incluir.Esp.Técnica'],
                colModel: [
                        { name: 'Ide', hidden: true },
                        { name: 'NroCodigo', width: 80, sorttype: 'string' },
                        { name: 'Descripcion', width: 200, sorttype: 'string' },
                        { name: 'Cantidad', width: 50, sorttype: 'string' },
                        { name: 'UnidadMedida', width: 100, sorttype: 'string' },
                        { name: 'Eliminado', width: 100, sorttype: 'string', hidden: true },
                        { name: 'IncluirEspTec', width: 50, sorttype: 'string', align: "center" }
                      ],
                rowNum: 10,
                rowList: [10, 20, 30],
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: 'desc'
            });
            // //Accion de Cabecera
            //            jQuery('#t_jqgridDetalleAdicional').append("<table><tr><td style='vertical-aling: middle'><a id='lblEliminar'>Eliminar</a></td></tr></table>");

            //            jQuery('a', '#t_jqgridDetalleAdicional').click(function (event) {

            //                var control = event.target.id;
            //                switch (control) {
            //                    case 'lblNuevo':
            //                        //NuevoModelo();
            //                        break;
            //                    case 'lblEditar':
            //                        //EditarModelo();
            //                        break;
            //                    case 'lblEliminar':
            //                        OcultarAdicional();
            //                        break;
            //                }

            //            });
            //===================================================================== 
            // Detalle Producto Alquiler 

            $('#btnAceptarProductoAlquiler').button();
            $('#btnCancelarProductoAlquiler').button();

            $("#diagDetalleProductoAlquiler").dialog({
                autoOpen: false,
                height: 320,
                width: 700,
                resizable: false,
                modal: true,
                close: function () { }
            });

            //Detalle Producto Accesorio
            $('#btnAceptarProductoAccesorio').button();
            $('#btnCancelarProductoAccesorio').button();

            $("#diagDetalleAccesorio").dialog({
                autoOpen: false,
                height: 320,
                width: 700,
                resizable: false,
                modal: true,
                close: function () { }
            });



            function AbrirDetallePrime(IdProducto) {

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdProducto';
                arreglo[0][1] = IdProducto.toString();
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/BuscarProductoPrimeId",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var eProducto = response.d;
                        if (eProducto != null) {

                            //1.Cargar Detalla Producto
                            $('#hdfIdProducto').val(eProducto.IdProducto);
                            $('#txtDetalleIDProducto').val(eProducto.IdProductoSap);
                            $('#txtDetalleProducto').val(eProducto.Descripcion);
                            $('#txtDetalleEstado').val(eProducto.NombreEstado);
                            $('#txtDetalleValorUnitario').val(eProducto.ValorUnitario);
                            $('#txtDetalleCantidad').val(eProducto.Cantidad);
                            $('#txtDetalleValorFinal').val(eProducto.ValorLista);
                            $('#txtDetalleFechaEstimadoCierre').val(eProducto.beProductoPrime.FechaEstimCierre);
                            $('#txtDetallePlazoEntregaProducto').val(eProducto.beProductoPrime.PlazoEntregaEstim);

                            $('#txtDetalleValorLista').val(eProducto.ValorLista);
                            $('#txtDetalleValorReal').val(eProducto.ValorReal);
                            $('#txtDetalleDescuentoPorcent').val(eProducto.PorcDescuento);
                            $('#txtDetalleDescuentoImporte').val(eProducto.DescuentoImp);
                            $('#txtDetalleFlete').val(eProducto.Flete);
                            $('#txtDetalleValorVentaTotal').val(eProducto.ValorVenta);
                            $('#txtDetalleImpuestoPorcentaje').val(eProducto.PorcImpuesto);
                            $('#txtDetallePrecioVentaTotal').val(eProducto.PrecioVentaFinal);


                            var incluyeCLC = eProducto.beProductoPrime.FlatIncluyeCLC
                            switch (incluyeCLC.toString().toUpperCase()) {
                                case '1':
                                    jQuery('#chkbDetalleIncluyeCLC').attr('checked', true);
                                    break;
                                case 'TRUE':
                                    jQuery('#chkbDetalleIncluyeCLC').attr('checked', true);
                                    break;
                                default:
                                    jQuery('#chkbDetalleIncluyeCLC').attr('checked', false);
                            }

                            //2.- Cargar Adicionales
                            jQuery("#jqgridDetalleAdicional").clearGridData();
                            var lista = eProducto.beProductoPrime.ListabeProductoAdicional;
                            if (lista.length == 0) {
                            } else {
                                for (var i = 0; i < lista.length; i++) {

                                var incluirEspecificacionTecnic='NO';
                                if (lista[i].FlatMostrarEspTecnica.toUpperCase()  == 'TRUE')
                                { incluirEspecificacionTecnic='SI';}

                                    var mydata = [
                                                        { Ide: lista[i].IdProductoAdicional,
                                                            NroCodigo: lista[i].CodigoProductoAdicional.toString(),
                                                            Descripcion: lista[i].NombreProdutoAdicional,
                                                            Cantidad: lista[i].Cantidad,
                                                            UnidadMedida: lista[i].UnidadMedida,
                                                            IncluirEspTec:incluirEspecificacionTecnic
                                                        }
                                                        ];

                                    jQuery("#jqgridDetalleAdicional").jqGrid('addRowData', i + 1, mydata[0]);
                                }
                            }

                            //3.- Cargar Accesorio
                            jQuery('#jqgridDetalleAccesorio').clearGridData();
                            var listAccesorio= eProducto.beProductoPrime.ListabeProductoAccesorio;
                            if ( listAccesorio.length==0){
                            }else{
                                for (var j=0; j< listAccesorio.length; j++){

                                var incluirEspecificacionTecnAcces='NO';
                                if (listAccesorio[j].FlatMostrarEspTecnica.toUpperCase()  == 'TRUE')
                                { incluirEspecificacionTecnAcces='SI';}

                                var mydataAccesor=[
                                                {ide:listAccesorio[j].IdProductoAccesorio,
                                                    NroCodigo:listAccesorio[j].IdAccesorio,
                                                    Descripcion: listAccesorio[j].NombreProductoAccesorio,
                                                    Cantidad: listAccesorio[j].Cantidad,
                                                    UnidadMedida: listAccesorio[j].UnidadMedida,
                                                    IncluirEspTec:incluirEspecificacionTecnAcces
                                                 }
                                           ];
                                jQuery('#jqgridDetalleAccesorio').jqGrid('addRowData',j + 1,mydataAccesor[0]);
                                }
                            }

                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

                //Abrir Popup
                jQuery('#diagDetalleProductoPrime').dialog('open');

                var valWidth = jQuery('#diagDetalleProductoPrime').width();
                valWidth = (valWidth * 0.98);
                jQuery('#divDetallBotonesPrime').attr('width', valWidth);
                jQuery('#jqgridDetalleAdicional').setGridWidth(valWidth);
                jQuery('#jqgridDetalleAccesorio').setGridWidth(valWidth);
            }

            function AbrirDiagDetalleProductoSolucionCombinada(IdProducto)
            {

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdProducto';
                arreglo[0][1] = IdProducto.toString();
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/BuscarProductoSolucionCombinadaId",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var eProducto = response.d;
                        if (eProducto != null) {

                                $('#txtIdProductoSolComb').val(eProducto.IdProductoSap);
                                $('#txtNombreProductoSolComb').val(eProducto.Descripcion);
                                $('#txtEstadoProductoSolComb').val(eProducto.NombreEstado);
                                $('#txtValorNetoProductoSolComb').val(eProducto.ValorNeto);
                                $('#txtDuracionPlanProductoSolComb').val(eProducto.beProductoSolucionCombinada.Duracion);
                                $('#txtPlazoRenovacionProductoSolComb').val(eProducto.beProductoSolucionCombinada.PlazoRenovacion);
                                $('#txtFechaInicioContratoProductoSolComb').val(eProducto.beProductoSolucionCombinada.FechaInicioContrato);
                                $('#txtFechaEstimadoCierreProductoSolComb').val(eProducto.beProductoSolucionCombinada.FechaEstimadaCierre);
                                $('#txtPorcentajeVendedor1ProductoSolComb').val(eProducto.beProductoSolucionCombinada.ParticipacionVendedor1);
                                $('#txtPorcentajeVendedor2ProductoSolComb').val(eProducto.beProductoSolucionCombinada.ParticipacionVendedor2);
                                $('#txtValorListaProductoSolComb').val(eProducto.ValorLista);
                                $('#txtValorRealProductoSolComb').val(eProducto.ValorReal);
                                $('#txtPorcentajeDescuentoProductoSolComb').val(eProducto.PorcDescuento);
                                $('#txtImporteDescuentoProductoSolComb').val(eProducto.DescuentoImp);
                                $('#txtFleteProductoSolComb').val(eProducto.Flete);
                                $('#txtValorVentaTotalProductoSolComb').val(eProducto.ValorVenta);
                                $('#txtPorcentajeImpuestoProductoSolComb').val(eProducto.PorcImpuesto);
                                $('#txtPrecioVentaTotalProductoSolComb').val(eProducto.PrecioVentaFinal);                             
                                $('#hdfCodigoLineaProductoSolComb').val(eProducto.CodigoLinea);  

                                var llaveConfiguracion = '';
                                llaveConfiguracion= CrearLlaves(eProducto.CodigoLinea);
                                $('#divConfiguracionLlavesProducto').empty();
                                $('#divConfiguracionLlavesProducto').append(llaveConfiguracion);

                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //jAlert.error('<h1>Cotizador</h1><br/>' + textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
                jQuery('#diagDetalleProductoSolucionCombinada').dialog('open');
            }
            
            function CrearLlaves(CodigoLinea){
                var divControles='';
                var nombreLLave='';
                var nombreCampo='';
                var nombreComboLlave='';
                var nombreSpanLlave='';
                var cantidLLave=0;
                var ElemHtmlSelect='';
                var varoption=''; 
                var flatCambio='NO'; 
                var dependencia =''               
            if (CodigoLinea == ''){
                alert('No se encuentra Linea asignada.');
                return;
            }
            var arreglo = null;
            var parametro = null;
            arreglo = fc_redimencionarArray(2);
            arreglo[0][0] = "session";
            arreglo[0][1] = $('#MainContent_lblIdSession').html();
            arreglo[1][0] = "codigoLinea";
            arreglo[1][1] = CodigoLinea;
            parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/ConsultarListaLLave",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) { 
                        var lista = response.d;
                        if (lista.length > 0) {                       

                            nombreLLave= lista[0].DescripcionCorta;
                            nombreCampo=lista[0].Campo;
                            divControles='<table>';
                            for (var i = 0; i < lista.length; i++) {

                                   if ( nombreLLave != lista[i].DescripcionCorta)
                                   {
                                    flatCambio='SI';
                                   }

                                if( flatCambio!='SI')
                                {
                                    varoption = varoption + '<option value="' + nombreCampo +'" >' + lista[i].DescripcionLarga + '</option>';
                                }                                                               

                                if( flatCambio=='SI')
                                {
                                    cantidLLave = parseInt(parseInt(cantidLLave) + 1);
                                    nombreComboLlave='ComboLLave' +  cantidLLave
                                    nombreSpanLlave='spanLlave'+ cantidLLave
                                    varoption = '<option value="' + nombreCampo+ '" >' + lista[i-1].valorDefault + '</option>' + varoption;                               
                                    ElemHtmlSelect= '<select onchange="eventoChange('+cantidLLave+')" id="'+nombreComboLlave+'" campo ="'+nombreCampo+ '" >'+varoption+'</select>';
                                    divControles= divControles + '<tr><td><span id ="'+nombreSpanLlave+'">'+nombreLLave+'</span></td><td> '+ElemHtmlSelect+'</td> </tr>';
                                    // Reiniciar valores
                                     nombreLLave = lista[i].DescripcionCorta 
                                     nombreCampo=lista[i].Campo 
                                     //varoption = ''; 
                                     flatCambio='NO'
                                     varoption = '<option value="' + nombreCampo + '" >' + lista[i].DescripcionLarga + '</option>';                                      
                                }
                                 
                                if (i == lista.length -1)
                                 {  
                                    cantidLLave = parseInt(parseInt(cantidLLave) + 1);
                                    nombreComboLlave='ComboLLave' +  cantidLLave
                                    nombreSpanLlave='spanLlave'+ cantidLLave 
                                    varoption = '<option value="' + nombreCampo +'" >' + lista[i].valorDefault + '</option>' + varoption;                            
                                    ElemHtmlSelect= '<select onchange="eventoChange('+cantidLLave+')" id="'+nombreComboLlave+'" campo="'+nombreCampo+ '" >'+varoption+'</select>';
                                    divControles= divControles + '<tr><td><span id ="'+nombreSpanLlave+'">'+nombreLLave+'</span></td><td> '+ElemHtmlSelect+'</td> </tr>';
                                    // Reiniciar valores
                                     nombreLLave = lista[i].DescripcionCorta  
                                      nombreCampo=lista[i].Campo 
                                     flatCambio='NO'
                                     varoption = '';
                                 }
                            }
                            divControles=divControles + '<tr><td></td><td> <input id="hdfCantidaLllaves" type="hidden"  value="'+cantidLLave+'"/></td> </tr>' + '</table>';
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
             
                return divControles;
            }
            
            function CerrarDiagDetalleProductoSolucionCombinada() {
                jQuery('#diagDetalleProductoSolucionCombinada').dialog('close');
            }

            function CerrarDiagDetallePrime() {
                jQuery('#diagDetalleProductoPrime').dialog('close');
            }

            function AbrirDetalleAlquiler(IdProducto) {
                //Abrir Popup

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdProducto';
                arreglo[0][1] = IdProducto.toString();
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/BuscarProductoAlquilerId",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var eProducto = response.d;
                        if (eProducto != null) {

                            //1.Cargar Detalla Producto
                            $('#hdfIdProductoAlquiler').val(eProducto.IdProducto);
                            $('#txtIdDetalleProductoAlquiler').val(eProducto.IdProductoSap);
                            $('#txtNombreProductoAlquiler').val(eProducto.Descripcion);
                            $('#txtEstadoProductoAlquiler').val(eProducto.NombreEstado);
                            $('#txtValorUnitarioProductoAlquiler').val(eProducto.ValorUnitario);
                            $('#txtCantidadProductoAlquiler').val(eProducto.Cantidad);
                            $('#txtValorNetoProductoAlquiler').val(eProducto.ValorNeto);
                            $('#txtTipoAlquiler').val(eProducto.beProductoAlquiler.DesTipoAlquiler);
                            $('#txtTipoPago').val(eProducto.beProductoAlquiler.DesTipoPago);
                            $('#txtTipoFacturacion').val(eProducto.beProductoAlquiler.DesTipoFacturacion);
                            $('#txtMesAlquiler').val(eProducto.beProductoAlquiler.DesMesAlquilar);


                            $('#txtValorListaAlquiler').val(eProducto.ValorLista);
                            $('#txtValorRealAlquiler').val(eProducto.ValorReal);
                            $('#txtDescuentoPorcentAlquiler').val(eProducto.PorcDescuento);
                            $('#txtDescuentoImporteAlquiler').val(eProducto.DescuentoImp);
                            $('#txtFleteAlquiler').val(eProducto.Flete);
                            $('#txtValorVentaTotalAlquiler').val(eProducto.ValorVenta);
                            $('#txtImpuestoPorcentajeAlquiler').val(eProducto.PorcImpuesto);
                            $('#txtPrecioVentaTotalAlquiler').val(eProducto.PrecioVentaFinal);

                            $('#diagDetalleProductoAlquiler').dialog('open');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function AbrirDetalleAccesorio(IdProducto) {
                //Abrir Popup

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdProducto';
                arreglo[0][1] = IdProducto.toString();
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/BuscarProductoAccesorioId",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var eProducto = response.d;
                        if (eProducto != null) {

                            //1.Cargar Detalla Producto
                            $('#hdfIdProductoAccesorio').val(eProducto.IdProducto);
                            $('#txtIdProductoAccesorio').val(eProducto.IdProductoSap);
                            $('#txtNombreProductoAccesorio').val(eProducto.Descripcion);
                            $('#txtEstadoProductoAccesorio').val(eProducto.NombreEstado);
                            $('#txtValorUnitarioProductoAccesorio').val(eProducto.ValorUnitario);
                            $('#txtCantidadProductoAccesorio').val(eProducto.Cantidad);
                            $('#txtValorNetoProductoAccesorio').val(eProducto.ValorNeto);

                            $('#txtValorListaProductoAccesorio').val(eProducto.ValorLista);
                            $('#txtValorRealProductoAccesorio').val(eProducto.ValorReal);
                            $('#txtDescuentoPorcentProductoAccesorio').val(eProducto.PorcDescuento);
                            $('#txtDescuentoImporteProductoAccesorio').val(eProducto.DescuentoImp);
                            $('#txtFleteProductoAccesorio').val(eProducto.Flete);
                            $('#txtValorVentaProductoAccesorio').val(eProducto.ValorVenta);
                            $('#txtImpuestoPorcentProductoAccesorio').val(eProducto.PorcImpuesto);
                            $('#txtPrecioVentaProductoAccesorio').val(eProducto.PrecioVentaFinal);

                            $('#diagDetalleAccesorio').dialog('open');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function CerrarDiagDetalleAlquiler() {
                $('#diagDetalleProductoAlquiler').dialog('close');
            }

            function CerrarDiagDetalleAccesorio() {
                $('#diagDetalleAccesorio').dialog('close');
            }

            function BuscarCotizacionCargar(){

                jQuery("#jqgBuscarListaMaquinaria").clearGridData();

                var numcot= $('#txtBuscarNroCotizacion').val();                   

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = 'numeroCotizacion';
                arreglo[0][1] = numcot.toString();

                parametro = fc_parametrosData(arreglo);

                $("#cbBuscarPosicionProducto option").remove();

            $.ajax({
                type: "POST",
                url: location.pathname + "/BuscarCotizacionCargar",
                data: parametro,
                async:false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) { 
                    var lista = response.d;
                    if (lista.length == 0) {
                        $('#txtBuscarNroCotizacion').focus();
                        alert('No se encontró ninguna Cotizacion');
                    } else {
                        $('#cbBuscarPosicionProducto').append('<option value="0" selected="selected">Seleccione... </option>')
                        for (var i = 0; i < lista.length; i++) {
                            
                            $('#hdfIdCotizacionBusqueda').val(lista[i].IdCotizacion );
                            $('#txtBuscarNroCotizacion').val(lista[i].IdCotizacionSap );

                            var varoption = '<option value="' + lista[i].IdProductoSap + '" >' + lista[i].IdPosicion + '</option>';
                            $('#cbBuscarPosicionProducto').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        
            }

            function CerrarDiagCargarMaquinaria() {
                $('#divDialogBuscarMaquinaria').dialog('close');
            }

            function aceptarCargarMaquinaria() { 
                var valPosicion = $('#cbBuscarPosicionProducto :selected').text(); 
                var valorCod = $('#cbBuscarPosicionProducto').val();
                var nroCotizacion = $('#txtBuscarNroCotizacion').val();     
                var arrayId = jQuery('#jqgBuscarListaMaquinaria').getDataIDs();
                var cantidRegistro = arrayId.length.toString();
                var CodMaquinarias='';
                
                for (var i = 0; i < cantidRegistro; i++) {
                var control=$('#chbSelCargarMaquina'+i);
                var val=$('#chbSelCargarMaquina'+i).attr('checked');
                 
                    if ($('#chbSelCargarMaquina'+i).is(':checked')) 
                    {                        
                        rowGrid = jQuery("#jqgBuscarListaMaquinaria").jqGrid('getRowData', arrayId[i]);
                        CodMaquinarias= CodMaquinarias + ',' + rowGrid.IdMaquinaria ;
                    }
                }
                  

            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(5);
            arreglo[0][0] = 'IdCotizacionSap';
            arreglo[0][1] = nroCotizacion.toString();
            arreglo[1][0] = 'IdPosicion';
            arreglo[1][1] = valPosicion.toString();
            arreglo[2][0] = 'IdProductoSap';
            arreglo[2][1] = valorCod.toString();
            arreglo[3][0] = 'idMaquinaria';
            arreglo[3][1] = CodMaquinarias.toString();
            arreglo[4][0] = 'session';
            arreglo[4][1] = $('#MainContent_lblIdSession').html(); 

            parametro = fc_parametrosData(arreglo);

            jQuery.ajax({
                type: "POST",
                url: location.pathname + "/AceptarCargarMaquinaria",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) { 
//                            actualizarDialogProductoCsa();
//                            jQuery("#gvMaquinaria").trigger("reloadGrid");                             
//                            CerrarDiagCargarMaquinaria(); 

                    var result = response.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            actualizarDialogProductoCsa();
                            jQuery("#gvMaquinaria").trigger("reloadGrid");                             
                            CerrarDiagCargarMaquinaria(); 
                        }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });  


            }

            function OcultarAdicional() {
                var indice = jQuery('#jqgridDetalleAdicional').jqGrid('getGridParam', 'selrow');
                if (!indice) {
                    alert('Por favor seleccione una fila');
                }
                else {
                    if (!confirm('¿Esta seguro de Eliminar este Registro?')) {
                        return;
                    }
                    jQuery("#jqgridDetalleAdicional").jqGrid('delRowData', indice);
                }
            }
            function EliminarAdicional() {

                var ArrayIdNOElminado = new Array();

                var arrayId = jQuery('#jqgridDetalleAdicional').getDataIDs();
                var cantidRegistro = arrayId.length.toString();
                for (var i = 0; i < cantidRegistro; i++) {

                    rowGrid = jQuery("#jqgridDetalleAdicional").jqGrid('getRowData', arrayId[i]);
                    var IdProductoAdicional = rowGrid.Ide
                    ArrayIdNOElminado[i] = IdProductoAdicional;
                }

                arreglo = parametro = null;

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = "IdProductoAdicional";
                arreglo[0][1] = ArrayIdNOElminado;
                arreglo[1][0] = "session";
                arreglo[1][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/EliminarAdicional",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            jQuery("#gvMaquinaria").trigger("reloadGrid");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            function GuardarDetallePrime() {

                if ($('#txtDetalleCantidad').val() == '') {
                    alert('Ingrese la Cantidad...');
                    $('#txtDetalleCantidad').focus();
                    return false;
                }

                var IdProducto = jQuery('#hdfIdProducto').val();
                var Cantidad = jQuery('#txtDetalleCantidad').val()
                var IncluyeCLC = false;

                if (jQuery('#chkbDetalleIncluyeCLC').is(':checked'))
                { IncluyeCLC = true; }
                else
                { IncluyeCLC = false; }

                var PlazoEntrega = jQuery('#txtDetallePlazoEntregaProducto').val()

                arreglo = parametro = null;

                arreglo = fc_redimencionarArray(5);
                arreglo[0][0] = "IdProducto";
                arreglo[0][1] = IdProducto;
                arreglo[1][0] = "Cantidad";
                arreglo[1][1] = Cantidad;
                arreglo[2][0] = "IncluyeCLC";
                arreglo[2][1] = IncluyeCLC;
                arreglo[3][0] = "PlazoEntrega";
                arreglo[3][1] = PlazoEntrega;
                arreglo[4][0] = "session";
                arreglo[4][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/AceptarProductoPrime",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            EliminarAdicional()
                            presentarCotizacion();
                            jQuery("#gvProducto").trigger("reloadGrid");
                            jQuery("#btnDetalleCancelar").click();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }
            //*************************************************************
            function GuardarDetalleAlquiler() {
                $('#diagDetalleProductoAlquiler').dialog('close');
            }

            function GuardarDetalleAccesorio() {
                $('#diagDetalleAccesorio').dialog('close');
            }

            function GuardarDetalleProductoSolucionCombinada() {
           
            if ($('#hdfCodigoLineaProductoSolComb').val()==''){
                alert('No hay datos que guardar. No existe Linea, verifique BD.');
                return;
            }
            var cantLlaves='0';
            var ParamConsulta='';
            var paramvalores='';

                cantLlaves=$('#hdfCantidaLllaves').val();
                               
                if(cantLlaves!=null)
                {
                    for(i=1; i<=cantLlaves; i++)
                    {
                        if($( "#ComboLLave" + i + " option:selected" ).text()=='' || $( "#ComboLLave" + i + " option:selected" ).text().toUpperCase()=='SELECCIONE')
                        {
                            alert(' Seleccione un valor para '+ $( "#spanLlave" + i ).html());
                            $( "#ComboLLave" + i).focus();
                            return;
                        }
                        else
                        {
                            if (ParamConsulta=='')
                            {
                                ParamConsulta= $( "#ComboLLave" + i + " option:selected" ).val();
                                paramvalores=  $( "#ComboLLave" + i + " option:selected" ).text();
                            }
                            else
                            {
                                ParamConsulta=ParamConsulta + '|' + $( "#ComboLLave" + i + " option:selected" ).val();
                                 paramvalores= paramvalores + '|'+ $( "#ComboLLave" + i + " option:selected" ).text();
                            }
                            
                        }
                    }
                }
                 

                var arreglo = null;
                var parametro = null;

                arreglo = fc_redimencionarArray(5);
                arreglo[0][0] = "session";
                arreglo[0][1] = $('#MainContent_lblIdSession').html();
                arreglo[1][0] = "codigoLinea";
                arreglo[1][1] = $('#hdfCodigoLineaProductoSolComb').val();
                arreglo[2][0] = "campos";
                arreglo[2][1] = ParamConsulta; 
                arreglo[3][0] = "valores";
                arreglo[3][1] = paramvalores; 
                arreglo[4][0] = "idProducto";
                arreglo[4][1] = $('#txtIdProductoSolComb').val();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/BuscarCombinacionLlave",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        var lista = response.d; 
                        if (lista.length > 0) {
                            if(lista.length==1)
                            {
                                alert('Tarifa encontrada. Registro guardado correctamente.'); 
                                jQuery("#gvProducto").trigger("reloadGrid");
                                presentarCotizacion();
                                $('#diagDetalleProductoSolucionCombinada').dialog('close');
                            }
                            else
                            {
                                alert('Error. Se encontró mas de una tarifa para esta combinación.'); 
                            }                            
                        } 
                        else{
                            alert('No se encontró ninguna tarifa correspodiente a la combinación seleccionada.');
                        }
                        
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });                
            }
        });
        // Fin de Ready
        
        // Funciones Fuera de Ready

        function eventoChange(id){

            var strText = $.trim($('#ComboLLave'+id).find("option:selected").text());
            var campo = $.trim($('#ComboLLave'+id).val());
            var cantidadCombos = $('#hdfCantidaLllaves').val();
            var sw='0';
            var valorActual = '';
            var valorAnterior = '';
            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(3);
            arreglo[0][0] = 'valor';
            arreglo[0][1] = strText;
            arreglo[1][0] = 'campo';
            arreglo[1][1] = campo;
            arreglo[2][0] = 'session';
            arreglo[2][1] =  $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);
                jQuery.ajax({
                    type: "POST",
                    url: location.pathname + "/dependenciaCombos",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var eTarifaRS = response.d;
                        if (eTarifaRS != null) {
//                            restaurarValoresSesionTarifaRS();
                            for (var i = 1; i <= cantidadCombos; i++){
                                var campoCombo = $('#ComboLLave'+i).attr("campo");
                                var valorCombo = $('#ComboLLave'+i).find("option:selected").text();                                

                                if (campoCombo ==  campo){
                                    var parametro = null;
                                    var arreglo = null;

                                    arreglo = fc_redimencionarArray(3);
                                    arreglo[0][0] = 'campo';
                                    arreglo[0][1] = campoCombo;
                                    arreglo[1][0] = 'valor';
                                    arreglo[1][1] = valorCombo;
                                    arreglo[2][0] = 'session';
                                    arreglo[2][1] =  $('#MainContent_lblIdSession').html();

                                    parametro = fc_parametrosData(arreglo);

                                    jQuery.ajax({
                                        type: "POST",
                                        url: location.pathname + "/filtrarCombos",
                                        data: parametro,
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,
                                        success: function (response) {                    
                                            eTarifaRS = response.d;                                               
                                        },
                                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                                        }
                                    });

                                    sw = '1';
                                    continue;
                                }

                                if(campoCombo !=  campo && sw == '0'){
                                    var parametro = null;
                                    var arreglo = null;

                                    arreglo = fc_redimencionarArray(3);
                                    arreglo[0][0] = 'campo';
                                    arreglo[0][1] = campoCombo;
                                    arreglo[1][0] = 'valor';
                                    arreglo[1][1] = valorCombo;
                                    arreglo[2][0] = 'session';
                                    arreglo[2][1] =  $('#MainContent_lblIdSession').html();

                                    parametro = fc_parametrosData(arreglo);

                                    jQuery.ajax({
                                        type: "POST",
                                        url: location.pathname + "/filtrarCombos",
                                        data: parametro,
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,
                                        success: function (response) {                    
                                            eTarifaRS = response.d;                                               
                                        },
                                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                                        }
                                    });
                                }

                                if(sw=='1'){
                                    var value = $('#ComboLLave'+i).find('option:selected').attr('value');
                                    $('#ComboLLave'+ i +' option').remove();
                                    $('#ComboLLave'+ i).append('<option value="'+value+'" selected="selected">seleccione</option>');
                                    eTarifaRS.sort(function(a, b) {
                                        var aID = eval('a.' + campoCombo);
                                        var bID = eval('b.' + campoCombo);
                                        return (aID == bID) ? 0 : (aID > bID) ? 1 : -1;
                                    }); 

                                    for (var j = 0; j < eTarifaRS.length; j++) {
                                        valorActual = eval('eTarifaRS['+j+'].' + campoCombo);
                                        if (valorActual != valorAnterior){
                                            var varoption = '<option value="' + value + '" >' + valorActual + '</option>';
                                            $('#ComboLLave'+ i).append(varoption);
                                            valorAnterior = valorActual;
                                        }
                                        
                                    }
                                }
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
        }

        function restaurarValoresSesionTarifaRS(){
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'session';
            arreglo[0][1] = $('#MainContent_lblIdSession').html();

            parametro = fc_parametrosData(arreglo);

            jQuery.ajax({
                type: "POST",
                url: location.pathname + "/restaurarDatosSesion",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var lista = response.d;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function consultarProductoVersion(IdCotizacionVersion) {

            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'IdCotizacionVersion';
            arreglo[0][1] = IdCotizacionVersion.toString();

            parametro = fc_parametrosData(arreglo);

            jQuery.ajax({
                type: "POST",
                url: location.pathname + "/ListarProductoVersion",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    jQuery("#jqgProductoVersion").clearGridData();
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No hay Productos');
                    } else {
                        for (var i = 0; i < lista.length; i++) {

                            var mydata = [
                                { Ide: lista[i].IdCotizacionVersionProducto,
                                    Codigo: lista[i].CodigoProducto,
                                    Nombre: lista[i].NombreProducto.toString()
                                }
                            ];
                            $("#jqgProductoVersion").jqGrid('addRowData', i + 1, mydata[0]);
                        }

                        $("#diagProductoVersion").dialog("open");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function CalcularValorNeto() {
            var cantidad = '0';
            if ($('#txtDetalleCantidad').val() != '') {
                cantidad = $('#txtDetalleCantidad').val();
            }

            var ValorUnitario = $('#txtDetalleValorUnitario').val();

            var ValorNeto = (cantidad * ValorUnitario);
            if (isNaN(ValorNeto) == false) {
                $('#txtDetalleValorFinal').val(ValorNeto);
            }
            else {
                $('#txtDetalleValorFinal').val('0');
            }

        }

        function AsignarRolesEdicion(valor)
        {
         
            switch(valor)
            {
             case 'NO':

                $('#lbnNuevo').attr('disabled', 'true');
                $('#lbnEditar').attr('disabled', 'true');
                $('#lbnEliminar').attr('disabled', 'true');

                $('#btnGenerarCotizacion').attr('disabled', 'true');
                $('#lbnGrabar').attr('disabled', 'true'); 
                $('#ibtnAceptarProductoCsa').attr('disabled', 'true');
                $('.classControlGrillaSubir').attr('disabled', 'true');            
                return;

            case 'USUARIO_EDICION':
                
                $('#lbnNuevo').removeAttr('disabled');
                $('#lbnEditar').removeAttr('disabled');
                $('#lbnEliminar').removeAttr('disabled');

                $('#btnGenerarCotizacion').removeAttr('disabled');
                $('#lbnGrabar').removeAttr('disabled');
                $('#ibtnAceptarProductoCsa').removeAttr('disabled');
                $('.classControlGrillaSubir').removeAttr('disabled');
                return;
            }
             

        }

 function AsignarRolesBloqueo(valor){
            switch(valor){

                case 'NO':
                    //$('.classControlGrillaSubir').removeAttr('disabled');
                return;

                case 'VENDEDOR':
                    $('.classControlGrillaSubir').attr('disabled', 'true'); 
                return;
            }
 }

 function ConfigurarCotizarPrevia(valor)
 {
    switch(valor){
    case 'SI':
        jQuery('#btnGenerarCotizacionPrevia').css('display','inline-block');
        jQuery('#btnGenerarCotizacionPrevia').removeAttr('display');
        return;
    case '1':
        jQuery('#btnGenerarCotizacionPrevia').css('display','block');
        return ;
    default:
    jQuery('#btnGenerarCotizacionPrevia').css('display','none');
    }
 }

 function VerificarEstadoSession(result) {

            if (result.d == '1') {
                var cantidadSession = $('#hdfNumeroSession').val();
                cantidadSession = parseInt(cantidadSession) + 1;
                $('#hdfNumeroSession').val(cantidadSession);
//                $("#EstadoSession").text("activo" + cantidadSession);                 
            }
            else {
//                $("#EstadoSession").text("expiro");
                $('#spMensaje').text('Sessión expirada, vuelva ingresar a la cotización desde SAP CRM');
                $('#diagMensaje').dialog('open'); 
            }

 }


function ActualizarCotizacionVersion(tipoArchivo){
        var NombreArchivo = $('#lklNombreArchivoActualizar').html();
        NombreArchivo= ExtraerNombreArchivo(NombreArchivo); 
         NombreArchivo= NombreArchivo +  tipoArchivo

                 var arreglo = null;
                 var parametro = null;

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = "IdCotizacionVersion";
                arreglo[0][1] = $('#hdfIdCotizacionVersion').val();
                arreglo[1][0] = "NombreArchivo";
                arreglo[1][1] = NombreArchivo;

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ActualizarCotizacionVersion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                            $('#hdfFlatArchivoActualizado').val('0');
                        } else {
                            $('#hdfFlatArchivoActualizado').val('1');
                            var nombreControl='';
                            nombreControl =$('#hdfNombreLinkArchivo').val();

                            var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreArchivo + '&Opc=COTIZACION';
                            $('#' + nombreControl ).html(NombreArchivo);                            
                            $('#' + nombreControl).attr('href', urlArchivo);
                            $('#' + nombreControl).attr('target', '_blank');

                            alert('Se Actualizó correctamente.');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
    
}

function BuscarMaquinariaCargar(){
        var valPosicion = $('#cbBuscarPosicionProducto :selected').text(); 
        var valorCod = $('#cbBuscarPosicionProducto').val();
        var nroCotizacion = $('#txtBuscarNroCotizacion').val();

            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(3);
            arreglo[0][0] = 'IdCotizacionSap';
            arreglo[0][1] = nroCotizacion.toString();
            arreglo[1][0] = 'IdPosicion';
            arreglo[1][1] = valPosicion.toString();
            arreglo[2][0] = 'IdProductoSap';
            arreglo[2][1] = valorCod.toString();

            parametro = fc_parametrosData(arreglo);

            jQuery.ajax({
                type: "POST",
                url: location.pathname + "/BuscarMaquinariaCargar",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    jQuery("#jqgBuscarListaMaquinaria").clearGridData();
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No hay maquinarias');
                    } else {
                        for (var i = 0; i < lista.length; i++) { 
                        var controlSel="<input id='chbSelCargarMaquina" + i + "' type='checkbox' checked ='checked'/>";
                            var mydata = [
                                {   sel: controlSel,
                                    IdMaquinaria:lista[i].codigo,
                                    Familia: lista[i].familia,
                                    Modelo: lista[i].modelo,
                                    ModeloBase: lista[i].modeloBase,
                                    Prefijo: lista[i].prefijo,
                                    Tipo: lista[i].IdCotizacionVersionProducto,
                                    NroSerie: lista[i].numeroSerie,
                                    Horometro: lista[i].horometroInicial,
                                    FechaHorom: lista[i].fechaHorometro,
                                    HrsPromoUso: lista[i].horasPromedioMensual,
                                    HoromFin: lista[i].horometroFinal,
                                    Renov: lista[i].renovacion,
                                    RenovVal: lista[i].renovacionValida,
                                    Departamento: lista[i].departamento
                                }
                            ];
                            $("#jqgBuscarListaMaquinaria").jqGrid('addRowData', i + 1, mydata[0]);
                        } 
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });  

}
 
 function SelOptionComboTimer(NombControlCombo, valOption, NombControlFocus, tiempo)
 {
    setTimeout(function() { 

                            jQuery("#" + NombControlCombo).val("'"+valOption+"'"); 

                            if(NombControlFocus!="")
                            {
                                jQuery("#" + NombControlFocus).focus(); 
                            }

                            var valSeleccionado=jQuery("#" + NombControlCombo).val();
//                            alert(valSeleccionado);
                            if (valSeleccionado== null || valSeleccionado=='')
                             {
                                SelOptionComboTimer(NombControlCombo, valOption, NombControlFocus, tiempo);
                              }
                             
                        }, tiempo);

    
 }

 function GenerarCotizacionPrevia(){
 
                var arreglo = parametro = null;
                arreglo = fc_redimencionarArray(1); 
                arreglo[0][0] = "session";
                arreglo[0][1] = $('#MainContent_lblIdSession').html();

                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/DescargarCotizacionPrevia",
                    dataType: "json",
                    type: "post",
                    cache: false,
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    data: parametro,
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else {
                            if (result.mensaje != "") { 
                                var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + result.mensaje + '&Opc=COTIZACION';
                                 
                                $('#lklDescargarArchivo').attr('href', urlArchivo);
                                $('#lklDescargarArchivo').attr('target', '_blank');
                                $("#diagDescargarArchivo").dialog("open");
                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                }); 
 }
function getAbsolutePath() {
    var urlResult='';
    var loc = window.location; 
   
  var locpathName = loc.pathname.toString();
  var locsearch = loc.search.toString();
  var lochash = loc.hash.toString();
  var lochref = loc.href.toString()
   
   var valpathName = locpathName.substring(0, locpathName.lastIndexOf('/') + 1);
//     var valTrim=locpathName + locsearch + lochash;
   var valTrim='/PAGINAS/publico/frmCotizacionRegistro.aspx' + locsearch + lochash; 
   urlResult= lochref.substring(0, lochref.length - (valTrim.length));
 
    return urlResult;
}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frmCotizacionRegistro" action="#">
    <table width="100%">
        <tr>
            <td>
                <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td class="TextoTitulo">
                            Registro de Cotización
                        </td>
                        <td align="right" style="padding-right: 40px;">
                            <div id="submenu">
                                <p>
                                    <%--<label id="EstadoSession">| </label> --%>
                                    <input id="hdfNumeroSession" type="hidden" value="0" />
                                    <a id="lbnGrabar">Grabar | </a><a id="lblCerrarVentana">Cerrar</a>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tabs-1">General </a></li>
                                    <li><a href="#tabs-2">Producto </a></li>
                                    <li><a href="#tabs-3">Ofertas de Valor </a></li>
                                </ul>
                                <div id="tabs-1">
                                    <input id="hdfvalRolEdicion" type="hidden" />
                                    <input id="hdfvalRolBloqueo" type="hidden" />
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <%-- <fieldset>
                                                    <legend style="text-align: left" class="TDTexto">Cotización</legend>--%>
                                                <div class="divHeadTitle100">
                                                    Cotización
                                                </div>
                                                <div class="divContenido100">
                                                    <table width="100%" cellpadding="5">
                                                        <tr>
                                                            <td align="left" class="Texto" style="width: 18%">
                                                                ID Cotización :
                                                            </td>
                                                            <td class="TDTexto" align="left" style="width: 32%">
                                                                <div id="lbnIdCotizacionSap">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto" style="width: 18%">
                                                                Estado :
                                                            </td>
                                                            <td class="TDTexto" align="left" style="width: 32%">
                                                                <div id="lbnEstado">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="Texto">
                                                                Solicitante :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnSolicitante">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto">
                                                                Persona Responsable :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnPersonaResponsable">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="Texto">
                                                                Fecha Inic. Validez :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnFechaInicioValidez">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto">
                                                                Fecha Fin Validez :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnFechaFinValidez">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="Texto">
                                                                Valor Neto :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnValorNeto">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto">
                                                                Fecha de Precio :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnFechaPrecio">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="Texto">
                                                                Importe Impuesto :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnImporteImpuesto">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto">
                                                                Tipo de cambio :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnTipoCambio">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="Texto">
                                                                Valor bruto :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnValorBruto">
                                                                </div>
                                                            </td>
                                                            <td align="left" class="Texto">
                                                                Fecha est. Facturación :
                                                            </td>
                                                            <td class="TDTexto" align="left">
                                                                <div id="lbnFechaEstFacturacion">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%--</fieldset>--%>
                                                <%--<fieldset>
                                                    <legend style="text-align: left" class="TDTexto">Contacto</legend>--%>
                                                <div class="divHeadTitle100">
                                                    Contacto
                                                </div>
                                                <div class="divContenido100">
                                                    <table id="gvContacto">
                                                    </table>
                                                </div>
                                                <%--</fieldset>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="tabs-2">
                                    <table id="gvProducto">
                                    </table>
                                    <div id="pgProducto">
                                    </div>
                                </div>
                                <div id="tabs-3">
                                    <div style="padding-bottom: 5px;">
                                        <a id="btnGenerarCotizacion" title="Cotización Detallada" style="width: 200px; height: 28px">
                                            Cotización Detallada</a> <a id="btnGenerarCotizacionPrevia" title="Cotización Resumen"
                                                style="width: 220px; height: 28px">Cotización Resumen</a>
                                    </div>
                                    <table id="gvOfertaValor">
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="diagDetalleProductoPrime" title="Detalle de Producto Prime">
        <div class="divHeadTitle100">
            Detalle de Producto
        </div>
        <div class="divContenido100">
            <table style="width: 100%;">
                <tr class="FormatoTexto">
                    <td>
                        ID Producto:
                    </td>
                    <td>
                        <input type="hidden" id="hdfIdProducto" />
                        <input type="text" id="txtDetalleIDProducto" disabled="disabled" />
                    </td>
                    <td>
                        Producto:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleProducto" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleCantidad" disabled="disabled" />
                    </td>
                    <td>
                        Estado:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleEstado" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Fecha Estim.de Cierre:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleFechaEstimadoCierre" disabled="disabled" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input id="chkbDetalleIncluyeCLC" type="checkbox" value="IncluyeCLC" disabled="disabled" />Incluye
                        CLC
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Plazo de entrega del Producto:
                    </td>
                    <td colspan="3">
                        <input type="text" id="txtDetallePlazoEntregaProducto" style="width: 93%" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Valor Unitario:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleValorUnitario" disabled="disabled" />
                    </td>
                    <td>
                        Valor Final:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleValorFinal" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td colspan="4" style="font-weight: bold;">
                        Detalle de Precio
                        <hr />
                        <%-- <div style="width:20%;float:left;">Detalle de Precio</div>
                        <div style="width:80%;float:left; vertical-align :bottom "><hr style="vertical-align:bottom;"/></div>--%>
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Valor Lista:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleValorLista" disabled="disabled" />
                    </td>
                    <td>
                        Valor Real:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleValorReal" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Descuento %:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleDescuentoPorcent" disabled="disabled" />
                    </td>
                    <td>
                        Descuento Importe:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleDescuentoImporte" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Flete:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleFlete" disabled="disabled" />
                    </td>
                    <td>
                        Valor Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleValorVentaTotal" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Impuesto %:
                    </td>
                    <td>
                        <input type="text" id="txtDetalleImpuestoPorcentaje" disabled="disabled" />
                    </td>
                    <td>
                        Precio Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtDetallePrecioVentaTotal" disabled="disabled" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHeadTitle100">
            Beneficios Adicionales
        </div>
        <div class="divContenido100">
            <table id="jqgridDetalleAdicional">
            </table>
        </div>
        <div class="divHeadTitle100">
            Accesorios
        </div>
        <div class="divContenido100">
            <table id="jqgridDetalleAccesorio">
            </table>
        </div>
        <div id="divDetallBotonesPrime" style="text-align: center; padding-top: 5px;">
            <input type="button" id="btnDetalleAceptar" value="Aceptar" />
            <input type="button" id="btnDetalleCancelar" value="Cancelar" />
        </div>
    </div>
    <div id="diagDetalleProductoAlquiler" title="Detalle de Producto de Alquiler">
        <div class="divHeadTitle100">
            Detalle de Producto
        </div>
        <div class="divContenido100">
            <table style="width: 100%;">
                <tr class="FormatoTexto">
                    <td>
                        ID Producto:
                    </td>
                    <td>
                        <input type="hidden" id="hdfIdProductoAlquiler" />
                        <input type="text" id="txtIdDetalleProductoAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Producto:
                    </td>
                    <td>
                        <input type="text" id="txtNombreProductoAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Estado:
                    </td>
                    <td>
                        <input type="text" id="txtEstadoProductoAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Valor Unitario:
                    </td>
                    <td>
                        <input type="text" id="txtValorUnitarioProductoAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <input type="text" id="txtCantidadProductoAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Valor Neto:
                    </td>
                    <td>
                        <input type="text" id="txtValorNetoProductoAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Tipo de Alquiler:
                    </td>
                    <td>
                        <input type="text" id="txtTipoAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Tipo de Pago:
                    </td>
                    <td>
                        <input type="text" id="txtTipoPago" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Tipo de Facturación:
                    </td>
                    <td>
                        <input type="text" id="txtTipoFacturacion" disabled="disabled" />
                    </td>
                    <td>
                        Mes de Alquiler:
                    </td>
                    <td>
                        <input type="text" id="txtMesAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Valor Lista:
                    </td>
                    <td>
                        <input type="text" id="txtValorListaAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Valor Real:
                    </td>
                    <td>
                        <input type="text" id="txtValorRealAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Descuento %:
                    </td>
                    <td>
                        <input type="text" id="txtDescuentoPorcentAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Descuento Importe:
                    </td>
                    <td>
                        <input type="text" id="txtDescuentoImporteAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Flete:
                    </td>
                    <td>
                        <input type="text" id="txtFleteAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Valor Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtValorVentaTotalAlquiler" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Impuesto %:
                    </td>
                    <td>
                        <input type="text" id="txtImpuestoPorcentajeAlquiler" disabled="disabled" />
                    </td>
                    <td>
                        Precio Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtPrecioVentaTotalAlquiler" disabled="disabled" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="text-align: center; padding-top: 5px;">
            <input type="button" id="btnAceptarProductoAlquiler" value="Aceptar" />
            <input type="button" id="btnCancelarProductoAlquiler" value="Cancelar" />
        </div>
    </div>
    <div id="diagDetalleAccesorio" title="Detalle de Accesorio">
        <div class="divHeadTitle100">
            Detalle de Producto
        </div>
        <div class="divContenido100">
            <table style="width: 100%;">
                <tr class="FormatoTexto">
                    <td>
                        ID Producto:
                    </td>
                    <td>
                        <input type="hidden" id="hdfIdProductoAccesorio" />
                        <input type="text" id="txtIdProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Producto:
                    </td>
                    <td>
                        <input type="text" id="txtNombreProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Estado:
                    </td>
                    <td>
                        <input type="text" id="txtEstadoProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Valor Unitario:
                    </td>
                    <td>
                        <input type="text" id="txtValorUnitarioProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <input type="text" id="txtCantidadProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Valor Neto:
                    </td>
                    <td>
                        <input type="text" id="txtValorNetoProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Valor Lista:
                    </td>
                    <td>
                        <input type="text" id="txtValorListaProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Valor Real:
                    </td>
                    <td>
                        <input type="text" id="txtValorRealProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Descuento %:
                    </td>
                    <td>
                        <input type="text" id="txtDescuentoPorcentProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Descuento Importe:
                    </td>
                    <td>
                        <input type="text" id="txtDescuentoImporteProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Flete:
                    </td>
                    <td>
                        <input type="text" id="txtFleteProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Valor Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtValorVentaProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Impuesto %:
                    </td>
                    <td>
                        <input type="text" id="txtImpuestoPorcentProductoAccesorio" disabled="disabled" />
                    </td>
                    <td>
                        Precio Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtPrecioVentaProductoAccesorio" disabled="disabled" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div4" style="text-align: center; padding-top: 5px;">
            <input type="button" id="btnAceptarProductoAccesorio" value="Aceptar" />
            <input type="button" id="btnCancelarProductoAccesorio" value="Cancelar" />
        </div>
    </div>
    <div id="diagDetalleProductoSolucionCombinada" title="Detalle de Producto">
        <div class="divHeadTitle100">
            Detalle de Producto
        </div>
        <div class="divContenido100">
            <table style="width: 100%;">
                <tr class="FormatoTexto">
                    <td>
                        ID Producto:
                    </td>
                    <td>
                        <input type="hidden" id="Hidden1" />
                        <input type="text" id="txtIdProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Producto:
                    </td>
                    <td>
                        <input type="text" id="txtNombreProductoSolComb" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Estado:
                    </td>
                    <td>
                        <input type="text" id="txtEstadoProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Valor Neto:
                    </td>
                    <td>
                        <input type="text" id="txtValorNetoProductoSolComb" disabled="disabled" />
                    </td>
                </tr>
                <%--<tr class="FormatoTexto">
                    <td>
                        Duración de Plan:
                    </td>
                    <td>
                        <input type="text" id="txtDuracionPlanProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Plazo de Renovación:
                    </td>
                    <td>
                         <input type="text" id="txtPlazoRenovacionProductoSolComb" disabled="disabled" />
                    </td>
                </tr>                 --%>
                <%--<tr class="FormatoTexto">
                    <td>
                        Fecha Inicio Contrato:
                    </td>
                    <td>
                        <input type="text" id="txtFechaInicioContratoProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Fecha Est.de cierre:
                    </td>
                    <td>
                        <input type="text" id="txtFechaEstimadoCierreProductoSolComb" disabled="disabled" />
                    </td>
                </tr>--%>
                <%--<tr class="FormatoTexto">
                    <td>
                        % Particip.vendedor 1:
                    </td>
                    <td>
                        <input type="text" id="txtPorcentajeVendedor1ProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        % Particip.vendedor 2:
                    </td>
                    <td>
                        <input type="text" id="txtPorcentajeVendedor2ProductoSolComb" disabled="disabled" />
                    </td>
                </tr>--%>
                <tr class="FormatoTexto">
                    <td>
                        Valor Lista:
                    </td>
                    <td>
                        <input type="text" id="txtValorListaProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Valor Real:
                    </td>
                    <td>
                        <input type="text" id="txtValorRealProductoSolComb" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Descuento %:
                    </td>
                    <td>
                        <input type="text" id="txtPorcentajeDescuentoProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Descuento Importe:
                    </td>
                    <td>
                        <input type="text" id="txtImporteDescuentoProductoSolComb" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Flete:
                    </td>
                    <td>
                        <input type="text" id="txtFleteProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Valor Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtValorVentaTotalProductoSolComb" disabled="disabled" />
                    </td>
                </tr>
                <tr class="FormatoTexto">
                    <td>
                        Impuesto %:
                    </td>
                    <td>
                        <input type="text" id="txtPorcentajeImpuestoProductoSolComb" disabled="disabled" />
                    </td>
                    <td>
                        Precio Venta Total:
                    </td>
                    <td>
                        <input type="text" id="txtPrecioVentaTotalProductoSolComb" disabled="disabled" />
                        <input type="hidden" id="hdfCodigoLineaProductoSolComb" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHeadTitle100">
            Configuración de Producto
        </div>
        <div class="divContenido100">
            <div id="divConfiguracionLlavesProducto">
            </div>
        </div>
        <div id="div6" style="text-align: center; padding-top: 5px;">
            <input type="button" id="btnAceptarProductoSolComb" value="Aceptar" />
            <input type="button" id="btnCancelarProductoSolComb" value="Cancelar" />
        </div>
    </div>
    <div id="idDialogProductoCsa" title="Producto CSA" style="display: none">
        <table width="100%">
            <tr>
                <td align="center" style="width: 100%">
                    <fieldset style="width: 860px; text-align: left; padding-top: 1px; margin-top: 1px;">
                        <legend class="TDTexto">Detalle de Producto</legend>
                        <table width="100%" cellpadding="2">
                            <tr>
                                <td align="left" class="Texto" style="width: 200px">
                                    ID Producto
                                </td>
                                <td class="TDTexto" style="width: 230px; text-align: left">
                                    <div id="ilbnIdProducto">
                                    </div>
                                </td>
                                <td align="left" class="Texto" style="width: 200px">
                                    Producto
                                </td>
                                <td class="TDTexto" style="width: 230px; text-align: left">
                                    <div id="ilbnProducto">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Texto">
                                    Estado
                                </td>
                                <td class="TDTexto" style="text-align: left">
                                    <div id="ilbnEstado">
                                    </div>
                                </td>
                                <td align="left" class="Texto">
                                    Valor Neto
                                </td>
                                <td class="TDTexto" style="text-align: left">
                                    <div id="ilbnValorNeto">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Texto">
                                    Duración del Plan
                                </td>
                                <td class="TDTexto" style="text-align: left">
                                    <div id="ilbnDuracion">
                                    </div>
                                </td>
                                <td align="left" class="Texto">
                                    Plazo de Renovación
                                </td>
                                <td class="TDTexto" style="text-align: left">
                                    <div id="ilbnPlazoRenovacion">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Texto">
                                    Fluidos
                                </td>
                                <td align="left">
                                    <input id="irdnFluidosSi" name="rdnFluidos" type="radio" disabled="disabled" /><label
                                        for="irdnFluidosSi">Si</label>
                                    <input id="irdnFluidosNo" name="rdnFluidos" type="radio" disabled="disabled" /><label
                                        for="irdnFluidosNo">No</label>
                                </td>
                                <td align="left" class="Texto" style="visibility: hidden">
                                    Detalle de Partes
                                </td>
                                <td align="left" style="visibility: hidden">
                                    <input id="irdnDetalleSi" name="rdnDetalle" type="radio" disabled="disabled" /><label
                                        for="irdnDetalleSi">Si</label>
                                    <input id="irdnDetalleNo" name="rdnDetalle" type="radio" disabled="disabled" /><label
                                        for="irdnDetalleNo">No</label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Texto">
                                    Fecha Inicio Contrato
                                </td>
                                <td align="left">
                                    <input id="itxtFecInicioContrato" type="text" maxlength="10" style="width: 100px"
                                        disabled="disabled" />
                                </td>
                                <td align="left" class="Texto">
                                    Fecha Est. De Cierre
                                </td>
                                <td align="left">
                                    <input id="itxtFecEstCierre" type="text" maxlength="10" style="width: 100px" disabled="disabled" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Texto">
                                    % Particip. Vendedor 1
                                </td>
                                <td align="left">
                                    <input id="itxtPartVenededor1" type="text" maxlength="3" style="width: 40px" disabled="disabled" />
                                    <label class="TDTexto">
                                        %</label>
                                </td>
                                <td align="left" class="Texto">
                                    % Particip. Vendedor 2
                                </td>
                                <td align="left">
                                    <input id="itxtPartVenededor2" type="text" maxlength="3" style="width: 40px" disabled="disabled" />
                                    <label class="TDTexto">
                                        %</label>
                                </td>
                            </tr>
                            <tr class="FormatoTexto">
                                <td>
                                    Valor Lista:
                                </td>
                                <td>
                                    <input type="text" id="itxtValorLista" disabled="disabled" />
                                </td>
                                <td>
                                    Valor Real:
                                </td>
                                <td>
                                    <input type="text" id="itxtValorReal" disabled="disabled" />
                                </td>
                            </tr>
                            <tr class="FormatoTexto">
                                <td>
                                    Descuento %:
                                </td>
                                <td>
                                    <input type="text" id="itxtDescuentoPorcent" disabled="disabled" />
                                </td>
                                <td>
                                    Descuento Importe:
                                </td>
                                <td>
                                    <input type="text" id="itxtDescuentoImporte" disabled="disabled" />
                                </td>
                            </tr>
                            <tr class="FormatoTexto">
                                <td>
                                    Flete:
                                </td>
                                <td>
                                    <input type="text" id="itxtFlete" disabled="disabled" />
                                </td>
                                <td>
                                    Valor Venta Total:
                                </td>
                                <td>
                                    <input type="text" id="itxtValorVentaTotal" disabled="disabled" />
                                </td>
                            </tr>
                            <tr class="FormatoTexto">
                                <td>
                                    Impuesto %:
                                </td>
                                <td>
                                    <input type="text" id="itxtImpuestoPorcentaje" disabled="disabled" />
                                </td>
                                <td>
                                    Precio Venta Total:
                                </td>
                                <td>
                                    <input type="text" id="itxtPrecioVentaTotal" disabled="disabled" />
                                </td>
                            </tr>
                        </table>
                        <input id="hdfClaseCSA" type="hidden" />
                    </fieldset>
                    <fieldset style="width: 860px; text-align: left; padding-top: 1px; margin-top: 1px;">
                        <legend class="TDTexto">Lista de Máquinas</legend>
                        <table id="gvMaquinaria">
                        </table>
                        <div id="pgMaquinaria">
                        </div>
                    </fieldset>
                    <div id="divDialogBuscarMaquinaria" title="Cargar Maquinarias" style="display: none">
                        <div class="divHeadTitle100">
                            Busqueda
                        </div>
                        <div class="divContenido100">
                            <div class="FormatoTexto">
                                Numero de Cotización :
                                <input id="txtBuscarNroCotizacion" type="text" />
                                <input id="btnBuscarCotizacion" type="button" value="Buscar" />
                                <input id="hdfIdCotizacionBusqueda" type="hidden" />
                            </div>
                            <div class="FormatoTexto">
                                Posición de producto :
                                <select id="cbBuscarPosicionProducto" style="width: 100px">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <table id="jqgBuscarListaMaquinaria">
                        </table>
                        <div id="div3" style="text-align: center; padding-top: 5px;">
                            <input type="button" id="btnBuscarAceptarMaquina" value="Aceptar" />
                            <input type="button" id="btnBuscarCancelarMaquina" value="Cancelar" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="div1" style="text-align: center;">
            <a id="ibtnAceptarProductoCsa" title="Aceptar" style="width: 90px; height: 28px">Aceptar</a>
            <a id="ibtnCancelarProductoCsa" title="Cancelar" style="width: 90px; height: 28px">Cancelar</a>
        </div>
        <div id="idDialogMaquinaria" title="Maquinaria" style="display: none">
            <input id="ihddTipoOperacion" type="hidden" />
            <input id="ihdnIdMaquinaria" type="hidden" />
            <table width="500px">
                <tr>
                    <td align="left" class="Texto" style="width: 200px">
                        Maquina Nueva :
                    </td>
                    <td align="left" style="width: 300px">
                        <input id="irdnMaquinaNuevaSi" name="iRadio" type="radio" /><label for="irdnMaquinaNuevaSi">Si</label>
                        <input id="irdnMaquinaNuevaNo" name="iRadio" type="radio" checked="checked" /><label
                            for="irdnMaquinaNuevaNo">No</label>
                    </td>
                </tr>
                <tr>
                    <td class="Texto" style="vertical-align: top">
                        Familia :
                    </td>
                    <td align="left">
                        <select id="iddlFamilia" class="ArialFrm" style="width: 200px">
                        </select>
                        <br />
                        <input id="itxtFamilia" type="text" class="Texto" maxlength="100" style="width: 195px;
                            display: none" />
                        <input id="ihddFamilia" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto" style="vertical-align: top">
                        Modelo Base :
                    </td>
                    <td align="left">
                        <select id="iddlModeloBase" class="ArialFrm" style="width: 200px">
                        </select>
                        <br />
                        <input id="itxtModeloBase" type="text" class="Texto" maxlength="100" style="width: 195px;
                            display: none" />
                        <input id="ihddModeloBase" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto" style="vertical-align: top">
                        Prefijo :
                    </td>
                    <td align="left">
                        <select id="iddlPrefijo" class="ArialFrm" style="width: 200px">
                            <option></option>
                        </select>
                        <br />
                        <input id="itxtPrefijo" type="text" class="Texto" maxlength="100" style="width: 195px;
                            display: none" />
                        <input id="ihddPrefijo" type="hidden" />
                    </td>
                </tr>
                <tr id="trNroMaquinas">
                    <td class="Texto">
                        <span id="labelNroMaquinas">Nro Maquinas :</span>
                    </td>
                    <td align="left">
                        <input id="itxtNroMaquinas" type="text" class="Texto" maxlength="5" style="width: 100px" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto" style="vertical-align: top">
                        Nro de Serie :
                    </td>
                    <td align="left">
                        <select id="iddlNroSerie" class="ArialFrm" style="width: 200px">
                        </select>
                        <br />
                        <input id="itxtNroSerie" type="text" class="Texto" maxlength="100" style="width: 195px;
                            display: none" />
                        <input id="ihddNroSerie" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Horometro :
                    </td>
                    <td align="left">
                        <input id="itxtHorometro" type="text" class="Texto" maxlength="5" style="width: 100px" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Fecha de Horometro :
                    </td>
                    <td align="left">
                        <input id="itxtFechaHorometro" type="text" class="Texto" maxlength="10" style="width: 100px" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Horas Prom. Uso Mensual :
                    </td>
                    <td align="left">
                        <input id="itxtHorasPromMensual" type="text" class="Texto" maxlength="5" style="width: 100px" />
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Departamento :
                    </td>
                    <td align="left">
                        <select id="iddlDepartamento" class="ArialFrm" style="width: 200px">
                        </select>
                        <input id="ihddDepartamento" type="hidden" />
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr style="text-align: right; vertical-align: middle">
                    <td align="right">
                        <a id="ibtnMaquinariaAceptar" title="Aceptar" style="width: 90px; height: 28px">Aceptar</a>
                    </td>
                    <td align="left" style="width: 90px">
                        <a id="ibtnMaquinariaCancelar" title="Cancelar" style="width: 90px; height: 28px">Cancelar</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="idDialogLista" title="Lista" style="display: none">
        <table width="100%">
            <tr>
                <td>
                    <table id="gvVerLista">
                    </table>
                    <input id="ichkGuardarLista" type="checkbox" checked="checked" /><label class="TDTexto"
                        for="ichkGuardarLista">Guardar los cambios</label>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr style="text-align: right; vertical-align: middle">
                <td align="right">
                    <a id="ibtnDescargarDocumento" title="Descargar" style="width: 96px; height: 28px;">
                        Generar</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="diagProductoVersion" title="Productos">
        <table id="jqgProductoVersion">
        </table>
    </div>
    <div id="diagDescargarArchivo" title="Descargar Cotizacion">
        <a id="lklDescargarArchivo" style="color: #0000FF">Descargar Cotización</a>
    </div>
    <div id="contActualizarArchivo">
        <div id="diagActualizarArchivo" title="Actualizar Archivo">
            <div style="height: 20px; padding-top: 20px;">
                <div style="float: left; padding-right: 10px; font-weight: bold">
                    Archivo :</div>
                <div>
                    <a id="lklNombreArchivoActualizar" style="color: #0000FF"></a>
                </div>
                <div>
                    <br />
                    <input type="hidden" id="hdfNombreLinkArchivo" />
                    <input type="hidden" id="hdfIdCotizacionVersion" />
                    <input type="hidden" id="hdfFlatArchivoActualizado" value="0" />
                </div>
            </div>
            <div>
                <div>
                    <iframe id="myFrame" src="frmCargaNE.aspx" width="310" height="85" frameborder="0"></iframe>
                </div>
                <%--<div style="float: left; padding-right: 10px;">
                    <input id="FileActualizarArchivo" type="file" />
                </div>--%>
                <%--<div><input type="button" id="btnCancelarArchivoActualizar" value ="Cancelar"/></div>--%>
            </div>
        </div>
    </div>
    <div id="diagMensaje">
        <p>
            <span id="spMensaje"></span>
        </p>
    </div>
    <asp:Label ID="lblIdSession" runat="server" ForeColor="White"></asp:Label>
    </form>
</asp:Content>
