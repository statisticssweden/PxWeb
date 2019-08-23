using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using System.Linq;
using System.Collections.Generic;

namespace PCAxis.Menu.Test
{
	[TestClass]
	public class DatamodelMenuTest
	{
		[TestMethod]
		public void MenuTest()
		{
			var menu = new TestMenuData();

			Assert.AreEqual("START", menu.RootItem.ID.Menu);
			Assert.AreEqual("START", menu.RootItem.ID.Selection);
			Assert.AreEqual(2, menu.RootItem.Cast<PxMenuItem>().SubItems.Count);
			Assert.IsTrue(menu.RootItem.Cast<PxMenuItem>().SubItems.All(x => !(x is PxMenuItem) || !x.Cast<PxMenuItem>().HasSubItems));

			menu.SetCurrentItemBySelection("START", "02");
			menu.GoUp();
			menu.SetCurrentItemBySelection("START", "05");

			//Gælder stadig
			Assert.AreEqual("START", menu.RootItem.ID.Menu);
			Assert.AreEqual("START", menu.RootItem.ID.Selection);
			Assert.AreEqual(2, menu.RootItem.Cast<PxMenuItem>().SubItems.Count);

			//Nu er der loaded subitems på et niveau mere
			Assert.IsTrue(menu.RootItem.Cast<PxMenuItem>().SubItems.All(x => !(x is PxMenuItem) || x.Cast<PxMenuItem>().HasSubItems));
		}

		[TestMethod]
		public void MenuTestLevels()
		{
			var menu = new TestMenuData(numberOfLevels: 9);

			Assert.AreEqual("START", menu.RootItem.ID.Menu);
			Assert.AreEqual("START", menu.RootItem.ID.Selection);
			Assert.AreEqual(2, menu.RootItem.Cast<PxMenuItem>().SubItems.Count);

			foreach (var level1 in menu.RootItem.SubItems)
			{
				Assert.IsTrue(level1.Cast<PxMenuItem>().HasSubItems);

				foreach (var level2 in level1.Cast<PxMenuItem>().SubItems)
				{
					Assert.IsTrue(level2.Cast<PxMenuItem>().HasSubItems);

					foreach (var level3 in level2.Cast<PxMenuItem>().SubItems)
					{
						Assert.IsFalse(level3 is PxMenuItem);
					}
				}
			}
		}

		[TestMethod]
		public void MenuWithUrlTest()
		{
			var menu = new TestMenuDataSweden(numberOfLevels: 1);
			
			menu.SetCurrentItemBySelection("START", "KU");
			Assert.AreEqual(0, menu.CurrentItem.Urls.Count());
			Assert.AreEqual(1, menu.CurrentItem.Cast<PxMenuItem>().SubItems.Count);
			Assert.IsTrue(menu.CurrentItem.Cast<PxMenuItem>().GetAttribute<bool>("HasBeenLoaded"));
			
			PxMenuItem testItem;
			
			testItem = menu.CurrentItem.Cast<PxMenuItem>().SubItems.First().Cast<PxMenuItem>();
			Assert.AreEqual("KU", testItem.ID.Menu);
			Assert.AreEqual("KU0101", testItem.ID.Selection);
			Assert.AreEqual(1, testItem.Urls.Count());
			Assert.IsFalse(testItem.HasSubItems);
			Assert.IsFalse(testItem.HasAttribute("HasBeenLoaded"));

			//Efter at være valgt skal den være den samme, stadig har url, men også have subitems.
			menu.SetCurrentItemBySelection("KU", "KU0101");
			Assert.AreEqual("KU", testItem.ID.Menu);
			Assert.AreEqual("KU0101", testItem.ID.Selection);
			Assert.AreEqual(1, testItem.Urls.Count());
			Assert.IsTrue(testItem.HasSubItems);
			Assert.IsTrue(testItem.GetAttribute<bool>("HasBeenLoaded"));

			//Test også med flere niveauer
			var menu2 = new TestMenuDataSweden(numberOfLevels: 9);

			menu2.SetCurrentItemBySelection("START", "KU");
			Assert.AreEqual(0, menu2.CurrentItem.Urls.Count());
			Assert.AreEqual(1, menu2.CurrentItem.Cast<PxMenuItem>().SubItems.Count);

			testItem = menu2.CurrentItem.Cast<PxMenuItem>().SubItems.First().Cast<PxMenuItem>();
			Assert.AreEqual("KU", testItem.ID.Menu);
			Assert.AreEqual("KU0101", testItem.ID.Selection);
			Assert.AreEqual(1, testItem.Urls.Count());
			Assert.IsTrue(testItem.HasSubItems);
			Assert.IsTrue(testItem.GetAttribute<bool>("HasBeenLoaded"));

			//Efter at være valgt skal den være den samme.
			menu2.SetCurrentItemBySelection("KU", "KU0101");
			Assert.AreEqual("KU", testItem.ID.Menu);
			Assert.AreEqual("KU0101", testItem.ID.Selection);
			Assert.AreEqual(1, testItem.Urls.Count());
			Assert.IsTrue(testItem.HasSubItems);
			Assert.IsTrue(testItem.GetAttribute<bool>("HasBeenLoaded"));
		}
	}

	static class TestExtensions
	{
		public static T Cast<T>(this object o)
		{
			return (T)o;
		}
	}
}
