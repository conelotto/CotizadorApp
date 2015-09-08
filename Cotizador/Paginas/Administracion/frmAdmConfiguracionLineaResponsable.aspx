<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAdmConfiguracionLineaResponsable.aspx.vb" Inherits="Cotizador.frmAdmConfiguracionLineaResponsable" %>
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

            $('#btnAceptarResponsableLinea').button();
            $('#btnCerrar').button();

            $("#jqgLineaResponsable").jqGrid({
                datatype: function () {
                    listarResponsableLinea();
                },
                height: 400,
                width: (valwidth * 0.98),
                colNames: ['IdResponsableLinea', 'Usuario', 'IdLinea', 'Código de Linea', 'Descripción de Linea'],
                colModel: [
                            { name: 'IdResponsableLinea', index: 'IdResponsableLinea', hidden: true },
                            { name: 'Usuario', index: 'Usuario', width: 30, sorttype: "string" },
                            { name: 'IdLinea', index: 'IdLinea', hidden: true },
                            { name: 'CodigoLinea', index: 'CodigoLinea', width: 25, sorttype: "string" },
                            { name: 'DescripcionLinea', index: 'DescripcionLinea', width: 45, sorttype: "string" },
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
                caption: "Responsables por Linea: "
            });
            $('#t_jqgLineaResponsable').append("<table><tr><td style='vertical-aling: middle'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a> | <a id='lblELiminar'>Eliminar</a></td></tr></table>");

            $('a', '#t_jqgLineaResponsable').click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lblNuevo':
                        AbrirMantenimiento('NUE');
                        break;
                    case 'lblEditar':
                        AbrirMantenimiento('MOD');
                        break;
                    case 'lblELiminar':
                        EliminarResponsableLinea();
                        break;
                }
            });

            $("#diagMantResponsableLinea").dialog({
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
            $("#cbLinea option").remove();
            cargarComboLineas(Accion);
            $('#hdnEstado').val(Accion);
            switch (Accion) {
                case 'NUE':                    
                    $('#txtUsuario').val('');
                    $('#diagMantResponsableLinea').dialog('open');
                    $('#txtUsuario').removeAttr('disabled', true);
                    break;
                case 'MOD':
                    var id = $("#jqgLineaResponsable").jqGrid('getGridParam', 'selrow');
                    if (!id) {
                        alert("Por favor seleccione un registro de la tabla.");
                        return;
                    }
                    var fila = $('#jqgLineaResponsable').jqGrid('getRowData', id);
                    $('#txtUsuario').val(fila.Usuario);
                    $('#cbLinea').val(fila.IdLinea);
                    $('#hdnIdResponsableLinea').val(fila.IdResponsableLinea);
                    $('#diagMantResponsableLinea').dialog('open');
                    $('#txtUsuario').attr('disabled', 'disabled');
                    break;
            }
        }

        function listarResponsableLinea() {
            arreglo = parametro = null;
            sortColumn = jQuery('#jqgLineaResponsable').getGridParam("sortname");
            sortOrder = jQuery('#jqgLineaResponsable').getGridParam("sortorder");
            pageSize = jQuery('#jqgLineaResponsable').getGridParam("rowNum");
            currentPage = jQuery('#jqgLineaResponsable').getGridParam("page");

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
                url: location.pathname + "/listarResponsableLinea",
                data: parametro,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                complete: function (jsondata, stat) {
                    if (stat == "success") {
                        $("#jqgLineaResponsable").clearGridData();
                        var thegrid = jQuery("#jqgLineaResponsable")[0];
                        thegrid.addJSONData(JSON.parse(jsondata.responseText).d);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function AceptarResponsableLinea() {

            var arreglo = null;
            var parametros = null;
            var accion = $('#hdnEstado').val();
            var idResponsableLinea = $('#hdnIdResponsableLinea').val();
            var usuario = $('#txtUsuario').val();
            if ($.trim(usuario) == "") {
                alert("Debe ingresar un usuario.");
                return;
            }
            var idLinea = $('#cbLinea').val()
            if (idLinea == "0") {
                alert("Debe seleccionar una Linea.");
                return;
            }

            arreglo = fc_redimencionarArray(4);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = usuario;
            arreglo[2][0] = 'idLinea';
            arreglo[2][1] = idLinea;
            arreglo[3][0] = 'idResponsableLinea';
            arreglo[3][1] = idResponsableLinea;

            parametros = fc_parametrosData(arreglo);

            $.ajax({
                type: 'POST',
                url: location.pathname + '/GuardarResponsableLinea',
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (rpta) {
                    alert('Registro guardado correctamente.');
                    jQuery("#jqgLineaResponsable").trigger("reloadGrid");
                    $('#diagMantResponsableLinea').dialog('close');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function EliminarResponsableLinea() {

            var id = $("#jqgLineaResponsable").jqGrid('getGridParam', 'selrow');
            var fila = $('#jqgLineaResponsable').jqGrid('getRowData', id);
            var IdResponsableLinea = fila.IdResponsableLinea;
            if (!id) {
                alert("Por favor seleccione un registro de la tabla.");
                return;
            }
            if (!confirm('¿Esta seguro que desea eliminar este registro?')) {
                return;
            }


            var parametro = null;
            var arreglo = null;

            arreglo = fc_redimencionarArray(1);

            arreglo[0][0] = 'idResponsableLinea';
            arreglo[0][1] = IdResponsableLinea;

            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/EliminarResponsableLinea",
                data: parametro,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    jQuery("#jqgLineaResponsable").trigger("reloadGrid");
                    alert('Registro eliminado correctamente.');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function cargarComboLineas(accion) {
            var parametro = null;
            var arreglo = null;
            arreglo = fc_redimencionarArray(2);
            arreglo[0][0] = 'accion';
            arreglo[0][1] = accion;
            arreglo[1][0] = 'usuario';
            arreglo[1][1] = $('#txtUsuario').val();
            parametro = fc_parametrosData(arreglo);

            $.ajax({
                type: "POST",
                url: location.pathname + "/llenarComboLineas",
                data: parametro,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#cbLinea option").remove();
                    $('#cbLinea').append('<option value="0" selected="selected">Seleccione</option>');
                    var lista = response.d;
                    if (lista.length == 0) {
                        alert('No se encontró ningun opción para seleccionar.');
                        return;
                    } else {
                        for (var i = 0; i < lista.length; i++) {
                            var varoption = '<option value="' + lista[i].id + '" >' + lista[i].Descripcion + '</option>';
                            $('#cbLinea').append(varoption);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });
        }

        function Cerrar() {
            $('#diagMantResponsableLinea').dialog('close');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td style="text-align: left" class="TextoTitulo">
                <asp:Label ID="lblTitulo" runat="server" Text="Configuración Responsables por Linea"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="jqgLineaResponsable"></table>
    <div id="diagMantResponsableLinea" title="Registro de Responsables por Linea: ">
        <div class="divHeadTitle100">Registrar:</div>
        <div class="divContenido100">
            <input id="hdnEstado" type="hidden" value=""/>
            <input id="hdnIdResponsableLinea" type="hidden" value=""/>
            <table style="width: 100%; text-align: left">
                <tr>
                    <td>
                        <br />
                    </td>                    
                </tr>
                <tr>
                    <td style="width:40%;">
                        Usuario:
                    </td>                    
                    <td>
                        <input type="text" id="txtUsuario" style="width: 80%;" />
                    </td>
                </tr>                
                <tr>
                    <td style="width:40%;">
                        Linea:
                    </td>
                    <td>
                        <select id="cbLinea" style="width: 150px"  >
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesMantTabla" style="text-align: center; padding-top: 5px; border-bottom-color: Navy;
            border-width: 5px;">
            <input id="btnAceptarResponsableLinea" type="button" value="Aceptar" onclick="AceptarResponsableLinea(); return false;" />
            <input id="btnCerrar" type="button" value="Cerrar" onclick="Cerrar(); return false;" />
        </div>
    </div>
   <%-- <div id="pagerMarcadores">
    </div>--%>
</asp:Content>