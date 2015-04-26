<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailExport.aspx.cs" Inherits="Siteworx.Web.layouts.debug.EmailExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <style>
        table{border: solid 1px black;}
        td{width: 300px; border: solid 1px black;}
    </style>
    <a href="csvUsers.aspx">Export Table as CSV file</a><p>&nbsp;</p>
    <asp:PlaceHolder ID="emails" runat="server"></asp:PlaceHolder>
</body>
</html>
