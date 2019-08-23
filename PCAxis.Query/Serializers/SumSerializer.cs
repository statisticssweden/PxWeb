using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public class SumSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is SumDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as SumDescription;
            var step = new WorkStep() { Id = Guid.NewGuid().ToString() };

            step.Type = OperationConstants.SUM;

            step.Params.Add("variableCode", desc.VariableCode);
            step.Params.Add("newValueName", desc.NewValueName);
            step.Params.Add("newValueCode", desc.NewValueCode);
            step.Params.Add("keepValues", desc.KeepValues.ToString());
            step.Params.Add("constantValue", desc.ConstantValue.ToString());
            step.Params.Add("calcWithContant", desc.CalculateWithConstant.ToString());
            step.Params.Add("operation", desc.Operation.ToString());
            step.Params.Add("groupedVariableIndex", desc.GroupedVariableIndex.ToString());
            step.Params.Add("doEiminationForSumAll", desc.DoEliminationForSumAll.ToString());

            step.Params.Add("values._count", desc.ValueCodes.Count.ToString());
            for (int i = 0; i < desc.ValueCodes.Count; i++)
            {
                step.Params.Add("values." + i, desc.ValueCodes[i]);    
            }
            
            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.SUM)
            {
                //TODO throw exception
                return null;
            }

            var desc = new SumDescription();

            desc.VariableCode = step.Params["variableCode"];
            desc.NewValueName = step.Params["newValueName"];
            desc.NewValueCode = step.Params["newValueCode"];
            desc.KeepValues = bool.Parse(step.Params["keepValues"]);
            desc.ConstantValue = double.Parse(step.Params["constantValue"]);
            desc.CalculateWithConstant = bool.Parse(step.Params["calcWithContant"]);
            desc.Operation = (SumOperationType)Enum.Parse(typeof(SumOperationType), step.Params["operation"], true);
            desc.GroupedVariableIndex = int.Parse(step.Params["groupedVariableIndex"]);
            desc.DoEliminationForSumAll = bool.Parse(step.Params["doEiminationForSumAll"]);

            int count = int.Parse(step.Params["values._count"]);

            for (int i = 0; i < count; i++)
            {
                desc.ValueCodes.Add(step.Params["values." + i]);
            }            

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new Sum();
        }
    }
}
