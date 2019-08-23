using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using log4net;

namespace PCAxis.Sql.Pxs {

    public partial class PxsQuery {




        private void ReadFlatFile(String[] fileLines, string resultLanguage) {
            FlatFileReaderHelper helper = new FlatFileReaderHelper();//Stores tmp data
            char[] splitter = new char[1];
            splitter[0] = '=';

            this.init1();

            this.Information.CreatedBy.userId = Environment.UserName;
            this.Information.CreatedBy.UserName = Environment.UserName;


            String segmentFlag = "";

            log.Debug("fileLines count:" + fileLines.Length);
            int processedLines = 0;
            foreach (String aLine in fileLines) {
                if (String.IsNullOrEmpty(aLine)) {
                    continue;
                }
                processedLines++;
                String line = aLine.Trim();
                log.Debug("line='" + line + "'");
                String[] lineParts = line.Split(splitter, 2);
                if (line.StartsWith("[")) {

                    String ucLine = line.Trim().Replace("[", "").Replace("]", "").ToUpper();

                    if (ucLine.Equals("QUERY") || ucLine.Equals("CONTENTS") || ucLine.Equals("TIME")) {
                        segmentFlag = ucLine;
                    } else if (ucLine.Equals("FILES")) {

                        this.Information.Files = new InformationTypeFiles();
                        segmentFlag = ucLine;
                    } else if (ucLine.Equals("OPTIONS")) {
                        this.Information.BatchOptions = new InformationTypeBatchOptions();

                        //this.Information.BatchOptions.ContinuePxq = " ";//denne er kanskje ikke i bruk
                        this.Information.BatchOptions.Sqltest = " ";
                        this.Information.BatchOptions.PxDatabase = " ";
                        this.Information.BatchOptions.Replace = " ";
                        this.Information.BatchOptions.Metabase = " ";
                        segmentFlag = ucLine;

                    } else if (ucLine.StartsWith("VAR")) {
                        String shouldBeInt = ucLine.Substring(3);
                        helper.var_counter = int.Parse(shouldBeInt);
                        helper.tmpVariable[helper.var_counter] = new PQVariable();
                        helper.tmpVariable[helper.var_counter].Values = new PQVariableValues();
                        helper.variableValues[helper.var_counter] = new List<ValueTypeWithGroup>();
                        helper.groupsValuesByValueCode[helper.var_counter] = new Dictionary<string, List<GroupValueType>>();
                        helper.groupTextByValueSortOrder[helper.var_counter] = new Dictionary<int, string>();

                        segmentFlag = "VAR";
                    } else {
                        throw new Exception("Unknown thing in square brackets:" + line);//in production-release:should be just a warning
                    }
                } else if (lineParts.Length == 2) {
                    if (segmentFlag.Equals("QUERY")) {
                        fromQuerySeg(lineParts[0], lineParts[1], helper);
                    } else if (segmentFlag.Equals("CONTENTS")) {
                        fromContentSeg(lineParts[0], lineParts[1], helper);
                    } else if (segmentFlag.Equals("VAR")) {
                        fromVarSeg(lineParts[0], lineParts[1], helper);
                    } else if (segmentFlag.Equals("TIME")) {
                        fromTimeSeg(lineParts[0], lineParts[1], helper);
                    } else if (segmentFlag.Equals("FILES")) {
                        fromFilesSeg(lineParts[0], lineParts[1]);
                    } else if (segmentFlag.Equals("OPTIONS")) {
                        fromOptionsSeg(lineParts[0], lineParts[1]);

                    } else {
                        throw new Exception("Unknown segmentFlag:" + segmentFlag + " line:" + line);//in production-release:should be just a warning
                    }
                } else {
                    throw new Exception("Hmmm: not a segment and not key=value: " + line);//in production-release:should be just a warning
                }
            }
            if (processedLines < 5) {
                throw new ApplicationException("Too few lines to be a pxs");

            }
            postParse(helper, resultLanguage);
        }


