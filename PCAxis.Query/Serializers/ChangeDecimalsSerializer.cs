using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Query.Serializers
{
    public class ChangeDecimalsSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is ChangeDecimalsDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as ChangeDecimalsDescription;
            var step = new WorkStep();

            step.Id = Guid.NewGuid().ToString();

            step.Type = OperationConstants.CHANGE_DECIMALS;
            step.Params.Add("decimals", desc.Decimals.ToString());
            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.CHANGE_DECIMALS)
            {
                //TODO throw exception
                return null;
            }

            var desc = new ChangeDecimalsDescription();
            desc.Decimals = step.CheckParameter("decimals") ? int.Parse(step.Params["decimals"]) : 0;

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new ChangeDecimals();
        }
    }
}
