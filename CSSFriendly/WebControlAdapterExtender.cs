using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSSFriendly
{
    public class WebControlAdapterExtender
    {
        private WebControl _adaptedControl = null;

        private bool _disableAutoAccessKey = false;
                     // used when dealing with things like read-only textboxes that should not have access keys

        public WebControlAdapterExtender(WebControl adaptedControl)
        {
            _adaptedControl = adaptedControl;
        }

        public WebControl AdaptedControl
        {
            get
            {
                Debug.Assert(_adaptedControl != null, "CSS Friendly adapters internal error",
                             "No control has been defined for the adapter extender");
                return _adaptedControl;
            }
        }

        public bool AdapterEnabled
        {
            get
            {
                bool bReturn = true; // normally the adapters are enabled

                //  Individual controls can use the expando property called AdapterEnabled
                //  as a way to turn the adapters off.
                //  <asp:TreeView runat="server" AdapterEnabled="false" />
                if ((AdaptedControl != null) &&
                    (!String.IsNullOrEmpty(AdaptedControl.Attributes["AdapterEnabled"])) &&
                    (AdaptedControl.Attributes["AdapterEnabled"].IndexOf("false", StringComparison.OrdinalIgnoreCase) ==
                     0))
                {
                    bReturn = false;
                }

                return bReturn;
            }
        }

        public bool AutoAccessKey
        {
            get
            {
                //  Individual controls can use the expando property called AdapterEnabled
                //  as a way to turn on/off the heurisitic for automatically setting the AccessKey
                //  attribute in the rendered HTML.  The default is shown below in the initialization
                //  of the bReturn variable.
                //  <asp:TreeView runat="server" AutoAccessKey="false" />

                bool bReturn = true; // by default, the adapter will make access keys are available
                if (_disableAutoAccessKey ||
                    ((AdaptedControl != null) &&
                     (!String.IsNullOrEmpty(AdaptedControl.Attributes["AutoAccessKey"])) &&
                     (AdaptedControl.Attributes["AutoAccessKey"].IndexOf("false", StringComparison.OrdinalIgnoreCase) ==
                      0)))
                {
                    bReturn = false;
                }

                return bReturn;
            }
        }

        public void RegisterScripts()
        {
            /* 
			 * Modified for support of compiled CSSFriendly assembly
			 * 
			 * We will first search for embedded JavaScript files. If they are not
			 * found, we default to the standard approach.
			 */

            Type type = GetType();

            // AdapterUtils.js
            string resource = "CSSFriendly.JavaScript.AdapterUtils.js";
            string filePath = AdaptedControl.Page.ClientScript.GetWebResourceUrl(type, resource);

            // if filePath is empty, use the old approach
            if (String.IsNullOrEmpty(filePath))
            {
                string folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
                if (String.IsNullOrEmpty(folderPath))
                {
                    folderPath = "~/JavaScript";
                }
                filePath = folderPath.EndsWith("/")
                               ? folderPath + "AdapterUtils.js"
                               : folderPath + "/TreeViewAdapter.js";
            }

            if (!AdaptedControl.Page.ClientScript.IsClientScriptIncludeRegistered(type, resource))
                AdaptedControl.Page.ClientScript.RegisterClientScriptInclude(type, resource,
                                                                             AdaptedControl.Page.ResolveUrl(filePath));
        }

        public string ResolveUrl(string url)
        {
            string urlToResolve = url;
            int nPound = url.LastIndexOf("#");
            int nSlash = url.LastIndexOf("/");
            if ((nPound > -1) && (nSlash > -1) && ((nSlash + 1) == nPound))
            {
                //  We have been given a somewhat strange URL.  It has a foreward slash (/) immediately followed
                //  by a pound sign (#) like this xxx/#yyy.  This sort of oddly shaped URL is what you get when
                //  you use named anchors instead of pages in the url attribute of a sitemapnode in an ASP.NET
                //  sitemap like this:
                //
                //  <siteMapNode url="#Introduction" title="Introduction"  description="Introduction" />
                //
                //  The intend of the sitemap author is clearly to create a link to a named anchor in the page
                //  that looks like these:
                //
                //  <a id="Introduction"></a>       (XHTML 1.1 Strict compliant)
                //  <a name="Introduction"></a>     (more old fashioned but quite common in many pages)
                //
                //  However, the sitemap interpretter in ASP.NET doesn't understand url values that start with
                //  a pound.  It prepends the current site's path in front of it before making it into a link
                //  (typically for a TreeView or Menu).  We'll undo that problem, however, by converting this
                //  sort of oddly shaped URL back into what was intended: a simple reference to a named anchor
                //  that is expected to be within the current page.

                urlToResolve = url.Substring(nPound);
            }
            else
            {
                urlToResolve = AdaptedControl.ResolveClientUrl(urlToResolve);
            }

            //  And, just to be safe, we'll make sure there aren't any troublesome characters in whatever URL
            //  we have decided to use at this point.
            string newUrl = AdaptedControl.Page.Server.HtmlEncode(urlToResolve);

            return newUrl;
        }

        public void RaiseAdaptedEvent(string eventName, EventArgs e)
        {
            string attr = "OnAdapted" + eventName;
            if ((AdaptedControl != null) && (!String.IsNullOrEmpty(AdaptedControl.Attributes[attr])))
            {
                string delegateName = AdaptedControl.Attributes[attr];
                Control methodOwner = AdaptedControl.Parent;
                MethodInfo method = methodOwner.GetType().GetMethod(delegateName);
                if (method == null)
                {
                    methodOwner = AdaptedControl.Page;
                    method = methodOwner.GetType().GetMethod(delegateName);
                }
                if (method != null)
                {
                    var args = new object[2];
                    args[0] = AdaptedControl;
                    args[1] = e;
                    method.Invoke(methodOwner, args);
                }
            }
        }

        public void RenderBeginTag(HtmlTextWriter writer, string cssClass)
        {
            string id = (AdaptedControl != null) ? AdaptedControl.ClientID : "";

            if (!String.IsNullOrEmpty(AdaptedControl.Attributes["CssSelectorClass"]))
            {
                WriteBeginDiv(writer, AdaptedControl.Attributes["CssSelectorClass"], id);
                id = "";
            }

            WriteBeginDiv(writer, cssClass, id);
        }

        public void RenderEndTag(HtmlTextWriter writer)
        {
            WriteEndDiv(writer);

            if (!String.IsNullOrEmpty(AdaptedControl.Attributes["CssSelectorClass"]))
            {
                WriteEndDiv(writer);
            }
        }

        public static void RemoveProblemChildren(Control ctrl, List<ControlRestorationInfo> stashedControls)
        {
            RemoveProblemTypes(ctrl.Controls, stashedControls);
        }

        public static void RemoveProblemTypes(ControlCollection coll, List<ControlRestorationInfo> stashedControls)
        {
            foreach (Control ctrl in coll)
            {
                if (typeof (RequiredFieldValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof (CompareValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof (RegularExpressionValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof (ValidationSummary).IsAssignableFrom(ctrl.GetType()))
                {
                    var cri = new ControlRestorationInfo(ctrl, coll);
                    stashedControls.Add(cri);
                    coll.Remove(ctrl);
                    continue;
                }

                if (ctrl.HasControls())
                {
                    RemoveProblemTypes(ctrl.Controls, stashedControls);
                }
            }
        }

        public static void RestoreProblemChildren(List<ControlRestorationInfo> stashedControls)
        {
            foreach (ControlRestorationInfo cri in stashedControls)
            {
                cri.Restore();
            }
        }

        public string MakeChildId(string postfix)
        {
            return AdaptedControl.ClientID + "_" + postfix;
        }

        public static string MakeNameFromId(string id)
        {
            string name = "";
            for (int i = 0; i < id.Length; i++)
            {
                char thisChar = id[i];
                char prevChar = ((i - 1) > -1) ? id[i - 1] : ' ';
                char nextChar = ((i + 1) < id.Length) ? id[i + 1] : ' ';
                if (thisChar == '_')
                {
                    if (prevChar == '_')
                    {
                        name += "_";
                    }
                    else if (nextChar == '_')
                    {
                        name += "$_";
                    }
                    else
                    {
                        name += "$";
                    }
                }
                else
                {
                    name += thisChar;
                }
            }
            return name;
        }

        public static string MakeIdWithButtonType(string id, ButtonType type)
        {
            string idWithType = id;
            switch (type)
            {
                case ButtonType.Button:
                    idWithType += "Button";
                    break;
                case ButtonType.Image:
                    idWithType += "ImageButton";
                    break;
                case ButtonType.Link:
                    idWithType += "LinkButton";
                    break;
            }
            return idWithType;
        }

        public string MakeChildName(string postfix)
        {
            return MakeNameFromId(MakeChildId(postfix));
        }

        public static void WriteBeginDiv(HtmlTextWriter writer, string className, string id)
        {
            writer.WriteLine();
            writer.WriteBeginTag("div");
            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }
            if (!String.IsNullOrEmpty(id))
            {
                writer.WriteAttribute("id", id);
            }
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
        }

        public static void WriteEndDiv(HtmlTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("div");
        }

        public static void WriteSpan(HtmlTextWriter writer, string className, string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                writer.WriteLine();
                writer.WriteBeginTag("span");
                if (!String.IsNullOrEmpty(className))
                {
                    writer.WriteAttribute("class", className);
                }
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(content);
                writer.WriteEndTag("span");
            }
        }

        public static void WriteImage(HtmlTextWriter writer, string url, string alt)
        {
            if (!String.IsNullOrEmpty(url))
            {
                writer.WriteLine();
                writer.WriteBeginTag("img");
                writer.WriteAttribute("src", url);
                writer.WriteAttribute("alt", alt);
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            }
        }

        public static void WriteLink(HtmlTextWriter writer, string className, string url, string title, string content)
        {
            if ((!String.IsNullOrEmpty(url)) && (!String.IsNullOrEmpty(content)))
            {
                writer.WriteLine();
                writer.WriteBeginTag("a");
                if (!String.IsNullOrEmpty(className))
                {
                    writer.WriteAttribute("class", className);
                }
                writer.WriteAttribute("href", url);
                writer.WriteAttribute("title", title);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(content);
                writer.WriteEndTag("a");
            }
        }

        //  Can't be static because it uses MakeChildId
        public void WriteLabel(HtmlTextWriter writer, string className, string text, string forId)
        {
            if (!String.IsNullOrEmpty(text))
            {
                writer.WriteLine();
                writer.WriteBeginTag("label");
                writer.WriteAttribute("for", MakeChildId(forId));
                if (!String.IsNullOrEmpty(className))
                {
                    writer.WriteAttribute("class", className);
                }
                writer.Write(HtmlTextWriter.TagRightChar);

                if (AutoAccessKey)
                {
                    writer.WriteBeginTag("em");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(text[0].ToString());
                    writer.WriteEndTag("em");
                    if (!String.IsNullOrEmpty(text))
                    {
                        writer.Write(text.Substring(1));
                    }
                }
                else
                {
                    writer.Write(text);
                }

                writer.WriteEndTag("label");
            }
        }

        //  Can't be static because it uses MakeChildId
        public void WriteTextBox(HtmlTextWriter writer, bool isPassword, string labelClassName, string labelText,
                                 string inputClassName, string id, string value)
        {
            WriteLabel(writer, labelClassName, labelText, id);

            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", isPassword ? "password" : "text");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }
            writer.WriteAttribute("id", MakeChildId(id));
            writer.WriteAttribute("name", MakeChildName(id));
            writer.WriteAttribute("value", value);
            if (AutoAccessKey && (!String.IsNullOrEmpty(labelText)))
            {
                writer.WriteAttribute("accesskey", labelText[0].ToString().ToLower());
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
        }

        //  Can't be static because it uses MakeChildId
        public void WriteReadOnlyTextBox(HtmlTextWriter writer, string labelClassName, string labelText,
                                         string inputClassName, string value)
        {
            bool oldDisableAutoAccessKey = _disableAutoAccessKey;
            _disableAutoAccessKey = true;

            WriteLabel(writer, labelClassName, labelText, "");

            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("readonly", "readonly");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }
            writer.WriteAttribute("value", value);
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            _disableAutoAccessKey = oldDisableAutoAccessKey;
        }

        //  Can't be static because it uses MakeChildId
        public void WriteCheckBox(HtmlTextWriter writer, string labelClassName, string labelText, string inputClassName,
                                  string id, bool isChecked)
        {
            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "checkbox");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }
            writer.WriteAttribute("id", MakeChildId(id));
            writer.WriteAttribute("name", MakeChildName(id));
            if (isChecked)
            {
                writer.WriteAttribute("checked", "checked");
            }
            if (AutoAccessKey && (!String.IsNullOrEmpty(labelText)))
            {
                writer.WriteAttribute("accesskey", labelText[0].ToString());
            }
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            WriteLabel(writer, labelClassName, labelText, id);
        }

        //  Can't be static because it uses MakeChildId
        public void WriteSubmit(HtmlTextWriter writer, ButtonType buttonType, string className, string id,
                                string imageUrl, string javascript, string text)
        {
            writer.WriteLine();

            string idWithType = id;

            switch (buttonType)
            {
                case ButtonType.Button:
                    writer.WriteBeginTag("input");
                    writer.WriteAttribute("type", "submit");
                    writer.WriteAttribute("value", text);
                    idWithType += "Button";
                    break;
                case ButtonType.Image:
                    writer.WriteBeginTag("input");
                    writer.WriteAttribute("type", "image");
                    writer.WriteAttribute("src", imageUrl);
                    idWithType += "ImageButton";
                    break;
                case ButtonType.Link:
                    writer.WriteBeginTag("a");
                    idWithType += "LinkButton";
                    break;
            }

            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }
            writer.WriteAttribute("id", MakeChildId(idWithType));
            writer.WriteAttribute("name", MakeChildName(idWithType));

            if (!String.IsNullOrEmpty(javascript))
            {
                string pureJS = javascript;
                if (pureJS.StartsWith("javascript:"))
                {
                    pureJS = pureJS.Substring("javascript:".Length);
                }
                switch (buttonType)
                {
                    case ButtonType.Button:
                        writer.WriteAttribute("onclick", pureJS);
                        break;
                    case ButtonType.Image:
                        writer.WriteAttribute("onclick", pureJS);
                        break;
                    case ButtonType.Link:
                        writer.WriteAttribute("href", javascript);
                        break;
                }
            }

            if (buttonType == ButtonType.Link)
            {
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(text);
                writer.WriteEndTag("a");
            }
            else
            {
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            }
        }

        public static void WriteRequiredFieldValidator(HtmlTextWriter writer, RequiredFieldValidator rfv,
                                                       string className, string controlToValidate, string msg)
        {
            if (rfv != null)
            {
                rfv.CssClass = className;
                rfv.ControlToValidate = controlToValidate;
                rfv.ErrorMessage = msg;
                rfv.RenderControl(writer);
            }
        }

        public static void WriteRegularExpressionValidator(HtmlTextWriter writer, RegularExpressionValidator rev,
                                                           string className, string controlToValidate, string msg,
                                                           string expression)
        {
            if (rev != null)
            {
                rev.CssClass = className;
                rev.ControlToValidate = controlToValidate;
                rev.ErrorMessage = msg;
                rev.ValidationExpression = expression;
                rev.RenderControl(writer);
            }
        }

        public static void WriteCompareValidator(HtmlTextWriter writer, CompareValidator cv, string className,
                                                 string controlToValidate, string msg, string controlToCompare)
        {
            if (cv != null)
            {
                cv.CssClass = className;
                cv.ControlToValidate = controlToValidate;
                cv.ErrorMessage = msg;
                cv.ControlToCompare = controlToCompare;
                cv.RenderControl(writer);
            }
        }

        public static void WriteTargetAttribute(HtmlTextWriter writer, string targetValue)
        {
            if ((writer != null) && (!String.IsNullOrEmpty(targetValue)))
            {
                //  If the targetValue is _blank then we have an opportunity to use attributes other than "target"
                //  which allows us to be compliant at the XHTML 1.1 Strict level. Specifically, we can use a combination
                //  of "onclick" and "onkeypress" to achieve what we want to achieve when we used to render
                //  target='blank'.
                //
                //  If the targetValue is other than _blank then we fall back to using the "target" attribute.
                //  This is a heuristic that can be refined over time.
                if (targetValue.Equals("_blank", StringComparison.OrdinalIgnoreCase))
                {
                    string js = "window.open(this.href, '_blank', ''); return false;";
                    writer.WriteAttribute("onclick", js);
                    writer.WriteAttribute("onkeypress", js);
                }
                else
                {
                    writer.WriteAttribute("target", targetValue);
                }
            }
        }
    }

    public class ControlRestorationInfo
    {
        private ControlCollection _coll = null;
        private Control _ctrl = null;

        public ControlRestorationInfo(Control ctrl, ControlCollection coll)
        {
            _ctrl = ctrl;
            _coll = coll;
        }

        public Control Control
        {
            get { return _ctrl; }
        }

        public ControlCollection Collection
        {
            get { return _coll; }
        }

        public bool IsValid
        {
            get { return (Control != null) && (Collection != null); }
        }

        public void Restore()
        {
            if (IsValid)
            {
                _coll.Add(_ctrl);
            }
        }
    }
}