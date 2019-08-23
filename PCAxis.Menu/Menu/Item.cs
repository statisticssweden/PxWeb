using System;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using PCAxis.Menu.Extensions;
using System.Linq;

namespace PCAxis.Menu
{
	/// <summary>
	/// Abstract class for all menu items
	/// </summary>
	public abstract class Item : IComparable
	{
		private string text;
		private string textShort;
		private string sortCode;
        private ItemSelection id;
		private string description;

        private Item parent;

		private List<Url> urls = new List<Url>();

		/// <summary>
		/// Dictionary for storing attribues about the item.
		/// </summary>
		protected Dictionary<string, object> attributes = new Dictionary<string, object>();

		/// <summary>
		/// Abstract Item that acts as base for the items in the menu.
		/// </summary>
		/// <param name="text">Presentation text</param>
		/// <param name="textShort">Short presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the item</param>
		protected Item(string text, string textShort, string sortCode, string menu, string selection, string description)
		{
			Text = text;
			TextShort = textShort;
			SortCode = sortCode;
            ID = new ItemSelection(menu, selection);
			Description = description;
		}

		/// <summary>
		/// Returns IEnumrable with all the urls connected to this item
		/// </summary>
		public IEnumerable<Url> Urls
		{
			get
			{
				return urls;
			}
		}

		internal void AddUrl(Url url)
		{
			url.Parent = this;
			urls.Add(url);
		}

		/// <summary>
		/// Element in which the current item resides
		/// </summary>
		public Item Parent
		{
			get
			{
				return parent;
			}
			internal set
			{
				parent = value;
			}
		}

		/// <summary>
		/// Returns the route to this item in the menu hierarchy
		/// </summary>
		public List<PxMenuItem> Breadcrumbs
		{
			get
			{
				List<PxMenuItem> crumbs = parent == null ? new List<PxMenuItem>() : parent.Breadcrumbs;

				if (this is PxMenuItem)
					crumbs.Add((PxMenuItem)this);

				return crumbs;
			}
		}

		/// <summary>
		/// Items level number (actual level number, can be different from database)
		/// </summary>
		public int LevelNumber
		{
			get
			{
				if (parent == null)
					return 0;

				return parent.LevelNumber + 1;
			}
		}

		/// <summary>
		/// Presentation text for the item
		/// </summary>
		/// <returns></returns>
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		/// <summary>
		/// Short presentation text for the item
		/// </summary>
		public string TextShort
		{
			get { return textShort; }
			set { textShort = value; }
		}

		/// <summary>
		/// The description of the item
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// Sortcode for the item
		/// </summary>
		/// <returns></returns>
		public string SortCode
		{
			get { return sortCode; }
			set { sortCode = value; }
		}

		/// <summary>
		/// ItemSelection code for the item
		/// </summary>
		/// <returns></returns>
		public ItemSelection ID
		{
			get { return id; }
			set { id = value; }
		}

		/// <summary>
		/// Returns the item as an XElement
		/// </summary>
		/// <returns></returns>
		public virtual XElement GetAsXML()
		{
			return GetAsXML(null);
		}

		/// <summary>
		/// Returns the item as an XElement
		/// </summary>
		/// <param name="tagName">Name of the created tag, use null for class name</param>
		/// <returns></returns>
		protected virtual XElement GetAsXML(string tagName)
		{
			XElement element = 
				new XElement(
					tagName ?? this.GetType().Name,
					new XAttribute("selection", ID.Selection),
					new XAttribute("sortCode", SortCode),
					new XElement("Description", Description),
					new XElement("Text", Text),
					this.IsNotOfType<Url>() ? new XElement("TextShort", TextShort) : null,
					Urls.Select(u => u.GetAsXML())
				);

			return element;
		}

		/// <summary>
		/// Sets an attribute not accesible as property
		/// </summary>
		/// <param name="name">Attribute name</param>
		/// <param name="value">Attribute value</param>
		public void SetAttribute(string name, object value)
		{
			attributes[name] = value;
		}

		/// <summary>
		/// Removes an attribute not accesible as property
		/// </summary>
		/// <param name="name">Attribute name</param>
		public void RemoveAttribute(string name)
		{
			attributes.Remove(name);
		}

		/// <summary>
		/// Gets an attribute not accesible as property
		/// </summary>
		/// <param name="name">Attribute name</param>
		public object GetAttribute(string name)
		{
			return attributes[name];
		}

		/// <summary>
		/// Gets and casts an attribute not accesible as property
		/// </summary>
		/// <param name="name">Attribute name</param>
		public T GetAttribute<T>(string name)
		{
			return GetAttribute(name).Cast<T>();
		}

		/// <summary>
		/// Does the link have a specific attribute.
		/// </summary>
		/// <param name="name">Name of the attribute to test for</param>
		/// <returns></returns>
		public bool HasAttribute(string name)
		{
			return attributes.ContainsKey(name);
		}

		/// <summary>
		/// Compare this item to another
		/// </summary>
		/// <param name="obj">Item to compare to</param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			if (obj is Item)
			{
				Item temp = (Item)obj;
				return this.sortCode.CompareTo(temp.SortCode);
			}

			throw new ArgumentException("Object is not a subclass of Item.");
		}

		/// <summary>
		/// Return a textual representation of the item for debug purposes
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return
				String.Format(
					"{0}: {1} ({2}) - \"{3}\"",
					GetType(),
					ID,
					SortCode,
					Text
				);
		}
	}
}