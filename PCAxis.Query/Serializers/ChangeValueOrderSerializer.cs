using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class ChangeValueOrderSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is ChangeValueOrderDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as ChangeValueOrderDescription;
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.CHANGE_VALUE_ORDER;

            step.Params.Add("variableCode", desc.VariableCode);

            step.Params.Add("indexies._count", desc.ModifiedVariableValueWeight.Length.ToString());

            for (int i = 0; i < desc.ModifiedVariableValueWeight.Length; i++)
            {
                step.Params.Add("indexies." + i , desc.ModifiedVariableValueWeight[i].ToString());
            }

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.CHANGE_VALUE_ORDER)
            {
                //TODO throw exception
                return null;
            }

            var desc = new ChangeValueOrderDescription();

            desc.VariableCode = step.Params["variableCode"];
            int count = int.Parse(step.Params["indexies._count"]);
            var values = new List<int>();
            for (int i = 0; i < count; i++)
            {
                values.Add(int.Parse(step.Params["indexies." + i]));
            }
            desc.ModifiedVariableValueWeight = values.ToArray();

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new ChangeValueOrder();
        }
    }
}