        private void fromQuerySeg(String key, String value, FlatFileReaderHelper helper) {
            string ucKey = key.ToUpper();
            if (ucKey.Equals("TIMEOPT")) {
                this.Query.Time.TimeOption = (TimeTypeTimeOption)int.Parse(value);
            } else if (ucKey.Equals("USERID")) {
                this.Information.CreatedBy.userId = value;
            } else if (ucKey.Equals("USERNAME")) {
                this.Information.CreatedBy.UserName = value;
            } else if (ucKey.Equals("METAVERSION")) {
                this.Information.MetaVersion = value;
                log.Warn("assumes METAVERSION means the same as PxsVersion");
            } else if (ucKey.Equals("SUBJECT")) {
                // denne finnes ikke noe subject
                fixMenuSel("OBS_OBS_fra_subject", value, helper);
            } else if (ucKey.Equals("MENUSEL1")) {
                fixMenuSel("1", value, helper);
            } else if (ucKey.Equals("MENUSEL2")) {
                fixMenuSel("2", value, helper);
            } else if (ucKey.Equals("MENUSEL3")) {
                fixMenuSel("3", value, helper);
            } else if (ucKey.Equals("MENUSEL4")) {
                fixMenuSel("4", value, helper);
            } else if (ucKey.Equals("MENUSEL5")) {
                fixMenuSel("5", value, helper);
            } else if (ucKey.Equals("MENUSEL6")) {
                fixMenuSel("6", value, helper);
            } else if (ucKey.Equals("TABLE")) {
                this.Query.TableSource = value;
            } else if (ucKey.Equals("VARANT")) { //number of variables (not including time)
                helper.no_vars = int.Parse(value);

                helper.tmpVariable = new PQVariable[helper.no_vars];
                helper.variableValues = new List<ValueTypeWithGroup>[helper.no_vars];
                helper.groupsValuesByValueCode = new Dictionary<string, List<GroupValueType>>[helper.no_vars];
                helper.groupTextByValueSortOrder = new Dictionary<int, string>[helper.no_vars];

            } else if (ucKey.Equals("SUBTAB")) {
                this.Query.SubTable = value;
            } else if (ucKey.Equals("LANG")) {
                helper.langInFile = value;

            } else if (ucKey.Equals("HEADING")) {//how many variables before heading (those in stub)
                helper.no_vars_before_heading = int.Parse(value);
            } else if (ucKey.Equals("TIMEORDER")) {//how many variables before the time variable (example time first in heading)
                helper.no_vars_before_time = int.Parse(value);
            } else if (ucKey.Equals("CONTENTS")) {
                log.Warn("Keyword CONTENTS will be ignored. So you can no longer override the short title from the DB.");
                //data.Query.Contents.ContText = value;
            } else if (ucKey.Equals("PRODUCT")) {
                log.Info("Keyword PRODUCT found and will be ignored.");
            } else {
                throw new Exception("Unknown key in query:" + key);//in production-release:should be just a warning
            }
        }


        private void fromContentSeg(String key, String value, FlatFileReaderHelper helper) {
            string ucKey = key.ToUpper();
            if (!ucKey.StartsWith("CONT")) {
                throw new Exception("Unknown key in content-segment:" + key);//in production-release:should be just a warning
            }
            String shouldBeInt = key.Substring(4);//Cont1=...
            BasicValueType tmp = new BasicValueType(value, int.Parse(shouldBeInt));

            helper.contentValues.Add(tmp);
        }


        private void fromVarSeg(String key, String value, FlatFileReaderHelper helper) {
            string ucKey = key.ToUpper();
            if (ucKey.Equals("KOD")) {
                helper.tmpVariable[helper.var_counter].code = value;
            } else if (ucKey.Equals("PRESTEXT")) {
                helper.tmpVariable[helper.var_counter].PresTextOption = value;
            } else if (ucKey.Equals("ELIM")) {
                log.Info("Warning: keyword Elim will be ignored. Read: " + key + " value:" + value);
            } else if (ucKey.Equals("AGGREG")) {
                switch (value.ToUpper()) {
                    case "G":
                        helper.tmpVariable[helper.var_counter].Aggregation = PQVariableAggregation.G;
                        break;
                    case "YES":
                        helper.tmpVariable[helper.var_counter].Aggregation = PQVariableAggregation.G;
                        break;
                    case "H":
                        helper.tmpVariable[helper.var_counter].Aggregation = PQVariableAggregation.H;
                        break;
                    default:
                        helper.tmpVariable[helper.var_counter].Aggregation = PQVariableAggregation.N;
                        break;
                }
            } else if (ucKey.Equals("GROUPING")) {
                helper.tmpVariable[helper.var_counter].StructureId = value;
                helper.tmpVariable[helper.var_counter].Aggregation = PQVariableAggregation.G;
            } else if (ucKey.Equals("GENERAL")) {
                if ((value.ToUpper().StartsWith("Y")) && (helper.tmpVariable[helper.var_counter].StructureId == null)) {
                    helper.tmpVariable[helper.var_counter].StructureId = "UNKNOWNSTRUCTUREID";
                }
            } else if (ucKey.StartsWith("VALUE")) {
                if (ucKey.EndsWith("TEXT")) {
                    //valueNNtext   (belongs to valueNN)
                    String NN = ucKey.Replace("VALUE", "").Replace("TEXT", "");
                    helper.groupTextByValueSortOrder[helper.var_counter][int.Parse(NN)] = value;
                } else {
                    //VALUEnn
                    ValueTypeWithGroup tmp = new ValueTypeWithGroup(value, int.Parse(ucKey.Substring(5)));
                    helper.variableValues[helper.var_counter].Add(tmp);
                }
            } else if (ucKey.Equals("GEOAREA")) {
                log.Info("GEOAREA will hereafter be retrieved from DB. Is no longer part of query.");
            } else if (ucKey.StartsWith("GRVALUE")) {

                int grvalue_number = int.Parse(ucKey.Substring(7));
                String[] valueSplit = value.Split(',');
                String codeOfAtom = valueSplit[0].Replace("'", "");
                String codeOfMolecule = valueSplit[1].Replace("'", "");

                if (!helper.groupsValuesByValueCode[helper.var_counter].ContainsKey(codeOfMolecule)) {
                    helper.groupsValuesByValueCode[helper.var_counter][codeOfMolecule] = new List<GroupValueType>();
                }
                GroupValueType tmp = new GroupValueType();
                tmp.code = codeOfAtom;
                helper.groupsValuesByValueCode[helper.var_counter][codeOfMolecule].Add(tmp);

            } else {
                throw new Exception("Unknown keyword in var-segment:" + key + " (value=" + value + ")");//in production-release:should be just a warning
            }
        }


