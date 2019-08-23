using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using PCAxis.Sql.Exceptions;
using System.Data.Common;

using log4net;
using log4net.Config;


namespace PCAxis.Sql.DbConfig
{

    /// <summary>
    /// Abstract parent class for SqlDbConfig_21, SqlDbConfig_22 ....
    /// Holds the information that is not CNMM-version depentent and the Factory-method that returns the SqlDbConfig_nn
    /// which holds the information that is.
    /// </summary>
    public abstract class SqlDbConfig
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDbConfig));


        //TODO; refactoring:  move these to PCAxis.Sql.Exceptions

        private const string errFindTable = "Error when trying to locate table with table modelname ";
        private const string errFindAlias = "Error when trying to locate alias with table modelname ";
        private const string errFindColumn1 = "Error when trying to locate column with table modelName ";
        private const string errFindColumn2 = " and column modelName ";
        private const string errMetatables = "Can't locate Metatables element";


        private Database mDatabase;

        protected bool allowConfigDefaults = true;

        //TODO; refactoring: is this used? Yes, but the uses could/should be removed.
        public Database Database
        {

            get
            {
                return this.mDatabase;
            }
        }
        private XPathNavigator nav;
        private XmlReader xmlReader;

        private LanguageType mMainLanguage;

        /// <summary>
        /// The main language of the database according to the sqldbconfig file. Is public to allow a hack for norwegian testing of swedish database.
        /// </summary>
        public LanguageType MainLanguage
        {
            get
            {
                return this.mMainLanguage;
            }
        }

    
        /// <summary>
        /// the @id of this part of the config-file, get value in construktor, has no value in the beginning of the Factory-method, usefull in errormessages.
        /// </summary>
        private string myID;


        /// <summary>
        /// The ConnectionString with default user credentials. Default as opposed to having user/password replaced with login (setUserCredentials on builder).
        /// If the connectionstring in the configfile contains "=USER;" and "=PASSWORD;" these are replaced before storing. 
        /// </summary>
        private string defaultConnectionString;

       
        /// <summary>
        /// keyForUserName in the connectionstring (depends on vendor)
        /// </summary>
        private string keyForUserName;

        /// <summary>
        /// keyForPassword in the connectionstring (depends on vendor)
        /// </summary>
        private string keyForPassword;

        private string mMetaModel;
        /// <summary>
        /// The MetaModell aka CNMM version
        /// </summary>
        public string MetaModel
        {
            get { return mMetaModel; }
        }

        private bool mUseTemporaryTables = true;
        /// <summary>
        /// Should the use any database-vendor special table type be used in dataextraction.
        /// </summary>
        public bool UseTemporaryTables
        {
            get { return mUseTemporaryTables; }
        }
        private string mMetatablesSchema = "";
        /// <summary>
        /// Has value for Oracle otherwise ""
        /// </summary>
        public string MetatablesSchema
        {
            get { return mMetatablesSchema; }
        }

        /// <summary>
        /// The tablesuffixes used to add _AAA to language dependent tables
        /// </summary>
        internal Dictionary<string, string> MetaSuffixByLanguage = new Dictionary<string, string>();

        /// <summary>
        /// Same as  MetatablesSchema
        /// </summary>
        public string MetaOwner
        {
            get { return mMetatablesSchema; }
        }
        

        

        /// <summary>Reads the part of the config file that corresponds to SqlDbConfig/Database[@id='_this_database_']"
        /// and returns a SqlDbConfig for the CNMM-version (@metaModel).
        /// </summary>
        /// <param name="xmlReader">a reader for the node</param>
        /// <param name="nav">a navigator for the node</param>
        /// <returns>a SqlDbConfig_XX for the CNMM-version XX </returns>
        public static SqlDbConfig GetSqlDbConfig(XmlReader xmlReader, XPathNavigator nav)
        {
            log.Debug("SqlDbConfig called");
            try
            {
                string docPath = ".";
                XPathNavigator node = nav.SelectSingleNode(docPath);

                string cnmmVersion = node.SelectSingleNode("@metaModel").Value;
                if (String.IsNullOrEmpty(cnmmVersion))
                {
                    throw new NotImplementedException();
                }
                else if (cnmmVersion.Equals("2.4"))
                {
                    //TODO;Lag sjekker på at config inneholder det den skal
                    //TODO; Forklar godt at ValuePoolAlias egentlig ikke hører til hovedspråket, men ...
                    return new SqlDbConfig_24(xmlReader, nav);
                }
                else if (cnmmVersion.Equals("2.3"))
                {
                    //TODO;Lag sjekker på at config inneholder det den skal
                    //TODO; Forklar godt at ValuePoolAlias egentlig ikke hører til hovedspråket, men ...
                    return new SqlDbConfig_23(xmlReader, nav);
                }
                else if (cnmmVersion.Equals("2.2"))
                {
                    //TODO;Lag sjekker på at config inneholder det den skal
                    //TODO; Forklar godt at ValuePoolAlias egentlig ikke hører til hovedspråket, men ...
                    return new SqlDbConfig_22(xmlReader, nav);
                }
                else if (cnmmVersion.Equals("2.1") || cnmmVersion.Equals("2.0"))
                {
                    return new SqlDbConfig_21(xmlReader, nav);
                }
                else
                {
                    //TODO; 2.2 send warning eller noe
                    return new SqlDbConfig_21(xmlReader, nav);
                }
            }
            catch (Exception e)
            {
                log.Error("An error occured.", e);
                throw e;
            }
        }

      
        protected SqlDbConfig(XmlReader xmlReader, XPathNavigator nav)
        {
            
            string docPath = ".";
            XPathNavigator node = nav.SelectSingleNode(docPath);
            myID = node.SelectSingleNode("@id").Value;

            this.nav = nav;
            this.xmlReader = xmlReader;
            XmlSerializer serializer = new XmlSerializer(typeof(Database));

            mDatabase = (Database)serializer.Deserialize(xmlReader);
            mDatabase.postSerialize();

            

            foreach (LanguageType lang in mDatabase.Languages)
            {
                this.MetaSuffixByLanguage.Add(lang.code, lang.metaSuffix);
            }
            //had to use v2_1 not 2.1 in xsd, the xsd.exe dont like "."
            this.mMetaModel = mDatabase.metaModel.ToString();
            this.mMainLanguage = mDatabase.MainLanguage; 
            
            
            this.mUseTemporaryTables = mDatabase.Connection.useTemporaryTables;
            this.setMetatablesSchema();
            
            //

            this.defaultConnectionString = mDatabase.Connection.ConnectionString;

            if (mDatabase.Connection.ConnectionString.IndexOf("=USER;") == -1)
            {
                this.keyForPassword = mDatabase.Connection.KeyForPassword;
                this.keyForUserName = mDatabase.Connection.KeyForUser;
            }
            else
            {
                bool passwFound = false;
                string[] keyValues = mDatabase.Connection.ConnectionString.Split(';');

                foreach (string keyValue in keyValues)
                {
                    if (keyValue.IndexOf("=USER") > 0)
                    {
                        this.keyForUserName = keyValue.Replace("=USER", "").Trim();
                    }
                    else if (keyValue.IndexOf("=PASSWORD") > 0)
                    {
                        this.keyForPassword = keyValue.Replace("=PASSWORD", "").Trim();
                        passwFound = true;
                    }
                }
                if (!passwFound)
                {
                    throw new ConfigException(40, mDatabase.Connection.ConnectionString);
                }
                if (String.IsNullOrEmpty(mDatabase.Connection.DefaultUser))
                {
                    throw new ConfigException("When user/password is not given in ConnectionString it must be given in DefaultUser and DefaultPassword");
                }
                this.defaultConnectionString = this.GetConnectionString(mDatabase.Connection.DefaultUser, mDatabase.Connection.DefaultPassword);
            }
            
            

            if ( MetaModel.Equals("2.1") || MetaModel.Equals("2.0"))
            {
                allowConfigDefaults = false;
            }
            else
            {
                allowConfigDefaults = mDatabase.allowConfigDefaults;
            }
        }
       


        #region Methods


        internal void ResetConnectionString(string newConnectionstring)
        {
            if (newConnectionstring.IndexOf("=USER;") > -1) {
                throw new ConfigException("The text =USER is present in the newConnectionstring or it is empty");
            }
            this.defaultConnectionString = newConnectionstring;
        }


        private void setMetatablesSchema()
        {
            if (mDatabase.Connection.metatablesSchema == null)
            {
                mMetatablesSchema = "";
            } else
            {
                mMetatablesSchema = mDatabase.Connection.metatablesSchema.Trim() + ".";
            }
        }



       

        /// <summary>
        /// Gets the description for the database so the user may choose a nice one
        /// </summary>
        /// <param name="langCode">the language</param>
        /// <returns></returns>
        public string GetDescription(string langCode)
        {
            foreach (DatabaseDescription desc in mDatabase.Descriptions)
            {
                if (desc.lang == langCode)
                {
                    return desc.Value;
                }
            }
            // if description for lang not found, then throw exception
            throw new ConfigException(41, "Description for lanuage " + langCode + " does not exist." );
        }

        private string GetDatabaseType()
        {
            return mDatabase.Connection.databaseType.ToString();
        }


        //TODO; refactoring: clients should use GetInfoForDbConnection
        public string GetDataProvider()
        {
            return mDatabase.Connection.dataProvider.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">empty string means default</param>
        /// <param name="password">empty string means default</param>
        /// <returns></returns>
        public InfoForDbConnection GetInfoForDbConnection(String user, String password)
        {
	        return GetInfoForDbConnection(this.GetConnectionString(user, password));
        }

		public InfoForDbConnection GetInfoForDbConnection(string connectionString)
		{

			return new InfoForDbConnection(this.GetDatabaseType(), this.GetDataProvider(), connectionString);
        }

        /// <summary>
        /// Gets the connectionstring. 
        /// </summary>
        /// <param name="user">empty string means default</param>
        /// <param name="password">empty string means default</param>
        /// <returns>The ready to use connectionstring</returns>
        private string GetConnectionString(String user, String password)
        {
            return  GetDbStringProvider().GetConnectionString(this, user, password);
        }

        private IDbStringProvider GetDbStringProvider()
        {
            
            if (string.IsNullOrEmpty(this.Database.Connection.ConnectionStringProvider))
            {
                return new DefaultDbStringProvider();
            }

            return Activator.CreateInstance(Type.GetType(this.Database.Connection.ConnectionStringProvider)) as IDbStringProvider;
        }

        
        
        /// <summary>
        /// replaces the user and password in the connection string  with default user and password
        /// </summary>
        /// <returns>The default connection string after replacing the user and password </returns>
        public String GetDefaultConnString()
        {
            return this.defaultConnectionString;
        }


        /// <summary>
        /// Gets the "language" suffix for the metatables. It may be "" (normal for the main language).  
        /// </summary>
        /// <param name="languageCode">The language to be used</param>
        /// <returns>the MetaSuffix</returns>
        public string GetMetaSuffix(string languageCode)
        {
            String returnMe = "";
            Boolean hasLanguage = false;

            foreach (LanguageType lang in mDatabase.Languages)
            {
                if (lang.code.Equals(languageCode))
                {
                    returnMe = lang.metaSuffix;
                    hasLanguage = true;
                    break;
                }
            }
            if (!hasLanguage)
            {
                throw new ConfigException(41, languageCode);
            }

            return returnMe;
        }

        /// <summary>
        /// Checks if the given language code equals the code of the mainLanguage read from the configfile.
        /// </summary>
        /// <param name="langCode">The language code to check</param>
        /// <returns>true if they differ</returns>
        public Boolean isSecondaryLanguage(String langCode)
        {
            return !mDatabase.MainLanguage.code.Equals(langCode);
        }

        

        /// <summary>
        /// return all the language codes spesifyed in this config( intended to be used when
        /// the PxsQuery spesifies "all"
        /// </summary>
        public StringCollection GetAllLanguages()
        {
            StringCollection myOut = new StringCollection();
            foreach (LanguageType langType in mDatabase.Languages)
            {
                myOut.Add(langType.code);
            }
            return myOut;
        }



        /// <summary>
        /// Gets the local table name for the given model name
        /// </summary>
        /// <param name="ModelName">The modelname of the table</param>
        /// <returns>the local table name</returns>
        protected string ExtractTableName(string ModelName)
        {
            this.allowConfigDefaults = false;
            return this.ExtractTableName(ModelName, "BUGBUGBUG");
        }

        /// <summary>
        /// Gets the local table name for the given model name
        /// </summary>
        /// <param name="ModelName">The modelname of the table</param>
        /// <param name="defaultLocalTableName">If the table modelName is not found and allowConfigDefaults, this value is returned.</param>
        /// <returns>the local table name</returns>
        protected string ExtractTableName(string ModelName, string defaultLocalTableName)
        {

            string docPath;
            try
            {
                //docPath = "//Tables/Table[@modelName='" + ModelName + "']";
                docPath = "Tables/Table[@modelName='" + ModelName + "']";
                XPathNavigator node = nav.SelectSingleNode(docPath);
                if (node == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultLocalTableName;
                    }
                }

                XPathNavigator tableNode = node.SelectSingleNode("@tableName");

                 if (tableNode == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultLocalTableName;
                    }
                }


                return tableNode.Value;
            } catch (System.Xml.XPath.XPathException XPathExp)
            {
                throw new ConfigException(XPathExp.Message);
            } catch (System.Exception otherExp)
            {
                throw new ConfigException(errFindTable + ModelName, otherExp);
            }


        }


        /// <summary>
        /// Gets the table alias from the table-element with the given ModelName
        /// </summary>
        /// <param name="ModelName">The table modelName</param>
        /// <returns>the table alias</returns>
        protected string ExtractAliasName(string ModelName)
        {
            this.allowConfigDefaults = false;
            return this.ExtractAliasName(ModelName, "BUGBUGBUG");
        }

        /// <summary>
        /// Gets the table alias from the table-element with the given ModelName
        /// </summary>
        /// <param name="ModelName">The table modelName</param>
        /// <param name="defaultTableAlias">If the table modelName is not found and allowConfigDefaults, this value is returned.</param>
        /// <returns>the table alias</returns>
        protected string ExtractAliasName(string ModelName, string defaultTableAlias)
        {
            string docPath;
            try
            {
                //docPath = "//Tables/Table[@modelName='" + ModelName + "']";
                docPath = "Tables/Table[@modelName='" + ModelName + "']";
                XPathNavigator node = nav.SelectSingleNode(docPath);
                if (node == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultTableAlias;
                    }
                }
                XPathNavigator aliasNode =node.SelectSingleNode("@alias");
                if (aliasNode == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultTableAlias;
                    }
                }

                return aliasNode.Value;
            } catch (System.Xml.XPath.XPathException XPathExp)
            {
                throw new ConfigException(XPathExp.Message);
            } catch (System.Exception otherExp)
            {
                throw new ConfigException(errFindAlias + ModelName, otherExp);
            }


        }

        /// <summary>
        /// Get the local column name for the given model names
        /// </summary>
        /// <param name="tableModelName">The modelname of the table</param>
        /// <param name="columnModelName">The modelname of the column in that table</param>
        /// <returns>the local column name</returns>
        protected string ExtractColumnName(string tableModelName, string columnModelName)
        {
            this.allowConfigDefaults = false;
            return this.ExtractColumnName(tableModelName, columnModelName, "BUGBUGBUG");
        }

        /// <summary>
        /// Get the local column name for the given model names
        /// </summary>
        /// <param name="tableModelName">The modelname of the table</param>
        /// <param name="columnModelName">The modelname of the column in that table</param>
        /// <param name="defaultLocalColumnName">If the column is not spesified in the config-xml and allowConfigDefaults, this value is returned.</param>
        /// <returns>the local column name</returns>
        protected string ExtractColumnName(string tableModelName, string columnModelName, string defaultLocalColumnName)
        {            
            
            string docPath;
            try
            {
                

                docPath = "Tables/Table[@modelName='" + tableModelName + "']";
                XPathNavigator tableNode = nav.SelectSingleNode(docPath);

                if (tableNode == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultLocalColumnName;
                    }
                    else
                    {
                        throw new ConfigException(510, myID, tableModelName);
                    }
                }

                string colPath = "Columns/Column[@modelName='" + columnModelName + "']";
                XPathNavigator colNode = tableNode.SelectSingleNode( colPath );

                if (colNode == null)
                {
                    if (allowConfigDefaults)
                    {
                        return defaultLocalColumnName;
                    }
                    else
                    {
                        throw new ConfigException(511, myID, tableModelName, columnModelName);
                    }
                }

                string theAttributeName = "@columnName";
                XPathNavigator theNode = colNode.SelectSingleNode(theAttributeName);

                if (theNode == null)
                {

                    throw new ConfigException(512, myID, tableModelName, columnModelName, theAttributeName);
                }
                return theNode.Value;

            } catch (ConfigException e)
            {
                    throw e;

            } catch (System.Xml.XPath.XPathException XPathExp)
            {
                throw new ConfigException(XPathExp.Message);
            } catch (System.Exception otherExp)
            {
                throw new ConfigException(errFindColumn1 + tableModelName + errFindColumn2 + columnModelName, otherExp);
            }
        }

        /// <summary>
        /// Gets the local Code string for the given model Code
        /// </summary>
        /// <param name="CodeName">Name of Code in the model</param>
        /// <returns>the local Code string</returns>
        protected string ExtractCode(string CodeName)
        {
            this.allowConfigDefaults = false;
            return this.ExtractCode(CodeName, "BUGBUGBUG");
        }


        /// <summary>
        /// Gets the local Code string for the given model Code.
        /// </summary>
        /// <param name="CodeName">Name of Code in the model</param>
        /// <param name="defaultCodevalue">If no code with CodeName exists in the xml and allowConfigDefaults, this value is returned.</param>
        /// <returns>the local Code string</returns>
        protected string ExtractCode(string CodeName, string defaultCodevalue)
        {
            string docPath;
            //docPath = "//Codes/Code[@codeName='" + CodeName + "']";
            docPath = "Codes/Code[@codeName='" + CodeName + "']";
            //nav = doc.CreateNavigator();
            XPathNavigator node = nav.SelectSingleNode(docPath);
            if (node == null)
            {
                if (allowConfigDefaults)
                {
                    return defaultCodevalue;
                }
                else
                {
                    throw new ConfigException(500, CodeName, myID);
                }
            }
            string theAttributeName = "@codeValue";
            XPathNavigator theNode = node.SelectSingleNode(theAttributeName);
            if (theNode == null)
            {
                throw new ConfigException(501, CodeName, myID, theAttributeName);
            }
            return theNode.Value;
        }



        /// <summary>
        /// Checks if the configfile has keyword-element for the given Keyword 
        /// </summary>
        /// <param name="modelName">Name of Keyword in the model</param>
        /// <returns>True if the modelName is found</returns>
        protected bool FileHasKeyword(string modelName)
        {
            //string docPath = "//Keywords/Keyword[@modelName='" + modelName + "']";
            string docPath = "Keywords/Keyword[@modelName='" + modelName + "']";

            XPathNavigator node = nav.SelectSingleNode(docPath);

            return node != null;
        }


        /// <summary>
        /// Gets the local Keyword string for the given model Keyword
        /// </summary>
        /// <param name="modelName">Name of Keyword in the model</param>
        /// <returns>the local Keyword string</returns>
        protected string ExtractKeyword(string modelName)
        {
            this.allowConfigDefaults = false;
            
            return this.ExtractKeyword(modelName,"BUGBUGBUG");
        }

        /// <summary>
        /// Gets the local Keyword string for the given model Keyword.
        /// </summary>
        /// <param name="modelName">Name of Keyword in the model</param>
        /// <param name="defaultKeywordName">return this string if the keyword is not in the xml and allowConfigDefaults.</param>
        /// <returns>the local Keyword string</returns>
        protected string ExtractKeyword(string modelName, string defaultKeywordName)
        {

            //string docPath = "//Keywords/Keyword[@modelName='" + modelName + "']";
            string docPath = "Keywords/Keyword[@modelName='" + modelName + "']";
            //nav = doc.CreateNavigator();
            XPathNavigator node = nav.SelectSingleNode(docPath);
            if (node == null)
            {
                
                if (allowConfigDefaults)
                {
                    return defaultKeywordName;
                }
                else
                {
                    throw new ConfigException(503, modelName, myID);
                }
            
            }




            string attributeName = "@keywordName";
            XPathNavigator theNode = node.SelectSingleNode(attributeName);
            if (theNode == null)
            {
                throw new ConfigException(504, modelName, myID, attributeName);
            }

            return theNode.Value;
        }

  

   

    }
        #endregion
}

