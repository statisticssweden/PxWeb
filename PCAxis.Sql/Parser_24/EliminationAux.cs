using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Sql.Parser_24
{
    public class EliminationAux
    {

        public string EliminationMethod { get; private set; }
        public string EliminationCode { get; private set; }

        private DbConfig.SqlDbConfig_24.Ccodes ccodes;

        private string DBCodeForBySpecificValuecode()
        {
            return ccodes.EliminationC;
        }
        private string DBCodeForByAggregation()
        {
            return ccodes.EliminationA;
        }

        private string DBCodeForByNotAllowed()
        {
            return ccodes.EliminationN;
        }

       

        public EliminationAux(string eliminationMethod, string eliminationCode, DbConfig.SqlDbConfig_24.Ccodes ccodes)
        {
            this.EliminationMethod = eliminationMethod;
            this.EliminationCode = eliminationCode;
            this.ccodes = ccodes;

            if (String.IsNullOrEmpty(this.EliminationMethod))
            {
                this.EliminationMethod = this.DBCodeForByNotAllowed();
            }
        }


        public bool IsNotAllowed()
        {
            return this.EliminationMethod == ccodes.EliminationN;
        }


        public string GetClassicEliminationValue()
        {
            if (EliminationMethod == this.DBCodeForBySpecificValuecode()) return EliminationCode;
            return EliminationMethod;
        }

        public static EliminationAux MergeEliminations(List<EliminationAux> elims)
        {
            if (elims.Count == 1)
            {
                return elims[0];
            }



            int numberOfElimA = 0;
            int numberOfElimN = 0;
            int numberOfElimOtherCode = 0;
            EliminationAux elimByCode = elims[0];

            foreach (EliminationAux elimval in elims)
            {
                if (elimval.EliminationMethod == elimval.DBCodeForByAggregation())
                {
                    numberOfElimA++;
                }
                else if (elimval.EliminationMethod == elimval.DBCodeForByNotAllowed())  
                {
                    numberOfElimN++;
                }
                else
                {
                    numberOfElimOtherCode++;
                    elimByCode = elimval;
                }
            }

            //jfi sep 2015. This:
            //if ((numberOfElimA == ElimValues.Count - 1) && (numberOfElimOtherCode == 1))
            //was found to be too strict in VariablerOgVerdimengder-4.doc

            if ((numberOfElimOtherCode == 1))
            {
                return elimByCode;
            }
            else
            {
                return new EliminationAux(null,null,elims[0].ccodes);
            }
            
        }

       
    }
}