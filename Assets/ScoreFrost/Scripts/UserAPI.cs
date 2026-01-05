using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScoreFrostSDK.Models;
using UnityEngine;

namespace ScoreFrostSDK {
	/// <summary>
	/// API for user-related operations.
	/// </summary>
	public sealed class UserAPI {
		/// <summary>
		/// Internal constructor - only the ScoreFrost SDK can create instances.
		/// </summary>
		internal UserAPI() {
		}

		#region Credentials
		private const string PrefId = "ScoreFrost.UserId";
		[SerializeField] private string _id;
		/// <summary>
		/// Unique identifier for the current user.
		/// </summary>
		public string Id {
			get => PlayerPrefs.GetString(PrefId, string.Empty);
			set => PlayerPrefs.SetString(PrefId, _id = value);
		}

		private const string PrefApiKey = "ScoreFrost.ApiKey";
		[SerializeField] private string _apiKey;
		/// <summary>
		/// API key for authenticating requests on behalf of the user.
		/// </summary>
		public string ApiKey {
			get => PlayerPrefs.GetString(PrefApiKey, string.Empty);
			set => PlayerPrefs.SetString(PrefApiKey, _apiKey = value);
		}

		/// <summary>
		/// Whether the user has been provisioned with an ID and API key.
		/// </summary>
		public bool Provisioned => !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ApiKey);
		#endregion

		private const string PrefCachedSelf = "ScoreFrost.UserFull";
		[SerializeField] private UserFull _cachedSelf;
		/// <summary>
		/// Cached user information for the current user. Null if not cached.
		/// </summary>
		public UserFull CachedSelf {
			get => JsonConvert.DeserializeObject<UserFull>(PlayerPrefs.GetString(PrefCachedSelf, "null"));
			set => PlayerPrefs.SetString(PrefCachedSelf, JsonConvert.SerializeObject(_cachedSelf = value));
		}

		/// <summary>
		/// If provisioned, authenticates the user by fetching their details from the server.
		/// If not provisioned, creates a new user and stores the credentials.
		/// Stores the returned user details in <see cref="CachedSelf"/>.
		/// </summary>
		public async Task LoginAsync() {
			// Show initial values in inspector
			_id = Id;
			_apiKey = ApiKey;

			try {
				var response = await ScoreFrost.Post<UserFull>("user", new LoginRequest {
					GameId = ScoreFrost.Settings.GameId,
					GameVersion = Application.version
				});
				if (response.StatusCode == 201) {
					// New user created
					Id = response.Id;
					ApiKey = response.ApiKey;
					CachedSelf = response;
					ScoreFrost.Log(LogType.Log, $"New user created: {response.DisplayName} ({response.FriendCode})");

				} else if (response.StatusCode == 200) {
					// Existing user authenticated
					ScoreFrost.Log(LogType.Log, $"Existing user authenticated: {response.DisplayName} ({response.FriendCode})");
					CachedSelf = response;

				} else {
					ScoreFrost.Log(LogType.Error, $"User login failed: {response.StatusCode} {response.Message}");

				}
			} catch (Exception ex) {
				ScoreFrost.Log(LogType.Error, $"User login failed: {ex.Message}");
			}
		}

		/// <summary>
		/// Sets the display name for the current authenticated user.
		/// </summary>
		/// <param name="newName">New display name (3-20 characters, alphanumeric plus spaces, underscores, hyphens)</param>
		/// <returns>Display name update response</returns>
		public async Task<User> SetDisplayNameAsync(string newName) {
			if (!ValidateDisplayName(newName)) {
				throw new ArgumentException("Invalid display name format", nameof(newName));
			}

			// TODO: Get current user ID from stored credentials and update display name
			Debug.LogWarning("SetDisplayNameAsync not yet implemented");
			return new User();
		}

		/// <summary>
		/// Gets user information by user ID or friend code.
		/// </summary>
		/// <param name="userIdOrFriendCode">User ID (UUID) or 6-character friend code</param>
		/// <returns>User information</returns>
		public async Task<UserFull> GetAsync(string userIdOrFriendCode) {
			if (string.IsNullOrEmpty(userIdOrFriendCode)) {
				throw new ArgumentException("User ID or friend code cannot be null or empty", nameof(userIdOrFriendCode));
			}

			// TODO: Determine if input is UUID or friend code and call appropriate endpoint
			// For now, assume it's a user ID
			Debug.LogWarning("GetAsync not yet implemented");
			return new UserFull();
		}

		/// <summary>
		/// Validates a display name format before submission.
		/// </summary>
		/// <param name="displayName">Display name to validate</param>
		/// <returns>True if valid, false otherwise</returns>
		public bool ValidateDisplayName(string displayName) {
			if (string.IsNullOrEmpty(displayName))
				return false;

			if (displayName.Length < 3 || displayName.Length > 20)
				return false;

			// Check pattern: alphanumeric plus spaces, underscores, hyphens
			foreach (char c in displayName) {
				if (!char.IsLetterOrDigit(c) && c != ' ' && c != '_' && c != '-')
					return false;
			}

			return true;
		}
	}
}