using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;

namespace PCAxis.PxExtend
{
	/// <summary>
	/// Static methods end extension methods for "shortcutting" to PX funcationality.
	/// </summary>
	public static class PxExtend
	{
		/// <summary>
		/// Creates a PXModel intance from a file.
		/// </summary>
		/// <param name="filePath">Path to PX-file.</param>
		/// <returns></returns>
		public static PXModel CreatePxModel(string filePath)
		{
			return CreatePxModel(filePath, true);
		}

		/// <summary>
		/// Creates a PXModel intance from a file.
		/// </summary>
		/// <param name="filePath">Path to PX-file.</param>
		/// <param name="loadData">Whether to load data. False = only meta data.</param>
		/// <returns></returns>
		public static PXModel CreatePxModel(string filePath, bool loadData)
		{
			return CreatePxModel(new FileInfo(filePath), loadData, null, null, null);
		}

		/// <summary>
		/// Creates a PXModel instance from a file.
		/// </summary>
		/// <param name="file">PX-file.</param>
		/// <param name="loadData">Whether to load data. False = only meta data.</param>
		/// <param name="except">Array of selections NOT to be included. Can be null.</param>
		/// <param name="explodeVariableCode">Variable code for variable to show one value at a time. Can be null.</param>
		/// <param name="explodeValueCode">Value code for the value to select for the variable explodeVariableCode. Can be null.</param>
		/// <returns></returns>
		public static PXModel CreatePxModel(FileInfo file, bool loadData, Selection[] except, string explodeVariableCode, string explodeValueCode)
		{
			PXFileBuilder builder = new PXFileBuilder();
			builder.SetPath(file.FullName);
			builder.BuildForSelection();

			if (loadData)
			{
				Selection[] selections = Selection.SelectAll(builder.Model.Meta);

				if (except != null)
					selections = selections.Except(except);

				if (explodeVariableCode != null && explodeValueCode != null)
					selections = selections.Explode(explodeVariableCode, explodeValueCode);

				builder.BuildForPresentation(selections);
			}

			if (builder.Model.Meta.GetPreferredLanguage() == null)
				builder.Model.Meta.SetPreferredLanguage(builder.Model.Meta.Language);

			return builder.Model;
		}

		/// <summary>
		/// Removes valuecodes from Selection instances in array.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="excepts">Selection instances containing valuecodes to remove from selections with same variable code.</param>
		/// <returns></returns>
		public static Selection[] Except(this Selection[] s, params Selection[] excepts)
		{
			if (excepts != null)
			{
				foreach (Selection except in excepts)
				{
					foreach (Selection selection in s.Where(x => x.VariableCode == except.VariableCode))
					{
						foreach (string valueCode in except.ValueCodes)
							selection.ValueCodes.Remove(valueCode);
					}
				}
			}

			return s;
		}

		/// <summary>
		/// Unselects all values but one for a specific variable.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="variableCode">Code of variable to select only one value from.</param>
		/// <param name="valueCode">Code of value to select for chosen variable.</param>
		/// <returns></returns>
		public static Selection[] Explode(this Selection[] s, string variableCode, string valueCode)
		{
			foreach (Selection selection in s)
			{
				if (selection.VariableCode == variableCode && selection.ValueCodes.Contains(valueCode))
				{
					selection.ValueCodes.Clear();
					selection.ValueCodes.Add(valueCode);
				}
			}

			return s;
		}

		/// <summary>
		/// Serializes PXModel instance to PX-file. Does not use keys format.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="path">Save to stream.</param>
		public static void Save(this PXModel px, string path)
		{
			px.Save(path, false);
		}

		/// <summary>
		/// Serializes PXModel instance to PX-file.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="path">Save to stream.</param>
		/// <param name="keysFormat">Whether to save in keys format.</param>
		public static void Save(this PXModel px, string path, bool keysFormat)
		{
			PXFileSerializer serializer = new PXFileSerializer();
			serializer.SerializeInKeysFormat = keysFormat;
			serializer.Serialize(px, path);
		}

