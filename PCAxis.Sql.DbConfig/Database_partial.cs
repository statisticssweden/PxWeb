using System;
using System.Collections.Generic;
using System.Text;

namespace PCAxis.Sql.DbConfig 
{
       public partial class Database
    {
        private LanguageType mainLanguageField;

        public LanguageType MainLanguage
        {

            get
            {
                return this.mainLanguageField;
            }
            set
            {
                this.mainLanguageField = value;
            }
        }


       public void postSerialize(){
            foreach (LanguageType lang in Languages) {
                if (lang.main) {
                    mainLanguageField = lang;
                    break;
                }
            }
    }

}
}