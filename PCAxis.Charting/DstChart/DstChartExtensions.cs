using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using PCAxis.Paxiom;

namespace PCAxis.Charting.InternalExtensions
{
	/// <summary>
	/// Extentsion methods for DstChart.
	/// </summary>
	public static class DstChartExtensions
	{
		/// <summary>
		/// Determines the pixel position of a custom label in a chart.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="findLabel">Which of the labels to find in the collection of custom labels. Can be specified by a Lambda expression in the form of a CustomLabelsCollection => one of them.</param>
		/// <returns></returns>
		public static double PixelPositionForGrid(this Axis a, Func<CustomLabelsCollection, CustomLabel> findLabel)
		{
			CustomLabel label = findLabel(a.CustomLabels);
			return a.ValueToPixelPosition(label.FromPosition + (label.ToPosition - label.FromPosition) / 2);
		}

		/// <summary>
		/// Make first letter upper case
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string FirstLetterToUpper(this string s)
		{
			if (s.Length < 2)
				return s.ToUpper();

			return s.Substring(0, 1).ToUpper() + (s.Length > 1 ? s.Substring(1) : "");
		}
	}
}