        private void fromTimeSeg(String key, String value, FlatFileReaderHelper helper) {
            string ucKey = key.ToUpper();
            if (ucKey.Equals("KOD")) {
                this.Query.Time.code = value;
            } else if (ucKey.Equals("VALUES")) {  // when Moving time interval
                this.Query.Time.Item = int.Parse(value);
            } else if (ucKey.StartsWith("VALUE")) {  // 0 or 3
                if ((int)this.Query.Time.TimeOption == 3) {
                    this.Query.Time.Item = value;
                } else {

                    BasicValueType tmp = new BasicValueType(value, int.Parse(ucKey.Substring(5)));
                    helper.timeValues.Add(tmp);
                }
            } else if (ucKey.Equals("TIMEVAL")) {
                this.Query.Time.TimeVal = value;
            } else {
                throw new Exception("Unknown keyword in time-segment:" + key + " (value=" + value + ")");//in production-release:should be just a warning
            }
        }


        private void fromFilesSeg(String key, String value) {
            string ucKey = key.ToUpper();

            if (ucKey.Equals("PXSFILE")) {
                this.Information.Files.Pxsfile = value;
            } else if (ucKey.Equals("OUTFILE")) {
                this.Information.Files.Outfile = value;
            } else if (ucKey.Equals("LOGFILE")) {
                this.Information.Files.Logfile = value;
            } else if (ucKey.Equals("TEXTFILE")) {
                this.Information.Files.Textfile = value;
            } else if (ucKey.Equals("SQLFILE")) {
                this.Information.Files.Sqlfile = value;
            } else {
                throw new Exception("Unknown keyword in files-segment:" + key + " (value=" + value + ")");//in production-release:should be just a warning
            }
        }


        private void fromOptionsSeg(String key, String value) {
            string ucKey = key.ToUpper();
            if (ucKey.Equals("SQLTEST")) {
                this.Information.BatchOptions.Sqltest = value;
            } else if (ucKey.Equals("PXDATABASE")) {
                this.Information.BatchOptions.PxDatabase = value;
            } else if (ucKey.Equals("REPLACE")) {
                this.Information.BatchOptions.Replace = value;
            } else if (ucKey.Equals("METABASE")) {
                this.Information.BatchOptions.Metabase = value;
            } else {
                throw new Exception("Unknown keyword in options-segment:" + key + " (value=" + value + ")");//in production-release:should be just a warning
            }
        }


