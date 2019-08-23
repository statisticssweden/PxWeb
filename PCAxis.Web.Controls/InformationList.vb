Imports PCAxis.Enums
Imports PCAxis.Paxiom
Imports System.Text.RegularExpressions

Public Class InformationList
    ''' <summary>
    ''' Gets a list with all information types. Used by the information webcontrol.
    ''' </summary>
    ''' <param name="informationTypes">A list with all information types to return informationfor.</param>
    ''' <param name="contactForEveryContent">If contact information shall be displayed for every content or not when there are more than one content. If not contact information is taken from the first content</param>
    ''' <returns>All information for all specified information types</returns>
    ''' <param name="lastUpdatedForEveryContent">If last updated information shall be displayed for every content or not when there are more than one content. If not last updated information is taken from the first content</param>
    Public Shared Function GetInformationList(ByVal meta As PXMeta, ByVal informationTypes As List(Of Enums.InformationType), ByVal contactForEveryContent As Boolean, ByVal lastUpdatedForEveryContent As Boolean) As List(Of InformationListItem)
        Dim informationList As New List(Of InformationListItem)
        Dim contactAdded As Boolean = False
        Dim lastUpdatedAdded As Boolean = False
        'If informationList.Count = 0 Then
        For Each informationType As Enums.InformationType In informationTypes
            Dim li As InformationListItem = New InformationListItem(informationType)
            Dim hasVariables As Boolean = informationType <= Enums.InformationType.BasePeriod
            If meta.ContentVariable IsNot Nothing AndAlso meta.ContentVariable.Values.Count > 0 Then
                For Each v As Value In meta.ContentVariable.Values
                    If v.ContentInfo Is Nothing Then
                        Continue For
                    End If
                    Select Case informationType
                        Case informationType.Unit
                            AddInformationVariable(li, v.Value, v.ContentInfo.Units)
                        Case informationType.Contact
                            If contactForEveryContent Then
                                AddInformationVariable(li, v.Value, v.ContentInfo.Contact)
                            Else
                                If Not contactAdded Then
                                    If Not v.ContentInfo.Contact Is Nothing Then
                                        li.Definition = v.ContentInfo.Contact
                                        contactAdded = True
                                    End If
                                End If
                            End If
                        Case informationType.LastUpdated
                            If lastUpdatedForEveryContent Then
                                AddInformationVariable(li, v.Value, v.ContentInfo.LastUpdated.PxDate())
                            Else
                                If Not lastUpdatedAdded Then
                                    If Not v.ContentInfo.LastUpdated.PxDate() Is Nothing Then
                                        li.Definition = v.ContentInfo.LastUpdated.PxDate()
                                        lastUpdatedAdded = True
                                    End If
                                End If
                            End If
                        Case informationType.RefPeriod
                            AddInformationVariable(li, v.Value, v.ContentInfo.RefPeriod)
                        Case informationType.StockFa
                            AddInformationVariable(li, v.Value, v.ContentInfo.StockFa)
                        Case informationType.CFPrices
                            AddInformationVariable(li, v.Value, v.ContentInfo.CFPrices)
                        Case informationType.DayAdj
                            AddInformationVariable(li, v.Value, v.ContentInfo.DayAdj)
                        Case informationType.SeasAdj
                            AddInformationVariable(li, v.Value, v.ContentInfo.SeasAdj)
                        Case informationType.BasePeriod
                            AddInformationVariable(li, v.Value, v.ContentInfo.Baseperiod)
                        Case Enums.InformationType.OfficialStatistics
                            li.Definition = DirectCast(IIf(meta.OfficialStatistics, "YES", "NO"), String)
                        Case informationType.UpdateFrequency
                            li.Definition = meta.UpdateFrequency
                        Case informationType.NextUpdate
                            li.Definition = meta.NextUpdate.PxDate()
                        Case informationType.Survey
                            li.Definition = meta.Survey
                        Case informationType.Link
                            li.Definition = CreateLink(meta.Link)
                        Case informationType.CreationDate
                            li.Definition = meta.CreationDate.PxDate()
                        Case informationType.Copyright
                            li.Definition = DirectCast(IIf(meta.Copyright, "YES", "NO"), String)
                        Case informationType.Source
                            li.Definition = meta.Source
                        Case informationType.Matrix
                            li.Definition = meta.Matrix
                        Case informationType.Database
                            li.Definition = meta.Database
                    End Select
                Next
            Else
                Select Case informationType
                    Case Enums.InformationType.OfficialStatistics
                        li.Definition = DirectCast(IIf(meta.OfficialStatistics, "YES", "NO"), String)
                    Case informationType.Unit
                        li.Definition = meta.ContentInfo.Units
                    Case informationType.Contact
                        li.Definition = meta.ContentInfo.Contact
                    Case informationType.LastUpdated
                        li.Definition = meta.ContentInfo.LastUpdated.PxDate()
                    Case informationType.RefPeriod
                        li.Definition = meta.ContentInfo.RefPeriod
                    Case informationType.StockFa
                        li.Definition = meta.ContentInfo.StockFa
                    Case informationType.CFPrices
                        li.Definition = meta.ContentInfo.CFPrices
                    Case informationType.DayAdj
                        li.Definition = meta.ContentInfo.DayAdj
                    Case informationType.SeasAdj
                        li.Definition = meta.ContentInfo.SeasAdj
                    Case informationType.BasePeriod
                        li.Definition = meta.ContentInfo.Baseperiod
                    Case informationType.UpdateFrequency
                        li.Definition = meta.UpdateFrequency
                    Case informationType.NextUpdate
                        li.Definition = meta.NextUpdate.PxDate()
                    Case informationType.Survey
                        li.Definition = meta.Survey
                    Case informationType.Link
                        li.Definition = CreateLink(meta.Link)
                    Case informationType.CreationDate
                        li.Definition = meta.CreationDate.PxDate()
                    Case informationType.Copyright
                        li.Definition = DirectCast(IIf(meta.Copyright, "YES", "NO"), String)
                    Case informationType.Source
                        li.Definition = meta.Source
                    Case informationType.Matrix
                        li.Definition = meta.Matrix
                    Case informationType.Database
                        li.Definition = meta.Database
                End Select
            End If
            If Not String.IsNullOrEmpty(li.Definition) Or li.InformationVariables.Count > 0 Then
                informationList.Add(li)
            End If
        Next

        'End If
        Return informationList
    End Function

    Private Shared Sub AddInformationVariable(ByVal li As InformationListItem, ByVal term As String, _
                                        ByVal definition As String)
        If Not String.IsNullOrEmpty(term) AndAlso Not String.IsNullOrEmpty(definition) Then
            term = term.Trim
            definition = definition.Trim
            If Not String.IsNullOrEmpty(term) AndAlso Not String.IsNullOrEmpty(definition) Then
                li.InformationVariables.Add(New InformationVariable(li.InformationType, term, definition))
            End If
        End If
    End Sub

    Private Shared Function CreateLink(link As String) As String
        If (Not String.IsNullOrWhiteSpace(link) AndAlso link.StartsWith("http")) Then
            Return String.Format("<a href='{0}'>{0}</a>", link)
        End If

        If (Not String.IsNullOrWhiteSpace(link) AndAlso link.StartsWith("www.")) Then
            Return String.Format("<a href='http://{0}'>{0}</a>", link)
        End If

        Return link

    End Function

End Class
