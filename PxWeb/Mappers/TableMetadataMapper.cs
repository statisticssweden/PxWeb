using J2N.Collections.Generic;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Extensions;
using Microsoft.Extensions.Options;
using PxWeb.Config.Api2;

namespace PxWeb.Mappers
{
    public class TableMetadataMapper : ITableMetadataMapper
    {
        private ILinkCreator _linkCreator;
        private PxApiConfigurationOptions _configOptions;

        public TableMetadataMapper(ILinkCreator linkCreator, IOptions<PxApiConfigurationOptions> configOptions)
        {
            _linkCreator = linkCreator;
            _configOptions = configOptions.Value; 
        }

        public TableMetadata Map(PXModel model, string id, string language)
        {
            TableMetadata tm = new TableMetadata();

            tm.Id = id.ToUpper();
            tm.Language = language;
            tm.Label = model.Meta.Title;  
            tm.Description = model.Meta.Description;
            tm.Source = model.Meta.Source;
            //tm.Tags = new System.Collections.Generic.List<string>(); // TODO: Implement later
            tm.OfficalStatistics = model.Meta.OfficialStatistics;
            tm.SubjectLabel = model.Meta.SubjectArea; 
            tm.SubjectCode = model.Meta.SubjectCode;
            tm.Licence = _configOptions.License;
            tm.AggregationAllowed = model.Meta.AggregAllowed;
            //tm.Discontinued = false; // TODO: Implement later. Not in spec!

            tm.Variables = new System.Collections.Generic.List<AbstractVariable>(); 

            foreach (Variable variable in model.Meta.Variables)
            {
                tm.Variables.Add(Map(tm, variable));
            }

            MapTableNotes(tm, model);

            tm.Links = new System.Collections.Generic.List<Link>();

            foreach (var lang in _configOptions.Languages)
            {
                tm.Links.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.self, id.ToUpper(), lang.Id));
            }

