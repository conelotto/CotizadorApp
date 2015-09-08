<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SapSite.Master" CodeBehind="frmAdmRegistroArchivoConfiguracion.aspx.vb" Inherits="Cotizador.frmAdmRegistroArchivoConfiguracion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<input id="txtCodigo" type="text" readonly="readonly"  />--%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" /> 
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />

    <%--<input id="txtNombre" type="text" style="width:98%"/>--%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <%-- <textarea id="txtValor" cols="20" rows="4" style="width:100% "></textarea>--%>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.uploadify.min.js" type="text/javascript"></script>
  
    <script src="../../Scripts/jQuery-1.8.0/json2.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id ="diagMantenimiento" title ="Registro Archivo de Sección y Sub Sección">
    <div class ="divHeadTitle100" style ="width:50%">Datos <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label> </div>
    <div class ="divContenido100" style ="width:50%">
        
        <table style="width: 100%; text-align:left">
            <tr>
                <td style="width:80px;"> Código : </td>
                <td> 
                    <input id="txtCodigo" type="text" readonly="readonly"  />
                </td>
                <td>
                <input type ="hidden" id="hdfAccionArchivoConfiguracion"/>
                <input type ="hidden" id="hdfIdArchivoConfiguracion"/>
                <input type ="hidden" id="hdfIdSeccionCriterio"/>
                <input type ="hidden" id="hdfIdSubSeccionCriterio"/>
                <input type ="hidden" id="hdfTipo"/>
                <input type ="hidden" id="hdfNombreArchivoInicial"/>
                <input type ="hidden" id="hdfNombreArchivoGenerado"/>
                <input type ="hidden" id="hdfCargarArchivo"/> 
                </td> 
            </tr>
            <tr>
            <td style="width:80px;"> Criterio : </td>
            <td >  <select id ="cbCriterio" style ="width:150px"></select> </td>
                <%--<select id="Select1">
                    <option></option>
                </select>--%>

            </tr>
            <tr>
                <td style="width:80px;">Nombre :</td>
                <td colspan ="3">  
                    <textarea id="txtNombre" cols="20" rows="2" style="width:98% "></textarea>
                </td> 
            </tr>
            <tr style="visibility:collapse;">
                <td valign="top" style="width:80px;"> Valor : </td>
                <td valign="top">  
                 <textarea id="txtValor" cols="20" rows="1" style="width:100% "></textarea>
                </td>                
            </tr>
            <tr>
                <td valign="middle">
                    Archivo :
                </td>
                <td valign="middle">
                    <div>
                        <iframe id="myFrame" src="frmAdmCargaNE.aspx" width="400" height="85" frameborder="0"></iframe>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td valign="top" style="padding-left: 10px;">
                    <a id="lklNombreArchivo" style="color: #0000FF"></a>
                </td>
            </tr>
            <%--<tr>
            <td valign="middle"> Archivo : </td>
                <td valign="top" >
                <table>
                    <tr>                    
                        <td valign="top"><input id="fileArchivo" type="file" /></td>
                        <td valign="top" style="padding-left :10px;"><a id="lklNombreArchivo" style="color:#0000FF" ></a> 
                            <img alt="" src="../../Images/ActionDelete.gif" id="imgCerrar" style="visibility:hidden" onclick="LimpiarUrlArchivo(); return false;" /> 
                        </td>
                    </tr>
                </table>                  
            </td>
            </tr>--%>          
            
        </table>
        <asp:HiddenField ID="hdfCodigoBusqueda" runat="server" />
        <asp:HiddenField ID="hdfTipoBusqueda" runat="server" />
        <asp:HiddenField ID="hdfUsuarioBusqueda" runat="server" />
        <asp:HiddenField ID="hdfCodSeccionBusqueda" runat="server" />
        <asp:HiddenField ID="hdfDescripcionBusqueda" runat="server" />
    </div>
     <div id="divBotonesMantenimiento" style =" text-align:center; padding-top:5px; border-bottom-color:Navy; border-width:5px;width:50%">
        <input id="btnAceptar" type="button" value="Grabar" onclick ="GuardarConfiguracionArchivo(); return false;" />
        <input id="btnCerrar" type="button" value="Cerrar" onclick ="CerrarMantenimiento(); return false;" />
        
    </div>
    </div>
    <script type ="text/javascript" language ="javascript" >

        var frameSW = false;

        $(document).ready(function () {

            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            document.getElementsByTagName('iframe')[0].onload = function () {

                EliminarArchivoTemporal();

                if (/(MSIE\ [0-9]{1})/i.test(navigator.userAgent)) {
                    window.frameSW = true;
                }
                if (window.frameSW) {
                    var childiFrame = document.getElementById("myFrame");
                    var innerDoc = childiFrame.contentDocument || ChildiFrame.contentWindow.document;
                    var yourChildiFrameControl = innerDoc.getElementById("hdnASPArchivo");
                    var value = yourChildiFrameControl.value;
                    $('#hdfCargarArchivo').val('SI');

                    $('#lklNombreArchivo').html(value);
                    var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + value.toString() + '&Opc=TEMPORAL';
                    //Asignar la url para descargar el archivo
                    $('#lklNombreArchivo').attr('href', urlArchivo);
                    $('#lklNombreArchivo').attr('target', '_blank');
                }
                else {
                    window.frameSW = true;
                }
            }

            $("#fileArchivo").uploadify({
                'swf': '../../Styles/uploadify.swf',
                'uploader': '../../Comunes/UploadVB.ashx',
                'cancelImg': '../../Styles/uploadify-cancel.png',
                'displayData': 'percentage',
                'fileTypeExts': '*.docx', //'*.*',
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
                'onSelect': function (file) {
                    if ($('#hdfAccionArchivoConfiguracion').val() == 'N' || $('#lklNombreArchivo').html() == '') {
                        EliminarArchivoTemporal();
                        GenerarNombreArchivo(file.name);
                    }
                    var auth = '<%=If(Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing, String.Empty, Request.Cookies(FormsAuthentication.FormsCookieName).Value) %>';
                    var ASPSESSID = "<%=Session.SessionID%>";
                    var TIPOACCION = 'SUBIR_ARCHIVO';
                    var NombreArchivoActualizar = '';

                    NombreArchivoActualizar = ExtraerNombreArchivo($('#lklNombreArchivo').html());
                    NombreArchivoActualizar = NombreArchivoActualizar + file.type

                    $("#fileArchivo").uploadify('settings', 'formData', { 'Archivo': NombreArchivoActualizar, 'ASPSESSID': ASPSESSID, 'AUTHID': auth, 'TIPOACCION': TIPOACCION, 'DESTINO': 'TEMPORAL' });
                },
                'onUploadSuccess': function (file, data, response) {
                    $('#hdfCargarArchivo').val('SI');
                    alert('Se cargó correctamente.');
                },
                'onCancel': function (file) {
                    $('#hdfCargarArchivo').val('NO');
                },
                'onUploadError': function (file, errorCode, errorMsg, errorString) {
                    var nombArchivoGenerado = '';
                    $('#lklNombreArchivo').html(nombArchivoGenerado);
                    alert('Error: '+ errorCode + ' - ' + errorMsg + ': El archivo ' + file.name + ' no pudo guardarse. Vuelva intentarlo - ' + errorString);
                }
            });

            CriterioPorSeccion();
            BuscarRegistro();
        });
 
        $(window).unload(function () {
            EliminarArchivoTemporal();
            //$('#lklNombreArchivo').html()
        });

        function BuscarRegistro() {
            var cod = $('#MainContent_hdfCodigoBusqueda').attr('value');
            var tip = $('#MainContent_hdfTipoBusqueda').val();

            // asignacion Descripcion
            $('#txtNombre').val($('#MainContent_hdfDescripcionBusqueda').val());

            var parametro = null;
            var arreglo = null;             

            arreglo = fc_redimencionarArray(2);
            arreglo[0][0] = 'cod';
            arreglo[0][1] = cod;
            arreglo[1][0] = 'tipo';
            arreglo[1][1] = tip;
            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/ConsultarRegistro",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    var nombreArchivo = '';

                    if (result.IdArchivoConfiguracion != null) {

                        if (result.Archivo != null) {
                            nombreArchivo = result.Archivo.toString()
                        }

                        $('#hdfIdArchivoConfiguracion').val(result.IdArchivoConfiguracion);
                        $('#txtCodigo').val(result.Codigo);
                        $('#txtNombre').val(result.Nombre);
                        $('#lklNombreArchivo').html(result.Archivo);
                        //     result.CodigoSeccion;
                        $('#hdfAccionArchivoConfiguracion').val('M');
                         

                        $('#hdfIdSeccionCriterio').val(result.IdSeccionCriterio);
                        $("#cbCriterio option[value=" + result.IdSeccionCriterio + "]").attr("selected", true);

                        $('#hdfIdSubSeccionCriterio').val(result.IdSubSeccionCriterio);
                        $('#hdfTipo').val(result.Tipo);
                        $('#hdfNombreArchivoInicial').val(result.Archivo);
                        $('#hdfCargarArchivo').val('NO');
                        $('#txtValor').val(result.Valor);
                        //                        $('#imgCerrar').attr('');
                        var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + nombreArchivo + '&Opc=PLANTILLAS';
                        //Asignar la url para descargar el archivo
                        $('#lklNombreArchivo').attr('href', urlArchivo);
                        $('#lklNombreArchivo').attr('target', '_blank');
                    }
                    else {
                        $('#hdfAccionArchivoConfiguracion').val('N');
                        
                        $('#txtCodigo').val(cod);
                        $('#hdfCargarArchivo').val('NO');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }


        // Guardar Configuracion de Archivo
        function GuardarConfiguracionArchivo() {

            if ($('#cbCriterio').val() == '0') {
                $('#cbCriterio').focus();
                alert('Seleccione un criterio');
                return false;
             }

            if ($('#txtNombre').val() == '') {
                $('#txtNombre').focus();
                alert('Ingrese un nombre');                
                return false;
             }

            var Accion = $('#hdfAccionArchivoConfiguracion').val();
            var CodigoRegistro = $('#txtCodigo').val();
            var Tipo = '';

            if (Accion == 'N') {
                Tipo = 'SECCION';
            }
            else {
                Tipo = $('#hdfTipo').val();
            }

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(11);
            arreglo[0][0] = 'Accion';
            arreglo[0][1] = Accion;
            arreglo[1][0] = 'IdArchivoConfiguracion';
            arreglo[1][1] = $('#hdfIdArchivoConfiguracion').val();
            arreglo[2][0] = 'IdSeccionCriterio';
            arreglo[2][1] = $('#cbCriterio').val();
            arreglo[3][0] = 'IdSubSeccionCriterio';
            arreglo[3][1] = $('#hdfIdSubSeccionCriterio').val();
            arreglo[4][0] = 'Tipo';
            arreglo[4][1] = Tipo;
            arreglo[5][0] = 'Codigo';
            arreglo[5][1] = $('#txtCodigo').val();
            arreglo[6][0] = 'Nombre';
            arreglo[6][1] = $('#txtNombre').val();
            arreglo[7][0] = 'Archivo';
            arreglo[7][1] = $('#lklNombreArchivo').html();
            arreglo[8][0] = 'Valor';
            arreglo[8][1] = $('#txtValor').val();
            arreglo[9][0] = 'CargarArchivo';
            arreglo[9][1] = $('#hdfCargarArchivo').val();
            arreglo[10][0] = 'Usuario';
            arreglo[10][1] = $('#MainContent_hdfUsuarioBusqueda').val();

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarMantenimiento',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var rpta = msg.d;
                    switch (rpta) {
                        case "1":
                            $('#hdfCargarArchivo').val('NO');
                            alert('Se guardó correctamente.');

                            if (Accion == 'N') {
                                BuscarRegistro();
                            }

                            window.frameSW = false;
                            reload_element('myFrame');
                            break;
                        case "2":
                            alert('Ya existe un registro con este codigo para el criterio seleccionado');
                            break;
                        case "0":
                            alert('No se pudo guardar');
                            break;
                        default:
                            alert(rpta);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function reload_element(id) {

            if (navigator.appName == "Microsoft Internet Explorer") {
                location.reload();
            } else {
                window.document.getElementById(id).src = window.document.getElementById(id).src;
            }
        }

        function CerrarMantenimiento() {
             
                if (!(confirm('¿Seguro desea Cerrar?'))) {
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
                    $('#hdfNombreArchivoGenerado').val(NombreGenerado);
                     
                    var urlArchivo = '';

                    if ($('#hdfAccionArchivoConfiguracion').val() == 'N') {
                        urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreGenerado + '&Opc=TEMPORAL';
                    }
                    else {
                        urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + NombreGenerado + '&Opc=PLANTILLAS';
                    }

                    //Asignar la url para descargar el archivo
                    $('#lklNombreArchivo').attr('href', urlArchivo);
                    $('#lklNombreArchivo').attr('target', '_blank');
                    //                    $('#imgCerrar').css('visibility', 'visible');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function CriterioPorSeccion() {
            var IdSeccion = $('#MainContent_hdfCodSeccionBusqueda').val();

            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'IdSeccion';
            arreglo[0][1] = IdSeccion.toString();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/ListarCriteriosPorSeccion",
                data: parametro,
                async:false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) { 
                    var lista = response.d;
                    if (lista.length == 0) {
                    } else {
//                        $('#cbCriterio').append('<option value="0" selected="selected">Seleccione... </option>')
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].IdSeccionCriterio + '" >' + lista[i].Nombre + '</option>';
                            $('#cbCriterio').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }
        
        function EliminarArchivoTemporal() { 

            var NombreArchivo = $('#lklNombreArchivo').html();
            var parametro = null;
            var arreglo = null;
            var Resultado='-1'

            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'NombreArchivo';
            arreglo[0][1] = NombreArchivo.toString();

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
    </script>
</asp:Content>
