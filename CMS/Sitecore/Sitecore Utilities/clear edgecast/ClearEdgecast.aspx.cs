using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Siteworx.EdgecastUtilities;

namespace Website.scripts
{
    public partial class ClearEdgecast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void DoPurge(object sender, EventArgs args)
        {
            string status;

            //get edgecast settings from app config
            string token = ConfigurationManager.AppSettings["edgecast.token"];
            string custId = ConfigurationManager.AppSettings["edgecast.custid"];
            string api = ConfigurationManager.AppSettings["edgecast.api"];

            //if input path has starting /, remove it
            string path = Path.Text;
            if (path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            //create purge request url            
            string purgePath = ConfigurationManager.AppSettings["edgecast.hostname"] + "/" + path;

            PurgeContent.Execute(api, custId, token, purgePath, 8, out status);

            StatusLit.Text = status;
        }
    }

    
}
