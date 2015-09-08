<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmErrorArchivoFTP.aspx.vb" Inherits="Cotizador.frmErrorArchivoFTP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
  
<html xmlns="http://www.w3.org/1999/xhtml">


   

<head runat="server">
<%--Style--------------------------------------------------------------------------------------%>
    <link href="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Principal.css" rel="stylesheet" type="text/css" /> 
    <%--JavaScript--------------------------------------------------------------------------------------%>
    <script src="../../Scripts/jQuery-1.8.0/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQuery-1.8.0/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <script type="text/javascript" language ="javascript">
        $(document).ready(function () {
                    
            alert('Archivo no disponible...');
            window.open('', '_parent', '');
            window.close();
        });          
    </script>
</body>
</html>
