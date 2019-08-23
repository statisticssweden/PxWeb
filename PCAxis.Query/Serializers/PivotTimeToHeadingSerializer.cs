using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Query.Serializers
{
    /// <summary>
    /// Class to serialize the PivotTimeToHeading operation
    /// </summary>
    public class PivotTimeToHeadingSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            var step = new WorkStep();
            step.Id = Guid.NewGuid().ToString();
            step.Type = OperationConstants.PIVOT_TIME_TO_HEADING;
            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.PIVOT_TIME_TO_HEADING)
            {
                //TODO throw exception
                return null;
            }
            var desc = new PivotTimeToHeadingDescription();

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new PivotTimeToHeading();
        }
    }
}
