using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PCAxis.Menu.Extensions
{
	/// <summary>
	/// Extensions for the PxMenu functionality
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Converts string to Enum
		/// </summary>
		/// <typeparam name="T">Enum type</typeparam>
		/// <param name="s">String to convert</param>
		/// <returns></returns>
		public static T ToEnum<T>(this string s)
		{
			return (T)Enum.Parse(typeof(T), s, true);
		}

		/// <summary>
		/// Cast an object "inline" (used when it makes code more readable)
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="o">Object to cast</param>
		/// <returns></returns>
		public static T Cast<T>(this object o)
		{
			return (T)o;
		}

		/// <summary>
		/// Determines "inline" whether an object is not of a specific type (used when it makes code more readable)
		/// </summary>
		/// <typeparam name="T">Type to check for</typeparam>
		/// <param name="o">Object to check</param>
		/// <returns></returns>
		public static bool IsNotOfType<T>(this object o)
		{
			return !(o is T);
		}

		/// <summary>
		/// Determines "inline" whether an object is of a specific type (used when it makes code more readable)
		/// </summary>
		/// <typeparam name="T">Type to check for</typeparam>
		/// <param name="o">Object to check</param>
		/// <returns></returns>
		public static bool IsOfType<T>(this object o)
		{
			return (o is T);
		}

		/// <summary>
		/// Converts an XDocument to an XmlDocument for those who want's the "old" format
		/// </summary>
		/// <param name="x">XDocument to convert</param>
		/// <returns></returns>
		public static XmlDocument ToXmlDocument(this XDocument x)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(x.ToString());
			return xmlDocument;
		}
	}
}
