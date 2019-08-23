using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace PCAxis.Menu
{
	/// <summary>
	/// Item for containing a menu headline.
	/// </summary>
	public class Headline : Item
	{
		/// <summary>
		/// Create instance with data
		/// </summary>
		/// <param name="text">Presentation text</param>
		/// <param name="textShort">Short presentation text</param>
		/// <param name="sortCode">Sort code</param>
		/// <param name="menu">Menu code</param>
		/// <param name="selection">Selection code</param>
		/// <param name="description">Description of the headline</param>
		public Headline(string text, string textShort, string sortCode, string menu, string selection, string description)
			: base(text, textShort, sortCode, menu, selection, description)
		{
		}
	}
}