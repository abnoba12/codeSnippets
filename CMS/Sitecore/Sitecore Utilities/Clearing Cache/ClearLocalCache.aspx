﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearLocalCache.aspx.cs" Inherits="Website.TCWAdmin.ClearLocalCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Clear Sitecore cache</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>
        <asp:Label ID="lblIntroText" runat="server">Please click the button below to clear the cache.</asp:Label>
    </p>
    <asp:Button ID="btnSubmit" runat="server" Text="Clear Sitecore Cache" onclick="btnSubmit_Click" />
    <br />
    <asp:Label ID="lblResponse" runat="server" />
    
    </div>
        <asp:TextBox ID="pass" runat="server"></asp:TextBox>
    </form>
</body>
</html>
