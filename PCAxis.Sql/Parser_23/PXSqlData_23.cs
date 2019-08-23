

namespace PCAxis.Sql.Parser_23
{

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using log4net;
    using PCAxis.Sql.Parser;
    using PCAxis.Sql.QueryLib_23;
    using System.Linq;
    

    /// <summary>
    /// <remarks>
    /// MINDEX:
    /// We need to convert a point(one value for each variable) in 
    /// the cube to a number(the index of the array).
    /// index= ( Nj*Ni*(k-1) + Ni * (j - 1)+ 1*(i-1))
    /// k,j, i... 1-based counters
    /// Nx number of values for x
    /// Factor_k=Nj*Ni
    /// Factor_j= Ni
    /// Factor_i = 1
    /// index = Factor_k*(k-1) + Factor_j*(j-1) + Factor_i(i-1) </remarks>
    /// </summary>
    public class PXSqlData_23 : PXSqlData
    {
        /// <summary>The Log</summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlData_23));
        #region members and propreties
        /// <summary>Column that helps determin the index in the output array</summary>
        private const string MINDEX_COL = "MINDEX";

        private PXSqlMeta_23 mMeta;

        //these booleans influence the SQL-query
        private bool npm = false;
        private bool hasGrouping = false;

        private bool eliminationBySum = false;
        private bool useSum = false;// = hasGrouping || eliminationBySum

        //number of output variables:( does not include eleminated variables  )
        // hva med eleminated by value?
        private int numberOfOutputVariables;

        //number of values indexed by variable:
        private Dictionary<string, int> mValueCount;

        // holds results for one ore more attributecolumns.
        private const string attributeResultColumn = "attributeResultColumn";

        // Do selected rows have attributes?
        private bool hasAttributes = false;

        /// <summary>
        /// </summary>
        protected Dictionary<string, StringCollection> _AttributesEntries = new Dictionary<string, StringCollection>();

        /// <summary>
        /// </summary>
        internal Dictionary<string, StringCollection> AttributesEntries
        {
            get { return _AttributesEntries; }
        }

        /// <summary>
        /// </summary>
        private string _DefaultAttributes;
        /// <summary>
        /// </summary>
        internal string DefaultAttributes
        {
            get { return _DefaultAttributes; }
        }

        /// <summary>
        /// keys of the Output Variables in Reversed Output order. (Is List so that in can be reversed.)
        /// </summary>
        private List<string> variableIDsInReverseOutputOrder;
        //keys for variables that need TempTables: Output and eliminated by value
        //private StringCollection keysForTempTables = new StringCollection();

        private StringCollection keysOfEliminatedByValue = new StringCollection();
        private StringCollection keysOfEliminatedBySum = new StringCollection();

        private string theKeyOfTheContentsVariableVariable = "";

        private Dictionary<string, int> mIndexFactor;

        //one part of the sql (the Join part :-)

        /// <summary>
        /// Contains the columnnames of the columns needed to construct the id of the cell for DATANOTE CELL/SUM  
        /// </summary>
        private StringCollection DataNoteCellId_Columns = new StringCollection();
        private StringCollection AttributeCellId_Columns = new StringCollection();


        /// <summary>
        /// The number of expected in-rows per out-row due to elimination by sum.
        /// (the product of the number of possible codes for each 
        /// eliminated variable.
        /// </summary>
        int eliminationFactor = 1;

        /// <summary>
        /// the names of the content variables 
        /// </summary>
        private StringCollection contKeys = new StringCollection();


        private readonly PXSqlNpm symbols;


        private HashSet<PXSqlNpm.NPMCharacter> _usedNPMCharacters = new HashSet<PXSqlNpm.NPMCharacter>();

        internal IEnumerable<PXSqlNpm.NPMCharacter> UsedNPMCharacters
        {
            get
            {
                return _usedNPMCharacters;
            }
        }

        private string[] _dataCellNotes = null;

        internal override string[] DataCellNotes
        {
            get
            {
                return _dataCellNotes;
            }
        }

        //value to be used if row is missing
        private readonly double[] ValueOfCellsInMissingRows;
        //NMP-category(extended by "0" for 0.0) of value to be used if row is missing
        private readonly string[] CategoryOfCellsInMissingRows;

        private readonly Dictionary<double, PXSqlNpm.NPMCharacter> npmCharachterByMissingRowValue = new Dictionary<double, PXSqlNpm.NPMCharacter>();

        //false if missing row means 0.0 for all cont variables
        private bool anyDefaultMRNotZero;
        private bool anyDefaultMROfCat3;

        //true if anyDefaultMROfCat3 && hasGrouping
        private bool needGroupingFactor;

        #endregion members and propreties

