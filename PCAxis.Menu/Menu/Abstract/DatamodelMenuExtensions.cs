using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Menu;

namespace PCAxis.Menu.Implementations.DatamodelMenuExtensions
{
    /// <summary>
    /// Extensionmethods for DatamodelMenu
    /// </summary>
	public static class DatamodelMenuExtensions
	{
        //TODO
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
		public static bool HasBeenLoaded(this PxMenuItem i)
		{
			return i.HasAttribute("HasBeenLoaded") && i.GetAttribute<bool>("HasBeenLoaded");
		}

        //TODO
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="value"></param>
		public static void HasBeenLoaded(this PxMenuItem i, bool value)
		{
			i.SetAttribute("HasBeenLoaded", value);
		}
	}
}
