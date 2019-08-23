using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Menu.Implementations
{
	/// <summary>
	/// Information about parameter for database
	/// </summary>
	public class DatabaseParameterCollection
	{
		private List<DatabaseParameter> collection = new List<DatabaseParameter>();

		/// <summary>
		/// Creates a collection of parameters descriptions
		/// </summary>
		/// <param name="parameters"></param>
		public DatabaseParameterCollection(params DatabaseParameter[] parameters)
		{
			collection.AddRange(parameters);
		}

		/// <summary>
		/// Gets a parameter from the collection
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <returns></returns>
		public DatabaseParameter this[string name]
		{
			get
			{
				return collection.First(x => x.Name.Equals(name));
			}
		}

		/// <summary>
		/// Gets a parameter from the collection
		/// </summary>
		/// <param name="i">The index of the parameter</param>
		/// <returns></returns>
		public DatabaseParameter this[int i]
		{
			get
			{
				return collection[i];
			}
		}

		/// <summary>
		/// Gets the number of parameters in the collection
		/// </summary>
		public int Count
		{
			get
			{
				return collection.Count;
			}
		}
	}

	/// <summary>
	/// Information about parameter for database
	/// </summary>
	public class DatabaseParameter
	{
		/// <summary>
		/// Gets or sets the name of the parameter
		/// </summary>
		public string Name;

		/// <summary>
		/// Gets or sets the size of the parameter
		/// </summary>
		public int Size;

		/// <summary>
		/// Gets or sets the type of the parameter
		/// </summary>
		public Type Type;

		/// <summary>
		/// </summary>
		public DatabaseParameter(string name, int size, Type type)
		{
			this.Name = name;
			this.Size = size;
			this.Type = type;
		}
	}
}