        #region contructors
        public PXSqlData_23(PXSqlMeta_23 mPXSqlMeta)
        {
            log.Debug("Start PXSqlData mPXSqlMeta.Name: " + mPXSqlMeta.Name);

            this.mMeta = mPXSqlMeta;


            symbols = mMeta.mPxsqlNpm;

            this.npm = mMeta.SpecCharExists;
            this.hasGrouping = mMeta.Variables.HasAnyoneGroupingOnNonstoredData();
            //                .HasAnyoneGrouping();

            this.eliminationBySum = mMeta.EliminatedVariablesExist;

            //System.Console.WriteLine("DEBUG tukler med nmp og summ");
            //npm = true;
            //alwaysUseSum = true;

            this.useSum = hasGrouping || eliminationBySum;

            this.hasAttributes = mMeta.Attributes.HasAttributes;

            variableIDsInReverseOutputOrder = mMeta.GetVariableIDsInReverseOutputOrder();

            numberOfOutputVariables = variableIDsInReverseOutputOrder.Count;

            mValueCount = new Dictionary<string, int>(numberOfOutputVariables);
            mIndexFactor = new Dictionary<string, int>(numberOfOutputVariables);

            foreach (PXSqlVariable var in mMeta.Variables.Values)
            {
                if (var.IsEliminatedByValue)
                {
                    log.Debug(var.Name + " Is Eliminated By Value");
                    keysOfEliminatedByValue.Add(var.Name);
                }
                else if (var.IsContentVariable)
                {
                    log.Debug(var.Name + " Is Content Variable");
                    theKeyOfTheContentsVariableVariable = var.Name;
                }
                else if (!var.isSelected)
                {
                    log.Debug(var.Name + " Is Eliminated By SUM");
                    keysOfEliminatedBySum.Add(var.Name);
                    eliminationFactor *= var.TotalNumberOfValuesInDB;
                }
            }

            #region init contKeys
            if (String.IsNullOrEmpty(theKeyOfTheContentsVariableVariable))
            { //just one contVar
                throw new ApplicationException("Bug");
                // the size of ContentsVariableVariable should not influence how it is stored 
                //                contKeys.Add( mMeta.FirstContents );
            }
            else
            {
                PXSqlVariable theContentsVariable = mMeta.Variables[theKeyOfTheContentsVariableVariable];
                foreach (PXSqlValue contCode in theContentsVariable.GetValuesForParsing())
                {
                    //contKeys.Add(contCode.ValueCode);
                    contKeys.Add(contCode.ContentsCode); // 2010.05.07  replaces line above because valuecode is now Prescode from contents. New contentscode added to PXSqlValue
                }
                foreach (string contCode in theContentsVariable.Values.Keys)
                {
                    //    contKeys.Add(contCode);
                }

            }

            #endregion init contKeys

            #region init defaults for missing rows
            //value to be used if row is missing
            ValueOfCellsInMissingRows = new double[contKeys.Count];
            CategoryOfCellsInMissingRows = new string[contKeys.Count];

            int contCount2 = 0;

            anyDefaultMRNotZero = false;  // MR = missing record
            anyDefaultMROfCat3 = false;   // MR = missing record
            foreach (string contCode in contKeys)
            {
                PXSqlContent tmpCont = mMeta.Contents[contCode];
                log.Debug("PXSqlContents for " + contCode + " PresCellsZero:" + tmpCont.PresCellsZero + " PresMissingLine:" + tmpCont.PresMissingLine);

                CategoryOfCellsInMissingRows[contCount2] = tmpCont.CategoryOfCellsInMissingRows;
                ValueOfCellsInMissingRows[contCount2] = tmpCont.ValueOfCellsInMissingRows;

                if (!npmCharachterByMissingRowValue.ContainsKey(tmpCont.ValueOfCellsInMissingRows))
                {
                    npmCharachterByMissingRowValue[tmpCont.ValueOfCellsInMissingRows] = tmpCont.NPMcharacterinMissingRows;
                }

                if (!CategoryOfCellsInMissingRows[contCount2].Equals("0"))
                {
                    anyDefaultMRNotZero = true;
                    if (CategoryOfCellsInMissingRows[contCount2].Equals("3"))
                    {
                        anyDefaultMROfCat3 = true;
                    }
                }
                contCount2++;
            }
            #endregion init defaults for missing rows


            log.Debug("eliminationFactor: " + eliminationFactor);



            log.Debug("useSum:" + useSum + " , npm: " + npm);
            log.Debug("Constructor done.");
        }
        #endregion contructors


        private string CreateSqlString()
        {




            string sqlString = "select /" + "*+STAR_TRANSFORMATION *" + "/ ";


            int TempNumber = 25;

            string sqlJoinString = "";
            string sqlGroupByString = "";
            string tempGroupFactorSQL = "";


            foreach (string key in variableIDsInReverseOutputOrder)
            {
                PXSqlVariable var = mMeta.Variables[key];

                if (!var.IsContentVariable)
                {
                    #region if (!var.IsContentVariable)

                    //TODO piv
                    if (hasAttributes)
                    {
                        sqlString += key + ",";
                    }
                    var.TempTableNo = TempNumber.ToString();
                    TempNumber++;

                    if (var.UsesGroupingOnNonstoredData())
                    {
                        List<PXSqlGroup> groupCodes = ((PXSqlVariableClassification)var).GetGroupsForDataParsing();
                        if (groupCodes.Count == 0)
                        {
                            throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(2, var.Name);
                        }

                        var.TempTableName = mMeta.MetaQuery.MakeTempTable(var.Name, var.TempTableNo, groupCodes, needGroupingFactor && var.UsesGroupingOnNonstoredData());

                    }
                    else
                    {

                        List<PXSqlValue> valueCodes = var.GetValuesForParsing();
                        if (valueCodes.Count == 0)
                        {
                            throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(2, var.Name);
                        }
                        var.TempTableName = mMeta.MetaQuery.MakeTempTable(var.Name, var.TempTableNo, valueCodes);



                    }


                    #region sql
                    sqlJoinString += " JOIN " + var.TempTableName + " ON dt." + key +
                            " = " + var.TempTableName + ".a" + key + " ";

                    if (useSum)
                    {
                        sqlGroupByString += ", Groupnr" + var.TempTableNo;
                    }

                    if (npm)
                    {
                        //We need the grupp-variables in case of a DataNoteCell
                        if (useSum)
                        {
                            sqlGroupByString += ", Group" + var.TempTableNo;
                        }
                        DataNoteCellId_Columns.Add("Group" + var.TempTableNo);
                        sqlString += "Group" + var.TempTableNo + ", ";
                    }

                    // tempGroupFactorSQL will be used in an UPDATE statement to adjust the GroupFactor in the temptables
                    if (needGroupingFactor && var.UsesGroupingOnNonstoredData())
                    {
                        if (!String.IsNullOrEmpty(tempGroupFactorSQL))
                        {
                            tempGroupFactorSQL += " * ";
                        }
                        tempGroupFactorSQL += "MAX(GroupFactor" + var.TempTableNo + ")";
                        //Max: they should all be equal
                    }
                    #endregion sql
                    #endregion
                }   // (!var.IsContentVariable)
                else
                {
                    if (npm)
                    {
                        DataNoteCellId_Columns.Add("");
                    }
                    log.Debug(" is ContentVariable ");
                }
                mValueCount[key] = var.Values.Count;
            }   // foreach (string key in keysInReverseOutputOrder)

            // INDEXFACTOR

            string lPrevKey = "";
            foreach (string key in variableIDsInReverseOutputOrder)
            {
                if (String.IsNullOrEmpty(lPrevKey))
                {
                    mIndexFactor.Add(key, 1);
                }
                else
                {
                    mIndexFactor.Add(key, mIndexFactor[lPrevKey] * mValueCount[lPrevKey]);
                }
                lPrevKey = key;
            }

            // mSize = matrix size, i.e. the number of cells in the resulting matrix
            mSize = mIndexFactor[lPrevKey] * mValueCount[lPrevKey];// = all ValueCount entries multiplied

            // CONTENTS 

            foreach (string contCode in contKeys)
            {
                sqlString += getContSelectPart(contCode, useSum, npm);
            }

            if (useSum)
            {
                sqlString += " COUNT(*) AS SqlMarx777777, ";
            } //else {
            //  sqlString += " 1 SqlMarx777777 , ";
            //}

            // MINDEX

            string mindexSQLString = " (";

            foreach (string key in variableIDsInReverseOutputOrder)
            {
                if (key.Equals(theKeyOfTheContentsVariableVariable))
                {
                    continue;
                }
                mindexSQLString += " " + mIndexFactor[key] +
                             " * (Groupnr" + mMeta.Variables[key].TempTableNo + " -1) + ";
            }
            mindexSQLString += "0) AS " + MINDEX_COL + " \n";

            sqlString += mindexSQLString;

            if (needGroupingFactor)
            {
                sqlString += ", " + tempGroupFactorSQL + " GroupFactor ";
            }
            //TODO piv
            if (hasAttributes)
            {
                sqlString += getAttrSelectPart(mMeta.Attributes) + " ";
            }

            // FROM & JOIN 

            sqlString += getFROMClause() + sqlJoinString;

            sqlString += "\n WHERE 1=1 ";
            foreach (string key in keysOfEliminatedByValue)
            {
                foreach (PXSqlValue val in mMeta.Variables[key].Values.Values)
                { // there will only be one
                    sqlString += " AND " + key + " = '" + val.ValueCode + "'";
                }
            }

            // GROUP BY
            if (useSum)
            {
                sqlString += " GROUP BY " + sqlGroupByString.Substring(1);
            }

            return sqlString;
            //log.Debug("SQL:\n" + sqlString);
        }   // private void CreateSqlString()


