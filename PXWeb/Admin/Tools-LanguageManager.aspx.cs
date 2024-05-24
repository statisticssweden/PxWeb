using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace PXWeb.Admin
{
    public partial class Tools_LanguageManager : System.Web.UI.Page
    {
        string _fallbackPath;
        string _languagePath;
        XmlNodeList _xmlnodesFallback;
        /// <summary>
        /// Log object
        /// </summary>
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Tools_LanguageManager));

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                GetCultures();
            }

            InitFields();
            ShowTable();
        }

        /// <summary>
        /// Save language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            PXWeb.Language.LanguageData langData;
            string key;
            string value;
            TextBox txt;

            if (_xmlnodesFallback == null)
            {
                return;
            }

            if (Page.IsValid)
            {
                langData = new PXWeb.Language.LanguageData(_languagePath, cboLanguage.SelectedValue);
                langData.ClearSentences();

                foreach (XmlNode node in _xmlnodesFallback)
                {
                    key = node.Attributes["name"].Value;

                    if (PlaceHolderTable.FindControl("txt" + key) != null)
                    {
                        txt = (TextBox)PlaceHolderTable.FindControl("txt" + key);
                        value = txt.Text;
                    }
                    else
                    {
                        value = "";
                    }

                    if (!String.IsNullOrEmpty(value))
                    {
                        langData.InsertSentence(key, value);
                    }
                }

                langData.Save();
            }
        }

        /// <summary>
        /// Initialize private fields
        /// </summary>
        private void InitFields()
        {
            string basepath = System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.LanguagesPath, PCAxis.Paxiom.Configuration.ConfigurationHelper.LocalizationSection.BaseFile);

            _fallbackPath = MapPath(basepath + ".xml");

            if (!System.IO.File.Exists(_fallbackPath))
            {
                _logger.ErrorFormat("Could not find fallback language file '{0}'", _fallbackPath);
                return;
            }

            _xmlnodesFallback = GetXmlNodes(_fallbackPath);
            if (string.Compare(cboLanguage.SelectedValue, "en", true) == 0)
            {
                _languagePath = MapPath(basepath + ".xml");
            }
            else
            {
                _languagePath = MapPath(basepath + "." + cboLanguage.SelectedValue + ".xml");
            }

        }

        /// <summary>
        /// Display table 
        /// </summary>
        private void ShowTable()
        {
            string key;
            string value;
            HtmlTableRow tr;
            HtmlTableCell td;
            TextBox txt;
            Label lbl;
            Image img;
            bool alternating = false;

            foreach (XmlNode node in _xmlnodesFallback)
            {
                key = node.Attributes["name"].Value;
                value = node.Attributes["value"].Value;

                tr = new HtmlTableRow();

                if (alternating)
                {
                    tr.Attributes.Add("Class", "languageManagerTableAlternatingRow");
                }
                else
                {
                    tr.Attributes.Add("Class", "languageManagerTableRow");
                }

                alternating = !alternating;

                td = new HtmlTableCell();
                td.Attributes.Add("Class", "languageManagerKeyColumn");
                img = new Image();
                img.ImageUrl = System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, "questionmark.gif");
                img.ToolTip = key;
                td.Controls.Add(img);
                //lbl = new Label();
                //lbl.Text = key;
                //lbl.CssClass = "languageManagerKeyColumn";
                //td.Controls.Add(lbl);
                //td.InnerText = key;
                tr.Cells.Add(td);

                td = new HtmlTableCell();
                td.Attributes.Add("Class", "languageManagerFallbackColumn");
                //td.InnerText = value;
                lbl = new Label();
                lbl.Text = value;
                lbl.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
                //lbl.CssClass = "languageManagerKeyColumn";
                td.Controls.Add(lbl);
                tr.Cells.Add(td);

                td = new HtmlTableCell();
                td.Attributes.Add("Class", "languageManagerLanguageColumn");
                lbl = new Label();
                lbl.Text = key;
                //lbl.CssClass = "languageManagerKeyColumn";

                txt = new TextBox();
                txt.ID = "txt" + key;
                txt.Width = new System.Web.UI.WebControls.Unit(98, UnitType.Percentage);
                txt.TextMode = TextBoxMode.MultiLine;
                lbl.Controls.Add(txt);
                td.Controls.Add(lbl);
                //td.Controls.Add(txt);
                tr.Cells.Add(td);

                tblLanguage.Rows.Add(tr);
            }
        }

        /// <summary>
        /// Populate table with the selected language
        /// </summary>
        private void GetLanguage()
        {
            PXWeb.Language.LanguageData langData;
            string key;
            TextBox txt;

            langData = new PXWeb.Language.LanguageData(_languagePath, cboLanguage.SelectedValue);

            foreach (XmlNode node in _xmlnodesFallback)
            {
                key = node.Attributes["name"].Value;

                if (PlaceHolderTable.FindControl("txt" + key) != null)
                {
                    txt = (TextBox)PlaceHolderTable.FindControl("txt" + key);
                    txt.Text = langData.GetSentence(key);
                }
            }
        }

        /// <summary>
        /// Get sentence nodes for the given file
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>List of sentence xml-nodes</returns>
        private XmlNodeList GetXmlNodes(string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmlnodes;

            if (System.IO.File.Exists(path))
            {
                xmldoc.Load(path);
                xmlnodes = xmldoc.GetElementsByTagName("sentence");
                return xmlnodes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Populate dropdown with languages
        /// </summary>
        private void GetCultures()
        {
            cboLanguage.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);
            cboLanguage.DataTextField = "EnglishName";
            cboLanguage.DataValueField = "Name";
            cboLanguage.DataBind();
            sortDropDown(ref cboLanguage, false);

            ListItem li = new ListItem(Master.GetLocalizedString("PxWebAdminToolsLanguageManagerSelectLanguage"), "");
            cboLanguage.Items.Insert(0, li);
        }

        /// <summary> 
        /// Sorts the dropdown in descending order 
        /// </summary> 
        /// <param name="pList">the list to sort</param> 
        /// <param name="pByValue">Sort the list by values or text</param>  
        private void sortDropDown(ref DropDownList pList, bool pByValue)
        {
            SortedList lListItems = new SortedList();
            string key;

            //add listbox items to SortedList  
            foreach (ListItem lItem in pList.Items)
            {
                if (!string.IsNullOrEmpty(lItem.Value))
                {
                    if (pByValue)
                    {
                        key = lItem.Value + " (" + lItem.Text + ")";
                    }
                    else
                    {
                        key = lItem.Text + " (" + lItem.Value + ")";
                    }

                    if (!lListItems.Contains(key))
                    {
                        lListItems.Add(key, lItem);
                    }
                }
            }

            //clear dropdown 
            pList.Items.Clear();

            //add sorted items to dropdown 
            for (int i = 0; i < lListItems.Count; i++)
            {
                pList.Items.Add((ListItem)lListItems[lListItems.GetKey(i)]);
            }
        }

        /// <summary>
        /// Selected language is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLanguage();
        }

        /// <summary>
        /// Validate the Language dropdown
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateLanguage(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminToolsLanguageManagerSelectLanguageError");
                return;
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Show help about the Language Manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LanguageManagerInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminMenuToolsLanguageManager", "PxWebAdminToolsLanguageManagerInfo");
        }

    }
}
