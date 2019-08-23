using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// Exceptions for ConfigDatamodelMenu.
	/// </summary>
	public class ConfigDatamodelMenuExceptions
	{
		/// <summary>
		/// Exception thrown when type supplied for overriding extraction of data is not valid
		/// </summary>
		[Serializable]
		public class TypeForExtractingDataNotValidException : Exception
		{
			public TypeForExtractingDataNotValidException() { }
			public TypeForExtractingDataNotValidException(string message) : base(message) { }
			public TypeForExtractingDataNotValidException(string message, Exception inner) : base(message, inner) { }
			protected TypeForExtractingDataNotValidException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		/// <summary>
		/// Exception thrown when no SqlDbConfig instance supplied
		/// </summary>
		[global::System.Serializable]
		public class NoDbConfigurationException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			/// <summary>
			/// </summary>
			public NoDbConfigurationException() { }
			/// <summary>
			/// </summary>
			public NoDbConfigurationException(string message) : base(message) { }
			/// <summary>
			/// </summary>
			public NoDbConfigurationException(string message, Exception inner) : base(message, inner) { }
			/// <summary>
			/// </summary>
			protected NoDbConfigurationException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		/// <summary>
		/// Exception thrown when setting the PxMenu language code to at code, that is not corresponding to a language in the SqlDbConfig-XML.
		/// </summary>
		[global::System.Serializable]
		public class LanguageNotInConfigXmlException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			/// <summary>
			/// </summary>
			public LanguageNotInConfigXmlException() { }
			/// <summary>
			/// </summary>
			public LanguageNotInConfigXmlException(string message) : base(message) { }
			/// <summary>
			/// </summary>
			public LanguageNotInConfigXmlException(string message, Exception inner) : base(message, inner) { }
			/// <summary>
			/// </summary>
			protected LanguageNotInConfigXmlException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		/// <summary>
		/// Exception thrown when unable to parse pattern from SQL according to an SqlDbConfig instance
		/// </summary>
		[global::System.Serializable]
		public class UnrecognizedPatternInSpecifiedSQLException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			/// <summary>
			/// </summary>
			public UnrecognizedPatternInSpecifiedSQLException() { }
			/// <summary>
			/// </summary>
			public UnrecognizedPatternInSpecifiedSQLException(string message) : base(message) { }
			/// <summary>
			/// </summary>
			public UnrecognizedPatternInSpecifiedSQLException(string message, Exception inner) : base(message, inner) { }
			/// <summary>
			/// </summary>
			protected UnrecognizedPatternInSpecifiedSQLException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}
	}
}
