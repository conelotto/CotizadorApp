<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmHomologacion.aspx.vb" Inherits="Cotizador.frmAdmHomologacion" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0" >
        <tr>
            <td style ="text-align:left" class="TextoTitulo" >
               <asp:Label ID="lblTitulo" runat="server" Text="Mantenimiento de Homologación"  ></asp:Label>
            </td>
            
        </tr> 
    </table>
     <br />
     <table id="jqgHomologacion"></table>
      <div id ="diagMantenimiento" title ="Registro de Homologación">
            <div class ="divHeadTitle100">Datos</div>
            <div class ="divContenido100">
            <table style="width: 100%; text-align:left">
            <tr>
                <td style="width:80px;"> Tabla : </td>
                <td colspan ="3"> 
                    <input id="txtTabla" type="text" style="width:80% " />
                    <input type ="hidden" id="hdfAccionHomologacion"/>
                    <input type ="hidden" id="hdfIdHomologacion"/> 
                </td>  
            </tr>
            <tr>
                <td style="width:80px;">Descripción :</td>
                <td colspan ="3">  
                    <textarea id="txtDescripcion" cols="20" rows="2" style="width:98% "></textarea>
                </td> 
            </tr>
            <tr>
                <td valign="top" style="width:80px;"> Valor Sap : </td>
                <td valign="top" colspan ="3">   
                 <input id="txtValorSap" type="text" style="width:80%"  />   
                </td>                              
            </tr>  
            <tr>
             <td valign="top" style="width:80px;"> Valor Cotizador: </td>
                <td valign="top" colspan ="3">   
                 <input id="txtValorCotizador" type="text" style="width:80%"  />   
                </td> 
            </tr>       
            
        </table>
            </div>
            <div id="divBotonesMantenimiento" style =" text-align:center; padding-top:5px; border-bottom-color:Navy; border-width:5px;"> 
                <input id="btnAceptar" type="button" value="Aceptar" onclick ="GuardarHomologacion(); return false;"  />
                <input id="btnCerrar" type="button" value="Cerrar" onclick ="CerrarMantenimiento(); return false;" /> 
            </div>
    </div>

    <%--Contenedor de Paginacion--%>
    <div id="pagerHomologacion"></div>

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
    <option selected ="selected" value="Tabla">Tabla</option>
    <option value="Descripcion">Descripcion</option>
    <option value="ValorSap">Valor Sap</option>
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

    <script language ="javascript"  type ="text/javascript" >
        
        $(document).ready(function () {

            //Agregar la funcionalidad a un string de poder reemplazar sus caracteres vacios
            String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ""); };

         var valwidth = $(window).width();

         $('#btnAceptar').button()
         $('#btnCerrar').button();

            //1.- Modelo de grilla Homologacion
            $("#jqgHomologacion").jqGrid({
                datatype: function () {
                    paginacionHomologacion();
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
                width: (valwidth * 0.95),
                colNames: ['Ide', 'Tabla', 'Descripcion', 'Valor Sap', 'Valor Cotizador'],
                colModel: [
                            { name: 'Ide', hidden: true },
                            { name: 'Tabla',  width: 100, sorttype: "string"},
                            { name: 'Descripcion',  width: 100, sorttype: "string"},
                            { name: 'ValorSap',  width: 100, sorttype: "string" },
                            { name: 'ValorCotizador', width: 100, sorttype: "string" }
                          ],
                rowNum: 10,
                rowList: [5, 10, 20],
                pager: '#pagerHomologacion',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Valores de Homologación"
            });
            //4.- Barra de la Grilla --------------------------------------
            $('#t_jqgHomologacion').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a>| <a id='lblELiminar'>Eliminar</a> | <a id='lblBuscar'>Buscar</a>| <a id='lblListar'>Mostar todos</a></td></tr></table>");

            /* ----------------------------------------------------------------------------------------------------------- */
            jQuery("#jqgHomologacion").jqGrid('navGrid', '#pagerjqgHomologacion', { search: false, refresh: false, edit: false, add: false, del: false });
            /* ----------------------------------------------------------------------------------------------------------- */

            $('a', '#t_jqgHomologacion').click(function (event) {

                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantenimiento('NUEVO');
                        break;
                    case 'lblEditar':
                        AbrirMantenimiento('MODIFICAR');
                        break; 
                    case 'lblELiminar':
                         eliminarHomologacion();
                        break;
                    case 'lblBuscar':
                        BuscarConfiguracion();
                        break;
                    case 'lblListar':
                        LimpiarBusqueda();
                        ResetearValoresPaginacion();
                        paginacionHomologacion();
                        break;
                }

            });

            // 2.- Diag Mantenimiento
            $("#diagMantenimiento").dialog({
                autoOpen: false,
                height: 220,
                width: 700,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

            // 3.- Diag Busqueda
            $("#divBuscar").dialog({
                autoOpen: false,
                height: 120,
                width: 400,
                resizable: true,
                modal: true,
                close: function () {
                }
            });

            // 4.- Evento de Busqueda
            $('#btnBuscarBusqueda').click(function () {
                ResetearValoresPaginacion();
                paginacionHomologacion();

                $("#divBuscar").dialog('close');
            });
            $('#btnLimpiarBusqueda').click(function () {
                LimpiarBusqueda();
            });

        }); // fin de Ready

        function AbrirMantenimiento(Accion) {
            switch (Accion) {
                case 'NUEVO':
                    $('#txtTabla').val('');
                    $('#txtDescripcion').val('');
                    $('#txtValorSap').val('');
                    $('#txtValorCotizador').val('');

                    $('#hdfAccionHomologacion').val('N');
                    $('#hdfIdHomologacion').val('');
                      

                    $('#diagMantenimiento').dialog('open'); 
                    break;
                case 'MODIFICAR':
                    $('#hdfAccionHomologacion').val('M');
                    MostrarConfiguracion();
                    break;
            }


        }

         
        // Consultar Informacion de Homologacion
        function paginacionHomologacion() {

             arreglo = parametro = null;
            sortColumn = jQuery('#jqgHomologacion').getGridParam("sortname");
            sortOrder = jQuery('#jqgHomologacion').getGridParam("sortorder");
            pageSize = jQuery('#jqgHomologacion').getGridParam("rowNum");
            currentPage = jQuery('#jqgHomologacion').getGridParam("page");

            arreglo = fc_redimencionarArray(7);
            arreglo[0][0] = "sortColumn";
            arreglo[0][1] = sortColumn;
            arreglo[1][0] = "sortOrder";
            arreglo[1][1] = sortOrder;
            arreglo[2][0] = "pageSize";
            arreglo[2][1] = pageSize;
            arreglo[3][0] = "currentPage";
            arreglo[3][1] = currentPage; 

            arreglo[4][0] = 'campoBusqueda';
            arreglo[4][1] = $('#cbCampobuscar').val();
            arreglo[5][0] = 'comparacionBusqueda';
            arreglo[5][1] = $('#cbComparacionBuscar').val();
            arreglo[6][0] = 'textoBuscar';
            arreglo[6][1] = $('#txtTextoBuscar').val();


            parametro = fc_parametrosData(arreglo);


            jQuery.ajax({
                url: location.pathname + "/ListarPaginado",
                dataType: "json",
                data: parametro,
                type: "post",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {

                    if (stat == "success") {
                        $("#jqgHomologacion").clearGridData();
                        jQuery("#jqgHomologacion")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

        }

        function ResetearValoresPaginacion() {
            $("#jqgHomologacion").trigger('reloadGrid', [{ page: 1}]);
        }
        function BuscarConfiguracion() {
            $("#divBuscar").dialog('open');
        }
        function CerrarMantenimiento() {
            $('#diagMantenimiento').dialog('close');
        }
        function LimpiarBusqueda() {
            $('#cbCampobuscar').val('Tabla');
            $('#cbComparacionBuscar').val('cn');
            $('#txtTextoBuscar').val('');
        }

        function MostrarConfiguracion() {
            var indice = $('#jqgHomologacion').jqGrid('getGridParam', 'selrow');
            if (!indice) {
                alert('Por favor seleccione una fila');
                return;
            }
            else {
                var reg = $('#jqgHomologacion').jqGrid('getRowData', indice);

                $('#hdfIdHomologacion').val(reg.Ide);
                $('#txtTabla').val(reg.Tabla);
                $('#txtDescripcion').val(reg.Descripcion);
                $('#txtValorSap').val(reg.ValorSap);
                $('#txtValorCotizador').val(reg.ValorCotizador);                  

                $('#diagMantenimiento').dialog('open');
            }
        }

        // Guardar Configuracion de Archivo
        function GuardarHomologacion() {

            var Accion = $('#hdfAccionHomologacion').val();
            var valorSap =  $('#txtValorSap').val();
            var NombTabla = $('#txtTabla').val();

            valorSap = valorSap.trim();
            NombTabla = NombTabla.trim();

            // === Validacion de Ingreso =========

            if (NombTabla=='') {
                alert('Ingrese un nombre de tabla');
                $('#txtTabla').focus();
                return;
            }
            if (valorSap == '') {
                alert('Ingrese un valor Sap');
                $('#txtValorSap').focus();
                return;
            }            

            //====================================

            var arreglo = null;
            var parametros = null;

            arreglo = fc_redimencionarArray(6);
            arreglo[0][0] = 'Accion';
            arreglo[0][1] = $('#hdfAccionHomologacion').val();
            arreglo[1][0] = 'IdHomologacion';
            arreglo[1][1] = $('#hdfIdHomologacion').val();
            arreglo[2][0] = 'Tabla';
            arreglo[2][1] = $('#txtTabla').val();
            arreglo[3][0] = 'Descripcion';
            arreglo[3][1] = $('#txtDescripcion').val();
            arreglo[4][0] = 'ValorSap';
            arreglo[4][1] = $('#txtValorSap').val();
            arreglo[5][0] = 'ValorCotizador';
            arreglo[5][1] = $('#txtValorCotizador').val(); 

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
                             
                            paginacionHomologacion();
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


        function eliminarHomologacion() {

            var id = $("#jqgHomologacion").jqGrid('getGridParam', 'selrow');

            if (!id) {
                alert("Por favor seleccione una fila");
            } else {
                if (!(confirm('¿Seguro desea eliminar?'))) {
                    return;
                }

                var parametro = null;
                var arreglo = null;

                var Regist = $('#jqgHomologacion').jqGrid('getRowData', id);
                var IdHomologacion = Regist.Ide

                arreglo = fc_redimencionarArray(2);
                arreglo[0][0] = 'id';
                arreglo[0][1] = IdHomologacion;
                arreglo[1][0] = 'usuario';
                arreglo[1][1] = 'NombreUsuarioEliminacion';
                parametro = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/EliminarHomologacion",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            // Listar info   
                            paginacionHomologacion();
                            alert('Se eliminó correctamente');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });


            }
        }
        // Redimensionar Grillas
        $(window).bind('resize', function () {
            var valwidth = $(window).width(); 
            $('#jqgHomologacion').setGridWidth(valwidth * 0.95);
        });

    </script>
</asp:Content>
