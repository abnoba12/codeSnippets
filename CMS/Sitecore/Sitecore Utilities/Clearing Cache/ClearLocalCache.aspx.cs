using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;

namespace Website.TCWAdmin
{
    public partial class ClearLocalCache : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (pass.Text == "tcwS1teworx!")
            {
                Sitecore.Caching.CacheManager.ClearAllCaches();
                lblIntroText.Text = "Cache has been cleared";
            }
            else
            {
                lblIntroText.Text = "No";
            }
        }        
    }
}
