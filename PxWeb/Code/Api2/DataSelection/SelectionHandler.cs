using Lucene.Net.Util;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PxWeb.Code.Api2.DataSelection
{
    public class SelectionHandler : ISelectionHandler
    {
        // Regular expressions for selection expression validation

        // TOP(xxx), TOP(xxx,yyy), top(xxx) and top(xxx,yyy)
        private static string REGEX_TOP = "^(TOP\\([1-9]\\d*\\)|TOP\\([1-9]\\d*,[1-9]\\d*\\))$";

        // BOTTOM(xxx), BOTTOM(xxx,yyy), bottom(xxx) and bottom(xxx,yyy)
        private static string REGEX_BOTTOM = "^(BOTTOM\\([1-9]\\d*\\)|BOTTOM\\([1-9]\\d*,[1-9]\\d*\\))$";

        // RANGE(xxx,yyy) and range(xxx,yyy)
        private static string REGEX_RANGE = "^(RANGE\\(([^,]+)\\d*,([^,)]+)\\d*\\))$";

        // FROM(xxx) and from(xxx)
        private static string REGEX_FROM = "^(FROM\\(([^,]+)\\d*\\))$";

        // TO(xxx) and to(xxx)
        private static string REGEX_TO = "^(TO\\(([^,]+)\\d*\\))$";

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
            else if (expression.StartsWith("TOP(", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return VerifyTopExpression(expression);
            }
            else if (expression.StartsWith("BOTTOM(", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return VerifyBottomExpression(expression);
            }
            else if (expression.StartsWith("RANGE(", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return VerifyRangeExpression(expression);
            }
            else if (expression.StartsWith("FROM(", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return VerifyFromExpression(expression);
            }
            else if (expression.StartsWith("TO(", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return VerifyToExpression(expression);
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
            if (expression.Equals("*"))
            {
                return true; 
            }

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
            // What could be wrong?
            return true;
        }

        /// <summary>
        /// Verifies that the TOP(xxx) or TOP(xxx,yyy) selection expression is valid
        /// </summary>
        /// <param name="expression">The TOP selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyTopExpression(string expression)
        {
            return Regex.IsMatch(expression, REGEX_TOP, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Verifies that the BOTTOM(xxx) or BOTTOM(xxx,yyy) selection expression is valid
        /// </summary>
        /// <param name="expression">The BOTTOM selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyBottomExpression(string expression)
        {
            return Regex.IsMatch(expression, REGEX_BOTTOM, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Verifies that the RANGE(xxx,yyy) selection expression is valid
        /// </summary>
        /// <param name="expression">The RANGE selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyRangeExpression(string expression)
        {
            return Regex.IsMatch(expression, REGEX_RANGE, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Verifies that the FROM(xxx) selection expression is valid
        /// </summary>
        /// <param name="expression">The FROM selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyFromExpression(string expression)
        {
            return Regex.IsMatch(expression, REGEX_FROM, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Verifies that the TO(xxx) selection expression is valid
        /// </summary>
        /// <param name="expression">The TO selection expression to validate</param>
        /// <returns>True if the expression is valid, else false</returns>
        private bool VerifyToExpression(string expression)
        {
            return Regex.IsMatch(expression, REGEX_TO, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns true if the value string is a selection expression, else false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsSelectionExpression(string value)
        {
            return value.Contains('*') || 
                   value.Contains('?') || 
                   value.StartsWith("TOP(", System.StringComparison.InvariantCultureIgnoreCase) ||
                   value.StartsWith("BOTTOM(", System.StringComparison.InvariantCultureIgnoreCase) ||
                   value.StartsWith("RANGE(", System.StringComparison.InvariantCultureIgnoreCase) ||
                   value.StartsWith("FROM(", System.StringComparison.InvariantCultureIgnoreCase) ||
                   value.StartsWith("TO(", System.StringComparison.InvariantCultureIgnoreCase);
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

            foreach (var value in varSelection.ValueCodes)
            {
                if (value.Contains('*'))
                {
                    AddWildcardStarValues(variable, values, value);
                }
                else if (value.Contains('?'))
                {
                    AddWildcardQuestionmarkValues(variable, values, value);
                }
                else if (value.StartsWith("TOP(", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    AddTopValues(variable, values, value);
                }
                else if (value.StartsWith("BOTTOM(", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    AddBottomValues(variable, values, value);
                }
                else if (value.StartsWith("RANGE(", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    AddRangeValues(variable, values, value);
                }
                else if (value.StartsWith("FROM(", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    AddFromValues(variable, values, value);
                }
                else if (value.StartsWith("TO(", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    AddToValues(variable, values, value);
                }
                else if (!values.Contains(value))
                {
                    values.Add(value);
                }
            }

            var sortedValues = SortValues(variable, values);

            selection.ValueCodes.AddRange(sortedValues.ToArray());
            return selection;
        }

        /// <summary>
        /// Sort selected values so that they appear in the same order as in the Paxiom variable
        /// </summary>
        /// <param name="variable">The Paxiom variable</param>
        /// <param name="values">Unsorted list of selected values</param>
        /// <returns>Sorted list of selected values</returns>
        private List<string> SortValues(Variable variable, List<string> values)
        {
            var sortedValues = new List<string>();

            foreach (var value in variable.Values)
            {
                if (values.Contains(value.Code))
                {
                    sortedValues.Add(value.Code);
                }
            }

            return sortedValues;    
        }

        /// <summary>
        /// Add values for variable based on wildcard * selection. * represents 0 to many characters.
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="wildcard">The wildcard string</param>
        private void AddWildcardStarValues(Variable variable, List<string> values, string wildcard)
        {
            if (wildcard.Equals("*"))
            {
                // Select all values
                var variableValues = variable.Values.Select(v => v.Code);
                foreach (var variableValue in variableValues)
                {
                    if (!values.Contains(variableValue))
                    {
                        values.Add(variableValue);
                    }
                }
            }
            else if (wildcard.StartsWith("*") && wildcard.EndsWith("*"))
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
        /// Add values for variable based on wildcard ? selection. ? reperesent any 1 character.
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="wildcard">The wildcard string</param>
        private void AddWildcardQuestionmarkValues(Variable variable, List<string> values, string wildcard)
        {
            string regexPattern = string.Concat("^", Regex.Escape(wildcard).Replace("\\?", "."), "$");
            var variableValues = variable.Values.Where(v => Regex.IsMatch(v.Code, regexPattern)).Select(v => v.Code);
            foreach (var variableValue in variableValues)
            {
                if (!values.Contains(variableValue))
                {
                    values.Add(variableValue);
                }
            }
        }

        /// <summary>
        /// Add values for variable based on TOP(xxx) and TOP(xxx,yyy) selection expression. 
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="expression">The TOP selection expression string</param>
        private void AddTopValues(Variable variable, List<string> values, string expression)
        {
            int count;
            int offset;

            if (!GetCountAndOffset(expression, out count, out offset))
            {
                return; // Something went wrong
            }

            var codes = variable.Values.Select(value => value.Code).ToArray();

            if (variable.IsTime)
            {
                codes.Sort((a, b) => b.CompareTo(a)); // Descending sort
            }

            for (int i = (0 + offset); i < (count + offset); i++)
            {
                if (i < codes.Length && !values.Contains(codes[i]))
                {
                    values.Add(codes[i]);
                }
            }
        }

        /// <summary>
        /// Add values for variable based on BOTTOM(xxx) and BOTTOM(xxx,yyy) selection expression. 
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="expression">The BOTTOM selection expression string</param>
        private void AddBottomValues(Variable variable, List<string> values, string expression)
        {
            int count;
            int offset;

            if (!GetCountAndOffset(expression, out count, out offset))
            {
                return; // Something went wrong
            }

            var codes = variable.Values.Select(value => value.Code).ToArray();

            if (variable.IsTime)
            {
                codes.Sort((a, b) => b.CompareTo(a)); // Descending sort
            }

            if (codes.Length - offset > 0)
            {
                int startIndex = codes.Length - offset - 1;
                int endIndex = codes.Length - offset - count;

                for (int i = startIndex; i >= endIndex; i--)
                {
                    if (i >= 0 && !values.Contains(codes[i]))
                    {
                        values.Add(codes[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Add values for variable based on RANGE(xxx,yyy) selection expression. 
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="expression">The RANGE selection expression string</param>
        private void AddRangeValues(Variable variable, List<string> values, string expression)
        {
            string code1 = "";
            string code2 = "";

            if (!GetRangeCodes(expression, out code1, out code2))
            {
                return; // Something went wrong
            }

            var codes = variable.Values.Select(value => value.Code).ToArray();

            if (variable.IsTime)
            {
                codes.Sort((a, b) => b.CompareTo(a)); // Descending sort
            }

            int index1 = Array.IndexOf(codes, code1);
            int index2 = Array.IndexOf(codes, code2);

            if (index1 > -1 && index2 > -1 && index2 > index1)
            {
                for (int i = index1; i <= index2; i++)
                {
                    if (!values.Contains(codes[i]))
                    {
                        values.Add(codes[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Add values for variable based on FROM(xxx) selection expression. 
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="expression">The FROM selection expression string</param>
        private void AddFromValues(Variable variable, List<string> values, string expression)
        {
            string code = "";

            if (!GetSingleCode(expression, out code))
            {
                return; // Something went wrong
            }

            var codes = variable.Values.Select(value => value.Code).ToArray();

            if (variable.IsTime)
            {
                codes.Sort((a, b) => a.CompareTo(b)); // Ascending sort
            }

            int index1 = Array.IndexOf(codes, code);

            if (index1 > -1)
            {
                for (int i = index1; i < codes.Length; i++)
                {
                    if (!values.Contains(codes[i]))
                    {
                        values.Add(codes[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Add values for variable based on TO(xxx) selection expression. 
        /// </summary>
        /// <param name="variable">Paxiom variable</param>
        /// <param name="values">List that the values shall be added to</param>
        /// <param name="expression">The TO selection expression string</param>
        private void AddToValues(Variable variable, List<string> values, string expression)
        {
            string code = "";

            if (!GetSingleCode(expression, out code))
            {
                return; // Something went wrong
            }

            var codes = variable.Values.Select(value => value.Code).ToArray();

            if (variable.IsTime)
            {
                codes.Sort((a, b) => a.CompareTo(b)); // Ascending sort
            }

            int index = Array.IndexOf(codes, code);

            if (index > -1)
            {
                for (int i = 0; i <= index; i++)
                {
                    if (!values.Contains(codes[i]))
                    {
                        values.Add(codes[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts the count and offset from selection expressions like TOP(count), TOP(count,offset), BOTTOM(count), BOTTOM(count,offset)
        /// </summary>
        /// <param name="expression">The selection expression to extract count and offset from</param>
        /// <param name="count">Set to the count value if it could be extracted, else 0</param>
        /// <param name="offset">Set to the offset value if it could be extracted, else 0</param>
        /// <returns>True if values could be extracted, false if something went wrong</returns>
        private bool GetCountAndOffset(string expression, out int count, out int offset)
        {
            count = 0;
            offset = 0;

            try
            {
                int firstParanteses = expression.IndexOf('(');

                if (firstParanteses == -1)
                {
                    return false;
                }

                string strNumbers = expression.Substring(firstParanteses + 1, expression.Length - (firstParanteses + 2)); // extract the numbers part of TOP(xxx) or TOP(xxx,yyy)
                string[] numbers = strNumbers.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

                if (!int.TryParse(numbers[0], out count))
                {
                    return false; // Something went wrong
                }

                if (numbers.Length == 2)
                {
                    if (!int.TryParse(numbers[1], out offset))
                    {
                        return false; // Something went wrong
                    }
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts code1 and code2 from RANGE selection expressions like RANGE(xxx,yyy)
        /// </summary>
        /// <param name="expression">The Range selection expression to extract codes from</param>
        /// <param name="code1">The firts code</param>
        /// <param name="code2">The second code</param>
        /// <returns>True if the codes could be extracted, false if something went wrong</returns>
        private bool GetRangeCodes(string expression, out string code1, out string code2)
        {
            code1 = "";
            code2 = "";

            try
            {
                int firstParanteses = expression.IndexOf('(');

                if (firstParanteses == -1)
                {
                    return false;
                }

                string strCodes = expression.Substring(firstParanteses + 1, expression.Length - (firstParanteses + 2)); // extract the codes
                string[] codes = strCodes.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

                if (codes.Length != 2)
                {
                    return false;
                }

                code1 = codes[0];
                code2 = codes[1];

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts the code from selection expressions like FROM(xxx) or TO(xxx)
        /// </summary>
        /// <param name="expression">The Range selection expression to extract the code from</param>
        /// <param name="code">The code</param>
        /// <returns>True if teh code could be extracted, false if something went wrong</returns>
        private bool GetSingleCode(string expression, out string code)
        {
            code = "";

            try
            {
                int firstParanteses = expression.IndexOf('(');

                if (firstParanteses == -1)
                {
                    return false;
                }

                code = expression.Substring(firstParanteses + 1, expression.Length - (firstParanteses + 2)); // extract the code

                return true;
            }
            catch (System.Exception)
            {
                return false;
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
