using System;
using System.Collections.Generic;
using System.Text;


namespace PCAxis.Sql.Exceptions 
{
    /// <summary>
    /// Converts warning number and any arguments to a text. The language of the text is
    /// </summary>
    public class WarnNo2Text {
        string outString = "";
        string formatString = "";
        


        public WarnNo2Text(int warnno) {
            outString = lookupFormatstring(warnno);
        }

        public WarnNo2Text(int warnno, Object arg0) {
            formatString = lookupFormatstring(warnno);
            outString = String.Format(formatString, arg0);
        }

        public WarnNo2Text(int warnno, Object arg0, Object arg1) {
            formatString = lookupFormatstring(warnno);
            outString = String.Format(formatString, arg0, arg1);
        }

        public WarnNo2Text(int warnno, Object arg0, Object arg1, Object arg2) {
            formatString = lookupFormatstring(warnno);
            outString = String.Format(formatString, arg0, arg1, arg2);
        }

        private string lookupFormatstring(int warnno){
            string myOut="";

            //log.Debug("dllen er på: " + System.Reflection.Assembly.GetAssembly(this.GetType()).Location);
            //denne må åpne filer og herje, men kan ikke lage feil
            Dictionary<int, string> formatStringsByWarnNo = new Dictionary<int, string>();
            formatStringsByWarnNo.Add(1,"The method or operation is not fully implemented: {0}");
         
           

            if(formatStringsByWarnNo.ContainsKey(warnno)){
                myOut = formatStringsByWarnNo[warnno];
            } else {
                myOut = "Warning " + warnno.ToString()+" has occured. This warning has no text.";
            }
            return myOut;
        }

        public string getText() {
            return outString;
        }

    }
}
