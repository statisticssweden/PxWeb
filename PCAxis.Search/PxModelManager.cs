using PCAxis.Menu;
using PCAxis.Paxiom;
using PCAxis.Web.Core.Enums;
using System;
using System.Web;

namespace PCAxis.Search
{
    /// <summary>
    /// Class for getting PxModel object
    /// </summary>
    public class PxModelManager
    {
        #region "Private fields"

        private string _pxDatabaseBaseDirectory;
        private static PxModelManager _current = new PxModelManager();
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PxModelManager));

        #endregion

        #region "Public properties"

        /// <summary>
        /// Get the (Singleton) PxModelManager object
        /// </summary>
        public static PxModelManager Current
        {
            get
            {
                return _current;
            }
        }

        #endregion

        /// <summary>
        /// Private constructor
        /// </summary>
        private PxModelManager()
        {
        }

        #region "Public methods"


        /// <summary>
        /// Initialize the (Singleton) PxModelManager
        /// </summary>
        /// <param name="pxDatabaseBaseDirectory"></param>
        public void Initialize(string pxDatabaseBaseDirectory)
        {
            SetPxBaseDirectory(pxDatabaseBaseDirectory);
        }

        /// <summary>
        /// Get PxModel object for the specified table
        /// </summary>
        /// <param name="dbType">Type of database</param>
        /// <param name="dbId">Database id</param>
        /// <param name="language">Language</param>
        /// <param name="tableId">ItemSelection object specifiying the table</param>
        /// <returns>PxModel object for the specified table</returns>
        public PXModel GetModel(DatabaseType dbType, string dbId, string language, ItemSelection tableId)
        {
            return GetModel(dbType, dbId, language, tableId.Selection);
        }

        /// <summary>
        /// Get PxModel object for the specified table
        /// </summary>
        /// <param name="dbType">Type of database</param>
        /// <param name="dbId">Database id</param>
        /// <param name="language">Language</param>
        /// <param name="tableId">string specifiying the table</param>
        /// <returns>PxModel object for the specified table</returns>
        public PXModel GetModel(DatabaseType dbType, string dbId, string language, string tableId)
        {
            if (dbType == DatabaseType.PX)
            {
                return GetPxModel(dbId, language, tableId);
            }
            else
            {
                return GetCnmmModel(dbId, language, tableId);
            }
        }


        #endregion


        #region "Private methods"

        /// <summary>
        /// Get PxModel object from CNMM database
        /// </summary>
        /// <param name="dbId">Database id</param>
        /// <param name="language">Language</param>
        /// <param name="tableId">String specifiying the table</param>
        /// <returns>PxModel object for the specified table</returns>
        private PXModel GetCnmmModel(string dbId, string language, string tableId)
        {
            try
            {
                PCAxis.PlugIn.Sql.PXSQLBuilder builder = new PCAxis.PlugIn.Sql.PXSQLBuilder();
                builder.SetPreferredLanguage(language);
                builder.SetPath(string.Format("{0}:{1}", dbId, tableId));
                builder.BuildForSelection();

                return builder.Model;
            }
            catch (Exception ex)
            {
                _logger.Error("PCAxis.Search PxModelManager::GetCnmmModel : " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get PxModel object from PX database
        /// </summary>
        /// <param name="dbId">Database id</param>
        /// <param name="language">Language</param>
        /// <param name="tableId">String specifiying the table</param>
        /// <returns>PxModel object for the specified table</returns>
        private PXModel GetPxModel(string dbId, string language, string tableId)
        {
            try
            {
                if (string.IsNullOrEmpty(_pxDatabaseBaseDirectory))
                {
                    throw new System.Exception("PxModelManager has not been INitialized with the PX database base directory");
                }

                string fullPath = System.IO.Path.Combine(_pxDatabaseBaseDirectory, tableId);
                PCAxis.Paxiom.PXFileBuilder builder = new PCAxis.Paxiom.PXFileBuilder();
                builder.SetPath(fullPath);

                builder.SetPreferredLanguage(language);
                builder.BuildForSelection();

                return builder.Model;
            }
            catch (Exception ex)
            {
                _logger.Error("PCAxis.Search PxModelManager::GetPxModel : " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Set the PX database base directory
        /// </summary>
        /// <param name="pxBaseDirectory">Base directory for PX Databases</param>
        private void SetPxBaseDirectory(string pxBaseDirectory)
        {
            if (!System.IO.Path.IsPathRooted(pxBaseDirectory))
            {
                pxBaseDirectory = HttpContext.Current.Server.MapPath(pxBaseDirectory);
            }

            if (System.IO.Directory.Exists(pxBaseDirectory))
            {
                _pxDatabaseBaseDirectory = pxBaseDirectory;
            }
            else
            {
                _logger.Error("Failed to set search index base directory. Directory '" + pxBaseDirectory + "' does not exist");
                _pxDatabaseBaseDirectory = "";
            }
        }

        #endregion
    }
}
