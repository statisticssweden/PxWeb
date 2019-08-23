using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class SplitTimevariableSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.SPLIT_TIME;

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.SPLIT_TIME)
            {
                //TODO throw exception
                return null;
            }

            return null;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new SplitTimevariable();
        }
    }
}
