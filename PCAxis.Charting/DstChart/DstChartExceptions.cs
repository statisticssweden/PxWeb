using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Charting
{
#pragma warning disable 1591
	public class DstChartExceptions
	{
		[global::System.Serializable]
		public class DstChartException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public DstChartException() { }
			public DstChartException(string message) : base(message) { }
			public DstChartException(string message, Exception inner) : base(message, inner) { }
			protected DstChartException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}
	}
#pragma warning restore 1591
}