		/// <summary>
		/// Serializes PXModel instance as PX-file to stream. Does not use keys format.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="stream">Save to stream.</param>
		public static void Save(this PXModel px, Stream stream)
		{
			px.Save(stream, false);
		}

		/// <summary>
		/// Serializes PXModel instance as PX-file to stream.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="stream">Save to stream.</param>
		/// <param name="keysFormat">Whether to save in keys format.</param>
		public static void Save(this PXModel px, Stream stream, bool keysFormat)
		{
			PXFileSerializer serializer = new PXFileSerializer();
			serializer.SerializeInKeysFormat = keysFormat;
			serializer.Serialize(px, stream);
		}

		/// <summary>
		/// Adds valuecode to selection. For chainability.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="valueCode">Valuecodes to add.</param>
		/// <returns></returns>
		public static Selection AddValueCode(this Selection s, params string[] valueCode)
		{
			s.ValueCodes.AddRange(valueCode);
			return s;
		}

		private static Sum sumInstance = null;
		private static Sum sum
		{
			get
			{
				if (sumInstance == null)
					sumInstance = new Sum();

				return sumInstance;
			}
		}

		/// <summary>
		/// Shorthand method for calculating a addition
		/// </summary>
		/// <param name="px"></param>
		/// <param name="newValueCode">New value code</param>
		/// <param name="newValueName">New value name</param>
		/// <param name="variableCode">The variable on which to perform the calculation</param>
		/// <param name="valueCodes">The codes to be included in the calculation</param>
		/// <returns></returns>
		public static PXModel CalcAddition(this PXModel px, string newValueCode, string newValueName, string variableCode, params string[] valueCodes)
		{
			return
				px.Calculate(SumOperationType.Addition, newValueCode, newValueName, variableCode, valueCodes);
		}

		/// <summary>
		/// Shorthand method for calculating a division
		/// </summary>
		/// <param name="px"></param>
		/// <param name="newValueCode">New value code</param>
		/// <param name="newValueName">New value name</param>
		/// <param name="variableCode">The variable on which to perform the calculation</param>
		/// <param name="valueCodes">The codes to be included in the calculation</param>
		/// <returns></returns>
		public static PXModel CalcDivision(this PXModel px, string newValueCode, string newValueName, string variableCode, params string[] valueCodes)
		{
			return
				px.Calculate(SumOperationType.Division, newValueCode, newValueName, variableCode, valueCodes);
		}

		/// <summary>
		/// Shorthand method for calculating a multiplication
		/// </summary>
		/// <param name="px"></param>
		/// <param name="newValueCode">New value code</param>
		/// <param name="newValueName">New value name</param>
		/// <param name="variableCode">The variable on which to perform the calculation</param>
		/// <param name="valueCodes">The codes to be included in the calculation</param>
		/// <returns></returns>
		public static PXModel CalcMultiplication(this PXModel px, string newValueCode, string newValueName, string variableCode, params string[] valueCodes)
		{
			return
				px.Calculate(SumOperationType.Multiplication, newValueCode, newValueName, variableCode, valueCodes);
		}

		/// <summary>
		/// Shorthand method for calculating a substraction
		/// </summary>
		/// <param name="px"></param>
		/// <param name="newValueCode">New value code</param>
		/// <param name="newValueName">New value name</param>
		/// <param name="variableCode">The variable on which to perform the calculation</param>
		/// <param name="valueCodes">The codes to be included in the calculation</param>
		/// <returns></returns>
		public static PXModel CalcSubstraction(this PXModel px, string newValueCode, string newValueName, string variableCode, params string[] valueCodes)
		{
			return
				px.Calculate(SumOperationType.Subtraction, newValueCode, newValueName, variableCode, valueCodes);
		}

