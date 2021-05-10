using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXWeb.CustomControls
{
    public class HeadingLabel : System.Web.UI.WebControls.Label
    {
        public enum HeadingLevel
        {
            H1,
            H2,
            H3,
            H4,
            H5,
            H6
        }

        public HeadingLevel Level { get; set; } = HeadingLevel.H1;

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get 
            { 
                switch (Level)
                {
                    case HeadingLevel.H1:
                        return System.Web.UI.HtmlTextWriterTag.H1;
                    case HeadingLevel.H2:
                        return System.Web.UI.HtmlTextWriterTag.H2;
                    case HeadingLevel.H3:
                        return System.Web.UI.HtmlTextWriterTag.H3;
                    case HeadingLevel.H4:
                        return System.Web.UI.HtmlTextWriterTag.H4;
                    case HeadingLevel.H5:
                        return System.Web.UI.HtmlTextWriterTag.H5;
                    case HeadingLevel.H6:
                        return System.Web.UI.HtmlTextWriterTag.H6;
                    default:
                        return System.Web.UI.HtmlTextWriterTag.H1;
                }
            }
        }
    }
}