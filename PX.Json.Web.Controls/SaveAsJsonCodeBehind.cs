using PCAxis.Web.Core;

namespace PX.Json.Web.Controls
{
    public class SaveAsJsonCodebehind : FileTypeControlBase<SaveAsJsonCodebehind, SaveAsJson>
    {
        public SaveAsJsonCodebehind()
        {
            this.Load += Json_Load;
        }


        /// <summary>
        /// Called when the user control is loaded
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks></remarks>
        private void Json_Load(object sender, System.EventArgs e)
        {
            OnFinished();
            Marker.SerializeAndStream();
        }
    }
}
