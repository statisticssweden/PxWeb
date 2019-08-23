Imports System.IO
Imports PCAxis.Menu

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class for creating PxMenu Links from a PX-file
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MenuPxLinkBuilder

#Region "Private members"
        Private _parser As Parsers.PXFileParser
        Private _language As String
        Private _title As String
        Private _description As String
        Private _lastUpdated As String
        Private _languageRead As Boolean
        Private _titleRead As Boolean
        Private _descriptionRead As Boolean
        Private _lastUpdatedRead As Boolean
#End Region

        ''' <summary>
        ''' Creates a PXMenu.Link object for the specified PX-file
        ''' </summary>
        ''' <param name="f">FileInfo object for the PX-file</param>
        ''' <param name="language">
        ''' If specified, the link is only created if the px-file is in the given language.
        ''' If not specified (""), links are created for all PX-files
        ''' </param>
        ''' <param name="sort">Sortcode</param>
        ''' <returns>The instantiated PxMenu.Link object</returns>
        ''' <remarks></remarks>
        Public Function BuildLink(ByVal f As FileInfo, ByVal language As String, ByVal sort As String) As Link
            Dim lnk As Link
            Dim presText As String
            Dim modified As String
            Dim size As String
            Dim updated As String

            Try
                If f.Extension.ToLower <> ".px" Then
                    Return Nothing
                End If

                InitBuild()

                _parser = New Parsers.PXFileParser(f.FullName)
                _parser.ParseMeta(AddressOf SetLinkMeta, "")

                'If language dependent - check language
                If Not String.IsNullOrEmpty(language) Then
                    If _language <> language Then
                        Return Nothing
                    End If
                End If

                'Presentation text
                If Not String.IsNullOrEmpty(_description) Then
                    presText = _description
                Else
                    If Not String.IsNullOrEmpty(_title) Then
                        presText = _title
                    Else
                        presText = f.Name
                    End If
                End If

                If Not String.IsNullOrEmpty(_lastUpdated) Then
                    modified = _lastUpdated
                    'px.Model.Meta.ContentInfo.LastUpdated is supposed to have the format "CCYYMMDD hh:mm"
                    If modified.Length = 14 Then
                        Try
                            Dim dtMod As New DateTime(CInt(modified.Substring(0, 4)), _
                                                      CInt(modified.Substring(4, 2)), _
                                                      CInt(modified.Substring(6, 2)), _
                                                      CInt(modified.Substring(9, 2)), _
                                                      CInt(modified.Substring(12, 2)), _
                                                      0)
                            modified = dtMod.ToShortDateString
                        Catch ex As Exception
                            modified = _lastUpdated
                        End Try
                    End If
                Else
                    modified = _lastUpdated
                End If

                updated = f.LastWriteTime.ToShortDateString
                size = CStr(CInt(f.Length / 1000)) & " kb"

                'lnk = New PxMenu.Link(presText, sort, PxMenu.Link.LinkTypes.PxFile, f.FullName)
                lnk = New TableLink(presText, "", sort, IO.Path.GetDirectoryName(f.FullName), f.FullName, "", LinkType.PX, TableStatus.AccessibleToAll, Nothing, "", "", "", PresCategory.Official)

                'Size attribute
                lnk.SetAttribute("size", size)
                'Modified attribute
                If Not String.IsNullOrEmpty(modified) Then
                    lnk.SetAttribute("modified", modified)
                End If
                If Not String.IsNullOrEmpty(updated) Then
                    lnk.SetAttribute("updated", updated)
                End If

                Return lnk
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Performs initiation of memebers befor link build
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitBuild()
            _language = ""
            _title = ""
            _description = ""
            _lastUpdated = ""
            _languageRead = False
            _titleRead = False
            _descriptionRead = False
            _lastUpdatedRead = False
        End Sub

        Private Function SetLinkMeta(ByVal keyword As String, ByVal language As String, ByVal subkey As String, ByVal values As System.Collections.Specialized.StringCollection) As Boolean
            Select Case keyword
                Case PXKeywords.LANGUAGE
                    _language = values(0)
                    _languageRead = True
                Case PXKeywords.DESCRIPTION
                    _description = values(0)
                    _descriptionRead = True
                Case PXKeywords.TITLE
                    _title = values(0)
                    _titleRead = True
                Case PXKeywords.LAST_UPDATED
                    _lastUpdated = values(0)
                    _lastUpdatedRead = True
            End Select

            If (_languageRead And _descriptionRead And _lastUpdatedRead) Or _
            (_languageRead And _descriptionRead And _titleRead And _lastUpdatedRead) Then
                _parser.StopParsing()
            End If

            Return True
        End Function

    End Class

End Namespace
