using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PCAxis.Sql.DbConfig;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;
using PCAxis.Menu.Implementations;
using System.Text.RegularExpressions;
using System.Reflection;
using PCAxis.Sql.DbConfig.ConfigDatamodelMenuExtensions;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// Static class for creating instances of eg. OracleDatamodelMenu or MsSqlDatamodelMenu.
	/// </summary>
	public static partial class ConfigDatamodelMenu
	{
		static ConfigDatamodelMenu()
		{
			if (ConfigurationManager.AppSettings["PxMenuConfigDatamodelMenuOverrideType"] != null)
			{
				try
				{
					string[] typeInfo =
						ConfigurationManager.AppSettings["PxMenuConfigDatamodelMenuOverrideType"].Split(';').Select(s => s.Trim()).ToArray();

					OverrideDatamodelMenuType = Assembly.Load(typeInfo[0]).GetType(typeInfo[1], true);
				}
				catch
				{
					throw
						new ConfigDatamodelMenuExceptions.TypeForExtractingDataNotValidException(
							"Type supplied in AppSettings for overriding extraction of data is not valid."
						);
				}
			}
		}

		/// <summary>
		/// Gets or sets type used for connecting to the database instead of the one selected by ConfigDatamodelMenu.
		/// The type must have a constructor accepting string, string, DatamodelMenuInitialization containing connectionString, language and settings as a DatamodelMenuInitialization-instance.
		/// The default value is null.
		/// </summary>
		public static Type OverrideDatamodelMenuType = null;

		/// <summary>
		/// Create instance of appropriate DatamodelMenu
		/// </summary>
		/// <param name="language">Language code</param>
		/// <returns></returns>
		public static DatamodelMenu Create(string language)
		{
			return Create(language, m => { });
		}

		/// <summary>
		/// Create instance of appropriate DatamodelMenu
		/// </summary>
		/// <param name="language">Language code</param>
		/// <param name="menuInitialization">Further menuinitialization as eg. Lambda expression</param>
		/// <returns></returns>
		public static DatamodelMenu Create(string language, PCAxis.Menu.Implementations.DatamodelMenu.DatamodelMenuInitialization menuInitialization)
		{
			return Create(language, null, menuInitialization);
		}

		/// <summary>
		/// Create instance of appropriate DatamodelMenu
		/// </summary>
		/// <param name="language">Language code</param>
		/// <param name="sqlDbConfig">Instance of SqlDbConfig for configuring the menu</param>
		/// <param name="menuInitialization">Further menuinitialization as eg. Lambda expression</param>
		/// <returns></returns>
		public static DatamodelMenu Create(string language, SqlDbConfig sqlDbConfig, PCAxis.Menu.Implementations.DatamodelMenu.DatamodelMenuInitialization menuInitialization)
		{
			//TODO; SqlDbConfig_XX //Norway, JF
			return Create(language, sqlDbConfig, menuInitialization, null);
		}

		/// <summary>
		/// Create instance of appropriate DatamodelMenu
		/// </summary>
		/// <param name="language">Language code</param>
		/// <param name="sqlDbConfig">Instance of SqlDbConfig for configuring the menu</param>
		/// <param name="menuInitialization">Further menuinitialization as eg. Lambda expression</param>
		/// <param name="alterSql">Changes to SQL as eg. Lambda expression</param>
		/// <returns></returns>
		public static DatamodelMenu Create(string language, SqlDbConfig sqlDbConfig, DatamodelMenu.DatamodelMenuInitialization menuInitialization, Func<string, string> alterSql)
		{
			SqlDbConfig dbConf;

			//TODO; SqlDbConfig_XX //Norway, JF
			if (sqlDbConfig != null)
				dbConf = sqlDbConfig;
			else if (ConfigurationManager.AppSettings["dbconfigfile"] != null)
				dbConf = SqlDbConfigsStatic.DefaultDatabase;
			else
				throw new ConfigDatamodelMenuExceptions.NoDbConfigurationException("A dbconfiguration must be supplied to ConfigDatamodelMenu.");

			if (!dbConf.GetAllLanguages().Contains(language))
				throw new ConfigDatamodelMenuExceptions.LanguageNotInConfigXmlException("Cant' find a language with the code \"" + language + "\" in the Language section of the SqlDbConfig-XML.");

			DatamodelMenu.DatamodelMenuInitialization newSettings =
				m =>
				{
					m.OverrideSQL =
						convertSQL(
							dbConf,
							m.FormatSql.Replace(
								"{1}",
								dbConf.isSecondaryLanguage(language) ? "Lang2" : ""
							),
							language
						);

					if (dbConf.MetaModel == "2.2")
					{
						m.Parameters["menu"].Size = 80; //Norway, JF
						m.Parameters["selection"].Size = 80;
					}

					if (alterSql != null)
						m.OverrideSQL = alterSql(m.OverrideSQL);

					if (menuInitialization != null)
						menuInitialization(m);
				};


            var dbInfo = dbConf.GetInfoForDbConnection("", "");
            string connectionString = dbInfo.ConnectionString;

			if (OverrideDatamodelMenuType != null)
				try
				{
					return
						OverrideDatamodelMenuType.GetConstructor(
							new Type[] { typeof(string), typeof(string), typeof(DatamodelMenu.DatamodelMenuInitialization) }
						).Invoke(
							new object[] { connectionString, language, newSettings }
						) as DatamodelMenu;
				}
				catch (Exception e)
				{
					throw
						new ConfigDatamodelMenuExceptions.TypeForExtractingDataNotValidException(
							"Type supplied for overriding extraction of data doesn't have the requested constructor (string, string, DatamodelMenuInitialization)"
						);
				}

			
			string provider = dbInfo.DataProvider;

			switch (provider)
			{
				case "Oracle":
					return new OracleDatamodelMenu(connectionString, language, newSettings);
				case "Sql":
					return new MsSqlDatamodelMenu(connectionString, language, newSettings);
				//case "Sybase":
				//	return new SybaseDatamodelMenu(connectionString, language, newSettings);
				case "OleDb":
					return
						new OleDbDatamodelMenu(
							dbInfo.DataBaseType == "ORACLE" ? ':' : '@',
							dbInfo.DataBaseType != "ORACLE",
							connectionString,
							language,
							newSettings
						);
				default:
					var menuPlugins = ((Dictionary<string, string>)ConfigurationManager.GetSection("PCAxis.Menu"));

					if (menuPlugins == null || !menuPlugins.ContainsKey(provider))
						throw new NotImplementedException("Code for retrieving data from a " + provider + " database is not (yet) implemented in ConfigDatamodelMenu. No plugin for " + provider + " found.");

					try
					{
						return (DatamodelMenu)Activator.CreateInstance(Type.GetType(menuPlugins[provider]), new object[] { connectionString, language, newSettings });
					}
					catch
					{
						throw new NotImplementedException("Failed using plugin for retrieving data from a " + provider + " database.");
					}
			}
		}

		/// <summary>
		/// Replace in SQL from SqlDbConfig instance according to markings in SQL
		/// </summary>
		/// <param name="c"></param>
		/// <param name="sql"></param>
		/// <param name="languageCode"></param>
		/// <returns></returns>
		private static string convertSQL(SqlDbConfig c, string sql, string languageCode)
		{
			//TODO; SqlDbConfig_XX //Norway, JF


			//string secondLang =
			//    languageCode == c.getMainLanguageCode()
			//        ?
			//            c.GetAllLanguages().OfType<string>().First(s => !s.Equals(languageCode))
			//        :
			//            languageCode;
			//        ;

			string a =
				Regex.Replace(
					sql,
					@"(\.*)/\*\[([\w\d_]*)([\. ]{0,1})([\w\d_]*)\*/([\d\w_\. =']*)/\*]\*/",
					m =>
					{
						try
						{
							string replacement;

							bool keyword = m.Groups[5].Value.StartsWith("=");

							if (keyword)
							{
								replacement = "= lower('" + c.GetKeyword(m.Groups[2].Value) + "')";
							}
							else
							{
								bool refToColumn = m.Groups[3].Value == ".";
								string tabName = m.Groups[2].Value;

								//TODO: Fjern linie og ret til
								object tab = c.GetType().GetField(tabName).GetValue(c);

								replacement = m.Groups[1].Value;

								if (refToColumn)
								{
									bool onlyColumnName = m.Groups[1].Value == ".";
									string colName = m.Groups[4].Value;

									if (onlyColumnName)
										replacement += c.GetColumnName(tabName, colName);
									else
										replacement += c.GetColumnId(tabName, colName, languageCode);
								}
								else
								{
									replacement += c.GetTableNameAndAlias(tabName, languageCode); //.Replace("_ ", " ");

									string alias = m.Groups[4].Value;
									if (alias != "")
									{
										replacement = replacement.Substring(0, replacement.IndexOf(" ") + 1) + alias;
									}
								}
							}

							return replacement;
						}
						catch (Exception e)
						{
							throw
								new ConfigDatamodelMenuExceptions.UnrecognizedPatternInSpecifiedSQLException(
									"Pattern, " + m.Groups[0] + ", in SQL not recognized for replacement from SqlDbConfig instance.",
									e
								);
						}
					}
				);

			return a;
		}
	}
}
