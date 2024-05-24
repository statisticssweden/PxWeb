namespace PXWeb
{
    /// <summary>
    /// Interface for the Selection.Hierarchies settings
    /// </summary>
    public interface IHierarchiesSettings
    {
        /// <summary>
        /// If the hierarchies button shall be displayed or not
        /// </summary>
        bool ShowHierarchies { get; }

        /// <summary>
        /// Specifies number of expanded levels in hierarchical tree by default
        /// </summary>
        int HierarchicalLevelsOpen { get; }

    }
}
