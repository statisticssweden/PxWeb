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
        /// <summary>
        /// Get Selection-array for the wanted variables and values
        /// </summary>
        /// <param name="model">Paxiom model</param>
        /// <param name="variablesSelection">VariablesSelection object describing wanted variables and values</param>
        /// <returns></returns>
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

        /// <summary>
        /// Verify that VariablesSelection object has valid variables and values
        /// </summary>
        /// <param name="model">Paxiom model</param>
        /// <param name="variablesSelection">The VariablesSelection object to verify</param>
        /// <param name="problem">Null if everything is ok, otherwise it describes whats wrong</param>
        /// <returns></returns>
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
                if (!VerifyVariableValues(model, variablesSelection, out problem))
                {
                    return false;                
                }
            }

            return true;
        }

        /// <summary>
        /// Verify that the wanted variable values has valid codes
        /// </summary>
        /// <param name="model">Paxiom model</param>
        /// <param name="variablesSelection">VariablesSelection with the wanted variables and values</param>
        /// <param name="problem">Will be null if everything is ok, oterwise it will describe the problem</param>
        /// <returns></returns>
        private bool VerifyVariableValues(PXModel model, VariablesSelection variablesSelection, out Problem? problem)
        {
            problem = null;

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
                    var modelVariable = model.Meta.Variables.GetByCode(variable.VariableCode);
                    foreach (var value in variable.ValueCodes)
                    {
                        if (!IsSelectionExpression(value))
                        {
                            if (modelVariable.Values.GetByCode(value) == null)
                            {
                                problem = NonExistentValue();
                                return false;
                            }
                        }
                        else
                        {
                            if (!VerifySelectionExpression(value))
                            {
                                problem = IllegalSelectionExpression();
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Verifies that a selection expression is valid
        /// </summary>
        /// <param name="expression">The selection expression to verify</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifySelectionExpression(string expression)
        {
            if (expression.Contains('*'))
            { 
                return VerifyWildcardStarExpression(expression);
            }
            else if (expression.Contains('?'))
            {
                return VerifyWildcardQuestionmarkExpression(expression);
            }

            return false;
        }

        /// <summary>
        /// Verifies that the wildcard * selection expression is valid
        /// </summary>
        /// <param name="expression">The wildcard selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyWildcardStarExpression(string expression)
        {
            int count = expression.Count(c => c == '*');

            if (count > 2)
            {
                // More than 2 * is not allowed
                return false;
            }

            if ((count == 1) && !(expression.StartsWith('*') || expression.EndsWith('*')))
            {
                // * must be in the beginning or end of the value
                return false;
            }

            if ((count == 2) && !(expression.StartsWith('*') && expression.EndsWith('*')))
            {
                // The * must be in the beginning and the end of the value
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies that the wildcard ? selection expression is valid
        /// </summary>
        /// <param name="expression">The wildcard selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyWildcardQuestionmarkExpression(string expression)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the value string is a selection expression, else false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsSelectionExpression(string value)
        {
            return value.Contains('*') || value.Contains('?');
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
                var variable = model.Meta.Variables.GetByCode(varSelection.VariableCode);
                selections.Add(GetSelection(variable, varSelection));
            }

            return selections.ToArray();
        }

        /// <summary>
        /// Add all values for variable
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="varSelection">VariableSelection object with wanted values from user</param>
        /// <returns></returns>
        private Selection GetSelection(Variable variable, VariableSelection varSelection)
        {
            var selection = new Selection(varSelection.VariableCode);
            var values = new List<string>();

            foreach(var value in varSelection.ValueCodes)
            {
                if (value.Contains('*'))
                {
                    AddWildcardStarValues(variable, values, value);
                }
                else if (value.Contains('?'))
                {
                    AddWildcardQuestionmarkValues(variable, values, value);
                }
                else if (!values.Contains(value))
                {
                    values.Add(value);
                }
            }

            selection.ValueCodes.AddRange(values.ToArray());
            return selection;
        }

        /// <summary>
        /// Add values for variable based on wildcard * selection
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="wildcard">The wildcard string</param>
        private void AddWildcardStarValues(Variable variable, List<string> values, string wildcard)
        {
            if (wildcard.StartsWith("*") && wildcard.EndsWith("*"))
            {
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
        /// Add values for variable based on wildcard ? selection
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="wildcard">The wildcard string</param>
        private void AddWildcardQuestionmarkValues(Variable variable, List<string> values, string wildcard)
        {
            // Value codes must have the same length as wildcard
            var variableValues = variable.Values.Where(v => v.Code.Length.Equals(wildcard.Length)).Select(v => v.Code);
            foreach (var variableValue in variableValues)
            {
                if (!values.Contains(variableValue))
                {
                    values.Add(variableValue);
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

        private Problem IllegalSelectionExpression()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Illegal selection expression";
            return p;
        }

    }
}
