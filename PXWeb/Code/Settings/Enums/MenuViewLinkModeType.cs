namespace PXWeb
{
    /// <summary>
    /// Describes what action that will take place when the user clicks the "View" link for a table in Menu
    /// and are associated with the setting Menu.ViewLinkMode
    /// </summary>
    public enum MenuViewLinkModeType
    {
        /// <summary>
        /// The "View" link will be hidden
        /// </summary>
        Hidden,
        /// <summary>
        /// When the user clicks the "View" link the default values will be displayed in the table.
        /// Number of values are defined by the setting Menu.NumberOfValuesInDefaultView.
        /// </summary>
        DefaultValues,
        /// <summary>
        /// All values will be displayed in the table
        /// </summary>
        AllValues
    }
}
