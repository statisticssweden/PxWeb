using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using log4net;
using System.Collections.Specialized;

namespace PCAxis.Sql.Pxs {

    public partial class PxsQuery : System.ICloneable {
        private static readonly ILog log = LogManager.GetLogger(typeof(PxsQuery));

        public PxsQuery() { }

     

        /// <summary>
        /// Creates PxsQuery from an array of fileLines.
        /// </summary>
        /// <param name="fileLines">The lines</param>
        /// <param name="resultLanguage">If no language is found in the lines, this is used</param>
        public PxsQuery(String[] fileLines, string resultLanguage) {
            this.ReadFlatFile(fileLines,resultLanguage);
        }

     
        /// <summary>
        /// Creates PxsQuery from a file. If the pxsPath ends with pxs or pxsb the file 
        /// is assumed a pxs-flatfile. Otherwise it is assumed to be a xml-file. 
        /// </summary>
        /// <param name="pxsPath">Path of file to be read</param>
        /// <param name="resultLanguage">When used for old pxs:If no language is found in the lines, this is used</param>
        public PxsQuery(string pxsPath, string resultLanguage) {

            if (pxsPath.EndsWith(".pxs") || pxsPath.EndsWith(".pxsb")) {
                log.Debug("Flat File");
                String[] fileLines = File.ReadAllLines(pxsPath, Encoding.Default);
                this.ReadFlatFile(fileLines,resultLanguage);
            } else {
                //string pxsPath = @"PxsTest.xml";
                XmlTextReader pxsReader = new XmlTextReader(pxsPath);
                //FileStream pxsReader = new FileStream(pxsPath,FileMode.Open);   
                XmlSerializer pxsSerial = new XmlSerializer(typeof(PxsQuery));

                /*PxsQuery PxsXmlDeserialized = new PxsQuery();*/
                PxsQuery PxsXmlDeserialized;
                PxsXmlDeserialized = (PxsQuery)pxsSerial.Deserialize(pxsReader);

                /*this.PxsXmlDeserialized = PxsXmlDeserialized;*/

                this.Information = PxsXmlDeserialized.Information;
                this.Query = PxsXmlDeserialized.Query;
                this.Presentation = PxsXmlDeserialized.Presentation;

            }
        }



        /* ville det vært bedre med?
        public static PxsQuery GetPxsQueryFromXML(string pxsPath) {
            XmlTextReader pxsReader = new XmlTextReader(pxsPath);
           
            XmlSerializer pxsSerial = new XmlSerializer(typeof(PxsQuery));

            
            PxsQuery PxsXmlDeserialized = (PxsQuery)pxsSerial.Deserialize(pxsReader);
            return PxsXmlDeserialized;
        }
        */



        private void init1() {
            this.Information = new InformationType();

            this.Information.MetaVersion = "2.0";
            this.Information.CreatedBy = new CreatedByType();
            this.Information.CreatedDate = DateTime.Now;

            this.Query = new QueryType();
            this.Query.Time = new TimeType();
            this.Query.Contents = new QueryTypeContents();
            this.Query.Contents.code = "ContentsCode";
            this.Query.Languages = new QueryTypeLanguages();
        }

        /// <summary>
        /// Sets an indication that texts should be extracted in all languages of 
        /// the datasource at the time of extraction.
        /// </summary>
        public void setLangToAll(){
            allType[] tmpLang = new allType[1];
                tmpLang[0] = new allType();
                this.Query.Languages.Items = tmpLang;
        }

        /// <summary>
        /// Sets the timeOption to "Everything you got"
        /// </summary>
        public void setTimeOptionToAll() {
            this.Query.Time.TimeOption = (TimeTypeTimeOption)4;
            this.Query.Time.Item = null;
        }


       
       
        /// <summary>
        /// Sets an indication that texts should be extracted in the languages of langs.
        /// </summary>
        /// <param name="langs">List of languagecodes. If empty setLangToAll() is called.</param>
        public void setLangs(List<string> langs){
            if (langs.Count == 0) {
                setLangToAll();
            } else {
                myLanguageType[] tmpMyLangs = new myLanguageType[langs.Count];
                for (int n = 0; n < langs.Count; n++) {
                    tmpMyLangs[n] = new myLanguageType();
                    tmpMyLangs[n].Value = langs[n];
                }

                this.Query.Languages.Items = tmpMyLangs;
            }
        }


