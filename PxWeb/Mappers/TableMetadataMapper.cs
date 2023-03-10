using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Mappers
{
    public class TableMetadataMapper : ITableMetadataMapper
    {
        ILinkCreator _linkCreator;
        public TableMetadataMapper(ILinkCreator linkCreator)
        {
            _linkCreator = linkCreator; 
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
            tm.SubjectCode = model.Meta.SubjectCode;
            tm.Licence = "???"; // TODO: Get from appsettings?
            tm.AggregationAllowed = model.Meta.AggregAllowed;
            //tm.Discontinued = ???; // TODO: Implement later

            tm.Variables = new System.Collections.Generic.List<AbstractVariable>();

            foreach (Variable variable in model.Meta.Variables)
            {
                tm.Variables.Add(Map(variable));
            }

            tm.Links = new System.Collections.Generic.List<Link>();
            tm.Links.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.self, id.ToUpper()));

            return tm;
        }

        private AbstractVariable Map(Variable variable)
        {
            AbstractVariable v;

            if (variable.IsTime)
            {
                TimeVariable timeVariable = new TimeVariable();
                timeVariable.Type = AbstractVariable.TypeEnum.TimeVariableEnum; // TODO: should it be TIME?
                timeVariable.FirstPeriod = "1900"; // TODO: 
                timeVariable.LastPeriod = "2023"; // TODO:
                timeVariable.TimeUnit = GetTimeUnit(variable.TimeScale);

                timeVariable.Values = new System.Collections.Generic.List<Api2.Server.Models.Value>();
                MapValues(timeVariable.Values, variable);
                
                v = timeVariable;
            }
            else if (variable.IsContentVariable)
            {
                ContentsVariable contentsVariable = new ContentsVariable();
                contentsVariable.Type = AbstractVariable.TypeEnum.ContentsVariableEnum;

                contentsVariable.Values = new System.Collections.Generic.List<ContentValue>();

                foreach (var value in variable.Values)
                {
                    contentsVariable.Values.Add(MapContentValue(value));  
                }

                v = contentsVariable;
            }
            else if (!string.IsNullOrWhiteSpace(variable.Map))
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

                v = geographicalVariable;   
            }
            else
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

                v = regularVariable;
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

            return v;
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

        private ContentValue MapContentValue(PCAxis.Paxiom.Value value)
        {
            ContentValue cv = new ContentValue();

            cv.Code = value.Code;
            cv.Label = value.Text;

            if (value.ContentInfo != null)
            {
                //cv.MeasuringType = ContentValue.MeasuringTypeEnum.FlowEnum; // TODO: How map this???
                //cv.Adjustment = value.ContentInfo.DayAdj;
                cv.Unit = value.ContentInfo.Units;
                cv.Baseperiod = value.ContentInfo.Baseperiod;
                cv.RefrencePeriod = value.ContentInfo.RefPeriod;
                //cv.PreferedNumberOfDecimals = value.ContentInfo.Value ???;
                //cv.PriceType = value.ContentInfo.CFPrices;
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

        private Api2.Server.Models.Note Map(PCAxis.Paxiom.Note note)
        {
            Api2.Server.Models.Note n = new Api2.Server.Models.Note();
            n.Text = note.Text;
            n.Mandatory = note.Mandantory;

            return n;
        }

        private void MapValues(System.Collections.Generic.List<Api2.Server.Models.Value> values, Variable variable)
        {
            foreach (var value in variable.Values)
            {
                values.Add(Map(value));
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
    }
}
