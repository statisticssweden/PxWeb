using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class SortTimeVariableSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is SortTimeVariableDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as SortTimeVariableDescription;
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.SORT_TIME;

            step.Params.Add("sortOrder", desc.SortOrder.ToString());

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.SORT_TIME)
            {
                //TODO throw exception
                return null;
            }

            return new SortTimeVariableDescription(
            (SortTimeVariableDescription.SortOrderType)Enum.Parse(typeof(SortTimeVariableDescription.SortOrderType), step.Params["sortOrder"], true));

        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new SortTimeVariable();
        }
    }
}
