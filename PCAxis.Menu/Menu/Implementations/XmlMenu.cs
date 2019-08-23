using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PCAxis.Menu;
using System.Linq;
using System.Xml.Linq;
using PCAxis.Menu.Exceptions;
using PCAxis.Menu.Extensions;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// Menu class reading from an XML structure
	/// </summary>
	public class XmlMenu : PxMenuBase
	{
		/// <summary>
		/// Create from file
		/// </summary>
		/// <param name="xmlFilePath">File to import data from</param>
		/// <param name="language">Language code to search for in file</param>
		public XmlMenu(string xmlFilePath, string language) : this(XDocument.Load(xmlFilePath), language, null)
		{
		}

		/// <summary>
		/// Create from XML structure in memory
		/// </summary>
		/// <param name="document">XML document to import data from</param>
		public XmlMenu(XDocument document)
			: this(document, "", null)
		{
		}

		/// <summary>
		/// Delegate with specific type for initialization lambda.
		/// </summary>
		/// <param name="xmlMenu"></param>
		public delegate void XmlMenuInitialization(XmlMenu xmlMenu);

		/// <summary>
		/// Create from XML structure in memory
		/// </summary>
		/// <param name="document">XML document to import data from</param>
		/// <param name="language">Language code to search for in document</param>
        /// <param name="initializationFunction">Lambda for initialization</param>
        public XmlMenu(XDocument document, string language, XmlMenuInitialization initializationFunction)
		{
			if (initializationFunction != null)
				initializationFunction(this);

			XElement root;

			try
			{
				root = document.Root.Elements().Where(e => e.Attribute("lang").Value == language || e.Attribute("default").Value == "yes").First();
			}
			catch (InvalidOperationException e)
			{
				throw new InvalidMenuFromXMLException("Selected language not in XML.", e);
			}
			
			Language = root.Attribute("lang").Value;

			RootItem = (PxMenuItem)parseXml(root.Elements().First());

			RootItem.Sort();
		}

		private Item parseXml(XElement element)
		{
			Item item = null;

			switch (element.Name.ToString())
			{
				case "MenuItem":
					item =
						new PxMenuItem(
							this,
							element.Element("Text").Value,
							element.Element("TextShort").Value,
							element.Attribute("sortCode").Value,
							element.Parent.Attribute("selection") != null ? element.Parent.Attribute("selection").Value : "",
							element.Attribute("selection").Value,
							element.Element("Description").Value
						);

					XName[] subItemLabels = new XName[] { "MenuItem", "Headline", "Link" };

					item.Cast<PxMenuItem>().AddSubItemRange(
						from e in element.Elements()
						where subItemLabels.Contains(e.Name)
						select parseXml(e)
					);

					break;
				case "Headline":
					item =
						new Headline(
							element.Element("Text").Value,
							element.Element("TextShort").Value,
							element.Attribute("sortCode").Value,
							element.Parent.Attribute("selection").Value,
							element.Attribute("selection").Value,
							element.Element("Description").Value
						);

					break;
				case "Link":
					item =
						new TableLink(
							element.Element("Text").Value,
							element.Element("TextShort").Value,
							element.Attribute("sortCode").Value,
							element.Parent.Attribute("selection").Value,
							element.Attribute("selection").Value,
							element.Element("Description").Value,
							element.Attribute("type").Value.ToEnum<LinkType>(),
							element.Attribute("status").Value == "" ? TableStatus.NotSet : (TableStatus)element.Attribute("status").Value[0],
							element.Element("Published").Value == "" ? null : (DateTime?)DateTime.Parse(element.Element("Published").Value),
							element.Element("LastUpdated").Value == "" ? null : (DateTime?)DateTime.Parse(element.Element("LastUpdated").Value),
							element.Element("StartTime").Value,
							element.Element("EndTime").Value,
							element.Attribute("tableId").Value,
							element.Attribute("category").Value == "" ? PresCategory.NotSet : (PresCategory)element.Attribute("category").Value[0]
						);

					foreach (XElement a in element.Elements("Attribute"))
						item.Cast<Link>().SetAttribute(a.Attribute("name").Value, a.Value);

					break;
				case "Url":
					item =
						new Url(
							element.Element("Text").Value,
							element.Attribute("sortCode").Value,
							element.Parent.Attribute("selection").Value,
							element.Attribute("selection").Value,
							element.Element("Description").Value,
							element.Attribute("category").Value == "" ? PresCategory.NotSet : (PresCategory)element.Attribute("category").Value[0],
							element.Element("Url").Value,
							element.Attribute("linkPres").Value == "" ? LinkPres.NotSet : (LinkPres)element.Attribute("linkPres").Value[0]
						);

					foreach (XElement a in element.Elements("Attribute"))
						item.Cast<Link>().SetAttribute(a.Attribute("name").Value, a.Value);

					break;
				
				default:
					break;
			}

			if (item.IsNotOfType<Url>())
				foreach (Url u in element.Elements("Url").Select(e => parseXml(e)))
					if (u != null) item.AddUrl(u);

			item.ID.Menu = element.Parent.Attribute("selection") != null ? element.Parent.Attribute("selection").Value : "";

            if (Restriction == null || Restriction(item))
            {
                if (AlterItemBeforeStorage != null)
                    AlterItemBeforeStorage(item);

                return item;
            }
            else
            { 
                return null;
            }
		}
	}
}
