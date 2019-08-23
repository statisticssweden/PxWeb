using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PCAxis.Charting;
using PCAxis.Paxiom;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using PCAxis.PxExtend;
using System.IO;
using PCAxis.Paxiom.Operations;
using PCAxis.Charting.InternalExtensions;
using System.Diagnostics;

namespace PxGraphTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		PXModel px;
		string fileName;
		List<SeriesChartType> chartTypes = new List<SeriesChartType>();

		private void Form1_Load(object sender, EventArgs e)
		{
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

			Width -= infoLabel.Width - 584;
			Height -= infoLabel.Height - 324;

			CultureInfo ci = new CultureInfo("da-DK");
			ci.NumberFormat.NumberGroupSeparator = " ";
			ci.NumberFormat.CurrencyDecimalSeparator = ",";
			Thread.CurrentThread.CurrentUICulture = ci;


			for (int i = 0; i < 34; i++)
				chartType.Items.Add(((SeriesChartType)i).ToString());

			chartType.Items.Add("Population");

			//dstChart.Font.Family = new FontFamily("Arial Narrow");

#if DEBUG
			string type = "Line";
			
			fileName = @"h:\118943.px"; //Forkert akse
			fileName = @"h:\2012912131220111204936FOLK1.px"; //Meget lang under-overskrift
			//fileName = @"h:\pyramid10.px"; //Befolkningspyramide, 10-års-aldersgrupper
			//fileName = @"h:\pyramid5.px"; //Befolkningspyramide, 5-års-aldersgrupper
			//fileName = @"h:\pyramid1.px"; //Befolkningspyramide, 1-års-aldersgrupper
			//fileName = @"h:\134509.px"; //tegnsæt
			//fileName = @"h:\152976.px"; //akser
			//fileName = @"h:\155160.px";
			fileName = @"h:\140203.px"; //Dobbelt enhed
			fileName = @"h:\126275.px"; type = "Bar"; //for stort interval for bar
			fileName = @"h:\152976.px"; type = "Line"; //Engelske tider
			fileName = @"h:\pyramid10test.px"; type = "Population"; //Befolkningspyramide, 10-års-aldersgrupper
			fileName = @"h:\146698.px"; type = "Line"; //For meget luft i siderne
			fileName = @"h:\126275.px"; type = "Bar";
			fileName = @"h:\legendtime2.px"; type = "Column"; //Skal beholde tid i underoverskrift
			fileName = @"h:\2012102152456111965792FOLK1.px"; type = "Population"; //Mange aldre
			fileName = @"h:\pyramid10test.px"; type = "Population"; //Befolkningspyramide, 10-års-aldersgrupper
			fileName = @"h:\134509.px"; type = "Bar"; //PXModel læser forkert tegnsæt
			fileName = @"h:\146467.px"; type = "Pie"; //Pie med uheldigt placerede tekster
			fileName = @"h:\162006.px"; type = "Pie"; //Mangler legend
			fileName = @"h:\146751.px"; type = "Bar"; //Tegnsæt-problemer
			fileName = @"h:\150729.px"; type = "Column"; //Aksetitel skæres i visse højder
			fileName = @"h:\14679432589377428.px"; type = "Pie"; //Kan ikke danne pie
			fileName = @"h:\10535235322713474.px"; type = "Pie"; //Pie mangler akseenhed
			fileName = @"h:\11886255520046261.px"; type = "Bar"; //Mangler en legend-tekst
			fileName = @"h:\legendtime.px"; type = "Column"; //Skal have fjernet tid i underoverskrift
			fileName = @"h:\20121022135543112768999FOLK1.px"; type = "Column"; //Mange 1-værdi-variable
			fileName = @"h:\10535235322713474.px"; type = "Bar"; //Meget lang akseenhed
			fileName = @"h:\14469936119071536.px"; type = "Line"; //Meget lang værditekst
			fileName = @"h:\15058160770215484.px"; type = "Pie"; //Lang værditekst
			//fileName = @"h:\2012102317022112833663FOLK1.px"; type = "Column";
            fileName = @"h:\8966747670373370.px"; type = "Line"; //Viser for stort dataområde
			fileName = @"h:\010_khi_tau_101_en.px"; type = "Line"; //Tid opdelt i to variable (Baumgartner)
			fileName = @"h:\16386346604344427.px"; type = "Column"; //Ikke plads til akse-labels
			fileName = @"h:\2012125143921114954423FOLK1.px"; type = "Column"; //Test af variabel på X
			fileName = @"h:\2012126133738114999498UHV1.px"; type = "Line"; //Test af variabel på x
			fileName = @"h:\16329652362087594.px"; type = "Line"; //Labels afkortes i bredder ml. ca. 577 og 596.
			fileName = @"h:\15597160537048749.px"; type = "Line";
			fileName = @"h:\pyramid5.px"; type = "Population"; //Befolkningspyramide, 5-års-aldersgrupper
			fileName = @"h:\167653.px"; type = "Column";

			//Hele vises uden halve, hvis halve findes.
			//Y-akse 2 har kun top og bund værdi

			px = PxExtend.CreatePxModel(fileName);



			px = 
				PxExtend.CreatePxModel(
					new FileInfo(fileName), 
					true,
					new Selection[] 
					{
						//new Selection("alder").AddValueCode("TOT")
					},
					null,
					null
				);

			//DstChart testChart =
			//    new DstChart(
			//        px,
			//        c =>
			//        {
			//            c.Width = 563;
			//            c.Height = 337;
			//            c.AdjustAxes = true;
			//            c.ChartType = SeriesChartType.Bar;
			//            //c.MaleValueNameForPopulationPyramid = "mænd";
			//        }
			//    );

			//testChart.SaveImage(@"h:\test.png", ChartImageFormat.Png);
			//Process p = new Process() { StartInfo = new ProcessStartInfo(@"h:\test.png") };
			//p.Start();
			//Environment.Exit(0);

			//px.Save(@"h:\1.px");
			//px = px.Eliminate("tid");
			//px.Save(@"h:\2.px");
			//Environment.Exit(0);

			//button2.Text = "Px&Chart";
			chartType.SelectedIndex = chartType.Items.IndexOf(type);
			editPanel.Enabled = true;

			//updateImage();
#else
			chartType.SelectedIndex = chartType.Items.IndexOf("Column");
#endif

			updateChartTypesLabel();
		}

		void openFile()
		{
			OpenFileDialog fileDialog =
				new OpenFileDialog()
				{
					Title = "Åbn PC-AXIS-fil som graf",
					Filter = "PC-Axis (*.px)|*.px|Alle filer (*.*)|*.*"
				};

			if (fileDialog.ShowDialog() == DialogResult.Cancel)
				return;

			fileName = fileDialog.FileName;

			editPanel.Enabled = true;

			px = PxExtend.CreatePxModel(fileName);

			updateImage();
		}

		void updateImage()
		{
#if !DEBUG
			try
			{
#endif
				Text = (!fileName.Equals(String.Empty) ? fileName + " - " : "") + "Test af PxChart BETA";

				chartSizeLabel.Text = "Str.: " + pxChart.Width + ", " + pxChart.Height;

				if (button2.Text == "Dst&Chart")
				{
					dstChart.AddPxData(
						px,
						c =>
						{
							//c.PieLabelStyle = "Outside";

							((DstChart)c).PXMeta = px != null ? px.Meta : null;

							//c.AllowDecimalInterval = false;
							//c.MaleValueNameForPopulationPyramid = chartType.SelectedIndex == 34 ? "mænd" : null;
							c.MaleValueNameForPopulationPyramid = chartType.SelectedIndex == 34 ? px.Meta.Variables.First(x => x.Values.Count == 2).Values.First().Value : null;
							c.ChartTypes = chartTypes.Concat(new SeriesChartType[] { (SeriesChartType)chartType.SelectedIndex }).ToArray();
							c.AutoMoveMostValuesVariableToX = autoPivot.Checked;
							c.OverrideTitle = showTitle.Checked ? null : "";
							c.HideLegend = !showLegend.Checked;

							//((DstChart)c).ColorCollectionsForChart =
							//    new List<Color[]>() 
							//    {
							//        new Color[] { Color.Bisque, Color.Beige, Color.Blue, Color.Brown }
							//    };

							if (showCopyright.Checked)
							{
								c.InfoText = "© Danmarks Statistik";

								//if (px != null && px.Meta.Matrix != "")
								//    c.InfoText += ", kilde: http://www.statistikbanken.dk/" + px.Meta.Matrix.ToLower();
							}
							else
								c.InfoText = "";

							//c.AutoMoveMostValuesVariableToX = false;
							//c.HideLegend = false;
							//c.LegendHeightInPixels = 40;

							c.AdjustAxes = adjust.Checked;

							//c.SortDataPoints = sort.Checked ? PxChart.SortType.Descending : PxChart.SortType.None;

							c.SortDataPoints = (PxChart.SortType)Enum.Parse(typeof(PxChart.SortType), (listBox1.SelectedItem ?? "None").ToString());

							//c.DecimalPlaces = 5;
							//c.AllowDecimalInterval = false;

							try
							{
								c.Font.Family = new FontFamily("Arial");
								c.Font.SizeRegular = 13;
								//c.Font.SizeSmall = 23;
							}
							catch
							{ }
						}
					);
					//if (adjust.Checked)
					//    dstChart.adjustAxes();
					//if (sort.Checked)
					//    dstChart.Sort();

					//dstChart.ChartAreas["Main"].AxisX.LabelStyle.Angle = 45;
					//dstChart.ChartAreas["Main"].AxisX.IsLabelAutoFit = false;

					//dstChart.ChartAreas.First().AxisY.LabelStyle.Format = "{0:#,##0.##}";


					//dstChart.AntiAliasing = AntiAliasingStyles.All;
					//dstChart.Units = "Mio. kr.";
					//dstChart.ChartAreas.First().AxisY.LabelStyle.Format = "N";
					//dstChart.ChartAreas.First().AxisX.LabelStyle.Format = "N";
					//dstChart.ChartAreas.First().AxisX.IsLabelAutoFit = true;
				}

				if (button2.Text == "Px&Chart")
				{
					pxChart.AddPxData(
						px,
						c =>
						{
							//c.AllowDecimalInterval = false;
							//c.MaleValueNameForPopulationPyramid = chartType.SelectedIndex == 34 ? "mænd" : null;
							c.MaleValueNameForPopulationPyramid = chartType.SelectedIndex == 34 ? px.Meta.Variables.First(x => x.Values.Count == 2).Values.First().Value : null;
							c.ChartTypes = chartTypes.Concat(new SeriesChartType[] { (SeriesChartType)chartType.SelectedIndex }).ToArray();
							c.AutoMoveMostValuesVariableToX = autoPivot.Checked;
							c.InfoText = showCopyright.Checked ? "© Danmarks Statistik" : "";
							c.AdjustAxes = adjust.Checked;
							//c.SortDataPoints = sort.Checked ? PxChart.SortType.Descending : PxChart.SortType.None;
							c.SortDataPoints = (PxChart.SortType)Enum.Parse(typeof(PxChart.SortType), (listBox1.SelectedItem ?? "None").ToString());
						}
					);
					//if (adjust.Checked)
					//    pxChart.adjustAxes();
					//if (sort.Checked)
					//    pxChart.Sort();
				}

				if (button2.Text == "Ms&Chart")
				{
					msChart.Series.Clear();
					if (autoPivot.Checked)
						px = px.PivotSpecificVariableToAloneInHead(px.MostValuesVariable().Name);
					msChart.DataBindCrossTable(
						px.AsDataTableCompressed().AsEnumerable(),
						"Series",
						"Label",
						"DATA",
						""
					);
					foreach (Series s in msChart.Series)
						s.ChartType = (SeriesChartType)chartType.SelectedIndex;
				}

				dstChart.Visible = button2.Text == "Dst&Chart";
				pxChart.Visible = button2.Text == "Px&Chart";
				msChart.Visible = button2.Text == "Ms&Chart";

				infoLabel.Hide();

				//Activate();
#if !DEBUG
			}
			catch (Exception e)
			{
				infoLabel.Text = fileName != null ? e.Message : "Åbn en px-fil for at vise en graf.\nKlik på Åbn-knappen (Alt + Å).";
				infoLabel.Show();
			}
#endif
		}

		private void openToolStripButton_Click(object sender, EventArgs e)
		{
			openFile();
		}

		private void chartType_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateChartTypesLabel();
			updateImage();
		}

		private void dstLayout_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void adjust_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void showAs3D_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void sort_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			chartSizeLabel.Text = "Str.: " + pxChart.Width + ", " + pxChart.Height;
		}

		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			updateImage();
		}

		private void autoPivot_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void chartTypeAdd_Click(object sender, EventArgs e)
		{
			chartTypes.Add(((SeriesChartType)chartType.SelectedIndex));
			updateChartTypesLabel();
			updateImage();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			chartTypes.Clear();
			updateChartTypesLabel();
			updateImage();
		}

		void updateChartTypesLabel()
		{
			chartTypesLabel.Text = "Serietyper: " + String.Join(", ", chartTypes.Select(x => ((SeriesChartType)x).ToString()).Concat(new string[] { ((SeriesChartType)chartType.SelectedIndex).ToString() }).ToArray());
		}

		private void showCopyright_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void chartSizeLabel_Click(object sender, EventArgs e)
		{
			Width -= infoLabel.Width - 584;
			Height -= infoLabel.Height - 324;
			updateImage();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Button s = (Button)sender;

			switch (s.Text)
			{
				case "Dst&Chart":
					s.Text = "Px&Chart";
					break;
				case "Px&Chart":
					s.Text = "Ms&Chart";
					break;
				case "Ms&Chart":
					s.Text = "Dst&Chart";
					break;
			}

			updateImage();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			updateImage();
			
			string p = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
			string f = new FileInfo(fileName).Name.Replace(".", "_") + ".png";

			pxChart.SaveImage(p + "PxChart_" + f, ChartImageFormat.Png);
			dstChart.SaveImage(p + "DstChart_" + f, ChartImageFormat.Png);
			msChart.SaveImage(p + "MsChart_" + f, ChartImageFormat.Png);
		}

		private void showTitle_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private void showLegend_CheckedChanged(object sender, EventArgs e)
		{
			updateImage();
		}

		private string listBox1Selected = null;
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((listBox1.SelectedItem ?? "").ToString().Equals(listBox1Selected))
				listBox1.SelectedItem = null;

			listBox1Selected = (listBox1.SelectedItem ?? "").ToString();

			updateImage();
		}
	}
}
