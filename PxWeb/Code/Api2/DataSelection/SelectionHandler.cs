using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using Lucene.Net.Util;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PxWeb.Code.Api2.DataSelection
{
    public class SelectionHandler : ISelectionHandler
    {
        public Selection[] GetSelection(PXModel model, VariablesSelection? variablesSelection)
        {
            if  (variablesSelection is not null && HasSelection(variablesSelection))
            {
                //Add variables that the user did not post
                variablesSelection = AddVariables(variablesSelection, model);

                //Map VariablesSelection to PCaxis.Paxiom.Selection[] 
                return MapCustomizedSelection(model, variablesSelection).ToArray();
            }
            else
            {
                return GetDefaultSelection(model);
            }

        }

        public bool Verify(PXModel model, VariablesSelection? variablesSelection, out Problem? problem)
        {
            problem = null;
            if (variablesSelection is not null && HasSelection(variablesSelection))
            {
                //Verify that variable exists
                foreach (var variable in variablesSelection.Selection)
                {
                    if (!model.Meta.Variables.Any(x => x.Code.ToUpper().Equals(variable.VariableCode.ToUpper())))
                    {
                        problem = NonExistentVariable();
                        return false;
                    }
                }

                //Verify that all the mandatory variables exists
                foreach (var mandatoryVariable in GetAllMandatoryVariables(model))
                {
                    if (!variablesSelection.Selection.Any(x => x.VariableCode.ToUpper().Equals(mandatoryVariable.Code.ToUpper())))
                    {
                        problem = MissingSelection();
                        return false;
                    }
                }

                //Verify variable values
                foreach (var variable in variablesSelection.Selection)
                {
                    //Verify that variables have at least one value selected for mandatory varibles
                    var mandatory = Mandatory(model, variable);
                    if (variable.ValueCodes.Count().Equals(0) && mandatory) 
                    {
                        problem = NonExistentValue();
                        return false;
                    }

                    //Check variable values if they exists in model.Metadata
                    if (!variable.ValueCodes.Count().Equals(0)) 
                    {
                        //var valueList = variable.ValueCodes[0].ToString().Split(',').ToList();
                        var modelVariable = model.Meta.Variables.GetByCode(variable.VariableCode);
                        //var modelVariableValues = model.Meta.Variables.Where(x => x.Code.Equals(variable.VariableCode)).Select(x => x.Values).ToList();
                        //foreach (var value in valueList)
                        foreach (var value in variable.ValueCodes)
                        {
                            if (!value.Contains("*"))
                            {
                                //if (!modelVariableValues.Any(x => x.Any(y => y.Code.Equals(value))))
                                if (modelVariable.Values.GetByCode(value) == null)
                                {
                                    problem = NonExistentValue();
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Add all varibles for a table
        /// </summary>
        /// <param name="variablesSelection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private VariablesSelection AddVariables(VariablesSelection variablesSelection, PXModel model)
        {
            foreach (var variable in model.Meta.Variables)
            {
                if (!variablesSelection.Selection.Any(x => x.VariableCode.ToUpper().Equals(variable.Code.ToUpper())))
                {
                    //Add variable
                    var variableSelectionObject = new VariableSelection
                    { 
                        VariableCode = variable.Code,
                        ValueCodes = new List<string>()
                    };

                    variablesSelection.Selection.Add(variableSelectionObject);
                }
            }
            
            return variablesSelection;
        }

        /// <summary>
        /// Map VariablesSelection to PCaxis.Paxiom.Selection[]
        /// </summary>
        /// <param name="variablesSelection"></param>
        /// <returns></returns>
        private Selection[] MapCustomizedSelection(PXModel model, VariablesSelection variablesSelection)
        {
            var selections = new List<Selection>();

            foreach (var varSelection in variablesSelection.Selection)
            {
                //var selection = new Selection(variable.VariableCode);

                ////Add values if they exist
                //if (!variable.ValueCodes.Count().Equals(0)) 
                //{
                //    //var valueList = variable.ValueCodes[0].ToString().Split(',').ToList();
                //    //selection.ValueCodes.AddRange(valueList.ToArray());
                //    selection.ValueCodes.AddRange(variable.ValueCodes.ToArray());
                //}
                var variable = model.Meta.Variables.GetByCode(varSelection.VariableCode);
                selections.Add(GetSelection(variable, varSelection));
            }

            return selections.ToArray();
        }

        private Selection GetSelection(Variable variable, VariableSelection varSelection)
        {
            var selection = new Selection(varSelection.VariableCode);
            var values = new List<string>();

            //Add values if they exist
            //if (!varSelection.ValueCodes.Count().Equals(0))
            //{
                
            //    selection.ValueCodes.AddRange(varSelection.ValueCodes.ToArray());
            //}

            foreach(var value in varSelection.ValueCodes)
            {
                if (value.Contains("*"))
                {
                    AddWildcardValues(variable, values, value);
                }
                else if (!values.Contains(value))
                {
                    values.Add(value);
                }
            }

            selection.ValueCodes.AddRange(values.ToArray());
            return selection;
        }

        private void AddWildcardValues(Variable variable, List<string> values, string wildcard)
        {
            if (wildcard.StartsWith("*") && wildcard.EndsWith("*"))
            {
                //var codeContains = wildcard.Substring(1, wildcard.Length - 2);
                var variableValues = variable.Values.Where(v => v.Code.Contains(wildcard.Substring(1, wildcard.Length - 2))).Select(v => v.Code);
                foreach (var variableValue in variableValues)
                {
                    if (!values.Contains(variableValue))
                    {
                        values.Add(variableValue);
                    }
                }
            }
            else if (wildcard.StartsWith("*"))
            {
                var variableValues = variable.Values.Where(v => v.Code.EndsWith(wildcard.Substring(1))).Select(v => v.Code);
                foreach (var variableValue in variableValues)
                {
                    if (!values.Contains(variableValue))
                    {
                        values.Add(variableValue);
                    }
                }
            }
            else if (wildcard.EndsWith("*"))
            {
                var variableValues = variable.Values.Where(v => v.Code.StartsWith(wildcard.Substring(0, wildcard.Length - 1))).Select(v => v.Code);
                foreach (var variableValue in variableValues)
                {
                    if (!values.Contains(variableValue))
                    {
                        values.Add(variableValue);
                    }
                }
            }
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

        private List<Variable> GetAllMandatoryVariables(PXModel model)
        {
            var mandatoryVariables = model.Meta.Variables.Where(x => x.Elimination.Equals(false)).ToList();
            return mandatoryVariables;
        }

        private bool Mandatory(PXModel model, VariableSelection variable)
        {
            bool mandatory = false;
            var mandatoryVariable = model.Meta.Variables.Where(x => x.Code.Equals(variable.VariableCode) && x.Elimination.Equals(false));
            
            if (mandatoryVariable.Count() != 0)
            {
                mandatory = true;
            }
            return mandatory;
        }



        private string[] GetCodes(Variable variable, int count)
        {
            var codes = variable.Values.Take(count).Select(value => value.Code).ToArray();

            return codes;
        }

        private string[] GetAllCodes(Variable variable)
        {
            var codes = variable.Values.Select(value => value.Code).ToArray();

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
