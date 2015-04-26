using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Siteworx.Web.layouts.debug
{
    public partial class EmailExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Sitecore.Context.IsLoggedIn)
            {
                Session["SC_LOGIN_REDIRECT"] = Sitecore.Context.RawUrl;
                Response.Redirect("/sitecore/login");
            }
            else
            {
                //Get a list of all the users
                MembershipUserCollection memberCollection = Membership.GetAllUsers();
                emails.Controls.Add(RenderEmailTable(memberCollection));
     
            }
        }

        private static Control RenderEmailTable(MembershipUserCollection memberCollection)
	    {
            var table = new Table();
            TableRow rowHeader = new TableRow();
            TableCell userNameHeader = new TableCell();
            TableCell fullNameHeader = new TableCell();
            TableCell emailHeader = new TableCell();
            TableCell roleHeader = new TableCell();
            TableCell treeHeader = new TableCell();
            TableCell statusHeader = new TableCell();
            TableCell createdHeader = new TableCell();
            TableCell loggedInHeader = new TableCell();

            userNameHeader.Text = "<b>Domain \\ User Name</b>";
            fullNameHeader.Text = "<b>Full Name</b>";
            emailHeader.Text = "<b>EMail</b>";
            roleHeader.Text = "<b>Role</b>";
            treeHeader.Text = "<b>Comments</b>";
            statusHeader.Text = "<b>Enabled/Disabled</b>";
            createdHeader.Text = "<b>Created</b>";
            loggedInHeader.Text = "<b>Last Login</b>";

            rowHeader.Cells.Add(userNameHeader);
            rowHeader.Cells.Add(fullNameHeader);
            rowHeader.Cells.Add(emailHeader);
            rowHeader.Cells.Add(roleHeader);
            rowHeader.Cells.Add(treeHeader);
            rowHeader.Cells.Add(statusHeader);
            rowHeader.Cells.Add(createdHeader);
            rowHeader.Cells.Add(loggedInHeader);
            table.Rows.Add(rowHeader);

            //Add each user do the dropdown list
            foreach (MembershipUser m in memberCollection)
            {
                TableRow row = new TableRow();
                TableCell userName = new TableCell();
                TableCell fullName = new TableCell();
                TableCell email = new TableCell();
                TableCell role = new TableCell();
                TableCell tree = new TableCell();
                TableCell status = new TableCell();
                TableCell created = new TableCell();
                TableCell loggedIn = new TableCell();

                userName.Text = nbsp(m.UserName);
                Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(@m.UserName.ToString(), true);
                fullName.Text = nbsp(user.Profile.FullName);
                email.Text = nbsp(m.Email);
                String userRoleText = "";
                if (user.IsAdministrator)
                {
                    userRoleText = "Administrator";
                }
                foreach (Sitecore.Security.Accounts.Role userRole in user.Roles)
                {
                    if (userRoleText == "")
                    {
                        userRoleText = userRoleText + nbsp(userRole.Name.Replace("sitecore\\", ""));
                    }
                    else
                    {
                        userRoleText = userRoleText + ", " + nbsp(userRole.Name.Replace("sitecore\\", ""));
                    }
                }
                role.Text = nbsp(userRoleText);
                tree.Text = nbsp(user.Profile.Comment);
                status.Text = nbsp(user.Profile.State);
                created.Text = String.Format("{0:M/d/yyyy}", m.CreationDate);
                loggedIn.Text = String.Format("{0:M/d/yyyy}", m.LastLoginDate);

                row.Cells.Add(userName);
                row.Cells.Add(fullName);
                row.Cells.Add(email);
                row.Cells.Add(role);
                row.Cells.Add(tree);
                row.Cells.Add(status);
                row.Cells.Add(created);
                row.Cells.Add(loggedIn);
                table.Rows.Add(row);
            }
            return table;
	    }

        private static String nbsp(String input)
        {
            if (input == "" || input == null)
            {
                return "&nbsp;";
            }
            else
            {
                return input;
            }
        }
    }
}
