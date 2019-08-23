using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PCAxis.Menu.Implementations;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// DatamodelMenu implementation for usage with MS SQL.
	/// </summary>
	public class MsSqlDatamodelMenu : DatamodelMenu
	{
		private string connectionString;

		/// <summary>
		/// DatamodelMenu implementation for usage with MS SQL.
		/// </summary>
		/// <param name="connectionString">The connection string for the database</param>
		/// <param name="language">Language code for retrived data</param>
		/// <param name="initializationFunction">Lambda for initialization of the menu instance</param>
		public MsSqlDatamodelMenu(string connectionString, string language, DatamodelMenuInitialization initializationFunction)
			: base(
				'@', 
				language, 
				m => 
				{
					((MsSqlDatamodelMenu)m).connectionString = connectionString;

					m.AddSqlHints(SqlHint.UseRecursiveCTE);
					
					if (initializationFunction != null)
						initializationFunction(m);
				}
			)
		{}

		/// <summary>
		/// </summary>
		public override DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters)
		{
            DataTable dataTable = new System.Data.DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString)) 
			{
                connection.Open();

                SqlCommand selectCommand = new SqlCommand(sql, connection);

				selectCommand.Parameters.AddRange(
					new SqlParameter[] 
					{
						new SqlParameter("@menu", SqlDbType.VarChar, parameters["menu"].Size) { Value = menu },
						new SqlParameter("@selection", SqlDbType.VarChar, parameters["selection"].Size) { Value = selection },
						new SqlParameter("@levels", SqlDbType.Int, parameters["levels"].Size) { Value = numberOfLevels }
					}
				);
            
                new SqlDataAdapter(selectCommand).Fill(dataTable);
            }

            return dataTable;
		}
	}
}
