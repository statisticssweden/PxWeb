using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml;
using log4net;


using PCAxis.Sql.Exceptions;

namespace PCAxis.Sql.DbConfig {
    /// <summary>
    /// Configuration of the SQL base read from file.</summary>
    public class SqlDbConfigs {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDbConfigs));

        private XPathNavigator nav;
       
        private SqlDbConfig mDatabase;
        public SqlDbConfig database;
        private string mDefaultDbId;
        public string DefaultDbId
        {
            get { return mDefaultDbId; }
        }

        private Dictionary<string, SqlDbConfig> mDataBases;
        public Dictionary<string, SqlDbConfig> Databases
    {
        get { return mDataBases; }
    }

       
        private Dictionary<string, string> mDatabaselistDescById = new Dictionary<string, string>();
        /// <summary>
        /// Use this to select which database to use
        /// </summary>
        public Dictionary<string, string> DatabaselistDescById {
            get { return mDatabaselistDescById; }

        }

   
        public SqlDbConfigs(string SqlDbConfigPath)
        {

            XPathDocument doc = new XPathDocument(SqlDbConfigPath);
            log.Debug("SqlDbConfigPath = "+SqlDbConfigPath);
            nav = doc.CreateNavigator();
            //makes list of available databases:
            XPathNodeIterator dbs = nav.Select("//SqlDbConfig/Database");
            string dbId = "";
            int counter = 0;
            foreach (XPathNavigator db in dbs)
            {
                dbId = db.SelectSingleNode("@id").Value;
                if (counter == 0)
                {
                    mDefaultDbId = dbId;
                }
                //string desc = db.SelectSingleNode("Description").Value;
                string desc = db.SelectSingleNode("Descriptions/Description[1]").Value;
                mDatabaselistDescById.Add(dbId, desc);
                counter++;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(SqlDbConfigPath);
            PCAxis.Encryption.SqlDbEncrypter.Decrypt(xdoc);

            mDataBases = new Dictionary<string, SqlDbConfig>();
            foreach (string databaseId in mDatabaselistDescById.Keys)
            {                
                string xPathToDb = "//SqlDbConfig/Database[@id='" + databaseId + "']";
                XmlNode node = xdoc.SelectSingleNode(xPathToDb);
                XmlReader xmlReader = new XmlNodeReader(node);
                nav = nav.SelectSingleNode(xPathToDb);

                //mDatabase = new SqlDbConfig(xmlReader,nav);
                mDatabase = SqlDbConfig.GetSqlDbConfig(xmlReader, nav);
                mDataBases.Add(databaseId, mDatabase);              
            }
        }
    }
}
