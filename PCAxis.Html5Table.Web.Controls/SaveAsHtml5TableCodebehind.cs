using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Controls.CommandBar.Plugin;
using System.Web.UI.WebControls;

namespace PCAxis.Html5Table.Web.Controls
{
    public class SaveAsHtml5TableCodebehind : FileTypeControlBase<SaveAsHtml5TableCodebehind, SaveAsHtml5Table>
    {
        public SaveAsHtml5TableCodebehind()
        {
            this.Load += Html5Table_Load;
        }


        /// <summary>
        /// Called when the user control is loaded
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks></remarks>
        private void Html5Table_Load(object sender, System.EventArgs e)
        {
            OnFinished();
            Marker.SerializeAndStream();
        }
    }
}
