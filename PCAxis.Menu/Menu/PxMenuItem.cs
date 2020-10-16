using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using PCAxis.Menu.Extensions;

namespace PCAxis.Menu
{
	/// <summary>
	/// Item for containing a menu item with sub items
	/// </summary>
	public class PxMenuItem : Item
	{
		List<Item> subItems = new List<Item>();

		private PxMenuBase parentMenu;

		/// <summary>
		/// Create instance without data
		/// </summary>
		public PxMenuItem(PxMenuBase parentMenu) : this(parentMenu, "", "", "", "", "", "")
		{
		}

		/// <summary>
		/// Create instance with data
		/// </summary>
		/// <param name="parentMenu">The parent PxMenu for this item</param>
		/// <param name="text">Presentation text</param>
		/// <param name="textShort">Short presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the item</param>
		public PxMenuItem(PxMenuBase parentMenu, string text, string textShort, string sortCode, string menu, string selection, string description)
			: base(text, textShort, sortCode, menu, selection, description)
		{
			this.parentMenu = parentMenu;
		}

		/// <summary>
		/// Gets whether the item has subitems
		/// </summary>
		public bool HasSubItems
		{
			get { return subItems.Count > 0; }
		}

		/// <summary>
		/// Adds subitem to the menu item
		/// </summary>
		/// <param name="subItem">Item to add</param>
		public void AddSubItem(Item subItem)
		{
			if (subItem != null)
			{
				subItem.Parent = this;
				subItems.Add(subItem);
			}
		}

		/// <summary>
		/// Adds subitems to the menu item
		/// </summary>
		/// <param name="items">Items to add</param>
		public void AddSubItemRange(IEnumerable<Item> items)
		{
			foreach (Item i in items)
				AddSubItem(i);
		}

		/// <summary>
		/// Returns the collection of subitems for editing
		/// </summary>
		public List<Item> SubItems
		{
            get
            {
                return subItems;
            }
		}

        /// <summary>
        /// Returns the collection go subitems for presentation
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> SubItemsForPresentation()
        {
            return
                SubItemsForPresentation(false);
        }

		/// <summary>
		/// Returns the collection of subitems
		/// </summary>
		/// <param name="forceIncludeEmptyFolders">Whether to include empty folders items disregarding PxMenu-setting. Used for e.g. loading items.</param>
		/// <returns></returns>
		public IEnumerable<Item> SubItemsForPresentation(bool forceIncludeEmptyFolders)
		{
			if (forceIncludeEmptyFolders || !parentMenu.HideEmptyFolders)
				return subItems;
			else
				return subItems.Where(i => !(i is PxMenuItem) || i.Cast<PxMenuItem>().HasSubItems);
		}

		/// <summary>
		/// Returns the item as XML with all subitems
		/// </summary>
		/// <returns></returns>
		public override XElement GetAsXML()
		{
			return GetAsXML(-1);
		}

		/// <summary>
		/// Returns the item as XML
		/// </summary>
		/// <param name="noOfLevels">Number of levels to include in the XML</param>
		/// <returns></returns>
		public XElement GetAsXML(int noOfLevels)
		{
			XElement element = base.GetAsXML("MenuItem");

			if (noOfLevels != 0)
				element.Add(
					subItems.Select(
						i =>
						{
							if (i is PxMenuItem)
								return i.Cast<PxMenuItem>().GetAsXML(noOfLevels - 1);
							else
								return i.GetAsXML();
						}
					)
				);

			return element;
		}

		/// <summary>
		/// Returns an item if found in the subitems collection
		/// </summary>
		/// <param name="menu">Menu code of the requested item</param>
		/// <param name="selection">Selection code of the requested item</param>
		/// <returns></returns>
		public Item FindSelection(string menu, string selection)
		{
			Item m;

            if (ID.Match(menu, selection))
				return this;

			foreach (Item curItem in subItems)
			{
				if (curItem is PxMenuItem)
				{                    
                    m = curItem.Cast<PxMenuItem>().FindSelection(menu, selection);

					if (m != null)
						return m;
				}
                else if (curItem is Link)
                {
                    Link l = (Link)curItem;

                    if (l.ID.Selection.Equals(selection, StringComparison.OrdinalIgnoreCase))
                        return l;
                }
			}

			return null;
		}

		/// <summary>
		/// Sorts the subitems
		/// </summary>
		public void Sort()
		{
			subItems.Sort();

            foreach (Item item in subItems)
                if (item is PxMenuItem)
                    ((PxMenuItem)item).Sort();
		}
	}
}