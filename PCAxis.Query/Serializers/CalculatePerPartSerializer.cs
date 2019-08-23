using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class CalculatePerPartSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is CalculatePerPartDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as CalculatePerPartDescription;
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.PER_PART;

            step.Params.Add("calculationVariant", desc.CalculationVariant.ToString());
            step.Params.Add("operationType", desc.OperationType.ToString());
            step.Params.Add("valueName", desc.ValueName);
            step.Params.Add("keepValue", desc.KeepValue.ToString());

            desc.ValueSelection.Serialize(step, "selection.");

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.PER_PART)
            {
                //TODO throw exception
                return null;
            }

            var desc = new CalculatePerPartDescription();
            desc.CalculationVariant = (CalculatePerPartSelectionType)Enum.Parse(typeof(CalculatePerPartSelectionType), step.Params["calculationVariant"], true);
            desc.OperationType = (CalculatePerPartType)Enum.Parse(typeof(CalculatePerPartType), step.Params["operationType"], true);
            desc.ValueName = step.Params["valueName"];
            desc.KeepValue = bool.Parse(step.Params["keepValue"]);

            desc.ValueSelection = step.DeserializeSelectionArray("selection.");

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new CalculatePerPart();
        }
    }
}
