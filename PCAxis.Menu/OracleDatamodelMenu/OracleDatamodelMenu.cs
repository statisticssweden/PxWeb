using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using PCAxis.Menu.Implementations;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// DatamodelMenu implementation for usage with Oracle.
	/// </summary>
	public class OracleDatamodelMenu : DatamodelMenu
	{
		private log4net.ILog log = log4net.LogManager.GetLogger("PCAxis.Menu.Implementations.OracleDatamodelMenu");

		private string connectionString;

		/// <summary>
		/// DatamodelMenu implementation for usage with Oracle.
		/// </summary>
		/// <param name="connectionString">The connection string for the database</param>
		/// <param name="language">Language code for retrived data</param>
		/// <param name="initializationFunction">Lambda for initialization of the menu instance</param>
		public OracleDatamodelMenu(string connectionString, string language, DatamodelMenuInitialization initializationFunction)
			: base(
				':', 
				language, 
				m => 
				{ 
					((OracleDatamodelMenu)m).connectionString = connectionString;

					m.AddSqlHints(SqlHint.UseConnectByPrior);

					if (initializationFunction != null)
						initializationFunction(m);
				}
			)
		{}

		/// <summary>
		/// </summary>
		public override DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters)
		{
			log.DebugFormat("Getting menu for menu, selection, numberOfLevels: {0}, {1}, {2}", menu, selection, numberOfLevels);
			log.DebugFormat("SQL: {0}", sql);

			DataTable dataTable = new DataTable();

			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				connection.Open();

				OracleCommand command = new OracleCommand(sql, connection) { BindByName = true };

				command.Parameters.Add("levels", OracleDbType.Int32, parameters["levels"].Size, numberOfLevels, ParameterDirection.Input);
				command.Parameters.Add("menu", OracleDbType.Varchar2, parameters["menu"].Size, menu, ParameterDirection.Input);
				command.Parameters.Add("selection", OracleDbType.Varchar2, parameters["selection"].Size, selection, ParameterDirection.Input);

				new OracleDataAdapter(command).Fill(dataTable);
			}

			return dataTable;
		}
	}
}