		/// <summary>
		/// Make calculation on PXModel instance
		/// </summary>
		/// <param name="px"></param>
		/// <param name="operation">Type of calculation to perform</param>
		/// <param name="newValueCode">New value code</param>
		/// <param name="newValueName">New value name</param>
		/// <param name="variableCode">The variable on which to perform the calculation</param>
		/// <param name="valueCodes">The codes to be included in the calculation</param>
		/// <returns></returns>
		public static PXModel Calculate(this PXModel px, SumOperationType operation, string newValueCode, string newValueName, string variableCode, params string[] valueCodes)
		{
			return
				Calculate(
					px,
					new SumDescription()
					{
						DoEliminationForSumAll = true,
						KeepValues = true,
						Operation = operation,
						NewValueCode = newValueCode,
						NewValueName = newValueName,
						VariableCode = variableCode,
						ValueCodes = valueCodes.ToList()
					}
				);
		}

		/// <summary>
		/// Make calculation on PXModel instance
		/// </summary>
		/// <param name="px"></param>
		/// <param name="sumDescription">Instance of SumDescription describing the calculation</param>
		/// <returns></returns>
		public static PXModel Calculate(this PXModel px, SumDescription sumDescription)
		{
			return
				sum.Execute(px, sumDescription);
		}

		private static Pivot pivotInstance = null;
		private static Pivot pivot
		{
			get
			{
				if (pivotInstance == null)
					pivotInstance = new Pivot();

				return pivotInstance;
			}
		}
		/// <summary>
		/// Pivot a PXModel instance clockwise.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="times">Number of times to pivot.</param>
		/// <returns></returns>
		public static PXModel Pivot(this PXModel px, int times)
		{
			for (int i = 0; i < times; i++)
				px = pivot.PivotCW(px);

			return px;
		}

		/// <summary>
		/// Pivot a PXModel instance so that specific variable is the only variable in the head of the table.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="onlyHeadVariableName">Name of variable in head of table.</param>
		/// <returns></returns>
		public static PXModel PivotSpecificVariableToAloneInHead(this PXModel px, string onlyHeadVariableName)
		{
			var q = from v in px.Meta.Variables
					select new PivotDescription(v.Name, v.Name == onlyHeadVariableName ? PlacementType.Heading : PlacementType.Stub);

			return pivot.Execute(px, q.ToArray());
		}

		/// <summary>
		/// Pivot a PXModel instance
		/// </summary>
		/// <param name="px"></param>
		/// <param name="variableCodesInHead">Codes of variables to be placed in the table head</param>
		/// <returns></returns>
		public static PXModel Pivot(this PXModel px, params string[] variableCodesInHead)
		{
			return px.Pivot(variableCodesInHead, null);
		}

		/// <summary>
		/// Pivot a PXModel instance
		/// </summary>
		/// <param name="px"></param>
		/// <param name="variablesNamesInHead">Names of variables to be placed in the table head</param>
		/// <param name="variablesNamesInStub">Names of variables to be placed in the table stub</param>
		/// <returns></returns>
		public static PXModel Pivot(this PXModel px, IEnumerable<string> variablesNamesInHead, IEnumerable<string> variablesNamesInStub)
		{
			variablesNamesInHead = 
				variablesNamesInHead.Select(x => px.Meta.Variables.First(v => v.Name.ToLower() == x.ToLower()).Name).ToArray();

			variablesNamesInStub =
				variablesNamesInStub == null ? null : variablesNamesInStub.Select(x => px.Meta.Variables.First(v => v.Name.ToLower() == x.ToLower()).Name).ToArray();

			var h = from c in variablesNamesInHead
					select new PivotDescription(c, PlacementType.Heading);

			var s = from c in variablesNamesInStub ?? px.Meta.Variables.Select(v => v.Name).Except(variablesNamesInHead)
					select new PivotDescription(c, PlacementType.Stub);

			return pivot.Execute(px, h.Concat(s).ToArray());
		}

		private static Elimination elimination = null;
		/// <summary>
		/// Eliminates a variable from PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="variableCode">Code of variable to eliminate.</param>
		/// <returns></returns>
		public static PXModel Eliminate(this PXModel px, string variableCode)
		{
			if (elimination == null)
				elimination = new Elimination();

			Variable v = px.Meta.Variables.First(x => x.Code == variableCode);

			if (!v.Elimination && v.Values.Count > 1)
				throw new PxExtendExceptions.EliminatinNotAllowedException("Elimination of variable \"" + v.Name + "\" is not allowed.");

			return
				elimination.Execute(
					px,
					new EliminationDescription[]
					{
						new EliminationDescription(v.Code, v.EliminationValue != null)
					}
				);
		}

