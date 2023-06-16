using PCAxis.Paxiom;
using Px.Abstractions;

namespace PxWeb.Mappers
{
    public class CodelistMapper : ICodelistMapper
    {
        public Codelist Map(Grouping grouping)
        {
            Codelist codelist = new Codelist(grouping.ID, grouping.Name);
            codelist.CodelistType = Codelist.CodelistTypeEnum.Aggregation;

            foreach (Group group in grouping.Groups)
            {
                codelist.Values.Add(Map(group));
            }

            return codelist;
        }

        public Codelist Map(Valueset valueset)
        {
            throw new System.NotImplementedException();
        }

        private CodelistValue Map(Group group)
        {
            CodelistValue codelistValue = new CodelistValue();

            codelistValue.Code = group.GroupCode;
            codelistValue.Label = group.Name;

            codelistValue.ValueMap = new System.Collections.Generic.List<string>();

            foreach (GroupChildValue child in group.ChildCodes)
            {
                codelistValue.ValueMap.Add(child.Code);
            }

            return codelistValue;
        }
    }
}
