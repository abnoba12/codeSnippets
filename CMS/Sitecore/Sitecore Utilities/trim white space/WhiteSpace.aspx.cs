using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Siteworx.Domain.Search;

namespace Siteworx.Web.layouts.debug
{
    public partial class WhiteSpace : System.Web.UI.Page
    {
        protected List<Sitecore.Data.Items.Item> ProgramList = new List<Sitecore.Data.Items.Item>();
        protected List<Sitecore.Data.Items.Item> EpisodeList = new List<Sitecore.Data.Items.Item>();

        public void ProgramsWhite(object sender, EventArgs e)
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("master");
            Sitecore.Data.Items.Item[] programItems = database.SelectItems("fast:/sitecore/content/Home/Programs/descendant::*[@@templatename='IndividualProgramPage']");
                        
            if (programItems.Count() > 0)
            {
                foreach (Sitecore.Data.Items.Item result in programItems)
                {
                    Domain.Pages.Program program = Domain.Context.Get<Domain.Pages.Program>(result.ID);
                    if (program != null && program.Title != null)
                    {
                        String t = program.Title;
                        if (t.StartsWith(" ") || t.EndsWith(" "))
                        {
                            Programs.Text += "<li>" + result.ID.ToString() + ": ##" + t.ToString() + "##</li>\n";
                            ProgramList.Add(result);
                        }
                    }
                }
            }
        }

        public void removeProgramsWhite(object sender, EventArgs e)
        {
            ProgramsWhite(sender, e);
            foreach (Sitecore.Data.Items.Item result in ProgramList)
            {                
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Domain.Pages.Program program = Domain.Context.Get<Domain.Pages.Program>(result.ID);
                    result.Editing.BeginEdit();
                    try
                    {
                        result.Fields["Title"].Value = program.Title.Trim();
                        result.Editing.EndEdit();
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Could not update item " + result.Paths.FullPath + ": " + ex.Message, this);
                        result.Editing.CancelEdit();
                    }

                    Sitecore.Publishing.PublishOptions publishOptions =
                      new Sitecore.Publishing.PublishOptions(result.Database, Sitecore.Data.Database.GetDatabase("web"), Sitecore.Publishing.PublishMode.SingleItem,
                                                             result.Language, System.DateTime.Now);  // Create a publisher with the publishoptions
                    Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

                    // Choose where to publish from
                    publisher.Options.RootItem = result;

                    // Do the publish!
                    publisher.Publish();
                }
            }
            Programs.Text += ProgramList.Count.ToString() + " Program Titles Trimed";
        }

        public void EpisodesWhite(object sender, EventArgs e)
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("master");
            Sitecore.Data.Items.Item[] episodeItems = database.SelectItems("fast:/sitecore/content/Home/Programs/descendant::*[@@templatename='IndividualEpisodePage']");

            if (episodeItems.Count() > 0)
            {
                foreach (Sitecore.Data.Items.Item result in episodeItems)
                {
                    Domain.Pages.Episode episode = Domain.Context.Get<Domain.Pages.Episode>(result.ID);
                    if (episode != null && episode.Title != null)
                    {
                        String ep = episode.Title;
                        if (ep.StartsWith(" ") || ep.EndsWith(" "))
                        {
                            Episodes.Text += "<li>" + episode.ID.ToString() + ": ##" + ep.ToString() + "##</li>\n";
                            EpisodeList.Add(result);
                        }
                    }
                }
            }
            
        }

        public void removeEpisodesWhite(object sender, EventArgs e)
        {
            EpisodesWhite(sender, e);
            foreach (Sitecore.Data.Items.Item result in EpisodeList)
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Domain.Pages.Episode episode = Domain.Context.Get<Domain.Pages.Episode>(result.ID);
                    result.Editing.BeginEdit();
                    try
                    {
                        result.Fields["Title"].Value = episode.Title.Trim();
                        result.Editing.EndEdit();
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Could not update item " + result.Paths.FullPath + ": " + ex.Message, this);
                        result.Editing.CancelEdit();
                    }

                    Sitecore.Publishing.PublishOptions publishOptions =
                      new Sitecore.Publishing.PublishOptions(result.Database, Sitecore.Data.Database.GetDatabase("web"), Sitecore.Publishing.PublishMode.SingleItem,
                                                             result.Language, System.DateTime.Now);  // Create a publisher with the publishoptions
                    Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

                    // Choose where to publish from
                    publisher.Options.RootItem = result;

                    // Do the publish!
                    publisher.Publish();
                }
            }
            Episodes.Text += EpisodeList.Count.ToString() + " Episode Titles Trimed";
        }
    }
}