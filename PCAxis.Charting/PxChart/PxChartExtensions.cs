using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace PCAxis.Charting.InternalExtensions
{
    /// <summary>
    /// Extension methods for PxChart.
    /// </summary>
    public static class PxChartExtensions
    {
        /// <summary>
        /// Returns whether the chart has any series from a group of series defined by ChartTypeGroup.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="group">ChartTypeGroup to check for.</param>
        /// <returns></returns>
        public static bool HasAnySeriesOfTypeGroup(this PxChart chart, ChartTypeGroup group)
        {
            //return chart.Series.Any(s => group.Contains(s.ChartType));
            return chart.ChartTypes.Any(t => group.Contains(t));
        }

        /// <summary>
        /// List of chart types used for handling groups of chart types with common characteristics.
        /// </summary>
        public class ChartTypeGroup : List<SeriesChartType>
        {
        }

        /// <summary>
        /// Charts that are presented as columns.
        /// </summary>
        public static ChartTypeGroup ChartTypesColumn =
            new ChartTypeGroup()
                {
                    SeriesChartType.Column,
                    SeriesChartType.StackedColumn,
                    SeriesChartType.StackedColumn100
                };

        /// <summary>
        /// Charts that can only display one series of data.
        /// </summary>
        public static ChartTypeGroup ChartTypesSingleSeriesLegend =
            new ChartTypeGroup()
                {
                    SeriesChartType.Pie,
                    SeriesChartType.Funnel
                };

        /// <summary>
        /// Charts that displays the distribution of data rather than the value itself.
        /// </summary>
        public static ChartTypeGroup ChartTypesDistribution =
            new ChartTypeGroup()
                {
                    SeriesChartType.Pie,
                    SeriesChartType.Doughnut,
                    SeriesChartType.StackedArea100,
                    SeriesChartType.StackedBar100,
                    SeriesChartType.StackedColumn100,
                    SeriesChartType.Pyramid,
                    SeriesChartType.Funnel
                };

        /// <summary>
        /// Charts that displays the values stacked rather than displaying each value.
        /// </summary>
        public static ChartTypeGroup ChartTypesStacked =
            new ChartTypeGroup()
            {
                SeriesChartType.StackedArea,
                SeriesChartType.StackedArea100,
                SeriesChartType.StackedBar,
                SeriesChartType.StackedBar100,
                SeriesChartType.StackedColumn,
                SeriesChartType.StackedColumn100
            };

        /// <summary>
        /// Charts than are displayed horizontally.
        /// </summary>
        public static ChartTypeGroup ChartTypesRotated =
            new ChartTypeGroup()
            {
                SeriesChartType.Bar,
                SeriesChartType.RangeBar,
                SeriesChartType.StackedBar,
                SeriesChartType.StackedBar100,
                SeriesChartType.Radar,
                SeriesChartType.Polar
            };

        /// <summary>
        /// Charts that should display the value 0 even when no values in the dataset are 0.
        /// </summary>
        public static ChartTypeGroup ChartTypesMustHaveZero =
            new ChartTypeGroup()
            {
                SeriesChartType.Column,
                SeriesChartType.Area,
                SeriesChartType.Bar,
                SeriesChartType.StackedArea,
                SeriesChartType.StackedArea100,
                SeriesChartType.StackedBar,
                SeriesChartType.StackedBar100,
                SeriesChartType.StackedColumn,
                SeriesChartType.StackedColumn100
            };

        /// <summary>
        /// Determines the number of pixels between the minimum and the maximum values on the axis.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double PixelSize(this Axis a)
        {
            return a.ValueToPixelPosition(a.Minimum).DistanceTo(a.ValueToPixelPosition(a.Maximum));
        }

        /// <summary>
        /// Determines the difference of minimum and maximum.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Size(this Axis a)
        {
            return a.Maximum.DistanceTo(a.Minimum);
        }

        /// <summary>
        /// Determines the difference between this double to another double.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n">The double to compare with.</param>
        /// <returns></returns>
        public static double DistanceTo(this double d, double n)
        {
            return Math.Abs(d - n);
        }

        /// <summary>
        /// Returns whether the chart has any values that has decimals.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool HasDecimalValues(this PxChart c)
        {
            return
                c.Series.Any(s => s.Points.Any(p => p.YValues.Any(v => v.HasDecimals())));
        }

        /// <summary>
        /// Determines whether this double has decimals.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool HasDecimals(this double d)
        {
            return
                d != Math.Floor(d);
        }

        /// <summary>
        /// Sets the interval of the axis as close as possible to a specific number of pixels taking into account that the axis is used for values and the rules for allowed interval values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pixelsForEachLabel">The number of pixels wanted between labels.</param>
        public static void SetIntervalForValuesByPixels(this Axis a, int pixelsForEachLabel)
        {
            a.SetIntervalForValuesByPixels(pixelsForEachLabel, null, null);
        }

        /// <summary>
        /// Sets the interval of the axis as close as possible to a specific number of pixels taking into account that the axis is used for values and the rules for allowed interval values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pixelsForEachLabel">The number of pixels wanted between labels.</param>
        /// <param name="numberOfDecimalsAllowed">The number of decimals allowed in the axis values. Null if it should not be checked.</param>
        /// <param name="intervalMinPctOfAxisSize">The smallest allowed interval in percentage of axis size. Null if it should not be checked.</param>
        public static void SetIntervalForValuesByPixels(this Axis a, int pixelsForEachLabel, int? numberOfDecimalsAllowed, int? intervalMinPctOfAxisSize)
        {
            int numberOfLabels = Convert.ToInt32(a.PixelSize() / pixelsForEachLabel);

            double i = Math.Max(Math.Abs(a.Maximum), Math.Abs(a.Minimum));

            i = i.AdjustUp();

            double size = a.Size();
            while (size / i < numberOfLabels)
            {
                double f = i.NumberSize() / 2;

                if (intervalMinPctOfAxisSize != null && (i - f).PctOf(size) < intervalMinPctOfAxisSize)
                    break;

                i -= f;

                if (i <= 0)
                {
                    i += f;
                    break;
                }
            }

            a.Interval =
                numberOfDecimalsAllowed == null
                ?
                i
                :
                Math.Max(i, numberOfDecimalsAllowed == 0 ? 1 : 1 / (10 * (double)numberOfDecimalsAllowed))
                ;

            if (a.Maximum != 0 && a.Maximum.ModWithDecimals(a.Interval) != 0)
                a.Maximum = a.Maximum.AdjustUpToInterval(a.Interval);

            if (a.Minimum != 0 && a.Minimum.ModWithDecimals(a.Interval) != 0)
                a.Minimum = a.Minimum.AdjustDownToInterval(a.Interval);

            if (a.Minimum < 0)
                a.IntervalOffset = Math.Abs(a.Minimum.ModWithDecimals(i));
        }

        /// <summary>
        /// Adjust number to current or next interval stop over number 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="interval">Interval for adjustment</param>
        /// <returns></returns>
        public static double AdjustUpToInterval(this double d, double interval)
        {
            if (d.ModWithDecimals(interval) == 0)
                return d;

            return d - d.ModWithDecimals(interval) + (d.IsPositive() ? interval : 0);
        }

        /// <summary>
        /// Adjust number to current or next interval stop under number
        /// </summary>
        /// <param name="d"></param>
        /// <param name="interval">interval for adjustment</param>
        /// <returns></returns>
        public static double AdjustDownToInterval(this double d, double interval)
        {
            if (d.ModWithDecimals(interval) == 0)
                return d;

            return d - d.ModWithDecimals(interval) - (d.IsNegative() ? interval : 0);
        }

        /// <summary>
        /// Sets the interval of the axis as close as possible to a specific number of pixels taking into account that the axis is used for labels and the rules for allowed interval values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pixelsForEachLabel">The number of pixels wanted for each label</param>
        public static void SetIntervalForLabelsByPixels(this Axis a, int pixelsForEachLabel)
        {
            int numberOfLabels = Convert.ToInt32(a.PixelSize() / pixelsForEachLabel);

            a.Interval = 1;
            if (numberOfLabels < a.Maximum)
                a.Interval = Convert.ToInt32(a.Size() / numberOfLabels);
        }

        private static double intervalForAdjust(this double d)
        {
            if (d == 0)
                return 0;

            d = Math.Abs(d);

            double size = d.NumberSize();

            return size / 10;
        }

        /// <summary>
        /// The appropriate maximum axis value for this maximum data value.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double FindMax(this double d)
        {
            double i = d.intervalForAdjust();
            return d.AdjustUp(i);
        }

        /// <summary>
        /// The appropriate minimum axis value for this minimum data value.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double FindMin(this double d)
        {
            double i = d.intervalForAdjust();
            return d.AdjustDown(i);
        }

        /// <summary>
        /// Returns whether this double is a positive number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsPositive(this double d)
        {
            return !d.IsNegative();
        }

        /// <summary>
        /// Returns whether this double is a negative number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsNegative(this double d)
        {
            return Math.Sign(d) == -1;
        }

        /// <summary>
        /// Converts doubles to decimals, calculates modulus and converts result back to double
        /// </summary>
        /// <param name="d"></param>
        /// <param name="x">Value</param>
        /// <returns></returns>
        public static double ModWithDecimals(this double d, double x)
        {
            return
                Convert.ToDouble(
                    Convert.ToDecimal(d) % Convert.ToDecimal(x)
                );
        }

        /// <summary>
        /// Returns the first axis value higher than this data value if the axis interval is the NumberSize() of this value.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double AdjustUp(this double d)
        {
            return d.AdjustUp(d.NumberSize());
        }
        /// <summary>
        /// Returns the first axis value higher than this data value for a specific axis interval.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="interval">The interval of the axis.</param>
        /// <returns></returns>
        public static double AdjustUp(this double d, double interval)
        {
            if (d == 0)
                return 1;
            else if (d.IsNegative())
                return Math.Floor(Math.Abs(d) / interval) * interval * Math.Sign(d);
            else
                return Math.Ceiling(Math.Abs(d) / interval) * interval * Math.Sign(d);
        }

        /// <summary>
        /// Returns the first axis value lower than this data value if the axis interval is the NumberSize() of this value.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double AdjustDown(this double d)
        {
            return d.AdjustDown(d.NumberSize());
        }
        /// <summary>
        /// Returns the first axis value lower than this data value for a specific axis interval.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="interval">The interval of the axis.</param>
        /// <returns></returns>
        public static double AdjustDown(this double d, double interval)
        {
            if (d == 0)
                return 0;
            else if (d.IsNegative())
                return Math.Ceiling(Math.Abs(d) / interval) * interval * Math.Sign(d);
            else
                return Math.Floor(Math.Abs(d) / interval) * interval * Math.Sign(d);
        }

        /// <summary>
        /// Returns the "size" of this value. Eg. 5.0 => 1.0, 18.0 => 10.0, 478.0 => 100.0 etc.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double NumberSize(this double n)
        {
            return Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(n))));
        }

        /// <summary>
        /// Returns how many percentage this value is of another value.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n">The double to compare to.</param>
        /// <returns></returns>
        public static double PctOf(this double d, double n)
        {
            return (d * 100) / n;
        }

        /// <summary>
        /// Determines the number of empty top interval sections on this axis with the supplied maximum data value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="max">The maximum data value shown in the chart.</param>
        /// <param name="interval">The interval for the axis.</param>
        /// <returns></returns>
        public static double EmptyTopIntervals(this Axis a, double max, double interval)
        {
            return Math.Floor(a.Maximum.DistanceTo(max) / interval);
        }

        /// <summary>
        /// Determines the number of empty bottom interval sections on this axis with the supplied minimum data value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="min">The minimum data value shown in the chart.</param>
        /// <param name="interval">The interval for the axis.</param>
        /// <returns></returns>
        public static double EmptyBottomIntervals(this Axis a, double min, double interval)
        {
            return Math.Floor(a.Minimum.DistanceTo(min).AsAbs() / interval);
        }

        /// <summary>
        /// Dertermines the number of pixels between the max datapoint value and the top grid.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="max">The maximum data value shown in the chart.</param>
        /// <returns></returns>
        public static double PixelsToMaximumPoint(this Axis a, double max)
        {
            return a.ValueToPixelPosition(a.MaximumGridLine()).DistanceTo(a.ValueToPixelPosition(max));
        }

        /// <summary>
        /// Dertermines the number of pixels between the min datapoint value and the bottom grid.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="min">The minimum data value shown in the chart.</param>
        /// <returns></returns>
        public static double PixelsToMinimumPoint(this Axis a, double min)
        {
            return a.ValueToPixelPosition(a.MinimumGridLine()).DistanceTo(a.ValueToPixelPosition(min));
        }

        /// <summary>
        /// Determines the number of pixels in the axis interval.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="interval">The interval for the axis.</param>
        /// <returns></returns>
        public static double PixelsInInterval(this Axis a, double interval)
        {
            return a.ValueToPixelPosition(a.Minimum).DistanceTo(a.ValueToPixelPosition(a.Minimum + interval));
        }

        /// <summary>
        /// Determines the highest gridline.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double MaximumGridLine(this Axis a)
        {
            return a.Maximum.AdjustDownToInterval(a.IntervalMajorGridOrAxis());
        }

        /// <summary>
        /// Determines the lowest gridline.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double MinimumGridLine(this Axis a)
        {
            return a.Minimum.AdjustUpToInterval(a.IntervalMajorGridOrAxis());
        }

        /// <summary>
        /// Returns the major grid interval if not NaN, else the axis interval
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double IntervalMajorGridOrAxis(this Axis a)
        {
            return
                a.Interval != 0 ? a.Interval : a.MajorGrid.Interval;
        }

        /// <summary>
        /// Returns this value as an absolute.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double AsAbs(this double d)
        {
            return Math.Abs(d);
        }

        /// <summary>
        /// Returns whether the double is NaN.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsNaN(this double d)
        {
            return Double.IsNaN(d);
        }

        /// <summary>
        /// Returns whether the double is not NaN.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsANumber(this double d)
        {
            return !d.IsNaN();
        }

        /// <summary>
        /// Returns the string with sections divided by newline
        /// </summary>
        /// <param name="s"></param>
        /// <param name="minSectionSize">The minimum size of each section</param>
        /// <returns></returns>
        public static string InsertBreaks(this string s, int minSectionSize)
        {
            StringBuilder sb = new StringBuilder();

            bool search = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (!search)
                {
                    if (i + minSectionSize > s.Length)
                    {
                        sb.Append(s.Substring(i));
                        break;
                    }

                    sb.Append(s.Substring(i, minSectionSize));
                    i += minSectionSize - 1;

                    search = true;
                }
                else
                {
                    if (s[i].Equals(' '))
                    {
                        sb.Append(Environment.NewLine);
                        search = false;
                    }
                    else
                        sb.Append(s[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a direction and a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector</typeparam>
        /// <param name="source">A sequence of values to order</param>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <param name="orderByAscending">Whether to sort in ascending order - otherwise descending</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderByDirection<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool orderByAscending)
        {
            return
                orderByAscending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }
    }
}
