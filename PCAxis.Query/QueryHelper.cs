using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;

namespace PCAxis.Query
{
    /// <summary>
    /// Query helper functions
    /// </summary>
    public class QueryHelper
    {
        /// <summary>
        /// Checks if the filter describes an aggregation
        /// </summary>
        /// <param name="filter">selection filter</param>
        /// <returns>True if it is an aggregation, else false</returns>
        public static bool IsAggregation(string filter)
        {
            if ((filter.StartsWith("agg:", StringComparison.InvariantCultureIgnoreCase)) || (filter.StartsWith("agg_", StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to parse the aggregation name and type from the filter string
        /// </summary>
        /// <param name="filter">Filter string</param>
        /// <param name="aggregation">Aggregation name</param>
        /// <param name="aggregationType">Aggregation type</param>
        /// <returns>True if aggregation name and type could be parsed, else false</returns>
        public static bool TryParseAggregationAndType(string filter, out string aggregation, out GroupingIncludesType aggregationType)
        {
            int startIndex;

            if (filter.StartsWith("agg:"))
            {
                aggregationType = GroupingIncludesType.AggregatedValues;
                startIndex = 4;
            }
            else if (filter.StartsWith("agg_single:"))
            {
                aggregationType = GroupingIncludesType.SingleValues;
                startIndex = 11;
            }
            else if (filter.StartsWith("agg_all:"))
            {
                aggregationType = GroupingIncludesType.All;
                startIndex = 8;
            }
            else
            {
                aggregation = "";
                aggregationType = GroupingIncludesType.AggregatedValues;
                return false; //Not an aggregation
            }

            aggregation = filter.Substring(startIndex);
            return true;
        }

        /// <summary>
        /// Get filter string for aggregation type
        /// </summary>
        /// <param name="aggType">Aggregation type</param>
        /// <returns>Filter string for the aggregation type</returns>
        public static string GetAggregationTypeFilterString(GroupingIncludesType aggType)
        {
            switch (aggType)
            {
                case GroupingIncludesType.AggregatedValues:
                    return "agg:";
                case GroupingIncludesType.SingleValues:
                    return "agg_single:";
                case GroupingIncludesType.All:
                    return "agg_all:";
                default:
                    return "agg:";
            }
        }

        public static string GetTableName(TableSource src)
        {
            if (src.SourceIdType == "path")
            {
                var parts = src.Source.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                return parts[parts.Length - 1];
            }

            return src.Source;
        }

        public static PCAxis.Paxiom.Selection SelectValueSet(Variable variable, Query query, PCAxis.Paxiom.IPXModelBuilder builder)
        {
            if (!query.Selection.Filter.StartsWith("vs:"))
            {
                throw new Exception(); //TODO it must be an aggregation
            }

            string vs = query.Selection.Filter.Substring(3);
            var vsInfo = variable.GetValuesetById(vs);
            if (vsInfo == null)
            {
                throw new Exception(); //TODO no such aggregation
            }

            builder.ApplyValueSet(variable.Code, vsInfo);

            var v = builder.Model.Meta.Variables.First(x => x.Code == variable.Code);

            return SelectItem(v, query);
        }

        public static PCAxis.Paxiom.Selection SelectAggregation(Variable variable, Query query, PCAxis.Paxiom.IPXModelBuilder builder)
        {
            GroupingIncludesType aggType;
            string agg;

            if (!PCAxis.Query.QueryHelper.TryParseAggregationAndType(query.Selection.Filter, out agg, out aggType))
            {
                throw new Exception(); //TODO could not parse aggregation
            }

            var aggInfo = variable.GetGroupingInfoById(agg);
            if (aggInfo == null)
            {
                throw new Exception(); //TODO no such aggregation
            }

            builder.ApplyGrouping(variable.Code, aggInfo, aggType);

            var v = builder.Model.Meta.Variables.First(x => x.Code == variable.Code);

            return SelectItem(v, query);

        }

        /// <summary>
        /// selects all values that matches the wildcard expressions. Only one wild card can be given per expression
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PCAxis.Paxiom.Selection SelectAll(Variable variable, Query query)
        {
            PCAxis.Paxiom.Selection s = new PCAxis.Paxiom.Selection(query.Code);
            var values = new HashSet<string>();
            foreach (var value in query.Selection.Values)
            {
                if (value.StartsWith("*"))
                {
                    var variableValues = variable.Values.Where(v => v.Code.EndsWith(value.Substring(1))).Select(v => v.Code);
                    foreach (var variableValue in variableValues)
                    {
                        values.Add(variableValue);
                    }
                }
                else if (value.EndsWith("*"))
                {
                    var variableValues = variable.Values.Where(v => v.Code.StartsWith(value.Substring(0, value.Length - 1))).Select(v => v.Code);
                    foreach (var variableValue in variableValues)
                    {
                        values.Add(variableValue);
                    }
                }
                else
                {
                    throw new Exception("Invalid value supplied to ALL-filter");
                }
            }

            //Remove duplicates
            s.ValueCodes.AddRange(values.Distinct().ToArray());
            return s;
        }

        /// <summary>
        /// Selects all values after a certain value. This filter is only applicable on a time variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PCAxis.Paxiom.Selection SelectFrom(Variable variable, Query query)
        {

            if (!variable.IsTime)
            {
                throw new Exception(); //TODO only the time variable is applicable for this filter
            }

            if (query.Selection.Values.Length != 1)
            {
                throw new Exception();//TODO throw parameter exception there must be only one value
            }

            //Check if the time periods is order in ascending or descending order
            bool isDescending = true;
            if (variable.Values.Count > 1)
            {
                if (string.Compare(variable.Values[0].TimeValue, variable.Values[1].TimeValue) < 0)
                {
                    isDescending = false;
                }
            }

            string value = query.Selection.Values[0];
            int index = variable.Values.FindIndex(val => val.Code == value);

            PCAxis.Paxiom.Selection s = new PCAxis.Paxiom.Selection(query.Code);

            if (variable.Values.Count > 1)
            {
                if (isDescending)
                {
                    s.ValueCodes.AddRange(variable.Values.Take(index + 1).Select(val => val.Code).ToArray());
                }
                else
                {
                    s.ValueCodes.AddRange(variable.Values.Skip(index).Take(variable.Values.Count - index).Select(val => val.Code).ToArray());
                }
            }
            else
            {
                s.ValueCodes.Add(value);
            }

            return s;
        }


        /// <summary>
        /// Selects the values of the variable if the variable is time then the latest time periods for that variable is selected
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PCAxis.Paxiom.Selection SelectTop(Variable variable, Query query)
        {
            if (query.Selection.Values.Length != 1)
            {
                throw new Exception();//TODO throw parameter exception there must be only one value
            }

            int value;
            if (!int.TryParse(query.Selection.Values[0], out value))
            {
                throw new Exception();//TODO throw parameter exception it must be an integer
            }

            if (value < 1)
            {
                throw new Exception();//TODO throw parameter exception the value must be larger than 0
            }

            bool isDescending = true;
            if (variable.IsTime)
            {
                //Check if the time periods is order in ascending or descending order
                if (variable.Values.Count > 1)
                {
                    if (string.Compare(variable.Values[0].Value, variable.Values[1].Value) < 0)
                    {
                        isDescending = false;
                    }
                }
            }

            PCAxis.Paxiom.Selection s = new PCAxis.Paxiom.Selection(query.Code);
            if (isDescending)
            {
                s.ValueCodes.AddRange(variable.Values.Take(value).Select(val => val.Code).ToArray());
            }
            else
            {
                s.ValueCodes.AddRange(variable.Values.Skip(Math.Max(0, variable.Values.Count() - value)).Take(value).Select(val => val.Code).ToArray());
            }

            return s;
        }

        /// <summary>
        /// Selects all values from the variable that is in the query by the value codes
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PCAxis.Paxiom.Selection SelectItem(Variable variable, Query query)
        {

            PCAxis.Paxiom.Selection s = new PCAxis.Paxiom.Selection(query.Code);
            s.ValueCodes.AddRange(
                variable.Values
                    .Where(val => query.Selection.Values.Contains(val.Code))
                    .Select(val => val.Code).ToArray());

            if (s.ValueCodes.Count != query.Selection.Values.Length)
            {
                //Error missmatch between the number of values
                return null;
            }

            return s;
        }

        /// <summary>
        /// Runs all operations in the saved query and return the final model
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PXModel RunWorkflow(SavedQuery sq, PXModel model)
        {
            PXModel m = model;

            foreach (var step in sq.Workflow)
            {
                var ser = OperationsTracker.CreateSerializer(step.Type);
                var op = ser.CreateOperation();
                var m2 = op.Execute(m, ser.Deserialize(step));
                if (m != m2) m.Dispose();
                m = m2;
            }
            return m;
        }

        public static bool TryParseSavedQueryOutputFormat(string desiredFormat, out string formatString)
        {

            if (Enum.TryParse(desiredFormat, out SavedQueryOutputFormatType newFormat))
            {
                switch (newFormat)
                {
                    case SavedQueryOutputFormatType.px:
                        formatString = "FileTypePX";
                        break;
                    case SavedQueryOutputFormatType.csv:
                        formatString = "FileTypeCsvWithoutHeadingAndComma";
                        break;
                    case SavedQueryOutputFormatType.csv_comma:
                        formatString = "FileTypeCsvWithoutHeadingAndComma";
                        break;
                    case SavedQueryOutputFormatType.csv_commahead:
                        formatString = "FileTypeCsvWithHeadingAndComma";
                        break;
                    case SavedQueryOutputFormatType.csv_semicolon:
                        formatString = "FileTypeCsvWithoutHeadingAndSemiColon";
                        break;
                    case SavedQueryOutputFormatType.csv_semicolonhead:
                        formatString = "FileTypeCsvWithHeadingAndSemiColon";
                        break;
                    case SavedQueryOutputFormatType.csv_space:
                        formatString = "FileTypeCsvWithoutHeadingAndSpace";
                        break;
                    case SavedQueryOutputFormatType.csv_spacehead:
                        formatString = "FileTypeCsvWithHeadingAndSpace";
                        break;
                    case SavedQueryOutputFormatType.csv_tab:
                        formatString = "FileTypeCsvWithoutHeadingAndTabulator";
                        break;
                    case SavedQueryOutputFormatType.csv_tabhead:
                        formatString = "FileTypeCsvWithHeadingAndTabulator";
                        break;
                    case SavedQueryOutputFormatType.xlsx:
                        formatString = "FileTypeExcelX";
                        break;
                    case SavedQueryOutputFormatType.xlsx_doublecolumn:
                        formatString = "FileTypeExcelXDoubleColumn";
                        break;
                    case SavedQueryOutputFormatType.json_stat:
                        formatString = "FileTypeJsonStat";
                        break;
					case SavedQueryOutputFormatType.json_stat2:
						formatString = "FileTypeJsonStat2";
						break;
					case SavedQueryOutputFormatType.json:
                        formatString = "FileTypeJson";
                        break;
                    case SavedQueryOutputFormatType.html5_table:
                        formatString = "FileTypeHtml5Table";
                        break;
                    case SavedQueryOutputFormatType.relational_table:
                        formatString = "FileTypeRelational";
                        break;
                    default:
                        formatString = "";
                        return false;
                }

                return true;
            }
            else
            {
                formatString = "";
                return false;
            }


        }

    }
}