        public void fixPresentation(string[] allHeadCodes, string[] allStubCodes ) {
            this.Presentation = new PresentationType();
            StringCollection allCodesInQuery = new StringCollection();
            allCodesInQuery.Add(this.Query.Time.code); 
           
          
            //if (this.Query.Contents.Content.Length > 1) {
            allCodesInQuery.Add(this.Query.Contents.code);
            //}
            foreach (PQVariable var in this.Query.Variables) {
                log.Debug("'" + var.code + "'");
                allCodesInQuery.Add(var.code);
            }
            
            if (allHeadCodes.Length > 0) {
                this.Presentation.Heading = new AxisType[allHeadCodes.Length];
                for (int n = 0; n < allHeadCodes.Length; n++) {
                    if (!allCodesInQuery.Contains(allHeadCodes[n])) {
                        String allCodesInQueryStr = "";
                        foreach (string code in allCodesInQuery) {
                            allCodesInQueryStr += ",'" + code+"'";
                        }
                        allCodesInQueryStr = allCodesInQueryStr.Substring(1);
                        throw new System.ApplicationException("Error: No code in query matches code for heading presentation '" + allHeadCodes[n] + "'. The following are valid:"+allCodesInQueryStr);
                    } else {
                        allCodesInQuery.Remove(allHeadCodes[n]);
                    }
                    this.Presentation.Heading[n] = new AxisType(allHeadCodes[n], n);
                }
            }
            if (allStubCodes.Length > 0) {
                this.Presentation.Stub = new AxisType[allStubCodes.Length];
                for (int n = 0; n < allStubCodes.Length; n++) {
                    if (allCodesInQuery.IndexOf(allStubCodes[n]) < 0) {
                        throw new System.ApplicationException("Error: No code in query matches code for stub presentation '" + allStubCodes[n] + "'");
                    } else {
                        allCodesInQuery.Remove(allStubCodes[n]);
                    }
                    this.Presentation.Stub[n] = new AxisType(allStubCodes[n], n);
                }
            }
            //what to do with leftovers?
            foreach (string leftoverCode in allCodesInQuery) {
                log.Info("Code: '" + leftoverCode + "' is not in stub nor heading");
            }
        }


        public void WriteToFile(string outfile) {
            System.Xml.Serialization.XmlSerializer mySer = new System.Xml.Serialization.XmlSerializer(typeof(PCAxis.Sql.Pxs.PxsQuery));
            StreamWriter myWriter = new StreamWriter(outfile);
            mySer.Serialize(myWriter, this);
            myWriter.Close();
        }

        #region ICloneable Members

        public object Clone()
        {

            var serializer = new XmlSerializer(this.GetType());
            var stream = new MemoryStream();
            serializer.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            return serializer.Deserialize(stream);

        }

        #endregion
    }


    public partial class BasicValueType {
        public BasicValueType() { }

        public BasicValueType(string valueCode, int sortOrder) {
            this.code = valueCode;
            this.sortOrder = sortOrder;
        }
    }


    public partial class ValueTypeWithGroup {
        public ValueTypeWithGroup() { }

        public ValueTypeWithGroup(string valueCode, int sortOrder) {
            this.code = valueCode;
            this.sortOrder = sortOrder;
        }
    }


    public partial class AxisType {
        public AxisType() { }
        public AxisType(string code, int index) {
            this.code = code;
            this.index = index;
        }

    }

    public partial class PQVariable
    {

        /// <summary>
        /// Returns the codes in document order from the pxs, throws exception if Wildcards is used a code
        /// </summary>
        /// <returns></returns>
        public StringCollection GetCodesNoWildcards()
        {
            StringCollection myOut = new StringCollection();
            if (this.Values.Items.Length > 0)
            {
                foreach (ValueTypeWithGroup value in this.Values.Items)
                {
                    if (value.code.Contains("*") || value.code.Contains("?"))
                    {
                        throw new ApplicationException("Wildcards are not allowed in this context");
                    }
                    myOut.Add(value.code);
                }
            }
            return myOut;
        }
      
    }
   

}




