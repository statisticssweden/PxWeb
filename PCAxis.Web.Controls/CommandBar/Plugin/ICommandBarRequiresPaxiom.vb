Namespace CommandBar.Plugin
    ''' <summary>
    ''' Interface for plugins of the type <see cref="ICommandBarNoGUIPlugin" /> that needs access to a <see cref="Paxiom.PXModel" />
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommandBarRequiresPaxiom

        ''' <summary>
        ''' Gets or sets an instance of <see cref="Paxiom.PXModel" />
        ''' </summary>
        ''' <value>Instance of <see cref="Paxiom.PXModel" /></value>
        ''' <returns>An instance of <see cref="Paxiom.PXModel" /></returns>
        ''' <remarks></remarks>
        Property PaxiomModel() As Paxiom.PXModel

    End Interface
End Namespace

