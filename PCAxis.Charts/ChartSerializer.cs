using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;

namespace PCAxis.Chart
{
    public class ChartSerializer : PCAxis.Paxiom.IPXModelStreamSerializer
    {
        public ChartSettings Settings { get; set; }
        public ImageFormat Format { get; set; }

        public ChartSerializer()
        {
            Settings = new ChartSettings();
            Format = ImageFormat.Png;
        }

        public void Serialize(PCAxis.Paxiom.PXModel model, System.IO.Stream stream)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            PxWebChart chart;
            chart = ChartHelper.GetChart(Settings, model);
            chart.SaveImage(s, Format);
            s.WriteTo(stream);
        }

        public void Serialize(PCAxis.Paxiom.PXModel model, string path)
        {
            if (model == null) throw new ArgumentNullException("model");

            // Let the StreamWriter verify the path argument
            using (System.IO.FileStream writer = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                Serialize(model, writer);
            }

        }
    }
}
