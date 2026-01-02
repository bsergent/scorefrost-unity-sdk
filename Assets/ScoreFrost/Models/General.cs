using System;
using Newtonsoft.Json;

namespace ScoreFrostSDK.Models {
	public class ApiRequest {
		[JsonProperty("api_key")] public string ApiKey { get; set; }
	}

	public class ApiResponse {
		[JsonProperty("message")] public string Message { get; set; }
	}

	[Serializable]
	public class PaginationInfo {
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