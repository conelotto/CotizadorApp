<%@ Page Title="Página principal" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="Default.aspx.vb" Inherits="Pruebas._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<table><tr><td style='vertical-aling: middle'> 
<div id='divMantenimiento' style ='float:left; width:140px'> <a id='lblNuevo'>Nuevo</a> | <a id='lblEditar'>Editar</a>| <a id='lblELiminar'>Eliminar</a></div>
<div id='divMarcador' style ='float:left; width:80px'> | <a id='lblMarcadores'>Marcadores</a></div>
<div id ='divBusqueda'> | <a id='lblBuscar'>Buscar</a>| <a id='lblListar'>Mostar todos</a></div>
</td></tr></table>
    <input id="Button1" type="button" value="button" style ="display:inline-block " />
    <h2>
        ASP.NET
    </h2>
    <p>
        Para obtener más información acerca de ASP.NET, visite <a href="http://www.asp.net" title="Sitio web de ASP.NET">www.asp.net</a>.
    </p>
    <div style ='width:100%'>
    </div>
    <p>
        También puede encontrar <a href="http://go.microsoft.com/fwlink/?LinkID=152368"
            title="Documentación de ASP.NET en MSDN">documentación sobre ASP.NET en MSDN</a>.
        <asp:Button ID="btnPrueba" runat="server" Text="Prueba" />
        <asp:Button ID="btnPrueba2" runat="server" Text="Prueba2" />
        <asp:Button ID="btnPrueba3" runat="server" Text="Prueba 3" />
        <asp:Button ID="btnPrueba4" runat="server" Text="Prueba 4" />
    </p>
    
    <script type="text/javascript" >
        

</script>
</asp:Content>
 
