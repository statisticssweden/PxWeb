using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using PCAxis.Menu.Extensions;

namespace PCAxis.Menu
{
	/// <summary>
	/// Abstract class for different kinds of links
	/// </summary>	
	public abstract class Link : Item
	{
		private LinkType type;
		private PresCategory category = PresCategory.NotSet;
		
		/// <summary>
		/// Create instance with data
		/// </summary>
		/// <param name="text">Presentation text</param>
		/// <param name="textShort">Short presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the item</param>
		/// <param name="type">Type of link</param>
		/// <param name="category">Links category</param>
        protected Link(string text, string textShort, string sortCode, string menu, string selection, string description, LinkType type, PresCategory category)
			: base(text, textShort, sortCode, menu, selection, description)
		{
			Type = type;
			Category = category;
		}

		/// <summary>
		/// Gets the link type
		/// </summary>
		public LinkType Type
		{
			get { return type; }
			private set { type = value; }
		}

		/// <summary>
		/// Gets the link category
		/// </summary>
		public PresCategory Category
		{
			get { return category; }
			private set { category = value; }
		}

		/// <summary>
		/// Returns the link item as XmlElement.
		/// </summary>
		/// <returns></returns>
		protected override XElement GetAsXML(string tagName)
		{
			XElement element = base.GetAsXML(tagName);

			element.Add(
				new XAttribute("type", Type.ToString()),
				new XAttribute("category", ((char)Category).ToString().Trim()),
				from a in attributes select new XElement("Attribute", new XAttribute("name", a.Key), a.Value.ToString())
			);

			return element;
		}
	}
}