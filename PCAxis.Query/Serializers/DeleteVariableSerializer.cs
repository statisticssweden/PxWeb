using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class DeleteVariableSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is DeleteVariableDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as DeleteVariableDescription;
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.DELETE_VARIABLE;

            step.Params.Add("variableCode", desc.variableCode);
            step.Params.Add("valueCode", desc.valueCode);
            step.Params.Add("addToContents", desc.addToContents.ToString());

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.DELETE_VARIABLE)
            {
                //TODO throw exception
                return null;
            }

            var desc = new DeleteVariableDescription();

            desc.variableCode = step.Params["variableCode"];
            desc.valueCode = step.Params["valueCode"];
            desc.addToContents = bool.Parse(step.Params["addToContents"]);

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new DeleteVariable();
        }
    }
}
