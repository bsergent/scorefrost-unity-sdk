using System;
using Newtonsoft.Json;

namespace ScoreFrostSDK.Models
{
	[Serializable]
	public class PaginationInfo
	{
		[JsonProperty("offset")] public int Offset { get; set; }
		[JsonProperty("size")] public int Size { get; set; }
		[JsonProperty("total")] public int Total { get; set; }
	}

	public enum Scope {
		Personal,
		Friends,
		Regional,
		Global
	}
}