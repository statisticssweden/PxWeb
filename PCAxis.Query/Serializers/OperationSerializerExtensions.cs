using PCAxis.Paxiom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query.Serializers
{
    public static class OperationSerializerExtensions
    {
        public static void Serialize(this Selection[] selection, WorkStep step, string prefix)
        {
            step.Params.Add(prefix + "variables._count", selection.Length.ToString());

            for (int i = 0; i < selection.Length; i++)
            {
                step.Params.Add(prefix + "variables." + i + ".variableCode", selection[i].VariableCode);
                step.Params.Add(prefix + "variables." + i + ".values._count", selection[i].ValueCodes.Count.ToString());
                for (int j = 0; j < selection[i].ValueCodes.Count; j++)
                {
                    step.Params.Add(prefix + "variables." + i + ".values." + j + ".code", selection[i].ValueCodes[j]);
                }
            }
        }

        public static Selection[] DeserializeSelectionArray(this WorkStep step, string prefix)
        {
            var selections = new List<Selection>();
            int iCount = int.Parse(step.Params[prefix + "variables._count"]);
            for (int i = 0; i < iCount; i++)
            {
                var selection = new Selection(step.Params[prefix + "variables." + i + ".variableCode"]);
                int jCount = int.Parse(step.Params[prefix + "variables." + i + ".values._count"]);
                for (int j = 0; j < jCount; j++)
                {
                    selection.ValueCodes.Add(step.Params[prefix + "variables." + i + ".values." + j + ".code"]);
                }
                selections.Add(selection);
            }

            return selections.ToArray();
        }

        public static bool CheckParameter(this WorkStep step, string key)
        {
            if (step.Params.ContainsKey(key))
            {
                if (step.Params[key] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
