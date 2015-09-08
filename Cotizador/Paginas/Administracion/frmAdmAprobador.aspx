<%@ Page Title="Aprobador" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="frmAdmAprobador.aspx.vb" Inherits="Cotizador.frmAdmAprobador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--JQuery----------------------------------------------------------------------------------%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.layout.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/i18n/grid.locale-es.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/ui.multiselect.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.jqGrid.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.tablednd.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.contextmenu.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.alphaNumeric.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/jcomunes.js" type="text/javascript"></script>
    <%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jQuery-1.8.0/ui.multiselect.css" rel="stylesheet" type="text/css" />
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script type="text/javascript">
        jQuery(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        jQuery(document).ready(function () {

            //que se conviertan en botones
            jQuery("#btnGrabar").button();
            jQuery("#btnCancelar").button();
            //style de los controles
            jQuery("#btnGrabar").css("width", "90");
            jQuery("#btnGrabar").css("height", "28");
            jQuery("#btnCancelar").css("width", "90");
            jQuery("#btnCancelar").css("height", "28");
            jQuery("#ddlCorporacion").css("width", "175");
            $("#ddlCompanhia").css("width", "175");
            $("#txtAprobador").css("width", "410");
            $("#iddlCorporacion").css("width", "200");
            $("#iddlCompanhia").css("width", "200");
            $("#itxtDescripcion").css("width", "198");
            $("#itxtUsuario").css("width", "198");
            $("#itxtUsuario").css("text-transform", "uppercase");

            //solo letras 
            $('#itxtUsuario').alpha();

            $.ajax({
                type: "POST",
                url: location.pathname + "/ListarCombos",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var ls = data.d;
                    $.each(ls[0], function () {
                        $("#ddlCorporacion").append($("<option></option>").val(this['Codigo']).html(this['Descripcion']));
                        $("#iddlCorporacion").append($("<option></option>").val(this['Codigo']).html(this['Descripcion']));
                    });
                    $.each(ls[1], function () {
                        $("#ddlCompanhia").append($("<option></option>").val(this['Codigo']).html(this['Descripcion']));
                        $("#iddlCompanhia").append($("<option></option>").val(this['Codigo']).html(this['Descripcion']));
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                }
            });

            function ocultarControlModal(val) {

                if (!val) {
                    $('#itxtDescripcion').attr('readonly', false);
                    $("#iddlCorporacion").attr('disabled', false);
                    $("#iddlCompanhia").attr('disabled', false);
                    $('#idbotonModal').css('display', 'block');
                    $('#idControlUsuario').css('display', 'block');
                    $("#btnGrabar").css('display', 'block');
                    $("#t_gvUsuario").css('display', 'block');
                } else {
                    $('#itxtDescripcion').attr('readonly', true);
                    $("#iddlCorporacion").attr('disabled', true);
                    $("#iddlCompanhia").attr('disabled', true);
                    $('#idbotonModal').css('display', 'none');
                    $('#idControlUsuario').css('display', 'none');
                    $("#btnGrabar").css('display', 'none');
                    $("#t_gvUsuario").css('display', 'none');
                }

            }

            function eliminarAprobador() {

                var usuario = '<%= HttpContext.Current.Session("idUsuario").toString() %>';
                var id = jQuery("#gvAprobador").jqGrid('getGridParam', 'selrow');

                if (!(id)) {
                    alert("Por favor seleccione una fila");
                } else {

                    if (!(confirm('¿Seguro desea eliminar el aprobador?'))) {
                        return;
                    }

                    var ret = jQuery("#gvAprobador").jqGrid('getRowData', id);
                    var arreglo = null;
                    var parametro = null;
                    arreglo = fc_redimencionarArray(2);
                    arreglo[0][0] = 'idAprobador';
                    arreglo[0][1] = ret.IdAprobador;
                    arreglo[1][0] = 'usuario';
                    arreglo[1][1] = usuario;
                    parametro = fc_parametrosData(arreglo);

                    $.ajax({
                        type: "POST",
                        url: location.pathname + "/EliminarAprobador",
                        data: parametro,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var result = data.d;
                            if (!result.validacion) {
                                alert(result.mensaje);
                            } else {
                                consultarAprobador();
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });
                }
            }

            function visualizar(id) {

                if (!id) {
                    alert('Por favor seleccione una fila');
                } else {
                    var ret = jQuery("#gvAprobador").jqGrid('getRowData', id);
                    $('#itxtDescripcion')[0].value = ret.Aprobador;
                    $("#iddlCorporacion").val(ret.IdCorporacion);
                    $("#iddlCompanhia").val(ret.IdCompañia);
                    $("#ihddTipo").attr('value', 'C');
                    $("#ihdnIdAprobador").attr('value', ret.IdAprobador);
                    $('#itxtUsuario')[0].value = '';
                    jQuery("#gvUsuario").clearGridData();
                    ocultarControlModal(true);

                    var arreglo = null;
                    var parametro = null;
                    arreglo = fc_redimencionarArray(1);
                    arreglo[0][0] = 'idAprobador';
                    arreglo[0][1] = $("#ihdnIdAprobador").get(0).value;
                    parametro = fc_parametrosData(arreglo);

                    $.ajax({
                        type: "POST",
                        url: location.pathname + "/ConsultarAprobadorUsuario",
                        data: parametro,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var lista = data.d;
                            for (var j = 0; j < lista.length; j++) {
                                var mydata = [
                                { MatriculaUsuario: lista[j].MatriculaUsuario,
                                    NombreUsuario: lista[j].NombreUsuario,
                                    Correo: lista[j].CorreoUsuario
                                }];
                                jQuery("#gvUsuario").jqGrid('addRowData', j + 1, mydata[0]);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });

                    $("#idDialogForm").dialog("open");
                }
            }

            function Grabar() {

                var usuario = '<%= HttpContext.Current.Session("idUsuario").toString() %>';
                var tipo = $("#ihddTipo").get(0).value;
                var idAprobador = $("#ihdnIdAprobador").get(0).value;
                var corporacion = $("#iddlCorporacion").val();
                var companhia = $("#iddlCompanhia").val();
                var aprobador = $('#itxtDescripcion')[0].value;
                var parametros = null;
                var arreglo = null;

                if (!(tipo == 'N' || tipo == 'E')) {
                    alert('El tipo de operación es incorrecto');
                    return;
                }

                if (corporacion == '') {
                    alert('Seleccione una corporación');
                    return;
                }
                if (companhia == '') {
                    alert('Seleccione una compañia');
                    return;
                }
                if (aprobador == '') {
                    alert('Ingrese el aprobador');
                    return;
                }

                var filas = jQuery('#gvUsuario').jqGrid('getGridParam', 'data');
                var lsUsuario = fc_cadenaContenidoGrilla(filas);

                if (!(confirm('¿Seguro desea grabar?'))) {
                    return;
                }

                arreglo = fc_redimencionarArray(7)
                arreglo[0][0] = 'tipo';
                arreglo[0][1] = tipo;
                arreglo[1][0] = 'idAprobador';
                arreglo[1][1] = idAprobador;
                arreglo[2][0] = 'corporacion';
                arreglo[2][1] = corporacion;
                arreglo[3][0] = 'companhia';
                arreglo[3][1] = companhia;
                arreglo[4][0] = 'aprobador';
                arreglo[4][1] = aprobador;
                arreglo[5][0] = 'usuario';
                arreglo[5][1] = usuario;
                arreglo[6][0] = 'lsUsuario';
                arreglo[6][1] = lsUsuario;
                parametros = fc_parametrosData(arreglo);

                $.ajax({
                    type: "POST",
                    url: location.pathname + "/Grabar",
                    data: parametros,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        if (!result.validacion) {
                            alert(result.mensaje);
                        } else {
                            consultarAprobador();
                            $("#btnCancelar").click();
                            alert('Se grabo correctamente');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });

            }

            function pasarGridUsuarios(dato) {
                if (dato) {
                    var band = true;
                    var filas = jQuery('#gvUsuario').jqGrid('getGridParam', 'data');
                    var i = jQuery('#gvUsuario').jqGrid('getGridParam', 'records');
                    var arr = dato.split(" | ");
                    if (arr) {
                        for (var j = 0; j < filas.length; j++) {
                            if (filas[j].MatriculaUsuario == arr[0]) {
                                band = false;
                                alert('el usuario seleccionado ya esta ingresado');
                                break;
                            }
                        }
                        if (band) {
                            var arreglo = null;
                            var parametro = null;
                            arreglo = fc_redimencionarArray(2);
                            arreglo[0][0] = 'matriculaUsuario';
                            arreglo[0][1] = arr[0];
                            arreglo[1][0] = 'nombreUsuario';
                            arreglo[1][1] = arr[1];
                            parametro = fc_parametrosData(arreglo)
                            $.ajax({
                                type: "POST",
                                url: location.pathname + "/AgregarUsuario",
                                data: parametro,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    var user = data.d;
                                    var mydata = [{
                                        MatriculaUsuario: user.MatriculaUsuario,
                                        NombreUsuario: user.NombreUsuario,
                                        Correo: user.CorreoUsuario
                                    }];
                                    jQuery("#gvUsuario").jqGrid('addRowData', i + 1, mydata[0]);
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert(textStatus + ': ' + XMLHttpRequest.responseText);
                                }
                            });
                        }
                    }
                }
            }

            function nuevoAprobador() {
                $("#iddlCorporacion").val('');
                $("#iddlCompanhia").val('');
                $('#itxtDescripcion')[0].value = '';
                $('#itxtUsuario')[0].value = '';
                $("#ihddTipo").attr('value', 'N');
                $("#ihdnIdAprobador").attr('value', '');
                jQuery("#gvUsuario").clearGridData();
                ocultarControlModal(false);
                $("#idDialogForm").dialog("open");
            }

            function consultarAprobador() {

                var corporacion = $("#ddlCorporacion");
                var companhia = $("#ddlCompanhia");
                var aprobador = $("#txtAprobador");
                var arreglo = null;
                var parametro = null;
                arreglo = fc_redimencionarArray(3);
                arreglo[0][0] = 'corporacion';
                arreglo[0][1] = corporacion.val();
                arreglo[1][0] = 'companhia';
                arreglo[1][1] = companhia.val();
                arreglo[2][0] = 'aprobador';
                arreglo[2][1] = aprobador.val();
                parametro = fc_parametrosData(arreglo);



                $.ajax({
                    type: "POST",
                    url: location.pathname + "/Consultar",
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: cargarGrid,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus + ': ' + XMLHttpRequest.responseText);
                    }
                });
            }

            function cargarGrid(response) {
                jQuery("#gvAprobador").clearGridData();
                var lista = response.d;
                if (lista.length == 0) {
                } else {
                    for (var i = 0; i < lista.length; i++) {
                        var mydata = [
                                { IdAprobador: lista[i].IdAprobador,
                                    IdCorporacion: lista[i].IdCorporacion,
                                    IdCompañia: lista[i].IdCompañia,
                                    Aprobador: lista[i].Aprobador,
                                    Estado: lista[i].Estado
                                }
                            ];
                        jQuery("#gvAprobador").jqGrid('addRowData', i + 1, mydata[0]);
                    }
                }
            }

            function editarAprobador() {
                var id = jQuery("#gvAprobador").jqGrid('getGridParam', 'selrow');
                if (!id) {
                    alert("Por favor seleccione una fila");
                } else {
                    var arreglo = null;
                    var parametro = null;
                    var ret = jQuery("#gvAprobador").jqGrid('getRowData', id);
                    $('#itxtDescripcion')[0].value = ret.Aprobador;
                    $("#iddlCorporacion").val(ret.IdCorporacion);
                    $("#iddlCompanhia").val(ret.IdCompañia);
                    $("#ihddTipo").attr('value', 'E');
                    $("#ihdnIdAprobador").attr('value', ret.IdAprobador);
                    $('#itxtUsuario')[0].value = '';
                    jQuery("#gvUsuario").clearGridData();
                    ocultarControlModal(false);

                    arreglo = fc_redimencionarArray(1);
                    arreglo[0][0] = 'idAprobador';
                    arreglo[0][1] = $("#ihdnIdAprobador").get(0).value;
                    parametro = fc_parametrosData(arreglo);

                    $.ajax({
                        type: "POST",
                        url: location.pathname + "/ConsultarAprobadorUsuario",
                        data: parametro,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var lista = data.d;
                            for (var j = 0; j < lista.length; j++) {
                                var mydata = [
                                { MatriculaUsuario: lista[j].MatriculaUsuario,
                                    NombreUsuario: lista[j].NombreUsuario,
                                    Correo: lista[j].CorreoUsuario
                                }];
                                jQuery("#gvUsuario").jqGrid('addRowData', j + 1, mydata[0]);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });

                    $("#idDialogForm").dialog("open");
                }
            }

            function eliminarUsuario() {
                var id = jQuery("#gvUsuario").jqGrid('getGridParam', 'selrow');
                if (!id) {
                    alert('Por favor seleccione una fila');
                } else {
                    var ret = jQuery("#gvUsuario").jqGrid('getRowData', id);
                    var filas = jQuery('#gvUsuario').jqGrid('getGridParam', 'data');
                    var i = 1;
                    jQuery("#gvUsuario").clearGridData();
                    for (var j = 0; j < filas.length; j++) {
                        if (filas[j].MatriculaUsuario != ret.MatriculaUsuario) {
                            var mydata = [
                                { MatriculaUsuario: filas[j].MatriculaUsuario,
                                    NombreUsuario: filas[j].NombreUsuario,
                                    Correo: filas[j].Correo
                                }
                            ];
                            jQuery("#gvUsuario").jqGrid('addRowData', i, mydata[0]);
                            i = i + 1;
                        }
                    }
                }
            }

            $("#btnGrabar").click(function () {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    alert('La ventana es solo de lectura');
                } else {
                    Grabar();
                }
            });

            $("#btnCancelar").click(function () {
                $("#idDialogForm").dialog("close");
            });

            $("#itxtUsuario").autocomplete({
                source: function (request, response) {
                    var arreglo = null;
                    var parametro = null
                    arreglo = fc_redimencionarArray(2);
                    arreglo[0][0] = 'companhia';
                    arreglo[0][1] = $("#iddlCompanhia").val();
                    arreglo[1][0] = 'prefijoUsuario';
                    arreglo[1][1] = request.term;
                    parametro = fc_parametrosData(arreglo);
                    $.ajax({
                        type: "POST",
                        url: location.pathname + "/MostrarUsuario",
                        data: parametro,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.MatriculaUsuario + ' | ' + item.NombreUsuario,
                                    value: item.NombreUsuario
                                }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    pasarGridUsuarios(ui.item ? ui.item.label : "");
                },
                search: function (event, ui) {
                    var companhia = $("#iddlCompanhia").val();
                    if (companhia == '') {
                        alert('Seleccione una compañia');
                        return false;
                    }
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

            $("#idDialogForm").dialog({
                autoOpen: false,
                height: 510,
                width: 670,
                resizable: false,
                modal: true
            });

            /***************** APROBADOR *******************************************************************************/
            jQuery("#gvAprobador").jqGrid({
                datatype: "local",
                height: 250,
                colNames: ['IdAprobador', 'IdCorporacion', 'IdCompañia', 'Descripción', 'Estado'],
                colModel: [
                        { name: 'IdAprobador', index: 'IdAprobador', sorttype: "string", hidden: true },
                        { name: 'IdCorporacion', index: 'IdCorporacion', sorttype: "string", hidden: true },
                        { name: 'IdCompañia', index: 'IdCompañia', sorttype: "string", hidden: true },
                        { name: 'Aprobador', index: 'Aprobador', sorttype: "string", width: 600, editable: true, editoptions: { size: 50} },
   		                { name: 'Estado', index: 'Estado', sorttype: "string", hidden: true }
   	                    ],
                rowNum: 100,
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                ondblClickRow: function (rowId) { visualizar(rowId); },
                caption: "Listado de Aprobadores"
            });
            /* ----------------------------------------------------------------------------------------------------------- */
            $("#t_gvAprobador").append("<table><tr><td style='vertical-align: middle'><a id='lbnNuevo'>Nuevo</a> | <a id='lbnEditar'>Editar</a> | <a id='lbnEliminar'>Eliminar</a></td></tr></table>");
            $("a", "#t_gvAprobador").click(function (event) {
                var control = event.target.id;
                switch (control) {
                    case 'lbnNuevo':
                        nuevoAprobador();
                        break;
                    case 'lbnEditar':
                        editarAprobador();
                        break;
                    case 'lbnEliminar':
                        eliminarAprobador();
                        break;
                }
            });
            /***************** USUARIO *******************************************************************************/
            jQuery("#gvUsuario").jqGrid({
                datatype: "local",
                colNames: ['Matricula', 'Nombre', 'Correo'],
                colModel: [
                        { name: 'MatriculaUsuario', index: 'MatriculaUsuario', align: "center", sorttype: "string", width: 60 },
                        { name: 'NombreUsuario', index: 'NombreUsuario', sorttype: "string", width: 300 },
                        { name: 'Correo', index: 'Correo', sorttype: "string", width: 160 }
   	                    ],
                rowNum: 10,
                rowList: [10, 20, 30],
                rownumbers: true,
                viewrecords: true,
                toolbar: [true, "top"],
                sortorder: "desc",
                caption: "Listado de Usuarios"
            });

            $("#t_gvUsuario").append("<table><tr><td style='vertical-align: middle'><a id='lbnLimpiar'>Limpiar</a> | <a id='lbnEliminar'>Eliminar</a></td></tr></table>");
            $("a", "#t_gvUsuario").click(function (event) {
                var tipo = $("#ihddTipo").get(0).value;
                if (!(tipo == 'N' || tipo == 'E')) {
                    return;
                }
                var control = event.target.id;
                switch (control) {
                    case 'lbnLimpiar':
                        $('#itxtUsuario')[0].value = '';
                        jQuery("#gvUsuario").clearGridData();
                        break;
                    case 'lbnEliminar':
                        eliminarUsuario();
                        break;
                }
            });

            /***************** BOTONES *******************************************************************************/

            $("#lbnLimpiar").click(function () {
                $("#ddlCorporacion").val('');
                $("#ddlCompanhia").val('');
                $('#txtAprobador')[0].value = '';
                jQuery("#gvAprobador").clearGridData();
            });

            $("#lbnConsultar").click(function (evento) {
                consultarAprobador();
            });


            $("#lbnExportar").click(function (evento) {
                var i = $("#gvAprobador").getGridParam("reccount");
                if (i == 0) {
                    alert('No existe registros para exportar');
                } else {
                    $.ajax({
                        type: "POST",
                        url: location.pathname + "/Exportar",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var val = data.d;
                            if (!val) {
                                alert('No se pudo exportar');
                            } else {
                                window.open('../Publico/frmReporte.aspx', '', 'height=200,width=400,status=no,toolbar=no,menubar=no,location=no');
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus + ': ' + XMLHttpRequest.responseText);
                        }
                    });
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="frmAdmAprobador" action="#">
    <table width="100%" cellpadding="0" border="0" cellspacing="0" class="ToolbarTitulo">
        <tr valign="middle">
            <td align="left">
                <label class="TextoTitulo">
                    Aprobador
                </label>
            </td>
            <td align="right">
                <div id="submenu">
                    <p>
                        <a id="lbnConsultar">Consultar</a> | 
                        <a id="lbnLimpiar">Limpiar</a> | 
                        <a id="lbnExportar">Exportar</a>
                    </p>
                </div>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left" style="width: 100%">
                <fieldset>
                    <legend style="text-align: left" class="TDTexto">DATOS DE CONSULTA</legend>
                    <table>
                        <tr>
                            <td align="left">
                                <label class="Texto">
                                    Corporación :</label>
                            </td>
                            <td>
                                <select id="ddlCorporacion" class="ArialFrm">
                                </select>
                            </td>
                            <td align="left">
                                <label class="Texto">
                                    Compañia :</label>
                            </td>
                            <td>
                                <select id="ddlCompanhia" class="ArialFrm">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <label class="Texto">
                                    Descripción :</label>
                            </td>
                            <td colspan="3">
                                <input id="txtAprobador" type="text" class="Texto" maxlength="50" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <fieldset>
                    <legend style="text-align: left" class="TDTexto">DETALLE DE CONSULTA</legend>
                    <table id="gvAprobador">
                    </table>
                    <div id="pager" />
                </fieldset>
            </td>
        </tr>
    </table>
    <div id="idDialogForm" title="Registro de Aprobador" style="display: none">
        <input id="ihddTipo" type="hidden" />
        <input id="ihdnIdAprobador" type="hidden" />
        <fieldset style="width: 600px">
            <legend style="text-align: left" class="TDTexto">Datos Generales</legend>
            <table width="600px">
                <tr>
                    <td align="left" class="Texto" style="width: 150px">
                        Corporación :
                    </td>
                    <td align="left" style="width: 450px">
                        <select id="iddlCorporacion" class="ArialFrm">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Compañia :
                    </td>
                    <td align="left">
                        <select id="iddlCompanhia" class="ArialFrm">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Texto">
                        Descripción :
                    </td>
                    <td align="left">
                        <input id="itxtDescripcion" type="text" class="Texto" maxlength="50" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="width: 600px">
            <legend style="text-align: left" class="TDTexto">Usuarios</legend>
            <table width="600px">
                <tr id="idControlUsuario">
                    <td align="left" class="Texto" style="width: 150px">
                        Usuario :
                    </td>
                    <td align="left" style="width: 450px">
                        <input id="itxtUsuario" type="text" class="Texto" maxlength="20" />
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <table id="gvUsuario">
                        </table>
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
    </form>
</asp:Content>
