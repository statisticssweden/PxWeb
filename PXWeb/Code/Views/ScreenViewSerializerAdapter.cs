using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Query;
using PCAxis.Web.Core.Management;
using PCAxis.Paxiom;
using PCAxis.Web.Controls;

namespace PXWeb.Views
{
    public abstract class ScreenViewSerializerAdapter : IViewSerializer
    {
        public abstract Output Save();

        public abstract void Render(string format, SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe);

        protected void RenderToScreen(SavedQuery query, PXModel model, string defaultLayout, string page, bool safe) {
            if (query.Sources.Count < 1) throw new Exception("No source specified"); //TODO fix message

            var src = query.Sources[0];

            if (string.Compare(src.SourceIdType, "path") != 0) throw new Exception("Incompatible source type"); //TODO fix

            string layout;
            if (query.Output.Params.ContainsKey("layout"))
            {
                layout = query.Output.Params["layout"] ?? defaultLayout;
            }
            else
            {
                layout = defaultLayout;
            }
                 

            string path = src.Source;
            var tableName = GetTableName(src);

            path = path.Substring(0, path.Length - (tableName.Length + 1));

            if (string.Compare(query.Sources[0].Type, "CNMM") == 0)
            {
                if (!path.StartsWith("START"))
                {
                    path = "START__" + path;
                }
            }

            path = path.Replace(@"/", PxPathHandler.NODE_DIVIDER);
            
            string url = null;

            if (RouteInstance.RouteExtender == null)
            {
                List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();

                linkItems.Add(new LinkManager.LinkItem(PxUrl.TABLE_KEY, tableName));
                linkItems.Add(new LinkManager.LinkItem(PxUrl.PATH_KEY, path.ToString()));
                linkItems.Add(new LinkManager.LinkItem(PxUrl.DB_KEY, src.DatabaseId));
                linkItems.Add(new LinkManager.LinkItem(PxUrl.LANGUAGE_KEY, model.Meta.CurrentLanguage));
                linkItems.Add(new LinkManager.LinkItem(PxUrl.LAYOUT_KEY, layout));

                url = LinkManager.CreateLink(page, false, linkItems.ToArray());
            }
            else
            {
                string tableId = RouteInstance.RouteExtender.GetTableIdByName(tableName);
                url = RouteInstance.RouteExtender.GetPresentationRedirectUrl(tableId, layout);
            }

            //info about loaded saved query in query string
            if (Settings.Current.Features.SavedQuery.ShowPeriodAndId)
            {
                var tvar = model.Meta.Variables.FirstOrDefault(v => v.IsTime);

                if (tvar != null)
                {
                    if (!string.IsNullOrEmpty(query.LoadedQueryName))
                    {
                        url += "?loadedQueryId=" + System.IO.Path.GetFileNameWithoutExtension(query.LoadedQueryName);
                    }

                    string timeType = "item"; //default
                    string timeValue = null;
                    var timeQueryItem = query.Sources[0].Quieries.FirstOrDefault(x => x.Code == tvar.Code);

                    if (timeQueryItem != null)
                    {
                        if (timeQueryItem.Selection.Filter == "top")
                        {
                            timeType = "top";
                            timeValue = timeQueryItem.Selection.Values[0];
                        }
                        else if (timeQueryItem.Selection.Filter == "from")
                        {
                            timeType = "from";
                            timeValue = timeQueryItem.Selection.Values[0];
                        }

                        url += "&timeType=" + timeType;

                        if (timeValue != null)
                        {
                            url += "&timeValue=" + timeValue;
                        }
                    }
                }
            }

            PCAxis.Web.Core.Management.LocalizationManager.ChangeLanguage(model.Meta.CurrentLanguage);
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = model;

            PaxiomManager.OperationsTracker = new OperationsTracker(query.Workflow.ToArray());
            if (!safe)
            {
                PaxiomManager.OperationsTracker.IsUnsafe = true;
            }
            PaxiomManager.OperationsTracker.IsTimeDependent = query.TimeDependent;


            HttpContext.Current.Response.Redirect(url);
        }

        private string GetTableName(TableSource src)
        {
            if (src.SourceIdType == "path")
            {
                var parts = src.Source.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                return parts[parts.Length - 1];
            }

            return src.Source;
        }

        /// <summary>
        /// Check that the parameter exists and that it has a value
        /// </summary>
        /// <param name="query">Query object</param>
        /// <param name="key">Key for the parameter</param>
        /// <returns>True if the parameter exists and has a value, else false</returns>
        protected bool CheckParameter(PCAxis.Query.SavedQuery query, string key)
        {
            if (query.Output.Params.ContainsKey(key))
            {
                if (query.Output.Params[key] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
