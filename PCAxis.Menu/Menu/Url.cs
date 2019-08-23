using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace PCAxis.Menu
{
	/// <summary>
	/// Class for url links
	/// </summary>	
	public class Url : Link
	{
		/// <summary>
		/// The url to the resource
		/// </summary>
		public string LinkUrl;

		/// <summary>
		/// Link presentation code
		/// </summary>
		public LinkPres LinkPres;

		/// <summary>
		/// Create instance with data
		/// </summary>
		/// <param name="text">Presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the item</param>
		/// <param name="linkUrl">The url for the resource</param>
		/// <param name="linkPres">Link presentation code</param>
		/// <param name="category">Presentation category</param>
		public Url(string text, string sortCode, string menu, string selection, string description, PresCategory category, string linkUrl, LinkPres linkPres)
			: base(text, "", sortCode, menu, selection, description, LinkType.URL, category)
		{
			this.LinkUrl = linkUrl;
			this.LinkPres = linkPres;
		}

		/// <summary>
		/// Returns the link item as XmlElement.
		/// </summary>
		/// <returns></returns>
		protected override XElement GetAsXML(string tagName)
		{
			XElement element = base.GetAsXML(tagName);
			
			element.Add(
				new XAttribute("linkPres", ((char)LinkPres).ToString().Trim()),
				new XElement("Url", LinkUrl)
			);

			return element;
		}
	}
}