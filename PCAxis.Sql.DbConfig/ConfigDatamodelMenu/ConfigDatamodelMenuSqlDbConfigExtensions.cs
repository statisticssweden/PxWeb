using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace PCAxis.Sql.DbConfig.ConfigDatamodelMenuExtensions
{
	/// <summary>
	/// Extensions for hiding the fact from PCAxis.Menu, that there are different classes of SqlDbConfig for the different metamodels.
	/// Agreed on with Norway, June 2013.
	/// </summary>
	public static class ConfigDatamodelMenuSqlDbConfigExtensions
	{
		public static string GetKeyword(this SqlDbConfig sqlDbConfig, string name)
		{
			return
				sqlDbConfig.getFieldValue("Keywords").getFieldValue(name).ToString();
		}

		public static string GetTableNameAndAlias(this SqlDbConfig sqlDbConfig, string table, string languageCode)
		{
			var tab = sqlDbConfig.getFieldValue(table);
			
			return
				(
					tab is Lang2Tab
					?
						tab.getMethodValue("GetNameAndAlias", languageCode)
						:
						tab.getMethodValue("GetNameAndAlias")
				).ToString();
		}

		public static string GetColumnName(this SqlDbConfig sqlDbConfig, string table, string column)
		{
            var col = sqlDbConfig.getFieldValue(table).getFieldValue(column + "Col");
            string myOut = (col.getMethodValue("PureColumnName")).ToString();
            return myOut;
		}

		public static string GetColumnId(this SqlDbConfig sqlDbConfig, string table, string column, string languageCode)
		{
			var col = sqlDbConfig.getFieldValue(table).getFieldValue(column + "Col");

			return
				(
					col is Lang2Col
					?
						col.getMethodValue("Id", languageCode)
						:
						col.getMethodValue("Id")
				).ToString();
		}

		private static object getFieldValue(this object o, string name)
		{
			return o.GetType().GetField(name).GetValue(o);
		}

		private static object getMethodValue(this object o, string name, params object[] parameters)
		{
			return
				o.GetType().GetMethod(name).Invoke(o, parameters.Length == 0 ? null : parameters);
		}
	}
}
