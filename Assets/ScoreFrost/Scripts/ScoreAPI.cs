using System;
using System.Collections.Generic;
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
		/// Submits a solution and scores for a level
		/// </summary>
		/// <param name="solution">Base64 encoded solution data</param>
		/// <param name="levelId">Unique identifier for the level</param>
		/// <param name="levelVersion">Version number of the level</param>
		/// <param name="gameVersion">Version of the game client</param>
		/// <param name="solutionHash">SHA256 hash of solution + secret salt for integrity verification</param>
		/// <param name="scores">Map of score type to score value</param>
		/// <returns>Score submission response</returns>
		public async Task<SolutionSubmissionResponse> SubmitSolutionAsync(
			string solution,
			string levelId,
			int levelVersion,
			string gameVersion,
			string solutionHash,
			Dictionary<string, int> scores) {
			
			if (string.IsNullOrEmpty(solution)) {
				throw new ArgumentException("Solution cannot be null or empty", nameof(solution));
			}
			if (string.IsNullOrEmpty(levelId)) {
				throw new ArgumentException("Level ID cannot be null or empty", nameof(levelId));
			}
			if (string.IsNullOrEmpty(gameVersion)) {
				throw new ArgumentException("Game version cannot be null or empty", nameof(gameVersion));
			}
			if (string.IsNullOrEmpty(solutionHash)) {
				throw new ArgumentException("Solution hash cannot be null or empty", nameof(solutionHash));
			}
			if (scores == null || scores.Count == 0) {
				throw new ArgumentException("Scores cannot be null or empty", nameof(scores));
			}

			var request = new SolutionSubmissionRequest {
				Solution = solution,
				LevelId = levelId,
				LevelVersion = levelVersion,
				GameVersion = gameVersion,
				SolutionHash = solutionHash,
				Scores = scores
			};

			// TODO: Implement HTTP POST request to /score/submit
			Debug.LogWarning("SubmitSolutionAsync not yet implemented");
			return new SolutionSubmissionResponse { Success = false, Message = "Not implemented" };
		}

		/// <summary>
		/// Gets the current user's best scores for a specific level
		/// </summary>
		/// <param name="levelId">Level identifier</param>
		/// <param name="levelVersion">Level version (optional, defaults to latest)</param>
		/// <param name="scoreType">Filter by specific score type (optional)</param>
		/// <returns>Best score entries for the level</returns>
		public async Task<BestScoreEntry[]> GetBestScoresForLevelAsync(
			string levelId,
			int? levelVersion = null,
			string scoreType = null) {
			
			if (string.IsNullOrEmpty(levelId)) {
				throw new ArgumentException("Level ID cannot be null or empty", nameof(levelId));
			}

			string levelSpec = levelVersion.HasValue ? $"{levelId}.{levelVersion}" : levelId;
			var response = await GetBestScoresAsync(levelSpec, scoreType);

			return response.Scores ?? new BestScoreEntry[0];
		}

		/// <summary>
		/// Gets the current user's best score for a specific level and score type
		/// </summary>
		/// <param name="levelId">Level identifier</param>
		/// <param name="levelVersion">Level version (optional, defaults to latest)</param>
		/// <param name="scoreType">Score type to retrieve</param>
		/// <returns>Best score entry or null if not found</returns>
		public async Task<BestScoreEntry> GetBestScoreForLevelAsync(
			string levelId,
			int? levelVersion = null,
			string scoreType = null) {
			
			var scores = await GetBestScoresForLevelAsync(levelId, levelVersion, scoreType);
			return scores.Length > 0 ? scores[0] : null;
		}

		/// <summary>
		/// Retrieves the authenticated user's best scores
		/// </summary>
		/// <param name="levels">Comma-separated list of level specifications (optional)</param>
		/// <param name="scoreType">Filter by specific score type (optional)</param>
		/// <returns>Best scores response</returns>
		public async Task<BestScoresResponse> GetBestScoresAsync(
			string levels = null,
			string scoreType = null) {
			// TODO: Implement HTTP GET request to /score/best with query parameters
			Debug.LogWarning("GetBestScoresAsync not yet implemented");
			return new BestScoresResponse { Scores = new BestScoreEntry[0], Count = 0, Scope = "user" };
		}

		/// <summary>
		/// Retrieves leaderboard with top scores per level and score type
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
				Scores = new LeaderboardEntry[0],
				Count = 0,
				Scope = "global",
				Pagination = new PaginationInfo { Offset = offset, Size = size, Total = 0 }
			};
		}

		/// <summary>
		/// Gets the leaderboard rank for a specific user on a level
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
	}
}