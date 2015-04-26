<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register TagPrefix="dgg" Namespace="MyCustomColumn" Assembly="MyCustomColumn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Using DataGrid and ADO.NET to Add, Edit and Delete from Access Database in ASP.NET 2.0 and VB.NET</title>
   <style type="text/css">
<!--
body {margin-left: 0px;	margin-top: 0px;margin-right: 0px;margin-bottom: 0px;}
a:link {color: #0000FF;}
a:visited {color: #0000FF;}
a:hover {color: #0000FF;text-decoration: none;}
a:active {color: #0000FF;}
.basix {font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 11px;}
.header1 {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 11px;font-weight: bold;color: #006699;}
.lgHeader1 {font-family: Arial, Helvetica, sans-serif;font-size: 18px;font-weight: bold;color: #0066CC;background-color: #CEE9FF;}
-->
</style> 
</head>
<body>
<br />
<div align="left" style="text-align: center">
    <form id="form1" runat="server">
    <div>
    <hr />

<fieldset style="width: 804px" align="center">
   
<legend>Using DataGrid and ADO.NET to Add, Edit and Delete from SQL Database</legend>

<asp:DataGrid id="dgContacts" AutoGenerateColumns="False" AllowPaging="True"
  DataKeyField="ContactID" ForeColor="#333333" GridLines="None" 
  PageSize="5" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
  onPageIndexChanged="dgContacts_PageIndexChanged" cellpadding="4" Runat="Server" ShowFooter="true"
  OnDeleteCommand="dgContacts_DeleteCommand"
  OnEditCommand="dgContacts_EditCommand"
  OnCancelCommand="dgContacts_CancelCommand"
  OnUpdateCommand="dgContacts_UpdateCommand"
  OnItemCommand="dgContacts_InsertCommand">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditItemStyle BackColor="#2461BF" />
    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" 
            NextPageText="Next &amp;gt;" PrevPageText="&amp;lt; Previous" />
    <AlternatingItemStyle BackColor="White" />
    <ItemStyle BackColor="#EFF3FB" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <Columns>
        <asp:TemplateColumn HeaderText="Contact ID">
            <FooterTemplate>
                <asp:Label ID="add_ID" Runat="Server" />
            </FooterTemplate>
            <ItemTemplate>
                <%#Container.DataItem("ContactID")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="ContactID" Text='<%# Container.DataItem("ContactID") %>' Runat="server" />
            </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="First Name">
            <FooterTemplate>
                <asp:TextBox ID="add_FirstName" Columns="10" Runat="Server" />
            </FooterTemplate>
            <ItemTemplate>
                <%#Container.DataItem("FirstName")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="FirstName" Columns="10" 
                    Text='<%# Container.DataItem("FirstName") %>' Runat="server" />
            </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Last Name">
            <FooterTemplate>
                <asp:TextBox ID="add_LastName" Columns="10" Runat="Server" />
            </FooterTemplate>
            <ItemTemplate>
                <%#Container.DataItem("LastName")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="LastName" Columns="10" 
                    Text='<%# Container.DataItem("LastName") %>' Runat="server" />
            </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Location">
            <FooterTemplate>
                <asp:TextBox ID="add_Location" Columns="10" Runat="Server" />
            </FooterTemplate>
            <ItemTemplate>
                <%#Container.DataItem("Location")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlLocations"/>
                <%--<dgg:DropDownColumn ID="ddlLocations" DataField="LocationID" DataTextField="Location" DataValueField="Location" HeaderText="Location" />--%>
            </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Phone Number">
            <FooterTemplate>
                <asp:TextBox ID="add_PhoneNumber" Columns="15" Runat="Server" />
            </FooterTemplate>
            <ItemTemplate>
                <%#Container.DataItem("PhoneNumber")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="PhoneNumber" Columns="15" 
                    Text='<%# Container.DataItem("PhoneNumber") %>' Runat="server" />
            </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel" EditText="Edit" HeaderText="Edit"></asp:EditCommandColumn>
        <asp:TemplateColumn HeaderText="Delete">
            <FooterTemplate>
                <asp:Button CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" />
            </FooterTemplate>
            <ItemTemplate>
                <asp:Button CommandName="Delete" Text="Delete" ID="btnDel" Runat="server" />
            </ItemTemplate>
        </asp:TemplateColumn>
            
    </Columns>
</asp:DataGrid>

<br />
<br />
<asp:DropDownList ID="ddlState" runat="server" DataSourceID="xmlDataStates" DataTextField="name" DataValueField="code"></asp:DropDownList>
<br />
<br />
<asp:DropDownList ID="ddlRelationship" runat="server" DataSourceID="XmlBeneRelationshipData" DataTextField="desc" DataValueField="code"></asp:DropDownList>
 
</fieldset>    

    </form>
   </div>
<asp:XmlDataSource ID="xmlDataStates" runat="server" DataFile="~/App_Data/States.xml"></asp:XmlDataSource>
<asp:XmlDataSource ID="XmlBeneRelationshipData" runat="server" DataFile="~/App_Data/BeneRelationship.xml"></asp:XmlDataSource>

</body>
</html>
