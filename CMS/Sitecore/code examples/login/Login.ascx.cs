using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Siteworx.Web.layouts.renderings.login
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Request.QueryString["item"] != null && Request.QueryString["item"].ToString() != "")
                {
                    Session["referrer"] = Request.QueryString["item"].ToString();
                }
                else if (Request.UrlReferrer != null && Request.Url.Authority == Request.UrlReferrer.Authority)
                {
                    Session["referrer"] = Request.UrlReferrer.ToString();
                }
                else
                {
                    Session["referrer"] = null;
                }
            }
        }

        protected void SignIn(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                lblMessage.Text = "Invalid username.";
                Message.Visible = true;
            }
            else if (String.IsNullOrEmpty(txtPassword.Text))
            {
                lblMessage.Text = "Invalid password.";
                Message.Visible = true;
            }
            else
            {
                try
                {
                    Sitecore.Security.Domains.Domain domain = Sitecore.Context.Domain;
                    string domainUser = domain.Name + @"\" + txtUsername.Text;

                    if (Sitecore.Security.Authentication.AuthenticationManager.Login(domainUser, txtPassword.Text, Persist.Checked))
                    {
                        if (Session["referrer"] != null && Session["referrer"].ToString() != "")
                        {
                            Sitecore.Web.WebUtil.Redirect(Session["referrer"].ToString());
                        }
                        else
                        {
                            Sitecore.Web.WebUtil.Redirect("/");
                        }
                    }
                    else
                    {
                        throw new System.Security.Authentication.AuthenticationException("Invalid username or password.");
                    }
                }
                catch (System.Security.Authentication.AuthenticationException)
                {
                    lblMessage.Text = "Invalid username or password";
                    Message.Visible = true;
                }
            }
        }

        protected void CheckUser(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser("extranet\\" + txtResetUser.Text);
            
            if (user != null)
            {
                string password = user.ResetPassword();

                SendResetPassword(user.Email, password, user);
            }
            else
            {
                lblResetUserMessage.Text = "Sorry, that username does not exist.  Try re-typing it or registering that username now.";
                lblResetUserMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void SendResetPassword(string email, string password, MembershipUser u)
        {
            Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(u.UserName, true);

            string name = user.Profile.FullName;
            string username = user.LocalName;
            
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(email);
                message.From = new MailAddress("PressRoom@pbs.org");
                message.Subject = "Reset Password Request for: " + username;
                message.Body = name + ",\n\nA request to reset the password for your account has been made at PBS Pressroom.  Your new login information is as follows:\n\nUsername: " + username + "\nPassword: " + password + "\n\nYou may change your password in the same place you requested this new one.\n\nIf this request was a mistake please notify PBS Pressroom immediately by replying to this email.\n\nRegards,\nPBS Pressroom Team";
                Sitecore.MainUtil.SendMail(message);

                lblResetUserMessage.Text = "An email will be sent to '" + email + "' with your reset password.  Please allow up to an hour for the email to arrive.";
                lblResetUserMessage.ForeColor = System.Drawing.Color.Green;

                forgotPasswordClose.Visible = true;
                checkUser.Visible = false;
            }
            catch (Exception ex)
            {
                lblResetUserMessage.Text = "Error: The system was unable to send you a reset password, please try agian.";
                lblResetUserMessage.ForeColor = System.Drawing.Color.Red;

                Sitecore.Diagnostics.Log.Error("Could not send auto reply email!", ex, this);
            }
        }

        protected void ChangePassword(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtResetUsername.Text))
            {
                lblResetMessage.Text = "Invalid username.";
                lblResetMessage.ForeColor = System.Drawing.Color.Red;
            }
            else if (String.IsNullOrEmpty(txtResetPassword.Text))
            {
                lblResetMessage.Text = "Invalid password.";
                lblResetMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                try
                {
                    string domainUser = "extranet\\" + txtResetUsername.Text;

                    lblResetMessage.Text = "user: " + Membership.GetUser(domainUser).IsApproved;

                    if (Sitecore.Security.Authentication.AuthenticationManager.Login(domainUser, txtResetPassword.Text, Persist.Checked))
                    {
                        if (string.IsNullOrEmpty(txtNewPassword1.Text) && string.IsNullOrEmpty(txtNewPassword2.Text))
                        {
                            throw new Exception("You must choose a new password.");
                        }

                        if (txtNewPassword1.Text != txtNewPassword2.Text)
                        {
                            throw new Exception("Those passwords don't match");
                        }

                        MembershipUser user = Membership.GetUser(domainUser);
                        user.ChangePassword(txtResetPassword.Text, txtNewPassword1.Text);

                        Sitecore.Security.Authentication.AuthenticationManager.Logout();

                        lblResetMessage.Text = "Password successfully changed. Please sign-in using your new password.";
                        lblResetMessage.ForeColor = System.Drawing.Color.Green;

                        changePassword.Visible = false;
                        changePasswordClose.Visible = true;
                    }
                    else
                    {
                        lblResetMessage.Text = "Invalid username or password.";
                        lblResetMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (System.Security.Authentication.AuthenticationException)
                {
                    lblResetMessage.Text = "Invalid email address or password";
                    lblResetMessage.ForeColor = System.Drawing.Color.Red;
                }
                catch (Exception ex)
                {
                    lblResetMessage.Text = ex.Message;
                    lblResetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}