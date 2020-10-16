using Newtonsoft.Json;

namespace PCAxis.Serializers.JsonStat.Model
{
	public class JsonStatPx
	{
		/// <summary>
		/// InfoFile in PX
		/// </summary>
		[JsonProperty("infofile", NullValueHandling = NullValueHandling.Ignore)]
		public string infofile { get; set; }
		/// <summary>
		/// TableId in PX
		/// </summary>
		[JsonProperty("tableid", NullValueHandling = NullValueHandling.Ignore)]
		public string tableid { get; set; }
		/// <summary>
		/// Decimals in PX
		/// </summary>
		[JsonProperty("decimals", NullValueHandling = NullValueHandling.Ignore)]
		public int decimals { get; set; }
	}

}
