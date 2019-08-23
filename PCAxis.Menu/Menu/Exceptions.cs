using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Menu.Exceptions
{
#pragma warning disable 1591
	/// <summary>
	/// Exception thrown when the parameter char is not set for a DatamodelMenu
	/// </summary>
	[global::System.Serializable]
	public class NoParameterCharException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public NoParameterCharException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public NoParameterCharException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public NoParameterCharException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected NoParameterCharException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// Exception thrown when the requested menu doesn't exist in XML
	/// </summary>
	[global::System.Serializable]
	public class InvalidMenuFromXMLException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public InvalidMenuFromXMLException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public InvalidMenuFromXMLException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public InvalidMenuFromXMLException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected InvalidMenuFromXMLException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// Exception thrown when an item has already been loaded from a database
	/// </summary>
	[global::System.Serializable]
	public class ItemHasBeenLoadedException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public ItemHasBeenLoadedException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ItemHasBeenLoadedException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ItemHasBeenLoadedException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ItemHasBeenLoadedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// Exception thrown when actions are requested on a menu that should not be done after initialization
	/// </summary>
	[global::System.Serializable]
	public class MenuHasBeenInitializedException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public MenuHasBeenInitializedException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MenuHasBeenInitializedException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MenuHasBeenInitializedException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MenuHasBeenInitializedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// Exception thrown when items do not fit structure of items in DatamodelMenu.
	/// </summary>
	[global::System.Serializable]
	public class NotValidItemFromDatabaseException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// </summary>
		public NotValidItemFromDatabaseException() { }
		/// <summary>
		/// </summary>
		public NotValidItemFromDatabaseException(string message) : base(message) { }
		/// <summary>
		/// </summary>
		public NotValidItemFromDatabaseException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// </summary>
		protected NotValidItemFromDatabaseException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// Exception thrown when the supplied value is not considered safe for usage in SQL.
	/// </summary>
	[global::System.Serializable]
	public class ValueNotSafeForSQLException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// </summary>
		public ValueNotSafeForSQLException() { }
		/// <summary>
		/// </summary>
		public ValueNotSafeForSQLException(string message) : base(message) { }
		/// <summary>
		/// </summary>
		public ValueNotSafeForSQLException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// </summary>
		protected ValueNotSafeForSQLException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	[global::System.Serializable]
	public class SqlHintException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public SqlHintException() { }
		public SqlHintException(string message) : base(message) { }
		public SqlHintException(string message, Exception inner) : base(message, inner) { }
		protected SqlHintException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	[Serializable]
	public class DatamodelMenuExtractedDataErrorException : Exception
	{
		public DatamodelMenuExtractedDataErrorException() { }
		public DatamodelMenuExtractedDataErrorException(string message) : base(message) { }
		public DatamodelMenuExtractedDataErrorException(string message, Exception inner) : base(message, inner) { }
		protected DatamodelMenuExtractedDataErrorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	[Serializable]
	public class ParameterNotValidException : Exception
	{
		public ParameterNotValidException() { }
		public ParameterNotValidException(string message) : base(message) { }
		public ParameterNotValidException(string message, Exception inner) : base(message, inner) { }
		protected ParameterNotValidException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

#pragma warning restore 1591
}
