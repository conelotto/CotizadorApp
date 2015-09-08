<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SapSite.Master" CodeBehind="frmAdmEspecificacionTecnicaBackup.aspx.vb" Inherits="Cotizador.frmAdmEspecificacionTecnica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--Style--%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" /> 
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />

    <%--jquery--%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jQuery-1.8.0/jquery.alert.js" type="text/javascript"></script>--%>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.uploadify.min.js" type="text/javascript"></script>
  
    <script src="../../Scripts/jQuery-1.8.0/json2.js" type="text/javascript"></script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0" >
        <tr>
            <td style ="text-align:left" class="TextoTitulo" >
               <asp:Label ID="lblTitulo" runat="server" Text="Especificaciones Tecnicas de Productos"  ></asp:Label>
            </td>
            <td style="text-align:right" >
                <div id="submenu">
                    <p> 
                    <%--<a id="lblConsultar" onclick ="ConsultarModelo(); return false">Consultar</a> | <a id="lblLimpiar" onclick ="LimpiarDatosConsulta(); return false"> Limpiar</a> --%>
                    </p>
                </div>
            </td>
            
        </tr> 
    </table>
     <br />
    <div class ="divHeadTitle100"> Busqueda </div>
         <div class ="divContenido100">  
        <div style ="padding-bottom:5px; ">
           <span> &nbsp; &nbsp; Sección &nbsp; &nbsp; &nbsp;:</span>  <select id="cbSeccion" style ="width:30%" disabled="disabled">
            </select>
        </div>
    </div>
    <table >
    <tr>
    <td><table id="jqgCriterio"></table></td>
    <td><table id="jqgConfiguracion"></table></td>
    </tr>
    </table>
    <div id ="diagMantenimiento" title ="Registro Archivo de Sección y Sub Sección">
    <div class ="divHeadTitle100">Datos</div>
    <div class ="divContenido100">
        
        <table style="width: 100%; text-align:left">
            <tr>
                <td style="width:80px;"> Código : </td>
                <td>
                    <%--<input id="txtCodigo" type="text" readonly="readonly"  />--%>
                    <input id="txtCodigo" type="text" />
                </td>
                <td>
                <input type ="hidden" id="hdfAccionArchivoConfiguracion"/>
                <input type ="hidden" id="hdfIdArchivoConfiguracion"/>
                <input type ="hidden" id="hdfIdSeccionCriterio"/>
                <input type ="hidden" id="hdfIdSubSeccionCriterio"/>
                <input type ="hidden" id="hdfTipo"/>
                <input type ="hidden" id="hdfNombreArchivoInicial"/>
                <input type ="hidden" id="hdfCargarArchivo"/>

                </td> 
            </tr>
            <tr>
                <td style="width:80px;">Nombre :</td>
                <td colspan ="3"> 
                    <%--<input id="txtNombre" type="text" style="width:98%"/>--%>
                    <textarea id="txtNombre" cols="20" rows="2" style="width:98% "></textarea>
                </td> 
            </tr>
            <tr style="visibility:collapse;">
                <td valign="top" style="width:80px;"> Valor : </td>
                <td valign="top">  
                 <textarea id="txtValor" cols="20" rows="1" style="width:100% "></textarea>                   
                   <%-- <textarea id="txtValor" cols="20" rows="4" style="width:100% "></textarea>--%>
                </td>                
            </tr>
            <tr>
            <td valign="middle"> Archivo : </td>
                <td valign="top" >
                <table>
                    <tr>                    
                        <td valign="top"><input id="fileArchivo" type="file" /></td>
                        <td valign="top" style="padding-left :10px;"><a id="lklNombreArchivo" style="color:#0000FF" ></a> 
                            <img alt="" src="../../Images/ActionDelete.gif" id="imgCerrar" style="visibility:hidden" onclick="LimpiarUrlArchivo(); return false;" /> </td>
                    </tr>
                </table>                  
            </td>
            </tr>          
            
        </table>
        
    </div>
     <div id="divBotonesMantenimiento" style =" text-align:center; padding-top:5px; border-bottom-color:Navy; border-width:5px;">
        <input id="btnAceptar" type="button" value="Aceptar" onclick ="GuardarConfiguracionArchivo(); return false;" />
        <input id="btnCerrar" type="button" value="Cerrar" onclick ="CerrarMantenimiento(); return false;" />
    </div>
    </div>

    <div id="diagMarcadores" title ="Configuración de Marcadores">
       <table id ="jqgMarcadores"></table>
       <div id="divBotonesMarcadores" style =" text-align:center; padding-top:5px; border-bottom-color:Navy; border-width:5px;">
        <input id="btnAceptarMarcador" type="button" value="Aceptar" onclick ="GuardarConfiguracionMarcador(); return false;" />
        <input id="btnCancelarMarcador" type="button" value="Cerrar" onclick ="CerrarConfiguracionMarcador(); return false;" />
    </div>
    </div>
    <%--Contenedor de Paginacion--%>
    <div id="pagerConfiguracion"></div>
    <%--Busqueda--%>
    <div  id="divBuscar" title ="Busqueda">
