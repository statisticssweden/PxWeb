using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Excel
{
	public class XlsxSerializer : PCAxis.Paxiom.IPXModelStreamSerializer
	{

		public DoubleColumnType DoubleColumn { get; set; }
		public InformationLevelType InformationLevel { get; set; }

		private bool _showDataNoteCells = false;
		private DataNotePlacementType _modelDataNotePlacement;

		#region IPXModelStreamSerializer Members

		public void Serialize(PCAxis.Paxiom.PXModel model, System.IO.Stream stream)
		{
			serialize(model, stream);
		}

		public void Serialize(PCAxis.Paxiom.PXModel model, string path)
		{
			serialize(model, path);
		}

		private void serialize(PCAxis.Paxiom.PXModel model, object output)
		{
			XLWorkbook book = CreateWorkbook(model);

			AdditionalSheetFunctionality(book.Worksheet(model.Meta.Matrix), model);

			if (book != null)
			{
                book.Worksheet(model.Meta.Matrix).Columns().AdjustToContents(2, 2, 40);
				if (output is System.IO.Stream)
					book.SaveAs((System.IO.Stream)output);
				else if (output is string)
					book.SaveAs(output.ToString());
				book.Dispose();
			}
		}

		protected virtual void AdditionalSheetFunctionality(IXLWorksheet sheet, PXModel model)
		{
		}

		protected enum CellContentType
		{
			Undefined,
			Title,
			Code,
			Stub,
			Head,
			Data,
			Footnote,
			Info,
			DataNote,
			VariableNote,
			ValueNote,
			CellNote,
			Comment
		}

		protected delegate void FormatCellDescription(IXLCell cell);
		private void setCell(IXLCell cell, CellContentType type, object value, FormatCellDescription changes)
		{
            //cell.Style.NumberFormat.Format = "0.00";
			SetCellValue(cell, type, value);
			SetCellFormat(cell, type, value, changes);
		}

		protected virtual void SetCellValue(IXLCell cell, CellContentType type, object value)
		{
            if (value != null)
                if (type == CellContentType.Comment)
                    cell.Comment.AddText(value.ToString());
                else
                    cell.SetValue(value); //Change from cell.Value = value to SetValue(..) For not format e.g 10-11 to date
		}

		protected virtual void SetCellFormat(IXLCell cell, CellContentType type, object value, FormatCellDescription changes)
		{
			if (changes != null)
				changes(cell);
		}

		private XLWorkbook CreateWorkbook(PCAxis.Paxiom.PXModel model)
		{
			var book = new XLWorkbook();
			using (var sheet = book.Worksheets.Add(model.Meta.Matrix))
			{
				/*
				sheet.Cell(1, 1).Value = model.Meta.Title;
				sheet.Cell(1, 1).Style.Font.FontSize = 14;
				sheet.Cell(1, 1).Style.Font.Bold = true; 
				*/

				setCell(
					sheet.Cell(1, 1),
					CellContentType.Title,
					model.Meta.DescriptionDefault ? model.Meta.Description : model.Meta.Title,
					c => { c.Style.Font.FontSize = 14; c.Style.Font.Bold = true; }
				);

				DataFormatter fmt = new DataFormatter(model);
				fmt.ThousandSeparator = "";
				try
				{
					fmt.DecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				}
				catch (Exception)
				{
					fmt.DecimalSeparator = ",";
				}

				fmt.InformationLevel = InformationLevel;

				//Keep track of if the model has datanotecells
				_showDataNoteCells = (model.Meta.DataNoteCells.Count > 0);
				_modelDataNotePlacement = fmt.DataNotePlacment;

				if (_modelDataNotePlacement == DataNotePlacementType.None)
				{
					//Make sure we do not show any datanotecells
					_showDataNoteCells = false;
				}

				WriteHeading(model, sheet);

				string n = string.Empty;
				string dataNote = string.Empty;
				int row, column;
				string value;
				int dataNoteFactor = 1;
				int dataNoteValueOffset = 0;
				int dataNoteNoteOffset = 1;
				if (_showDataNoteCells)
				{
					dataNoteFactor = 2;
					if (_modelDataNotePlacement == DataNotePlacementType.Before)
					{
						dataNoteValueOffset = 1;
						dataNoteNoteOffset = 0;
					}
					else
					{
						dataNoteValueOffset = 0;
						dataNoteNoteOffset = 1;
					}
				}
				int sIndent = CalculateLeftIndentation(model);
				for (int i = 0; i < model.Data.MatrixRowCount; i++)
				{

					for (int k = 0; k < model.Meta.Stub.Count; k++)
					{
						GetStubCell(model, sheet, k, i, IsDoubleColumn(model.Meta.Stub[k]));
					}
					for (int j = 0; j < model.Data.MatrixColumnCount; j++)
					{
						row = 3 + model.Meta.Heading.Count + i;
						column = j * dataNoteFactor + sIndent + 1;
						value = fmt.ReadElement(i, j, ref n, ref dataNote);
                        //*Decimal valueDecimal = Decimal.Parse(value.ToString());
                        //*string valueDecimal = "70,0";
						//if (!value.IsNumeric())
						//{
						//    sheet.Cell(row, column + dataNoteValueOffset).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
						//    sheet.Cell(row, column + dataNoteValueOffset).Style.Fill.BackgroundColor = XLColor.LightSalmon;
						//}
						//else
						//{
						//    sheet.Cell(row, column + dataNoteValueOffset).DataType = XLCellValues.Number;
						//}
						//sheet.Cell(row, column + dataNoteValueOffset).Value = value;

                        setCell(
                            sheet.Cell(row, column + dataNoteValueOffset),                           
                            CellContentType.Data,
                            value,
                            !value.IsNumeric() ?
                                (FormatCellDescription)(c => { c.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right; c.Style.Fill.BackgroundColor = XLColor.LightSalmon; })
                                :
                                (FormatCellDescription)(c => { c.DataType = XLCellValues.Number; c.Style.NumberFormat.Format = FormatNumericCell(GetDecimalPrecision(value, fmt.DecimalSeparator)); })
                        );


						if (!string.IsNullOrEmpty(n))
						{
							//sheet.Cell(row, column + dataNoteValueOffset).Comment.AddText(n);
							setCell(
								sheet.Cell(row, column + dataNoteValueOffset),
								CellContentType.Comment,
								n,
								null
							);
						}

						if (_showDataNoteCells && !String.IsNullOrEmpty(dataNote))
						{
							//sheet.Cell(row, column + dataNoteNoteOffset).Value = dataNote;    
							setCell(
								sheet.Cell(row, column + dataNoteNoteOffset),
								CellContentType.DataNote,
								dataNote,
								null
							);
						}

					}

				}

				int r = model.Data.MatrixRowCount + model.Meta.Heading.Count + 4;
				//Writes the information
				if (InformationLevel > InformationLevelType.None)
				{
					r = WriteFootnotes(r, model, sheet);
					if (InformationLevel == InformationLevelType.AllInformation)
					{
						r = WriteTableInformation(r, model, sheet);
					}
				}
			}



			return book;

		}

		private int WriteFootnotes(int row, PXModel model, IXLWorksheet sheet)
		{
			if (InformationLevel <= InformationLevelType.None) return row;


			Note n;
			//Writes mandantory table notes
			for (int i = 0; i < model.Meta.Notes.Count; i++)
			{
				n = model.Meta.Notes[i];
				if ((n.Mandantory && InformationLevel == InformationLevelType.MandantoryFootnotesOnly) ||
					 InformationLevel > InformationLevelType.MandantoryFootnotesOnly)
				{
					//sheet.Cell(row, 1).Value = n.Text;
					setCell(
						sheet.Cell(row, 1),
						CellContentType.Footnote,
						n.Text,
						c => c.Style.Alignment.WrapText = true
					);
					//sheet.Cell(row, 1).Style.Alignment.WrapText = true;
					row++;

				}
			}

			//Writes mandantory variable notes
			Variable var;
			for (int i = 0; i < model.Meta.Variables.Count; i++)
			{
				var = model.Meta.Variables[i];
				if (var.HasNotes())
				{
					for (int j = 0; j < var.Notes.Count; j++)
					{
						n = var.Notes[j];
						if ((n.Mandantory && InformationLevel == InformationLevelType.MandantoryFootnotesOnly) ||
								InformationLevel > InformationLevelType.MandantoryFootnotesOnly)
						{
							//sheet.Cell(row, 1).Value = var.Name + ":";
							setCell(
								sheet.Cell(row, 1),
								CellContentType.VariableNote,
								var.Name + ":",
								null
							);
							row++;

							//sheet.Cell(row, 1).Value = n.Text;
							setCell(
								sheet.Cell(row, 1),
								CellContentType.VariableNote,
								n.Text,
								null
							);

							row += 2;
						}
					}
				}
			}

			//Writes mandantory value notes
			Value val;
			for (int i = 0; i < model.Meta.Variables.Count; i++)
			{
				var = model.Meta.Variables[i];
				for (int j = 0; j < var.Values.Count; j++)
				{
					val = var.Values[j];
					if (val.HasNotes())
					{
						for (int k = 0; k < val.Notes.Count; k++)
						{
							n = val.Notes[k];
							if ((n.Mandantory && InformationLevel == InformationLevelType.MandantoryFootnotesOnly) ||
									InformationLevel > InformationLevelType.MandantoryFootnotesOnly)
							{
								//sheet.Cell(row, 1).Value = var.Name + ":";
								setCell(
									sheet.Cell(row, 1),
									CellContentType.ValueNote,
									var.Name + ":",
									null
								);
								row++;
								//sheet.Cell(row, 1).Value = val.Value + ":";
								setCell(
									sheet.Cell(row, 1),
									CellContentType.ValueNote,
									val.Value + ":",
									null
								);
								row++;
								//sheet.Cell(row, 1).Value = n.Text;
								setCell(
									sheet.Cell(row, 1),
									CellContentType.ValueNote,
									n.Text,
									null
								);
								row += 2;
							}
						}
					}
				}
			}

			//Writes mandantory cellnotes 
			CellNote cn;
			VariableValuePair vvp;
			for (int i = 0; i < model.Meta.CellNotes.Count; i++)
			{
				cn = model.Meta.CellNotes[i];
				if ((cn.Mandatory && InformationLevel == InformationLevelType.MandantoryFootnotesOnly) ||
						InformationLevel > InformationLevelType.MandantoryFootnotesOnly)
				{
					for (int j = 0; j < cn.Conditions.Count; j++)
					{
						vvp = cn.Conditions[j];
						var = model.Meta.Variables.GetByCode(vvp.VariableCode);
						val = var.Values.GetByCode(vvp.ValueCode);
						//sheet.Cell(row, 1).Value =
						//sheet.Cell(row, 1).Value = var.Name + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.CellNote,
							var.Name + ":",
							null
						);
						row++;
						//sheet.Cell(row, 1).Value = val.Value + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.CellNote,
							val.Value + ":",
							null
						);
						row++;
					}
					//sheet.Cell(row, 1).Value = cn.Text;
					setCell(
						sheet.Cell(row, 1),
						CellContentType.CellNote,
						cn.Text,
						null
					);
					row += 2;
				}
			}
			return row++;
		}

		private int WriteTableInformation(int row, PXModel model, IXLWorksheet sheet)
		{
			bool contvar = false;
			Variable var;
			PCAxis.Paxiom.ContInfo info;
			string value;
			Dictionary<string, string> lastUpdated = new Dictionary<string, string>();
			Dictionary<string, string> contact = new Dictionary<string, string>();
			Dictionary<string, string> units = new Dictionary<string, string>();
			Dictionary<string, string> stockfa = new Dictionary<string, string>();
			Dictionary<string, string> refperiod = new Dictionary<string, string>();
			Dictionary<string, string> baseperiod = new Dictionary<string, string>();
			Dictionary<string, string> cfprices = new Dictionary<string, string>();
			Dictionary<string, string> dayadj = new Dictionary<string, string>();
			Dictionary<string, string> seasadj = new Dictionary<string, string>();

			//    With model.Meta
			if (model.Meta.ContentVariable != null && model.Meta.ContentVariable.Values.Count > 0)
			{
				contvar = true;
				var = model.Meta.ContentVariable;

				//1. Collect information for all the values
				//-----------------------------------------
				for (int i = 0; i < var.Values.Count; i++)
				{
					info = var.Values[i].ContentInfo;
					value = var.Values[i].Text;

					if (info != null)
					{
						//LAST-UPDATED
						if (!String.IsNullOrEmpty(info.LastUpdated))
						{
							lastUpdated.Add(value, info.LastUpdated);
						}

						//CONTACT
						if (!String.IsNullOrEmpty(info.Contact))
						{
							contact.Add(value, info.Contact);
						}

						//UNITS
						if (!String.IsNullOrEmpty(info.Units))
						{
							units.Add(value, info.Units);
						}

						//STOCKFA
						if (!String.IsNullOrEmpty(info.StockFa))
						{
							stockfa.Add(value, info.StockFa);
						}

						//REFPERIOD
						if (!String.IsNullOrEmpty(info.RefPeriod))
						{
							refperiod.Add(value, info.RefPeriod);
						}

						//BASEPERIOD
						if (!String.IsNullOrEmpty(info.Baseperiod))
						{
							baseperiod.Add(value, info.Baseperiod);
						}

						//CFPRICES
						if (!String.IsNullOrEmpty(info.CFPrices))
						{
							cfprices.Add(value, info.CFPrices);
						}

						//DAYADJ
						if (!String.IsNullOrEmpty(info.DayAdj))
						{
							if (info.DayAdj.ToUpper().Equals("YES"))
							{
								dayadj.Add(value, info.DayAdj);
							}
						}

						//SEASADJ
						if (!String.IsNullOrEmpty(info.SeasAdj))
						{
							if (info.SeasAdj.ToUpper().Equals("YES"))
							{
								seasadj.Add(value, info.SeasAdj);
							}
						}
					}
				}
			}

			//2. Write the collected information
			//----------------------------------

			//LAST-UPDATED
			row++;
			if (contvar)
			{
				//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordLastUpdated") + ":";
				setCell(
					sheet.Cell(row++, 1),
					CellContentType.Info,
					model.Meta.GetLocalizedString("PxcKeywordLastUpdated") + ":",
					null
				);
				foreach (KeyValuePair<string, string> kvp in lastUpdated)
				{
					//sheet.Cell(row, 1).Value = kvp.Key + ":";
					setCell(
						sheet.Cell(row, 1),
						CellContentType.Info,
						kvp.Key + ":",
						null
					);
                    row++;
					//sheet.Cell(row++, 2).Value = kvp.Value;
					setCell(
						sheet.Cell(row++, 1),
						CellContentType.Info,
						kvp.Value,
						null
					);
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.LastUpdated))
					{
						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordLastUpdated") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordLastUpdated") + ":",
							null
						);
						//TODO DATEFORMAT
						//sheet.Cell(row++, 2).Value = model.Meta.ContentInfo.LastUpdated;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.ContentInfo.LastUpdated,
							null
						);
					}
				}
			}

			//SOURCE
			row++;
			if (!String.IsNullOrEmpty(model.Meta.Source))
			{
				//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordSource") + ":";
				setCell(
					sheet.Cell(row, 1),
					CellContentType.Info,
					model.Meta.GetLocalizedString("PxcKeywordSource") + ":",
					null
				);
				//sheet.Cell(row++, 2).Value = model.Meta.Source;
                row++;
				setCell(
					sheet.Cell(row++, 1),
					CellContentType.Info,
					model.Meta.Source,
					null
				);
			}

			//CONTACT
			row++;
			if (contvar)
			{
				if (contact.Count > 0)
				{
					//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordContact") + ":";
					setCell(
						sheet.Cell(row++, 1),
						CellContentType.Info,
						model.Meta.GetLocalizedString("PxcKeywordContact") + ":",
						null
					);

                    string[] str;
                    var firstElement = contact.FirstOrDefault(); //Show only first contact person

                    setCell(
                        sheet.Cell(row, 1),
                        CellContentType.Info,
                        firstElement.Key + ":",
                        null
                    );
                    str = firstElement.Value.Split('#');

                    row++;
                    for (int i = 0; i < str.Length; i++)
                    {
                        setCell(
                            sheet.Cell(row++, 1),
                            CellContentType.Info,
                            str[i],
                            null
                        );
                    }
                  
                }
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.Contact))
					{
						string[] str;

						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordContact") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordContact") + ":",
							null
						);

						str = model.Meta.ContentInfo.Contact.Split('#');
                        row++;
						for (int i = 0; i < str.Length; i++)
						{
							//sheet.Cell(row++, 2).Value = str[i];                            
							setCell(
								sheet.Cell(row++, 1),
								CellContentType.Info,
								str[i],
								null
							);
						}
					}
				}
			}

			//COPYRIGHT
			row++;
			if (model.Meta.Copyright)
			{
				//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordCopyright");
				setCell(
					sheet.Cell(row++, 1),
					CellContentType.Info,
					model.Meta.GetLocalizedString("PxcKeywordCopyright"),
					null
				);
			}

			//UNITS
			row++;
			if (contvar)
			{
				if (units.Count > 0)
				{
					//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordUnits") + ":";
					setCell(
						sheet.Cell(row, 1),
						CellContentType.Info,
						model.Meta.GetLocalizedString("PxcKeywordUnits") + ":",
						null
					);
					foreach (KeyValuePair<string, String> kvp in units)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = kvp.Value;
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Value,
							null
						);
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.Units))
					{
						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordUnits") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordUnits") + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = model.Meta.ContentInfo.Units;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.ContentInfo.Units,
							null
						);
					}
				}
			}

			//STOCKFA
			row++;
			if (contvar)
			{
				if (stockfa.Count > 0)
				{
					//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordStockfa") + ":";

					setCell(
						sheet.Cell(row++, 1),
						CellContentType.Info,
						model.Meta.GetLocalizedString("PxcKeywordStockfa") + ":",
						null
					);

					foreach (KeyValuePair<String, String> kvp in stockfa)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						switch (kvp.Value.ToUpper())
						{
							case "S":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock"),
									null
								);
								break;
							case "F":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow"),
									null
								);
								break;
							case "A":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage"),
									null
								);
								break;
						}
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.StockFa))
					{
						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordStockfa") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordStockfa") + ":",
							null
						);
						switch (model.Meta.ContentInfo.StockFa.ToUpper())
						{
							case "S":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock"),
									null
								);
								break;
							case "F":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow"),
									null
								);
								break;
							case "A":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage"),
									null
								);
								break;
						}
					}
				}
			}

			//REFPERIOD
			row++;
			if (contvar)
			{
				if (refperiod.Count > 0)
				{
					//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordRefPeriod") + ":";
					setCell(
						sheet.Cell(row++, 1),
						CellContentType.Info,
						model.Meta.GetLocalizedString("PxcKeywordRefPeriod") + ":",
						null
					);
					foreach (KeyValuePair<string, string> kvp in refperiod)
					{
						//sheet.Cell(row, 1).Value = kvp.Key;
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key,
							null
						);
						//sheet.Cell(row++, 2).Value = kvp.Value;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							kvp.Value,
							null
						);
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.RefPeriod))
					{
						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordRefPeriod") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordRefPeriod") + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = model.Meta.ContentInfo.RefPeriod;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.ContentInfo.RefPeriod,
							null
						);
					}
				}
			}

			//BASEPERIOD
			row++;
			if (contvar)
			{
				if (baseperiod.Count > 0)
				{
					//writer.WriteEmptyRow()
					//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordBasePeriod") + ":";
					setCell(
						sheet.Cell(row, 1),
						CellContentType.Info,
						model.Meta.GetLocalizedString("PxcKeywordBasePeriod") + ":",
						null
					);
					foreach (KeyValuePair<string, string> kvp in baseperiod)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = kvp.Value;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							kvp.Value,
							null
						);
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.Baseperiod))
					{
						//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordBasePeriod") + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordBasePeriod") + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = model.Meta.ContentInfo.Baseperiod;
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.ContentInfo.Baseperiod,
							null
						);
					}
				}
			}

			//CFPRICES
			row++;
			if (contvar)
			{
				if (cfprices.Count > 0)
				{
					foreach (KeyValuePair<string, string> kvp in cfprices)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						switch (kvp.Value.ToUpper())
						{
							case "C":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent"),
									null
								);
								break;
							case "F":
								//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed");
                                row++;
								setCell(
									sheet.Cell(row++, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed"),
									null
								);
								break;
						}
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.CFPrices))
					{
						switch (model.Meta.ContentInfo.CFPrices.ToUpper())
						{
							case "C":
								//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent");
								setCell(
									sheet.Cell(row, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent"),
									null
								);
								break;
							case "F":
								//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed");
								setCell(
									sheet.Cell(row, 1),
									CellContentType.Info,
									model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed"),
									null
								);
								break;
						}
					}
				}
			}

			//DAYADJ
			row++;
			if (contvar)
			{
				if (dayadj.Count > 0)
				{
					//writer.WriteEmptyRow()
					foreach (KeyValuePair<string, string> kvp in dayadj)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordDayAdj");
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordDayAdj"),
							null
						);
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.DayAdj))
					{
						if (model.Meta.ContentInfo.DayAdj.ToUpper().Equals("YES"))
						{
							//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordDayAdj");
							setCell(
								sheet.Cell(row++, 1),
								CellContentType.Info,
								model.Meta.GetLocalizedString("PxcKeywordDayAdj"),
								null
							);
						}
					}
				}
			}

			//SEASADJ
			row++;
			if (contvar)
			{
				if (seasadj.Count > 0)
				{
					//writer.WriteEmptyRow()
					foreach (KeyValuePair<string, string> kvp in seasadj)
					{
						//sheet.Cell(row, 1).Value = kvp.Key + ":";
						setCell(
							sheet.Cell(row, 1),
							CellContentType.Info,
							kvp.Key + ":",
							null
						);
						//sheet.Cell(row++, 2).Value = model.Meta.GetLocalizedString("PxcKeywordSeasAdj");
                        row++;
						setCell(
							sheet.Cell(row++, 1),
							CellContentType.Info,
							model.Meta.GetLocalizedString("PxcKeywordSeasAdj"),
							null
						);
					}
				}
			}
			else
			{
				if (model.Meta.ContentInfo != null)
				{
					if (!String.IsNullOrEmpty(model.Meta.ContentInfo.SeasAdj))
					{
						if (model.Meta.ContentInfo.SeasAdj.ToUpper().Equals("YES"))
						{
							//sheet.Cell(row++, 1).Value = model.Meta.GetLocalizedString("PxcKeywordSeasAdj");
							setCell(
								sheet.Cell(row++, 1),
								CellContentType.Info,
								model.Meta.GetLocalizedString("PxcKeywordSeasAdj"),
								null
							);
						}
					}
				}
			}

            //OFFICIAL STATISTICS
            //If the statistics are official, insert information about that in the file 
            //Reqtest error report #406
            row++;
            if (model.Meta.OfficialStatistics)
            {

                setCell(
                    sheet.Cell(row++, 1),
                    CellContentType.Info,
                    model.Meta.GetLocalizedString("PxcKeywordOfficialStatistics"),
                    null
                );
            }
			//DATABASE
			row++;
			if (!String.IsNullOrEmpty(model.Meta.Database))
			{
				//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordDatabase") + ":";
				setCell(
					sheet.Cell(row, 1),
					CellContentType.Info,
					model.Meta.GetLocalizedString("PxcKeywordDatabase") + ":",
					null
				);
				//sheet.Cell(row++, 2).Value = model.Meta.Database;
                row++;
				setCell(
					sheet.Cell(row++, 1),
					CellContentType.Info,
					model.Meta.Database,
					null
				);
			}

			//MATRIX
			row++;
			if (!String.IsNullOrEmpty(model.Meta.Matrix))
			{
				//sheet.Cell(row, 1).Value = model.Meta.GetLocalizedString("PxcKeywordMatrix") + ":";
				setCell(
					sheet.Cell(row, 1),
					CellContentType.Info,
					model.Meta.GetLocalizedString("PxcKeywordMatrix") + ":",
					null
				);
				//sheet.Cell(row++, 2).Value = model.Meta.Matrix;
                row++;
				setCell(
					sheet.Cell(row++, 1),
					CellContentType.Info,
					model.Meta.Matrix,
					null
				);
			}
			return row;
		}

		private void WriteHeading(PXModel model, IXLWorksheet sheet)
		{
			//Calc left indention
			int lIndent = CalculateLeftIndentation(model);
			int p;

			int row;
			int column;

			//HEADING
			for (int i = 0; i < model.Meta.Heading.Count; i++)
			{
				//INTERVAL
				int hInterval = CalcPostHeadingInterval(i, model);
				//HEADING              
				p = CalcPreHeadingInterval(i, model); //0;

				for (int l = 0; l <= p; l++)
				{
					for (int j = 0; j < model.Meta.Heading[i].Values.Count; j++)
					{
						int dataNoteFactor = 1;
						if (_showDataNoteCells)
						{
							dataNoteFactor = 2;
						}
						row = 3 + i;

						column = lIndent + (j * (hInterval + 1) * dataNoteFactor) + (l * model.Meta.Heading[i].Values.Count * dataNoteFactor * (hInterval + 1)) + 1;
						//sheet.Cell(row, column).Value = model.Meta.Heading[i].Values[j].Text;

						setCell(
							sheet.Cell(row, column),
							CellContentType.Head,
							model.Meta.Heading[i].Values[j].Text,
							c => c.Style.Font.Bold = true
						);

						if (model.Meta.Heading[i].Values[j].HasNotes())
						{
							//sheet.Cell(row, column).Comment.AddText(model.Meta.Heading[i].Values[j].Notes.GetAllNotes());
							setCell(
								sheet.Cell(row, column),
								CellContentType.Comment,
								model.Meta.Heading[i].Values[j].Notes.GetAllNotes(),
								null
							);
						}

						//sheet.Cell(row, column).Style.Font.Bold = true;

					}
				}
			}
		}

		private int CalcPostHeadingInterval(int headingLevel, PXModel model)
		{
			int interv = 1;
			//Intervall
			if (headingLevel < model.Meta.Heading.Count)
			{
				for (int i = model.Meta.Heading.Count - 1; i > headingLevel; i--)
				{
					interv *= model.Meta.Heading[i].Values.Count;
				}
				interv -= 1;
			}
			else
			{
				interv = 0;
			}

			return interv;
		}

		private int CalcPreHeadingInterval(int headingLevel, PXModel model)
		{
			if (headingLevel == 0) return 0;
			int interv = 1;
			//Intervall
			if (headingLevel < model.Meta.Heading.Count)
			{
				for (int i = 0; i < headingLevel; i++)
				{
					interv *= model.Meta.Heading[i].Values.Count;
				}
				interv--;
			}
			else
			{
				interv = 0;
			}

			return interv;
		}

		private int CalculateLeftIndentation(PXModel model)
		{
			int lIndent = 0;
			for (int k = 0; k < model.Meta.Stub.Count; k++)
			{
				if (IsDoubleColumn(model.Meta.Stub[k]))
				{
					lIndent++;
				}
				lIndent++;
			}
			return lIndent;
		}

		private void GetStubCell(PXModel model, IXLWorksheet sheet, int stubNr, int rowNr)
		{
			GetStubCell(model, sheet, stubNr, rowNr, false);
		}
		private void GetStubCell(PXModel model, IXLWorksheet sheet, int stubNr, int rowNr, bool code)
		{
			int Interval;
			int count = model.Meta.Stub[stubNr].Values.Count;

			if (stubNr < model.Meta.Stub.Count - 1)
			{
				Interval = CalcStubInterval(stubNr + 1, model);
			}
			else
			{
				Interval = 1;
			}

			Value val;
			int row, column;
			if (rowNr % Interval == 0)
			{
				//Dim Cell As New Cell
				int offset = 0;
				for (int x = stubNr - 1; x > -1; x--)
				{
					if (IsDoubleColumn(model.Meta.Stub[x])) offset++;
				}
				val = model.Meta.Stub[stubNr].Values[(rowNr / Interval) % count];
				row = rowNr + 3 + model.Meta.Heading.Count;
				column = stubNr + 1 + offset;
				if (code)
				{
					//Writes the Code
					//sheet.Cell(row, column).Value = val.Code;
					setCell(
						sheet.Cell(row, column),
						CellContentType.Code,
						val.Code,
						c => { c.DataType = XLCellValues.Text; c.Style.Font.Bold = true; }
					);
					//sheet.Cell(row, column).DataType = XLCellValues.Text;
					//sheet.Cell(row, column).Style.Font.Bold = true;
					//Writes the Value
					//sheet.Cell(row, column + 1).Value = val.Value;
					setCell(
						sheet.Cell(row, column + 1),
						CellContentType.Stub,
						val.Value,
						c => c.Style.Font.Bold = true
					);
					//sheet.Cell(row, column + 1).Style.Font.Bold = true;
					if (val.HasNotes())
					{
						//sheet.Cell(row, column + 1).Comment.AddText(val.Notes.GetAllNotes());
						setCell(
							sheet.Cell(row, column + 1),
							CellContentType.Comment,
							val.Notes.GetAllNotes(),
							null
						);
					}
				}
				else
				{
					/*
					sheet.Cell(row, column).Value = val.Text;
					sheet.Cell(row, column).Style.Font.Bold = true; 
					*/

					setCell(
						sheet.Cell(row, column),
						CellContentType.Stub,
						val.Text,
						c => c.Style.Font.Bold = true
					);
					if (val.HasNotes())
					{
						//sheet.Cell(row, column).Comment.AddText(val.Notes.GetAllNotes());
						setCell(
							sheet.Cell(row, column),
							CellContentType.Comment,
							val.Notes.GetAllNotes(),
							null
						);
					}
				}
			}
		}

		private int CalcStubInterval(int StubChildNr, PXModel model)
		{
			int interv = 1;
			//Intervall
			for (int i = model.Meta.Stub.Count - 1; i >= StubChildNr; i--)
			{
				interv *= model.Meta.Stub[i].Values.Count;
			}

			return interv;
		}

		#endregion
        /// <summary>
        /// A method for format the cell in Excel that contains numeric/decimal values.
        /// </summary>
        /// <param name="dfm"></param>
        /// <returns></returns>
        private string FormatNumericCell(int dfm)
        {
            try
            {
                string[] arrFormat = new string[] { "0", "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000" };
                return arrFormat[dfm];
            }
            catch (Exception)
            {
                return "0.0";
            }
            
            
        }
		private bool IsDoubleColumn(Variable variable)
		{
			if (this.DoubleColumn == DoubleColumnType.AlwaysDoubleColumns)
			{
				if (!variable.Values.IsCodesFictional)
				{
					return true;
				}
			}
			else if (this.DoubleColumn == DoubleColumnType.OnlyDoubleColumnsWhenSpecified)
			{
				if (!variable.Values.IsCodesFictional)
				{
					if (variable.DoubleColumn == true)
					{
						return true;
					}
				}
			}
			return false;
		}
        private int GetDecimalPrecision(string value, string separtor)
        {
            if (!value.Contains(separtor))
            {
                return 0;
            }
            try
            {
                int decimalPrecision = value.Substring(value.IndexOf(separtor) + 1).Length;
                return decimalPrecision;
            }
            catch (Exception)
            {

                return 0;
            }

        }
    }




	public static class StringTests
	{
		public static bool IsNumeric(this string str)
		{
			double result;
			return double.TryParse(str, out result);
		}
	}


}
