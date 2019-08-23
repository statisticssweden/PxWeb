Namespace CommandBar.Plugin
    ''' <summary>
    ''' Defines where a shortcut should be placed
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CommandBarShortcutPlacementType
        ''' <summary>
        ''' The shortcut should be places under the functions <see  cref="System.Web.UI.WebControls.DropDownList" />
        ''' </summary>
        ''' <remarks></remarks>
        [Function]
        ''' <summary>
        ''' The shortcut should be places under the saveas <see  cref="System.Web.UI.WebControls.DropDownList" />
        ''' </summary>
        ''' <remarks></remarks>
        SaveAs
        ''' <summary>
        ''' The shortcut should be places under the links <see  cref="System.Web.UI.WebControls.DropDownList" />
        ''' </summary>
        ''' <remarks></remarks>
        Link
        ''' <summary>
        ''' The shortcut should be places to the right of the commandbars <see  cref="System.Web.UI.WebControls.DropDownList" />
        ''' </summary>
        ''' <remarks></remarks>
        CommandBar
    End Enum
End Namespace

