using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace PXWeb.Misc
{
    /// <summary>
    /// Contains methods for validating input fields
    /// </summary>
    public class InputValidation
    {
        /// <summary>
        /// Regular expression for color code (#AAAAAA)
        /// </summary>
        private static Regex _regexColorCode = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
        /// <summary>
        /// Regular expression for times  10:15,11:15
        /// </summary>
        private static Regex _regTimes = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
        //new Regex(@"(?!=^|,)(2[0-3]|1[0-9]|0[0-9]|[^0-9][0-9]):([0-5][0-9]|[0-9])$");//(@"^-*[0-9,\:]+$");
        //^(2[0-3]|1[0-9]|0[0-9]|[^0-9][0-9]):([0-5][0-9]|[0-9])(?:,|$)$
        //(?!=^|,)(2[0-3]|1[0-9]|0[0-9]|[^0-9][0-9]):([0-5][0-9]|[0-9])(?!=^|,)$
        /// <summary>
        /// Validates that value is a integer > 0. 
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="keyErrorMessage">Contains key for error message if value is not valid</param>
        /// <returns>True if value is valid, else false</returns>
        public static bool ValidateMandatoryPositiveInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args, out string keyErrorMessage)
        {
            CustomValidator val = (CustomValidator)source;
            int value;
            long lvalue;

            if (args.Value.Length == 0)
            {
                keyErrorMessage = "PxWebAdminSettingsValidationMandatorySetting";
                return false;
            }

            if (!int.TryParse(args.Value, out value))
            {
                if (long.TryParse(args.Value, out lvalue))
                {
                    keyErrorMessage = "PxWebAdminSettingsValidationTooLargeIntegerValue";
                    return false;
                }
                else
                {
                    keyErrorMessage = "PxWebAdminSettingsValidationIntegerValue";
                    return false;
                }
            }

            if (value < 1)
            {
                keyErrorMessage = "PxWebAdminSettingsValidationGreaterThanZero";
                return false;
            }


            keyErrorMessage = "";
            return true;
        }
        /// <summary>
        /// Validates that value is a integer >= 0. 
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="keyErrorMessage">Contains key for error message if value is not valid</param>
        /// <returns>True if value is valid, else false</returns>
        public static bool ValidatePositiveInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args, out string keyErrorMessage)
        {
            CustomValidator val = (CustomValidator)source;
            int value;
            long lvalue;

            if (!int.TryParse(args.Value, out value))
            {
                if (long.TryParse(args.Value, out lvalue))
                {
                    keyErrorMessage = "PxWebAdminSettingsValidationTooLargeIntegerValue";
                    return false;
                }
                else
                {
                    keyErrorMessage = "PxWebAdminSettingsValidationIntegerValue";
                    return false;
                }
            }

            if (value < 0)
            {
                keyErrorMessage = "PxWebAdminSettingsValidationGreaterThanZero";
                return false;
            }


            keyErrorMessage = "";
            return true;
        }
        /// <summary>
        /// Validates that value does not contain any illegal charcters
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="keyErrorMessage">Contains key for error message if value is not valid</param>
        /// <returns>True if value is valid, else false</returns>
        public static bool ValidateNoIllegalCharcters(object source, System.Web.UI.WebControls.ServerValidateEventArgs args, out string keyErrorMessage)
        {
            CustomValidator val = (CustomValidator)source;

            if ((args.Value.IndexOfAny("<>&?".ToCharArray()) != -1) || (!PCAxis.Web.Core.Management.ValidationManager.CheckValue(args.Value)))
            {
                keyErrorMessage = "PxWebIllegalCharactersErrorMessage";
                return false;
            }

            keyErrorMessage = "";
            return true;
        }

        public static bool ValidateNoIllegalCharcters(string inputText, out string keyErrorMessage)
        {
            if (inputText.IndexOfAny("<>&?".ToCharArray()) != -1 || inputText.StartsWith("*") || !PCAxis.Web.Core.Management.ValidationManager.CheckValue(inputText))
            {
                keyErrorMessage = "PxWebIllegalCharactersErrorMessage";
                return false;
            }

            keyErrorMessage = "";
            return true;
        }


        /// <summary>
        /// Validates that value is a legal color code (of format #AAAAAA)
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="keyErrorMessage">Contains key for error message if value is not valid</param>
        /// <returns>True if value is valid, else false</returns>
        public static bool ValidateColorCode(object source, System.Web.UI.WebControls.ServerValidateEventArgs args, out string keyErrorMessage)
        {
            CustomValidator val = (CustomValidator)source;

            Match match = _regexColorCode.Match(args.Value.ToLower());
            if (match.Success == false)
            {
                keyErrorMessage = "PxWebAdminSettingsValidationIllegalColorCode";
                return false;
            }

            keyErrorMessage = "";
            return true;
        }

        /// <summary>
        /// Validates that the string contains only correctly formatted times (hh:mm ex 23:00,02:30)
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="keyErrorMessage">Contains key for error message if value is not valid</param>
        /// <returns>True if value is valid, else false</returns>
        public static bool ValidateStringOfTimes(object source, System.Web.UI.WebControls.ServerValidateEventArgs args, out string keyErrorMessage)
        {
            char[] separators = new char[] { ',' }; // Comma and semicolon are allowed as separators
            string times = args.Value.ToLower();
            string[] parts = times.Split(separators);

            foreach (var time in parts)
            {
                if (!_regTimes.IsMatch(time.Trim()))
                {
                    keyErrorMessage = "PxWebAdminSettingsValidationIllegalTimeSeries";
                    return false;
                }
            }

            keyErrorMessage = "";
            return true;
        }
    }
}
