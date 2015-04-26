<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemUnlocker.aspx.cs" Inherits="LECTRIC.SC.Tools.ItemUnlocker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Item unlocker</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblUser" runat="server" Text="User: "></asp:Label>&nbsp;
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox><br />
        
        <asp:Label ID="lblNode" runat="server" Text="Node: "></asp:Label>&nbsp;
        <asp:TextBox ID="txtNode" runat="server">/sitecore/content</asp:TextBox><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
    </form>
</body>
</html>
