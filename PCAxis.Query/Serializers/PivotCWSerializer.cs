using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class PivotCWSerializer : IOperationSerializer
    {

        public WorkStep Serialize(object obj)
        {
            var step = new WorkStep();

            step.Id = Guid.NewGuid().ToString();

            step.Type = OperationConstants.PIVOT_CW;

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.PIVOT_CW)
            { 
                //TODO throw exception
                return null;
            }
            return null;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new PivotCW();
        }

        private class PivotCW : IPXOperation 
        {
            public PXModel Execute(PXModel lhs, object rhs)
            {
                var pvt = new Pivot();

                return pvt.PivotCW(lhs);
            }
        }
    }
}
