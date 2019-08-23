using System;
using System.Collections.Generic;

//using PCAxis.Sql.Parser_23;

using log4net;

namespace PCAxis.Sql.Parser_23
{
    /// <summary>
    /// Handles the sending of VALUE and CODE to paxiom 
    /// </summary>
    public class PXSqlParserForCodelists_23 : PCAxis.PlugIn.Sql.PXSqlParserForCodelists
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParserForCodelists_23));

        
        //private List<PXSqlVariable> _variables;
        private List<PXSqlVariableClassification> _variables;
    
        private PXSqlMeta_23 mPXSqlMeta;
   
        #region Constructors


        /// <summary>
        /// Creating a parser that sends data for the named variable
        /// </summary>
        /// <param name="inPXSqlMeta">PXSqlMeta holding variables</param>
        /// <param name="variableCode">Name of the vaiable</param>
         public PXSqlParserForCodelists_23(PXSqlMeta_23 inPXSqlMeta, String variableCode) {
             mPXSqlMeta = inPXSqlMeta;
             _variables = new List<PXSqlVariableClassification>();
             _variables.Add(inPXSqlMeta.VariablesClassification[variableCode]);
        }



        #endregion
  
        /// <summary>
        /// Sends the data
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="preferredLanguage"></param>
         public override void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage)
         {
             foreach (PXSqlVariableClassification variable in _variables)
             {
                
                 variable.ParseForApplyValueSet(handler, mPXSqlMeta.LanguageCodes, preferredLanguage);
             }


         }

  
        /// <summary>
         /// IDisposable implemenatation
        /// </summary>
         override public void Dispose()
         {
            mPXSqlMeta = null;
        }

    }
}


