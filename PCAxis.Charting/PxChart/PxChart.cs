using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using PCAxis.Charting.InternalExtensions;
using PCAxis.Paxiom;
using PCAxis.PxExtend;
using System.Text;

namespace PCAxis.Charting
{
	/// <summary>
	/// Displays a PXModel instance as a chart.
	/// </summary>
	public class PxChart : Chart
	{
		/// <summary>
		/// Only for usage in a form. If not used in a form, use a constructor with a signature including a PXModel instance.
		/// </summary>
		public PxChart()
		{
			this.PostPaint += new EventHandler<ChartPaintEventArgs>(PxChart_PostPaint);
		}

		/// <summary>
		/// Creates a PxChart from a PXModel instance.
		/// </summary>
		/// <param name="px">PXModel instance to create chart from.</param>
		public PxChart(PXModel px)
			: this(px, null)
		{ }

		/// <summary>
		/// Delegate for chart settings.
		/// </summary>
		/// <param name="pxChart">The instance of PxChart to apply settings to.</param>
		public delegate void PxChartSettings(PxChart pxChart);
		/// <summary>
		/// Creates PxChart with chart settings.
		/// </summary>
		/// <param name="px">PXModel instance to create chart from.</param>
		/// <param name="settings">Settings as PxChartSettings. Eg. as a Lambda expression.</param>
		public PxChart(PXModel px, PxChartSettings settings)
			: this()
		{
			initialize(px, settings);
		}

		void PxChart_PostPaint(object sender, ChartPaintEventArgs e)
		{
			if (e.ChartElement is PxChart)
				additionalGraphics(e.ChartGraphics.Graphics);
		}

		/// <summary>
		/// Clear chart and add data from PXModel instance.
		/// </summary>
		/// <param name="px">PXModel to add to chart.</param>
		public virtual void AddPxData(PXModel px)
		{
			AddPxData(px, null);
		}

		/// <summary>
		/// Clear chart and add data from PXModel instance.
		/// </summary>
		/// <param name="px">PXModel to add to chart.</param>
		/// <param name="settings">Settings as PxChartSettings. Eg. as a Lambda expression.</param>
		public virtual void AddPxData(PXModel px, PxChartSettings settings)
		{
			Titles.Clear();
			Series.Clear();
			Legends.Clear();
			ChartAreas.Clear();

			initialize(px, settings);
		}

		/// <summary>
		/// Font for usage in PxChart.
		/// </summary>
		public class PxChartFont
		{
			/// <summary>
			/// FontFamily used in PxChart instance.
			/// </summary>
			public FontFamily Family = new FontFamily("Arial");

			/// <summary>
			/// Font size for regular texts in PxChart instance.
			/// </summary>
			public float SizeRegular = 14;

			/// <summary>
			/// Font size for small texts in PxChart instance.
			/// </summary>
			public float SizeSmall = 11;

			/// <summary>
			/// Font color for title in PxChart instance.
			/// </summary>
			public Color TitleTextColor = Color.Black;

			/// <summary>
			/// Font color for info text as Brush in PxChart instance.
			/// </summary>
			public Brush InfoTextBrush = Brushes.Black;

			/// <summary>
			/// Returns font for regular texts in PxChart instance.
			/// </summary>
			public Font Regular
			{
				get { return new Font(Family, SizeRegular, GraphicsUnit.Pixel); }
			}

			/// <summary>
			/// Returns font for small texts in PxChart instance.
			/// </summary>
			public Font Small
			{
				get { return new Font(Family, SizeSmall, GraphicsUnit.Pixel); }
			}

			/// <summary>
			/// Returns font for bold texts in PxChart instance.
			/// </summary>
			public Font Bold
			{
				get { return new Font(Regular, FontStyle.Bold); }
			}
		}
		/// <summary>
		/// The PxChartFont for this PxChart.
		/// </summary>
		public new PxChartFont Font = new PxChartFont();

		/// <summary>
		/// Text for displaying information such as copyright notice, source etc.
		/// </summary>
		public string InfoText = "";
		/// <summary>
		/// Override in derived class to calculate space in percentage of the height used for InfoText.
		/// </summary>
		protected float SpaceUsedForInfoText
		{
			get { return InfoText != "" ? (Font.SizeSmall + 4f) * 100f / (float)Height : 0f; }
		}

		private string title = null;

