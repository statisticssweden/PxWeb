using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PCAxis.Menu.Configuration
{
	public class DatamodelPluginConfigurationSection : System.Configuration.IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			return
				section.ChildNodes.OfType<XmlNode>()
					.Where(x => x.Name == "datamodelMenu")
					.OfType<XmlNode>()
					.ToDictionary(x => x.Attributes["providerName"].Value, x => x.Attributes["type"].Value);
		}
	}
}
