using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ScoreFrostSDK.Models {
	[Serializable]
	public class User : ApiResponse {
		[JsonProperty("id")] public string Id;
		[JsonProperty("display_name")] public string DisplayName;
	}

	[Serializable]
	public class UserFull : User {
		[JsonProperty("friend_code")] public string FriendCode;
		[JsonProperty("date_time_created_utc")] public DateTime DateTimeCreatedUtc;
		[JsonProperty("playtime_ms")] public long PlayTimeMs;
	}

	[Serializable]
	public class UserNew : UserFull {
		[JsonProperty("api_key")] public string ApiKey;
	}

	[Serializable]
	public class UpdateDisplayNameRequest : ApiRequest {
		[JsonProperty("display_name")] public string DisplayName;
	}
}