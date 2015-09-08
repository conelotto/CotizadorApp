<%@ Page Title="Tablas de Impresion" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmTablasImpresion.aspx.vb" Inherits="Cotizador.frmAdmTablasImpresion" %>

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
    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/jquery-alert.css" rel="stylesheet" type="text/css" />
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script type="text/javascript">
        jQuery(document).ajaxStart(jQuery.blockUI).ajaxStop(jQuery.unblockUI);
        jQuery(document).ready(function () {
            jQuery(document).bind("contextmenu", function (e) { return false; });

            var arreglo, parametro, result;
            var TipoOperacion, IdSeccion, IdSubSeccion, PosicionInicial, Descripcion;
            var Tipo, Nombre, Opcional, CambioPosicion, Prioridad, Codigo;
            var IdSeccionCriterio, IdCriterio;

            jQuery("#itxtSeccionPosicionInicial").numeric();
            jQuery("#ibtnSeccionGrabar").button();
            jQuery("#ibtnSeccionCancelar").button();

            /********************************************************************************************************************************/

            jQuery("#igvSeccionCriterio").jqGrid({
                datatype: function () {
                    jQuery("#igvSeccionCriterio").clearGridData();
                    jQuery.ajax({
                        url: location.pathname + "/ActualizarSeccionCriterio",
                        data: "{}",
                        dataType: "json",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var lista = data.d;
                            if (lista.length > 0) {
                                for (var i = 0; i < lista.length; i++) {
                                    var mydata = [{
                                        IdSeccionCriterio: lista[i].IdSeccionCriterio,
                                        IdCriterio: lista[i].IdCriterio,
                                        Prioridad: lista[i].Prioridad,
                                        Nombre: lista[i].Nombre                                         
                                    }];
                                    jQuery("#igvSeccionCriterio").jqGrid('addRowData', i + 1, mydata[0]);
                                }
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });
                },
                colNames: ['', '', 'Prioridad', 'Nombre'],
                colModel: [
                            { name: 'IdSeccionCriterio', index: 'IdSeccionCriterio', hidden: true },
                            { name: 'IdCriterio', index: 'IdCriterio', hidden: true },
                            { name: 'Prioridad', index: 'Prioridad', width: 80, align: "right", editable: true, editrules: { number: true} },
                            { name: 'Nombre', index: 'Nombre', sorttype: "string", width: 300 },
   	                      ],
                forceFit: true,
                cellEdit: true,
                cellsubmit: 'clientArray',
                loadtext: 'Cargando datos...',
                emptyrecords: 'No hay resultados',
                height: 120,
                viewrecords: true,
                rownumbers: true,
                shrinkToFit: false,
                toolbar: [true, "top"],
                cmTemplate: { sortable: false }
            });

            jQuery("#t_igvSeccionCriterio").append("<table><tr><td style='vertical-align: middle'><a id='lbnAgregar'>Agregar</a> | <a id='lbnQuitar'>Quitar</a></td></tr></table>");

            jQuery("a", "#t_igvSeccionCriterio").click(function (event) {
                var control = event.target.id;
                if (control == 'lbnAgregar') {
                    agregarSeccionCriterio();
                }
                if (control == 'lbnQuitar') {
                    quitarSeccionCriterio();
                }
            });

            function quitarSeccionCriterio() {

                var idRow = jQuery("#igvSeccionCriterio").jqGrid('getGridParam', 'selrow');
                if (!idRow)
                    return alert('Por favor seleccione una fila');

                var rowGrid = jQuery("#igvSeccionCriterio").jqGrid('getRowData', idRow);

                IdCriterio = rowGrid.IdCriterio;

                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(3);
                arreglo[0][0] = "TipoOperacion";
                arreglo[0][1] = "2";
                arreglo[1][0] = "IdCriterio";
                arreglo[1][1] = IdCriterio;
                arreglo[2][0] = "Nombre";
                arreglo[2][1] = null;
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/AgregarQuitarSeccionCriterio",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else
                            jQuery("#igvSeccionCriterio").trigger("reloadGrid");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function agregarSeccionCriterio() {

                IdCriterio = jQuery("#idllSeccionCriterio").val();
                Nombre = jQuery("#idllSeccionCriterio option:selected").text();

                if (Nombre == '')
                    return alert("seleccione criterio");

                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(3);
                arreglo[0][0] = "TipoOperacion";
                arreglo[0][1] = "1";
                arreglo[1][0] = "IdCriterio";
                arreglo[1][1] = IdCriterio;
                arreglo[2][0] = "Nombre";
                arreglo[2][1] = Nombre;
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/AgregarQuitarSeccionCriterio",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else
                            jQuery("#igvSeccionCriterio").trigger("reloadGrid");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            /*************/

            jQuery("#gvTabla").jqGrid({
                jsonReader: {
                    root: "Items",
                    page: "CurrentPage",
                    total: "PageCount",
                    records: "RecordCount",
                    repeatitems: true,
                    cell: "Row",
                    id: "ID"
                },
                colNames: ['IdSeccionCriterio', 'IdSubSeccionCriterio', 'IdCriterio', 'IdSeccion', 'Tipo', 'Posición', 'Nombre', 'Opcional', 'Cambio'],
                colModel: [
                            { name: 'IdSeccionCriterio', index: 'IdSeccionCriterio', sorttype: "string", hidden: true },
                            { name: 'IdSubSeccionCriterio', index: 'IdSubSeccionCriterio', sorttype: "string", hidden: true },
                            { name: 'IdCriterio', index: 'IdCriterio', sorttype: "string", hidden: true },
                            { name: 'IdSeccion', index: 'IdSeccion', sorttype: "string", hidden: true },
                            { name: 'Tipo', index: 'Tipo', sorttype: "string", hidden: true },
                            { name: 'PosicionInicial', index: 'PosicionInicial', sorttype: "string", width: 60, align: 'center' },
                            { name: 'Nombre', index: 'Nombre', sorttype: "string", width: 400 },
                            { name: 'Opcional', index: 'Opcional', sorttype: "boolean",
                                formatter: 'checkbox', width: 70, align: 'center'
                            },
   		                    { name: 'CambioPosicion', index: 'CambioPosicion', sorttype: "boolean",
   		                        formatter: 'checkbox', width: 70, align: 'center'
   		                    }
   	                      ],
                loadtext: 'Cargando datos...',
                emptyrecords: 'No hay resultados',
                height: 440,
                viewrecords: true,
                rownumbers: true,
                shrinkToFit: false,
                toolbar: [true, "top"],
                cmTemplate: { sortable: false },
                caption: "Listado"
            });

            jQuery("#t_gvTabla").append("<table><tr><td style='vertical-align: middle'><a id='lbnNuevo'>Nuevo</a> | <a id='lbnEditar'>Editar</a> | <a id='lbnEliminar'>Eliminar</a></td></tr></table>");

            jQuery("a", "#t_gvTabla").click(function (event) {
                var control = event.target.id;
                var idRow, rowGrid;
                var tabla = jQuery("#ddlTabla").val();

                if (tabla == "0")
                    return;

                if (control == 'lbnNuevo') {
                    nuevaSeccion();
                } else {
                    idRow = jQuery("#gvTabla").jqGrid('getGridParam', 'selrow');
                    if (!idRow) {
                        return alert('Por favor seleccione una fila');
                    }
                    rowGrid = jQuery("#gvTabla").jqGrid('getRowData', idRow);
                    if (control == 'lbnEditar') {
                        editarSeccion(rowGrid);
                    } else {
                        eliminarSeccion(rowGrid);
                    }
                }
            });

            function consultarSeccionCriterio() {

                IdSeccion = jQuery("#ihddIdSeccion").val();

                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = "IdSeccion";
                arreglo[0][1] = IdSeccion;
                parametro = fc_parametrosData(arreglo);

                jQuery.ajax({
                    url: location.pathname + "/ConsultarSeccionCriterio",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else
                            jQuery("#igvSeccionCriterio").trigger("reloadGrid");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function nuevaSeccion() {

                jQuery("#ihddTipoOperacion").attr("value", "1");
                jQuery("#idDialogSeccion").dialog("open");
            }

            function editarSeccion(rowGrid) {
                //--------------------------------------------------//
                jQuery("#ihddTipoOperacion").attr("value", "2");
                //--------------------------------------------------//
                jQuery("#ihddIdSeccion").attr("value", rowGrid.IdSeccion);
                jQuery("#itxtSeccionPosicionInicial").attr("value", rowGrid.PosicionInicial);
                jQuery("#itxtSeccionNombre").attr("value", rowGrid.Nombre);
                Opcional = rowGrid.Opcional.toLowerCase();
                CambioPosicion = rowGrid.CambioPosicion.toLowerCase();
                //--------------------------------------------------//
                if (Opcional == 'true' || Opcional == 'yes' || Opcional == 'si')
                    jQuery("#ichkSeccionOpcional").attr("checked", true);
                else
                    jQuery("#ichkSeccionOpcional").attr("checked", false);
                //--------------------------------------------------//
                if (CambioPosicion == 'true' || CambioPosicion == 'yes' || CambioPosicion == 'si')
                    jQuery("#ichkSeccionCambioPosicion").attr("checked", true);
                else
                    jQuery("#ichkSeccionCambioPosicion").attr("checked", false);
                //--------------------------------------------------//
                jQuery("#idDialogSeccion").dialog("open");
            }

            function eliminarSeccion(rowGrid) {

                IdSeccion = rowGrid.IdSeccion;
                TipoOperacion = '3';
                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(7);
                arreglo[0][0] = "TipoOperacion";
                arreglo[0][1] = TipoOperacion;
                arreglo[1][0] = "IdSeccion";
                arreglo[1][1] = IdSeccion;
                arreglo[2][0] = "Tipo";
                arreglo[2][1] = jQuery("#ddlTabla").val();
                arreglo[3][0] = "PosicionInicial";
                arreglo[3][1] = null;
                arreglo[4][0] = "Nombre";
                arreglo[4][1] = null;
                arreglo[5][0] = "Opcional";
                arreglo[5][1] = null;
                arreglo[6][0] = "CambioPosicion";
                arreglo[6][1] = null;
                parametro = fc_parametrosData(arreglo);

                if (!confirm('¿Seguro desea eliminar?'))
                    return;

                jQuery.ajax({
                    url: location.pathname + "/MantenimientoSeccion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else
                            jQuery("#ddlTabla").change();
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            function consultarCriterio(ComboCriterio) {

                ComboCriterio.append(jQuery("<option></option>").val('').html(''));

                jQuery.ajax({
                    url: location.pathname + "/ConsultarCriterios",
                    data: "{}",
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        jQuery.each(data.d, function () {
                            ComboCriterio.append(jQuery("<option></option>").val(this['IdSeccion']).html(this['Nombre']));
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            jQuery("#ddlTabla").change(function (event) {

                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(1);
                arreglo[0][0] = "Tabla";
                arreglo[0][1] = jQuery("#ddlTabla").val();
                parametro = fc_parametrosData(arreglo);


                if (jQuery("#ddlTabla").val() == "1") {
                    jQuery("#gvTabla").jqGrid('showCol', ["PosicionInicial", "Opcional", "CambioPosicion"]);
                    jQuery("#gvTabla").showCol('subgrid');
                }
                if (jQuery("#ddlTabla").val() == "2") {
                    jQuery("#gvTabla").jqGrid('hideCol', ["PosicionInicial", "Opcional", "CambioPosicion"]);
                    jQuery("#gvTabla").hideCol('subgrid');
                }

                jQuery("#gvTabla").jqGrid('resetSelection');

                jQuery.ajax({
                    url: location.pathname + "/ConsultarTablaImpresion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    complete: function (jsondata, stat) {
                        if (stat == "success")
                            jQuery("#gvTabla")[0].addJSONData(JSON.parse(jsondata.responseText).d);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            });

            jQuery("#ibtnSeccionGrabar").click(function (event) {

                TipoOperacion = jQuery("#ihddTipoOperacion").val();
                IdSeccion = jQuery("#ihddIdSeccion").val();
                PosicionInicial = jQuery("#itxtSeccionPosicionInicial").val();
                Nombre = jQuery("#itxtSeccionNombre").val();
                Tipo = jQuery("#ddlTabla").val();

                if (Tipo == "1") {
                    if (jQuery('input:checkbox[id=ichkSeccionOpcional]:checked').val())
                        Opcional = true;
                    else
                        Opcional = false;

                    if (jQuery('input:checkbox[id=ichkSeccionCambioPosicion]:checked').val())
                        CambioPosicion = true;
                    else
                        CambioPosicion = false;
                    //--------------------------------------------------------
                    if (PosicionInicial == "") {
                        jQuery("#itxtSeccionPosicionInicial").focus();
                        return alert("¡ Ingrese la Posicion Inicial ...");
                    }
                    if (isNaN(PosicionInicial)) {
                        jQuery("#itxtSeccionPosicionInicial").focus();
                        return alert("¡ Ingrese la Posicion Inicial ...");
                    }
                }

                if (Nombre == "") {
                    jQuery("#itxtSeccionNombre").focus();
                    return alert("¡ Ingrese la descripcion ...");
                }

                arreglo = parametro = null;
                arreglo = fc_redimencionarArray(7);
                arreglo[0][0] = "TipoOperacion";
                arreglo[0][1] = TipoOperacion;
                arreglo[1][0] = "IdSeccion";
                arreglo[1][1] = IdSeccion;
                arreglo[2][0] = "Tipo";
                arreglo[2][1] = Tipo;
                arreglo[3][0] = "PosicionInicial";
                arreglo[3][1] = PosicionInicial;
                arreglo[4][0] = "Nombre";
                arreglo[4][1] = Nombre;
                arreglo[5][0] = "Opcional";
                arreglo[5][1] = Opcional;
                arreglo[6][0] = "CambioPosicion";
                arreglo[6][1] = CambioPosicion;
                parametro = fc_parametrosData(arreglo);

                if (!confirm('¿Seguro desea grabar?'))
                    return;

                jQuery.ajax({
                    url: location.pathname + "/MantenimientoSeccion",
                    data: parametro,
                    dataType: "json",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        result = data.d;
                        if (!result.validacion)
                            alert(result.mensaje);
                        else {
                            jQuery("#ddlTabla").change();
                            jQuery("#ibtnSeccionCancelar").click();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            });

            jQuery("#ibtnSeccionCancelar").click(function (event) {
                jQuery("#idDialogSeccion").dialog("close");
            });

            jQuery("#idDialogSeccion").dialog({
                autoOpen: false,
                //                height: 390,
                width: 500,
                resizable: false,
                modal: true,
                close: function () {
                    jQuery("#ihddTipoOperacion").attr("value", '');
                    jQuery("#ihddIdSeccion").attr("value", '');
                    jQuery("#itxtSeccionPosicionInicial").attr("value", '');
                    jQuery("#itxtSeccionNombre").attr("value", '');
                    jQuery("#ichkSeccionOpcional").attr("checked", false);
                    jQuery("#ichkSeccionCambioPosicion").attr("checked", false);
                    jQuery("#idllSeccionCriterio").find('option').remove().end();
                    jQuery("#igvSeccionCriterio").clearGridData();
                },
                open: function () {
                    var Tabla = jQuery("#ddlTabla").val();
                    if (Tabla == "1") {
                        consultarCriterio(jQuery("#idllSeccionCriterio"));
                        consultarSeccionCriterio();
                        jQuery("#iTblSeccionCriterio").css("display", "block");
                        jQuery("#itrPosicionInicial").css("display", "block");
                        jQuery("#idDialogSeccion").css("height", "390");
                    }
                    if (Tabla == "2") {
                        jQuery("#iTblSeccionCriterio").css("display", "none");
                        jQuery("#itrPosicionInicial").css("display", "none");
                        jQuery("#idDialogSeccion").css("height", "120");
                    }
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frmAdmSecciones" action="#">
    <table width="100%">
        <tr>
            <td>
                <table class="ToolbarTituloRedondeado" width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td class="TextoTitulo">
                            Tablas de Impresion
                        </td>
                        <td align="right">
                            <div id="submenu">
                                <p>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" border="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <fieldset>
                                <legend style="text-align: left" class="TDTexto">DATOS DE CONSULTA</legend>
                                <table>
                                    <tr>
                                        <td align="left" style="width: 140px">
                                            <label class="Texto">
                                                Seleccione Tabla :</label>
                                        </td>
                                        <td align="left">
                                            <select id="ddlTabla" class="ArialFrm" style="width: 100px">
                                                <option value="0"></option>
                                                <option value="1">Secciones</option>
                                                <option value="2">Criterios</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend style="text-align: left" class="TDTexto">DETALLE DE CONSULTA</legend>
                                <table id="gvTabla">
                                </table>
                                <div id="pgTabla" />
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="idDialogSeccion" title="Registro de Seccion" style="display: none">
        <table width="100%">
            <tr>
                <td align="center" style="width: 100%">
                    <table width="100%">
                        <tr id="itrPosicionInicial">
                            <td align="left" class="Texto" style="width: 140px">
                                Posición Inicial
                            </td>
                            <td class="TDTexto" style="text-align: left">
                                <input id="itxtSeccionPosicionInicial" type="text" style="width: 50px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="Texto">
                                Nombre
                            </td>
                            <td class="TDTexto" style="text-align: left">
                                <input id="itxtSeccionNombre" type="text" style="width: 290px" />
                            </td>
                        </tr>
                    </table>
                    <table id="iTblSeccionCriterio" width="100%" style="text-align: left">
                        <tr>
                            <td align="left" class="Texto" style="width: 140px">
                            </td>
                            <td align="left" class="TDTexto">
                                <input id="ichkSeccionOpcional" type="checkbox" /><label for="ichkSeccionOpcional">Opcional</label>
                                <br />
                                <input id="ichkSeccionCambioPosicion" type="checkbox" /><label for="ichkSeccionCambioPosicion">Cambio
                                    Posición</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="Texto">
                                Seleccionar Criterio
                            </td>
                            <td class="TDTexto" align="left">
                                <select id="idllSeccionCriterio" style="width: 200px">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <table id="igvSeccionCriterio">
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr style="text-align: right; vertical-align: middle">
                <td align="right">
                    <a id="ibtnSeccionGrabar" title="Grabar" style="width: 90px; height: 28px">Grabar</a>
                </td>
                <td align="left" style="width: 90px">
                    <a id="ibtnSeccionCancelar" title="Cancelar" style="width: 90px; height: 28px">Cancelar</a>
                </td>
            </tr>
        </table>
    </div>
    <input id="ihddTipoOperacion" type="hidden" />
    <input id="ihddIdSeccion" type="hidden" />
    </form>
</asp:Content>
