using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using System.Windows.Forms.DataVisualization.Charting;

namespace PCAxis.Charting
{
	/// <summary>
	/// Extensions for creating a PxChart from a PXModel instance.
	/// </summary>
	public static class PxChartExtensionsCreate
	{
		/// <summary>
		/// Create a PxChart from a PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public static PxChart AsPxChart(this PXModel px)
		{
			return px.AsPxChart(null, null, 0, 0);
		}

		/// <summary>
		/// Create a PxChart as population pyramid from a PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="maleValueName">The name of the value for male data.</param>
		/// <param name="width">The width in pixels of the chart.</param>
		/// <param name="height">The height in pixels of the chart.</param>
		/// <returns></returns>
		public static PxChart AsPxPopulationPyramidChart(this PXModel px, string maleValueName, int width, int height)
		{
			return
				new PxChart(
					px,
					c =>
					{
						c.MaleValueNameForPopulationPyramid = maleValueName;
						c.Width = width;
						c.Height = height;
					}
				);
		}

		/// <summary>
		/// Create a PxChart from a PXModel instance.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="chartType">The type of chart.</param>
		/// <param name="autoMoveMostValuesVariableToX">Whether to move the variable with most values to the X-axis.</param>
		/// <param name="width">The width in pixels of the chart.</param>
		/// <param name="height">The height in pixels of the chart.</param>
		/// <returns></returns>
		public static PxChart AsPxChart(this PXModel px, SeriesChartType? chartType, bool? autoMoveMostValuesVariableToX, int width, int height)
		{
			return
				new PxChart(
					px,
					c =>
					{
						c.ChartType = chartType ?? SeriesChartType.Column;
						c.AutoMoveMostValuesVariableToX = autoMoveMostValuesVariableToX ?? true;
						c.Width = width;
						c.Height = height;
					}
				);
		}
	}
}
