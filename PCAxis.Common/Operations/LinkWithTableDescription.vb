Namespace PCAxis.Paxiom.Operations
    Public Class LinkWithTableDescription
        ''' <summary>
        ''' PXModel to link into the existing model.
        ''' </summary>
        ''' <remarks></remarks>
        Public LinkWithTableModel As PCAxis.Paxiom.PXModel

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub
        ''' <summary>
        ''' Constructor for sent linkmodel
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal linkModel As PCAxis.Paxiom.PXModel)
            LinkWithTableModel = linkModel
        End Sub

    End Class
End Namespace
