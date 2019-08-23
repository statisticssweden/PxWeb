using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// DatamodelMenu implementation for usage with Sybase.
	/// </summary>
	public class SybaseDatamodelMenu : DatamodelMenu
	{
		private string connectionString;

		/// <summary>
		/// DatamodelMenu implementation for usage with Sybase.
		/// </summary>
		/// <param name="connectionString">The connection string for the database</param>
		/// <param name="language">Language code for retrived data</param>
		/// <param name="initializationFunction">Lambda for initialization of the menu instance</param>
		public SybaseDatamodelMenu(string connectionString, string language, DatamodelMenuInitialization initializationFunction) 
			: base(
				' ',  //TODO: char for parameter binding
				language, 
				m => 
				{ 
					//TODO: initializatin of DatamodelMenu for Sybase-usage 
					//do we need any changes to the sql for Sybase usage?

					((SybaseDatamodelMenu)m).connectionString = connectionString;

					if (initializationFunction != null)
						initializationFunction(m);
				}
			)
		{}

		public override System.Data.DataTable GetDataTableBySelection(string menu, string selection, int numberOfLevels, string sql, DatabaseParameterCollection parameters)
		{
			//TODO: Connect to Sybase, extract and return data.

			throw new NotImplementedException();
		}
	}
}
