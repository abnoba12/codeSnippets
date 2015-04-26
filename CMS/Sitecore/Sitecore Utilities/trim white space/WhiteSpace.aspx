<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhiteSpace.aspx.cs" Inherits="Siteworx.Web.layouts.debug.WhiteSpace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>White Space Report</title>
</head>
<body>
    <div class="heading">
        <h1 style="color: #FF0000;" >Remove the files for this after use, they are not secure</h1>
        <h3>Program and Episode titles that have white space at the begining or end</h3>
        <h4>This normally takes a really long time to run. This will edit, save, and publish these items to the "Web" database</h4>
    </div>    
    <form id="form1" method="post" runat="server">
        <asp:Button ID="ProgramsWh" runat="server" OnClick="ProgramsWhite" Text="Show Programs with white space" />
        <asp:Button ID="RemoveProgramsWh" runat="server" OnClick="removeProgramsWhite" Text="Trim white space from Program titles" />
        <br /> <br />
        <asp:Button ID="EpisodeWh" runat="server" OnClick="EpisodesWhite" Text="Show Episodes with white space" />
        <asp:Button ID="RemoveEpisodeWh" runat="server" OnClick="removeEpisodesWhite" Text="Trim white space from Episode titles" />
    </form>
    <div>
        <ul>
            <asp:Label runat="server" ID="Programs"></asp:Label>
            <asp:Label runat="server" ID="Episodes"></asp:Label>
        </ul>
    </div>
</body>
</html>