		/// <summary>
		/// Title to use instead of default title. Set to null for default title.
		/// </summary>
		public string OverrideTitle = null;

		/// <summary>
		/// Returns the height used for title.
		/// </summary>
		public virtual float TitleHeight
		{
			get
			{
				if (OverrideTitle == "")
					return 0;

				return
					(Font.SizeRegular + 2) * 100 / Height;
			}
		}

		/// <summary>
		/// Sets or gets the title displayed in the chart.
		/// </summary>
		public string Title
		{
			get
			{
				return title ?? "";
			}
			set
			{
				title = value;

				if (Titles.Count > 0)
					Titles.First().Text = title;
			}
		}

		/// <summary>
		/// The chart types of the series.
		/// </summary>
		public SeriesChartType[] ChartTypes = new SeriesChartType[] { };
		/// <summary>
		/// Sets a chart type to use for all series.
		/// </summary>
		public SeriesChartType ChartType
		{
			set
			{
				ChartTypes = new SeriesChartType[] { value };
			}
		}
		/// <summary>
		/// Returns the chart type to be used for a specific series in the chart.
		/// </summary>
		/// <param name="seriesNumber">The number of the series.</param>
		/// <returns></returns>
		protected SeriesChartType ChartTypeForSeries(int seriesNumber)
		{
			if (ChartTypes.Length == 0)
				return SeriesChartType.Column;

			return seriesNumber < ChartTypes.Length ? ChartTypes[seriesNumber] : ChartTypes.Last();
		}

		/// <summary>
		/// Whether to move the variable with most values to the X-axis.
		/// </summary>
		public bool AutoMoveMostValuesVariableToX = true;

		public string SpecificVariableOnX = null;

		private string pieLabelStyle = "Disabled";
		/// <summary>
		/// Used for displaying a pie chart. Gets or sets the style of labels.
		/// </summary>
		public string PieLabelStyle
		{
			get { return pieLabelStyle; }
			set { pieLabelStyle = value; }
		}

		/// <summary>
		/// Used for displaying a population pyramid. The name of the value containing data for males. Males are presented on the left hand side of the chart in compliance with international standards. Use null when another chart type is wanted.
		/// </summary>
		public string MaleValueNameForPopulationPyramid = null;
		/// <summary>
		/// Whether the chart is shown as a population pyramid.
		/// </summary>
		public bool AsPopulationPyramid
		{
			get { return MaleValueNameForPopulationPyramid != null; }
		}

		/// <summary>
		/// The number of the first series to show according to the secondary axis. Set to 0 if a secondary axis is not wanted.
		/// </summary>
		public int SecondaryAxisFromSeriesNumber = 0;
		/// <summary>
		/// String to add to the series name of series shown according to the secondary axis.
		/// </summary>
		public string SecondaryAxisAddToSeriesName = null;
		/// <summary>
		/// String to add to the series name of series shown according to the primary axis.
		/// </summary>
		public string SecondaryAxisAddToPrimarySeriesName = null;
		/// <summary>
		/// Override in derived class for initalizing the chart.
		/// You might wanna include execution of the base class.
		/// </summary>
		/// <param name="px">PXModel instance with data.</param>
		/// <param name="settings">Settings as eg. a Lambda expression</param>
		protected void initialize(PXModel px, PxChartSettings settings)
		{
			if (px != null && px.Meta.Copyright)
				InfoText = "© " + px.Meta.Source;

			settings?.Invoke(this);

			if (px != null && AsPopulationPyramid)
			{
				//px = px.PrepareForPopulationPyramid(MaleValueNameForPopulationPyramid);

				if (px.Meta.Variables.Count(x => x.Values.Count > 1) <= 2)
				{
					ChartType = SeriesChartType.StackedBar;
					AutoMoveMostValuesVariableToX = true;
				}
				else
				{
					ChartType = SeriesChartType.Bar;
					AutoMoveMostValuesVariableToX = false;
					px =
						px.PivotSpecificVariableToAloneInHead(
							px.Meta.Variables.First(x => !x.IsTime && !x.Values.Any(z => z.Value.ToLower() == MaleValueNameForPopulationPyramid.ToLower())).Name
						);
				}
			}

			additionalFunctionalityFirst();

			if (px != null)
			{
				initializePxData(px);
			}

			addTitle();
			addLegend();
			addChartArea();

			if (SecondaryAxisFromSeriesNumber > 0)
			{
				if (SecondaryAxisAddToSeriesName == null)
					throw new PxChartExceptions.PxChartException("SecondaryAxisAddToSeriesName must not be null, when moving series to secondary axis.");

				ChartAreas["Main"].AxisY2 = templateAxisY;

				foreach (Series s in Series.Skip(SecondaryAxisFromSeriesNumber - 1))
				{
					s.YAxisType = AxisType.Secondary;
					s.Name += SecondaryAxisAddToSeriesName;
				}

				if (SecondaryAxisAddToPrimarySeriesName != null)
					foreach (Series s in Series.Take(SecondaryAxisFromSeriesNumber - 1))
						s.Name += SecondaryAxisAddToPrimarySeriesName;
			}

			initializeDataLayout();

			additionalFunctionalityLast();
		}

