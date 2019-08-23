using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Metadata
{
    /// <summary>
    /// Link to a metadata system
    /// </summary>
    public class MetaLink
    {
        /// <summary>
        /// Name of the metadata system
        /// </summary>
        public string System { get; set; }
        
        /// <summary>
        /// Link text
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Link (URL)
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Hyperlink target (For example _blank)
        /// </summary>
        public string Target { get; set; }
    }
}
