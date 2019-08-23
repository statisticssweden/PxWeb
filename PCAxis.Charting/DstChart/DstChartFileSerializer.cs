using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using PCAxis.Paxiom;

namespace PCAxis.Charting
{
	/// <summary>
	/// Serializer for creating DstChart PNG-file
	/// </summary>
	public class DstChartFileSerializer : IPXModelStreamSerializer
	{
		PxChart.PxChartSettings settings;

		/// <summary>
		/// Serializer for creating standard DstChart PNG-file
		/// </summary>
		public DstChartFileSerializer()
		{
		}

		/// <summary>
		/// Serializer for creating DstChart PNG-file with settings
		/// </summary>
		/// <param name="settingsAsLambda"></param>
		public DstChartFileSerializer(PxChart.PxChartSettings settingsAsLambda)
		{
			settings = settingsAsLambda;
		}

		#region IPXModelStreamSerializer Members

		/// <summary>
		/// Create DstChart PNG-file
		/// </summary>
		/// <param name="model">Model to create DstChart from</param>
		/// <param name="stream">Stream for output</param>
		public void Serialize(PXModel model, Stream stream)
		{
			MemoryStream mem = new MemoryStream();
			new DstChart(model, settings).SaveImage(mem, ImageFormat.Png);
			mem.WriteTo(stream);
		}

		/// <summary>
		/// Create DstChart PNG-file
		/// </summary>
		/// <param name="model">Model to create DstChart from</param>
		/// <param name="path">Path for output</param>
		public void Serialize(PXModel model, string path)
		{
			MemoryStream mem = new MemoryStream();
			new DstChart(model, settings).SaveImage(path, ImageFormat.Png);
		}

		#endregion
	}
}
