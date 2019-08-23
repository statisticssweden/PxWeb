using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class DeleteValueSerializer : IOperationSerializer
    {

        public WorkStep Serialize(object obj)
        {
            if (!(obj is Selection[]))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as Selection[];
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.DELETE_VALUE;

            desc.Serialize(step, "");

            return step;

        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.DELETE_VALUE)
            {
                //TODO throw exception
                return null;
            }

            return step.DeserializeSelectionArray("");
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new DeleteValue();
        }
    }
}
