using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace ScoreFrostSDK.Models {
	[Serializable]
	public class ScoreSubmissionRequest : ApiRequest {
		[JsonProperty("level_id")] public string LevelId { get; set; }
		[JsonProperty("level_version")] public int LevelVersion { get; set; }
		[JsonProperty("game_version")] public string GameVersion { get; set; }

		[JsonProperty("scores")] public Dictionary<string, int> Scores { get; set; }

		/// <summary>
		/// Base64 encoded solution data. Should contain everything the server needs to
		/// reproduce and verify the scores.
		/// </summary>
		[JsonProperty("solution")] public string Solution { get; set; }
		/// <summary>
		/// SHA256 hash of solution + secret salt for integrity verification.
		/// Dynamically computed from <see cref="Solution"/> each call.
		/// </summary>
		[JsonProperty("solution_hash")]
		public string SolutionHash {
			get {
				if (string.IsNullOrWhiteSpace(Solution))
					return string.Empty;

				// Calculate hash: SHA256(solution + salt)
				var salt = ScoreFrost.Settings.SecretSalt;
				var saltedSolution = Solution + salt;
				using var sha256 = SHA256.Create();
				var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedSolution));
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}
	}

	[Serializable]
	public class LeaderboardEntry {
		public User User { get; set; }
		[JsonProperty("rank")] public int? Rank { get; set; }
		[JsonProperty("level_id")] public string LevelId { get; set; }
		[JsonProperty("level_version")] public int LevelVersion { get; set; }
		[JsonProperty("score_type")] public string ScoreType { get; set; }
		[JsonProperty("best_score")] public int BestScore { get; set; }
	}

	[Serializable]
	public class BestScoresResponse : ApiResponse {
		[JsonProperty("scores")] public LeaderboardEntry[] Scores { get; set; }
		[JsonProperty("count")] public int Count { get; set; }
		[JsonProperty("scope")] public string Scope { get; set; }
	}

	[Serializable]
	public class LeaderboardResponse : ApiResponse {
		[JsonProperty("entries")] public LeaderboardEntry[] Entries { get; set; }
		[JsonProperty("count")] public int Count { get; set; }
		[JsonProperty("scope")] public string Scope { get; set; }
		[JsonProperty("pagination")] public PaginationInfo Pagination { get; set; }
	}
}