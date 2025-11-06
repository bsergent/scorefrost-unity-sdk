using System;
using System.Threading.Tasks;
using ScoreFrostSDK.Models;
using UnityEngine;

namespace ScoreFrostSDK {
	/// <summary>
	/// API for user-related operations
	/// </summary>
	public sealed class UserAPI {
		/// <summary>
		/// Internal constructor - only the ScoreFrost SDK can create instances
		/// </summary>
		internal UserAPI() {
		}

		/// <summary>
		/// Gets or creates a user. If no stored credentials exist, creates a new user.
		/// If credentials exist, returns the current user information.
		/// </summary>
		/// <returns>User information</returns>
		public async Task<GetUserResponse> GetOrCreateAsync() {
			// TODO: Check for stored credentials first
			// If no credentials, create new user and store credentials
			// If credentials exist, get current user info
			Debug.LogWarning("GetOrCreateAsync not yet implemented");
			return new GetUserResponse();
		}

		/// <summary>
		/// Sets the display name for the current authenticated user
		/// </summary>
		/// <param name="newName">New display name (3-20 characters, alphanumeric plus spaces, underscores, hyphens)</param>
		/// <returns>Display name update response</returns>
		public async Task<UpdateDisplayNameResponse> SetDisplayNameAsync(string newName) {
			if (!ValidateDisplayName(newName)) {
				throw new ArgumentException("Invalid display name format", nameof(newName));
			}

			// TODO: Get current user ID from stored credentials and update display name
			Debug.LogWarning("SetDisplayNameAsync not yet implemented");
			return new UpdateDisplayNameResponse();
		}

		/// <summary>
		/// Gets user information by user ID or friend code
		/// </summary>
		/// <param name="userIdOrFriendCode">User ID (UUID) or 6-character friend code</param>
		/// <returns>User information</returns>
		public async Task<GetUserResponse> GetAsync(string userIdOrFriendCode) {
			if (string.IsNullOrEmpty(userIdOrFriendCode)) {
				throw new ArgumentException("User ID or friend code cannot be null or empty", nameof(userIdOrFriendCode));
			}

			// TODO: Determine if input is UUID or friend code and call appropriate endpoint
			// For now, assume it's a user ID
			Debug.LogWarning("GetAsync not yet implemented");
			return new GetUserResponse();
		}

		/// <summary>
		/// Gets the current authenticated user's information
		/// </summary>
		/// <returns>Current user information</returns>
		public async Task<GetUserResponse> GetCurrentAsync() {
			// TODO: Get current user ID from stored credentials and call GetAsync
			Debug.LogWarning("GetCurrentAsync not yet implemented");
			return new GetUserResponse();
		}

		/// <summary>
		/// Creates a new user with auto-generated credentials
		/// </summary>
		/// <returns>User creation response with credentials</returns>
		public async Task<CreateUserResponse> CreateAsync() {
			// TODO: Implement HTTP POST request to /user
			Debug.LogWarning("CreateAsync not yet implemented");
			return new CreateUserResponse();
		}

		/// <summary>
		/// Validates a display name format before submission
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