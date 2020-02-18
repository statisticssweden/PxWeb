using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Selection settings
    /// </summary>
    public interface ISelectionSettings
    {
        /// <summary>
        /// The maximum number of cells that can be displayed on screen
        /// </summary>
        int CellLimitScreen { get; }

        /// <summary>
        /// If the mandatory variable mark shall be displayed or not
        /// </summary>
        bool ShowMandatoryMark { get; }

        /// <summary>
        /// Show selection limits or not
        /// </summary>
        bool ShowSelectionLimits { get; }

        /// <summary>
        /// If aggregations are allowed or not
        /// </summary>
        bool AllowAggregations { get; }

        /// <summary>
        /// Hierarchies settings
        /// </summary>
        IHierarchiesSettings Hierarchies { get; }

        /// <summary>
        /// MarkingTips settings
        /// </summary>
        IMarkingTipsSettings MarkingTips { get; }

        /// <summary>
        /// Maximum number of values displayed for a variable in selection list
        /// </summary>
        int MaxRowsWithoutSearch { get; }

        /// <summary>
        /// Show all values for timevariables even if there are more then MaxRowsWithoutSearch gives
        /// </summary>
        bool AlwaysShowTimeVariableWithoutSearch { get; }

        /// <summary>
        /// Number of rows in the variable selection listboxes 
        /// </summary>
        int ListSize { get; }

        /// <summary>
        /// Available presentation views in the VariableSelector
        /// </summary>
        IEnumerable<string> PresentationViews { get; }

        /// <summary>
        /// Available output formats (file formats) in VariableSelector
        /// </summary>
        IEnumerable<string> OutputFormats { get; }

        /// <summary>
        /// If valueset has to be selected before selection of values can be done
        /// </summary>
        bool ValuesetMustBeSelectedFirst { get; }

        /// <summary>
        /// If the "Select all available values" button shall be displayed in the "Search values" view
        /// </summary>
        bool ShowAllAvailableValuesSearchButton { get; }

        /// <summary>
        /// Controls how the search values button shall be displayed
        /// </summary>
        PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode SearchButtonMode { get; }

        /// <summary>
        /// If table title shall be displayed in the same way it was displayed in the menu or not
        /// </summary>
        bool TitleFromMenu { get; }

        /// <summary>
        /// If the meta title shown in browser tab should be standard fixed
        /// </summary>
        bool StandardApplicationHeadTitle { get; }

        /// <summary>
        /// If table metadata (footnotes and informations shall be displayed as links or directly within the variable selection page)
        /// </summary>
        bool MetadataAsLinks { get; }

        /// <summary>
        /// If it shoud be possible to select singel values from group if the variable has a grouping. 
        /// </summary>
        bool SelectValuesFromGroup { get; }

        /// <summary>
        /// If buttons (Select all, deselect all, sort ascending...) shall be displayed for content variables or not
        /// </summary>
        bool ButtonsForContentVariable { get; }

        /// <summary>
        /// Decides default search option. If false the search is a inside text search
        /// </summary>
        bool SearchValuesBeginningOfWordCheckBoxDefaultChecked { get; }

        /// <summary>
        /// Decides if first value for content and time are pre selected
        /// </summary>
        bool PreSelectFirstContentAndTime { get; }
    }
}
