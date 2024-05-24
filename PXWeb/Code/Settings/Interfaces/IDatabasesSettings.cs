using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Databases settings
    /// </summary>
    public interface IDatabasesSettings
    {
        /// <summary>
        /// Enabled PX-file databases
        /// </summary>
        IEnumerable<string> PxDatabases { get; }

        /// <summary>
        /// Enabled CNMM (Common Nordic Meta Model) databases
        /// </summary>
        IEnumerable<string> CnmmDatabases { get; }

        /// <summary>
        /// All possible PX-file databases
        /// </summary>
        IEnumerable<DatabaseInfo> AllPxDatabases { get; }


        /// <summary>
        /// All possible CNMM databases
        /// </summary>
        IEnumerable<DatabaseInfo> AllCnmmDatabases { get; }

        /// <summary>
        /// Filename for PX-files databases
        /// </summary>
        string PxDatabaseFilename { get; }

        /// <summary>
        /// Get PX-file database by id
        /// </summary>
        /// <param name="id">database id</param>
        /// <returns></returns>
        DatabaseInfo GetPxDatabase(string id);

        /// <summary>
        /// Get CNMM database by id
        /// </summary>
        /// <param name="id">database id</param>
        /// <returns></returns>
        DatabaseInfo GetCnmmDatabase(string id);

        /// <summary>
        /// Get database (PX or CNMM) by id
        /// </summary>
        /// <param name="id">database id</param>
        /// <returns></returns>
        DatabaseInfo GetDatabase(string id);

        /// <summary>
        /// Resets databases.  
        /// </summary>
        void ResetDatabases();
    }
}
