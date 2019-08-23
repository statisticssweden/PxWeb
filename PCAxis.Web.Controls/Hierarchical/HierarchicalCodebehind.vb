

Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.StateProvider

''' <summary>
''' Present dialogue for selecting hierarchical variables
''' Requests existence of MetaModel, variablename to select as querystring (and previous selections done)
''' Returns new selection
''' </summary>

Public Class HierarchicalCodebehind
    Inherits PaxiomControlBase(Of HierarchicalCodebehind, Hierarchical)

    Protected _variable As Variable
    Protected _selectedvariables As Selection


#Region " Localized strings "

    Private Const LOC_SELECTALL_BUTTON As String = "CtrlHierarchicalVariableSelectAllButton"
    Private Const LOC_UNSELECTALL_BUTTON As String = "CtrlHierarchicalVariableUnselectAllButton"
    Private Const LOC_OPENALLNODES_BUTTON As String = "CtrlHierarchicalVariableOpenAllNodesButton"
    Private Const LOC_CLOSEALLNODES_BUTTON As String = "CtrlHierarchicalVariableCloseAllNodesButton"
    Private Const LOC_CONTINUE_BUTTON As String = "CtrlHierarchicalVariableContinueButtonText"
    Private Const LOC_ERROR_LABELTEXT As String = "CtrlHierarchicalVariableErrorMessage"

#End Region

#Region " Controls "

    Protected ErrorPanel As Panel
    Protected ErrorMessage As Label
    Protected CollapseTreeScriptPanel As Panel
    Protected HierarchicalVariableSelectPanel As Panel
    Protected WithEvents VariableTreeView As TreeView
    Protected VariableNameLabel As Label
    Protected WithEvents SelectAllLabel As Label
    Protected WithEvents SelectAllButton As ImageButton
    Protected WithEvents UnselectAllLabel As Label
    Protected WithEvents UnSelectAllButton As ImageButton
    Protected WithEvents OpenAllNodesLabel As Label
    Protected WithEvents OpenAllNodesButton As ImageButton
    Protected WithEvents CloseAllNodesLabel As Label
    Protected WithEvents CloseAllNodesButton As ImageButton
    Protected WithEvents ContinueButton As Button

#End Region


    Public Sub Hierarchical_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VariableTreeView.ExpandDepth = Marker.HierarchyLevelsOpen
        HandleButtons()
        Page.ClientScript.RegisterClientScriptBlock(GetType(Page), "treeCollapse", "var level =" & Marker.HierarchyLevelsOpen.ToString & ";", True)
    End Sub

    ''' <summary>
    ''' Render treeview for valueselection for variable
    ''' </summary>
    ''' <remarks>Consider previous selections done for variable if saved in state-variabel</remarks>
    Friend Sub RenderHierarchicalTree()

        If Marker.Variable IsNot Nothing Then

            VariableNameLabel.Text = Marker.Variable.Name

            Me.VariableTreeView.Nodes.Clear()
            Dim TreeStartNode As TreeNode

            'For Each hierarchyLevel As HierarchyLevel In Marker.Variable.Hierarchy.Children
            '    TreeStartNode = New TreeNode(GetTreeNodeName(hierarchyLevel.Code.ToString()))
            '    TreeStartNode.Text = GetTreeNodeName(hierarchyLevel.Code.ToString)
            '    TreeStartNode.Value = GetTreeNodeValue(hierarchyLevel.Code.ToString)
            '    If (IsPreviouslySelected(TreeStartNode.Value)) Then
            '        TreeStartNode.Checked = True
            '    End If
            '    If (Marker.HierarchyLevelsOpen = 1) Then
            '        TreeStartNode.Expand()
            '    End If
            '    TreeStartNode.ChildNodes.Add(TreeStartNode)
            '    PopulateTreeView(TreeStartNode, hierarchyLevel)
            '    Me.VariableTreeView.Nodes.Add(TreeStartNode)
            'Next

            TreeStartNode = New TreeNode(GetTreeNodeName(Marker.Variable.Hierarchy.RootLevel.Code.ToString()))
            TreeStartNode.Text = GetTreeNodeName(Marker.Variable.Hierarchy.RootLevel.Code.ToString)
            TreeStartNode.Value = GetTreeNodeValue(Marker.Variable.Hierarchy.RootLevel.Code.ToString)
            If (IsPreviouslySelected(TreeStartNode.Value)) Then
                TreeStartNode.Checked = True
            End If
            If (Marker.HierarchyLevelsOpen = 1) Then
                TreeStartNode.Expand()
            End If
            TreeStartNode.ChildNodes.Add(TreeStartNode)
            PopulateTreeView(TreeStartNode, Marker.Variable.Hierarchy.RootLevel)
            Me.VariableTreeView.Nodes.Add(TreeStartNode)


            VariableTreeView.DataBind()
        Else
            ErrorMessage.Text = GetLocalizedString(LOC_ERROR_LABELTEXT)
            ErrorPanel.Visible = True
            HierarchicalVariableSelectPanel.Visible = False
        End If

        If (Marker.HierarchyLevelsOpen = 0) Then
            VariableTreeView.ExpandAll()
        End If

    End Sub

    ''' <summary>
    ''' Decides if any selection has been done for a variable value in previous steps
    ''' </summary>
    ''' <param name="Treenode"></param>
    ''' <returns>Returns true if selection has previously been done for variablevalue</returns>
    ''' <remarks></remarks>
    Private Function IsPreviouslySelected(ByVal Treenode As String) As Boolean

        Return VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Contains(Treenode)

    End Function

    ''' <summary>
    ''' Render buttons for selection and continue actions
    ''' </summary>
    ''' <remarks>Consider previous selections done for variable if saved in state-variabel</remarks>
    Private Sub HandleButtons()
        'Set ImageUrl for buttons from embedded resource
        Dim imgurl As String = Page.ClientScript.GetWebResourceUrl(GetType(VariableSelectorValueSelectCodebehind), "PCAxis.Web.Controls.spacer.gif")
        SelectAllButton.ImageUrl = imgurl
        UnSelectAllButton.ImageUrl = imgurl
        OpenAllNodesButton.ImageUrl = imgurl
        CloseAllNodesButton.ImageUrl = imgurl

        SelectAllButton.AlternateText = Me.GetLocalizedString(LOC_SELECTALL_BUTTON)
        UnSelectAllButton.AlternateText = Me.GetLocalizedString(LOC_UNSELECTALL_BUTTON)
        OpenAllNodesButton.AlternateText = Me.GetLocalizedString(LOC_OPENALLNODES_BUTTON)
        CloseAllNodesButton.AlternateText = Me.GetLocalizedString(LOC_CLOSEALLNODES_BUTTON)

        If (Marker.ShowButtonLabels) Then
            SelectAllLabel.Text = Me.GetLocalizedString(LOC_SELECTALL_BUTTON)
            UnselectAllLabel.Text = Me.GetLocalizedString(LOC_UNSELECTALL_BUTTON)
            OpenAllNodesLabel.Text = Me.GetLocalizedString(LOC_OPENALLNODES_BUTTON)
            CloseAllNodesLabel.Text = Me.GetLocalizedString(LOC_CLOSEALLNODES_BUTTON)
        End If

        ContinueButton.Text = Me.GetLocalizedString(LOC_CONTINUE_BUTTON)
    End Sub

    ''' <summary>
    ''' Adds nodes to treeview for hierarchical selection
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateTreeView(ByVal startnode As TreeNode, ByVal hierarchy As HierarchyLevel)
        Dim childNode As TreeNode

        For Each hierarchyLevel As HierarchyLevel In hierarchy.Children
            childNode = New TreeNode(GetTreeNodeName(hierarchyLevel.Code.ToString()))
            childNode.Text = GetTreeNodeName(hierarchyLevel.Code.ToString)
            childNode.Value = GetTreeNodeValue(hierarchyLevel.Code.ToString)
            If (IsPreviouslySelected(childNode.Value)) Then
                childNode.Checked = True
            End If
            startnode.ChildNodes.Add(childNode)
            PopulateTreeView(childNode, hierarchyLevel)
        Next
    End Sub

    ''' <summary>
    ''' Get nodename as string from variable (by comparing codevalue in hierarchylevel and variableValues)
    ''' </summary>
    ''' <returns>Nodename as String</returns>
    ''' <remarks></remarks>
    Private Function GetTreeNodeName(ByVal hierarchyLevelCode As String) As String

        Dim TreeNodeName As String = hierarchyLevelCode

        For Each _variablevalue As Value In Marker.Variable.Values
            If _variablevalue.Code.ToString = hierarchyLevelCode Then
                TreeNodeName = _variablevalue.Value.ToString
            End If
        Next

        Return TreeNodeName
    End Function

    ''' <summary>
    ''' Get code as string from variable (by comparing codevalue in hierarchylevel and variableValues)
    ''' </summary>
    ''' <returns>Nodename as String</returns>
    ''' <remarks></remarks>
    Private Function GetTreeNodeValue(ByVal hierarchyLevelCode As String) As String

        Dim TreeNodeValue As String = hierarchyLevelCode

        For Each _variablevalue As Value In Marker.Variable.Values
            If _variablevalue.Code.ToString = hierarchyLevelCode Then
                TreeNodeValue = _variablevalue.Code.ToString
            End If
        Next

        Return TreeNodeValue
    End Function


    ''' <summary>
    ''' Change selection in treeview according to input parameter
    ''' </summary>
    ''' <param name="ckecked">True if node to be set to cecked, false otherwise</param>
    ''' <param name="TreeStartNode">Startnode, under with changes in selection should be made for</param>
    ''' <remarks></remarks>
    Protected Sub ChangeSelection(ByVal TreeStartNode As TreeNode, ByVal ckecked As Boolean)
        For Each treenode As TreeNode In TreeStartNode.ChildNodes
            treenode.Checked = ckecked
            ChangeSelection(treenode, ckecked)
        Next
    End Sub



    ''' <summary>
    ''' Add node to valueselection for variable if selected in hierarchical selection treeview
    ''' </summary>
    ''' <param name="treenode">Starttreenode under with selection should be updated </param>
    ''' <remarks></remarks>
    Private Sub AddToSelection(ByVal treenode As TreeNode)

        If (treenode.Checked) Then
            VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Add(treenode.Value)
        End If

        For Each node As TreeNode In treenode.ChildNodes
            AddToSelection(node)
        Next

    End Sub

    ''' <summary>
    ''' Handles Select all button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SelectAllButton_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SelectAllButton.Click
        For Each treenode As TreeNode In VariableTreeView.Nodes
            treenode.Checked = True
            ChangeSelection(treenode, True)
        Next
    End Sub
    ''' <summary>
    ''' Handles unselect all button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UnSelectAllButton_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UnSelectAllButton.Click
        For Each treenode As TreeNode In VariableTreeView.Nodes
            treenode.Checked = False
            ChangeSelection(treenode, False)
        Next
    End Sub
    ''' <summary>
    ''' Handles event Open all button clicked
    ''' </summary>
    ''' <remarks>expand all nodes in treeview</remarks>
    Private Sub OpenAllNodesButton_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles OpenAllNodesButton.Click
        CollapseTreeScriptPanel.Visible = False
        VariableTreeView.ExpandAll()
    End Sub
    ''' <summary>
    ''' Hndles event Close all button clicked
    ''' </summary>
    ''' <remarks>Collapse all nodes in treeview</remarks>
    Private Sub CloseAllNodesButton_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CloseAllNodesButton.Click
        CollapseTreeScriptPanel.Visible = False
        VariableTreeView.CollapseAll()
    End Sub


    ''' <summary>
    ''' Handles event Continue button clicked
    ''' </summary>
    ''' <remarks>Hierarchical selection done, casts event</remarks>
    Private Sub ContinueButton_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        VariableSelector.SelectedVariableValues(Marker.Variable.Code) = New Selection(Marker.Variable.Code)

        For Each treenode As TreeNode In VariableTreeView.Nodes
            AddToSelection(treenode)
        Next

        Marker.OnSelectionsDone()
    End Sub
End Class
