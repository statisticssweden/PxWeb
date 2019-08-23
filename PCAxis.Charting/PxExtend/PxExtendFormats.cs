using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PCAxis.Paxiom;
using System.IO;
using PCAxis.PxExtend;
using System.Globalization;

namespace PCAxis.PxExtend
{
	/// <summary>
	/// Extension methods for creating formats of data from a PXModel instance.
	/// </summary>
	public static class PxExtendFormats
	{
		/// <summary>
		/// Creates a CSV string from a PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public static string AsCSV(this PXModel px)
		{
			StringBuilder sb = new StringBuilder();

			DataTable table = px.AsDataTable(false, true);

			foreach (DataRow r in table.Rows)
				sb.AppendLine(String.Join(";", r.ItemArray.Select(x => x.ToString()).ToArray()));

			return sb.ToString();
		}

		/// <summary>
		/// Creates a DataTable from a PXModel instance.
		/// Includes values in datatable.
		/// Does not include codes in datatable.
		/// Does not include columns with info about codes and texts.
		/// This overload should be removed when we move to 4.0 - defaults used on non-obsolete overload.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		//TODO: 4.0: Remove this overload when migrating to 4.0
		public static DataTable AsDataTable(this PXModel px)
		{
			return px.AsDataTable(false, false);
		}

		/// <summary>
		/// Creates a DataTable from a PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="addExtraInfoColumns">Include columns with info about codes and texts (default is false)</param>
		/// <param name="storeDataAsFormattedString">Whether to store data as a formatted string according to the rules in the model - if not the value is converted back to Double (default is false)</param>
		/// <returns></returns>
		//public static DataTable AsDataTable(this PXModel px, bool showValues = true, bool showCodes = false, bool addExtraInfoColumns = false)
		//TODO: 4.0: Implement defaults when migrating to 4.0
		public static DataTable AsDataTable(this PXModel px, bool addExtraInfoColumns, bool storeDataAsFormattedString)
		{
			if (!px.IsComplete)
				throw new PxExtendExceptions.ModelNotReadyException("PXModel must be complete before converting it.");

			if (px.Meta.GetPreferredLanguage() == null)
				px.Meta.SetPreferredLanguage(px.Meta.Language);

			List<Variable> variables = px.Meta.Variables;

			DataTable table = new DataTable();

			table.Columns.AddRange(
				(
					from v in variables
					select new DataColumn(v.Name)
				).ToArray()
			);

			table.Columns.Add(
				new DataColumn("DATA")
			);

			if (addExtraInfoColumns)
				table.Columns.AddRange(
					new DataColumn[]
					{
						new DataColumn("CODES"),
						new DataColumn("TEXT")
					}
				);

			int rows = 1;
			foreach (Variable variable in variables)
				rows *= variable.Values.Count;

			for (int i = 0; i < rows; i++)
				table.Rows.Add(table.NewRow());

			int repeat = 1;
			for (int c = variables.Count - 1; c >= 0; c--)
			{
				Value[] data = px.expandVariable(variables[c], rows, repeat);

				for (int r = 0; r < rows; r++)
				{
					table.Rows[r][c] = data[r].Text ?? data[r].Value;

					if (addExtraInfoColumns)
					{
						table.Rows[r][variables.Count + 1] = variables[c].Name + "|" + (data[r].Code ?? data[r].Text) + (c < variables.Count - 1 ? "||" : "") + table.Rows[r][variables.Count + 1];
						table.Rows[r][variables.Count + 2] = data[r].Text + (c < variables.Count - 1 ? ", " : "") + table.Rows[r][variables.Count + 2];
					}
				}

				repeat *= variables[c].Values.Count;
			}

			DataFormatter df = new DataFormatter(px);

			NumberFormatInfo format =
				new NumberFormatInfo() { NumberGroupSeparator = df.ThousandSeparator, NumberDecimalSeparator = df.DecimalSeparator };

			int counter = 0;
			for (int r = 0; r < px.Data.MatrixRowCount; r++)
			{
				for (int c = 0; c < px.Data.MatrixColumnCount; c++)
				{
					if (storeDataAsFormattedString)
						table.Rows[counter++][variables.Count] = df.ReadElement(r, c);
					else
					{
						double d;

						bool parseSuccess =
							Double.TryParse(
								df.ReadElement(r, c),
								NumberStyles.Number,
								format,
								out d
							);

						if (parseSuccess)
							table.Rows[counter++][variables.Count] = d;
						else
							table.Rows[counter++][variables.Count] = df.ReadElement(r, c);
					}
				}
			}

			return table;
		}

