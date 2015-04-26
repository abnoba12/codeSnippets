using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;

namespace Website.TCWAdmin
{
    public partial class ClearSitecoreCache : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lblResponse.Text = string.Empty;
            if (!IsPostBack)
            {
                if (Request.QueryString["clearcache"] != null && Request.QueryString["clearcache"] == "true")
                    ClearAllSlaveServerCaches();
            }
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ClearAllSlaveServerCaches();
        }

        /// <summary>
        /// Claers the cache on all the configured slave servers.
        /// </summary>
        private void ClearAllSlaveServerCaches()
        {
            // Get a reference to the proxy for the webservice.
            StagingModuleService.StagingWebService service = new Website.StagingModuleService.StagingWebService();

            Item stagingModuleFolder = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/system/Staging");
            // Each Master Server might have multiple slave servers configured. Clear the cache on each of them.
            bool failure = false;
            if (stagingModuleFolder != null)
            {
                foreach (Item stagingServerItem in stagingModuleFolder.Children)
                {
                    try
                    {
                        if (stagingServerItem.Template.Key == "stagingserver") // Ignore folder and other items.
                            ClearServerCacheByServer(stagingServerItem, service);
                    }
                    catch (Exception ex)
                    {
                        failure = true;
                    }
                }
            }
            if (failure)
                lblResponse.Text = "Failed to clear the cache on or more of the slave servers.";
            else
                lblResponse.Text = "Successfully cleared the cache on all the slave servers.";
            lblIntroText.Visible = false;
            btnSubmit.Visible = false;
            
        }

        /// <summary>
        /// Clears the cache on a give server.
        /// </summary>
        /// <param name="stagingServerItem"></param>
        /// <param name="service"></param>
        private void ClearServerCacheByServer(Item stagingServerItem, Website.StagingModuleService.StagingWebService service)
        {
            // Set the preference from the Sitecore content item for staging module.
            if (stagingServerItem != null)
            {
                bool fullCaheClear = stagingServerItem.Fields["Cache"].Value.ToLower() == "full" ? true : false;
                // Replace the default webservice URL in proxy with the actual server.
                service.Url = stagingServerItem.Fields["URL"].Value + "/sitecore modules/staging/service/api.asmx";
                // Get the credentials for invoking the web method.
                StagingModuleService.StagingCredentials credentials = new Website.StagingModuleService.StagingCredentials();
                credentials.Username = stagingServerItem.Fields["Username"].Value;
                credentials.Password = stagingServerItem.Fields["Password"].Value;
                credentials.isEncrypted = false;

                // Invoke the Clear cahe method on the specified URL.
                service.ClearCache(fullCaheClear, credentials);                    
                    
            }
        }

        
    }
}
