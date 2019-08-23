using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// DatamodelMenu implementation for usage with Sybase.
	/// </summary>
	public class OleDbDatamodelMenu : DatamodelMenu
	{
		private string connectionString;

		/// <summary>
		/// DatamodelMenu implementation for usage with Sybase.
		/// </summary>
		/// <param name="parameterChar">Char used to indicate binding variables in database. This value should in NO WAY originate from the GUI!</param>
		/// <param name="useRecursiveCTE">Gets or sets whether to use recursive CTE for extracting hierarchical data.</param>
		/// <param name="connectionString">The connection string for the database</param>
		/// <param name="language">Language code for retrived data</param>
		/// <param name="initializationFunction">Lambda for initialization of the menu instance</param>
		public OleDbDatamodelMenu(char parameterChar, bool useRecursiveCTE, string connectionString, string language, DatamodelMenuInitialization initializationFunction)
			: base(
				parameterChar,
				language,
				m =>
				{
					((OleDbDatamodelMenu)m).connectionString = connectionString;

					if (useRecursiveCTE)
					{
						m.AddSqlHints(SqlHint.UseRecursiveCTE);
					}
					else
					{
						m.AddSqlHints(SqlHint.UseConnectByPrior);
						m.AddSqlHints(SqlHint.UseNedstedSelect);
					}

					if (initializationFunction != null)
						initializationFunction(m);
				}
			)
		{ }

		public override System.Data.DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters)
		{
			DataTable dataTable = new DataTable();

			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();

				string pattern = ParameterChar + "[a-z]*";
				int c = 0;

				List<OleDbParameter> values = new List<OleDbParameter>();

				foreach (Match m in Regex.Matches(sql, pattern))
				{
					object value;

                    if (m.Value == (ParameterChar + "menu"))
                    {
                        value = menu;
                    }
                    else if (m.Value == (ParameterChar + "selection"))
                    {
                        value = selection;
                    }
                    else if (m.Value == (ParameterChar + "levels"))
                    {
                        value = numberOfLevels;
                    }
                    else
                    {
                        value = null;
                    }

					values.Add(new OleDbParameter("paramOleDb" + (c++), value));
				}

				sql = Regex.Replace(sql, pattern, "?");

				OleDbCommand command = new OleDbCommand(sql, connection);
				command.Parameters.AddRange(values.ToArray());
				
				new OleDbDataAdapter(command).Fill(dataTable);
			}


			return dataTable;
		}
	}
}
