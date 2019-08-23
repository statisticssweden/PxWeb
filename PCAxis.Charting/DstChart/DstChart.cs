using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using PCAxis.Charting.InternalExtensions;
using PCAxis.Paxiom;
using PCAxis.PxExtend;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace PCAxis.Charting
{
	/// <summary>
	/// Displays a PXModel instance as a chart. Derived class for usage in Statistics Denmark.
	/// </summary>
	public class DstChart : PxChart
	{
		/// <summary>
		/// Only for usage in a form. If not used in a form, use a constructor with a signature including a PXModel instance.
		/// </summary>
		public DstChart()
		{ }

		/// <summary>
		/// Metadata for Charts data.
		/// </summary>
		public PXMeta PXMeta { get; set; }

		/// <summary>
		/// Creates a DstChart from a PXModel instance.
		/// </summary>
		/// <param name="px">PXModel instance to create chart from.</param>
		public DstChart(PXModel px)
			: this(px, null)
		{ }

		/// <summary>
		/// Creates a DstChart from a PXModel instance.
		/// </summary>
		/// <param name="px">PXModel instance to create chart from.</param>
		public DstChart(PxChartSettings settings)
			: this(null, settings)
		{ }

		/// <summary>
		/// Creates PxChart with chart settings.
		/// </summary>
		/// <param name="px">PXModel instance to create chart from.</param>
		/// <param name="settings">Settings as PxChartSettings. Eg. as a Lambda expression.</param>
		public DstChart(PXModel px, PxChartSettings settings)
			: base(
				px,
				c =>
				{
					c.EliminateAllSingleValueVariables = true;

					((DstChart)c).PXMeta = px != null ? px.Meta : null;

					//c.PieLabelStyle = "Outside";

					//c.InfoText = "© " + Resources.DstChart.Copyright;

					//if (px != null && px.Meta.Matrix != "")
					//{
					//    c.InfoText +=
					//        String.Format(
					//            ", {0}: http://{1}/" + px.Meta.Matrix.ToLower(),
					//            Resources.DstChart.Source,
					//            Resources.DstChart.Site
					//        );
					//}

					try
					{
						c.Font.Family = new FontFamily("Arial");
						c.Font.SizeRegular = 13;
						//c.Font.TitleTextColor = ColorTranslator.FromHtml("#333");
						c.Font.InfoTextBrush = new SolidBrush(ColorTranslator.FromHtml("#4c4c4c"));
					}
					catch
					{ }

					if (settings != null)
						settings(c);
				}
			)
		{ }

		/// <summary>
		/// Array of colors used in Statistics Denmark.
		/// </summary>
		public List<Color[]> ColorCollections =
			new List<Color[]>
			{
				new Color[]
		        {
		            //Blue
		            Color.FromArgb(0,145,212),
		            Color.FromArgb(38,163,221),
		            Color.FromArgb(117,182,229),
		            Color.FromArgb(163,204,238),
		            Color.FromArgb(207,226,246),
				},
				new Color[]
		        {
		            //Green
		            Color.FromArgb(0,134,59),
		            Color.FromArgb(71,150,81),
		            Color.FromArgb(118,170,113),
		            Color.FromArgb(163,193,149),
		            Color.FromArgb(198,215,182),
				},
				new Color[]
		        {
					//Gray
		            Color.FromArgb(111,109,92),
		            Color.FromArgb(144,142,126),
		            Color.FromArgb(174,172,157),
		            Color.FromArgb(202,201,189),
		            Color.FromArgb(229,228,222),
				},
				new Color[]
		        {
		            //Orange
		            Color.FromArgb(238,114,3),
		            Color.FromArgb(243,148,51),
		            Color.FromArgb(247,171,96),
		            Color.FromArgb(250,193,138),
		            Color.FromArgb(253,218,176),
				},
			};

		private List<ValueColor> valueColors = new List<ValueColor>();

		/// <summary>
		/// List of colors for series based on a certain value, and just that one.
		/// </summary>
		public void AddValueColor(params ValueColor[] valueColor)
		{
			valueColors.AddRange(valueColor);
		}

		/// <summary>
		/// Collection of collections of colors for usage in DstChart.
		/// </summary>
		private List<Color> getColorCollection()
		{
			var colors = new List<Color>();

			if (!Series.Any(x => PxChartExtensions.ChartTypesStacked.Contains(x.ChartType)))
			{
				for (int i = 0; i < ColorCollections.Max(x => x.Count()); i++)
				{
					foreach (var c in ColorCollections)
					{
						if (c.Count() > i)
							colors.Add(c[i]);
					}
				}
			}
			else
			{
				int cpc = (int)Math.Ceiling(Convert.ToDecimal(Series.Count) / Convert.ToDecimal(ColorCollections.Count));

				colors.AddRange(
					ColorCollections.SelectMany(x => x.Take(cpc))
				);
			}

			return colors.Distinct().ToList();
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for adding ChartArea.
		/// </summary>
		protected override void addChartArea()
		{
			base.addChartArea();

			//Since the base chart component doesn't accept this based on the axis template
			foreach (Axis axis in ChartAreas["Main"].Axes)
			{
				axis.TitleFont = Font.Regular;
				axis.TitleForeColor = ColorTranslator.FromHtml("#4c4c4c");
			}

			float top = spaceUsedForTitle + space + spaceUsedForLegend + (spaceUsedForLegend > 0 ? space : 0) + space;

			//float left = ChartTypes.Contains(SeriesChartType.Pie) ? 50 : 0;
			float width = ChartTypes.Contains(SeriesChartType.Pie) ? 50 : 100;
			ChartAreas["Main"].Position = new ElementPosition(0, top, width, 100 - top - SpaceUsedForInfoText);
		}

		float TitleFontSize = 13;

		public override float TitleHeight
		{
			get
			{
				if (OverrideTitle == "")
					return 0;

				return
					(TitleFontSize + 7) * 100 / Height;
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for adding Title.
		/// </summary>
		protected override void addTitle()
		{
			base.addTitle();

			foreach (var t in Titles)
			{
				t.Font = new Font("Arial", TitleFontSize, FontStyle.Bold);
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific settings for the X-axis template.
		/// </summary>
		protected override Axis templateAxisX
		{
			get
			{
				Axis axis = base.templateAxisX;
				//axis.LabelStyle.Angle = 90;
				//axis.IsLabelAutoFit = false;
				axis.LineColor = Color.FromArgb(174, 172, 157);
				axis.MajorGrid.LineColor = Color.FromArgb(174, 172, 157);
				axis.LabelStyle.ForeColor = ColorTranslator.FromHtml("#4c4c4c");
				axis.LabelStyle.Format = "#,##0.##";
				return axis;
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific settings for the Y-axis template.
		/// </summary>
		protected override Axis templateAxisY
		{
			get
			{
				Axis axis = base.templateAxisY;
				axis.LineColor = Color.FromArgb(174, 172, 157);
				axis.MajorGrid.LineColor = Color.FromArgb(174, 172, 157);
				axis.LabelStyle.ForeColor = ColorTranslator.FromHtml("#4c4c4c");
				axis.LabelStyle.Format = "#,##0.##";
				axis.IntervalAutoMode = IntervalAutoMode.VariableCount;
				return axis;
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for adding Legend.
		/// </summary>
		protected override void addLegend()
		{
			base.addLegend();

			if (showLegend)
			{
				Legends.First().Font = Font.Small;
				Legends.First().ForeColor = ColorTranslator.FromHtml("#4c4c4c");
				Legends.First().LegendStyle = LegendStyle.Table;

				if (!ChartTypes.Contains(SeriesChartType.Pie))
					Legends.First().TextWrapThreshold = 1000;

				Legends.First().TableStyle = LegendTableStyle.Wide;

				if (!ChartTypes.Contains(SeriesChartType.Pie))
				{
					float y = spaceUsedForTitle + space;
					float h = legendHeight;
					Legends.First().Position = new ElementPosition(0, y, 100, h);
				}
				else
				{
					Legends.First().LegendStyle = LegendStyle.Column;
					float top = spaceUsedForTitle + space + space;
					Legends.First().Position = new ElementPosition(50, top, 50, 100 - top - SpaceUsedForInfoText);
				}
			}
		}

		private float space
		{
			get { return pixels(10); }
		}

		private float pixels(int numberOfPixels)
		{
			return numberOfPixels * 100f / Height;
		}

		/// <summary>
		/// Spaced used for legend.
		/// </summary>
		private float spaceUsedForLegend
		{
			get
			{
				if (ChartTypes.Contains(SeriesChartType.Pie))
					return 0;

				return
					showLegend ? legendHeight : 0f;
			}
		}

		/// <summary>
		/// Adjusted title height for DstChart including sub-title.
		/// </summary>
		private float spaceUsedForTitle
		{
			get
			{
				if (TitleHeight == 0)
					return 0;

				return
					TitleHeight +
						measureSecondaryTitle(CreateGraphics(), 0, Color.Black, Color.Black) * 100 / Height;
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for chart initilization.
		/// </summary>
		protected override void additionalFunctionalityLast()
		{
			base.additionalFunctionalityLast();

			//foreach (var s in Series)
			//{
			//	s.BorderWidth = 1;
			//	s.BorderDashStyle = ChartDashStyle.Solid;
			//	s.BorderColor = Color.White;
			//}

			if (PXMeta != null)
			{
				string title = OverrideTitle ?? PXMeta.Contents;

				Titles.First().Text = title;

				if (!axisVariableIsTime)
					ChartAreas["Main"].AxisX.Title = axisCode.FirstLetterToUpper();
			}

			if (moveUnitToAxis)
				ChartAreas["Main"].AxisY.Title = Units.FirstLetterToUpper();

			var colorsNeeded =
				ChartTypes.Contains(SeriesChartType.Pie)
					? Series.First().Points.Count
					: Series.Count;

			var customColors = getColorCollection();

			if (valueColors != null)
			{
				foreach (var vColor in valueColors)
				{
					if (Series.Count == 1)
					{
						var p = Series.First().Points.Where(x => ("|" + x.GetCustomProperty("Codes") + "|").EndsWith(vColor.CodeString));

						if (p.Count() == 1)
						{
							p.First().Color = vColor.Color;
							customColors.Remove(vColor.Color);
						}
					}
					else
					{
						var s = Series.Where(x => ("|" + x.Points.First().GetCustomProperty("Codes") + "|").Contains(vColor.CodeString));

						if (s.Count() == 1)
						{
							s.First().Color = vColor.Color;
							customColors.Remove(vColor.Color);
						}
					}
				}
			}

			if (customColors.Count >= colorsNeeded)
			{
				Palette = ChartColorPalette.None;
				PaletteCustomColors = customColors.ToArray();
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for initializing the data layout.
		/// </summary>
		protected override void initializeDataLayout()
		{
			base.initializeDataLayout();

			double xWidth = ChartAreas["Main"].AxisX.PixelSize();

			foreach (var s in Series)
				if (s.ChartType.ToString().Contains("Column") && xWidth / s.Points.Count > 90)
					s["PixelPointWidth"] = "70";
		}

		private string axisCode
		{
			get
			{
				return
					Series.First().Points.First().GetCustomProperty("Codes").Split(new[] { "||" }, StringSplitOptions.None).Last().Split('|').First();
			}
		}

		private bool axisVariableIsTime
		{
			get
			{
				return
					PXMeta.Variables.Any(v => v.IsTime && v.Code.Equals(axisCode));
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for additional graphics.
		/// </summary>
		protected override void additionalGraphics(Graphics graphics)
		{
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

			base.additionalGraphics(graphics);

			if (moveUnitToGridline)
			{
				ChartArea ca = ChartAreas.First();

				float x = (float)ca.AxisX.ValueToPixelPosition(ca.AxisX.Minimum);

				if (ca.AxisY.CustomLabels.Count > 0)
				{
					CustomLabel tl = ca.AxisY.CustomLabels.Last();
					double topGrid = tl.FromPosition + (tl.ToPosition - tl.FromPosition) / 2;

					float y = (float)ca.AxisY.ValueToPixelPosition(topGrid) - (Font.SizeSmall + 2) - 4;

					graphics.DrawString(Units, Font.Small, new SolidBrush(ColorTranslator.FromHtml("#4c4c4c")), x, y);
				}
			}

			float top;

			if (OverrideTitle != "")
			{
				Color color = ColorTranslator.FromHtml("#4c4c4c");
				Color color2 = ColorTranslator.FromHtml("#cccccc");

				top = (float)Height * TitleHeight / 100 + 4;

				drawSecondaryTitle(graphics, top + 2, color, color2);
			}
			else
			{
				top = 1;
			}

			//graphics.DrawLine(new Pen(Color.FromArgb(0, 145, 212), 2), 0, top, Width, top);
			//graphics.DrawLine(new Pen(Color.FromArgb(0, 145, 212), 2), 0, Height - 1, Width, Height - 1);
		}

		private float measureSecondaryTitle(Graphics graphics, float top, Color color, Color color2)
		{
			return
				drawSecondaryTitle(false, graphics, top, color, color2);
		}

		private void drawSecondaryTitle(Graphics graphics, float top, Color color, Color color2)
		{
			drawSecondaryTitle(true, graphics, top, color, color2);
		}

		private float drawSecondaryTitle(bool doDraw, Graphics graphics, float top, Color color, Color color2)
		{
			PointF currLoc = new PointF(0, top);

			bool first = true;

			//Unit
			if (showUnit && !moveUnitToAxis && !moveUnitToGridline)
			{
				if (!first)
					currLoc = drawString(doDraw, graphics, currLoc, new DrawableSeparator(Font.Regular, color2));

				first = false;
				currLoc =
					drawString(doDraw, graphics, currLoc,
						new DrawableString(Font.Regular, color, Resources.DstChart.Unit + ": "),
						new DrawableString(Font.Bold, color, Units)
					);
			}

			if (PXMeta != null)
			{
				//One value variables
				foreach (Variable v in PXMeta.Variables.Where(v => v.Values.Count == 1 && !v.IsTime))
				{
					if (!first)
						currLoc = drawString(doDraw, graphics, currLoc, new DrawableSeparator(Font.Regular, color2));

					first = false;
					currLoc =
						drawString(doDraw, graphics, currLoc,
							new DrawableString(Font.Regular, color, v.Name.FirstLetterToUpper() + ": "),
							new DrawableString(Font.Bold, color, v.Values.First().Text)
						);
				}

				//Time if only one period
				foreach (Variable v in PXMeta.Variables.Where(v => v.Values.Count == 1 && v.IsTime))
				{
					if (!first)
						currLoc = drawString(doDraw, graphics, currLoc, new DrawableSeparator(Font.Regular, color2));

					first = false;
					currLoc =
						drawString(doDraw, graphics, currLoc,
							new DrawableString(Font.Regular, color, v.Name.FirstLetterToUpper() + ": "),
							new DrawableString(Font.Bold, color, v.Values.First().Text)
						);
				}

				//Multi value variables not on axis
				if (showLegend)
				{
					var multis = PXMeta.Variables.Where(v => v.Values.Count > 1 && !v.Name.Equals(axisCode));
					if (multis.Any() && !(multis.Count() == 1 && multis.First().IsTime))
					{
						string s =
							String.Join(
								", ",
								PXMeta.Variables.Where(v => v.Values.Count > 1 && !v.Name.Equals(axisCode)).Select(v => v.Name.FirstLetterToUpper()).ToArray()
							);

						if (!first)
							currLoc = drawString(doDraw, graphics, currLoc, new DrawableSeparator(Font.Regular, color2));

						first = false;
						currLoc = drawString(doDraw, graphics, currLoc, new DrawableString(Font.Regular, color, s + ": "));
					}
				}
			}

			//Axis title for pie charts
			if (ChartTypes.Contains(SeriesChartType.Pie))
			{
				if (!first)
					currLoc = drawString(doDraw, graphics, currLoc, new DrawableSeparator(Font.Regular, color2));

				first = false;
				currLoc =
					drawString(doDraw, graphics, currLoc,
						new DrawableString(Font.Bold, color, axisCode.FirstLetterToUpper())
					);
			}

			return currLoc.Y + Font.Regular.Size + 2;
		}

		private PointF drawString(bool doDraw, Graphics g, PointF location, Font f, Color c, string s)
		{
			return drawString(doDraw, g, location, new DrawableString(f, c, s));
		}

		private PointF drawString(bool doDraw, Graphics g, PointF location, params DrawableString[] drawableStrings)
		{
			RectangleF bounds = measureStrings(g, drawableStrings);

			if (location.X > 0 && location.X + bounds.Right > Width)
			{
				location = new PointF(0, location.Y + bounds.Height + 2);

				if (drawableStrings.Length == 1 && drawableStrings[0] is DrawableSeparator)
					return location;
			}

			float x = location.X;
			if (doDraw)
				foreach (var ds in drawableStrings)
				{
					g.DrawString(ds.S, ds.F, new SolidBrush(ds.C), x, location.Y);
					x += measureStrings(g, ds).Width;
				}

			return new PointF(location.X + bounds.Width, location.Y);
		}

		private RectangleF measureStrings(Graphics g, params DrawableString[] drawableStrings)
		{
			RectangleF result = new RectangleF();

			foreach (var ds in drawableStrings)
			{
				ds.S = ds.S.Replace(' ', (char)160);

				CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, ds.S.Length) };

				RectangleF rectangleF = new RectangleF(new PointF(0, 0), new SizeF(10000, 10000));
				StringFormat stringFormat = new StringFormat();
				stringFormat.SetMeasurableCharacterRanges(ranges);

				Region[] regions = g.MeasureCharacterRanges(ds.S, ds.F, rectangleF, stringFormat);
				RectangleF bounds = regions.First().GetBounds(g);

				result.Height = bounds.Height;
				result.Width += bounds.Width;
			}

			return result;
		}

		private class DrawableSeparator : DrawableString
		{
			public DrawableSeparator(Font f, Color c)
				: base(f, c, " | ")
			{ }
		}

		private class DrawableString
		{
			public Font F;
			public Color C;
			public string S;

			public DrawableString(Font f, Color c, String s)
			{
				F = f;
				C = c;
				S = s;
			}
		}

		private string units = null;
		/// <summary>
		/// Gets or overrides the unit text.
		/// </summary>
		public string Units
		{
			get { return units ?? originalUnits; }
			set { units = value; }
		}

		private string originalUnits = "";
		/// <summary>
		/// Adds Statistics Denmark specific functionality to the initializement of data.
		/// </summary>
		/// <param name="px"></param>
		protected override PXModel initializePxData(PXModel px)
		{
			System.Threading.Thread.CurrentThread.CurrentUICulture =
				CultureInfo.CreateSpecificCulture(px.Meta.Language);

			px.LocalizeTimeTexts(
				Resources.DstChart.Quarter
				//Resources.DstChart.ResourceManager.GetString("Quarter", culture),
				//culture
			);

			base.initializePxData(px);

			originalUnits = px.Meta.ContentInfo.Units;

			return px;
		}

		/// <summary>
		/// Determines whether to move the unit to the top gridline for this chart.
		/// </summary>
		protected bool moveUnitToGridline
		{
			get
			{
				return
					showUnit &&
					!this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesRotated) &&
					!ChartTypes.Contains(SeriesChartType.Pie);
			}
		}

		/// <summary>
		/// Determines if unit should be shown in chart. Eg. "-" is not shown.
		/// </summary>
		protected override bool showUnit
		{
			get
			{
				return base.showUnit && !Units.Equals("-");
			}
		}

		/// <summary>
		/// Determines whether to move the unit to the axis.
		/// </summary>
		protected bool moveUnitToAxis
		{
			get
			{
				return
					showUnit &&
					this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesRotated);
			}
		}

		/// <summary>
		/// Adds Statistics Denmark specific functionality for adjusting the chart for displaying a populations pyramid.
		/// This method is in most cases called by PxMenu itself when needed.
		/// </summary>
		protected override void adjustForPopulationPyramid()
		{
			base.adjustForPopulationPyramid();

			if (showLegend)
			{
				ChartArea ca = ChartAreas.First();
				Legends.First().Position.X = ca.InnerPlotPosition.X + pixels(5);
			}
		}

		/// <summary>
		/// Class for storing a valuecode and a corresponding color.
		/// </summary>
		public class ValueColor
		{
			/// <summary>
			/// The value the color should be used for.
			/// </summary>
			public string ValueCode = null;

			public string VariableName = null;

			/// <summary>
			/// The color to be used.
			/// </summary>
			public Color Color;

			public ValueColor(string valueCode, Color color)
				: this(null, valueCode, color)
			{
			}

			public ValueColor(string variableName, string valueCode, Color color)
			{
				this.ValueCode = valueCode;
				this.Color = color;
				this.VariableName = variableName;
			}

			public string CodeString
			{
				get
				{
					return
						String.Format(
							"|{0}{1}|",
							VariableName != null ? VariableName + "|" : "",
							ValueCode
						);
				}
			}
		}
	}
}
