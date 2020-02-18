Namespace PCAxis.Paxiom.Operations

    Public Class Pivot
        Implements PCAxis.PlugIn.IPlugIn, PCAxis.Paxiom.IPXOperation

        Public ReadOnly Property Description() As String Implements PCAxis.PlugIn.IPlugIn.Description
            Get
                Return "Pivot operation"
            End Get
        End Property

        Public ReadOnly Property Id() As System.Guid Implements PCAxis.PlugIn.IPlugIn.Id
            Get
                Return New Guid("EBF6C7ED-60B6-4c6a-86D3-72C11FD6BEEB")
            End Get
        End Property

        Public Sub Initialize(ByVal host As PCAxis.PlugIn.IPlugInHost, ByVal configuration As System.Collections.Generic.Dictionary(Of String, String)) Implements PCAxis.PlugIn.IPlugIn.Initialize

        End Sub

        Public ReadOnly Property Name() As String Implements PCAxis.PlugIn.IPlugIn.Name
            Get
                Return "Pivot"
            End Get
        End Property

        Public Sub Terminate() Implements PCAxis.PlugIn.IPlugIn.Terminate

        End Sub

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PCAxis.Paxiom.PXModel, ByVal rhs As Object) As PCAxis.Paxiom.PXModel Implements PCAxis.Paxiom.IPXOperation.Execute
            If Not TypeOf rhs Is PivotDescription() Then
                'TODO skicka Argument exception
                Throw New Exception("Paramater not suported")
            End If

            Return Execute(lhs, CType(rhs, PivotDescription()))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function NoPivot(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As PivotDescription()) As Boolean
            Dim shouldPivot As Boolean = True
            'Check to see if operation must be applied
            Dim hCount As Integer = -1
            Dim sCount As Integer = -1
            For i As Integer = 0 To rhs.Length - 1
                If rhs(i).VariablePlacement = PlacementType.Heading Then
                    hCount += 1
                    If hCount >= oldModel.Meta.Heading.Count Then
                        shouldPivot = False
                        Exit For
                    End If
                    If Not String.Compare(rhs(i).VariableName, oldModel.Meta.Heading(hCount).Name) = 0 Then
                        shouldPivot = False
                        Exit For
                    End If
                Else
                    sCount += 1
                    If sCount >= oldModel.Meta.Stub.Count Then
                        shouldPivot = False
                        Exit For
                    End If
                    If Not String.Compare(rhs(i).VariableName, oldModel.Meta.Stub(sCount).Name) = 0 Then
                        shouldPivot = False
                        Exit For
                    End If
                End If
            Next
            Return shouldPivot
        End Function

        ''' <summary>
        ''' Execute function handling the typecasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As PivotDescription()) As PCAxis.Paxiom.PXModel

            If oldModel.Meta.Variables.Count <> rhs.Length Then
                Throw New PXOperationException("The number of variables dose not match")
            End If


            If NoPivot(oldModel, rhs) Then
                Return oldModel
            End If

            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data

            Dim oldMeta As PCAxis.Paxiom.PXMeta = oldModel.Meta
            Dim oldData As PCAxis.Paxiom.PXData = oldModel.Data

            ' Rebuild Stub/Heading/Variables collections
            newMeta.Variables.Clear()
            newMeta.Stub.Clear()
            newMeta.Heading.Clear()

            Dim description As PivotDescription
            Dim var As Variable
            For i As Integer = 0 To rhs.Length - 1
                description = rhs(i)
                var = oldMeta.Variables.GetByName(description.VariableName).CreateCopyWithValues()
                var.Placement = description.VariablePlacement

                newMeta.AddVariable(var)
            Next

            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            'Matrisordningen old
            Dim newOrder(newMeta.Variables.Count - 1) As String
            Dim index As Integer = 0
            For i As Integer = newMeta.Heading.Count - 1 To 0 Step -1
                newOrder(index) = newMeta.Heading(i).Name
                index += 1
            Next
            For i As Integer = newMeta.Stub.Count - 1 To 0 Step -1
                newOrder(index) = newMeta.Stub(i).Name
                index += 1
            Next

            'Beräknar hjälpmatriser new
            Dim npm(newMeta.Variables.Count - 1) As Integer

            npm(0) = 1
            For i As Integer = 1 To newOrder.Length - 1
                npm(i) = npm(i - 1) * newMeta.Variables.GetByName(newOrder(i - 1)).Values.Count
            Next

            'Matrisordningen old
            Dim oldOrder(newMeta.Variables.Count - 1) As String
            index = 0
            Dim tIndex As Integer
            For i As Integer = oldMeta.Heading.Count - 1 To 0 Step -1
                oldOrder(index) = oldMeta.Heading(i).Name
                index += 1
            Next
            tIndex = index
            For i As Integer = oldMeta.Stub.Count - 1 To 0 Step -1
                oldOrder(index) = oldMeta.Stub(i).Name
                index += 1
            Next

            'Beräknar hjälpmatriser old
            Dim opmA(oldMeta.Variables.Count - 1) As Integer
            Dim opmB(oldMeta.Variables.Count - 1) As Integer

            For i As Integer = 0 To oldOrder.Length - 1
                If i = 0 Then
                    opmA(0) = oldMeta.Variables.GetByName(oldOrder(i)).Values.Count
                ElseIf i = tIndex Then
                    opmA(i) = oldMeta.Variables.GetByName(oldOrder(i)).Values.Count
                Else
                    opmA(i) = opmA(i - 1) * oldMeta.Variables.GetByName(oldOrder(i)).Values.Count
                End If
                opmB(i) = oldMeta.Variables.GetByName(oldOrder(i)).Values.Count
            Next

            'Sorterar new hjälptatrisen
            Dim tmp(npm.Length - 1) As Integer
            Dim k As Integer = 0
            For i As Integer = 0 To oldOrder.Length - 1
                k = GetIndex(newOrder, oldOrder(i))
                tmp(i) = npm(k)
            Next
            npm = tmp

            Dim pf As Integer
            Dim cc As Integer = oldData.MatrixColumnCount
            For i As Integer = 0 To oldData.MatrixSize - 1
                index = 0
                pf = 1
                For j As Integer = 0 To oldOrder.Length - 1
                    If j < tIndex Then
                        index += (((i Mod cc) \ pf) Mod opmB(j)) * npm(j)
                    Else
                        If j = tIndex Then
                            pf = 1
                        End If
                        index += (((i \ cc) \ pf) Mod opmB(j)) * npm(j)
                    End If
                    pf = opmA(j)
                Next
                newData.WriteElement(index, oldData.ReadElement(i))
                newData.WriteDataNoteCellElement(index, oldData.ReadDataCellNoteElement(i))

            Next

            newModel.Data.CurrentIndex = oldModel.Data.CurrentIndex

            If oldMeta.ContentVariable IsNot Nothing Then
                newMeta.ContentVariable = newMeta.Variables.GetByCode(oldMeta.ContentVariable.Code)
            End If

            'newModel = New PXModel(newMeta, newData)
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel
        End Function

        Private Shared Function GetIndex(ByVal variables() As String, ByVal variableName As String) As Integer
            Dim index As Integer = 0

            For index = 0 To variables.Length - 1
                If String.Compare(variables(index), variableName) = 0 Then
                    Exit For
                End If
            Next

            Return index
        End Function

