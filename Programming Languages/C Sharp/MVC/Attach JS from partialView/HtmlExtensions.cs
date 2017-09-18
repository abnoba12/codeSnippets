using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

/// <summary>
/// Modfied from https://stackoverflow.com/questions/5433531/using-sections-in-editor-display-templates/5433722#5433722
/// </summary>
public static class HtmlExtensions
{
    public static MvcHtmlString AddScript(this HtmlHelper htmlHelper, string path)
    {
        var webPage = htmlHelper.ViewDataContainer as WebPageBase;
        var virtualPath = webPage.VirtualPath;
        string jsPath = UrlHelper.GenerateContentUrl(path, htmlHelper.ViewContext.HttpContext);

        string scriptTag = "<script src=\"" + jsPath + "\" type = \"text/javascript\" ></script>";
        htmlHelper.ViewContext.HttpContext.Items["_partialscripts_"+virtualPath] = scriptTag;
        return MvcHtmlString.Empty;
    }

    public static IHtmlString RenderPartialScripts(this HtmlHelper htmlHelper)
    {
        foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
        {
            if (key.ToString().StartsWith("_partialscripts_"))
            {
                string scriptTag = htmlHelper.ViewContext.HttpContext.Items[key].ToString();
                if (!String.IsNullOrEmpty(scriptTag))
                {
                    htmlHelper.ViewContext.Writer.Write(scriptTag);
                }
            }
        }
        return MvcHtmlString.Empty;
    }
}