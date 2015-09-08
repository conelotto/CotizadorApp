<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmCargaNE.aspx.vb" Inherits="Cotizador.frmCargaNE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <link href="../../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function capturaValorParent() {
            var idCotizacion = parent.document.getElementById("hdfIdCotizacionVersion");
            var nombreArchivo = parent.document.getElementById("lklNombreArchivoActualizar");
            var valorIdCotizacion = idCotizacion.value;
            var valorNombreArchivo = nombreArchivo.innerText;
            $('#hdnNombreCotizacionVersion').val(valorNombreArchivo);
            $('#hdnIdCotizacionVersion').val(valorIdCotizacion);
        }
    </script>
</head>
<body onload="capturaValorParent()" style="background-color:#fcfcfc;">
    <form id="frmCarga" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:FileUpload ID="Archivo" runat="server" class="btn btn-default btn-sm"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCargar" runat="server" class="btn btn-default btn-sm" Text="Cargar" OnClick="btnCargar_Click" style="background-color:#428bca; color:White"/>
                    <asp:Label ID="Label1" runat="server" Text="" ></asp:Label>
                    <asp:HiddenField ID="hdnNombreCotizacionVersion" runat="server" />
                    <asp:HiddenField ID="hdnIdCotizacionVersion" runat="server" />
                    <asp:HiddenField ID="hdnEstado" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