		/// <summary>
		/// Override in derived class to execute code just after settings and populations pyramid preperation.
		/// </summary>
		protected virtual void additionalFunctionalityFirst()
		{
		}

		/// <summary>
		/// Override in derived class to add title. 
		/// You might wanna include execution of the base method.
		/// </summary>
		protected virtual void addTitle()
		{
			Titles.Add(
				new Title(Title, Docking.Top)
				{
					Position = new ElementPosition(0, 0, 100, TitleHeight),
					Alignment = ContentAlignment.TopLeft,
					ForeColor = Font.TitleTextColor,
					Font = Font.Regular,
					Text = Title
				}
			);
		}

		/// <summary>
		/// Gets or sets whether to hide the legend.
		/// </summary>
		public bool HideLegend = false;
		/// <summary>
		/// Returns whether legend should be shown.
		/// </summary>
		protected virtual bool showLegend
		{
			get
			{
				if (HideLegend)
					return false;

				if (ChartTypes.Contains(SeriesChartType.Pie) && PieLabelStyle.ToLower() == "outside")
					return false;

				if (Series.Count == 1 && Series.First().Name == "Series 1" && !PxChartExtensions.ChartTypesSingleSeriesLegend.Contains(ChartTypeForSeries(0)))
					return false;

				return true;
			}
		}

		private float? legendHeightAsPctOfChart = null;
		/// <summary>
		/// The height of the legend in percentage of the chart. Set to null if PxMenu should determine the height.
		/// </summary>
		public float? LegendHeightAsPctOfChart
		{
			get { return legendHeightAsPctOfChart; }
			set
			{
				if (legendHeightInPixels != null)
					throw new PxChartExceptions.SettingsException("LegendHeightInPixels must be null when setting LegendHeightAsPctOfChart.");

				legendHeightAsPctOfChart = value;
			}
		}

		private float? legendHeightInPixels = null;
		/// <summary>
		/// Height in pixels for area available for legend. Set to null if PxChart should estimate height.
		/// </summary>
		public float? LegendHeightInPixels
		{
			get { return legendHeightInPixels; }
			set
			{
				if (legendHeightAsPctOfChart != null)
					throw new PxChartExceptions.SettingsException("LegendHeightAsPctOfChart must be null when setting LegendHeightInPixels.");

				legendHeightInPixels = value;
			}
		}

		protected float? _legendHeight = null;

		/// <summary>
		///  Returns the height for legend area
		/// </summary>
		protected float legendHeight
		{
			get
			{
				if (_legendHeight != null)
				{
					return (float)_legendHeight;
				}

				if (legendHeightInPixels != null)
					return (float)legendHeightInPixels * 100 / Height;

				if (legendHeightAsPctOfChart != null)
					return (float)legendHeightAsPctOfChart;

				Graphics g = this.CreateGraphics();

				float[] widths =
					(
						this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesSingleSeriesLegend) ?
						Series.First().Points.Select(p => p.AxisLabel) :
						Series.Select(s => s.Name)
					).Select(
						t => g.MeasureString(t, Font.Small).Width + 42
					).ToArray();

				var rows = 0;

				float[] rowWidths;
				do
				{
					rows++;

					rowWidths =
						Enumerable.Range(1, rows)
							.Select(
								rowNumber =>
									widths
										.Where((width, i) => (i + 1 - rowNumber) % rows == 0)
										.Sum()
							)
							.ToArray();
				} while (rows < widths.Length && rowWidths.Max() > Width);

				return (float)(_legendHeight = rows * (Font.SizeSmall * 1.8f) * 100 / Height);
			}
		}

