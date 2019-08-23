namespace PCAxis.Sql.DbConfig {
    /// <summary>
    /// Holds the 3 strings needed by the PxSqlCommand constructor.
    /// The purpose of this class is just to keep the 3 strings in one place.
    /// </summary>
    public class InfoForDbConnection {
        public readonly string DataBaseType;
        public readonly string DataProvider;
        public readonly string ConnectionString;

        public InfoForDbConnection(string aDataBaseType, string aDataProvider, string aConnectionString) {
            DataBaseType = aDataBaseType;
            DataProvider = aDataProvider;
            ConnectionString = aConnectionString;
        }

    }
}
