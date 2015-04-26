<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearEdgecast.aspx.cs" Inherits="Website.scripts.ClearEdgecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>        
        <p><asp:Literal runat="server" ID="StatusLit" /></p>
        <p>http://www.westwood.edu/<asp:TextBox ID="Path" runat="server" /> <asp:Button runat="server" ID="SubmitBtn" Text="Submit" OnClick="DoPurge" /></p>
    </div>
    </form>
</body>
</html>
