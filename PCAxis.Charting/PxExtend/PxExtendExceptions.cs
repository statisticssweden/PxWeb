using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PCAxis.PxExtend
{
#pragma warning disable 1591
	public class PxExtendExceptions
	{
		[global::System.Serializable]
		public class PxExtendException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public PxExtendException() { }
			public PxExtendException(string message) : base(message) { }
			public PxExtendException(string message, Exception inner) : base(message, inner) { }
			protected PxExtendException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		[global::System.Serializable]
		public class EliminatinNotAllowedException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public EliminatinNotAllowedException() { }
			public EliminatinNotAllowedException(string message) : base(message) { }
			public EliminatinNotAllowedException(string message, Exception inner) : base(message, inner) { }
			protected EliminatinNotAllowedException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		[global::System.Serializable]
		public class SortDatasetFromPxException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public SortDatasetFromPxException() { }
			public SortDatasetFromPxException(string message) : base(message) { }
			public SortDatasetFromPxException(string message, Exception inner) : base(message, inner) { }
			protected SortDatasetFromPxException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}
		
		[global::System.Serializable]
		public class MissingMaleValueForPopulationPyramidException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public MissingMaleValueForPopulationPyramidException() { }
			public MissingMaleValueForPopulationPyramidException(string message) : base(message) { }
			public MissingMaleValueForPopulationPyramidException(string message, Exception inner) : base(message, inner) { }
			protected MissingMaleValueForPopulationPyramidException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		[Serializable]
		public class ModelNotReadyException : Exception
		{
			public ModelNotReadyException() { }
			public ModelNotReadyException(string message) : base(message) { }
			public ModelNotReadyException(string message, Exception inner) : base(message, inner) { }
			protected ModelNotReadyException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		[Serializable]
		public class ConvertToNegativeValuesException : Exception
		{
			public ConvertToNegativeValuesException()
			{
			}

			public ConvertToNegativeValuesException(string message) : base(message)
			{
			}

			public ConvertToNegativeValuesException(string message, Exception inner) : base(message, inner)
			{
			}

			protected ConvertToNegativeValuesException(
				SerializationInfo info,
				StreamingContext context) : base(info, context)
			{
			}
		}
	}
#pragma warning restore 1591
}
