using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PCAxis.Paxiom;

namespace PCAxis.Query
{
    public class TableQuery
    {
        [JsonProperty("query")]
        public Query[] Query { get; set; }

        [JsonProperty("response")]
        public QueryResponse Response { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TableQuery()
        {
        }

        /// <summary>
        /// Constructor
        /// Creates a TableQuery from the Paxiom model object
        /// </summary>
        /// <param name="model">Paxiom model</param>
        /// <param name="selection">Limit the Paxiom model to the specified selection</param>
        public TableQuery(PXModel model, Selection[] selections)
        {
            List<Query> queryLst = new List<Query>();

            foreach (Variable var in model.Meta.Variables)
            {
                Selection sel = selections.SingleOrDefault(x => x.VariableCode == var.Code);
                Query query = new Query();

                query.Code = sel.VariableCode;
               
                // If all values are selected it can be skipped
                if ((var.Values.Count == sel.ValueCodes.Count) && (!var.Elimination) && var.CurrentGrouping == null && var.CurrentValueSet == null)
                {
                    continue;
                }

                if (sel.ValueCodes.Count == 0 && var.CurrentGrouping == null && var.CurrentValueSet == null)
                {
                    continue;
                }

                query.Selection = new QuerySelection();

                if (var.CurrentGrouping != null)
                {
                    string aggType = QueryHelper.GetAggregationTypeFilterString(var.CurrentGrouping.GroupPres);

                    //switch (var.CurrentGrouping.GroupPres)
                    //{
                    //    case GroupingIncludesType.AggregatedValues:
                    //        aggType = "agg:";
                    //        break;
                    //    case GroupingIncludesType.SingleValues:
                    //        aggType = "agg_single:";
                    //        break;
                    //    case GroupingIncludesType.All:
                    //        aggType = "agg_all:";
                    //        break;
                    //    default:
                    //        aggType = "agg:";
                    //        break;
                    //}
                    
                    if (var.CurrentGrouping.ID != null)
                    {
                        query.Selection.Filter = aggType + var.CurrentGrouping.ID;
                    }
                    else
                    {
                        query.Selection.Filter = aggType + var.CurrentGrouping.Name;
                    }
                }
                else if (var.CurrentValueSet != null)
                {
                    query.Selection.Filter = "vs:" + var.CurrentValueSet.ID;
                }
                else
                {
                    query.Selection.Filter = "item";
                }

                query.Selection.Values = new string[sel.ValueCodes.Count];
                sel.ValueCodes.CopyTo(query.Selection.Values,0);

                queryLst.Add(query);
            }

            this.Query = queryLst.ToArray();

            // PX-file is the default response format
            this.Response = new QueryResponse();
            this.Response.Format = "px";
        }
        public TableQuery CreateCopy()
        {
            TableQuery newObject;
            newObject = (TableQuery)this.MemberwiseClone();

            newObject.Query = new Query[this.Query.Length];
            for (int i = 0; i < this.Query.Length; i++)
            {
                if(this.Query[i] != null)
                {
                    newObject.Query[i] = this.Query[i].CreateCopy();
                }
            }

            newObject.Response = this.Response.CreateCopy();
        
            return newObject;
        }
    }


    public class Query
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Lägg till variabltyp i JSON formatet T(ime)/C(ontents)/G(eograpical)/N(ormal)
        /// </summary>
        [JsonProperty("variableType")]
        public string VariableType { get; set; }

        [JsonProperty("selection")]
        public QuerySelection Selection { get; set; }

        public Query CreateCopy()
        {
            Query newObject;
            newObject = (Query)this.MemberwiseClone();

            newObject.Selection = this.Selection.CreateCopy();

            return newObject;
        }
    }

        public class QuerySelection
    {
        [JsonProperty("filter")]
        public string Filter { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }

        public QuerySelection CreateCopy()
        {
            QuerySelection newObject;
            newObject = (QuerySelection)this.MemberwiseClone();

            newObject.Values = new string[this.Values.Length];

            for (int i = 0; i < this.Values.Length; i++)
            {
                if (this.Values[i] != null)
                {
                    newObject.Values[i] = this.Values[i];
                }
            }
            return newObject;
        }
    }

    public class QueryResponse
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("params")]
        public QueryParam[] Params { get; set; }

        public QueryParam GetParam(string key)
        {
            if (Params != null)
                return Params.SingleOrDefault(p => p.Key.ToLower() == key);
            return null;
        }

        public string GetParamString(string key)
        {
            QueryParam param = GetParam(key);
            if (param != null)
                return param.Value;
            return null;
        }

        public int? GetParamInt(string key)
        {
            int iOut;
            if (int.TryParse(GetParamString(key), out iOut))
                return iOut;
            return null;
        }
        public QueryResponse CreateCopy()
        {
            QueryResponse newObject;
            newObject = (QueryResponse)this.MemberwiseClone();

            if (this.Params != null)
            {
                newObject.Params = new QueryParam[this.Params.Length];

                for (int i = 0; i < this.Params.Length; i++)
                {
                    if (this.Params[i] != null)
                    {
                        newObject.Params[i] = this.Params[i].CreateCopy();
                    }
                }
            }
            return newObject;
        }
    }

    public class QueryParam{
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public QueryParam CreateCopy()
        {
            QueryParam newObject;
            newObject = (QueryParam)this.MemberwiseClone();

            return newObject;
        }
    }

}
