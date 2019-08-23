using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Sql.QueryLib_22;
using PCAxis.Paxiom;
using PCAxis.Sql.Pxs;
using log4net;

namespace PCAxis.Sql.Parser_22
{
    public class PXSqlVariables : Dictionary<string, PXSqlVariable>
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlVariables));

        PXSqlMeta_22 meta;
        public PXSqlVariables(PXSqlMeta_22 meta)
        {
            this.meta = meta;
        }


        /// <summary>
        /// True if one or more variable has applied grouping and the grouping requires a sum in the dataextraction, i.e. not all data are stored .
        /// </summary>
        internal bool HasAnyoneGroupingOnNonstoredData()
        {
           
                foreach (PXSqlVariable var in this.Values)
                {
                    if (var.UsesGroupingOnNonstoredData() )
                    {
                        return true;
                    }
                }
                return false;
           
        }


        internal void setStubHeadPxs()
        {
            int highestUsedStubIndex = 0 ;
            if (meta.PxsFile.Presentation.Stub != null)
                foreach (  AxisType stub in meta.PxsFile.Presentation.Stub)
                {
                    //mSqlVariable = this[stub.code];
                    this[stub.code].Index = stub.index;
                    if(stub.index > highestUsedStubIndex ) {
                        highestUsedStubIndex = stub.index;
                    }
                    this[stub.code].IsStub = true;
                }

            if (meta.PxsFile.Presentation.Heading != null)
            {
                foreach (AxisType heading in meta.PxsFile.Presentation.Heading)
                {
                    //mSqlVariable = this[heading.code];
                     this[heading.code].Index = heading.index;
                     this[heading.code].IsHeading = true;
                }
            }

            foreach (PXSqlVariable tmpVar in this.Values) {
                if (tmpVar.isSelected && (!tmpVar.IsHeading) && (!tmpVar.IsStub)) {
                    log.Warn("Variable " + tmpVar.Name + " isSelected, but neither Heading nor Stub. Setting it to stub");
                    highestUsedStubIndex++;
                    tmpVar.IsStub = true;
                    tmpVar.Index = highestUsedStubIndex;

                }
            }



        }
        internal void setStubHeadDefault()
        {
            //int stubIndex = 1;// eller 0 ??????
            foreach (PXSqlVariable tmpVar in this.Values)
            {
                if (tmpVar.isSelected)
                {
                    if (tmpVar.IsTimevariable)
                    {
                       // tmpVar.Index = 1;
                        tmpVar.IsHeading = true;
                    }
                    else if (tmpVar.IsContentVariable)
                    {
                       // tmpVar.Index = 2;
                        tmpVar.IsHeading = true;
                    }
                    else
                    {
                       // tmpVar.Index = stubIndex;
                       // stubIndex++;
                        tmpVar.IsStub = true;
                    }
                }
            }

        }


        internal StringCollection GetSelectedClassificationVarableIds()
        {
            StringCollection mOut = new StringCollection();
            foreach (PXSqlVariable var in this.Values)
            {
                if (var.isSelected && var.IsClassificationVariable)
                {
                    mOut.Add(var.Name);
                }
            }
            return mOut;
        }


        internal List<PXSqlVariable> GetHeadingSorted()
        {
            List<PXSqlVariable> mHeadings = new List<PXSqlVariable>();
            foreach (PXSqlVariable var in this.Values)
            {
                if (var.isSelected)
                {
                    if (var.IsHeading)
                    {
                        mHeadings.Add(var);
                    }
                }
            }
                mHeadings.Sort();
                return mHeadings; 
        }
        internal List<PXSqlVariable> GetStubSorted()
        {
            List<PXSqlVariable>  mStubs = new List<PXSqlVariable>();
            foreach (PXSqlVariable var in this.Values)
            {
                if (var.isSelected)
                {
                    if (var.IsStub)
                    {
                        mStubs.Add(var);
                    }
                }
            }
            mStubs.Sort();
            return mStubs;
        }
        internal void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes)
        {
            string noLanguage = null;
            string keyword;
            string subkey = null;
            StringCollection values = new StringCollection();
            //STUB
            keyword = PXKeywords.STUB;
            foreach (PXSqlVariable var in this.GetStubSorted())
            {
                values.Add(var.Name);
            }
            handler(keyword, noLanguage, subkey, values);
            values.Clear();

            //HEADING
            keyword = PXKeywords.STUB;
            values = new StringCollection();
            foreach (PXSqlVariable var in this.GetHeadingSorted())
            {
                values.Add(var.Name);
            }
            handler(keyword, noLanguage, subkey, values);
            values.Clear();
        }
    }
}

    

