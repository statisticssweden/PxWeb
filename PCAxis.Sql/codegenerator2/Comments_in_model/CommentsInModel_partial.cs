using System;
using System.Collections.Generic;
using System.Text;

namespace CommentsInModel
{
    public partial class CommentsInModel 
    {
        public string getTabellBeskrivelse(string tabellnavn)
        {
            String myOut = "";
            foreach (Table nTabell in this.Tables.Table)
            {
                if (nTabell.name == tabellnavn)
                {
                    myOut = nTabell.Description;
                    break;
                }
            }
            return myOut;
        }

        public string getKolonneBeskrivelse(string tabellnavn,string kolonneNavn)
        {
            String myOut = "";
            foreach (Table nTabell in this.Tables.Table)
            {
                if (nTabell.name == tabellnavn)
                {
                    
                    foreach (Column nColumn in nTabell.Columns.Column)
                    {
                        if (nColumn.colname == kolonneNavn)
                        {
                            myOut = nColumn.Description;
                            break;
                        }
                    }


                    break;
                }
            }
            return myOut;
        }
    }
}