		/// <summary>
		/// Override in derived class to add legend. 
		/// You might wanna include execution of the base method.
		/// </summary>
		protected virtual void addLegend()
		{
			if (showLegend)
			{
				Legends.Add(
					new Legend("Main")
					{
						Position = new ElementPosition(0, 98 - legendHeight - SpaceUsedForInfoText, 100, legendHeight),
						LegendItemOrder = LegendItemOrder.ReversedSeriesOrder
					}
				);
			}
		}

		/// <summary>
		/// The number of decimals to use on the value axis. Set to -1 if it should be determined by the current threads culture.
		/// </summary>
		public int DecimalPlaces = -1;
		private string axisLabelFormat
		{
			get
			{
				if (!AllowDecimalInterval)
					return "0";

				if (DecimalPlaces < 0)
					return "N";

				return "#,##0." + "".PadRight(DecimalPlaces, '0');
			}
		}

		/// <summary>
		/// Whether to show guidelines.
		/// </summary>
		public bool ShowGuideLines = false;
		/// <summary>
		/// Override in derived class for creating template for creating the X-axis.
		/// You might wanna use the base property and then make adjustments.
		/// </summary>
		protected virtual Axis templateAxisX
		{
			get
			{
				return
					new Axis()
					{
						LabelStyle =
							new LabelStyle()
							{
								Font = Font.Small,
								Format = axisLabelFormat
							},
						//IsLabelAutoFit = false,
						LabelAutoFitStyle =
							!ChartTypes.Contains(SeriesChartType.Bar)
								? LabelAutoFitStyles.WordWrap | LabelAutoFitStyles.StaggeredLabels
								: LabelAutoFitStyles.WordWrap,
						MajorTickMark =
							new TickMark()
							{
								Enabled = false
							},
						MajorGrid =
							new Grid()
							{
								Enabled = ShowGuideLines
							},
						IntervalAutoMode = IntervalAutoMode.VariableCount
					};
			}
		}

