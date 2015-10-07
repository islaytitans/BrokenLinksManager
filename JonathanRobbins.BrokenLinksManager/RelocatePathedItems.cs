using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace JonathanRobbins.BrokenLinksManager
{
    public class RelocatePathedItems
    {
        public void Process()
        {
            Sitecore.Data.Database db = Sitecore.Context.Database;
            Sitecore.Links.LinkDatabase linkDb = Sitecore.Globals.LinkDatabase;
            Sitecore.Links.ItemLink[] brokenLinks = linkDb.GetBrokenLinks(db);

            foreach (ItemLink brokenLink in brokenLinks)
            {
            Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase("master");
            Sitecore.Links.LinkDatabase linkDb = Sitecore.Globals.LinkDatabase;
            Sitecore.Links.ItemLink[] brokenLinks = linkDb.GetBrokenLinks(db);

            foreach (ItemLink brokenLink in brokenLinks)
            {
                if (brokenLink.SourceFieldID == Sitecore.FieldIDs.LayoutField
                    && brokenLink.GetTargetItem() == null
                    && brokenLink.TargetPath.ToLower().StartsWith("/sitecore/content/shared content/widget data/"))
                {
                    Item sourceItem = brokenLink.GetSourceItem();
                    if (sourceItem == null)
                        return;

                    LayoutField layoutField = sourceItem.Fields[brokenLink.SourceFieldID];
                    if (layoutField == null)
                        return;

                    string newItemPath = brokenLink.TargetPath.ToLower().Replace("/sitecore/content/site data/",
                        "/sitecore/content/shared content/widget data/");

                    Item correctLinkItem = Sitecore.Context.Database.GetItem(newItemPath);
                    if (correctLinkItem == null)
                        return;

                    sourceItem.Editing.BeginEdit();
                    try
                    {
                        //string xml = sourceItem[Sitecore.FieldIDs.LayoutField];

                        //sourceItem[brokenLink.SourceFieldID] =
                        //    xml.ToLower().Replace(brokenLink.TargetPath.ToLower(),
                        //        correctLinkItem.Paths.FullPath);

                        layoutField.Relink(brokenLink, correctLinkItem);

                        sourceItem.Editing.EndEdit(true);
                    }
                    catch (Exception e)
                    {
                        sourceItem.Editing.CancelEdit();
                    }
                }
            }
                }
            }
        }
    }
}
