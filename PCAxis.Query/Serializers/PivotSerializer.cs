using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Operations;
using PCAxis.Paxiom;

namespace PCAxis.Query.Serializers
{
    public class PivotSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is PivotDescription[]))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as PivotDescription[];

            var step = new WorkStep();

            step.Id = Guid.NewGuid().ToString();
            step.Type = OperationConstants.PIVOT;

            step.Params.Add("_count", desc.Length.ToString());

            for (int i = 0; i < desc.Length; i++)
            {
                step.Params.Add(i + ".name", desc[i].VariableName);
                step.Params.Add(i + ".placement", desc[i].VariablePlacement.ToString());
            }

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.PIVOT)
            {
                //TODO throw exception
                return null;
            }

            var desc = new List<PivotDescription>();

            int count;
            if (!int.TryParse(step.Params["_count"], out count))
            {
                //TODO throw Exception
                return null;
            }

            for (int i = 0; i < count; i++)
            {
                var d = new PivotDescription();
                d.VariableName = step.Params[i + ".name"];
                d.VariablePlacement = (PlacementType) Enum.Parse(typeof(PlacementType), step.Params[i + ".placement"], true);
                desc.Add(d);
            }

            return desc.ToArray();
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new Pivot();
        }
    }
}
