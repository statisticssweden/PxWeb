Imports System.Xml.Serialization
Namespace PCAxis.Paxiom
    <XmlRoot("dictionary")> _
Public Class SerializableDictionary(Of TKey, TValue)
        Inherits Dictionary(Of TKey, TValue)
        Implements IXmlSerializable
#Region "IXmlSerializable Members"
        ''' <summary>
        ''' Always return Nothing
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements IXmlSerializable.GetSchema
            Return Nothing
        End Function

        ''' <summary>
        ''' Read xml
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <remarks></remarks>
        Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements IXmlSerializable.ReadXml
            Dim keySerializer As New XmlSerializer(GetType(TKey))
            Dim valueSerializer As New XmlSerializer(GetType(TValue))
            Dim wasEmpty As Boolean = reader.IsEmptyElement

            reader.Read()

            If wasEmpty Then
                Return
            End If

            While reader.NodeType <> System.Xml.XmlNodeType.EndElement

                reader.ReadStartElement("item")
                reader.ReadStartElement("key")

                Dim key As TKey = CType(keySerializer.Deserialize(reader), TKey)

                reader.ReadEndElement()
                reader.ReadStartElement("value")

                Dim value As TValue = CType(valueSerializer.Deserialize(reader), TValue)

                reader.ReadEndElement()
                Me.Add(key, value)

                reader.ReadEndElement()
                reader.MoveToContent()
            End While

            reader.ReadEndElement()
        End Sub

        ''' <summary>
        ''' Write xml
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements IXmlSerializable.WriteXml
            Dim keySerializer As New XmlSerializer(GetType(TKey))
            Dim valueSerializer As New Xml.Serialization.XmlSerializer(GetType(TValue))

            For Each key As TKey In Me.Keys
                writer.WriteStartElement("item")
                writer.WriteStartElement("key")
                keySerializer.Serialize(writer, key)

                writer.WriteEndElement()
                writer.WriteStartElement("value")

                Dim value As Object = Me(key)

                valueSerializer.Serialize(writer, value)

                writer.WriteEndElement()
                writer.WriteEndElement()
            Next
        End Sub
#End Region
    End Class

End Namespace