		private static Value[] expandVariable(this PXModel px, Variable variable, int numberOfRows, int repeat)
		{
			List<Value> rows = new List<Value>();

			do
			{
				foreach (Value value in variable.Values)
				{
					for (int i = 0; i < repeat; i++)
						rows.Add(value);
				}
			} while (rows.Count < numberOfRows);

			return rows.ToArray();
		}

		/// <summary>
		/// Creates a DataTable from a PXModel instance. Joins the columns for series name if several.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public static DataTable AsDataTableCompressed(this PXModel px)
		{
			return AsDataTableCompressed(px, null, null);
		}

		/// <summary>
		/// Creates a DataTable from a PXModel instance. Joins the columns for series name if several.
		/// This overload includes conversion of data for a specific variable/value to negative numbers for usage with population chart.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="variableName">Variable for which data for a specific value is converted to negative numbers. If null, value can only be present for one variable.</param>
		/// <param name="valueTextOrValue">Value for which data is converted to negative numbers.</param>
		/// <returns></returns>
		public static DataTable AsDataTableCompressed(this PXModel px, string variableName, string valueTextOrValue)
		{
			var dt = px.AsDataTable(true, false);

			if (valueTextOrValue != null)
			{
				var columnsWithValue =
					dt.Columns.OfType<DataColumn>()
						.Where(
							column =>
								dt.Rows.OfType<DataRow>()
									.Select(row => row.Field<object>(column))
									.Any(o => o is string && o.ToString() == valueTextOrValue)
						)
						.ToArray();

				if (columnsWithValue.Count() != 1)
					throw
						variableName == null
							? new PxExtendExceptions.ConvertToNegativeValuesException("Supplied value must be present for one variable og variable must be suplied.")
							: new PxExtendExceptions.ConvertToNegativeValuesException("Supplied variable/value-combination must be present.");

				variableName = variableName ?? columnsWithValue.Single().ColumnName;
			}

			var table = new DataTable();

			table.Columns.AddRange(
				new[]
				{
					new DataColumn("Series"),
					new DataColumn("Label"),
					new DataColumn("DATA", typeof(double)),
					new DataColumn("CODES"),
					new DataColumn("TEXT")
				}
			);

			foreach (DataRow row in dt.Rows)
			{
				double d;

				object data;

				if (Double.TryParse(row.ItemArray.Take(row.ItemArray.Length - 2).Last().ToString(), out d))
				{
					if (variableName == null)
					{
						data = d;
					}
					else
					{
						if (d < 0)
						{
							throw new PxExtendExceptions.ConvertToNegativeValuesException("Negative values not allowed, when conversion enabled.");
						}

						data =
							row.Field<string>(variableName) == valueTextOrValue
								? d * -1
								: d;
					}
				}
				else
				{
					data = DBNull.Value;
				}

				var r = table.NewRow();
				r[0] = row.ItemArray.Length < 5 ? "Series 1" : String.Join(", ", row.ItemArray.Take(row.ItemArray.Length - 4).Select(x => x.ToString()).ToArray());
				r[1] = row.ItemArray.Length < 5 ? row.ItemArray.First() : row.ItemArray[row.ItemArray.Length - 4];
				r[2] = data;
				r[3] = row.ItemArray.Take(row.ItemArray.Length - 1).Last();
				r[4] = row.ItemArray.Last();

				table.Rows.Add(r);
			}

			return table;
		}
	}
}
