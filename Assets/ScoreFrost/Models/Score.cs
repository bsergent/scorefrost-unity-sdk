using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScoreFrostSDK.Models {
	[Serializable]
	public class SolutionSubmissionRequest {
		[JsonProperty("level_id")] public string LevelId { get; set; }
		[JsonProperty("level_version")] public int LevelVersion { get; set; }
		[JsonProperty("game_version")] public string GameVersion { get; set; }
		[JsonProperty("solution")] public string Solution { get; set; }
		[JsonProperty("solution_hash")] public string SolutionHash { get; set; }
		[JsonProperty("scores")] public Dictionary<string, int> Scores { get; set; }
	}

	[Serializable]
	public class SolutionSubmissionResponse {
		[JsonProperty("success")] public bool Success { get; set; }
		[JsonProperty("solution_id")] public int SolutionId { get; set; }
		[JsonProperty("message")] public string Message { get; set; }
	}

	[Serializable]
	public class BestScoreEntry {
		[JsonProperty("level_id")] public string LevelId { get; set; }
		[JsonProperty("level_version")] public int LevelVersion { get; set; }
		[JsonProperty("score_type")] public string ScoreType { get; set; }
		[JsonProperty("best_score")] public int BestScore { get; set; }
		[JsonProperty("user_id")] public string UserId { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
		[JsonProperty("friend_code")] public string FriendCode { get; set; }
	}

	[Serializable]
	public class BestScoresResponse {
		[JsonProperty("scores")] public BestScoreEntry[] Scores { get; set; }
		[JsonProperty("count")] public int Count { get; set; }
		[JsonProperty("scope")] public string Scope { get; set; }
	}

	[Serializable]
	public class LeaderboardEntry {
		[JsonProperty("rank")] public int Rank { get; set; }
		[JsonProperty("level_id")] public string LevelId { get; set; }
		[JsonProperty("level_version")] public int LevelVersion { get; set; }
		[JsonProperty("score_type")] public string ScoreType { get; set; }
		[JsonProperty("best_score")] public int BestScore { get; set; }
		[JsonProperty("user_id")] public string UserId { get; set; }
		[JsonProperty("display_name")] public string DisplayName { get; set; }
		[JsonProperty("friend_code")] public string FriendCode { get; set; }
	}

	[Serializable]
	public class LeaderboardResponse {
		[JsonProperty("scores")] public LeaderboardEntry[] Scores { get; set; }
		[JsonProperty("count")] public int Count { get; set; }
		[JsonProperty("scope")] public string Scope { get; set; }
		[JsonProperty("pagination")] public PaginationInfo Pagination { get; set; }
	}
}