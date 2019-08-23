using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Configuration;

using GrandMaster;

//TODO -versjonsnummer inn i \QueryLib_21\
//     -de colonner som bare finnes på engelsk.

namespace generateClassesForQueryLib
{
    class Generator
    {
        private GrandMaster.GrandMaster myMaster;

      
        public Generator(string root, string versjon)
        {
            string inFile = root + @"\PCAxis.Sql\codegenerator2\MakeCodeForCNMMnn\master_" + versjon + ".xml";
            string configStubOutFile = root + @"PCAxis.Sql.DbConfig\configstub_" + versjon + ".xml";
            string outDirRows = root + @"PCAxis.Sql\QueryLib_" + versjon + @"\GeneratedRows\";
            string outDirMetaQ = root + @"PCAxis.Sql\QueryLib_" + versjon + @"\GeneratedMetaQueryParts\";
            string outFileSqlDbConfigNN = root + @"PCAxis.Sql.DbConfig\SqlDbConfig_" + versjon +".cs";
            string sqlOutFile = root + @"\PCAxis.Sql\codegenerator2\MakeCodeForCNMMnn\create_cnmm_" + versjon + "_oracle.sql";

            XmlTextReader xmlReader = new XmlTextReader(inFile);
            XmlSerializer pxsSerial = new XmlSerializer(typeof(GrandMaster.GrandMaster));
            myMaster = (GrandMaster.GrandMaster)pxsSerial.Deserialize(xmlReader);

            MetaQueryMaker mqm = new MetaQueryMaker(myMaster, outDirMetaQ);
            mqm.genMetaQuery();

            RowMaker rm = new RowMaker(myMaster, outDirRows);
            rm.genRowsClasses();

            SqlDbConfig_nnMaker sdcm = new SqlDbConfig_nnMaker(myMaster, outFileSqlDbConfigNN);
            sdcm.genSqlDbCongigSubclass();

            ConfigStubMaker csm = new ConfigStubMaker(configStubOutFile, myMaster);
            csm.genConfigStub();

            CreateOracleDBScriptMaker cod = new CreateOracleDBScriptMaker(sqlOutFile, myMaster);
            cod.genCreateDatabaseSql();

            

        }


        
 


    }
}
