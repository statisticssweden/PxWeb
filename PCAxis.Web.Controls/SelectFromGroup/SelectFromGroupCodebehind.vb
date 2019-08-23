
Imports System.Web.UI
Imports PCAxis.Enums
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Management

''' <summary>
''' Showing information
''' </summary>
Public Class SelectFromGroupCodebehind
    Inherits PaxiomControlBase(Of SelectFromGroupCodebehind, SelectFromGroup)

#Region "Fields"
    Protected lblHeading As Label
    Protected lblVariable As Label
    Protected WithEvents cboGrouping As DropDownList
    Protected WithEvents rblType As RadioButtonList
    Protected pnlGroups As Panel
    Protected lblGroups As Label
    Protected lstGroups As ListBox
    Protected WithEvents btnSelectGroups As Button
    Protected pnlValues As Panel
    Protected lblValues As Label
    Protected lstValues As ListBox
    Protected WithEvents btnSelectionDone As Button
    Protected WithEvents btnCancel As Button
#End Region

    ''' <summary>
    ''' Initiation of the select control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitiateSearch()
        Dim li As ListItem

        If Not Marker.Variable.HasGroupings() Then
            Return
        End If

        cboGrouping.Items.Clear()
        lstGroups.Items.Clear()
        lstValues.Items.Clear()

        lblHeading.Text = Me.GetLocalizedString("CtrlSelectFromGroupHeading")
        lblVariable.Text = Marker.Variable.Name

        li = New ListItem(Me.GetLocalizedString("CtrlSelectFromGroupSelectGrouping"), "")
        li.Selected = True
        If Not cboGrouping.Items.Contains(li) Then
            cboGrouping.Items.Add(li)
        End If
        For Each grpInfo As PCAxis.Paxiom.GroupingInfo In Marker.Variable.Groupings
            li = New ListItem(grpInfo.Name.Trim(), grpInfo.ID)
            If Not cboGrouping.Items.Contains(li) Then
                cboGrouping.Items.Add(li)
            End If
        Next

        rblType.Items.Clear()
        li = New ListItem(Me.GetLocalizedString("CtrlSelectFromGroupShowAggregatedValues"), "aggregated")
        If (Not PaxiomManager.PaxiomModel.Meta.AggregAllowed) Then
            li.Enabled = False
        End If
        rblType.Items.Add(li)

        li = New ListItem(Me.GetLocalizedString("CtrlSelectFromGroupShowSingleValues"), "single")
        li.Selected = True
        rblType.Items.Add(li)

        lblGroups.Text = Me.GetLocalizedString("CtrlSelectFromGroupGroups")
        If rblType.SelectedValue = "aggregated" Then
            btnSelectGroups.Text = Me.GetLocalizedString("CtrlSelectFromGroupSelectGroupsAggregated")
        Else
            btnSelectGroups.Text = Me.GetLocalizedString("CtrlSelectFromGroupSelectGroupsSingle")
        End If


        lblValues.Text = Me.GetLocalizedString("CtrlSelectFromGroupValues")
        btnSelectionDone.Text = Me.GetLocalizedString("CtrlSelectFromGroupSelectAndReturn")
        btnCancel.Text = Me.GetLocalizedString("CtrlSelectFromGroupCancel")

        btnSelectionDone.Enabled = False
    End Sub

    Protected Sub btnSelectionDone_Click(sender As Object, e As EventArgs) Handles btnSelectionDone.Click
        Dim variable As Variable = CType(Session("SelectFromGroupVariable"), Variable)

        Dim selection As New Selection(variable.Code)
        For Each item As ListItem In lstValues.Items
            If Not String.IsNullOrEmpty(item.Value) AndAlso item.Selected Then
                selection.ValueCodes.Add(item.Value)
            End If
        Next

        VariableSelector.SelectedVariableValues(variable.Code) = selection

        Dim args As New SelectFromGroupEventArgs()
        args.VariableCode = variable.Code
        args.Aggregation = variable.CurrentGrouping.ID
        If cboGrouping.SelectedIndex > 0 Then
            If rblType.SelectedValue.Equals("single") Then
                args.Includes = GroupingIncludesType.SingleValues
            Else
                args.Includes = GroupingIncludesType.AggregatedValues
            End If
        End If

        Marker.OnSelectionDone(args)
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Marker.OnSelectionDone(New SelectFromGroupEventArgs())
    End Sub

    Private Sub cboGrouping_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGrouping.SelectedIndexChanged
        SelectGrouping()
    End Sub

    Protected Sub btnSelectGroups_Click(sender As Object, e As EventArgs) Handles btnSelectGroups.Click
        Dim variable As Variable = CType(Session("SelectFromGroupVariable"), Variable)
        Dim itm As ListItem

        If variable Is Nothing Then
            Return
        End If

        For Each liGroup As ListItem In lstGroups.Items
            If liGroup.Selected Then
                For Each grp As Group In variable.CurrentGrouping.Groups
                    If grp.GroupCode.Equals(liGroup.Value) Then
                        If rblType.SelectedValue.Equals("single") Then
                            For Each val As GroupChildValue In grp.ChildCodes
                                itm = New ListItem(variable.Values.GetByCode(val.Code).Text, val.Code)
                                itm.Selected = True
                                lstValues.Items.Add(itm)
                            Next
                        Else
                            itm = New ListItem(variable.Values.GetByCode(grp.GroupCode).Text, grp.GroupCode)
                            itm.Selected = True
                            lstValues.Items.Add(itm)
                        End If
                    End If
                Next
            End If
        Next

        If lstValues.Items.Count > 0 Then
            btnSelectionDone.Enabled = True
        End If
    End Sub

    Private Sub rblType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblType.SelectedIndexChanged
        If rblType.SelectedValue = "aggregated" Then
            btnSelectGroups.Text = Me.GetLocalizedString("CtrlSelectFromGroupSelectGroupsAggregated")
        Else
            btnSelectGroups.Text = Me.GetLocalizedString("CtrlSelectFromGroupSelectGroupsSingle")
        End If

        SelectGrouping()
    End Sub

    Private Sub SelectGrouping()
        Dim workVariable As PCAxis.Paxiom.Variable 'Working-copy of the variable
        Dim grpInfo As PCAxis.Paxiom.GroupingInfo
        Dim grpType As PCAxis.Paxiom.GroupingIncludesType

        If cboGrouping.SelectedIndex > 0 Then
            If rblType.SelectedValue.Equals("single") Then
                grpType = GroupingIncludesType.SingleValues
            Else
                grpType = GroupingIncludesType.AggregatedValues
            End If

            grpInfo = Marker.Variable.GetGroupingInfoById(cboGrouping.SelectedValue)

            If Not grpInfo Is Nothing Then
                'ore.Management.PaxiomManager.PaxiomModelBuilder.ApplyGrouping(Marker.Variable.Code, grpInfo, grpType)
                Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyGrouping(Marker.Variable.Code, grpInfo, GroupingIncludesType.All) 'Todo test piv


                'Create a working-copy of the variable and store in session. If we do not do this the selected grouping will be overwritten...
                workVariable = Marker.Variable.CreateCopy()
                Session("SelectFromGroupVariable") = workVariable

                lstGroups.Items.Clear()
                lstValues.Items.Clear()

                For Each group As PCAxis.Paxiom.Group In workVariable.CurrentGrouping.Groups
                    Dim li As ListItem = New ListItem(workVariable.Values.GetByCode(group.GroupCode).Text, group.GroupCode)
                    lstGroups.Items.Add(li)
                Next

            End If

            btnSelectionDone.Enabled = False
        End If

    End Sub
End Class