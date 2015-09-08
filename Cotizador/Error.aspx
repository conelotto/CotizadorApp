<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Error.aspx.vb" Inherits="Cotizador._Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error en Aplicación</title>
</head>
<body>
    <form id="frmError" runat="server">
    <div>
        <table cellspacing="1" cellpadding="1" border="0" width="100%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Font-Size="16pt" Font-Bold="True" 
                        Width="294px" Text="Error al Procesar la Información" />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <strong><font size="4">Informe del Error :</font></strong>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <strong><font size="2">Error:</font></strong>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" Width="98%" Font-Size="8pt" Height="94px"
                        Font-Names="Tahoma"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <strong><font size="2">Origen:</font></strong>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSource" runat="server" Font-Size="8pt" Width="98%" Font-Names="Tahoma"
                        Height="25px"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
