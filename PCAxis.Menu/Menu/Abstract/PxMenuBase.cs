using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using PCAxis.Menu.Extensions;
using PCAxis.Menu.Exceptions;

namespace PCAxis.Menu
{
	/// <summary>
	/// Abstract class for creating versions of the Menu object such as e.g. XMLMenu, SQLMenu etc.
	/// </summary>
	public abstract class PxMenuBase
	{
		private PxMenuItem rootItem = null;
		private Item currentItem;

		/// <summary>
		/// Gets or sets the language code, e.g. "da".
		/// </summary>
		public string Language;

		/// <summary>
		/// Gets or sets whether to hide empty folders.
		/// </summary>
		public virtual bool HideEmptyFolders { get; set; }

		/// <summary>
		/// Determines whether a Link is selected.
		/// </summary>
		public bool LinkSelected
		{
			get
			{
				return CurrentItem is Link;
			}
		}

		/// <summary>
		/// Determines whether the item has constructed it's rootitem at this point
		/// </summary>
		protected bool RootItemIsSet
		{
			get { return rootItem != null; }
		}

		/// <summary>
		/// Returns the root item.
		/// </summary>
		public PxMenuItem RootItem
		{
			get { return rootItem ?? new PxMenuItem(this); }
			protected set { rootItem = value; }
		}

		/// <summary>
		/// Returns the currently selected item.
		/// </summary>
		public Item CurrentItem
		{
			get { return currentItem ?? rootItem; }
			protected set { currentItem = value; }
		}

		/// <summary>
		/// Move up one level in the menu hierarchy. 
		/// </summary>
		public void GoUp()
		{
			CurrentItem = CurrentItem.Parent;
		}

		/// <summary>
		/// Set the selected item, search from the root item
		/// </summary>
		/// <param name="menu">The menu of the item to select</param>
		/// <param name="selection">The selection of the item to select</param>
		/// <returns></returns>
		public virtual bool SetCurrentItemBySelection(string menu, string selection)
		{
			PxMenuItem startFrom = CurrentItem is PxMenuItem ? CurrentItem.Cast<PxMenuItem>() : RootItem;

			Item foundItem = startFrom.FindSelection(menu, selection);

			if (foundItem == null)
				return false;

			currentItem = foundItem;
			return true;
		}

		/// <summary>
		/// Returns the current level as XML
		/// </summary>
		/// <returns></returns>
		public XDocument GetCurrentItemAsXML()
		{
			return GetCurrentItemAsXML(-1);
		}

		/// <summary>
		/// Returns the menu as XML
		/// </summary>
		/// <returns></returns>
		public XDocument GetAsXML()
		{
			return GetAsXML(true);
		}

		/// <summary>
		/// Returns the menu as XML
		/// </summary>
		/// <param name="isDefault">Sets the default attribute of the menu in the XML</param>
		/// <returns></returns>
		public XDocument GetAsXML(bool isDefault)
		{
			return
				new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					new XElement("Menu",
						new XElement("Language",
							new XAttribute("lang", this.Language),
							new XAttribute("default", isDefault ? "yes" : "no"),
							RootItem.GetAsXML()
						)
					)
				);
		}

		/// <summary>
		/// Returns the current level as XML for a given number of levels
		/// </summary>
		/// <param name="noOfLevels">Number of levels to return</param>
		/// <returns></returns>
		public XDocument GetCurrentItemAsXML(int noOfLevels)
		{
			return
				new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					CurrentItem is PxMenuItem ? CurrentItem.Cast<PxMenuItem>().GetAsXML(noOfLevels) : CurrentItem.GetAsXML()
				);
		}

		/// <summary>
		/// Delegate for initialization lambda.
		/// </summary>
		/// <param name="menu"></param>
		public delegate void MenuInitialization(PxMenuBase menu);

		/// <summary>
		/// Method decribing how to retrieve data for and store extra attributes for Link-based items.
		/// </summary>
		/// <param name="link"></param>
		/// <returns></returns>
		public delegate void RetrieveAttributesMethod(Link link);

		/// <summary>
		/// Method describing how to determine which items to include in the menu of the items retrieved from the datasource.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public delegate bool RestrictionMethod(Item item);

		/// <summary>
		/// The default method to determine whether items retrieved from datasource should be saved in the menu.
		/// This functionality can be overridden by the Restriction setting of the DatamodelMenu object.
		/// DefaultRestriction() could be called from such an overriding restriction.
		/// </summary>
		/// <param name="item">Item to check</param>
		/// <returns></returns>
		public static bool DefaultRestriction(Item item)
		{
			if (item.IsNotOfType<Link>())
				return true;

			Link link = (Link)item;

			if (link is TableLink && link.Cast<TableLink>().Status != TableStatus.AccessibleToAll)
				return false;

			if (link.Category == PresCategory.Official)
				return true;

			return false;
		}

		private RestrictionMethod itemRestriction = null;

		/// <summary>
		/// RestrictionMethod describing how to determine which items to include in the menu of the items retrieved from the datasource
		/// Consider using DefaultRestriction in your new Restriction in additon to your added functionality.
		/// </summary>
		public RestrictionMethod Restriction
		{
			get
			{
				return itemRestriction ?? DefaultRestriction;
			}

			set
			{
				if (RootItemIsSet)
					throw new MenuHasBeenInitializedException("Changing the RestrictionMethod of DatamodelMenu after initialization could result in different restrictions for different levels and threads of the loaded menu structure.");

				itemRestriction = value;
			}
		}

		/// <summary>
		/// Determines whether the items is restricted in a non default manner
		/// </summary>
		public bool IsRestricted
		{
			get { return itemRestriction != null; }
		}

		/// <summary>
		/// Method describing what to do to an item before storing it.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public delegate void AlterItemBeforeStorageMethod(Item item);

		/// <summary>
		/// Method (e.g. as a Lambda expression) describing what to do to an item before storing it.
		/// </summary>
		public AlterItemBeforeStorageMethod AlterItemBeforeStorage;
	}
}
