using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Selection.MarkingTips settings
    /// </summary>
    public interface IMarkingTipsSettings
    {
        /// <summary>
        /// If marking tips shall be displayed or not
        /// </summary>
        bool ShowMarkingTips { get; }

        ///// <summary>
        ///// URL to the marking tips 
        ///// </summary>
        //string MarkingTipsUrl { get; }

    }
}