<div id="searchcntfbox_jqgConfiguracion">
<div style="OVERFLOW: auto" id="fbox_jqgConfiguracion" class="searchFilter" filter="true">
<div >
<table style=" width :100%;BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; BORDER-TOP: 0px; BORDER-RIGHT: 0px" class="group ui-widget ui-widget-content">
<tbody>
<tr style="DISPLAY: none" class="error">
<th class="ui-state-error" colSpan="5" align="left"></th>
</tr>
<tr>
<th colSpan="5" align="left"><sapn></span></th>
</tr>
<tr>
<td class="first"></td>
<td class="columns">
<select id ="cbCampobuscar">
    <option value="Codigo">Código</option>
    <option selected ="selected" value="Nombre">Nombre</option>
    <option value="Archivo">Archivo</option>
    </select>
</td>
<td class="operators">
<select class="selectopts" id ="cbComparacionBuscar" >
<option value="eq">igual</option>
<option value="bw">empiece por</option>
<option selected="selected"  value="cn">contiene</option>
</select>
</td>
<td class="data" style ="width:60%"><input id="txtTextoBuscar" class="input-elm" role="textbox" size="10" type="text"  style="width:98%"/></td>
<td></td></tr></tbody></table>
</div>
<table style=" width :100%; BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; MARGIN-TOP: 5px; BORDER-TOP: 0px; BORDER-RIGHT: 0px" id="fbox_jqgConfiguracion_2" class="EditTable">
<tbody>
<tr>
<td colSpan="2">
<hr style="MARGIN: 1px; width :100%" class="ui-widget-content"/>
</td></tr>
<tr>
<td style="TEXT-ALIGN: left" class="EditButton">
<a id="btnLimpiarBusqueda" class="fm-button ui-state-default ui-corner-all fm-button-icon-left ui-search" href="javascript:void(0)">
<span class="ui-icon ui-icon-arrowreturnthick-1-w"></span>Limpiar</a></td>
<td style="TEXT-ALIGN: right"  class="EditButton">
<a id="btnBuscarBusqueda" class="fm-button ui-state-default ui-corner-all fm-button-icon-right ui-reset" href="javascript:void(0)" >
<span class="ui-icon ui-icon-search"></span>Buscar</a>
</td>
</tr>
</tbody>
</table>
</div>
</div>  
</div>

    <script type="text/javascript" language ="javascript" >

        $(document).ready(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            var valwidth = $(window).width();
            //valwidth = (valwidth * 0.80)

            $('#btnAceptar').button()
            $('#btnCerrar').button();

            $('#btnAceptarMarcador').button()
            $('#btnCancelarMarcador').button();

            //1.- Modelo de Criterio
            $("#jqgCriterio").jqGrid({
                datatype: "local",
                height: 300,
                width: (valwidth * 0.15),
                colNames: ['Ide', 'IdCriterio', 'Tipo', 'Nombre'],
                colModel: [
                            { name: 'Ide', hidden: true },
                            { name: 'IdCriterio', hidden: true },
                            { name: 'Tipo', hidden: true },
                            { name: 'Nombre', width: 140, sorttype: "string" }
   	                    ],
                rowNum: 10,
                rowList: [10, 20, 30],
                pager: '#pagerCriterio',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                caption: "Criterios",
                onSelectRow: function (rowId) {
                    LimpiarBusqueda();
                    ResetearValoresPaginacion();
                    paginacionConfiguracion();
                } 
                // ondblClickRow: function (rowId) { FiltrarConfiguracion(rowId); }
            });

            //2.- Modelo de Configuracion
            $("#jqgConfiguracion").jqGrid({
                datatype: function () {
                    paginacionConfiguracion();
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
                height: 300,
                width: (valwidth * 0.80),
                colNames: ['Ide', 'IdSeccionCriterio', 'IdSubSeccionCriterio', 'Tipo', 'Código', 'Nombre', 'Archivo', 'Valor'],
                colModel: [
                            { name: 'Ide', hidden: true },
                            { name: 'IdSeccionCriterio', hidden: true },
                            { name: 'IdSubSeccionCriterio', hidden: true },
                            { name: 'Tipo', hidden: true },
                            { name: 'Codigo', width: 50, sorttype: "string" },
                            { name: 'Nombre', width: 140, sorttype: "string" },
                            { name: 'Archivo', width: 140, sorttype: "string" },
                            { name: 'Valor', width: 140, sorttype: "string", hidden: true }
   	                    ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerConfiguracion',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Configuración de Archivos"

            });
            //4.- Barra de la Grilla --------------------------------------
            $('#t_jqgConfiguracion').append("<table style='display: none;' ><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a>| <a id='lblELiminar'>Eliminar</a>| <a id='lblBuscar'>Buscar</a>| <a id='lblListar'>Mostar todos</a></td></tr></table>");

            /* ----------------------------------------------------------------------------------------------------------- */
            jQuery("#jqgConfiguracion").jqGrid('navGrid', '#pagerConfiguracion', { search: false, refresh: false, edit: false, add: false, del: false });
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
                    case 'lblMarcadores':
                        AbrirMarcadores();
                        break;
                    case 'lblELiminar':
                        eliminarConfiguracion();
                        break;
                    case 'lblBuscar':
                        BuscarConfiguracion();
                        break;
                    case 'lblListar':
                        LimpiarBusqueda();
                        ResetearValoresPaginacion();
                        paginacionConfiguracion();
                        break;
                }

            });

            // 3.- Diag Mantenimiento
            $("#diagMantenimiento").dialog({
                autoOpen: false,
                height: 300,
                width: 700,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            // 4.- File Archivo          

            $("#fileArchivo").uploadify({
                'swf': '../../Styles/uploadify.swf',
                'uploader': '../frmCargar.aspx',
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
                'onDialogOpen': function (queueData) {
                    $('#btnAceptar').attr('disabled', 'disabled');
                    $('#btnCerrar').attr('disabled', 'disabled');
                    //                    $('#fileArchivo').css('visibility', 'hidden');
                },
                'onDialogClose': function (queueData) {
                    $('#btnAceptar').removeAttr('disabled', true);
                    $('#btnCerrar').removeAttr('disabled', true);
                    //                    $('#fileArchivo').css('visibility', 'visible');
                },
                'onUploadStart': function (file) {
                    $('#btnAceptar').attr('disabled', 'disabled');
                    $('#btnCerrar').attr('disabled', 'disabled');
                    $("#fileArchivo").css('opacity', '0');


                    GenerarNombreArchivo(file.name);
                },
                'onUploadSuccess': function (file, data, response) {
                    $('#hdfCargarArchivo').val('SI');
                    $('#btnAceptar').removeAttr('disabled', true);
                    $('#btnCerrar').removeAttr('disabled', true);
                    $("#fileArchivo").css('opacity', '1');

                },
                'onCancel': function (file) {
                    //alert('The file ' + file.name + ' was cancelled.');
                    var NombreArchivoInicial = $('#hdfNombreArchivoInicial').val();
                    $('#lklNombreArchivo').html(NombreArchivoInicial);
                    $('#hdfCargarArchivo').val('NO');
                    $('#btnAceptar').removeAttr('disabled', true);
                    $('#btnCerrar').removeAttr('disabled', true);
                    $("#fileArchivo").css('opacity', '1');
                }
            });

            // 5.- Consultas
            ListarSeccion();
            // 6.- Listar Subseccion y Criterios segun Seccion
            $('#cbSeccion').change(function () {
                var valueSeccion = $('#cbSeccion option:selected').val();
                if (valueSeccion == '0') {
                    $("#jqgCriterio").clearGridData();
                    $("#jqgConfiguracion").clearGridData();
                    return;
                }
                else {
                    CriterioPorSeccion(valueSeccion);
                }
            });

            // 8.- Diag Mantenimiento
            $("#diagMarcadores").dialog({
                autoOpen: false,
                height: 500,
                width: 500,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            //9.- Modelo de Configuracion
            $("#jqgMarcadores").jqGrid({
                datatype: "local",
                height: 400,
                width: 450, //(valwidth * 0.60),
                colNames: ['Ide', 'IdArchivoConfiguracion', 'Marcador', 'Campo'],
                colModel: [
                            { name: 'Ide', width: 40, hidden: true },
                            { name: 'IdArchivoConfiguracion', hidden: true },
                            { name: 'NombreMarcadorCotizacion', width: 140, sorttype: "string" },
                            { name: 'NombreMarcador', width: 140, editable: true, sorttype: "string" }
   	                    ],
                rowNum: 10,
                rowList: [10, 20, 30],
                rownumbers: true,
                viewrecords: true,
                sortorder: "desc"
            });

            // 10.- Diag Busqueda
            $("#divBuscar").dialog({
                autoOpen: false,
                height: 120,
                width: 400,
                resizable: true,
                modal: true,
                close: function () {
                }
            });


            // 11.- Evento de Busqueda
            $('#btnBuscarBusqueda').click(function () {
                ResetearValoresPaginacion();
                paginacionConfiguracion();

                $("#divBuscar").dialog('close');
            });

            $('#btnLimpiarBusqueda').click(function () {
                LimpiarBusqueda();
            });

        });       // Fin de ready

        function LimpiarBusqueda() {
            $('#cbCampobuscar').val('Nombre');
            $('#cbComparacionBuscar').val('cn');
            $('#txtTextoBuscar').val('');
        }

        function eliminarConfiguracion() {

            var id = $("#jqgConfiguracion").jqGrid('getGridParam', 'selrow');

            if (!id) {
                alert("Por favor seleccione una fila");
            } else {
                if (!(confirm('¿Seguro desea eliminar?'))) {
                    return;
                }

                var parametro = null;
                var arreglo = null;

                var Regist = $('#jqgConfiguracion').jqGrid('getRowData', id);
                var IdArchivoConfiguracion = Regist.Ide

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'id';
                arreglo[0][1] = IdArchivoConfiguracion;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = 'NombreUsuarioEliminacion';
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/EliminarArchivoConfiguracion",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            // Listar info                       
                            var indice = $('#jqgCriterio').jqGrid('getGridParam', 'selrow');
                            paginacionConfiguracion();
                            alert('Se eliminó correctamente');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });


            }
        }

        function AbrirMantenimiento(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#txtCodigo').val('');
                    $('#txtNombre').val('');
                    $('#txtValor').val('');

                    $('#hdfAccionArchivoConfiguracion').val('N');
                    $('#hdfIdArchivoConfiguracion').val('');

                    var indice = $('#jqgCriterio').jqGrid('getGridParam', 'selrow');
                    if (!indice) {
                        alert('Por favor seleccione un Criterio');
                        return;
                    }
                    else {
                        var reg = $('#jqgCriterio').jqGrid('getRowData', indice);
                        $('#hdfIdSeccionCriterio').val(reg.Ide);
                        $('#hdfIdSubSeccionCriterio').val(reg.Ide);
                        $('#hdfTipo').val(reg.Tipo);
                    }


                    $('#lklNombreArchivo').html('');
                    $('#imgCerrar').css('visibility', 'hidden');

                    $('#diagMantenimiento').dialog('open');
                    $('#hdfNombreArchivoInicial').val('');
                    break;
                case 'MODIFICAR':
                    $('#hdfAccionArchivoConfiguracion').val('M');
                    MostrarConfiguracion();
                    break;
            }


        }

        function BuscarConfiguracion() { 
            $("#divBuscar").dialog('open');
        }

        function AbrirMarcadores() {
            var indice = $('#jqgConfiguracion').jqGrid('getGridParam', 'selrow');
            if (!indice) {
                alert('Por favor seleccione una fila');
                return;
            }
            else {
                ListarMarcadoresCotizacion();
                $('#diagMarcadores').dialog('open');
            }
        }

        function ListarMarcadoresCotizacion() {
            var indice = $('#jqgConfiguracion').jqGrid('getGridParam', 'selrow');
            if (!indice) {
                alert('Por favor seleccione una fila');
                return;
            }
            else {
                var reg = $('#jqgConfiguracion').jqGrid('getRowData', indice);
                var Id = reg.Ide;
                var Archivo = reg.Archivo;

                var parametro = null;
                var arreglo = null;
                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'IdArchivoConfiguracion';
                arreglo[0][1] = Id.toString();
                arreglo[1][0] = 'NombreArchivo';
                arreglo[1][1] = Archivo.toString();

                parametro = fc_parametrosData(arreglo);

                var ElementosOption = ObtenerListaMarcador();

                $.ajax({
                    url: location.pathname + "/ListarMarcadorCotizacion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        $("#jqgMarcadores").clearGridData();
                        var lista = response.d;
                        if (lista.length == 0) {
                        } else {
                            for (var i = 0; i < lista.length; i++) {
                                var NombreControl = 'cboMarcador' + i.toString()
                                var ComboMarcador = '<select id=\'' + NombreControl + '\' style="width:80%">'
                                ComboMarcador = ComboMarcador.concat(ElementosOption);
                                ComboMarcador = ComboMarcador.concat('</select>');

                                var mydata = [
                                { Ide: lista[i].IdMarcadorCotizacion,
                                    IdArchivoConfiguracion: lista[i].IdArchivoConfiguracion,
                                    NombreMarcadorCotizacion: lista[i].NombreMarcadorCotizacion,
                                    NombreMarcador: ComboMarcador
                                }
                            ];
                                $("#jqgMarcadores").jqGrid('addRowData', i + 1, mydata[0]);

                                // $('#' + NombreControl + ' option:selected','Fecha').text();
                                $('#' + NombreControl + ' option[label=' + lista[i].NombreMarcador + ']').attr('selected', 'selected');

                            }
                            //$("#jqgMarcadores").trigger("reloadGrid");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }
        }
        function ObtenerListaMarcador() {

            var ElementosOption = '<option value="0"> </option>';
            $.ajax({
                url: location.pathname + "/ListarMarcadores",
                data: '{}',
                dataType: "json",
                type: "post",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var listaMarcador = data.d;
                    for (var i = 0; i < listaMarcador.length; i++) {

                        ElementosOption = ElementosOption.concat('<option value="' + listaMarcador[i].IdMarcador + '" label ="' + listaMarcador[i].NombreMarcador + '">' + listaMarcador[i].NombreMarcador + '</option>');
                    }
                    //return listaMarcador;
                },
                onComplete: function (data, response) {
                    alert(data);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
            return ElementosOption;
        }

        function CerrarMantenimiento() {
            $('#diagMantenimiento').dialog('close');
        }

        function CerrarConfiguracionMarcador() {
            $('#diagMarcadores').dialog('close');
        }

        function ListarSeccion() {

            $("#cbSeccion").find('option').remove().end();
            jQuery.ajax({
                url: location.pathname + "/ListarSeccion",
                data: '{}',
                dataType: "json",
                type: "post",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var lista = data.d;
//                    $('#cbSeccion').append('<option value="" selected="selected"> </option>')
                    $.each(lista, function (index, value) {
                        var options = $('#cbSeccion').attr('options');
                        options[options.length] = new Option(value.Nombre, value.IdSeccion, true, true);
                    });

                    $("#cbSeccion option").eq(0).attr("selected", "selected");
                    var valueSeccion = $('#cbSeccion option:selected').val();
                    CriterioPorSeccion(valueSeccion)
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function CriterioPorSeccion(IdSeccion) {
            $("#jqgCriterio").clearGridData();
            $("#jqgConfiguracion").clearGridData();


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
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //$("#jqgCriterio").clearGridData();
                    var lista = response.d;
                    if (lista.length == 0) {
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            //                            var urlArchivo = '<a href ="#">Descargar</a>'
                            var mydata = [
                                { Ide: lista[i].IdSeccionCriterio,
                                    IdCriterio: lista[i].IdCriterio,
                                    Tipo: 'SECCION',
                                    Nombre: lista[i].Nombre
                                }
                            ];
                            $("#jqgCriterio").jqGrid('addRowData', i + 1, mydata[0]);
                        }
                        $("#jqgCriterio").trigger("reloadGrid");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

      
        function MostrarConfiguracion() {
            var indice = $('#jqgConfiguracion').jqGrid('getGridParam', 'selrow');
            if (!indice) {
                alert('Por favor seleccione una fila');
                return;
            }
            else {
                var reg = $('#jqgConfiguracion').jqGrid('getRowData', indice);

                $('#hdfIdArchivoConfiguracion').val(reg.Ide);
                $('#hdfIdSeccionCriterio').val(reg.IdSeccionCriterio);
                $('#hdfIdSubSeccionCriterio').val(reg.IdSubSeccionCriterio);
                $('#hdfTipo').val(reg.Tipo);
                $('#txtCodigo').val(reg.Codigo);
                $('#txtNombre').val(reg.Nombre);
                $('#txtValor').val(reg.Valor);
                $('#lklNombreArchivo').html(reg.Archivo);
                $('#hdfNombreArchivoInicial').val(reg.Archivo);
                //$('#fileArchivo').val(reg.Archivo + '88888');
                $('#imgCerrar').css('visibility', 'hidden');

                var urlArchivo = '../frmDescargarFTP.aspx?NombreArchivo=' + reg.Archivo.toString() + '&Opc=PLANTILLAS';
                //Asignar la url para descargar el archivo
                $('#lklNombreArchivo').attr('href', urlArchivo);
                $('#lklNombreArchivo').attr('target', '_blank');
                //alert(urlArchivo);

                $('#diagMantenimiento').dialog('open');
            }
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
                    $('#imgCerrar').css('visibility', 'visible');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        // Guardar Configuracion de Archivo
        function GuardarConfiguracionArchivo() {
            var Accion = $('#hdfAccionArchivoConfiguracion').val();
            var CodigoRegistro = $('#txtCodigo').val();
            // === Validacion de Ingreso =========
            if (CodigoRegistro == '') {
                alert('Ingrese un código');
                $('#txtCodigo').focus();
                return;
            }

            var arrayId = $('#jqgConfiguracion').getDataIDs();
            var cantidRegistro = arrayId.length.toString();
            var cadenaJson = ""
            for (var i = 0; i < cantidRegistro; i++) {
                rowGrid = $("#jqgConfiguracion").jqGrid('getRowData', arrayId[i]);

                if (CodigoRegistro == rowGrid.Codigo) {
                    switch (Accion) {
                        case 'N':
                            alert('El código ya está registrado. Ingrese otro por favor');
                            return;
                            break;
                        case 'M':
                            if ($('#hdfIdArchivoConfiguracion').val() != rowGrid.Ide) {
                                alert('El código ya está registrado. Ingrese otro por favor');
                                return;
                            }
                            break;
                    }
                }
            }

            //====================================

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(11);
            arreglo[0][0] = 'Accion';
            arreglo[0][1] = $('#hdfAccionArchivoConfiguracion').val();
            arreglo[1][0] = 'IdArchivoConfiguracion';
            arreglo[1][1] = $('#hdfIdArchivoConfiguracion').val();
            arreglo[2][0] = 'IdSeccionCriterio';
            arreglo[2][1] = $('#hdfIdSeccionCriterio').val();
            arreglo[3][0] = 'IdSubSeccionCriterio';
            arreglo[3][1] = $('#hdfIdSubSeccionCriterio').val();
            arreglo[4][0] = 'Tipo';
            arreglo[4][1] = $('#hdfTipo').val();
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
            arreglo[10][1] = 'Formulario Especificacion tecnica'; //$('#MainContent_hdfUsuarioBusqueda').val();

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
                            var indice = $('#jqgCriterio').jqGrid('getGridParam', 'selrow');
                            paginacionConfiguracion();
                            alert('Se guardó correctamente.');
                            CerrarMantenimiento();
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

        // limpiar
        function LimpiarUrlArchivo() {
            var NombreInicial = $('#hdfNombreArchivoInicial').val();
            $('#lklNombreArchivo').html(NombreInicial);
            $('#hdfCargarArchivo').val('NO');
            $('#imgCerrar').css('visibility', 'hidden');
        }

        //Guardar configuracion de marcadores
        function GuardarConfiguracionMarcador() {

            var data = new Array();

            var arrayId = $('#jqgMarcadores').getDataIDs();
            var cantidRegistro = arrayId.length.toString();
            var cadenaJson = ""
            for (var i = 0; i < cantidRegistro; i++) {
                rowGrid = $("#jqgMarcadores").jqGrid('getRowData', arrayId[i]);

                var CampoSelec = $("#cboMarcador" + i + " option:selected").text();

                cadenaJson = cadenaJson.concat(rowGrid.Ide + ';');
                cadenaJson = cadenaJson.concat(rowGrid.IdArchivoConfiguracion + ';');
                cadenaJson = cadenaJson.concat(rowGrid.NombreMarcadorCotizacion + ';');
                cadenaJson = cadenaJson.concat(CampoSelec);
                if (i < cantidRegistro - 1) {
                    cadenaJson = cadenaJson.concat('|');
                }
            }
            var arreglo = null;
            var parametros = null;
            arreglo = fc_redimencionarArray(1);
            arreglo[0][0] = 'data';
            arreglo[0][1] = cadenaJson;

            parametros = fc_parametrosData(arreglo);
            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarMarcadores',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {

                    var rpta = msg.d;
                    if (rpta != '0' && rpta != '-1') { // -1: no se pudo subir el archivo, 0: no se pudo Guardar
                        ListarMarcadoresCotizacion();
                        alert('Se guardó correctamente');
                        CerrarConfiguracionMarcador();
                    }
                    else {
                        alert('No se pudo guardar');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        // Consultar Informacion de Configuracion
        function paginacionConfiguracion() {

            var indice = $('#jqgCriterio').jqGrid('getGridParam', 'selrow');

            if (indice == null) {
                return false;
            }

            var reg = $('#jqgCriterio').jqGrid('getRowData', indice); 

            arreglo = parametro = null;
            sortColumn = jQuery('#jqgConfiguracion').getGridParam("sortname");
            sortOrder = jQuery('#jqgConfiguracion').getGridParam("sortorder");
            pageSize = jQuery('#jqgConfiguracion').getGridParam("rowNum");
            currentPage = jQuery('#jqgConfiguracion').getGridParam("page");
           
            arreglo = fc_redimencionarArray(10);
            arreglo[0][0] = "sortColumn";
            arreglo[0][1] = sortColumn;
            arreglo[1][0] = "sortOrder";
            arreglo[1][1] = sortOrder;
            arreglo[2][0] = "pageSize";
            arreglo[2][1] = pageSize;
            arreglo[3][0] = "currentPage";
            arreglo[3][1] = currentPage;

            arreglo[4][0] = 'Tipo';
            arreglo[4][1] = reg.Tipo.toString();
            arreglo[5][0] = 'IdSeccionCriterio';
            arreglo[5][1] = reg.Ide.toString();
            arreglo[6][0] = 'IdSubSeccionCriterio';
            arreglo[6][1] = reg.Ide.toString();

            arreglo[7][0] = 'campoBusqueda';
            arreglo[7][1] = $('#cbCampobuscar').val();
            arreglo[8][0] = 'comparacionBusqueda';
            arreglo[8][1] = $('#cbComparacionBuscar').val();
            arreglo[9][0] = 'textoBuscar';
            arreglo[9][1] = $('#txtTextoBuscar').val();
             

            parametro = fc_parametrosData(arreglo);


            jQuery.ajax({
                url: location.pathname + "/ConfiguracionBuscarPorCriterioPaginado",
                dataType: "json",
                data: parametro,
                type: "post",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {

                    if (stat == "success") {
                        $("#jqgConfiguracion").clearGridData();
                        jQuery("#jqgConfiguracion")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                     
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        function ResetearValoresPaginacion() {             
            $("#jqgConfiguracion").trigger('reloadGrid', [{ page: 1}]);
        }
        // Redimensionar Grillas
        $(window).bind('resize', function () {
            var valwidth = $(window).width();
            $('#jqgCriterio').setGridWidth(valwidth * 0.15);
            $('#jqgConfiguracion').setGridWidth(valwidth * 0.80);
        });

    </script>
</asp:Content>
