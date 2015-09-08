<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="Cotizador.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <input id="Button2" type="button" value="button" />
        <img src="../../Images/ajax-loader.gif" style ="padding-top:15px" />
        <asp:Button ID="Button3" runat="server" Text="probar Session" />
        <h1 style ="padding-bottom:15px">Por favor, espere</h1>
        <p style ="padding-bottom:15px">
            <asp:Label ID="lblId" runat="server" Font-Bold="True" Font-Size="X-Large" 
                Text="Label"></asp:Label><select id="Select1"><option></option></select>
                <input id="hdfCantidaLllaves" type="hidden"  value="0"/>
                <span id ="nombreSpanLlave"></span>
        </p>
    
    </div>
    </form>
</body>
</html>
