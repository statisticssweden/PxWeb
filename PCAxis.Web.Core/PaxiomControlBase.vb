Imports System.ComponentModel
Imports PCAxis.Web.Core.Enums
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Management

''' <summary>
''' Serves as the base class for all the controls that uses the <see cref="PCAxis.Paxiom.PXModel" />
''' </summary>
''' <typeparam name="TControl">The type of the usercontrol</typeparam>
''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
''' <remarks></remarks>
Public MustInherit Class PaxiomControlBase(Of TControl As ControlBase(Of TControl, TMarker), TMarker As MarkerControlBase(Of TControl, TMarker))
    Inherits ControlBase(Of TControl, TMarker)
    Private _paxiomModel As Paxiom.PXModel

    ''' <summary>
    ''' Gets or sets the <see cref="PXModel" />
    ''' </summary>
    ''' <value>The <see cref="PXModel" /> for the control to work with</value>
    ''' <returns>An instance of <see cref="PXModel" /> or <c>null</c></returns>
    ''' <remarks>If <see cref="PXModel" /> is nothing, it tries to get it from <see cref="PaxiomManager.PaxiomModel" /> otherwise it returns <c>null</c></remarks>
    <Browsable(False)> _
    Public Property PaxiomModel() As PXModel
        Get
            If _paxiomModel Is Nothing Then
                _paxiomModel = PaxiomManager.PaxiomModel

            End If
            Return _paxiomModel
        End Get
        Set(ByVal value As PXModel)
            _paxiomModel = value
            If Not Me.IsLoadingState Then
                OnPaxiomModelChanged(New EventArgs())
            End If
        End Set
    End Property

    ''' <summary>
    ''' Event that is fired whenever the <see cref="PaxiomModel" /> is changed
    ''' </summary>
    ''' <remarks></remarks>
    Protected Event PaxiomModelChanged As EventHandler

    ''' <summary>
    ''' Raises the <see cref=" PaxiomModelChanged" /> event
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub OnPaxiomModelChanged(ByVal args As EventArgs)
        RaiseEvent PaxiomModelChanged(Me, args)
    End Sub

    ''' <summary>
    ''' Overrides the <see cref="ControlBase(Of TControl,TMarker).OnLanguageChanged" /> 
    ''' to try and change the language of the <see cref="PXModel" />
    ''' </summary>
    ''' <param name="args">An EventArgs object that contains the event data</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLanguageChanged(ByVal args As EventArgs)
        If Me.PaxiomModel IsNot Nothing Then                        
            Dim langCode As String = LocalizationManager.GetTwoLetterLanguageCode()            

            Me.PaxiomModel.Meta.SetPreferredLanguage(langCode)
        End If
        MyBase.OnLanguageChanged(args)
    End Sub

    ''' <summary>
    ''' Uses the init event to register an listener for the Paxiom model changed event
    ''' </summary>
    ''' <param name="sender">The source of the event</param>
    ''' <param name="e">An <see cref="EventArgs" /> that contains no event data</param>
    ''' <remarks></remarks>
    Private Sub PaxiomControlBase_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        PaxiomManager.RegisterPaxiomModelChanged(AddressOf PaxiomControlBase_PaxiomModelChanged)
    End Sub

    ''' <summary>
    ''' Changes the <see cref="PaxiomModel" /> whenever the <see cref="PaxiomManager.PaxiomModel" /> is changed
    ''' </summary>
    ''' <param name="sender">The source of the event</param>
    ''' <param name="e">An <see cref="EventArgs" /> that contains no event data</param>
    ''' <remarks></remarks>
    Private Sub PaxiomControlBase_PaxiomModelChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.PaxiomModel = PaxiomManager.PaxiomModel
    End Sub

End Class
