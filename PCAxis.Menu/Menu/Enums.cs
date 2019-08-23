using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Menu
{
	/// <summary>
	/// Type of link
	/// </summary>
	public enum LinkType
	{
		/// <summary>
		/// Link to a table
		/// </summary>
		Table,
		/// <summary>
		/// Link to a PX file
		/// </summary>
		PX,
		/// <summary>
		/// Link to a XDF file
		/// </summary>
		XDF,
		/// <summary>
		/// Link to a PXS file
		/// </summary>
		PXS,
		/// <summary>
		/// Link to an internet URL
		/// </summary>
		URL
	}

	/// <summary>
	/// Link presentaton code
	/// </summary>
	public enum LinkPres
	{
		/// <summary>
		/// 
		/// </summary>
		NotSet = ' ',
		/// <summary>
		/// 
		/// </summary>
		Dictately = 'D',
		/// <summary>
		/// 
		/// </summary>
		Icon = 'I',
		/// <summary>
		/// 
		/// </summary>
		Both = 'B'
	}

	/// <summary>
	/// Category of an item
	/// </summary>
	public enum PresCategory
	{
		/// <summary>
		/// 
		/// </summary>
		NotSet = ' ',
		/// <summary>
		/// 
		/// </summary>
		Official = 'O',
		/// <summary>
		/// 
		/// </summary>
		Internal = 'I',
		/// <summary>
		/// 
		/// </summary>
		Private = 'P'
	}

	/// <summary>
	/// Status of the a table
	/// </summary>
	public enum TableStatus
	{ 
		/// <summary>
		/// 
		/// </summary>
		NotSet = ' ',
		/// <summary>
		/// 
		/// </summary>
		OnlyMetaData = 'M',
		/// <summary>
		/// 
		/// </summary>
		EmptyDataTable = 'E',
		/// <summary>
		/// 
		/// </summary>
		IsBeingUpdated = 'U',
		/// <summary>
		/// 
		/// </summary>
		NewNotOfficial = 'N',
		/// <summary>
		/// 
		/// </summary>
		ReadyForOfficial = 'O',
		/// <summary>
		/// 
		/// </summary>
		AccessibleToAll = 'A',
	}
}