		/// <summary>
		/// Returns the variable of a PXModel instance that has the most values.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public static Variable MostValuesVariable(this PXModel px)
		{
			return
				(
					from v in px.Meta.Variables
					orderby v.Values.Count descending
					select v
				).First();
		}

		/// <summary>
		/// Returns whether the last of the variables in the table head is also the variable that has the most values.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public static bool HasMostValuesVariableLastInHead(this PXModel px)
		{
			return
				px.Meta.Heading.Count > 0 && px.Meta.Heading.Last() == px.MostValuesVariable();
		}

		/// <summary>
		/// Returns the values of a variable in a PXMeta instance. Returns the values of the time variable in reversed order.
		/// </summary>
		/// <param name="meta"></param>
		/// <param name="code">The code of the variable for which to return values.</param>
		/// <returns></returns>
		public static IEnumerable<Value> ValuesForVariable(this PXMeta meta, string code)
		{
			Variable variable = meta.Variables.First(x => x.Code == code);
			return variable.IsTime ? variable.Values.Reverse<Value>() : variable.Values;
		}

		/// <summary>
		/// Localizes title. For when the default PX way is not convenient.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="byWord">Local word for "by".</param>
		/// <param name="andWord">Local word for "and".</param>
		/// <returns></returns>
		public static void LocalizeTitle(this PXModel px, string byWord, string andWord)
		{
			string t = px.Meta.Title;

			t = t.Replace("PxcMetaTitleBy", byWord);
			t = t.Replace("PXCMETATITLEBY", byWord.ToUpper());
			t = t.Replace("pxcmetatitleby", byWord.ToLower());

			t = t.Replace("PxcMetaTitleAnd", andWord);
			t = t.Replace("PXCMETATITLEAND", andWord.ToUpper());
			t = t.Replace("pxcmetatitleand", andWord.ToLower());

			px.Meta.Title = t;
		}

		/// <summary>
		/// Localizes time texts according to the current UI Culture.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="quarter">String for quarter</param>
		/// <returns></returns>
		public static void LocalizeTimeTexts(this PXModel px, string quarter)
		{
			LocalizeTimeTexts(px, quarter, Thread.CurrentThread.CurrentUICulture);
		}

		/// <summary>
		/// Localizes time texts according to a UI Culture.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="quarter">String for quarter</param>
		/// <param name="culture">Culture for month names</param>
		/// <returns></returns>
		public static void LocalizeTimeTexts(this PXModel px, string quarter, CultureInfo culture)
		{
			DateTimeFormatInfo format = culture.DateTimeFormat;

			foreach (Variable timeVar in px.Meta.Variables.Where(v => v.IsTime))
			{
				List<Value> newValues = new List<Value>();

				foreach (Value v in timeVar.Values)
				{
					if (Regex.IsMatch(v.Text, @"\d{4}M\d{2}"))
						newValues.Add(
							new Value(
								String.Format(
									"{0} {1}",
									firstLetterToUpper(
										format.AbbreviatedMonthNames[Convert.ToInt32(v.Text.Substring(5, 2)) - 1]
									),
									v.Text.Substring(0, 4)
								)
							)
						);
					else if (Regex.IsMatch(v.Text, @"\d{4}K\d") || Regex.IsMatch(v.Text, @"\d{4}Q\d"))
						newValues.Add(
							new Value(
								String.Format(
									"{0}. {1} {2}",
									v.Text.Substring(5, 1),
									quarter,
									v.Text.Substring(0, 4)
								)
							)
						);
					else
						newValues.Add(v);
				}

				timeVar.Values.Clear();
				timeVar.Values.AddRange(newValues);
			}
		}

		private static string firstLetterToUpper(string s)
		{
			if (s.Length < 2)
				return s.ToUpper();

			return s.Substring(0, 1).ToUpper() + (s.Length > 1 ? s.Substring(1) : "");
		}
	}
}