#Region "Quick functions"
        Public Function PivotCW(ByVal mModel As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel
            Dim meta As PCAxis.Paxiom.PXMeta = mModel.Meta
            Dim pd(meta.Variables.Count - 1) As PCAxis.Paxiom.Operations.PivotDescription
            Dim index As Integer = 0

            If meta.Stub.Count = 0 Then
                For i As Integer = 0 To meta.Heading.Count - 2
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(i).Name, PlacementType.Heading)
                    index += 1
                Next
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(meta.Heading.Count - 1).Name, PlacementType.Stub)

            ElseIf meta.Heading.Count = 0 Then
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(0).Name, PlacementType.Heading)
                index += 1
                For i As Integer = 1 To meta.Stub.Count - 1
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(i).Name, PlacementType.Stub)
                    index += 1
                Next
            Else
                'Heading
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(0).Name, PlacementType.Heading)
                index += 1
                For i As Integer = 0 To meta.Heading.Count - 2
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(i).Name, PlacementType.Heading)
                    index += 1
                Next

                'Stub
                For i As Integer = 1 To meta.Stub.Count - 1
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(i).Name, PlacementType.Stub)
                    index += 1
                Next
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(meta.Heading.Count - 1).Name, PlacementType.Stub)
            End If

            Dim p As New PCAxis.Paxiom.Operations.Pivot

            Return p.Execute(mModel, pd)

        End Function

        Public Function PivotCCW(ByVal model As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel

            Dim meta As PCAxis.Paxiom.PXMeta = model.Meta
            Dim pd(meta.Variables.Count - 1) As PCAxis.Paxiom.Operations.PivotDescription
            Dim index As Integer = 0

            If meta.Stub.Count = 0 Then
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(0).Name, PlacementType.Stub)
                index += 1
                For i As Integer = 1 To meta.Heading.Count - 1
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(i).Name, PlacementType.Heading)
                    index += 1
                Next
            ElseIf meta.Heading.Count = 0 Then
                For i As Integer = 0 To meta.Stub.Count - 2
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(i).Name, PlacementType.Stub)
                    index += 1
                Next
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(meta.Stub.Count - 1).Name, PlacementType.Heading)
            Else
                'Heading
                For i As Integer = 1 To meta.Heading.Count - 1
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(i).Name, PlacementType.Heading)
                    index += 1
                Next
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(meta.Stub.Count - 1).Name, PlacementType.Heading)
                index += 1

                'Stub
                pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Heading(0).Name, PlacementType.Stub)
                index += 1
                For i As Integer = 0 To meta.Stub.Count - 2
                    pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(meta.Stub(i).Name, PlacementType.Stub)
                    index += 1
                Next
            End If


            Dim p As New PCAxis.Paxiom.Operations.Pivot

            Return p.Execute(model, pd)

        End Function

        'Public Function Sort(ByVal model As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel
        '    Dim pd(model.Meta.Variables.Count - 1) As PCAxis.Paxiom.Operations.PivotDescription
        '    Dim index As Integer = 0

        '    For Each var As Variable In model.Meta.Stub
        '        pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(var.Name, PlacementType.Heading)
        '        index += 1
        '    Next
        '    For Each var As Variable In model.Meta.Heading
        '        pd(index) = New PCAxis.Paxiom.Operations.PivotDescription(var.Name, PlacementType.Heading)
        '        index += 1
        '    Next
        '    Dim p As New PCAxis.Paxiom.Operations.Pivot
        '    Return p.Execute(model, pd)
        'End Function

#End Region

    End Class

End Namespace
