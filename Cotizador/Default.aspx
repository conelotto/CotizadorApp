<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Cotizador._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inicio</title>
    <link href="Styles/StyleSheet.css" rel="Stylesheet" type="text/css" />
    <script src="Scripts/jcomunes.js" type="text/javascript"></script>
</head>
<body>
    <script language="javascript" type="text/javascript">

    function validarDatos() {

        var control = null;
        var validacion = document.getElementById("hddValidacion");
        
        validacion.value = "";
        control = document.getElementById("txtUsuario");

        if (fc_IsNullOrEmpty(control)) {
            control.focus();
            alert("Ingrese el Usuario");
            return;
        }
                
        control = document.getElementById("txtPassword");
        if (fc_IsNullOrEmpty(control)) {
            control.focus();
            alert("Ingrese la Contraseña");
            return;
        }

        validacion.value = "OK";
    }


        
    </script>
    <form id="frmLogin" runat="server">
    <div id="outer">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="vertical-align: top">
            <tr>
                <td style="height: 5px">
                </td>
            </tr>
            <tr>
                <td style="width: 15%" align="left" valign="top" colspan="2">
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/Logo Ferreyros.png" AlternateText="" />
                </td>
                <td style="width: 85%" align="right" valign="top">
                </td>
            </tr>
            <tr>
                <td style="height: 2px">
                </td>
            </tr>
            <tr style="width: 100%; height: 24px">
                <td colspan="3">
                    <table width="100%" style="height: 24px" cellpadding="0" cellspacing="0" border="0">
                        <tr style="height: 9px">
                            <td style="width: 100%; background-color: White;" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" valign="middle" colspan="3" style="background-image: url('Images/linea.PNG');
                    height: 1.9em; border-top: scrollbar 0.4em inset;">
                </td>
            </tr>
            <tr>
                <td style="height: 1px" colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" valign="middle">
                    <table width="1000px" style="height: 350px">
                        <tr>
                            <td style="width: 100px">
                            </td>
                            <td id="#m_Login" style="width: 800px; background-image: url('Images/Login.png');
                                height: 100px; background-repeat: no-repeat;" valign="top">
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td style="width: 80%">
                                        </td>
                                        <td style="width: 20%">
                                            <table border="0" cellspacing="1" style="width: 300px; background-color: Transparent;">
                                                <tr>
                                                    <td style="height: 20px; font-family: Verdana; font-size: larger; color: Black" align="center">
                                                        <b>Ingrese Usuario y Contraseña</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px" colspan="3">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <table cellpadding="3" cellspacing="2" border="0" style="background-color: Transparent;">
                                                            <tr>
                                                                <td align="right" class="Texto">
                                                                    Usuario :
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtUsuario" runat="server" MaxLength="20" CssClass="TextoControl"
                                                                        Width="150px" TabIndex="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 7px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" class="Texto">
                                                                    Contraseña :
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="TextoControl" MaxLength="30"
                                                                        Width="150px" TextMode="Password" TabIndex="2" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 7px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2">
                                                                    <asp:ImageButton ID="imgIngresar" runat="server" ImageUrl="~/Images/ingresar.gif"
                                                                        TabIndex="5" ToolTip="Ingresar Aplicación" OnClientClick="javascript:validarDatos()" />&nbsp;
                                                                    <asp:ImageButton ID="imgCerrar" runat="server" ImageUrl="~/Images/cerrar.gif" TabIndex="6"
                                                                        ToolTip="Cerrar Aplicación" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 5px" colspan="3">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label runat="server" Text="Usuario no registro." ID="txtMsg" CssClass="Texto"
                                                            ForeColor="Red" />
                                                            <asp:HiddenField ID="hddValidacion" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 100px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 5px" colspan="3">
                </td>
            </tr>
            <tr>
                <td style="height: 5px" colspan="3" align="center">
                    <div id="footer">
                        <asp:Label ID="idDerechos" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Image ID="imgCat" runat="server" ImageUrl="~/Images/cat.png" AlternateText=""
            Style="position: absolute; right: 20px; top: 24px;" />
    </div>
    </form>
</body>
</html>
