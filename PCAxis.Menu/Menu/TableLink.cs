using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace PCAxis.Menu
{
	/// <summary>
	/// Class for non url links
	/// </summary>	
	public class TableLink : Link
	{
		/// <summary>
		/// Tables status
		/// </summary>
		public TableStatus Status = TableStatus.NotSet;

		/// <summary>
		/// Tables publish date
		/// </summary>
		public DateTime? Published;

		/// <summary>
		/// Tables last updated date
		/// </summary>
		public DateTime? LastUpdated;

		/// <summary>
		/// Tables first period
		/// </summary>
		public string StartTime;

		/// <summary>
		/// Tables last period
		/// </summary>
		public string EndTime;

		/// <summary>
		/// The is of the table
		/// </summary>
		public string TableId;

		/// <summary>
		/// This constructor is obsolete and should not be used
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textShort"></param>
		/// <param name="sortCode"></param>
		/// <param name="menu"></param>
		/// <param name="selection"></param>
		/// <param name="description"></param>
		/// <param name="type"></param>
		/// <param name="status"></param>
		/// <param name="published"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <param name="tableId"></param>
		/// <param name="category"></param>
		[Obsolete("Please change your code, so a value for lastUpdated is supplied.")]
		public TableLink(string text, string textShort, string sortCode, string menu, string selection, string description, LinkType type, TableStatus status, DateTime? published, string startTime, string endTime, string tableId, PresCategory category) 
			: this(text, textShort, sortCode, menu, selection, description, type, status, published, null, startTime, endTime, tableId, category)
		{
		}

		/// <summary>
		/// Create instance with data
		/// </summary>
		/// <param name="text">Presentation text</param>
		/// <param name="textShort">Short presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the table</param>
		/// <param name="type">Type of link</param>
		/// <param name="status">Table status</param>
		/// <param name="published">Date published</param>
		/// <param name="lastUpdated">Date last updated</param>
		/// <param name="startTime">Data from period</param>
		/// <param name="endTime">Data to period</param>
		/// <param name="tableId">Table ID</param>
		/// <param name="category">Presentation category</param>
		public TableLink(string text, string textShort, string sortCode, string menu, string selection, string description, LinkType type, TableStatus status, DateTime? published, DateTime? lastUpdated, string startTime, string endTime, string tableId, PresCategory category)
			: base(text, textShort, sortCode, menu, selection, description, type, category)
		{
			Status = status;
			Published = published;
			LastUpdated = lastUpdated;
			StartTime = startTime;
			EndTime = endTime;
			TableId = tableId;
		}

		/// <summary>
		/// Returns the link item as XmlElement.
		/// </summary>
		/// <returns></returns>
		protected override XElement GetAsXML(string tagName)
		{
			XElement element = base.GetAsXML(tagName ?? "Link");
			
			element.Add(
				new XAttribute("tableId", TableId),
				new XAttribute("status", ((char)Status).ToString().Trim()),
				new XElement("Published", Published.ToString()),
				new XElement("LastUpdated", LastUpdated.ToString()),
				new XElement("StartTime", StartTime.ToString()),
				new XElement("EndTime", EndTime.ToString())
			);

			return element;
		}
	}
}