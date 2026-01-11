using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace ScoreFrostSDK.Models {
	[Serializable]
	public class User : ApiResponse {
		[JsonProperty("id")] public string Id;
		[JsonProperty("display_name")] public string DisplayName;
		[JsonProperty("friend_code")] public string FriendCode;
	}

	[Serializable]
	public class UserFull : User {
		[JsonProperty("date_time_created_utc"), SerializeField] private string dateTimeCreatedUtc;
		public DateTime DateTimeCreatedUtc {
			get => DateTime.TryParse(dateTimeCreatedUtc, out var dt) ? dt : DateTime.MinValue;
			set => dateTimeCreatedUtc = value.ToString("o");
		}

		[JsonProperty("date_time_active_utc"), SerializeField] private string dateTimeActiveUtc;
		public DateTime DateTimeActiveUtc {
			get => DateTime.TryParse(dateTimeActiveUtc, out var dt) ? dt : DateTime.MinValue;
			set => dateTimeActiveUtc = value.ToString("o");
		}

		[JsonProperty("game_version")] public string GameVersion;
		[JsonProperty("playtime_ms")] public long PlayTimeMs;
		/// <summary>
		/// API key for the user. Only returned upon creation.
		/// </summary>
		[JsonProperty("api_key")] public string ApiKey;
	}

	[Serializable]
	public class LoginRequest : ApiRequest {
		[JsonProperty("game_id")] public string GameId;
		[JsonProperty("game_version")] public string GameVersion;
	}

	[Serializable]
	public class UpdateDisplayNameRequest : ApiRequest {
		[JsonProperty("display_name")] public string DisplayName;
	}
}