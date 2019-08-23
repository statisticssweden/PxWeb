using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using System.Linq;
using System.Collections.Generic;

namespace PCAxis.Menu.Test
{
	abstract class TestMenuLikeDatabase : DatamodelMenu
	{
		protected DataTable data = new DataTable();

		public TestMenuLikeDatabase(int numberOfLevels = 1)
			: base(
				':',
				"da",
				x =>
				{
					x.AddSqlHints(SqlHint.UseConnectByPrior);
					x.NumberOfLevels = numberOfLevels;

					x.Cast<TestMenuLikeDatabase>().data.Columns.AddRange(
						new DataColumn[]
						{
							new DataColumn("menu"),
							new DataColumn("selection"),
							new DataColumn("itemtype"),
							new DataColumn("prestext"),
							new DataColumn("prestexts"),
							new DataColumn("description"),
							new DataColumn("levelno"),
							new DataColumn("sortcode"),
							new DataColumn("presentation"),
							new DataColumn("linkid"),
							new DataColumn("linkurl"),
							new DataColumn("linkpres"),
							new DataColumn("tablestatus"),
							new DataColumn("published"),
							new DataColumn("lastupdated"),
							new DataColumn("tableid"),
							new DataColumn("category"),
							new DataColumn("starttime"),
							new DataColumn("endtime"),
						}
					);

					x.Cast<TestMenuLikeDatabase>().addData();
				}
			)
		{
		}

		protected abstract void addData();

		public override DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters)
		{
			DataTable result = new DataTable();
			result.Columns.AddRange(data.Columns.OfType<DataColumn>().Select(x => new DataColumn(x.ColumnName)).ToArray());

			foreach (var r in getItems(menu, selection, numberOfLevels))
				result.ImportRow(r);

			return result;
		}

		IEnumerable<DataRow> getItems(string menu, string selection, int numberOfLevels, int level = 0, string parentSelection = null)
		{ 
			var q = from r in data.Rows.OfType<DataRow>()
					where
						(level <= numberOfLevels - (parentSelection == "START" ? 1 : 0) + (r["itemtype"].ToString() == "url" ? 1 : 0))
						&&
						r["menu"].ToString() == menu 
						&& 
						(selection == null || selection == "START" || r["selection"].ToString() == selection)
					from s in new DataRow[] { r }.Concat(getItems(r["selection"].ToString(), null, numberOfLevels, level + 1, selection))
					select s;

			return q;
		}
	}

	class TestMenuData : TestMenuLikeDatabase
	{
		public TestMenuData(int numberOfLevels = 1)
			: base(numberOfLevels)
		{
		}

		protected override void addData()
		{
			data.Rows.Add("START", "02", "menu", "Befolkning og valg", "02", "Befolkning og valg", "1", "102", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("02", "2401", "menu", "Befolkning og befolkningsfremskrivning", "2401", "Befolkning og befolkningsfremskrivning", "2", "101", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2401", "10021", "table", null, null, "Folketal", "3", "101", "P", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2401", "FOLK1", "table", "Folketal den 1. i kvartalet efter kommune køn alder civilstand herkomst oprindelsesland og statsborgerskab", "Folketal den 1. i kvartalet", null, "3", "102", "A", null, null, null, "A", "12/11/2013 09:30:00", "04/11/2013 12:14:20", "FOLK1", "O", "2008K1", "2014K1");
			data.Rows.Add("02", "2402", "menu", "Indvandrere og efterkommere", "2402", "Indvandrere og efterkommere", "2", "102", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2402", "10024", "table", null, null, "Indvandrere og efterkommere", "3", "101", "P", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2402", "FOLK3", "table", "Folketal 1. januar efter fødselsdag fødselsmåned og fødselsår", "Folketal 1. januar", null, "3", "103", "A", null, null, null, "A", "01/01/3000", "01/01/3000", "FOLK3", "O", "2008", "2014");
			data.Rows.Add("2402", "FOLK1", "table", "Folketal den 1. i kvartalet efter kommune køn alder civilstand herkomst oprindelsesland og statsborgerskab", "Folketal den 1. i kvartalet", null, "3", "103", "A", null, null, null, "A", "12/11/2013 09:30:00", "04/11/2013 12:14:20", "FOLK1", "O", "2008K1", "2014K1");
			data.Rows.Add("START", "05", "menu", "Levevilkår", "05", "Levevilkår", "1", "103", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("05", "2477", "menu", "Velfærdsindikatorer", "2477", "Velfærdsindikatorer", "2", "101", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2477", "10054", "table", null, null, "Velfærdsindikatorer", "3", "101", "P", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("2477", "IFV1", "table", "Husholdningsmedlemmer efter køn alder og hvor let husholdningen synes det er at få pengene til at slå til", "Husholdningsmedlemmer", null, "3", "102", "A", null, null, null, "A", "26/10/2012 09:30:00", "18/10/2012 09:29:23", "IFV1", "O", "2005", "2012");
			data.Rows.Add("2477", "IFV3", "table", "Husholdningsmedlemmer efter køn alder socio-økonomisk status og hvor let husholdningen synes det er at få pengene til at slå til", "Husholdningsmedlemmer", null, "3", "103", "A", null, null, null, "A", "26/10/2012 09:30:00", "20/09/2013 11:13:54", "IFV3", "O", "2005", "2012");
		}
	}

	class TestMenuDataSweden : TestMenuLikeDatabase
	{
		public TestMenuDataSweden(int numberOfLevels = 1)
			: base(numberOfLevels)
		{
		}

		protected override void addData()
		{
			data.Rows.Add("START", "KU", "menu", "Kultur och fritid", "", "", "1", "", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("KU", "KU0101", "menu", "Folkbiblioteksstatistik", "", "", "3", "", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("KU0101", "KU0101_S", "menu", "Folkbibliotekssystemet", "", "", "3", "", "A", null, null, null, null, null, null, null, null, null, null);
			data.Rows.Add("KU0101_S", "TAB", "table", "Testtabel med bib.stat.", "Testtabel", null, "3", "102", "A", null, null, null, "A", "26/10/2012 09:30:00", "18/10/2012 09:29:23", "TAB", "O", "2005", "2012");
			data.Rows.Add("KU0101", "", "url", "Kulturrådet", "", "", "", "", "", "8", "http://www.dst.dk", "B", null, null, null, null, "O", null, null);
		}
	}
}
