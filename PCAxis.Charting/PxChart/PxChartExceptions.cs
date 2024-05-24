using System;

namespace PCAxis.Charting
{
#pragma warning disable 1591
    public class PxChartExceptions
    {
        [global::System.Serializable]
        public class PxChartException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public PxChartException() { }
            public PxChartException(string message) : base(message) { }
            public PxChartException(string message, Exception inner) : base(message, inner) { }
            protected PxChartException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        [global::System.Serializable]
        public class AddingDataToPxChartException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public AddingDataToPxChartException() { }
            public AddingDataToPxChartException(string message) : base(message) { }
            public AddingDataToPxChartException(string message, Exception inner) : base(message, inner) { }
            protected AddingDataToPxChartException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        [global::System.Serializable]
        public class PopulationPyramidException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public PopulationPyramidException() { }
            public PopulationPyramidException(string message) : base(message) { }
            public PopulationPyramidException(string message, Exception inner) : base(message, inner) { }
            protected PopulationPyramidException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        [global::System.Serializable]
        public class SettingsException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public SettingsException() { }
            public SettingsException(string message) : base(message) { }
            public SettingsException(string message, Exception inner) : base(message, inner) { }
            protected SettingsException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
#pragma warning restore 1591
}
