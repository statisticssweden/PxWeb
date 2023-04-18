using Lucene.Net.Util;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PxWeb.Code.Api2.DataSelection
{
    public class SelectionHandler : ISelectionHandler
    {
        public Selection[] GetSelection(PXModel model, VariablesSelection? variablesSelection)
        {
            if  (variablesSelection is not null && HasSelection(variablesSelection))
            {
                var selections = new List<Selection>();

                // TODO: Parse values for each variable in variablesSelection and add to selections

                return selections.ToArray();
            }
            else
            {
                return GetDefaultSelection(model);
            }

        }

        public bool Verify(PXModel model, VariablesSelection? variablesSelection, out Problem? problem)
        {
            problem = null;

            // TODO: Verify that all variable and value codes defined in variablesSelection are found in model. If not return false and Problem. 
            // TODO: Verify that mandatory variables have at least one value selected
            //problem = NonExistentVariable();
            //return false;

            return true;
        }

        /// <summary>
        /// Get the default selection based on an algorithm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Selection[] GetDefaultSelection(PXModel model)
        {
            // Only select from variables that cannot be eliminated
            var variablesWithSelection = model.Meta.Variables.FindAll(v => v.Elimination == false);

            // Find the time variable
            var timeVar = variablesWithSelection.Find(v => v.IsTime == true);

            // Find the contents variable
            var contentVar = variablesWithSelection.Find(v => v.IsContentVariable == true);

            // 2 variables - Time and Content
            if (variablesWithSelection.Count == 2 && timeVar != null && contentVar != null)
            {
                return GetDefaultSelectionOnlyTimeAndContent(model);
            }
            // 3 variables - Time, Content (with only 1 value) and one more variable
            else if (variablesWithSelection.Count == 3 && timeVar != null && contentVar != null && contentVar.Values.Count == 1)
            {
                return GetDefaultSelectionThreeVariablesTimeAndContentOneValue(model);
            }
            // 3 variables - Time, Content (with more than 1 value) and one more variable
            else if (variablesWithSelection.Count == 3 && timeVar != null && contentVar != null && contentVar.Values.Count > 1)
            {
                return GetDefaultSelectionThreeVariablesTimeAndContentMoreThanOneValue(model);
            }
            // All other cases
            else
            {
                return GetDefaultSelectionAllOtherCases(model);
            }
        }

        /// <summary>
        /// Returns default selection when only the time- and contentvariable cannot be eliminated
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Selection[] GetDefaultSelectionOnlyTimeAndContent(PXModel model)
        {
            var selections = new List<Selection>();

            foreach (var variable in model.Meta.Variables)
            {
                var selection = new Selection(variable.Code);

                if (variable.IsContentVariable)
                {
                    // Content - Takes all values
                    selection.ValueCodes.AddRange(GetAllCodes(variable));
                }
                else if (variable.IsTime)
                {
                    // Time - Take the 12 last values
                    selection.ValueCodes.AddRange(GetTimeCodes(variable, 12));
                }
                else
                {
                    // Select nothing for the other variables
                }

                selections.Add(selection);
            }

            return selections.ToArray();
        }

        /// <summary>
        /// Returns default selection when the time-, the content variable and one more variable cannot be eliminated.
        /// Also the content variable only has one value.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Selection[] GetDefaultSelectionThreeVariablesTimeAndContentOneValue(PXModel model)
        {
            var selections = new List<Selection>();

            foreach (var variable in model.Meta.Variables)
            {
                var selection = new Selection(variable.Code);

                if (variable.IsContentVariable)
                {
                    // Content - Take the one value
                    selection.ValueCodes.AddRange(GetCodes(variable, 1));
                }
                else if (variable.IsTime)
                {
                    // Time - Take the 12 last values
                    selection.ValueCodes.AddRange(GetTimeCodes(variable, 12));
                }
                else if (!variable.Elimination)
                {
                    // Take all values for the third variable
                    selection.ValueCodes.AddRange(GetAllCodes(variable));
                }
                else
                {
                    // Select nothing for the other variables
                }

                selections.Add(selection);
            }

            return selections.ToArray();
        }

        /// <summary>
        /// Returns default selection when the time-, the content variable and one more variable cannot be eliminated.
        /// Also the content variable only has more than one value.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Selection[] GetDefaultSelectionThreeVariablesTimeAndContentMoreThanOneValue(PXModel model)
        {
            var selections = new List<Selection>();

            foreach (var variable in model.Meta.Variables)
            {
                var selection = new Selection(variable.Code);

                if (variable.IsContentVariable)
                {
                    // Content - Take all values
                    selection.ValueCodes.AddRange(GetAllCodes(variable));
                }
                else if (variable.IsTime)
                {
                    // Time - Take the last value
                    selection.ValueCodes.AddRange(GetTimeCodes(variable, 1));
                }
                else if (!variable.Elimination)
                {
                    // Take all values for the third variable
                    selection.ValueCodes.AddRange(GetAllCodes(variable));
                }
                else
                {
                    // Select nothing for the other variables
                }

                selections.Add(selection);
            }

            return selections.ToArray();
        }

        /// <summary>
        /// Returns default selection for all other cases
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Selection[] GetDefaultSelectionAllOtherCases(PXModel model)
        {
            var selections = new List<Selection>();
            int variableNumber = 1;

            foreach (var variable in model.Meta.Variables)
            {
                var selection = new Selection(variable.Code);

                if (variable.IsContentVariable)
                {
                    // Content - Take the first value
                    selection.ValueCodes.AddRange(GetCodes(variable, 1));
                }
                else if (variable.IsTime)
                {
                    // Time - Take the last value
                    selection.ValueCodes.AddRange(GetTimeCodes(variable, 1));
                }
                else if (!variable.Elimination && (variableNumber == 1 || variableNumber == 2))
                {
                    // Take all values for variable 1 and 2
                    selection.ValueCodes.AddRange(GetAllCodes(variable));
                    variableNumber++;
                }
                else if (!variable.Elimination && (variableNumber > 2))
                {
                    // Take 1 value 
                    selection.ValueCodes.AddRange(GetCodes(variable, 1));
                    variableNumber++;
                }
                else
                {
                    // Select nothing for the other variables
                }

                selections.Add(selection);
            }

            return selections.ToArray();
        }

        private string[] GetCodes(Variable variable, int count)
        {
            var codes = variable.Values.Take(count).Select(value => value.Code).ToArray();

            return codes;
        }

        private string[] GetAllCodes(Variable variable)
        {
            var codes = variable.Values.Take(variable.Values.Count).Select(value => value.Code).ToArray();

            return codes;
        }

        private string[] GetTimeCodes(Variable variable, int count)
        {
            var lstCodes = variable.Values.TakeLast(count).Select(value => value.Code).ToList();
            lstCodes.Sort((a, b) => b.CompareTo(a)); // Descending sort
            var codes = lstCodes.ToArray();

            return codes;
        }

        private bool HasSelection(VariablesSelection selection)
        {
            if (selection.Selection.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Problem NonExistentVariable()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Non-existent variable";
            return p;
        }

        private Problem NonExistentValue()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Non-existent value";
            return p;
        }

        private Problem MissingSelection()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Missing selection for mandantory variable";
            return p;
        }

    }
}
