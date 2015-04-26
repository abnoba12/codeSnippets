<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SetGlobals.aspx.vb" Inherits="SetGlobals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Globals</title>
    <style type="text/css">
		body, tr {font-family: Arial, Helvetica, sans-serif; font-size: 12px;}
		tr.rowHead {color: #FFF; background-color: #07356C; font-weight: bold;}
		tr.rowAlt {background-color: #d9e0ee;}
    </style>
</head>
<body>
    <form id="frmGlobals" runat="server">
	<table border="0" cellpadding="10" cellspacing="0">
		<tr valign="top">
			<td width="50%">
				<b>Application Variables: <asp:Label ID="lblAppItems" runat="server" Text="XX" /> Item(s)</b><br />
				<asp:Repeater ID="rptApplication" runat="server">
					<HeaderTemplate>
						<table border="1" cellspacing="0" cellpadding="2">
							<tr class="rowHead">
								<td><b>Item</b></td>
								<td><b>Value</b></td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr>
								<td><%#DataBinder.Eval(Container, "DataItem.Key")%></td>
								<td><%#DataBinder.Eval(Container, "DataItem.Value")%></td>
							</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
							<tr class="rowAlt">
								<td><%#DataBinder.Eval(Container, "DataItem.Key")%></td>
								<td><%#DataBinder.Eval(Container, "DataItem.Value")%></td>
							</tr>
					</AlternatingItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Button ID="btnSetGlobals" runat="server" Text="Reset Variables" Width="150px" />
			</td>
			<td width="50%">
    			<b>Session Variables: <asp:Label ID="lblSessItems" runat="server" Text="XX" /> Item(s)</b><br />
				<asp:Repeater ID="rptSession" runat="server">
					<HeaderTemplate>
						<table border="1" cellspacing="0" cellpadding="2">
							<tr class="rowHead">
								<td><b>Item</b></td>
								<td><b>Value</b></td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr>
								<td><%#DataBinder.Eval(Container, "DataItem.Key")%></td>
								<td><%#DataBinder.Eval(Container, "DataItem.Value")%></td>
							</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
							<tr class="rowAlt">
								<td><%#DataBinder.Eval(Container, "DataItem.Key")%></td>
								<td><%#DataBinder.Eval(Container, "DataItem.Value")%></td>
							</tr>
					</AlternatingItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Button ID="btnResetSession" runat="server" Text="Reset Session" Width="150px" />
			</td>
		</tr>
	</table>
    </form>
</body>
</html>
