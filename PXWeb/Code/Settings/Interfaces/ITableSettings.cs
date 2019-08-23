using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Presentation.Table settings
    /// </summary>
    public interface ITableSettings
    {
        /// <summary>
        /// Automatical table transformation
        /// </summary>
        PCAxis.Web.Controls.TableTransformationType TableTransformation { get; }

        /// <summary>
        /// The default layout of the table
        /// </summary>
        PCAxis.Web.Controls.TableLayoutType DefaultLayout { get; }

        /// <summary>
        /// The maximum number of columns that will be displayed in the table
        /// </summary>
        int MaxColumns { get; }

        /// <summary>
        /// The maximum number of rows that will be displayed in the table
        /// </summary>
        int MaxRows { get; }

        /// <summary>
        /// If the table title shall be displayed in the table or not
        /// </summary>
        bool TitleVisible { get; }

        /// <summary>
        /// Attribute settings
        /// </summary>
        IAttributeSettings Attributes { get; }

    }
}
