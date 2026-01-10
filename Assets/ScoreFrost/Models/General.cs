using System;
using Newtonsoft.Json;

namespace ScoreFrostSDK.Models {
	/// <summary>
	/// Body of an HTTP request to the ScoreFrost API backend.
	/// </summary>
	[Serializable]
	public class ApiRequest {
	}

	/// <summary>
	/// Body of an HTTP response from the ScoreFrost API backend.
	/// </summary>
	[Serializable]
	public class ApiResponse {
		public bool Success => StatusCode >= 200 && StatusCode < 300;
		[JsonProperty("status_code")] public int StatusCode { get; set; }
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