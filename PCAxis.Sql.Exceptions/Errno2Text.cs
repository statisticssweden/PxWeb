using System;
using System.Collections.Generic;
using System.Text;


namespace PCAxis.Sql.Exceptions 
{
    /// <summary>
    /// Converts error number and any arguments to a text. The language of the text is 
    /// </summary>
    public class ErrNo2Text {
        string outString = "";
        string formatString = "";
        


        public ErrNo2Text(int errno) {
            outString = lookupFormatstring(errno);
        }

        public ErrNo2Text(int errno, Object arg0) {
            formatString = lookupFormatstring(errno);
            outString = String.Format(formatString, arg0);
        }

        public ErrNo2Text(int errno, Object arg0, Object arg1) {
            formatString = lookupFormatstring(errno);
            outString = String.Format(formatString, arg0, arg1);
        }

        public ErrNo2Text(int errno, Object arg0, Object arg1, Object arg2) {
            formatString = lookupFormatstring(errno);
            outString = String.Format(formatString, arg0, arg1, arg2);
        }

        public ErrNo2Text(int errno, Object arg0, Object arg1, Object arg2, Object arg3)
        {
            formatString = lookupFormatstring(errno);
            outString = String.Format(formatString, arg0, arg1, arg2, arg3);
        }


        private string lookupFormatstring(int errno){
            string myOut="";

            //log.Debug("dllen er på: " + System.Reflection.Assembly.GetAssembly(this.GetType()).Location);
            //denne må åpne filer og herje, men kan ikke lage feil
            Dictionary<int, string> formatStringsByErrNo = new Dictionary<int, string>();
            formatStringsByErrNo.Add(1,"Not one and only one row for MainTable = {0} Contents = {1}");

            formatStringsByErrNo.Add(2, "Non-eliminated variable with no ValueCodes defined. Variablename={0}");
            formatStringsByErrNo.Add(3, "No Datasymbolnumber found for the specified symbolnumber: {0}");
            formatStringsByErrNo.Add(4, "No Datasymbolnumber found for the specified charactertype: {0}");
            formatStringsByErrNo.Add(5, "The database does not support any of the languages requested by the pxs");
            formatStringsByErrNo.Add(6, "Specified subtable (= {0} ) does not exists");
            formatStringsByErrNo.Add(7, "Variable specified in PXS(code= {0} ) does not exist in database");
            
             formatStringsByErrNo.Add(8, "You have to specify timevariable in PXS");
             formatStringsByErrNo.Add(9, "The specified timevariable in PXS cannot be found in the list of variables");
             formatStringsByErrNo.Add(10, "Time variable specified in PXS is not a timevariable");

             formatStringsByErrNo.Add(11, "You have to select at least 1 value for {0}");
             formatStringsByErrNo.Add(12, "Timeperiod for footnote is not selected");
             formatStringsByErrNo.Add(13, "Contents value is not selected");
             formatStringsByErrNo.Add(14, "Specified Meta model {0} version is not supported");
             formatStringsByErrNo.Add(15, "It's not possible to specify more than one Special character with " +
                            "DatacellPres set to NO and AggreggPossible set to Yes");

             formatStringsByErrNo.Add(16, "More than 6 Npm character with DatacellPres=Yes and AggreggPossible=Yes defined");
             formatStringsByErrNo.Add(17, "Invalid combination of DatacellPres and AggreggPossible");

             formatStringsByErrNo.Add(18, "The MetaAdm proprety {0} has value {1}, but SpecialCharacter table has no such row");
             formatStringsByErrNo.Add(19, "No datasymbols available for DataSymbolNil variable in MetaAdm"); //m.i.a. ?
             formatStringsByErrNo.Add(20, "The SpecialCharacter row for {0} (Primary key = {1}) is category {2}, but it shold be {3}.");

             formatStringsByErrNo.Add(21, "No datasymbols available for DataNotAvailable variable in MetaAdm");
             formatStringsByErrNo.Add(22, "No matching entry for {0} found in SpecialCharacter. (Was looking for DataNotAvailable).");
             formatStringsByErrNo.Add(23, "No matching entry for {0} found in SpecialCharacter. (Was looking for DataNoteSum) ");
             formatStringsByErrNo.Add(24, "No datasymbols available for DataSymbolSum (Det sto DataNotAvailable, men det må vel være feil) varaible in MetaAdm");
             formatStringsByErrNo.Add(25, "No matching entry for {0} found in SpecialCharacter . (Was looking for DataSymbolSum");
             formatStringsByErrNo.Add(26, "More than 6 Npm character with DatacellPres=Yes and AggreggPossible=Yes defined");
             formatStringsByErrNo.Add(27, "DefaultCodeMissingLine property not set");
             formatStringsByErrNo.Add(28, "DataNotAvailable property not set");
             formatStringsByErrNo.Add(29, "DataSymbolNil property not set");
             formatStringsByErrNo.Add(30, "DataSymbolSum property not set");

             formatStringsByErrNo.Add(31, "DataSymbol for characterType {0} not set");
             formatStringsByErrNo.Add(32, "DataSymbol{0} property not set");
             formatStringsByErrNo.Add(33, "DataNoteSum property not set");
             formatStringsByErrNo.Add(34, "DataSymbol for magic number {0} not set");
             formatStringsByErrNo.Add(35, "No rows for {0}.");

             formatStringsByErrNo.Add(36, "Not one and only one row for {0}.");
             formatStringsByErrNo.Add(37, "Parameter \"connectionString\" is empty/null!");
             formatStringsByErrNo.Add(38, "Parameter \"dataProvider\" is empty/null!");
             formatStringsByErrNo.Add(39, "DataProvider  must be one of {0}. not:{1}");

             formatStringsByErrNo.Add(40, "SqlDbConfig error, \"=USER;\" is present so \"=PASSWORD;\" must be, but it is missing. Add \"=PASSWORD;\" or remove \"=USER;\" . Configstring:\n{0}");
             formatStringsByErrNo.Add(41, "No language with code = {0} ");
             //formatStringsByErrNo.Add(42, "No rows found in MetabaseInfo.");
             //formatStringsByErrNo.Add(43, "No rows found in SpecialCharacter.");
             formatStringsByErrNo.Add(44, "The table (or view) with modelname {0} and local name {0} appears to be empty.");
             formatStringsByErrNo.Add(45, "Can't find dll configfile:{0}");
             formatStringsByErrNo.Add(46, "Can't find key=\"{0}\" in appsection of dll configfile: {1}");
             formatStringsByErrNo.Add(47, "The database does not support the requested language (=\"{0}\"). The valid choises are:{1}.");

             formatStringsByErrNo.Add(48, "Multiple Valueset marked as Default for maintable {0} and variable {1}.");
             formatStringsByErrNo.Add(49, "Multiple sortorder for maintable {0}, variable {1} and valueset {2}.");

             formatStringsByErrNo.Add(500, "For database id = {1}. Missing Code \"element\" for {0} in SqlDbConfig. Please add a Code element with codeName {0} and an codeValue to the Codes element for the database.");
             formatStringsByErrNo.Add(501, "For database id = {1} and Code element with codeName attribute {0} in SqlDbConfig. Missing  attribute {2} on that element. Is the sPeLliNg OK?");
            
             formatStringsByErrNo.Add(503, "For database id = {1}. Missing Keyword element for {0} in SqlDbConfig. Please add a Keyword element with modelName {0} and an keywordName to the Keywords element for the database.");
             formatStringsByErrNo.Add(504, "For database id = {1} and Keyword element with modelname {0} in SqlDbConfig. Missing  attribute {2} on that element. Is the sPeLliNg OK?");

             formatStringsByErrNo.Add(510, "For the database with id = {0}. Missing Table-element with modelName {1} in SqlDbConfig.");
             formatStringsByErrNo.Add(511, "For the database with id = {0}. Missing column-element with modelName {2} in the table-element with modelName {1} in SqlDbConfig.");
             formatStringsByErrNo.Add(512, "For the database with id = {0}. Missing attribute {3} in the column-element with modelName {2} in the table-element with modelName {1} in SqlDbConfig.");
            //Bugs: 
             formatStringsByErrNo.Add(10000, "Both Type to be compared must be of the same type");

            if(formatStringsByErrNo.ContainsKey(errno)){
                myOut = formatStringsByErrNo[errno];
            } else {
                myOut = "Error with internal errorcode = " + errno.ToString()+" has occured. This error has no text.";
            }
            return myOut;
        }

        public string getText() {
            return outString;
        }

    }
}