		/// <summary>
		/// Override in derived class for creating template for creating the Y-axis.
		/// You might wanna use the base property and then make adjustments.
		/// </summary>
		protected virtual Axis templateAxisY
		{
			get
			{
				return
					new Axis()
					{
						IsStartedFromZero =
							this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesMustHaveZero),
						LabelStyle =
							new LabelStyle()
							{
								Font = Font.Small,
								Format = axisLabelFormat
							},
						LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels,
						MajorTickMark =
							new TickMark()
							{
								Enabled = false
							},
						LineWidth = 0,
						MajorGrid =
							new Grid()
							{
								Enabled = true
							},
						IntervalAutoMode = IntervalAutoMode.VariableCount
					};
			}
		}

		/// <summary>
		/// Override in derived class to add ChartArea.
		/// You might wanna include execution of the base method.
		/// </summary>
		protected virtual void addChartArea()
		{
			float chartSpace = showLegend ? 99 - legendHeight : 86;
			chartSpace -= TitleHeight;
			chartSpace -= SpaceUsedForInfoText;

			ChartArea a =
				new ChartArea("Main")
				{
					Position = new ElementPosition(0, TitleHeight, 100, chartSpace),
					AxisX = templateAxisX,
					AxisY = templateAxisY
				};

			ChartAreas.Add(a);
		}

		/// <summary>
		/// Override in derived class to add additional graphics to the chart.
		/// You might wanna include execution of the base method which adds the InfoText if not "".
		/// </summary>
		/// <param name="graphics">The charts Graphics object of the.</param>
		protected virtual void additionalGraphics(Graphics graphics)
		{
			if (InfoText != "")
			{
				graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

				graphics.DrawString(
					InfoText,
					Font.Small,
					Font.InfoTextBrush,
					3,
					Height - 5 - Font.Small.Height
				);
			}
		}

		/// <summary>
		/// Settings for axes to use instead of the ones determined by PxMenu.
		/// </summary>
		public AxesSettings OverrideAxesSettings = null;

		/// <summary>
		/// Sets whether to adjust the axes further according to an internal set of directions in PxChart.
		/// </summary>
		public bool AdjustAxes = false;

		/// <summary>
		/// Sets whether to sort the dataseries. Will only take effect when there's only one series in the chart dataset.
		/// </summary>
		public SortType SortDataPoints = SortType.None;

		/// <summary>
		/// Describes the way data is sorted in a one-series-chart
		/// </summary>
		public enum SortType
		{
			/// <summary>
			/// The data points are not sorted
			/// </summary>
			None,
			/// <summary>
			/// The data points are sorted in descending order
			/// </summary>
			Descending,
			/// <summary>
			/// The data points are sorted in ascending order
			/// </summary>
			Ascending
		}

		/// <summary>
		/// Override in derived class to execute as the last part of the initialization.
		/// You might wanna include execution of the base method.
		/// </summary>
		protected virtual void additionalFunctionalityLast()
		{
			if (OverrideAxesSettings != null)
				ChartAreas["Main"].AxisY = OverrideAxesSettings.ApplyTo(this, ChartAreas["Main"].AxisY);
			else if (AdjustAxes)
				adjust();

			if (SortDataPoints != SortType.None && Series.Count == 1)
				sortDataPoints();
		}

		/// <summary>
		/// The number of pixels wanted for each label shown horizontally - taken into account the rules for allowed interval values.
		/// </summary>
		public int PixelsGuideForEachLabel = 0;

		/// <summary>
		/// The number of pixels wanted between labels shown vertically - taken into account the rules for allowed interval values for rotated chart types.
		/// </summary>
		public int PixelsGuideForEachLabelRotated = 0;

		/// <summary>
		/// The number of pixels wanted between labels shown horizontally for a population pyramid - taken into account the rules for allowed interval values for rotated chart types.
		/// </summary>
		public int PixelsGuideForEachLabelPopulation = 0;

		private int pixelsForLabelX
		{
			get
			{
				if (!this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesRotated))
					return PixelsGuideForEachLabel != 0 ? PixelsGuideForEachLabel : pixelsByFontSize(50);
				else
					return PixelsGuideForEachLabelRotated != 0 ? PixelsGuideForEachLabelRotated : pixelsByFontSize(20);
			}
		}

		private int pixelsForLabelY
		{
			get
			{
				if (AsPopulationPyramid)
					return PixelsGuideForEachLabelPopulation != 0 ? PixelsGuideForEachLabelPopulation : pixelsByFontSize(100);

				throw new NotImplementedException("Label adjust only implemented for population pyramids.");
			}
		}

		private int pixelsByFontSize(int pixels)
		{
			return pixels * (Convert.ToInt32(Font.SizeSmall) / 11);
		}

		/// <summary>
		/// Whether to allow decimals values on the value axis.
		/// </summary>
		public bool AllowDecimalInterval = true;

		private void adjust()
		{
			if (AsPopulationPyramid)
				return;

			SaveImage(Stream.Null, ChartImageFormat.Png);

			adjustAxis(ChartAreas["Main"].AxisX);
			adjustAxis(ChartAreas["Main"].AxisY);

			if (Series.Any(x => x.YAxisType == AxisType.Secondary))
				adjustAxis(ChartAreas["Main"].AxisY2);
		}

		/// <summary>
		/// Enum used for describing the type of an axis as either showing labels og values.
		/// </summary>
		public enum AxisLabelOrValue
		{
			/// <summary>
			/// The items on the axis are labels.
			/// </summary>
			Label,
			/// <summary>
			/// The items on the axis are values.
			/// </summary>
			Value
		}

		/// <summary>
		/// Adjusts an axis according to several rules implemented in PxChart.
		/// </summary>
		/// <param name="axis">The axis to adjust.</param>
		private void adjustAxis(Axis axis)
		{
			AxisLabelOrValue axisType = AxisLabelOrValue.Label;

			if (axis == ChartAreas["Main"].AxisY || axis == ChartAreas["Main"].AxisY2)
				axisType = AxisLabelOrValue.Value;

			if (axisType == AxisLabelOrValue.Label)
			{
				axis.SetIntervalForLabelsByPixels(pixelsForLabelX);
				return;
			}

			var currentType = axis == ChartAreas["Main"].AxisY ? AxisType.Primary : AxisType.Secondary;
			var series = Series.Where(x => x.YAxisType == currentType).ToArray();

			if (!series.Any())
				return;

			if (OverrideAxesSettings != null)
			{
				OverrideAxesSettings.ApplyTo(this, axis);
				return;
			}

			double min = 0;
			double max = 0;

			if (series.Any(x => PxChartExtensions.ChartTypesDistribution.Contains(x.ChartType)))
			{
				min = 0;
				max = 100;
			}
			else
			{
				if (series.Any(x => PxChartExtensions.ChartTypesStacked.Contains(x.ChartType)))
				{
					var points = series.First().Points.Count;

					double[] pos = null;
					double[] neg = null;

					foreach (var s in series)
					{
						for (int i = 0; i < points; i++)
						{
							double v = s.Points[i].YValues.Max();

							if (v.IsPositive())
							{
								if (pos == null)
									pos = new double[points];

								pos[i] += v;
							}
							else
							{
								if (neg == null)
									neg = new double[points];

								neg[i] += v;
							}
						}
					}

					max = pos != null ? pos.Max() : neg.Max();

					min = neg != null ? neg.Min() : pos.Min();
				}
				else
				{
					if (series.Any(x => x.Points.Any(y => !y.IsEmpty)))
					{
						max = series.Where(s => s.Points.Any(p => !p.IsEmpty)).Max(x => x.Points.Where(p => !p.IsEmpty).Max(p => p.YValues.Max()));
						min = series.Where(s => s.Points.Any(p => !p.IsEmpty)).Min(x => x.Points.Where(p => !p.IsEmpty).Min(p => p.YValues.Min()));
					}
				}

				if (series.Any(x => PxChartExtensions.ChartTypesMustHaveZero.Contains(x.ChartType)))
				{
					min = min < 0 ? min : 0;

					if (max < 0)
						max = 0;
				}
			}

			if (!this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesDistribution))
			{
				double interval = axis.IntervalMajorGridOrAxis();

				if (interval.IsANumber())
				{
					//Adjust axis so that whole area is within axis' min and max
					axis.Maximum = Math.Max(max, axis.Maximum.AdjustUpToInterval(interval));
					axis.Minimum = Math.Min(min, axis.Minimum.AdjustDownToInterval(interval));
					//Set axis own interval so that it complies with axis' min and max
					axis.Interval = interval;

					//Remove extra empty intervals 
					double emptyTopIntervals = axis.EmptyTopIntervals(max, interval);
					if (emptyTopIntervals > 0)
						axis.Maximum -= interval * emptyTopIntervals;

					double emptyBottomIntervals = axis.EmptyBottomIntervals(min, interval);
					if (emptyBottomIntervals > 0)
						axis.Minimum += interval * emptyBottomIntervals;

					//Add an extra interval if data close to end grid lines
					if (axis.Maximum != 0 && axis.PixelsToMaximumPoint(max) < 4)
						axis.Maximum += interval;

					if (axis.Minimum != 0 && axis.PixelsToMinimumPoint(min) < 4)
						axis.Minimum -= interval;

					//Check if data is within the grid area
					if (max > axis.MaximumGridLine() || max > axis.Maximum)
						throw new PxChartExceptions.PxChartException("Value out of chart.");

					if (min < axis.MinimumGridLine() || min < axis.Minimum)
						throw new PxChartExceptions.PxChartException("Value out of chart.");
				}
			}
		}

		protected int? MaximumNumberOfSeriesAllowed { get; set; }

		public delegate void MaximumNumberOfSeriesExceeded(int actual, int maximum);
		protected event MaximumNumberOfSeriesExceeded OnMaximumNumberOfSeriesExceeded;

		public void SetMaximumNumberOfSeries(int allowedSeries, PxChart.MaximumNumberOfSeriesExceeded onExceeded)
		{
			MaximumNumberOfSeriesAllowed = allowedSeries;
			OnMaximumNumberOfSeriesExceeded += onExceeded;
		}

		/// <summary>
		/// Whether to eliminate non-eliminable variables with only one value selected.
		/// </summary>
		public bool EliminateAllSingleValueVariables = false;

		/// <summary>
		/// Override in derived class to initialize the chart with data from a PXModel instance.
		/// The method returns the altered PXModel instance for usage in the derived class, after execution of the base class.
		/// </summary>
		/// <param name="px">The PXModel instance containing the data.</param>
		protected virtual PXModel initializePxData(PXModel px)
		{
			List<Variable> eliminated = new List<Variable>();

			if (SpecificVariableOnX != null && px.Meta.Variables.Any(v => v.Name.Equals(SpecificVariableOnX)))
				px = px.PivotSpecificVariableToAloneInHead(SpecificVariableOnX);
			else if (AutoMoveMostValuesVariableToX && !px.HasMostValuesVariableLastInHead())
				px = px.PivotSpecificVariableToAloneInHead(px.MostValuesVariable().Name);

			if (Series.Count > 0)
				throw new PxChartExceptions.AddingDataToPxChartException("Can't add data from PXModel to a PxChart that already contains data.");

			eliminated = new List<Variable>();

			foreach (Variable v in px.Meta.Variables.Where(x => x.Values.Count == 1 && (EliminateAllSingleValueVariables || x.Elimination)))
				if (px.Meta.Variables.Count > 1)
				{
					eliminated.Add(v);
					px = px.Eliminate(v.Code);
				}

			if (ChartTypes.Contains(SeriesChartType.Pie) && px.Meta.Variables.Count > 1)
			{
				throw new PxChartExceptions.PxChartException("Can't create PxChart with more than one variable as Pie Chart.");
			}

			DataBindCrossTable(
				px.AsDataTableCompressed(null, AsPopulationPyramid ? MaleValueNameForPopulationPyramid : null).AsEnumerable(),
				"Series",
				"Label",
				"DATA",
				"Codes=CODES,Text=TEXT"
			);

			if (MaximumNumberOfSeriesAllowed != null && Series.Count > MaximumNumberOfSeriesAllowed)
			{
				OnMaximumNumberOfSeriesExceeded?.Invoke(Series.Count, (int)MaximumNumberOfSeriesAllowed);
			}

			if (OverrideTitle != null)
			{
				Title = OverrideTitle;
			}
			else
			{
				Title = px.Meta.Title + ". " + (eliminated.Count > 0 ? String.Join(", ", eliminated.Select(x => x.Values.First().Value).ToArray()) : "");

				if (!px.Meta.ContentInfo.Units.Equals(String.Empty) && showUnit)
					Title += ". " + px.Meta.ContentInfo.Units;
			}

			if (AdjustAxes)
				if (this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesRotated))
					foreach (var p in Series.First().Points.Where(p => p.AxisLabel.Length > 30))
						p.AxisLabel = p.AxisLabel.InsertBreaks(30);

			return px;
		}

		/// <summary>
		/// Override in derived class to initialize the chart data layout.
		/// You might wanna include execution of the base class.
		/// </summary>
		protected virtual void initializeDataLayout()
		{
			this.SaveImage(Stream.Null, ChartImageFormat.Png);

			TextAntiAliasingQuality = TextAntiAliasingQuality.High;

			for (int i = 0; i < Series.Count; i++)
			{
				Series[i].ChartArea = "Main";
				Series[i].ChartType = ChartTypeForSeries(i);

				Series[i].BorderWidth =
					ChartTypeForSeries(i) == SeriesChartType.Line && ChartTypes.Any(t => t != SeriesChartType.Line) ? 5 : 2;

				Series[i]["MinimumRelativePieSize"] = "70";

				if (ChartTypeForSeries(i) == SeriesChartType.Pie)
				{
					Series[i]["PieStartAngle"] = "270";

					for (int p = 0; p < Series[i].Points.Count; p++)
						Series[i].Points[p]["PieLabelStyle"] = PieLabelStyle;

					int decimals = DecimalPlaces >= 0 ? DecimalPlaces : Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalDigits;
					Series[0].Label = "#VALX (#PERCENT{P" + decimals + "})";

					//Series[0].Label = "#VALY #VALX (#PERCENT{P0})";

				}
			}

			if (AsPopulationPyramid)
				adjustForPopulationPyramid();
		}

		/// <summary>
		/// Override in derived class to determine if unit should be shown in chart.
		/// You might wanna include execution of the base class.
		/// </summary>
		protected virtual bool showUnit
		{
			get
			{
				return !this.HasAnySeriesOfTypeGroup(PxChartExtensions.ChartTypesDistribution);
			}
		}

		/// <summary>
		/// Override in derived class to add change Population Pyramids.
		/// You might wanna include execution of the base class.
		/// </summary>
		protected virtual void adjustForPopulationPyramid()
		{
			SaveImage(Stream.Null, ChartImageFormat.Png);

			if (showLegend)
			{
				if (Series.First().Name.ToLower() == MaleValueNameForPopulationPyramid.ToLower())
					Legends.First().LegendItemOrder = LegendItemOrder.ReversedSeriesOrder;
				else
					Legends.First().LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;
			}

			Axis axisX = ChartAreas["Main"].AxisX;
			//To show lowest age interval
			axisX.IntervalOffset = 1;

			if (AdjustAxes)
				adjustAxis(axisX);
			//axisX.SetIntervalForLabelsByPixels(pixelsForLabelX);

			Axis axisY = ChartAreas["Main"].AxisY;
			if (OverrideAxesSettings == null)
			{
				double max = Series.Max(x => x.Points.Max(p => Math.Abs(p.YValues.Max())));
				max = max.FindMax();
				axisY.Minimum = max * -1;
				axisY.Maximum = max;

				axisY.SetIntervalForValuesByPixels(pixelsForLabelY);
			}
			else
			{
				if (OverrideAxesSettings != null)
					axisY = OverrideAxesSettings.ApplyTo(this, axisY);
			}

			if (AdjustAxes)
				adjustAxis(ChartAreas["Main"].AxisY);

			axisY.Maximum = Math.Max(axisY.Minimum.AsAbs(), axisY.Maximum.AsAbs());
			axisY.Minimum = axisY.Maximum * -1;

			axisY.CustomLabels.Clear();
			for (double i = axisY.Maximum * -1; i <= axisY.Maximum; i += axisY.Interval)
			{
				axisY.CustomLabels.Add(
					new CustomLabel()
					{
						LabelMark = LabelMarkStyle.SideMark,
						Text = Math.Abs(i).ToString(axisY.LabelStyle.Format),
						FromPosition = i - axisY.Interval / 2,
						ToPosition = i + axisY.Interval / 2
					}
				);
			}
		}

		/// <summary>
		/// Sorts the charts data by descending values.
		/// This can only be done for charts cotaining one series only.
		/// </summary>
		[Obsolete("This should be set using the Sort-property on chart instance creation (in the PxChartSettings).")]
		public void Sort()
		{
			SortDataPoints = SortType.Descending;
			sortDataPoints();
		}

		private void sortDataPoints()
		{
			if (SortDataPoints == SortType.None)
				return;

			if (Series.Count != 1)
				throw new PxChartExceptions.PxChartException("Sort can only be used on datasets with only one series.");

			//from p in Series.First().Points
			//orderby p.YValues.First() descending
			//select p
			List<DataPoint> sorted =
				Series.First().Points.OrderByDirection(p => p.YValues.First(), SortDataPoints == SortType.Ascending).ToList();

			Series.First().Points.Clear();

			foreach (var p in sorted)
				Series.First().Points.Add(p);
		}
	}

	/// <summary>
	/// Class for storing and applying settings for a primary and a secondary chart axis.
	/// </summary>
	public class AxesSettings
	{
		/// <summary>
		/// Settings for the primary axis.
		/// </summary>
		public AxisSettings Primary;
		/// <summary>
		/// Settings for the secondary axis.
		/// </summary>
		public AxisSettings Secondary;

		/// <summary>
		/// Applies settings to a specific axis in a specific chart. 
		/// </summary>
		/// <param name="chart">The chart that contain the axis on which the settings should be applied.</param>
		/// <param name="axis">The axis on which the settings should be applied.</param>
		/// <returns></returns>
		public Axis ApplyTo(PxChart chart, Axis axis)
		{
			if (axis == chart.ChartAreas["Main"].AxisY && Primary != null)
				return Primary.ApplyTo(axis);
			else if (axis == chart.ChartAreas["Main"].AxisY2 && Secondary != null)
				return Secondary.ApplyTo(axis);
			else
				return null;
		}
	}

	/// <summary>
	/// Class for storing settings for a chart axis.
	/// </summary>
	public class AxisSettings
	{
		/// <summary>
		/// The minimum value of the axis.
		/// </summary>
		public double Minimum = Double.NaN;
		/// <summary>
		/// The maximum value of the axis.
		/// </summary>
		public double Maximum = Double.NaN;
		/// <summary>
		/// The interval of the axis.
		/// </summary>
		public double Interval = Double.NaN;
		/// <summary>
		/// The interval offset value of the axis.
		/// </summary>
		public double IntervalOffset = Double.NaN;

		/// <summary>
		/// Applies settings to a specific axis instance. 
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public Axis ApplyTo(Axis axis)
		{
			axis.Minimum = Minimum;
			axis.Maximum = Maximum;
			axis.Interval = Interval;
			axis.IntervalOffset = IntervalOffset;
			return axis;
		}
	}
}