        /// <summary>
        /// creates and executes the sqlString and returns a double array
        /// </summary>
        /// <returns></returns>
        /// 

        override public double[] CreateMatrix()
        {
            log.Debug("Start CreateMatrix()");

            needGroupingFactor = anyDefaultMROfCat3 && hasGrouping;
            log.Debug("Post needGroupingFactor=" + needGroupingFactor.ToString() + " because: anyDefaultMROfCat3= " + anyDefaultMROfCat3.ToString() + " , hasGrouping= " + hasGrouping.ToString());

            string sqlString = CreateSqlString();

            //log.Debug("sqlString:" + sqlString);

            // the factor OfTheContentsVariableVariable:
            int contFactor = 100;// the value does not matter. it is either overwritten or  multiplied by 0  
            if (!String.IsNullOrEmpty(theKeyOfTheContentsVariableVariable))
            {
                contFactor = mIndexFactor[theKeyOfTheContentsVariableVariable];
            }

            double[] myOut = new double[mSize]; // the array to be returned initialized to 0.0
            _dataCellNotes = new string[mSize];

            #region init array
            // OBS debug:
            //anyDefaultMRNotZero = true;


            if (anyDefaultMRNotZero)
            {
                if (String.IsNullOrEmpty(theKeyOfTheContentsVariableVariable))
                {
                    for (int i = 0; i < mSize; i++)
                    {
                        myOut[i] = ValueOfCellsInMissingRows[0];
                    }
                }
                else
                {
                    int prevFactor = contFactor * contKeys.Count;
                    for (int cont = 0; cont < contKeys.Count; cont++)
                    {
                        if (ValueOfCellsInMissingRows[cont] == 0)
                        {
                            continue;  //myOut is already 0
                        }
                        for (int k = 0; k < mSize; k += prevFactor)
                        {
                            for (int l = 0; l < contFactor; l++)
                            {
                                myOut[l + cont * contFactor + k] = ValueOfCellsInMissingRows[cont];
                            }
                        }
                    }
                }
            }
            #endregion init array

            //dataoppslag må vel flyttes
            // det kan hende vi må bytte fra DataSet til DataReader her
            log.Debug("sql start");


            DataRowCollection myRows = mMeta.MetaQuery.ExecuteSelect(sqlString, null);
            log.Debug("sql done");

            int contCount = 0;
            int mIndex = 0;
            int arrayIndex = 0;

            bool hasBlankCells = false;
            bool hasNoMissingRows = true;
            int missingRowsCnt = 0;
            log.Debug("npm :" + npm.ToString() + " useSum:" + useSum.ToString() + " Row count: " + myRows.Count.ToString());
            if ((!npm) && (!useSum))
            {
                #region not npm and not useSum
                foreach (DataRow sqlRow in myRows)
                {
                    contCount = 0;

                    mIndex = int.Parse(sqlRow[MINDEX_COL].ToString());

                    foreach (string contCode in contKeys)
                    {
                        PXSqlNpm.NPMCharacter usedNPMCharacter = null;
                        arrayIndex = mIndex + contFactor * contCount;
                        _dataCellNotes[arrayIndex] = "";
                        hasBlankCells = int.Parse(sqlRow[contCode + "_NilCnt"].ToString()) > 0;
                        #region DNA or normal value
                        if (hasBlankCells)
                        {
                            //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                            myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                            usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                        }
                        else
                        {
                            myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                        }
                        #endregion DNA or normal value

                        //log.Debug(" arrayIndex:" + arrayIndex + " verdi " + myOut[arrayIndex] +
                        //    "      mIndex:" + mIndex + " contCode:" + contCode);

                        if (usedNPMCharacter != null && !_usedNPMCharacters.Contains(usedNPMCharacter))
                        {
                            _usedNPMCharacters.Add(usedNPMCharacter);
                        }

                        contCount++;
                    }
                    //TODO PIV
                    if (hasAttributes)
                    {
                        addAttribute(mMeta, sqlRow);
                    }
                }
                if (hasAttributes)
                {
                    setDefaultAttributes();
                }
                #endregion not npm and not useSum
            }
            else if ((!npm) && (useSum))
            {
                #region not npm and useSum

                foreach (DataRow sqlRow in myRows)
                {
                    contCount = 0;
                    mIndex = int.Parse(sqlRow[MINDEX_COL].ToString());

                    foreach (string contCode in contKeys)
                    {
                        PXSqlNpm.NPMCharacter usedNPMCharacter = null;
                        arrayIndex = mIndex + contFactor * contCount;
                        _dataCellNotes[arrayIndex] = "";
                        hasBlankCells = int.Parse(sqlRow[contCode + "_NilCnt"].ToString()) > 0;
                        // log.Debug(sqlRow[contCode].ToString());


                        if (!CategoryOfCellsInMissingRows[contCount].Equals("3"))
                        {
                            //missing rows are ignored
                            #region DNA or normal value
                            if (hasBlankCells)
                            {
                                //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                            }
                            else
                            {
                                myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                            }
                            #endregion DNA or normal value
                        }
                        else
                        {

                            //must look for missing rows
                            if (needGroupingFactor)
                            {
                                hasNoMissingRows = eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) <= int.Parse(sqlRow["SqlMarx777777"].ToString());
                            }
                            else
                            {
                                hasNoMissingRows = eliminationFactor <= int.Parse(sqlRow["SqlMarx777777"].ToString());

                            }
                            if (hasNoMissingRows)
                            {
                                #region DNA or normal value
                                if (hasBlankCells)
                                {
                                    //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                    myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                    usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                                }
                                else
                                {
                                    myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                }
                                #endregion DNA or normal value
                            }
                            else
                            {  //has missing row (of cat 3)
                                if (hasBlankCells)
                                {// and blank cells ( = DataNotAvailable = cat. 3
                                 //if (ValueOfCellsInMissingRows[contCount] != symbols.getDataNotAvailableDouble()) {
                                    if (ValueOfCellsInMissingRows[contCount] != symbols.DataNotAvailableMagic)
                                    {
                                        //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                        myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                        usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                                    } // else myOut[arrayIndex] =ValueOfCellsInMissingRows[contCount] done in init

                                }

                                if (usedNPMCharacter == null && npmCharachterByMissingRowValue.ContainsKey(myOut[arrayIndex]))
                                {
                                    // the init value of myOut has been hit/reached/used
                                    usedNPMCharacter = npmCharachterByMissingRowValue[myOut[arrayIndex]];
                                }
                            }


                        }

                        //log.Debug("DEBUG arrayIndex:" + arrayIndex + " verdi " + myOut[arrayIndex] +
                        //    "      mIndex:" + mIndex + " contCode:" + contCode);


                        if (usedNPMCharacter != null && !_usedNPMCharacters.Contains(usedNPMCharacter))
                        {
                            _usedNPMCharacters.Add(usedNPMCharacter);
                        }

                        contCount++;
                    }
                    //TODO PIV
                    if (hasAttributes)
                    {
                        addAttribute(mMeta, sqlRow);
                    }
                }
                setDefaultAttributes();
                #endregion not npm and useSum
            }
            else if ((npm) && (!useSum))
            {

                #region npm and not useSum

                foreach (DataRow sqlRow in myRows)
                {

                    contCount = 0;
                    mIndex = int.Parse(sqlRow[MINDEX_COL].ToString());

                    foreach (string contCode in contKeys)
                    {
                        PXSqlNpm.NPMCharacter usedNPMCharacter = null;
                        arrayIndex = mIndex + contFactor * contCount;
                        _dataCellNotes[arrayIndex] = "";
                        hasBlankCells = int.Parse(sqlRow[contCode + "_NilCnt"].ToString()) > 0;
                        //if blankCellCnt > 0 has cat. 3 missing either for .._XMAX or the default (=DNA) 


                        string npmMax = sqlRow[contCode + "_XMAX"].ToString();
                        if (String.IsNullOrEmpty(npmMax))
                        {
                            #region DNA or normal value
                            if (hasBlankCells)
                            {
                                //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                            }
                            else
                            {
                                myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                            }
                            #endregion DNA or normal value
                        }
                        else if (npmMax.StartsWith("B1"))
                        {
                            if (hasBlankCells)
                            {
                                //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                            }
                            else
                            {
                                myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                var npmCharacterType = npmMax.Substring(2);

                                addDataNoteCell(npmCharacterType, sqlRow, contCode);

                                var npmCharacter = symbols.GetNpmBySpeciaCharacterType(npmCharacterType);

                                if (npmCharacter.presCharacters.ContainsKey(mMeta.MainLanguageCode))
                                {
                                    _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npmCharacterType).presCharacters[mMeta.MainLanguageCode];
                                }
                                else if (npmCharacter.presCharacters.Count > 0)
                                {
                                    _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npmCharacterType).presCharacters.First().Value;
                                }
                            }
                        }
                        else
                        {// cat 2 or 3
                            //myOut[arrayIndex] = symbols.getSymbolsNoByCharType(npmMax.Substring(2));
                            myOut[arrayIndex] = symbols.DataSymbolNMagic(npmMax.Substring(2));
                            usedNPMCharacter = symbols.DataSymbolNNPMCharacter(npmMax.Substring(2));
                        }


                        //log.Debug("DEBUG arrayIndex:" + arrayIndex + " verdi " + myOut[arrayIndex] +
                        //   "      mIndex:" + mIndex + " contCode:" + contCode);

                        //log.Debug("DEBUG npm:" + sqlRow[contCode + "_xMax"].ToString());

                        if (usedNPMCharacter != null && !_usedNPMCharacters.Contains(usedNPMCharacter))
                        {
                            _usedNPMCharacters.Add(usedNPMCharacter);
                        }

                        contCount++;
                    }
                    //TODO PIV
                    if (hasAttributes)
                    {
                        addAttribute(mMeta, sqlRow);
                    }
                }
                if (hasAttributes)
                {
                    setDefaultAttributes();
                }
                #endregion npm and not useSum
            }
            else
            { // npm && useSum

                #region npm and useSum
                foreach (DataRow sqlRow in myRows)
                {
                    contCount = 0;
                    mIndex = int.Parse(sqlRow[MINDEX_COL].ToString());

                    foreach (string contCode in contKeys)
                    {
                        PXSqlNpm.NPMCharacter usedNPMCharacter = null;
                        arrayIndex = mIndex + contFactor * contCount;
                        _dataCellNotes[arrayIndex] = "";
                        hasBlankCells = int.Parse(sqlRow[contCode + "_NilCnt"].ToString()) - int.Parse(sqlRow[contCode + "_XCOUNT"].ToString()) > 0;
                        //if blankCellCnt > 0 har type 3 missing enten pga tom/blank celle
                        string npmMax = sqlRow[contCode + "_XMAX"].ToString();

                        if (String.IsNullOrEmpty(npmMax))
                        {
                            #region npmMax missing

                            // the region npmMax missing is a copy from  !npm && useSum:
                            if (!CategoryOfCellsInMissingRows[contCount].Equals("3"))
                            {
                                //missing rows are ignored
                                #region DNA or normal value
                                if (hasBlankCells)
                                {
                                    //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                    myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                    usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                                }
                                else
                                {
                                    myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                }
                                #endregion DNA or normal value
                            }
                            else
                            {

                                //must look for missing rows
                                if (needGroupingFactor)
                                {
                                    hasNoMissingRows = eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) <= int.Parse(sqlRow["SqlMarx777777"].ToString());
                                }
                                else
                                {
                                    hasNoMissingRows = eliminationFactor <= int.Parse(sqlRow["SqlMarx777777"].ToString());

                                }

                                if (hasNoMissingRows)
                                { // no missing rows
                                    #region DNA or normal value
                                    if (hasBlankCells)
                                    {
                                        //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                        myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                        usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                                    }
                                    else
                                    {
                                        myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                    }
                                    #endregion DNA or normal value
                                }
                                else
                                {  //has missing row (of cat 3)
                                    if (hasBlankCells)
                                    {// and blank cells ( = DataNotAvailable = cat. 3
                                        //if (ValueOfCellsInMissingRows[contCount] != symbols.getDataNotAvailableDouble()) {
                                        if (ValueOfCellsInMissingRows[contCount] != symbols.DataNotAvailableMagic)
                                        {
                                            //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                            myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                            usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                                        } // else myOut[arrayIndex] =ValueOfCellsInMissingRows[contCount] done in init
                                    }

                                    if (usedNPMCharacter == null && npmCharachterByMissingRowValue.ContainsKey(myOut[arrayIndex]))
                                    {
                                        // the init value of myOut has been hit/reached/used
                                        usedNPMCharacter = npmCharachterByMissingRowValue[myOut[arrayIndex]];
                                    }
                                }

                            }
                            #endregion npmMax missing

                        }
                        else if (npmMax.StartsWith("C3"))
                        {

                            #region npmMax is 3
                            // value will be a cat.3 : either DataSymbolSum or npmMax
                            string npm3Min = sqlRow[contCode + "_3MIN"].ToString();
                            if (!npmMax.Equals("C3" + npm3Min))
                            {
                                //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                                //} else if (hasBlankCells && symbols.getDoubleByCharacterType(npm3Min) != symbols.getDataNotAvailableDouble()) {
                            }
                            else if (hasBlankCells && symbols.DataSymbolNMagic(npm3Min) != symbols.DataNotAvailableMagic)
                            {
                                //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                            }
                            else
                            {
                                if (!CategoryOfCellsInMissingRows[contCount].Equals("3"))
                                { //ignore missing rows 
                                    //myOut[arrayIndex] = symbols.getDoubleByCharacterType(npm3Min);
                                    myOut[arrayIndex] = symbols.DataSymbolNMagic(npm3Min);
                                    usedNPMCharacter = symbols.DataSymbolNNPMCharacter(npm3Min);
                                }
                                else
                                { // must check for missing rows
                                    //must look for missing rows
                                    if (needGroupingFactor)
                                    {
                                        hasNoMissingRows = 1 > eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                    }
                                    else
                                    {
                                        hasNoMissingRows = 1 > eliminationFactor - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                    }

                                    if (hasNoMissingRows)
                                    {
                                        //myOut[arrayIndex] = symbols.getDoubleByCharacterType(npm3Min);
                                        myOut[arrayIndex] = symbols.DataSymbolNMagic(npm3Min);
                                        usedNPMCharacter = symbols.DataSymbolNNPMCharacter(npm3Min);
                                    }
                                    else
                                    {  //has missing row
                                        if (ValueOfCellsInMissingRows[contCount] != symbols.DataSymbolNMagic(npm3Min))
                                        {
                                            //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                            myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                            usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                                        } // else : nmpMax equals the initialization-value 


                                        if (usedNPMCharacter == null && npmCharachterByMissingRowValue.ContainsKey(myOut[arrayIndex]))
                                        {
                                            // the init value of myOut has been hit/reached/used
                                            usedNPMCharacter = npmCharachterByMissingRowValue[myOut[arrayIndex]];
                                        }
                                    }
                                }

                            }
                            #endregion npmMax is 3

                            //below: npmMax is cat. 1 or 2  but blank cells or missing rows migth give cat.3
                        }
                        else if (hasBlankCells)
                        {// must add DataNotAvaliable
                            #region has blankCell
                            if (!CategoryOfCellsInMissingRows[contCount].Equals("3"))
                            { //ignore missing rows 
                                //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                            }
                            else
                            { // must look for missing rows
                                if (needGroupingFactor)
                                {
                                    hasNoMissingRows = 1 > eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                }
                                else
                                {
                                    hasNoMissingRows = 1 > eliminationFactor - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                }

                                if (hasNoMissingRows)
                                {
                                    //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                    myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                    usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                                }
                                else
                                { //has missing rows
                                    //if (ValueOfCellsInMissingRows[contCount] == symbols.getDataNotAvailableDouble()) {
                                    if (ValueOfCellsInMissingRows[contCount] == symbols.DataNotAvailableMagic)
                                    {
                                        //myOut[arrayIndex] = symbols.getDataNotAvailableDouble();
                                        myOut[arrayIndex] = symbols.DataNotAvailableMagic;
                                        usedNPMCharacter = symbols.DataNotAvailableNPMCharacter;
                                    }
                                    else
                                    {
                                        //myOut[arrayIndex] = symbols.getDataSymbolSumDouble();
                                        myOut[arrayIndex] = symbols.DataSymbolSumMagic;
                                        usedNPMCharacter = symbols.DataSymbolSumNPMCharacter;
                                    }

                                }
                            }
                            #endregion has blankCell
                            //below: npmMax is cat. 1 or 2 , no blank cells, but missing rows migth give cat.3
                        }
                        else if (CategoryOfCellsInMissingRows[contCount].Equals("3"))
                        {
                            #region cat missingcells is 3
                            // must look for missing rows
                            if (needGroupingFactor)
                            {
                                missingRowsCnt = eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) - int.Parse(sqlRow["SqlMarx777777"].ToString());
                            }
                            else
                            {
                                missingRowsCnt = eliminationFactor - int.Parse(sqlRow["SqlMarx777777"].ToString());
                            }
                            if (missingRowsCnt < 1)
                            { // no missing rows
                                string npm1Min = sqlRow[contCode + "_1MIN"].ToString();
                                if (String.IsNullOrEmpty(npm1Min))
                                {//potential cat2 

                                    if (sqlRow["SqlMarx777777"].ToString().Equals(sqlRow[contCode + "_XCOUNT"].ToString()))
                                    {
                                        // cat 2 !
                                        //myOut[arrayIndex] = symbols.getDoubleByCharacterType(npmMax.Substring(2));
                                        myOut[arrayIndex] = symbols.DataSymbolNMagic(npmMax.Substring(2));
                                        usedNPMCharacter = symbols.DataSymbolNNPMCharacter(npmMax.Substring(2));
                                    }
                                    else
                                    { // normal number 
                                        myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                    }
                                }
                                else
                                { // cat.1 !
                                    myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());

                                    if (npmMax.Equals("B1" + npm1Min))
                                    {
                                        addDataNoteCell(npm1Min, sqlRow, contCode);

                                        var npmCharacter = symbols.GetNpmBySpeciaCharacterType(npm1Min);

                                        if (npmCharacter.presCharacters.ContainsKey(mMeta.MainLanguageCode))
                                        {
                                            _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters[mMeta.MainLanguageCode];
                                        }
                                        else if (npmCharacter.presCharacters.Count > 0)
                                        {
                                            _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters.First().Value;
                                        }
                                    }
                                    else
                                    {
                                        addDataNoteSum(sqlRow, contCode);

                                        var npmCharacter = symbols.GetNpmBySpeciaCharacterType(symbols.DataNoteSumCharacterType());

                                        if (npmCharacter.presCharacters.ContainsKey(mMeta.MainLanguageCode))
                                        {
                                            _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters[mMeta.MainLanguageCode];
                                        }
                                        else if (npmCharacter.presCharacters.Count > 0)
                                        {
                                            _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters.First().Value;
                                        }
                                    }
                                }
                            } //else    has missing rows : myOut[arrayIndex] = init value
                            #endregion cat missingcells is 3
                            //below:npmMax is cat. 1 or 2 , no blank cells, missing rows are kat 0 or 2 ( cat 1 is not valid)
                        }
                        else if (npmMax.StartsWith("B1"))
                        { // missing row don't matter
                            #region npmMax is 1
                            myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                            string npm1Min = sqlRow[contCode + "_1MIN"].ToString();
                            if (npmMax.Equals("B1" + npm1Min))
                            {
                                addDataNoteCell(npm1Min, sqlRow, contCode);
                                var npmCharacter = symbols.GetNpmBySpeciaCharacterType(npm1Min);

                                if (npmCharacter.presCharacters.ContainsKey(mMeta.MainLanguageCode))
                                {
                                    _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters[mMeta.MainLanguageCode];
                                }
                                else if (npmCharacter.presCharacters.Count > 0)
                                {
                                    _dataCellNotes[arrayIndex] = symbols.GetNpmBySpeciaCharacterType(npm1Min).presCharacters.First().Value;
                                }
                            }
                            else
                            {
                                addDataNoteSum(sqlRow, contCode);
                            }
                            #endregion npmMax is 1
                        }
                        else
                        { // //npmMax is cat.  2 , no blank cells, missing rows are kat 0 or 2
                            #region npmMax is 2
                            //all present and missing rows must be cat2 for myOut[arrayIndex] to be cat2 

                            if (!sqlRow["SqlMarx777777"].ToString().Equals(sqlRow[contCode + "_XCOUNT"]))
                            {
                                myOut[arrayIndex] = double.Parse(sqlRow[contCode].ToString());
                                //below: all present rows are cat. 2
                            }
                            else if (CategoryOfCellsInMissingRows[contCount].Equals("0"))
                            {
                                // must look for missing rows
                                if (needGroupingFactor)
                                {
                                    hasNoMissingRows = 1 > eliminationFactor * int.Parse(sqlRow["GROUPFACTOR"].ToString()) - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                }
                                else
                                {
                                    hasNoMissingRows = 1 > eliminationFactor - int.Parse(sqlRow["SqlMarx777777"].ToString());
                                }
                                if (hasNoMissingRows)
                                { // no missing rows
                                    //myOut[arrayIndex] = symbols.getDoubleByCharacterType(npmMax.Substring(2));
                                    myOut[arrayIndex] = symbols.DataSymbolNMagic(npmMax.Substring(2));
                                    usedNPMCharacter = symbols.DataSymbolNNPMCharacter(npmMax.Substring(2));
                                } //else  myOut[arrayIndex] = 0 but that has been done in init

                            } //else { // CategoryOfCellsInMissingRows[contCount] is  cat 2 !
                            // myOut[arrayIndex] = symbols.getDataSymbolNilDouble(); done in init
                            //}
                            #endregion npmMax is 2
                        }

                        if (usedNPMCharacter != null && !_usedNPMCharacters.Contains(usedNPMCharacter))
                        {
                            _usedNPMCharacters.Add(usedNPMCharacter);
                        }

                        contCount++;
                    }

                }
                #endregion npm and useSum
            }

            foreach (string key in variableIDsInReverseOutputOrder)
            {
                log.Debug("key: " + key + " lIndexFactor: " + mIndexFactor[key] + " antVerdier:" + mValueCount[key]);
            }

            //clean up
            mMeta.MetaQuery.DropTempTables();//In some cases they auto-drop. 

            if (ShouldAddDefaultCodeMissingLineNPMCharacter(myRows.Count))
            {
                _usedNPMCharacters.Add(symbols.DefaultCodeMissingLineNPMCharacter);
            }

            log.Debug("Done PXSqlData...");
            return myOut;
        }

        /// <summary>
        /// Checks if default code missing line NPM character should be added
        /// </summary>
        /// <param name="rowCount">number of data added through looping data fom select</param>
        /// <returns></returns>
        private bool ShouldAddDefaultCodeMissingLineNPMCharacter(int rowCount)
        {
            if (mSize > (rowCount * contKeys.Count) && !_usedNPMCharacters.Contains(symbols.DefaultCodeMissingLineNPMCharacter))
            {
                foreach (string contCode in contKeys)
                {
                    PXSqlContent content = mMeta.Contents[contCode];
                    
                    if (content.PresCellsZero == this.mMeta.Config.Codes.No && string.IsNullOrEmpty(content.PresMissingLine))
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        /// <summary>
        /// creates the select part of sql for a contentsVariableValue
        /// </summary>
        /// <param name="contCode">the name of the contents</param>
        /// <param name="Sum">true if sum should be used</param>
        /// <param name="npm">true if npm should be used</param>
        /// <returns>select string for one contents column</returns>
        private string getContSelectPart(string contCode, bool Sum, bool npm)
        {
            //SUM(Sysselsatte) Sysselsatte, MAX(Sysselsatte_x) Sysselsatte_xMax,
            string myOut = "";

            if (Sum)
            {
                myOut = " SUM(dt." + contCode + ") AS ";
                // trenger vi en AS ?
            }
            else
            {
                myOut += " dt.";
            }

            myOut += contCode + ", ";

            if (Sum)
            {
                myOut += " SUM";
            }
            myOut += "(CASE WHEN dt." + contCode + " IS NULL THEN 1 ELSE 0 END) AS " + contCode + "_NilCnt, ";

            if (npm)
            {
                //string mo= ;
                //ver 3,14 myOut += " MAX(" + contCode + "_x) " + contCode + "_xMax ,";
                string CodeForNo = "'" + mMeta.MetaQuery.DB.Codes.No + "'";
                string CodeForYes = "'" + mMeta.MetaQuery.DB.Codes.Yes + "'";
                PCAxis.Sql.DbConfig.SqlDbConfig_23.TblSpecialCharacter SpecialCharacter = mMeta.MetaQuery.DB.SpecialCharacter;


                string caseString = "(CASE " +
                    " WHEN st." + SpecialCharacter.AggregPossibleCol.PureColumnName() + " = " + CodeForNo + " THEN 'C3' " +
                    " WHEN st." + SpecialCharacter.DataCellPresCol.PureColumnName() + " = " + CodeForNo + " THEN 'A2' " +
                    " ELSE 'B1' END)";


                myOut += "\n      ";
                if (Sum)
                {
                    myOut += "MAX(";
                }

                string[] tmpConcatArray = new string[2];
                tmpConcatArray[0] = caseString;
                tmpConcatArray[1] = "st." + SpecialCharacter.CharacterTypeCol.PureColumnName();


                myOut += "(SELECT " + mMeta.MetaQuery.GetPxSqlCommand().getConcatString(tmpConcatArray) + " x_x\n";

                //                myOut += "(SELECT CONCAT(" + caseString + ", st." +
                //                               SpecialCharacter.CharacterType + ") x_x\n";
                myOut += "      FROM " + mMeta.MetaQuery.DB.MetaOwner + SpecialCharacter.TableName + " st \n";
                myOut += "      WHERE st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + " = dt." + contCode + "_X ) \n";
                if (Sum)
                {
                    myOut += ") ";
                }
                myOut += " AS " + contCode + "_xMax, ";
                myOut += "\n      ";

                if (Sum)
                {
                    //        min(select st.TeckenTyp from StatMeta.SpecialTecken st
                    //            where  st.TeckenTyp = dt.Sysselsatte_x and st.Summerbar = 'N') Sysselsatte_xMin_3
                    myOut += " MIN((";
                    myOut += " SELECT  st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + "\n";
                    myOut += "      FROM " + mMeta.MetaQuery.DB.MetaOwner + SpecialCharacter.TableName + " st \n";
                    myOut += "      WHERE st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + " = dt." + contCode + "_X AND st." + SpecialCharacter.AggregPossibleCol.PureColumnName() + " = " + CodeForNo + ")) \n";

                    myOut += " AS " + contCode + "_3MIN , ";
                    myOut += "\n ";

                    myOut += " MIN((";
                    myOut += " SELECT  st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + "\n";
                    myOut += "      FROM " + mMeta.MetaQuery.DB.MetaOwner + SpecialCharacter.TableName + " st \n";
                    myOut += "      WHERE st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + " = dt." + contCode + "_X AND st." + SpecialCharacter.AggregPossibleCol.PureColumnName() + " = " + CodeForYes + " AND st." + SpecialCharacter.DataCellPresCol.PureColumnName() + " = " + CodeForYes + ")) \n";

                    myOut += " AS " + contCode + "_1MIN , ";
                    myOut += "\n ";

                    // sum((select count(*) 
                    // from StatMeta.SpecialTecken st 
                    // where st.TeckenTyp = dt.Sysselsatte_x and category is 2 or 3)) Sysselsatte_xCount,
                    myOut += " SUM((SELECT COUNT(*) ";
                    myOut += "      FROM " + mMeta.MetaQuery.DB.MetaOwner + SpecialCharacter.TableName + " st \n";
                    myOut += "      WHERE st." + SpecialCharacter.CharacterTypeCol.PureColumnName() + " = dt." + contCode + "_X  AND st." + SpecialCharacter.AggregPossibleCol.PureColumnName() + " = " + CodeForNo + ")) AS " + contCode + "_XCount, \n";
                }
            }
            return myOut;
        }

        /// <summary>
        /// creates the FROM part of the sql
        /// </summary>
        /// <returns></returns>
        private string getFROMClause()
        {
            string myOut = "FROM ";
            StringCollection tmpTabs = mMeta.GetDataTableNames();

            if (tmpTabs.Count == 1)
            {
                myOut += tmpTabs[0];
            }
            else
            {
                myOut += "(";
                for (int tabCnt = 0; tabCnt < tmpTabs.Count; tabCnt++)
                {
                    if (tabCnt != 0)
                    {
                        myOut += " union all ";
                    }
                    myOut += "  select * from " + tmpTabs[tabCnt] + "\n";
                }
                myOut += ")";
            }
            myOut += " dt \n";
            return myOut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NPMChacterType"></param>
        /// <param name="sqlRow"></param>
        /// <param name="contCode"></param>
        private void addDataNoteCell(string NPMChacterType, DataRow sqlRow, string contCode)
        {
            string cellId = "";
            foreach (string colName in DataNoteCellId_Columns)
            {
                if (String.IsNullOrEmpty(colName))
                {
                    cellId = contCode + "," + cellId;
                }
                else
                {
                    cellId = sqlRow[colName].ToString() + "," + cellId;
                }
            }

            cellId = cellId.TrimEnd(',');


            _DataNoteCellEntries.Add(cellId, NPMChacterType);
            log.Warn(new PCAxis.Sql.Exceptions.WarnNo2Text(1, "addDataNoteCell").getText());
        }

        /// </summary>
        /// <param name="mMeta">PXSqlMeta</param>
        /// <param name="sqlRow">row containg result from datatable and temporary tables</param>
        private void addAttribute(PXSqlMeta_23 mMeta, DataRow sqlRow)
        {
            // keeps det selected value for each dimension to identify the cell.
            string cellId;

            // keeps cellids for cells with same attribute(s)
            StringCollection cellIds;

            //attributevalues for the datarow.
            string attValues = "";


            attValues = sqlRow[attributeResultColumn].ToString();
            foreach (PXSqlContent contvalue in mMeta.Contents.Values)
            {
                cellId = "";
                foreach (string key in mMeta.GetVariableIDsInOutputOrder())
                {
                    {

                        if (!mMeta.Variables[key].IsContentVariable)
                        {
                            cellId += sqlRow[key].ToString() + ",";
                        }
                        else
                        {
                            cellId += contvalue.PresCode + ",";
                        }
                    }
                }

                cellId = cellId.TrimEnd(',');
                if (_AttributesEntries.ContainsKey(attValues))
                {
                    _AttributesEntries[attValues].Add(cellId);
                }
                else
                {
                    cellIds = new StringCollection();
                    cellIds.Add(cellId);
                    _AttributesEntries.Add(attValues, cellIds);
                }
            }
        }

        /// <summary>
        /// Counts attribute combination. Highest i set to default.
        /// </summary>

        private void setDefaultAttributes()
        {
            string defaultAttributes = "";
            int lastAttributeCount = 0;
            foreach (KeyValuePair<string, StringCollection> attributeEntry in _AttributesEntries)
            {
                if (attributeEntry.Value.Count > lastAttributeCount)
                {
                    defaultAttributes = attributeEntry.Key;
                    lastAttributeCount = attributeEntry.Value.Count;
                }
            }
            _DefaultAttributes = defaultAttributes;
        }

        /// <summary>
        /// returns the part of sql retreiving attributes
        /// <param name="attributes">Attributes from metapart</param>
        /// </summary>

        private string getAttrSelectPart(PXSqlAttributes attributes)
        {
            string attrSelectPart = ",";
            string tmpAttributeColumn = "";
            if (attributes.AllAttributesInOneColumn)
            {
                attrSelectPart += attributes.TheOneAndOnlyAttributeColumn + " AS " + attributeResultColumn;
            }
            else
            {
                foreach (AttributeRow attrow in attributes.SortedAttributes.Values)
                {
                    if (attrow.AttributeColumn != tmpAttributeColumn)
                    {
                        attrSelectPart += attrow.AttributeColumn + "||':'||";
                    }
                    tmpAttributeColumn = attrow.AttributeColumn;
                }
                attrSelectPart = attrSelectPart.TrimEnd('|', '|', '\'', ':', '\'', '|', '|') + " AS " + attributeResultColumn;
            }
            return attrSelectPart;
        }

        private void addDataNoteSum(DataRow sqlRow, string contCode)
        {
            //addDataNoteCell(symbols.getDataNoteSumCharacterType(), sqlRow, contCode);
            addDataNoteCell(symbols.DataNoteSumCharacterType(), sqlRow, contCode);
        }


        #region IDisposable implemenatation

        override public void Dispose()
        {
            if (mMeta != null)
            {
                mMeta.Dispose();
            }
        }
        #endregion
    }
}