        // call this when there are no more segments
        // it converts lists to arrays and adds them to the object
        // (The lists are used when parsing since arrays cannot grow dynamically) 
        //recodes data.Query.Time.TimeOption "1" to "2" 
        private void postParse(FlatFileReaderHelper helper, string resultLanguage) {
            bool[] isElim = new bool[helper.no_vars];
            int elimCount = 0;

            // Traverses the variables one by one
            for (int n = 0; n < helper.no_vars; n++) {
                isElim[n] = helper.variableValues[n].Count < 1; // Common to all elimination: No values!
                if (isElim[n]) {
                    elimCount++;
                }
                log.Debug("isElim=" + isElim[n] + " code" + helper.tmpVariable[n].code);
                //adds any group values

                // For each value vt belonging to the current variable n do ....
                foreach (ValueTypeWithGroup vt in helper.variableValues[n]) {

                    // Checks if the current value code vt.code has a list of group codes associated with it
                    if (helper.groupsValuesByValueCode[n].ContainsKey(vt.code)) {

                        // If so, then create a temporary GroupValueType tmp to hold the list of group codes
                        List<GroupValueType> tmp = helper.groupsValuesByValueCode[n][vt.code];

                        // Creates an object of type ValueTypeWithGroupGroup and transfers the list of group codes to it
                        vt.Group = new ValueTypeWithGroupGroup();
                        vt.Group.GroupValue = tmp.ToArray();
                        // Checks (on the sort order) if there is a ValueText (Group text) attached to any of the values
                        if (helper.groupTextByValueSortOrder[n].ContainsKey(vt.sortOrder)) {
                            // text er bare for grupper som kommer fra filer, hva med språk?
                            StringLangType[] tmpSLT = new StringLangType[1];
                            tmpSLT[0] = new StringLangType();
                            tmpSLT[0].Value = helper.groupTextByValueSortOrder[n][vt.sortOrder];
                            if (helper.langInFile == null) {
                                tmpSLT[0].lang = resultLanguage;
                                log.Warn("Found text but no language. Using " + resultLanguage);
                            } else {
                                tmpSLT[0].lang = helper.langInFile;
                            }
                            vt.Group.GroupText = tmpSLT;
                        }
                    }
                }

                // helper.tmpVariable[n].Values is an array of pointers to ValueTypeWithGroup,
                // which means that it is similar to helper.variableValues (which is a list)
                helper.tmpVariable[n].Values.Items = helper.variableValues[n].ToArray();
            }

            if ((int)this.Query.Time.TimeOption == 0) {
                TimeTypeTimeValues tttv = new TimeTypeTimeValues();
                tttv.TimeValue = helper.timeValues.ToArray();
                this.Query.Time.Item = tttv;
            } else if ((int)this.Query.Time.TimeOption == 1) {
                this.Query.Time.TimeOption = (TimeTypeTimeOption)2;
                int noOfValues = 1;
                this.Query.Time.Item = noOfValues;
            }

            this.Query.Contents.Content = helper.contentValues.ToArray();

            if (helper.menuSelList.Count > 0) {
                this.Information.Menu = helper.menuSelList.ToArray();
            }

            #region set Language

            myLanguageType[] tmpMyLang = new myLanguageType[1];
            tmpMyLang[0] = new myLanguageType();

            if (helper.langInFile == null) {
                tmpMyLang[0].Value = resultLanguage;
                log.Warn(" no language. Using " + resultLanguage);
            } else {
                tmpMyLang[0].Value = helper.langInFile;
            }

            this.Query.Languages.Items = tmpMyLang;

            #endregion set Language

            // Allocating an array of PQVariable under QueryType.Query.Variables
            this.Query.Variables = new PQVariable[helper.no_vars - elimCount];
            //hva når det er 0

            //All [] helpers has entries for the eliminated. These must be skipped.



            #region fix stub heading
            // Hva med de som de eliminert??

            int stub_counter = 0;  //-1
            int head_counter = 0;  //-1
            List<AxisType> headList = new List<AxisType>();
            List<AxisType> stubList = new List<AxisType>();
            int outVarCounter = 0;

            // Traverses the variables found in the pxs file
            #region for loop
            for (int n = 1; n <= helper.no_vars; n++) {
                if (isElim[n - 1]) {
                    continue;  // If the variable is eliminated, then skip it
                }
                this.Query.Variables[outVarCounter] = helper.tmpVariable[n - 1];
                outVarCounter++;



                string code = helper.tmpVariable[n - 1].code;
                // helper.no_vars_before_heading show
                if (n <= helper.no_vars_before_heading) {
                    // stub_counter++;
                    stubList.Add(new AxisType(code, stub_counter++));
                } else {
                    // head_counter++;
                    headList.Add(new AxisType(code, head_counter++));
                }


                if (n == helper.no_vars_before_time) {  //Time
                    string timeCode = this.Query.Time.code;
                    if (n < helper.no_vars_before_heading) {
                        stubList.Add(new AxisType(timeCode, stub_counter++));
                    } else {
                        headList.Add(new AxisType(timeCode, head_counter++));
                    }
                }
            }
            #endregion for loop





            headList.Add(new AxisType(this.Query.Contents.code, head_counter++));


            this.Presentation = new PresentationType();
            if (stubList.Count > 0) {
                this.Presentation.Stub = stubList.ToArray();
            }
            if (headList.Count > 0) {
                this.Presentation.Heading = headList.ToArray();
            }

            #endregion fix stub heading

        }


        private void fixMenuSel(String level, String value, FlatFileReaderHelper helper) {
            MenuSelType tmp = new MenuSelType();
            tmp.level = level;
            tmp.Value = value;
            helper.menuSelList.Add(tmp);
        }
    }
}




