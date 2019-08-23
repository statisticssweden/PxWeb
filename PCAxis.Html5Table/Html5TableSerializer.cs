using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Html5Table
{
	public class Html5TableSerializer : PCAxis.Paxiom.IPXModelStreamSerializer
	{
		private int[] _subStubValues;
		private DataFormatter _fmt;


		public void Serialize(PXModel model, string path)
		{
			if (model != null)
			{
				throw new ArgumentNullException("model");
			}


			//Let the StreamWriter verify the path argument

			using (var writer = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding(model.Meta.CodePage)))
			{
				DoSerialize(model, writer);
			}


		}
		public void Serialize(PXModel model, Stream stream)
		{

			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentNullException("The stream does not support writing");
			}

			var writer = new System.IO.StreamWriter(stream, System.Text.Encoding.GetEncoding(model.Meta.CodePage));


			DoSerialize(model, writer);


		}


		private void DoSerialize(PXModel model, StreamWriter wr)
		{
			Paxiom.Variables stub = model.Meta.Stub;
			Paxiom.PXData data = model.Data;
						
			wr.WriteLine(@"<table id=""" + model.Meta.Matrix +"_"+ Guid.NewGuid().ToString() + @""" >"); //@""" aria-describedby="" "

            wr.Write("<caption>");
			wr.Write(model.Meta.Title);
			wr.WriteLine("</caption>");

			wr.WriteLine("<thead>");

			//  Write title to the page
			int tableColspan = 1;
			Variables headings = model.Meta.Heading;
			if (!(headings == null))
			{
				for (int i = 0; (i
							<= (model.Meta.Heading.Count - 1)); i++)
				{
					tableColspan = (tableColspan * model.Meta.Heading[i].Values.Count);
				}

			}

			tableColspan = (tableColspan + stub.Count);

			Array.Resize(ref _subStubValues, stub.Count +1);


			CalculateSubValues(stub, 0, ref _subStubValues);
			WriteHeadings(wr, model);

			wr.WriteLine("</thead>");

			wr.WriteLine("<tbody>");

			int levels = stub.Count;
			int row = 0;
			//  Start at row zero
			//  Write the table
			WriteTable(wr, model, levels, 0, model.Meta.ShowDecimals, ref row);
			wr.WriteLine("</tbody>");

			
			wr.WriteLine("</table>");
            
           // wr.WriteLine(@"<style type=""text/css""> td, th{ border: 1px solid #ddd; padding: 10px; }</ style >");
            wr.Flush();
		}

		private int CalculateSubValues(Variables vars, int level, ref int[] subValues)
		{
			if ((vars.Count == 0))
			{
				subValues[level] = 1;
				return 0;
			}
			else if (((vars.Count - 1)
						== level))
			{
				subValues[level] = 1;
				return vars[level].Values.Count;
			}
			else
			{
				int nextLevel = (level + 1);
				int ret = CalculateSubValues(vars, nextLevel, ref subValues);
				subValues[level] = ret;
				return (ret * vars[level].Values.Count);
			}

		}

		private void WriteHeadings(System.IO.StreamWriter wr, PCAxis.Paxiom.PXModel model)
		{
			Variables heading = model.Meta.Heading;
			if (!(heading == null))
			{
				int[] subHeadings = new int[2];
				if ((heading.Count > 0))
				{
					Array.Resize(ref subHeadings, heading.Count);
				}

				CalculateSubValues(heading, 0, ref subHeadings);
				int timesToWrite = 1;
				//  This keep track of the number of times the current heading shall be written
				int timesWritten = 0;
				//  This keep track of the number of times the current heading has been written
				for (int index = 0; (index 
							<= (heading.Count - 1)); index++)
				{
					wr.WriteLine("<tr>");
					if ((model.Meta.Stub.Count > 0))
					{
						
						wr.WriteLine("<th></th>");

					}

					//  Write the heading
					int valuesCount = heading[index].Values.Count;
					for (int j = 0; (j
								<= (timesToWrite - 1)); j++)
					{
						Paxiom.Values headingValues = heading[index].Values;
						for (int ix = 0; (ix
									<= (headingValues.Count - 1)); ix++)
						{

							if (index == heading.Count -1)
							{
								wr.Write(@"<th scope=""col""");
							}
							else
							{
								wr.Write("<th colspan=");
								wr.Write(subHeadings[index]);
							}
							

							
							wr.Write(">");
							wr.Write(headingValues[ix].Text);
							wr.WriteLine("</th>");
							timesWritten = (timesWritten + 1);
						}

					}

					timesToWrite = timesWritten;
					timesWritten = 0;
					wr.WriteLine("</tr>");
				}

			}

		}

		private void WriteDataLine(System.IO.StreamWriter wr, PCAxis.Paxiom.PXModel model, System.Globalization.CultureInfo ci, int precision, int row)
		{
			string value;
			string n = String.Empty;
			string dataNote = String.Empty;

			PCAxis.Paxiom.PXData data = model.Data;
			for (int c = 0; (c
						<= (data.MatrixColumnCount - 1)); c++)
			{
				wr.Write("<td>");
				value = _fmt.ReadElement(row, c, ref n, ref dataNote);
				wr.Write(value);
				wr.WriteLine("</td>");
			}

		}

		private void WriteTable(System.IO.StreamWriter wr, Paxiom.PXModel model, int levels, int level, int precision, ref int row)
		{
			System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
			_fmt = new DataFormatter(model);
            Variables heading = model.Meta.Heading;

            if ((level == levels))
			{
				//  Time to write the data to the file
				WriteDataLine(wr, model, ci, precision, row);
				//  Close this row. The closing tag is not writen if level + 1 < levels, se
				//  the else clause below
				wr.WriteLine("</tr>");
				row = (row + 1);
			}
			else
			{
				Paxiom.Values values = model.Meta.Stub[level].Values;
				int nextLevel = (level + 1);
                for (int i = 0; (i <= (values.Count - 1)); i++)
                {
                    wr.WriteLine("<tr>");
                    wr.Write(@"<th scope=""row"">");

                    wr.Write(values[i].Text);
                    wr.WriteLine("</th>");
                    _fmt = new DataFormatter(model);

                    int decimals = model.Meta.ShowDecimals;
					if (values[i].HasPrecision())
					{
						decimals = values[i].Precision;
					}
                    
                    if(level + 1 < levels)
                    {
                        for (int y = 0; y <= model.Data.MatrixColumnCount-1; y++)
                        {
                            wr.WriteLine("<td></td>");
                        }
                        wr.WriteLine("</tr>");
                    }


                    WriteTable(wr, model, levels, nextLevel, decimals, ref row);
				}

			}

		}
	}
}

