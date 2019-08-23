using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Controls.CommandBar.Plugin;
using System.Web.UI.WebControls;

namespace PCAxis.JsonStat.Web.Controls
{
    public class JsonStatFileTypeCodebehind : FileTypeControlBase<JsonStatFileTypeCodebehind, JsonStatFileType>
    {
        public JsonStatFileTypeCodebehind()
        {
            this.Load += JsonStat_Load;
        }


        /// <summary>
        /// Called when the user control is loaded
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks></remarks>
        private void JsonStat_Load(object sender, System.EventArgs e)
        {
            OnFinished();
            Marker.SerializeAndStream();
        }
    }
}
