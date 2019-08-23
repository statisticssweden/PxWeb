using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Serializers.JsonStat.Model
{
	public class JsonStatDimension : Dictionary<string, List<string>>
	{
		public void Add(string key, string value)
		{
			if (!ContainsKey(key))
			{
				base.Add(key, new List<string>());
			}

			this[key].Add(value);
		}
	}
}
