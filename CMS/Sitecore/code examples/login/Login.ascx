<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Siteworx.Web.layouts.renderings.login.Login" %>
<asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" />
<div class="featured_content sign_in border">    
    <div class="heading">
        <h3>Sign In</h3>
    </div>
    <div id="sign_in_form" class="fields cmxform">
        <p class="field error" runat="server" ID="Message" visible="false">
            <asp:label id="lblMessage" runat="server" /> 
        </p>        
        <div class="email field">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtUsername">Username</asp:Label>
            <asp:textbox id="txtUsername" CssClass="required email" runat="server" />
        </div>
        <div class="password field">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="txtPassword">Password</asp:Label>
            <asp:textbox id="txtPassword" CssClass="required" TextMode="Password" runat="server" />            
        </div>
        <br />
        <a class="field" id="password_forgot" href="JavaScript:void(0); " >Forgot your password?</a>
        <a class="field" id="password_reset" href="JavaScript:void(0);" >Change your password</a>
        <div class="remember_me field">
            <asp:checkbox id="Persist" runat="server" />
            <asp:Label ID="Label3" runat="server" AssociatedControlID="Persist">Remember me on this computer</asp:Label>
        </div>
        <div class="submit field">
            <asp:LinkButton id="btnGo" CssClass="button submit" OnClientClick="this.blur()" onclick="SignIn" runat="server" Text="Sign In"><span>Sign In</span></asp:LinkButton>
        </div>
    </div>
</div>
<div class="password-dialog">
    <div id="forgot_diag" title="Forgot your password?" >
        <asp:UpdatePanel ID="ResetPasswordPanel" runat="server">
            <ContentTemplate>
                <strong>Enter the username you signed up with and we'll send you a randomly generated password.</strong>
                <br /><br />
                <asp:Label ID="lblResetUser" runat="server" AssociatedControlID="txtResetUser">Username: </asp:Label>
                <asp:textbox id="txtResetUser" runat="server" />
                <br /><br />
                <p class="field error" runat="server" ID="ResetUserMessage" visible="true">
                    <asp:Label id="lblResetUserMessage" runat="server" />
                </p>  
                <asp:LinkButton id="checkUser" CssClass="button" onclick="CheckUser" runat="server" Text="Send" ><span>Send</span></asp:LinkButton>
                <asp:LinkButton id="forgotPasswordClose" CssClass="button" runat="server" Text="Close" OnClientClick="$('#forgot_diag').dialog('close');" visible="false" ><span>Close</span></asp:LinkButton>
                <div style="clear:both;"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div id="reset_diag" title="Change your password" >
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td colspan=2>
                            <strong>Enter your old password and the password you desire.</strong>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblResetUsername" runat="server" AssociatedControlID="txtResetUsername">Username: </asp:Label></td>
                        <td><asp:textbox id="txtResetUsername" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblResetPassword" runat="server" AssociatedControlID="txtResetPassword">Password: </asp:Label></td>
                        <td><asp:textbox id="txtResetPassword" TextMode="Password" runat="server" /></td>
                    </tr>
                    <tr><td colspan=2>&nbsp;</td></tr>
                    <tr>
                        <td><asp:Label ID="lblNewPassword1" runat="server" AssociatedControlID="txtNewPassword1">New Password: </asp:Label></td>
                        <td><asp:textbox id="txtNewPassword1" TextMode="Password" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblNewPassword2" runat="server" AssociatedControlID="txtNewPassword2">New Password (confirm): </asp:Label></td>
                        <td><asp:textbox id="txtNewPassword2" TextMode="Password" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan=2><asp:Label ID="lblResetMessage" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <asp:LinkButton id="changePassword" CssClass="button" OnClientClick="this.blur()" onclick="ChangePassword" runat="server" Text="Change"><span>Change</span></asp:LinkButton>
                            <asp:LinkButton id="changePasswordClose" CssClass="button" runat="server" Text="Close" OnClientClick="$('#reset_diag').dialog('close');" visible="false" ><span>Close</span></asp:LinkButton>
                            <div style="clear:both;"></div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>