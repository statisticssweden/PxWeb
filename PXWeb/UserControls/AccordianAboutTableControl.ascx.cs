using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Web.Core.Management;


namespace PXWeb.UserControls
{
    public partial class AccordianAboutTableControl : System.Web.UI.UserControl
    {
        private IPxUrl _pxUrl = null;

        private IPxUrl PxUrl
        {
            get
            {
                if (_pxUrl == null)
                {
                    _pxUrl = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrl;
            }
        }


        private PCAxis.Metadata.IMetaIdProvider _linkManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = PxUrl.Database;
            string path = PxUrl.Path;
            _linkManager = PXWeb.Settings.Current.Database[PxUrl.Database].Metadata.MetaLinkMethod;
            Localize();
            InitializeMetadata(path);
            DisplayTableMetadataLinks();
        }

        private void Localize()
        {
            lblInfo.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebAboutTable");
        }

        private void DisplayTableMetadataLinks()
        {
            if (!PXWeb.Settings.Current.Selection.MetadataAsLinks)
            {
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                if (PXWeb.Settings.Current.Database[url.Database].Metadata.UseMetadata)
                {
                    if (!string.IsNullOrWhiteSpace(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.MetaId))
                    {
                        List<PCAxis.Metadata.MetaLink> links = _linkManager.GetTableLinks(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.MetaId, LocalizationManager.CurrentCulture.Name).ToList();
                        if (links.Count > 0)
                        {
                            /*If we get other use cases for TableLink, we need to take a look at how we do this, 
                             * but for now we can put everything here */
                            Literal div = new Literal();
                            div.Text = "<div class=\"pxweb_about_the_statistics pxweb-link with-icon\">";
                            divTableLinks.Controls.Add(div);
                            foreach (PCAxis.Metadata.MetaLink link in links)
                            {
                                HyperLink lnk = new HyperLink();
                                lnk.Text = String.Format("<span class=\"link-text\">{0}</span>", Server.HtmlEncode(link.LinkText));
                                lnk.NavigateUrl = link.Link;
                                lnk.Target = link.Target;
                                lnk.CssClass = "external-link-icon";
                                divTableLinks.Controls.Add(lnk);
                            }
                            Literal divend = new Literal();
                            divend.Text = "</div>";
                            divTableLinks.Controls.Add(divend);
                        }
                    }
                }
            }
        }

        private void InitializeMetadata(string path)
        {
            if (PXWeb.Settings.Current.Selection.MetadataAsLinks)
            {
                InitializeDetailedInformation(path);
                SelectionInformation.Visible = false;
                InformationBox.Visible = false;

            }
            else
            {
                SelectionInformation.ShowInformationTypes = PXWeb.Settings.Current.General.Global.ShowInformationTypes.GetSelectedInformationTypes();
                InitializeDetailedInformation(path);
                SelectionInformation.Visible = true;
                InformationBox.Visible = true;
                lblInfo.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebAboutTable");
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                bool bMeta = PXWeb.Settings.Current.Database[url.Database].Metadata.UseMetadata;
            }
        }

        private void InitializeDetailedInformation(string path)
        {
            string strPathTmp = "";
            string strInfoFile = "";
            int iStart;
            char[] tilde = { '~' };


            if (!string.IsNullOrEmpty(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile) && PXWeb.Settings.Current.General.Global.ShowInfoFile)
            {
                try
                {

                    //Check if infofile is an URL
                    Uri uriResult;
                    if (Uri.TryCreate(PaxiomManager.PaxiomModel.Meta.InfoFile, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp)
                    {
                        SetDetailedInformationLink(uriResult.ToString());
                    }
                    else
                    {
                        path = path.Replace("__", @"\");

                        strPathTmp = Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath + path);

                        if (Directory.Exists(strPathTmp))
                        {
                            foreach (var file in Directory.GetFiles(strPathTmp))
                            {
                                if (file != null && Path.GetFileNameWithoutExtension(file).Equals(PaxiomManager.PaxiomModel.Meta.InfoFile, StringComparison.OrdinalIgnoreCase))
                                {
                                    strInfoFile = file;
                                }
                            }
                        }
                    }


                    if (strInfoFile.Length > 0)
                    {
                        strInfoFile = strInfoFile.Replace(@"\", @"/");
                        iStart = strInfoFile.LastIndexOf(PXWeb.Settings.Current.General.Paths.PxDatabasesPath.TrimStart(tilde));

                        if (iStart > -1)
                        {
                            strInfoFile = strInfoFile.Substring(iStart);
                            if (!strInfoFile.StartsWith("/"))
                            {
                                strInfoFile = "~/" + strInfoFile;
                            }
                            else
                            {
                                strInfoFile = "~" + strInfoFile;
                            }

                            SetDetailedInformationLink(HttpUtility.UrlPathEncode(strInfoFile));

                        }
                    }
                }
                catch (SystemException)
                {
                    // Not a realtive path

                    // Is it a HTML-link?
                    if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.Contains("<") &&
                        PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.Contains(">"))
                    {
                        //check if the infofile shall appears as links next to the footnote link or as a link in 
                        //the information section on the tab About table
                        if (PXWeb.Settings.Current.General.Global.ShowInfoFile)
                        {
                            litDetailedInformation2.Visible = true;
                            litDetailedInformation2.Text = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.ToLower();
                            lnkDetailedInformation2.Visible = false;
                            lnkDetailedInformation2.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebDetailedInformation");

                        }
                    }
                    else
                    {
                        litDetailedInformation2.Visible = true;
                        litDetailedInformation2.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebDetailedInformation");
                        lnkDetailedInformation2.Visible = true;
                        lnkDetailedInformation2.NavigateUrl = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile;
                        lnkDetailedInformation2.Text = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile;
                    }
                }
            }
        }

        private void SetDetailedInformationLink(string link)
        {
            if (PXWeb.Settings.Current.General.Global.ShowInfoFile)
            {
                lnkDetailedInformation2.Text = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.ToLower();
                lnkDetailedInformation2.Visible = true;
                lnkDetailedInformation2.NavigateUrl = link;
                litDetailedInformation2.Visible = true;
                litDetailedInformation2.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebDetailedInformation");
            }

        }
    }
}