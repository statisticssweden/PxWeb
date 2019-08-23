using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Paxiom;
using log4net;

namespace PCAxis.Sql.Parser_22
{

    /// <summary>
    /// Setter inn faste nøkkelord som ikke ligger i basen.
    /// </summary>
    public class ParseMetaIndependentOfMaintable
    {
     private static readonly ILog log = LogManager.GetLogger(typeof(ParseMetaIndependentOfMaintable));

     internal ParseMetaIndependentOfMaintable() { }

     ///<ships PXKeywords="CHARSET"> "ANSI" </ships> 
     ///<ships PXKeywords="AXIS_VERSION"> "2000" <hei>hei</hei></ships>
     ///<ships PXKeywords="CREATION_DATE"> System.DateTime.UtcNow </ships>
     /// <PXKeyword name="CODEPAGE">
     ///   <rule>
     ///     <description>If there exists a Optional_PXCodepage entry in the keywords-section of the dbconfig then the corresponding MetaAdm-row is used otherwise "iso-8859-1"</description>
     ///     <table modelName ="MetaAdm">
     ///     <column modelName="Value"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="AXIS_VERSION">
     ///   <rule>
     ///     <description>If there exists a Optional_PXAxisVersion entry in the keywords-section of the dbconfig then the corresponding MetaAdm-row is used otherwise "2000"</description>
     ///     <table modelName ="MetaAdm">
     ///     <column modelName="Value"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// 
     /// <PXKeyword name="CHARSET">
     ///   <rule>
     ///      <description>If there exists a Optional_PXCharset entry in the keywords-section of the dbconfig then the corresponding MetaAdm-row is used otherwise "ANSI"</description>
     ///     <table modelName ="MetaAdm">
     ///     <column modelName="Value"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="DESCRIPTIONDEFAULT">
     ///   <rule>
     ///     <description>If there exists a Optional_PXDescriptionDefault entry in the keywords-section of the dbconfig then the corresponding MetaAdm-row is used otherwise false</description>
     ///     <table modelName ="MetaAdm">
     ///     <column modelName="Value"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// 
     /// <PXKeyword name="CREATION_DATE">
     ///   <rule>
     ///     <description> System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz") </description>
     ///     
     ///   </rule>
     /// </PXKeyword>
        internal void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, PXMetaAdmValues metaAdm ) {
         string noLanguage = null;
         string subkey = null;
         StringCollection values = new StringCollection();

         values.Add(metaAdm.PXCharset);
         handler(PXKeywords.CHARSET, noLanguage, subkey, values);

        
         values.Clear();
         values.Add(metaAdm.PXAxisVersion);
         handler(PXKeywords.AXIS_VERSION, noLanguage, subkey, values);

         values.Clear();
         values.Add(metaAdm.PXCodepage);
         handler(PXKeywords.CODEPAGE, noLanguage, subkey, values);

         values.Clear();
         if (metaAdm.PXDescriptionDefault) {
             values.Add(PXConstant.YES);
         } else {
             values.Add(PXConstant.NO);
         }
         handler(PXKeywords.DESCRIPTIONDEFAULT, noLanguage, subkey, values);
         
         values.Clear();
         
        // string nowString = System.DateTime.UtcNow.ToString("yyyyMMdd hh:mm"); bug 193 wrong time
         //  string nowString = System.DateTime.Now.ToString();
        // string nowString = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
         // ørebro vedtak(beslut).
         //ørebro 27.11.2012 Nytt formatvedtak
            //string nowString = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
         string nowString = System.DateTime.Now.ToString("yyyyMMdd HH:mm");

         values.Add(nowString);
         
         handler(PXKeywords.CREATION_DATE, noLanguage, subkey, values);
     }


    }
}
