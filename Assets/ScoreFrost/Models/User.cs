using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ScoreFrostSDK.Models {
	[Serializable]
	public class User {
		[JsonProperty("id")] public string Id { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
	}

	[Serializable]
	public class UserFull : User {
		[JsonProperty("friend_code")] public string FriendCode { get; set; }
		[JsonProperty("date_time_created_utc")] public DateTime DateTimeCreatedUtc { get; set; }
		[JsonProperty("playtime_ms")] public long PlayTimeMs { get; set; }
	}

	[Serializable]
	public class CreateUserResponse {
		[JsonProperty("id")] public string Id { get; set; }
		[JsonProperty("friend_code")] public string FriendCode { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
		[JsonProperty("api_key")] public string ApiKey { get; set; }
	}

	[Serializable]
	public class GetUserResponse {
		[JsonProperty("id")] public string Id { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
		[JsonProperty("friend_code")] public string FriendCode { get; set; }
	}

	[Serializable]
	public class UpdateDisplayNameRequest {
		[JsonProperty("display_name")] public string DisplayName { get; set; }
	}

	[Serializable]
	public class UpdateDisplayNameResponse {
		[JsonProperty("id")] public string Id { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
		[JsonProperty("message")] public string Message { get; set; }
	}
}