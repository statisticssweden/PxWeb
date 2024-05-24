using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Presentation.CommandBar settings
    /// </summary>
    public interface ICommandBarSettings
    {
        /// <summary>
        /// View mode of the CommandBar
        /// </summary>
        PCAxis.Web.Controls.CommandBar.CommandBarViewMode ViewMode { get; }

        /// <summary>
        /// Plugins displayed in the Operations dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> Operations { get; }

        /// <summary>
        /// Shortcut buttons displayed under the operations dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> OperationShortcuts { get; }

        /// <summary>
        /// Fileformats displayed in the fileformat dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> OutputFormats { get; }

        /// <summary>
        /// Shortcut buttons displayed under the fileformat dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> OutputFormatShortcuts { get; }

        /// <summary>
        /// Plugins displayed in the presentation views dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> PresentationViews { get; }

        /// <summary>
        /// Shortcut buttons displayed under the presentation views dropdown when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> PresentationViewShortcuts { get; }

        /// <summary>
        /// Shortcut buttons displayed to the right of the dropdowns when CommandBar is in ViewMode = DropDown
        /// </summary>
        IEnumerable<string> CommandBarShortcuts { get; }

        /// <summary>
        /// Operation buttons displayed when CommandBar is in ViewMode = Buttons
        /// </summary>
        IEnumerable<string> OperationButtons { get; }

        /// <summary>
        /// Filetype buttons displayed when CommandBar is in ViewMode = Buttons
        /// </summary>
        IEnumerable<string> FileTypeButtons { get; }

        /// <summary>
        /// Presentation view buttons displayed when CommandBar is in ViewMode = Buttons
        /// </summary>
        IEnumerable<string> PresentationViewButtons { get; }
    }
}
