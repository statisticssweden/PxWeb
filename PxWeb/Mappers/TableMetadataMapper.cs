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
                timeVariable.Type = AbstractVariable.TypeEnum.TimeVariableEnum;
                timeVariable.FirstPeriod = "1900"; // TODO: 
                timeVariable.LastPeriod = "2023"; // TODO:
                timeVariable.TimeUnit = GetTimeUnit(variable.TimeScale);
                timeVariable.Values = new System.Collections.Generic.List<Api2.Server.Models.Value>();
                v = timeVariable;
            }
            else if (variable.IsContentVariable)
            {
                ContentsVariable contentsVariable = new ContentsVariable();
                contentsVariable.Type = AbstractVariable.TypeEnum.ContentsVariableEnum;
                contentsVariable.Values = new System.Collections.Generic.List<ContentValue>();
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
                v = regularVariable;
            }

            v.Id = variable.Code;
            v.Label = variable.Name;
            
            return v;
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
