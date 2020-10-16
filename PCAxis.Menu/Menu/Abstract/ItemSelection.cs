using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Menu.Exceptions;

namespace PCAxis.Menu
{
	//TODO
	/// <summary>
	/// 
	/// </summary>
	public class ItemSelection
	{
		/// <summary>
		/// 
		/// </summary>
		public string Menu;

		/// <summary>
		/// 
		/// </summary>
		public string Selection;

		/// <summary>
		/// 
		/// </summary>
		public ItemSelection()
		{
			Menu = "START";
			Selection = "START";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="menu"></param>
		/// <param name="selection"></param>
		public ItemSelection(string menu, string selection)
		{
			Set(menu, selection);
		}

		/// <summary>
		/// Returns whether the values of the item matches the given values
		/// </summary>
		/// <param name="menu"></param>
		/// <param name="selection"></param>
		public bool Match(string menu, string selection)
		{
           // return Menu == menu && Selection == selection;
            return string.Compare(Menu, menu, true) == 0 && string.Compare(Selection, selection, true) == 0;

        }

        /// <summary>
        /// Set values for this ItemSelection
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="selection"></param>
        public void Set(string menu, string selection)
		{
			this.Menu = menu;
			this.Selection = selection;
		}

		/// <summary>
		/// Set values for this ItemSelection
		/// </summary>
		/// <param name="values"></param>
		public void Set(string[] values)
		{
			if (values.Length != 2)
				throw new ParameterNotValidException("The parameter values must contains two strings, menu and selection.");

			this.Menu = values[0];
			this.Selection = values[1];
		}

		/// <summary>
		/// 
		/// </summary>
		public string[] Values
		{
			get { return new string[] { Menu, Selection }; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ItemSelection o = obj as ItemSelection;

			if (o == null)
				return false;

			return Menu == o.Menu && Selection == o.Selection;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (Menu + "," + Selection).GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("[{0}, {1}]", Menu, Selection);
		}
	}
}
