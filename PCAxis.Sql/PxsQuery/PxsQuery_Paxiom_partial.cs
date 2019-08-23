using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using log4net;
using PCAxis.Paxiom;
using System.Collections.Specialized;
using PCAxis.PlugIn.Sql;

namespace PCAxis.Sql.Pxs {

 

    public partial class PxsQuery {

 
        /// <summary>
        /// For creation after "build for selection GUI" 
        /// </summary>
        /// <param name="pax"></param>
        /// <param name="langs"></param>
        /// <param name="selections"></param>
        /// <param name="pxsqlMeta"></param>
        public PxsQuery(PCAxis.Paxiom.PXMeta pax,StringCollection langs, PCAxis.Paxiom.Selection[] selections, InfoFromPxSqlMeta2PxsQuery pxsqlMeta) {

            init1();

            
            #region this.Information
            //this.Information.CreatedDate = DateTime.Now;
            //skal bruke pax.Meta.CreationDate men hva er formatet til denne stringen?
            #endregion this.Information


            #region this.Query

            List<string> theLangs = new List<string>();
            foreach (string aLang in langs) {
                theLangs.Add(aLang);
            }


            this.setLangs(theLangs);

            //this.Query.TableSource = pax.TableID; // eller Matrix
            this.Query.TableSource = pax.MainTable;// changed. reqtest bug 325/339
 

           
           

            List<PQVariable> tmpPxsVars = new List<PQVariable>();


            foreach (Variable var in pax.Variables) {
                Selection correspondingSelection = null;
                foreach (Selection sel in selections) {
                    if (sel.VariableCode.Equals(var.Code)) {
                        correspondingSelection = sel;
                    }
                }




                if (var.IsContentVariable || var.Code.Equals("CONTENTSCODE") || var.IsTime || var.Code.Equals("TID") || var.Code.Equals("Tid")) {
                    List<BasicValueType> tmpList = new List<BasicValueType>();
                    
                    int sortOrder = 1;
                    
                    foreach (string valCode in correspondingSelection.ValueCodes) {
                        if (var.IsContentVariable || var.Code.Equals("CONTENTSCODE")) // new piv
                        {
                            string contents = var.Values.GetByCode(valCode).ContentInfo.RefrenceID;
                            tmpList.Add(new BasicValueType(contents, sortOrder++));
                        }
                        else
                        {
                            tmpList.Add(new BasicValueType(valCode, sortOrder++));
                        }
                    }

                    if (var.IsContentVariable || var.Code.Equals("CONTENTSCODE")) {
                        this.Query.Contents.code = var.Code;
                        //
                        
                        //
                        this.Query.Contents.Content = tmpList.ToArray();
                    } else {
                        //pax vet bare nok til å lage timeoption 0 :-< 
                        #region this.Query.Time
                        this.Query.Time.code = var.Code;
                        this.Query.Time.TimeOption = TimeTypeTimeOption.Item0;

                        TimeTypeTimeValues tttv = new TimeTypeTimeValues();
                        tttv.TimeValue = tmpList.ToArray();
                        this.Query.Time.Item = tttv;

                        this.Query.Time.TimeVal = var.TimeValue;//??
                        #endregion this.Query.Time
                    }
                } else { //classification
                    #region preparing for this.Query.Variables
                    PQVariable tmpPxsVar = new PQVariable();
                    tmpPxsVar.code = var.Code;
                    
                    //på tynn is her:

                    tmpPxsVar.Aggregation = PQVariableAggregation.N;

                    string selectedValueset = pxsqlMeta.GetSelectedValuesetId(var.Code);


                    string currentGrouping = pxsqlMeta.GetCurrentGroupingId(var.Code);

                  
                    //
                    // string currentGrouping = var.CurrentGrouping; will get trouble for pxs4selection
                    // with a grouping, since it is not an "applied grouping", but more of a "backgroud grouping"
                    

                    if (!String.IsNullOrEmpty(currentGrouping))
                    {
                        tmpPxsVar.Aggregation = PQVariableAggregation.G;
                        tmpPxsVar.StructureId = currentGrouping;
                        log.Debug(currentGrouping);
                    //} else if ( (! String.IsNullOrEmpty(selectedValueset)) &&
                    //    selectedValueset != PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS) {
                    //        tmpPxsVar.StructureId = pxsqlMeta.Variables[var.Code].SelectedValueset;
                    //        tmpPxsVar.Aggregation = PQVariableAggregation.V;
                    //    // denne skulle vel bort?
                        
                    }
                    //skip variables which does not have codes in Selection object  
                    if (correspondingSelection == null || correspondingSelection.ValueCodes.Count == 0)
                    {
                        if (pxsqlMeta.GetSelectedValuesetId(var.Code) == null || (pxsqlMeta.GetSelectedValuesetId(var.Code) == PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS && String.IsNullOrEmpty(currentGrouping))) // added to save info about selected valueset.
                            continue;
                    }

                    //Hmm: hva skal gjelde: paxiom eller pxs? 
                    //Hmm2: ved endring av kode/text i grensesntitt, bør det kjøres en applyCodeText noe som fører til at neste linje kan slettes og opsjon fra paxiom brukes
                    //tmpPxsVar.PresTextOption = "Both";


                    tmpPxsVar.Values = new PQVariableValues();
                    tmpPxsVar.Values.Items = new ValueTypeWithGroup[correspondingSelection.ValueCodes.Count];

                    for (int n = 0; n < tmpPxsVar.Values.Items.Length; n++) {
                        tmpPxsVar.Values.Items[n] = new ValueTypeWithGroup(correspondingSelection.ValueCodes[n], n + 1);

                    }

                    tmpPxsVar.SelectedValueset = pxsqlMeta.GetSelectedValuesetId(var.Code); // To fix problem with missing valueset info when more than two variabeles with multiple valuesets. e.g no subtable is set.
                    tmpPxsVars.Add(tmpPxsVar);
                    #endregion preparing for this.Query.Variables
                }
            }

            this.Query.Variables = tmpPxsVars.ToArray();
            #endregion this.Query

            #region this.Presentation
            this.Presentation = new PresentationType();
            if (pax.Stub.Count > 0) {
                this.Presentation.Stub = new AxisType[pax.Stub.Count];
                for (int n = 0; n < pax.Stub.Count; n++) {
                    this.Presentation.Stub[n] = new AxisType(pax.Stub[n].Code, n);
                }
            }

            if (pax.Heading.Count > 0) {
                this.Presentation.Heading = new AxisType[pax.Heading.Count];
                for (int n = 0; n < pax.Heading.Count; n++) {
                    this.Presentation.Heading[n] = new AxisType(pax.Heading[n].Code, n);
                }
            }

            #endregion this.Presentation

        }



        /// <summary>
        /// Sets the SelectedValueset 's  for Query.Variables[XXX]. And sets PxsObject.Query.SubTable =""
        /// SubTable is the old way, SelectedValueset is the new.
        /// </summary>
        /// <param name="mValuesetIdByVariableIdFromDB"></param>
        public void SetSelectedValuesetsAndNullSubtable(Dictionary<string, string> mValuesetIdByVariableIdFromDB)
        {
            foreach (string variableId in mValuesetIdByVariableIdFromDB.Keys)
            {
                bool found = false;
                foreach (PQVariable pqv in this.Query.Variables)
                {
                    if (pqv.code.Equals(variableId))
                    {
                        found = true;
                        pqv.SelectedValueset = mValuesetIdByVariableIdFromDB[variableId];
                        break;
                    }
                }
                if (!found)
                {
                    log.Info("variableId=" + variableId+ " cant be found in the pxs, so it is hopefully eliminated!");
                }

            }
            this.Query.SubTable = "";
        }
    }
}




