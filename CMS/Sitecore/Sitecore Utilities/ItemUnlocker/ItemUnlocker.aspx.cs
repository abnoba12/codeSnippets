using System;
using Sitecore.Data.Fields;

namespace LECTRIC.SC.Tools
{
    public partial class ItemUnlocker : System.Web.UI.Page
    {
        protected Sitecore.Data.Database myContentDatabase = Sitecore.Configuration.Factory.GetDatabase("master");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!txtNode.Text.Equals(string.Empty))
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Sitecore.Data.Items.Item myRootItem = myContentDatabase.Items.GetItem(txtNode.Text);
                    if (myRootItem != null)
                    {
                        RecursiveUnlockItems(myRootItem);
                    }
                    else
                    {
                        Response.Write("Node doesn't exsist");
                    }
                }
            }
            else
            {
                Response.Write("Fill a node into the field");
            }
        }
        protected void RecursiveUnlockItems(Sitecore.Data.Items.Item rootItem)
        {
            if (rootItem != null)
            {
                LockField lockField = rootItem.Fields[Sitecore.FieldIDs.Lock];
                if(lockField != null)
                {
                    //releasing items by just 1 user
                    if (txtUser.Text.Length > 0 && string.Compare(txtUser.Text, lockField.Owner, true) == 0)
                    {
                        lockField.ReleaseLock();
                    }
                    //releasing all items as txtUser isn't filled in
                    else if (txtUser.Text.Length == 0)
                    {
                        lockField.ReleaseLock();
                    }
                    Response.Write("Unlocked: " + rootItem.Paths.FullPath + "<br />\n");
                }
                

                if (rootItem.HasChildren)
                {
                    foreach (Sitecore.Data.Items.Item childItem in rootItem.Children)
                    {
                        RecursiveUnlockItems(childItem);
                    }
                }
            }

        }
    }
}
