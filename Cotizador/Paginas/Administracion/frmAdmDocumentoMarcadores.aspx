<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmDocumentoMarcadores.aspx.vb" Inherits="Cotizador.frmAdmDocumentoMarcadores" %>
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

            $('#btnAceptarMarcador').button();
            $('#btnCerrar').button();

            $("#jqgMarcadoresDoc").jqGrid({
                datatype: function () {
                    listarMarcadores();
                },
                height: 400,
                width: (valwidth * 0.98),
                colNames: ['IdMarcador', 'Descripción', 'IdCampo', 'Query', 'IdTipo', 'Tipo'],
                colModel: [
                            { name: 'IdMarcador', index: 'IdMarcador', hidden: true },
                            { name: 'Descripcion', index: 'Descripcion', width: 50, sorttype: "string" },
                            { name: 'IdCampo', index: 'IdCampo', hidden: true },
                            { name: 'Query', index: 'Query', width: 50, sorttype: "string" },
                            { name: 'IdTipo', index: 'IdTipo', hidden: true },
                            { name: 'Tipo', index: 'Tipo', width: 35, sorttype: "string", align: "center" }
                          ],
                rowNum: 500,
//                rowList: [5, 10, 20],
//                pager: '#pagerMarcadores',
//                sortname: 'IdMarcador',
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                loadtext: 'Cargando datos...',
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                pgtext: 'Pág: {0} de {1}',
                caption: "Campos/Marcadores: "
            });
            $('#t_jqgMarcadoresDoc').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");

            $('a', '#t_jqgMarcadoresDoc').click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantenimiento('NUEVO');
                        break;
                    case 'lblEditar':
                        AbrirMantenimiento('MODIFICAR');
                        break;
                    case 'lblELiminar':
                        EliminarMarcador();
                        break;
                }
            });

            $("#diagMantMarcadores").dialog({
                autoOpen: false,
                height: 200,
                width: 400,
                resizable: false,
                modal: true,
                close: function () {
                }
            });

        });   //fin ready

        function AbrirMantenimiento(Accion) {
            $("#cbQuery option").remove();
            cargarComboQuery(Accion);
            switch (Accion) {
                case 'NUEVO':
                    $('#hdnEstado').val('NUE');
                    $('#txtDescripcionMarcador').val('');
                    $('input:radio[name=tipo]').filter('[value=Marcador]').attr('checked', true);
                    $('#diagMantMarcadores').dialog('open');
                    break;
                case 'MODIFICAR':
                    $('#hdnEstado').val('MOD');
                    var id = $("#jqgMarcadoresDoc").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione un registro de la tabla.");
                        return;
                    }
                    var fila = $('#jqgMarcadoresDoc').jqGrid('getRowData', id);
                    $('#txtDescripcionMarcador').val(fila.Descripcion);
                    $('#cbQuery').val(fila.IdCampo);
                    $('#hdnIdMarcadorMod').val(fila.IdMarcador);
                    if (fila.IdTipo == "0") {
                        $('input:radio[name=tipo]').filter('[value=Campo]').attr('checked', true);
                    }
                    else {
                        $('input:radio[name=tipo]').filter('[value=Marcador]').attr('checked', true);
                    }
                    $('#diagMantMarcadores').dialog('open');
                    break;
            }
        }

        function listarMarcadores() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgMarcadoresDoc').getGridParam("sortname");
            sortOrder = jQuery('#jqgMarcadoresDoc').getGridParam("sortorder");
            pageSize = jQuery('#jqgMarcadoresDoc').getGridParam("rowNum");
            currentPage = jQuery('#jqgMarcadoresDoc').getGridParam("page");

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
                url: location.pathname + "/listarDocumentoMarcadores",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgMarcadoresDoc").clearGridData();
                        var thegrid = jQuery("#jqgMarcadoresDoc")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }
              
        function AceptarMarcador() {
            
            var arreglo = null;
            var parametros = null;
            var accion = $('#hdnEstado').val();
            var idMarcador = $('#hdnIdMarcadorMod').val();
            var idTipo = $('input:radio[name=tipo]:checked').val();
            var descipcion = $('#txtDescripcionMarcador').val();
            if ($.trim(descipcion) == "") {
                alert("Debe ingresar una descripción para este marcador.");
                return;
            }
            var QueryCampo = $('#cbQuery').val()
            if (QueryCampo == "0") {
                alert("Debe selccionar un dato para este marcador.");
                return;
            }

            arreglo = fc_redimencionarArray(6);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#hdfLogin').val();
            arreglo[2][0] = 'descripcion';
            arreglo[2][1] = descipcion;
            arreglo[3][0] = 'campo';
            arreglo[3][1] = QueryCampo;
            arreglo[4][0] = 'idMarcador';
            arreglo[4][1] = idMarcador;
            arreglo[5][0] = 'idTipo';
            arreglo[5][1] = idTipo;

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarDocumentoMarcadores',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                   alert('Registro guardado correctamente.');
                   jQuery("#jqgMarcadoresDoc").trigger("reloadGrid");
                   $('#diagMantMarcadores').dialog('close');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function EliminarMarcador(accion) {

            var id = $("#jqgMarcadoresDoc").jqGrid('getGridParam', 'selrow');
            var fila = $('#jqgMarcadoresDoc').jqGrid('getRowData', id);
            var IdMarcador = fila.IdMarcador;
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (!confirm('¿Esta seguro que desea eliminar este registro?')) {
                return;
            }


            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(3);

            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'id';
            arreglo[1][1] = IdMarcador;
            arreglo[2][0] = 'usuario';
            arreglo[2][1] = $('#hdfLogin').val();

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/EliminarMarcador",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    jQuery("#jqgMarcadoresDoc").trigger("reloadGrid");
                    alert('Registro eliminado correctamente.');
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
                    $("#cbQuery option").remove();
                    $('#cbQuery').append('<option value="0" selected="selected">Seleccione</option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun opción para seleccionar.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbQuery').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function Cerrar() {
            $('#diagMantMarcadores').dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Configuración de Campos/Marcadores para Documentos"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgMarcadoresDoc"></table>
    <div id="diagMantMarcadores" title="Registro de Campos/Marcadores: ">
        <div class="divHeadTitle100">Registrar:</div>
        <div class="divContenido100">
            <input id="hdnEstado" type="hidden" value=""/>
            <input id="hdnIdMarcadorMod" type="hidden" value=""/>
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 100px;">
                        <input type="radio" name="tipo" value="Marcador" checked="checked" />Marcador
                    </td>
                    <td style="text-align: left">
                        <input type="radio" name="tipo" value="Campo" />Campo
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>                    
                </tr>
                <tr>
                    <td style="width:40%;">
                        Descripción:
                    </td>                    
                    <td>
                        <input type="text" id="txtDescripcionMarcador" style="width: 80%;" />
                    </td>
                </tr>                
                <tr>
                    <td style="width:40%;">
                        Query:
                    </td>
                    <td>
                        <select id="cbQuery" style="width: 150px"  >
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantTabla" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptarMarcador" type="button" value="Aceptar" onclick="AceptarMarcador(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="Cerrar(); return false;" />
        </div>
    </div>
   <%-- <div id="pagerMarcadores">
    </div>--%>
</asp:Content>
