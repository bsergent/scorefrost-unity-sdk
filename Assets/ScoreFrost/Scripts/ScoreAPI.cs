using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreFrostSDK.Models;
using UnityEngine;

namespace ScoreFrostSDK {
	/// <summary>
	/// API for score-related operations
	/// </summary>
	public sealed class ScoreAPI {
		/// <summary>
		/// Internal constructor - only the ScoreFrost SDK can create instances
		/// </summary>
		internal ScoreAPI() {
		}

		/// <summary>
		/// Submits a solution and scores for a level.
		/// </summary>
		/// <param name="solution">Base64 encoded solution containing everything the server needs to reproduce and verify scores</param>
		/// <param name="levelId">Unique identifier for the level</param>
		/// <param name="levelVersion">Version number of the level</param>
		/// <param name="gameVersion">Version of the game client</param>
		/// <param name="scores">Map of score type to score value</param>
		/// <returns>API response</returns>
		public async Task<ApiResponse> SubmitSolutionAsync(
			string levelId,
			int levelVersion,
			string solution,
			Dictionary<Enum, int> scores) {

			// Ensure solution string is valid base64
			try {
				Convert.FromBase64String(solution);
			} catch (FormatException) {
				throw new ArgumentException("Invalid base64 solution", nameof(solution));
			}

			try {
				// Convert enum keys to snake_case string keys
				var stringScores = scores?.ToDictionary(
					kvp => PascalToSnakeCase(kvp.Key.ToString()),
					kvp => kvp.Value);

				var request = new ScoreSubmissionRequest {
					LevelId = levelId,
					LevelVersion = levelVersion,
					GameVersion = ScoreFrost.GameVersion,
					Scores = stringScores,
					Solution = solution,
				};
				var response = await ScoreFrost.Put<ApiResponse>(
					"score",
					request);

				if (response.Success) {
					var hash = request.SolutionHash;
					if (hash.Length > 8)
						hash = hash[0..8];
					ScoreFrost.Log(LogType.Log, $"Submitted solution: {hash}");
					return response;

				} else {
					ScoreFrost.Log(LogType.Warning, $"Failed to submit solution: {response.StatusCode} {response.Message}");
					return response;

				}
			} catch (Exception ex) {
				ScoreFrost.Log(LogType.Error, $"Failed to submit solution: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Gets the current user's best scores for a specific level.
		/// </summary>
		/// <param name="levelId">Level identifier</param>
		/// <param name="levelVersion">Level version (optional, defaults to latest)</param>
		/// <param name="scoreType">Filter by specific score type (optional)</param>
		/// <returns>Best score entries for the level</returns>
		public async Task<LeaderboardEntry[]> GetBestScoresForLevelAsync(
			string levelId,
			int? levelVersion = null,
			string scoreType = null) {

			if (string.IsNullOrEmpty(levelId)) {
				throw new ArgumentException("Level ID cannot be null or empty", nameof(levelId));
			}

			string levelSpec = levelVersion.HasValue ? $"{levelId}.{levelVersion}" : levelId;
			var response = await GetBestScoresAsync(levelSpec, scoreType);

			return response.Scores ?? new LeaderboardEntry[0];
		}

		/// <summary>
		/// Gets the current user's best score for a specific level and score type.
		/// </summary>
		/// <param name="levelId">Level identifier</param>
		/// <param name="levelVersion">Level version (optional, defaults to latest)</param>
		/// <param name="scoreType">Score type to retrieve</param>
		/// <returns>Best score entry or null if not found</returns>
		public async Task<LeaderboardEntry> GetBestScoreForLevelAsync(
			string levelId,
			int? levelVersion = null,
			string scoreType = null) {

			var scores = await GetBestScoresForLevelAsync(levelId, levelVersion, scoreType);
			return scores.Length > 0 ? scores[0] : null;
		}

		/// <summary>
		/// Retrieves the authenticated user's best scores.
		/// </summary>
		/// <param name="levels">Comma-separated list of level specifications (optional)</param>
		/// <param name="scoreType">Filter by specific score type (optional)</param>
		/// <returns>Best scores response</returns>
		public async Task<BestScoresResponse> GetBestScoresAsync(
			string levels = null,
			string scoreType = null) {
			// TODO: Implement HTTP GET request to /score/best with query parameters
			Debug.LogWarning("GetBestScoresAsync not yet implemented");
			return new BestScoresResponse { Scores = new LeaderboardEntry[0], Count = 0, Scope = "user" };
		}

		/// <summary>
		/// Retrieves leaderboard with top scores per level and score type.
		/// </summary>
		/// <param name="levels">Comma-separated list of level specifications (optional)</param>
		/// <param name="scoreType">Filter by specific score type (optional)</param>
		/// <param name="offset">Number of records to skip for pagination (default: 0)</param>
		/// <param name="size">Number of records to return, max 100 (default: 10)</param>
		/// <returns>Leaderboard response</returns>
		public async Task<LeaderboardResponse> GetLeaderboardAsync(
			string levels = null,
			string scoreType = null,
			int offset = 0,
			int size = 10) {
			// TODO: Implement HTTP GET request to /score/leaderboard with query parameters
			Debug.LogWarning("GetLeaderboardAsync not yet implemented");
			return new LeaderboardResponse {
				Entries = new LeaderboardEntry[0],
				Count = 0,
				Scope = "global",
				Pagination = new PaginationInfo { Offset = offset, Size = size, Total = 0 }
			};
		}

		/// <summary>
		/// Gets the leaderboard rank for a specific user on a level.
		/// </summary>
		/// <param name="levelId">Level identifier</param>
		/// <param name="levelVersion">Level version (optional, defaults to latest)</param>
		/// <param name="scoreType">Score type to check</param>
		/// <param name="userId">User ID to find rank for (optional, uses authenticated user)</param>
		/// <returns>Rank position or -1 if not found</returns>
		public async Task<int> GetUserRankAsync(
			string levelId,
			int? levelVersion = null,
			string scoreType = null,
			string userId = null) {
			// TODO: Implement by fetching leaderboard and finding user position
			Debug.LogWarning("GetUserRankAsync not yet implemented");
			return -1;
		}

		/// <summary>
		/// Converts an enum name from PascalCase to snake_case.
		/// </summary>
		private string PascalToSnakeCase(string enumName) {
			if (string.IsNullOrEmpty(enumName))
				return enumName;

			string result = "";
			for (int i = 0; i < enumName.Length; i++) {
				char c = enumName[i];
				if (i > 0 && char.IsUpper(c))
					result += "_";
				result += char.ToLower(c);
			}
			return result;
		}
	}
}