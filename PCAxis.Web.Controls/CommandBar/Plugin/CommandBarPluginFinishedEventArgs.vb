Namespace CommandBar.Plugin
    ''' <summary>
    ''' Provides data for the <see cref="ICommandBarGUIPlugin.Finished" />event
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommandBarPluginFinishedEventArgs
        Inherits EventArgs


        Private _paxiomModel As Paxiom.PXModel
        ''' <summary>
        ''' Gets an transformed instance of <see cref="Paxiom.PXModel" />  
        ''' </summary>
        ''' <value>Transformed instance of <see cref="Paxiom.PXModel" />  </value>
        ''' <returns>An transformed instance of <see cref="Paxiom.PXModel" />  </returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PaxiomModel() As Paxiom.PXModel
            Get
                Return _paxiomModel
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandBarPluginFinishedEventArgs"  />
        ''' </summary>
        ''' <param name="paxiomModel">Transformed instance of <see cref="Paxiom.PXModel" /> or <c>null</c></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal paxiomModel As Paxiom.PXModel)
            Me._paxiomModel = paxiomModel
        End Sub

    End Class
End Namespace