            return tm;
        }

        private AbstractVariable Map(TableMetadata tm, Variable variable)
        {
            AbstractVariable v;

            if (variable.IsTime)
            {
                v = MapTimeVariable(variable);
            }
            else if (variable.IsContentVariable)
            {
                v = MapContentsVariable(tm, variable);
            }
            else if (!string.IsNullOrWhiteSpace(variable.Map) || (variable.VariableType != null && variable.VariableType.Equals("G"))) 
            {
                v = MapGeographicalVariable(variable);   
            }
            else
            {
                v = MapRegularVariable(variable);
            }

            v.Id = variable.Code;
            v.Label = variable.Name;

            if (variable.Notes != null)
            {
                v.Notes = new System.Collections.Generic.List<Api2.Server.Models.Note>();

                foreach (var note in variable.Notes)
                {
                    v.Notes.Add(Map(note)); 
                }
            }
            
            if (variable.Notes != null)
            {
                v.Notes = new System.Collections.Generic.List<Api2.Server.Models.Note>();

                foreach (var note in variable.Notes)
                {
                    v.Notes.Add(Map(note));
                }
            }

            return v;
        }

        private TimeVariable MapTimeVariable(Variable variable)
        {
            TimeVariable timeVariable = new TimeVariable();
            timeVariable.Type = AbstractVariable.TypeEnum.TimeVariableEnum; 
            timeVariable.FirstPeriod = GetFirstTimePeriod(variable);    
            timeVariable.LastPeriod = GetLastTimePeriod(variable);
            timeVariable.TimeUnit = GetTimeUnit(variable.TimeScale);

            timeVariable.Values = new System.Collections.Generic.List<Api2.Server.Models.Value>();
            MapValues(timeVariable.Values, variable);

            return timeVariable;
        }

        private ContentsVariable MapContentsVariable(TableMetadata tm, Variable variable)
        {
            ContentsVariable contentsVariable = new ContentsVariable();
            contentsVariable.Type = AbstractVariable.TypeEnum.ContentsVariableEnum;

            contentsVariable.Values = new System.Collections.Generic.List<ContentValue>();

            foreach (var value in variable.Values)
            {
                contentsVariable.Values.Add(MapContentValue(tm, value));
            }

            return contentsVariable;    
        }

        private GeographicalVariable MapGeographicalVariable(Variable variable)
        {
            GeographicalVariable geographicalVariable = new GeographicalVariable();
            geographicalVariable.Type = AbstractVariable.TypeEnum.GeographicalVariableEnum;
            geographicalVariable.Map = variable.Map;
            geographicalVariable.Elimination = variable.Elimination;
            if (variable.EliminationValue != null)
            {
                geographicalVariable.EliminationValueCode = variable.EliminationValue.Code; 
            }

            geographicalVariable.Values = new System.Collections.Generic.List<Api2.Server.Models.Value>();
            MapValues(geographicalVariable.Values, variable);

            if (variable.HasGroupings() || variable.HasValuesets())
            {
                geographicalVariable.CodeLists = new System.Collections.Generic.List<CodeListInformation>();
                MapCodelists(geographicalVariable.CodeLists, variable);
            }

            return geographicalVariable;    
        }

        private RegularVariable MapRegularVariable(Variable variable)
        {
            RegularVariable regularVariable = new RegularVariable();
            regularVariable.Type = AbstractVariable.TypeEnum.RegularVariableEnum;
            regularVariable.Elimination = variable.Elimination;
            if (variable.EliminationValue != null)
            {
                regularVariable.EliminationValueCode = variable.EliminationValue.Code; 
            }

            regularVariable.Values = new System.Collections.Generic.List<Api2.Server.Models.Value>();
            MapValues(regularVariable.Values, variable);

            if (variable.HasGroupings() || variable.HasValuesets())
            {
                regularVariable.CodeLists = new System.Collections.Generic.List<CodeListInformation>();
                MapCodelists(regularVariable.CodeLists, variable);
            }

            return regularVariable;
        }

        private Api2.Server.Models.Value Map(PCAxis.Paxiom.Value value)
        {
            Api2.Server.Models.Value v = new Api2.Server.Models.Value();
            v.Code = value.Code;
            v.Label = value.Text;
            
            if (value.Notes != null)
            {
                v.Notes = new System.Collections.Generic.List<Api2.Server.Models.Note>();

                foreach (var note in value.Notes)
                {
                    v.Notes.Add(Map(note));
                }
            }

            return v;   
        }

        private ContentValue MapContentValue(TableMetadata tm, PCAxis.Paxiom.Value value)
        {
            ContentValue cv = new ContentValue();

            cv.Code = value.Code;
            cv.Label = value.Text;
            cv.PreferedNumberOfDecimals = value.Precision;

            if (value.ContentInfo != null)
            {
                GetContentInfo(tm, cv, value.ContentInfo);
            }
            else if (value.Variable.Meta.ContentInfo != null)
            {
                GetContentInfo(tm, cv, value.Variable.Meta.ContentInfo);
            }

            if (value.Notes != null)
            {
                cv.Notes = new System.Collections.Generic.List<Api2.Server.Models.Note>();

                foreach (var note in value.Notes)
                {
                    cv.Notes.Add(Map(note));
                }
            }

            return cv;
        }

        private void GetContentInfo(TableMetadata tm, ContentValue cv, ContInfo contInfo)
        {
            cv.MeasuringType = GetMeasuringType(contInfo.StockFa);
            cv.Adjustment = GetAdjustment(contInfo.DayAdj, contInfo.SeasAdj);
            cv.Unit = contInfo.Units;
            cv.Baseperiod = contInfo.Baseperiod;
            cv.RefrencePeriod = contInfo.RefPeriod;
            cv.PriceType = GetPriceType(contInfo.CFPrices);

            MapLastUpdated(tm, contInfo);

            if (contInfo.ContactInfo != null && contInfo.ContactInfo.Count > 0)
            {
                foreach (var contact in contInfo.ContactInfo)
                {
                    MapContact(tm, contact);
                }
            }
            else
            {
                MapContact(tm, contInfo.Contact);
            }
        }

        private void MapContact(TableMetadata tm, PCAxis.Paxiom.Contact contact)
        {
            if (tm.Contacts == null)
            {
                tm.Contacts = new System.Collections.Generic.List<Api2.Server.Models.Contact>();
            }
            
            Api2.Server.Models.Contact c = new Api2.Server.Models.Contact();

            StringBuilder sb = new StringBuilder();
            sb.Append(contact.Forname);
            sb.Append(" ");
            sb.Append(contact.Surname);

            c.Name = sb.ToString();
            c.Mail = contact.Email;
            c.Phone = contact.PhoneNo;

            // Only display unique contact once
            if(!tm.Contacts.Exists(x => x.Mail.Equals(c.Mail) && x.Name.Equals(c.Name) && x.Phone.Equals(c.Phone)))
            {
                tm.Contacts.Add(c);
            }
        }

        private void MapContact(TableMetadata tm, string contact)
        {
            if (contact != null)
            {
                if (tm.Contacts == null)
                {
                    tm.Contacts = new System.Collections.Generic.List<Api2.Server.Models.Contact>();
                }

                Api2.Server.Models.Contact c = new Api2.Server.Models.Contact();
                c.Raw = contact;

                // Only display unique contact once
                if (!tm.Contacts.Exists(x => x.Raw.Equals(c.Raw)))
                {
                    tm.Contacts.Add(c);
                }
            }
        }

        private void MapLastUpdated(TableMetadata tm, ContInfo contInfo)
        {
            if (contInfo.LastUpdated.IsPxDate())
            {
                DateTime tryDate = contInfo.LastUpdated.PxDateStringToDateTime();
                if (tm.Updated == null || tryDate > tm.Updated)
                {
                    tm.Updated = tryDate;
                }
            }
        }

        private Api2.Server.Models.Note Map(PCAxis.Paxiom.Note note)
        {
            Api2.Server.Models.Note n = new Api2.Server.Models.Note();
            n.Text = note.Text;
            n.Mandatory = note.Mandantory;

            return n;
        }

        private void MapTableNotes(TableMetadata tm, PCAxis.Paxiom.PXModel model)
        {
            if (model.Meta.Notes.Count > 0 || model.Meta.CellNotes.Count > 0)
            {
                tm.Notes = new System.Collections.Generic.List<Api2.Server.Models.CellNote>();

                foreach (var note in model.Meta.Notes)
                {
                    tm.Notes.Add(MapCellNote(note));
                }
                foreach (var note in model.Meta.CellNotes)
                {
                    tm.Notes.Add(MapCellNote(note));
                }
            }
        }

        private Api2.Server.Models.CellNote MapCellNote(PCAxis.Paxiom.Note note)
        {
            Api2.Server.Models.CellNote n = new Api2.Server.Models.CellNote();

            n.Text = note.Text;
            n.Mandatory = note.Mandantory;

            return n;
        }
        private Api2.Server.Models.CellNote MapCellNote(PCAxis.Paxiom.CellNote note)
        {
            Api2.Server.Models.CellNote n = new Api2.Server.Models.CellNote();

            n.Text = note.Text;
            n.Mandatory = note.Mandatory;

            if (note.Conditions.Count > 0)
            {
                n.Conditions = new System.Collections.Generic.List<Condition>();

                foreach (var cond in note.Conditions)
                {
                    Api2.Server.Models.Condition condition = new Api2.Server.Models.Condition();

                    condition.Variable = cond.VariableCode;
                    condition.Value = cond.ValueCode;

                    n.Conditions.Add(condition);
                }
            }

            return n;
        }
        private Api2.Server.Models.CodeListInformation Map(PCAxis.Paxiom.GroupingInfo grouping)
        {
            CodeListInformation codelist = new CodeListInformation();

            codelist.Id = "agg_" + grouping.ID;
            codelist.Label = grouping.Name;
            codelist.Type = CodeListType.AggregationEnum;
            codelist.Links = new System.Collections.Generic.List<Link>();
            codelist.Links.Add(_linkCreator.GetCodelistLink(LinkCreator.LinkRelationEnum.metadata, codelist.Id));

            return codelist;
        }
        private Api2.Server.Models.CodeListInformation Map(PCAxis.Paxiom.ValueSetInfo valueset)
        {
            CodeListInformation codelist = new CodeListInformation();

            codelist.Id = "vs_" + valueset.ID;
            codelist.Label = valueset.Name;
            codelist.Type = CodeListType.ValuesetEnum;
            codelist.Links = new System.Collections.Generic.List<Link>();
            codelist.Links.Add(_linkCreator.GetCodelistLink(LinkCreator.LinkRelationEnum.metadata, codelist.Id));

            return codelist;
        }

        private void MapValues(System.Collections.Generic.List<Api2.Server.Models.Value> values, Variable variable)
        {
            foreach (var value in variable.Values)
            {
                values.Add(Map(value));
            }
        }

        private void MapCodelists(System.Collections.Generic.List<CodeListInformation> codelists, Variable variable)
        {
            if (variable.HasGroupings())
            {
                foreach (var grouping in variable.Groupings)
                {
                    codelists.Add(Map(grouping));
                }
            }

            if (variable.HasValuesets())
            {
                foreach (var valueset in variable.ValueSets)
                {
                    if (!valueset.ID.Equals("_ALL_")) 
                    {
                        codelists.Add(Map(valueset));
                    }
                }
            }
        }


        private TimeVariable.TimeUnitEnum GetTimeUnit(TimeScaleType timeScaleType)
        {
            switch (timeScaleType)
            {
                case TimeScaleType.NotSet:
                    return TimeVariable.TimeUnitEnum.OtherEnum;
                case TimeScaleType.Annual:
                    return TimeVariable.TimeUnitEnum.AnnualEnum;
                case TimeScaleType.Halfyear:
                    return TimeVariable.TimeUnitEnum.HalfYearEnum;
                case TimeScaleType.Quartely:
                    return TimeVariable.TimeUnitEnum.QuarterlyEnum;
                case TimeScaleType.Monthly:
                    return TimeVariable.TimeUnitEnum.MonthlyEnum;
                case TimeScaleType.Weekly:
                    return TimeVariable.TimeUnitEnum.WeeklyEnum;
                default:
                    return TimeVariable.TimeUnitEnum.OtherEnum;
            }
        }

        private string GetFirstTimePeriod(Variable variable)
        {
            string first = "";

            if (variable.Values.Count > 0)
            {
                first = variable.Values.First().Text;
                string val2 = variable.Values.Last().Text;

                if (string.CompareOrdinal(first, val2) > 0)
                {
                    first = val2;
                }
            }

            return first;
        }
        private string GetLastTimePeriod(Variable variable)
        {
            string last = "";

            if (variable.Values.Count > 0)
            {
                last = variable.Values.Last().Text;
                string val2 = variable.Values.First().Text;

                if (string.CompareOrdinal(last, val2) < 0)
                {
                    last = val2;
                }
            }

            return last;
        }

        private ContentValue.MeasuringTypeEnum GetMeasuringType(string stockfa)
        {
            switch (stockfa.ToUpper())
            {
                case "S":
                    return ContentValue.MeasuringTypeEnum.StockEnum;
                case "F":
                    return ContentValue.MeasuringTypeEnum.FlowEnum;
                case "A":
                    return ContentValue.MeasuringTypeEnum.AverageEnum;
                default:
                    return ContentValue.MeasuringTypeEnum.OtherEnum;
            }
        }

        private ContentValue.AdjustmentEnum GetAdjustment(string dayAdj, string seasAdj)
        {
            string dadj = dayAdj != null ? dayAdj.ToUpper() : "";
            string sadj = seasAdj != null ? seasAdj.ToUpper() : "";

            if (dadj.Equals("YES") && sadj.Equals("YES"))
            {
                return ContentValue.AdjustmentEnum.WorkAndSesEnum;
            }
            else if (sadj.Equals("YES"))
            {
                return ContentValue.AdjustmentEnum.SesOnlyEnum;
            }
            else if (dadj.Equals("YES"))
            {
                return ContentValue.AdjustmentEnum.WorkOnlyEnum;
            }
            else
            {
                return ContentValue.AdjustmentEnum.NoneEnum;
            }
        }

        private ContentValue.PriceTypeEnum GetPriceType(string CFPrices)
        {
            string cfp = CFPrices != null ? CFPrices.ToUpper() : "";

            switch (cfp)
            {
                case "C":
                    return ContentValue.PriceTypeEnum.CurrentEnum;
                case "F":
                    return ContentValue.PriceTypeEnum.FixedEnum;
                default:
                    return ContentValue.PriceTypeEnum.UndefinedEnum; 
            }
        }
    }
}
