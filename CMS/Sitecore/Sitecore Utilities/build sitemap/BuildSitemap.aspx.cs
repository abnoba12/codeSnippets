using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml;
using Sitecore;
using Sitecore.Sites;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Links;
using Sitecore.Xml;
using Sitecore.Web;
using Sitecore.Security.Accounts;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace Website.layouts
{
    public partial class BuildSitemap : System.Web.UI.Page
    {
        private string nsPage = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private string nsImage = "http://www.google.com/schemas/sitemap-image/1.1";        

        protected void Page_Load(object sender, EventArgs e)
        {                        
            BuildSiteMap("website", "sitemap.xml");
        }

        protected void OnPublishEnd(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            BuildSiteMap("website", "sitemap.xml");
        }

        private void BuildSiteMap(string sitename, string sitemapUrlNew)
        {
            Site site = SiteManager.GetSite(sitename);
            string startPath = Factory.GetSite(sitename).StartPath;
            List<Item> sitemapItems = this.GetSitemapItems(startPath);
            string path = MainUtil.MapPath("/" + sitemapUrlNew);
            XmlDocument doc = this.BuildSitemapXML(sitemapItems, site);

            XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
        }

        private XmlDocument BuildSitemapItem(XmlDocument doc, Item item, Site site)
        {
            string text = HtmlEncode(this.GetItemUrl(item, false, site));
            string str2 = HtmlEncode(item.Statistics.Updated.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            XmlNode lastChild = doc.LastChild;
            XmlNode newChild = doc.CreateElement("url");
            lastChild.AppendChild(newChild);
            //location
            XmlNode loc = doc.CreateElement("loc");
            newChild.AppendChild(loc);
            loc.AppendChild(doc.CreateTextNode(text));
            //last modified
            XmlNode modified = doc.CreateElement("lastmod");
            newChild.AppendChild(modified);
            modified.AppendChild(doc.CreateTextNode(str2));
            //change frequency
            XmlNode freq = doc.CreateElement("changefreq");
            newChild.AppendChild(freq);
            freq.AppendChild(doc.CreateTextNode("monthly"));
            //priority
            XmlNode priority = doc.CreateElement("priority");
            newChild.AppendChild(priority);
            if (item.ID.Equals(BusinessLogic.Helper.Constants.Home))
                priority.AppendChild(doc.CreateTextNode("1.0"));
            else
                priority.AppendChild(doc.CreateTextNode("0.5"));

            //try to find any images 
            List<String> images = new List<String>();
            Sitecore.Data.Fields.TextField pagebody = item.Fields["pagebody"];
            Sitecore.Data.Fields.ImageField personimage = item.Fields["picture"];
            Sitecore.Data.Fields.ImageField campusimage = item.Fields["campusPhoto"];

            if (pagebody != null)
            {
                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(pagebody.Value);

                //get images
                var links = html.DocumentNode.SelectNodes("//img");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        string imgUrl = link.Attributes["src"].Value;                        
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            if (!images.Contains(imgUrl))
                                images.Add(imgUrl);
                        }
                    }
                }
            }

            if (personimage != null && personimage.MediaItem != null)
                images.Add(Sitecore.Resources.Media.MediaManager.GetMediaUrl(personimage.MediaItem));

            if (campusimage != null && campusimage.MediaItem != null)
                images.Add(Sitecore.Resources.Media.MediaManager.GetMediaUrl(campusimage.MediaItem));

            //add images to xml            
            foreach (String image in images)
            {
                string src = HtmlEncode(GetUrl(image, site));
                XmlNode img = doc.CreateNode(XmlNodeType.Element, "image", "image", nsImage);
                newChild.AppendChild(img);
                XmlNode imgSrc = doc.CreateNode(XmlNodeType.Element, "image", "loc", nsImage);
                img.AppendChild(imgSrc);
                imgSrc.AppendChild(doc.CreateTextNode(src));
            }

            return doc;
        }

        private XmlDocument BuildSitemapXML(List<Item> items, Site site)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode newChild = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(newChild);
            XmlNode node2 = doc.CreateElement("urlset");
            XmlAttribute node = doc.CreateAttribute("xmlns");
            node.Value = nsPage;
            node2.Attributes.Append(node);

            //image namespace
            XmlAttribute imagens = doc.CreateAttribute("xmlns:image");
            imagens.Value = nsImage;
            node2.Attributes.Append(imagens);

            doc.AppendChild(node2);
            foreach (Item item in items)
            {
                //don't include redirect items, plain text pages, or error pages
                if (!IsExcludedType(item))
                {
                    doc = this.BuildSitemapItem(doc, item, site);
                }
            }

            return doc;
        }

        private bool IsExcludedType(Item item)
        {
            if (item.TemplateID.Equals(BusinessLogic.Helper.Constants.RedirectPage))
            {
                return true;
            }
            else if (item.TemplateID.Equals(BusinessLogic.Helper.Constants.PlainTextPage))
            {
                return true;
            }
            else if (item.TemplateID.Equals(BusinessLogic.Helper.Constants.Folder))
            {
                return true;
            }
            else if (item.Name.Equals("404") || item.Name.Equals("500"))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        private string GetItemUrl(Item item, bool addAspxExtension, Site site)
        {
            UrlOptions options = new UrlOptions
            {
                SiteResolving = Settings.Rendering.SiteResolving,
                AddAspxExtension = addAspxExtension,
                Site = SiteContext.GetSite(site.Name),
                LanguageEmbedding = LanguageEmbedding.Never,
                AlwaysIncludeServerUrl = false
            };
            string itemUrl = LinkManager.GetItemUrl(item, options);

            return GetUrl(itemUrl, site);
        }

        private string GetUrl(string itemUrl, Site site)
        {
            StringBuilder builder = new StringBuilder();
            string hostname = ConfigurationManager.AppSettings["xmlSitemapHostname"];

            //add hostname to relative link
            if (!itemUrl.StartsWith("http"))
            {
                builder.Append("http://");
                builder.Append(hostname);
                //if a media url that doesn't start with a slash, add one
                if (itemUrl.StartsWith("~"))
                    builder.Append("/");
                builder.Append(itemUrl);
            }
            
            else
            {
                builder.Append(itemUrl);
            }
            return builder.ToString();
        }

        private List<Item> GetSitemapItems(string rootPath)
        {
            Item[] descendants;
            //string str = SitemapManagerConfiguration.EnabledTemplates;
            //string excludeItems = SitemapManagerConfiguration.ExcludeItems;
            Item item = Factory.GetDatabase("web").Items[rootPath];
            using (new UserSwitcher(Sitecore.Security.Accounts.User.FromName(@"extranet\Anonymous", true)))
            {
                descendants = item.Axes.GetDescendants();
            }
            List<Item> list = descendants.ToList<Item>();
            list.Insert(0, item);
            return list;
            /*
            List<string> enabledTemplates = this.BuildListFromString(str, '|');
            List<string> excludedNames = this.BuildListFromString(excludeItems, '|');
            return (from itm in list
                    where ((itm.Template != null) && enabledTemplates.Contains(itm.Template.ID.ToString())) && !excludedNames.Contains(itm.ID.ToString())
                    select itm).ToList<Item>();
            */
        }

        private static string HtmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        private List<string> BuildListFromString(string str, char separator)
        {
            return (from dtp in str.Split(new char[] { separator })
                    where !string.IsNullOrEmpty(dtp)
                    select dtp).ToList<string>();
        }       
    }
}
