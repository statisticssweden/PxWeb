Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Text
Imports System.Web.UI


''' <summary>
''' The JavascriptTester control is designed to provide a quick and
''' reliable way of determining if the client browser supports
''' javascript and if the javascript is enabled (on) in the client
''' browser.
''' </summary>
<DefaultProperty("Enabled"), ToolboxData("<{0}:JavascriptTester runat=server></{0}:JavascriptTester>")> _
Public Class JavascriptTester
    Inherits Control
    Implements IPostBackDataHandler
    ''' <summary>
    ''' Event which is raised when state changes
    ''' </summary>
    Public Event EnabledChanged As EventHandler

#Region "Private Helper Methods"

    ''' <summary>
    ''' A recursive function that unwinds the control tree to find
    ''' the first HtmlForm control's client ID value.
    ''' </summary>
    ''' <remarks>
    ''' This method is hard-coded to return a HtmlForm. It also 
    ''' assumes that it is getting a top level control, such as the 
    ''' Page control, and that this control contains the HtmlForm.
    ''' </remarks>
    ''' <param name="parentControl">Control</param>
    ''' <returns>String</returns>
    Private Function GetFormName(ByVal parentControl As Control) As String
        ' Init the name of the control
        Dim Name As String = ""

        ' Loop through the control collection of the parentControl
        For Each childControl As Control In parentControl.Controls
            ' Check if the type of control is a HtmlForm
            If childControl.[GetType]().ToString() = "System.Web.UI.HtmlControls.HtmlForm" Then
                ' Set the Name to the ClientID
                Name = childControl.ClientID

                ' Exit from the loop
                Exit For
            Else
                ' Check for any child controls of the child
                If childControl.HasControls() Then
                    ' Make a recursive call to loop through the child's children
                    Name = GetFormName(childControl)
                End If
            End If
        Next

        ' Return the name of the HtmlForm
        Return Name
    End Function


#End Region

#Region "Public Attributes"

    ''' <summary>
    ''' Public attribute that returns the name of the hidden
    ''' html element that is updated by javascript to test
    ''' if javascript is enabled.
    ''' </summary>
    ''' <remarks>
    ''' The ClientID is the ClientID of this control (JavascriptTest).
    ''' </remarks>
    Protected ReadOnly Property HelperID() As String
        Get
            Return "__" & ClientID & "_State"
        End Get
    End Property


    ''' <summary>
    ''' Public attribute indicating if javascript is enabled
    ''' on the clients' browsers.
    ''' </summary>
    Public Property Enabled() As Boolean
        Get
            ' Get a base objec to hold the viewstate value
            Dim obj As Object = ViewState(ClientID & "_Enabled")

            ' Check that obj is not null
            If obj Is Nothing Then
                ' object is null
                Return False
            Else
                ' return the converted value
                Return CBool(obj)
            End If
        End Get

        Set(ByVal value As Boolean)
            ' Save the value in viewstate
            ViewState(ClientID & "_Enabled") = value
        End Set
    End Property


#End Region

#Region "Control Events"

    ''' <summary>
    ''' Initialize settings needed during the lifetime of the 
    ''' incoming web request.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)

        If Page IsNot Nothing Then
            Page.RegisterRequiresPostBack(Me)
        End If
    End Sub


    ''' <summary>
    ''' Perform any updates before the output is rendered.
    ''' </summary>
    ''' <param name="e">EventArgs</param>
    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        ' Call the base classes' OnPreRender event
        MyBase.OnPreRender(e)

        ' verify that the page object is valid
        If Page IsNot Nothing Then
            ' Register the hidden form element



            Page.ClientScript.RegisterHiddenField(HelperID, Enabled.ToString())

            ' Verify that the startup script isn't already registered (postbacks)
            If Not Page.ClientScript.IsStartupScriptRegistered("JavascriptTester_Startup") Then
                ' Form the script to be registered at client side.
                Dim sb As New StringBuilder()
                Dim sFormName As String = GetFormName(Me.Page)

                sb.Append("<script lang='javascript'>")
                'sb.Append("if (document." & sFormName & "." & HelperID & ".value == 'False')")
                sb.Append("var theForm=" & "document.getElementById('" & sFormName & "'); ")
                sb.Append("if (theForm." & HelperID & ".value == 'False')")
                sb.Append("{")
                sb.Append("theForm." & HelperID & ".value = 'True';")
                sb.Append("theForm.submit();")
                sb.Append("}")
                sb.Append("</script>")
                ' Register the startup script
                Page.ClientScript.RegisterStartupScript(GetType(Page), "JavascriptTester_Startup", sb.ToString())
            End If
        End If
    End Sub


    ''' <summary>
    ''' Process incoming form data and update properties accordingly,
    ''' </summary>
    ''' <param name="postDataKey">String</param>
    ''' <param name="postCollection">NameValueCollection</param>
    ''' <returns></returns>
    Private Function IPostBackDataHandler_LoadPostData(ByVal postDataKey As String, ByVal postCollection As NameValueCollection) As Boolean Implements IPostBackDataHandler.LoadPostData
        ' Pull out the hidden form value from the collection of values with the name of the control
        Dim value As String = postCollection(HelperID)

        ' Verify that the value isn't null
        If value IsNot Nothing Then
            ' Grab the new value and compare it to 'true'
            Dim newValue As Boolean = ([String].Compare(value, "true", True, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) = 0)
            ' Set the old value to the enabled property value
            Dim oldValue As Boolean = Enabled

            Enabled = newValue

            ' Raise the change event if there was a change
            Return (newValue <> oldValue)
        End If

        ' value didn't change
        Return False
    End Function


    ''' <summary>
    ''' Raise change events in response to state changes between
    ''' the current and previous postbacks.
    ''' </summary>
    Private Sub IPostBackDataHandler_RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent

        ' There was a change,  so raise any events.
        RaiseEvent EnabledChanged(Me, EventArgs.Empty)
    End Sub


#End Region

End Class





