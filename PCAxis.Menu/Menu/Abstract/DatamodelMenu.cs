using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using PCAxis.Menu;
using PCAxis.Menu.Extensions;
using PCAxis.Menu.Exceptions;
using System.Text.RegularExpressions;
using PCAxis.Menu.Implementations.DatamodelMenuExtensions;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// Abstract class which only needs code for retrieving data from the database based on given SQL.
	/// Sub class constructor must call Initialize().
	/// </summary>
	public abstract partial class DatamodelMenu : PxMenuBase
	{
		private log4net.ILog log = log4net.LogManager.GetLogger("PCAxis.Menu.Implementations.DatamodelMenu");

		private char parameterChar;

		/// <summary>
		/// Descriptions of the parameters used to retrieve data from the database
		/// </summary>
		public DatabaseParameterCollection Parameters =
			new DatabaseParameterCollection(
				new DatabaseParameter("levels", 10, typeof(int)),
				new DatabaseParameter("menu", 20, typeof(string)),
				new DatabaseParameter("selection", 20, typeof(string))
			);

		/// <summary>
		/// The SQL to use instead of default Datamodel-SQL.
		/// Use {0} to mark positions of parameter char, e.g. ":" or "@".
		/// Use {1} to mark positions of the table name ending for extraction language code, e.g. "_ENG".
		/// </summary>
		public string OverrideSQL = null;

		private string extractionLanguageCode = null;

		/// <summary>
		/// The extension on the tables in the datamodel containing the current secondary language. This value should in NO WAY originate from the GUI!
		/// </summary>
		public string ExtractionLanguageCode
		{
			get { return extractionLanguageCode; }
			set
			{
				if (value.Length > 6 || !Regex.IsMatch(value, "^[a-zA-Z0-9_]*$"))
					throw new ValueNotSafeForSQLException();

				extractionLanguageCode = value;
			}
		}

		/// <summary>
		/// Selection used for root, when creating menus starting at lower levels of the menu hierachy.
		/// </summary>
		public ItemSelection RootSelection = new ItemSelection();

		/// <summary>
		/// Method (e.g. as a Lambda expression) describing how to retrieve data for and store extra attributes for Link-based items.
		/// </summary>
		public RetrieveAttributesMethod RetrieveAttributes;

		/// <summary>
		/// Constructor to be called from sub classses.
		/// </summary>
		/// <param name="parameterChar">Char used to indicate binding variables in database</param>
		/// <param name="language">Language code for this context</param>
		protected DatamodelMenu(char parameterChar, string language)
			: this(parameterChar, language, null)
		{ }

		/// <summary>
		/// Delegate with specific type for initialization lambda.
		/// </summary>
		/// <param name="datamodelMenu"></param>
		public delegate void DatamodelMenuInitialization(DatamodelMenu datamodelMenu);

		/// <summary>
		/// Constructor to be called from sub classses.
		/// </summary>
		/// <param name="parameterChar">Char used to indicate binding variables in database. This value should in NO WAY originate from the GUI!</param>
		/// <param name="language">Language code for this context</param>
		/// <param name="initializationFunction">Function for initializing this instance</param>
		protected DatamodelMenu(char parameterChar, string language, DatamodelMenuInitialization initializationFunction)
		{
			log.DebugFormat("Creating instance with parameterchar, language: {0}, {1}", parameterChar, language);

			Language = language;
			this.ParameterChar = parameterChar;

			if (initializationFunction != null)
			{
				log.Debug("Initialization supplied.");
				initializationFunction(this);
			}

			if (AlterSQL != null)
			{
				log.Debug("AlterSQL supplied.");
				OverrideSQL = AlterSQL(SQL);
			}

			RootItem = new PxMenuItem(this, "", "", "", RootSelection.Menu, RootSelection.Selection, "");
			SetCurrentItemBySelection(RootSelection.Menu, RootSelection.Selection);
		}

		/// <summary>
		/// Char used to indicate binding variables in database.
		/// </summary>
		public char ParameterChar
		{
			get { return parameterChar; }
			private set
			{
				if (!Regex.IsMatch(value.ToString(), "^[:@]{1}$"))
					throw new ValueNotSafeForSQLException();

				parameterChar = value;
			}
		}

		private List<SqlHint> sqlHints = new List<SqlHint>();

		/// <summary>
		/// Adds one or more hints on how to create the SQL used for extracting the menu.
		/// </summary>
		/// <param name="hints">Hints to add</param>
		public void AddSqlHints(params SqlHint[] hints)
		{
			log.DebugFormat("Hints supplied: {0}", String.Join(", ", hints.Select(h => h.ToString()).ToArray()));

			sqlHints.AddRange(hints);

			if (sqlHints.Contains(SqlHint.UseConnectByPrior) && sqlHints.Contains(SqlHint.UseRecursiveCTE))
				throw new SqlHintException("It's not allowed to add both UseConnectByPrior and UseRecursiveCTE.");

			if (sqlHints.Contains(SqlHint.UseNedstedSelect) && !sqlHints.Contains(SqlHint.UseConnectByPrior))
				throw new SqlHintException("UseNedstedSQL can only be used together with UseConnectByPrior.");
		}

		/// <summary>
		/// Removes a hint on how to create the SQL used for extracting the menu.
		/// </summary>
		/// <param name="hint">Hint to remove</param>
		public void RemoveSqlHint(SqlHint hint)
		{
			sqlHints.Remove(hint);
		}

		/// <summary>
		/// Hints on how to create the SQL used for extracting the menu.
		/// </summary>
		public enum SqlHint
		{
			/// <summary>
			/// Gets or sets whether to use connect by prior for extracting hierarchical data.
			/// This cannot be set along with UseRecursiveCTE.
			/// </summary>
			UseConnectByPrior,
			/// <summary>
			/// Gets or sets whether to use recursive CTE for extracting hierarchical data.
			/// This cannot be set along with UseConnectByPrior.
			/// </summary>
			UseRecursiveCTE,
			/// <summary>
			/// Gets or sets whether to use nested SQL instead of WITH clause.
			/// Can only be used together with with UseConnectByPrior.
			/// </summary>
			UseNedstedSelect
		}

		/// <summary>
		/// Returns the string which is formatted to create the SQL used for retrieving data from the datasource.
		/// {0} mark positions of parameter char, e.g. ":" or "@".
		/// {1} mark positions of the table name ending of tables with the non default language, e.g. "_Eng".
		/// </summary>
		public string FormatSql
		{
			get
			{
				string baseWith =
					@"(
						select  
							/*[MenuSelection.Menu*/ms.menu/*]*/ menu, 
							/*[MenuSelection.Selection*/ms.selection/*]*/ selection,
							case when /*[MenuSelection.LevelNo*/ms.levelno/*]*/ = (select /*[MetaAdm.Value*/metaadm.value/*]*/ from /*[MetaAdm*/metaadm/*]*/ where lower(/*[MetaAdm.Property*/metaadm.property/*]*/) /*[MenuLevels*/= 'menulevels'/*]*/) then
								'table' 
							else 
								'menu' 
							end itemtype, 
							pxlangms./*[MenuSelection{1}.PresText*/prestext/*]*/ prestext,
							pxlangms./*[MenuSelection{1}.PresTextS*/prestexts/*]*/ prestexts,
							pxlangms./*[MenuSelection{1}.Description*/description/*]*/ description,  
							/*[MenuSelection.LevelNo*/ms.levelno/*]*/ levelno, 
							pxlangms./*[MenuSelection{1}.SortCode*/sortcode/*]*/ sortcode,  
							pxlangms./*[MenuSelection.Presentation*/presentation/*]*/ presentation,
							null linkid
						from /*[MenuSelection*/menuselection ms/*]*/ left outer join /*[MenuSelection{1} pxlangms*/menuselection{1} pxlangms/*]*/ on pxlangms./*[MenuSelection{1}.Menu*/menu/*]*/ = /*[MenuSelection.Menu*/ms.menu/*]*/ and pxlangms./*[MenuSelection{1}.Selection*/selection/*]*/ = /*[MenuSelection.Selection*/ms.selection/*]*/ 
							where 
								lower(pxlangms./*[MenuSelection.Presentation*/presentation/*]*/) in ('a', 'p')
								and /*[MenuSelection.LevelNo*/ms.levelno/*]*/ <= (select /*[MetaAdm.Value*/metaadm.value/*]*/ from /*[MetaAdm*/metaadm/*]*/ where lower(/*[MetaAdm.Property*/metaadm.property/*]*/) /*[MenuLevels*/= 'menulevels'/*]*/)
						union
						select
							pxlangms./*[MenuSelection{1}.Selection*/selection/*]*/ menu, 
							null selection, 
							'url' itemtype, 
							null prestext,  
							null prestexts,  
							null description,  
							null levelno,
							null sortcode,  
							null presentation,
							/*[LinkMenuSelection.LinkId*/lms.linkid/*]*/ linkid
						from /*[MenuSelection*/menuselection ms/*]*/ left outer join /*[MenuSelection{1} pxlangms*/menuselection{1} pxlangms/*]*/ on pxlangms./*[MenuSelection{1}.Menu*/menu/*]*/ = /*[MenuSelection.Menu*/ms.menu/*]*/ and pxlangms./*[MenuSelection{1}.Selection*/selection/*]*/ = /*[MenuSelection.Selection*/ms.selection/*]*/ 
						inner join /*[LinkMenuSelection*/linkmenuselection lms/*]*/ on /*[LinkMenuSelection.Menu*/lms.menu/*]*/ = pxlangms./*[MenuSelection{1}.Menu*/menu/*]*/ and /*[LinkMenuSelection.Selection*/lms.selection/*]*/ = pxlangms./*[MenuSelection{1}.Selection*/selection/*]*/
					)";

				string selectedWith;

				if (sqlHints.Contains(SqlHint.UseConnectByPrior))
				{
					selectedWith =
						@"(
							select * from /*<WITH*/base/*WITH>*/ base
							where level <= case when {0}selection = 'START' then case when base.itemtype = 'url' then {0}levels + 1 else {0}levels end else case when base.itemtype = 'url' then {0}levels + 2 else {0}levels + 1 end end
							start with case when {0}selection = 'START' then menu else selection end = {0}selection and menu = {0}menu connect by prior selection = menu
						)";
				}
				else
				{
					selectedWith =
						@"(
							select
								menu, 
								selection, 
								itemtype, 
								prestext,  
								prestexts,  
								description,  
								levelno,
								sortcode,  
								presentation,
								linkid,
								1 as ctelevel
							from base
								where 
									/* start with */ case when {0}selection = 'START' then menu else selection end = {0}selection and menu = {0}menu
							union all
							select
								base.menu, 
								base.selection, 
								base.itemtype, 
								base.prestext,  
								base.prestexts,  
								base.description,  
								base.levelno,
								base.sortcode,  
								base.presentation,
								base.linkid,
								ctelevel + 1 ctelevel
							from base
							inner join PxMenuCTE1 on /* connect by prior */ PxMenuCTE1.selection = base.menu
							where ctelevel <= case when {0}selection = 'START' then case when base.itemtype = 'url' then {0}levels else {0}levels - 1 end else case when base.itemtype = 'url' then {0}levels + 1 else {0}levels end end
						), selected as
						(
							select *
							from PxMenuCTE1
						)";
				}

				string resultsWith =
					@"(
						select 
							menu, 
							selection, 
							itemtype,
							case when itemtype = 'table' then pxlangm./*[MainTable{1}.PresText*/prestext/*]*/ when itemtype = 'url' then pxlangl./*[Link{1}.LinkText*/linktext/*]*/ else selected.prestext end prestext,  
							case when itemtype = 'table' then pxlangm./*[MainTable{1}.PresTextS*/prestexts/*]*/ else selected.prestexts end prestexts,  
							case when itemtype = 'url' then pxlangl./*[Link{1}.Description*/description/*]*/ else selected.description end description,  
							levelno,
							case when itemtype = 'url' then pxlangl./*[Link{1}.SortCode*/sortcode/*]*/ else selected.sortcode end sortcode,  
							presentation,
							selected.linkid,
							pxlangl./*[Link{1}.Link*/link/*]*/ linkurl,
							/*[Link.LinkPres*/l.linkpres/*]*/ linkpres,
							/*[MainTable.TableStatus*/m.tablestatus/*]*/ tablestatus,
							/*[Contents.Published*/c.published/*]*/ published,
							/*[Contents.LastUpdated*/c.lastupdated/*]*/ lastupdated,
							/*[MainTable.TableId*/m.tableid/*]*/ tableid,
							case when itemtype = 'url' then /*[Link.PresCategory*/l.prescategory/*]*/ else /*[MainTable.PresCategory*/m.prescategory/*]*/ end category,
							min(/*[ContentsTime.TimePeriod*/ct.timeperiod/*]*/) starttime,
							max(/*[ContentsTime.TimePeriod*/ct.timeperiod/*]*/) endtime
						from /*<WITH*/selected/*WITH>*/ selected
						left outer join /*[MainTable*/maintable m/*]*/ on /*[MainTable.MainTable*/m.maintable/*]*/ = selected.selection left outer join /*[MainTable{1} pxlangm*/maintable{1} pxlangm/*]*/ on pxlangm./*[MainTable{1}.MainTable*/maintable/*]*/ = /*[MainTable.MainTable*/m.maintable/*]*/
						left outer join /*[Link*/link l/*]*/ on /*[Link.LinkId*/l.linkid/*]*/ = selected.linkid left outer join /*[Link{1} pxlangl*/link{1} pxlangl/*]*/ on pxlangl./*[Link{1}.LinkId*/linkid/*]*/ = /*[Link.LinkId*/l.linkid/*]*/
						left outer join /*[Contents*/contents c/*]*/ on /*[Contents.MainTable*/c.maintable/*]*/ = selected.selection
						left outer join /*[ContentsTime*/contentstime ct/*]*/ on selected.itemtype = 'table' and /*[ContentsTime.MainTable*/ct.maintable/*]*/ = selected.selection
						group by
							menu, 
							selection, 
							itemtype,
							case when itemtype = 'table' then pxlangm./*[MainTable{1}.PresText*/prestext/*]*/ when itemtype = 'url' then pxlangl./*[Link{1}.LinkText*/linktext/*]*/ else selected.prestext end,  
							case when itemtype = 'table' then pxlangm./*[MainTable{1}.PresTextS*/prestexts/*]*/ else selected.prestexts end,  
							case when itemtype = 'url' then pxlangl./*[Link{1}.Description*/description/*]*/ else selected.description end,  
							levelno,
							case when itemtype = 'url' then pxlangl./*[Link{1}.SortCode*/sortcode/*]*/ else selected.sortcode end,  
							presentation,
							selected.linkid,
							pxlangl./*[Link{1}.Link*/link/*]*/,
							/*[Link.LinkPres*/l.linkpres/*]*/,
							/*[MainTable.TableStatus*/m.tablestatus/*]*/,
							/*[Contents.Published*/c.published/*]*/,
							/*[Contents.LastUpdated*/c.lastupdated/*]*/,
							/*[MainTable.TableId*/m.tableid/*]*/,
							case when itemtype = 'url' then /*[Link.PresCategory*/l.prescategory/*]*/ else /*[MainTable.PresCategory*/m.prescategory/*]*/ end
					)";

				string finalSql;

				if (sqlHints.Contains(SqlHint.UseConnectByPrior))
				{
					finalSql =
						@"
							select * from /*<WITH*/results/*WITH>*/ results
							start with case when {0}selection = 'START' then menu else selection end = {0}selection connect by prior selection = menu
							order siblings by sortcode
						";
				}
				else
				{
					finalSql =
						@"(
								select 
									menu, 
									selection, 
									itemtype,
									prestext,  
									prestexts,  
									description,  
									levelno,
									sortcode,  
									presentation,
									linkid,
									linkurl,
									linkpres,
									tablestatus,
									published,
									lastupdated,
									tableid,
									category,
									starttime,
									endtime,
									1 as ctelevel
								from results
									where 
										/* start with */ case when {0}selection = 'START' then menu else selection end = {0}selection
								union all
								select
									results.menu, 
									results.selection, 
									results.itemtype,
									results.prestext,  
									results.prestexts,  
									results.description,  
									results.levelno,
									results.sortcode,  
									results.presentation,
									results.linkid,
									results.linkurl,
									results.linkpres,
									results.tablestatus,
									results.published,
									results.lastupdated,
									results.tableid,
									results.category,
									results.starttime,
									results.endtime,
									ctelevel + 1 ctelevel
								from results
								inner join PxMenuCTE2 on /* connect by prior */ PxMenuCTE2.selection = results.menu
							)
							select *
							from PxMenuCTE2
							order by ctelevel, sortcode
						";
				}

				StringBuilder sb = new StringBuilder();

				if (!sqlHints.Contains(SqlHint.UseNedstedSelect))
				{
					sb.Append("with base as ");
					sb.Append(baseWith);
					sb.Append(sqlHints.Contains(SqlHint.UseRecursiveCTE) ? ", PxMenuCTE1 as " : ", selected as ");
					sb.Append(selectedWith);
					sb.Append(", results as ");
					sb.Append(resultsWith);
					sb.Append(sqlHints.Contains(SqlHint.UseRecursiveCTE) ? ", PxMenuCTE2 as " : "");
					sb.Append(finalSql);
				}
				else
				{
					if (!sqlHints.Contains(SqlHint.UseConnectByPrior))
					{
						//This should not be possible.
						throw new SqlHintException("Either SqlHint.UseNedstedSelect must be left out or SqlHint.UseConnectByPrior should be added.");
					}

					selectedWith = selectedWith.Replace("/*<WITH*/base/*WITH>*/", baseWith);
					resultsWith = resultsWith.Replace("/*<WITH*/selected/*WITH>*/", selectedWith);
					finalSql = finalSql.Replace("/*<WITH*/results/*WITH>*/", resultsWith);

					sb.Append(finalSql);
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Changes to SQL
		/// </summary>
		public Func<string, string> AlterSQL = null;

		/// <summary>
		/// Returns the formatted SQL used to retrieve data from the datasource
		/// </summary>
		protected virtual string SQL
		{
			get
			{
				if (parameterChar == 0)
					throw new NoParameterCharException("ParameterChar must be set.");

				return String.Format(
					OverrideSQL ?? FormatSql,
					ParameterChar,
					ExtractionLanguageCode
				);
			}
		}

		/// <summary>
		/// Returns true if an alternative SQL is used.
		/// </summary>
		public bool SqlIsOverridden
		{
			get { return OverrideSQL != null; }
		}

		/// <summary>
		/// Retrieves data from database for a given SQL. The method must be implemented in the derived class.
		/// Items must be returned in such an order, that a parent is before a child of that item.
		/// </summary>
		/// <param name="menu">The menu for which to retrieve data</param>
		/// <param name="selection">The selection for which to retrieve data</param>
		/// <param name="numberOfLevels">Number of levels to retrieve from the database</param>
		/// <param name="sql">The SQL supplied by the base class</param>
		/// <param name="parameters">List of parameters</param>
		/// <returns></returns>
		/// Parameters are actually accessible. They are included for implicit documentation.
		public abstract DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters);

		/// <summary>
		/// The number of levels retrieved from the database on each load. Default value = 1.
		/// </summary>
		public int NumberOfLevels = 1;

		/// <summary>
		/// Loads properties and subitems for a certain MenuItem.
		/// </summary>
		/// <param name="menuItem">The item to update</param>
		protected void Load(PxMenuItem menuItem)
		{
			Load(menuItem, NumberOfLevels);
		}

		/// <summary>
		/// Loads properties and subitems for a certain MenuItem.
		/// </summary>
		/// <param name="menuItem">The item to update</param>
		/// <param name="numberOfLevels">Number of levels to retrieve from the database - overrides property NumberOfLevels</param>
		/// <returns></returns>
		protected void Load(PxMenuItem menuItem, int numberOfLevels)
		{
			//if (menuItem.HasSubItems)
			//    throw new ItemHasBeenLoadedException("DatamodelMenu does not allow subsequent loads on its MenuItems.");

			if (menuItem.HasBeenLoaded())
				throw new ItemHasBeenLoadedException("DatamodelMenu does not allow subsequent loads on its MenuItems.");

			menuItem.HasBeenLoaded(true);

			DataTable dataTable = GetDataTableBySelection(menuItem.ID.Menu, menuItem.ID.Selection, numberOfLevels, SQL, Parameters);

			List<Item> items = itemCollection(dataTable);

			Item self = null;

			if (menuItem.ID.Selection != "START")
			{
				self = items.FirstOrDefault(i => i is PxMenuItem && i.ID.Equals(menuItem.ID));

				if (self == null)
				{
					menuItem.ID.Set("", "");
					return;
				}

				menuItem.Text = self.Text;
				menuItem.TextShort = self.TextShort;
				menuItem.SortCode = self.SortCode;
				menuItem.ID.Set(self.ID.Values);
				menuItem.Description = self.Description;

				items.Remove(self);
			}

			PxMenuItem parent = null;

			for (int i = 0; i < items.Count; i++)
			{
				Item item = items[i];

				if (item.ID.Menu.Equals(menuItem.ID.Selection))
					parent = menuItem;
				else if (item.ID.Menu != parent.ID.Selection)
				{
					Item tempParent = items.Take(i).Last(x => x.ID.Selection.Equals(item.ID.Menu));

					if (tempParent.IsNotOfType<PxMenuItem>())
						if (tempParent.ID.Selection.Equals(item.ID.Menu))
							throw new Exceptions.DatamodelMenuExtractedDataErrorException("Non-menu type item has subitem. Not allowed.");
						else
							throw new Exceptions.DatamodelMenuExtractedDataErrorException("Unable to find items parent in extracted data.");

					parent = tempParent.Cast<PxMenuItem>();
				}

				if (parent == null)
					throw new Exceptions.DatamodelMenuExtractedDataErrorException("Unable to find items parent in extracted data.");

				//TODO: Test Url?
				if (item is Url && !parent.Urls.Any(u => u.ID.Equals(item.ID)))
					parent.AddUrl(item.Cast<Url>());
				else
				{
					parent.HasBeenLoaded(true);
					parent.AddSubItem(item);
				}
			}
		}

		private List<Item> itemCollection(DataTable dataTable)
		{
			List<DataColumn> attributeColumns = new List<DataColumn>();

			//For convenience of functionality in GetDataTableBySelection() in classes inheriting DatamodelMenu
			foreach (DataColumn column in dataTable.Columns)
				if (column.Prefix.ToLower() == "attribute")
					attributeColumns.Add(column);

			List<Item> items =
				dataTable.Rows.OfType<DataRow>().Select(
					r =>
					{
						var item = itemFromRow(r);

						if (item != null)
						{
							if (item is Link)
							{
								if (RetrieveAttributes != null)
									RetrieveAttributes(item.Cast<Link>());

								foreach (DataColumn column in attributeColumns)
									item.Cast<Link>().SetAttribute(column.ColumnName, r[column.ColumnName]);
							}
						}

						return item;
					}
				).Where(i => i != null).ToList();

			return items.ToList();
		}

		private Item itemFromRow(DataRow row)
		{
			Item item;

			if (row["PRESENTATION"].ToString().ToUpper() == "P")
			{
				item =
					new Headline(
						row["PRESTEXT"].ToString(),
						row["PRESTEXTS"].ToString(),
						row["SORTCODE"].ToString(),
						row["MENU"].ToString(),
						row["SELECTION"].ToString(),
						row["DESCRIPTION"].ToString()
					);
			}
			else if (row["ITEMTYPE"].ToString() == "table")
			{
				item =
					new TableLink(
						row["PRESTEXT"].ToString(),
						row["PRESTEXTS"].ToString(),
						row["SORTCODE"].ToString(),
						row["MENU"].ToString(),
						row["SELECTION"].ToString(),
						row["DESCRIPTION"].ToString(),
						LinkType.Table,
						row["TABLESTATUS"].ToString() == "" ? TableStatus.NotSet : (TableStatus)row["TABLESTATUS"].ToString().ToUpper()[0],
						row["PUBLISHED"].ToString() == "" ? null : (DateTime?)DateTime.Parse(row["PUBLISHED"].ToString()),
						row["LASTUPDATED"].ToString() == "" ? null : (DateTime?)DateTime.Parse(row["LASTUPDATED"].ToString()),
						row["STARTTIME"].ToString(),
						row["ENDTIME"].ToString(),
						row["TABLEID"].ToString(),
						row["CATEGORY"].ToString() == "" ? PresCategory.NotSet : (PresCategory)row["CATEGORY"].ToString().ToUpper()[0]
					);
			}
			else if (row["ITEMTYPE"].ToString() == "menu")
			{
				item =
					new PxMenuItem(
						this,
						row["PRESTEXT"].ToString(),
						row["PRESTEXTS"].ToString(),
						row["SORTCODE"].ToString(),
						row["MENU"].ToString(),
						row["SELECTION"].ToString(),
						row["DESCRIPTION"].ToString()
					);
			}
			else if (row["ITEMTYPE"].ToString() == "url")
			{
				item =
					new Url(
						row["PRESTEXT"].ToString(),
						row["SORTCODE"].ToString(),
						row["MENU"].ToString(),
						row["LINKID"].ToString(),
						row["DESCRIPTION"].ToString(),
						(PresCategory)row["CATEGORY"].ToString().ToUpper()[0],
						row["LINKURL"].ToString(),
						(LinkPres)row["LINKPRES"].ToString().ToUpper()[0]
					);
			}
			else
				throw new Exceptions.NotValidItemFromDatabaseException("Row from database not valid for item creation.");

			item.ID.Menu = row["MENU"].ToString();

			if (Restriction(item))
			{
				if (AlterItemBeforeStorage != null)
					AlterItemBeforeStorage(item);

				return item;
			}
			else
				return null;
		}

		/// <summary>
		/// Sets the current selection and retrieves the items subitems.
		/// </summary>
		/// <param name="selections">The menu/selections of the item to select in the form menu,selection</param>
		/// <returns></returns>
		public bool SetCurrentItemBySelections(params string[] selections)
		{
			foreach (string s in selections)
				if (!SetCurrentItemBySelection(s.Split(',')[0], s.Split(',')[1]))
					return false;

			return true;
		}

		/// <summary>
		/// Sets the current selection and retrieves the items subitems.
		/// </summary>
		/// <param name="menu">The menu of the item to select</param>
		/// <param name="selection">The selection of the item to select</param>
		/// <returns></returns>
		public override bool SetCurrentItemBySelection(string menu, string selection)
		{
			if (base.SetCurrentItemBySelection(menu, selection))
			{
				Item item = RootItem.FindSelection(menu, selection);

				if (item is PxMenuItem)
				{
					PxMenuItem menuItem = (PxMenuItem)item;
					if (!menuItem.HasBeenLoaded())
						Load(menuItem);
				}
			}
			else
				return false;

			return true;
		}
	}
}
