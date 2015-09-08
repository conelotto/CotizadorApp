<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmCargaArchivoRS.aspx.vb" Inherits="Cotizador.frmAdmCargaArchivoRS" %>
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
        var frameSW = false;

        $(document).ready(function () {

            document.getElementsByTagName('iframe')[0].onload = function () {

                EliminarArchivoTemporal();

                if (/(MSIE\ [0-9]{1})/i.test(navigator.userAgent)) {
                    window.frameSW = true;
                    $('#hdfCargarArchivo').val('NO');
                }
                if (window.frameSW) {
                    var childiFrame = document.getElementById("myFrame");
                    var innerDoc = childiFrame.contentDocument || ChildiFrame.contentWindow.document;
                    var yourChildiFrameControl = innerDoc.getElementById("hdnASPArchivo");
                    var value = yourChildiFrameControl.value;
                    $('#hdfCargarArchivo').val('SI');

                    $('#hdnNombreArchivo').val(value);
                }
                else {
                    window.frameSW = true;
                    $('#hdfCargarArchivo').val('NO');
                }
            }

            $("#diagDescargarArchivo").dialog({
                autoOpen: false,
                height: 100,
                width: 200,
                resizable: false,
                closeOnEscape: false,
                modal: true
            });

            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            $('#lklDescargarTarifas').click(function (evento) {
                if ($('#cbLineas').val() == '0') {
                    alert("Elija una linea para descargar el último archivo actualizado de Tarifas.");
                    return;
                } else {
                    var urlArchivoTarifas = '../frmDescargarFTP.aspx?NombreArchivo=tarifas_' + $('#cbLineas :selected').text() + '.xlsx' + '&Opc=TARIFASRS';
                    $('#lklDescargarTarifas').attr('href', urlArchivoTarifas);
                    $('#lklDescargarTarifas').attr('target', '_blank');
                    $("#diagDescargarArchivo").dialog("open");
                }
            });

            $('#lklDescargarPartes').click(function (evento) {
                if ($('#cbLineas').val() == '0') {
                    alert("Elija una linea para descargar el último archivo actualizado de Detalle de Partes.");
                    return;
                } else {
                    var urlArchivoPartes = '../frmDescargarFTP.aspx?NombreArchivo=partes_' + $('#cbLineas :selected').text() + '.xlsx' + '&Opc=PARTESRS';
                    $('#lklDescargarPartes').attr('href', urlArchivoPartes);
                    $('#lklDescargarPartes').attr('target', '_blank');
                    $("#diagDescargarArchivo").dialog("open");
                }
            });

            IndicarRoles();

            $("#fileArchivo").uploadify({
                'swf': '../../Styles/uploadify.swf',
                'uploader': '../../Comunes/UploadVB.ashx',
                'cancelImg': '../../Styles/uploadify-cancel.png',
                'displayData': 'percentage',
                'fileTypeExts': '*.xlsx; *.xls', //'*.*',
                'fileTypeDesc': 'Todos los Archivos',
                'buttonText': 'Examinar',
                'auto': true,
                'multi': false,
                'wmode': 'transparent',
                'removeTimeout': 0.5,
                'hideButton': true,
                'method': 'get',
                'height': 15,
                'width': 80,
                'onSelect': function (file) {
                    if ($('#hdfAccionArchivoConfiguracion').val() == 'N') {
                        EliminarArchivoTemporal();
                        GenerarNombreArchivo(file.name);
                    }
                    var auth = '';
                    var ASPSESSID = "<%=Session.SessionID%>";
                    var TIPOACCION = 'SUBIR_ARCHIVO';
                    var NombreArchivoActualizar = '';

                    NombreArchivoActualizar = ExtraerNombreArchivo(file.name); // ExtraerNombreArchivo($('#lklNombreArchivo').html());
                    NombreArchivoActualizar = NombreArchivoActualizar + file.type;
                    //settings
                    $("#fileArchivo").uploadify('settings', 'formData', { 'Archivo': NombreArchivoActualizar, 'ASPSESSID': ASPSESSID, 'AUTHID': auth, 'TIPOACCION': TIPOACCION, 'DESTINO': 'TEMPORAL' });
                },
                'onUploadSuccess': function (file, data, response) {
                    $('#hdfCargarArchivo').val('SI');
                    $('#txtDireccion').val(file.name);
                    //alert('Archivo preparado.');
                },
                'onCancel': function (file) {
                    $('#hdfCargarArchivo').val('NO');
                }
            });
        });

        $(window).unload(function () {
            EliminarArchivoTemporal();
        });

        function cargarArchivo() {

            if ($('#hdnNombreArchivo').val() == '') {
                alert('No se ha preparado ningun archivo.');
                return false;
            }

            var TipoCarga = $('input:radio[name=tipoCarga]:checked').val();
            var TipoRS = $('input:radio[name=tipoRS]:checked').val();
            var arreglo = null;
            var parametros = null;
            var idLinea = $('#cbLineas').val();
            var Linea = $('#cbLineas :selected').text();

            arreglo = fc_redimencionarArray(5);
            arreglo[0][0] = 'TipoCarga';
            arreglo[0][1] = 'anadir';
            arreglo[1][0] = 'TipoRS';
            arreglo[1][1] = TipoRS;
            arreglo[2][0] = 'Archivo';
            arreglo[2][1] = $('#hdnNombreArchivo').val();
            arreglo[3][0] = 'idLinea';
            arreglo[3][1] = idLinea;
            arreglo[4][0] = 'Linea';
            arreglo[4][1] = Linea;

            if (idLinea == '0') {
                alert("Elija una linea para continuar con la carga de archivos.");
                return;
            }
            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/CargarArchivo',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var rpta = msg.d;
                    switch (rpta) {
                        case "1":
                            $('#hdfCargarArchivo').val('NO');
                            alert('Archivo cargado correctamente.');
                            window.frameSW = false;                            
                            reload_element('myFrame');   
                            break;
                        default:
                            alert(rpta);
                    }
                    $('#hdnNombreArchivo').val('');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function reload_element(id) {

            if (navigator.appName == "Microsoft Internet Explorer") {
                //                                window.document.getElementById('myFrame').contentWindow.location.reload(true);
                location.reload();
            } else {
                window.document.getElementById(id).src = window.document.getElementById(id).src;
            }

//            if (window.document.getElementById(id).location) {
//                window.document.getElementById(id).location.reload(true);
//            } else if (window.document.getElementById(id).contentWindow.location) {
//                window.document.getElementById(id).contentWindow.location.reload(true);
//            } else if (window.document.getElementById(id).src) {
//                window.document.getElementById(id).src = window.document.getElementById(id).src;
//            } else {
//                location.reload();
//            }
        }

        function cerrar() {

            if (!(confirm('¿Esta seguro de Salir?'))) {
                return;
            }
            window.open('', '_parent', '');
            window.close();
        }

        function GenerarNombreArchivo(NombreArchivo) {
            var NombreGenerado = '';
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'NombreArchivo';
            arreglo[0][1] = NombreArchivo.toString();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GenerarNombreArchivo',
                data: parametro,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    NombreGenerado = msg.d;
                    $('#lklNombreArchivo').html(NombreGenerado);
                    var urlArchivo = '';

                    if ($('#hdfAccionArchivoConfiguracion').val() == 'N') {
                        urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreGenerado + '&Opc=TEMPORAL';
                    }
                    else {
                        urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreGenerado + '&Opc=PLANTILLAS';
                    }
                    $('#lklNombreArchivo').attr('href', urlArchivo);
                    $('#lklNombreArchivo').attr('target', '_blank');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function EliminarArchivoTemporal() {

            var NombreArchivo = $('#hdnNombreArchivo').val();
            var parametro = '';
            var arreglo = '';
            var Resultado = '-1'

            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'NombreArchivo';
            arreglo[0][1] = NombreArchivo;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/EliminarArchivoTemporal',
                data: parametro,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    Resultado = msg.d;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        function IndicarRoles() {

            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'usuario';
            arreglo[0][1] = $('#hdfLogin').val();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/IndicarRoles",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    $('#lblMsjResponsable').text(result.mensaje);
                    if (result.validacion == true) {
                        llenarComboLineas(result.usuario);
                        $("#btnAceptar").removeAttr("disabled");
                    }
                    else {
                        $("#cbLineas").hide();                                                
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function llenarComboLineas(usuario) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'usuario';
            arreglo[0][1] = usuario;

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
                    $('#cbLineas').append('<option value="0" selected="selected">Seleccione</option>')
                    var lista = response.d;
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbLineas').append(varoption);
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
    <div id="diagMantenimiento" title="Administrador de Carga de Archivos para Repuestos y Servicios">
        <div class="divHeadTitle100" style="width: 40%">
            Seleccione Opciones:<asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
        </div>        
        <div class="divContenido100" style="width: 40%">
           
            <table style="width: 100%; text-align: left">
                <tr >
                    <td colspan="4">
                        <br />
                        <label id="lblMsjResponsable"></label>
                        <br />                        
                        <select id="cbLineas">
                            <option></option>
                        </select>
                    </td>                    
                </tr>
                <tr >
                    <td>
                        <br />
                    </td>                    
                </tr>
                <tr>
                    <td style="width: 80px;">
                    </td>
                    <td>
                        <input type="radio" name="tipoRS" value="tarifas" checked="checked" />
                        tarifas
                        <input type="radio" name="tipoRS" value="partes" />
                        partes
                    </td>
                    <td>
                        <input type="hidden" id="hdfAccionArchivoConfiguracion" />
                        <input type="hidden" id="hdfCargarArchivo" />
                        <input id="hdnNombreArchivo" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 80px;">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Archivo :
                    </td>
                    <td valign="middle">
                        <div>
                            <iframe id="myFrame" src="frmAdmCargaNE.aspx" width="400" height="85" frameborder="0"></iframe>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 80px;">
                    </td>
                </tr>
            </table>
            
            <div id="divBotonesMantenimiento" style =" text-align:center; padding-top:5px; border-bottom-color:Navy; border-width:5px;">
                <input id="btnAceptar" type="button" value="Aceptar" disabled="disabled" onclick="cargarArchivo(); return false;" />
                <input id="btnCerrar" type="button" value="Cancelar" onclick="cerrar(); return false;" />
            </div>
            
            <div id="Div1" title="Descargar Tarifas">
                <table>
                    <tr>
                        <td style="width: 25%;">
                        </td>
                        <td style="width: 25%;">
                        </td>
                        <td>
                            <a id="lklDescargarTarifas" style="color: #0000FF">Descargar Tarifas</a>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <a id="lklDescargarPartes" style="color: #0000FF">Descargar Partes</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

