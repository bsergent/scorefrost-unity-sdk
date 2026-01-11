using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ScoreFrostSDK;
using System;
using System.Text;

/// <summary>
/// Example demonstrating how to use the ScoreFrost static API
/// </summary>
public class ScoreFrostExample : MonoBehaviour {

	public enum ScoreType {
		TimeMs,
		Striping,
		Fuel,
		Stars,
	}

	private async void Start() {
		// Initialize the SDK
		await ScoreFrost.InitializeAsync(Application.version);

		// Wait a frame to ensure initialization
		await Task.Yield();

		// Example usage of the new static APIs
		await ExampleUsage();
	}

	private async Task ExampleUsage() {
		try {
			// User operations
			var user = ScoreFrost.User.CachedSelf;

			// Set display name
			// await ScoreFrost.User.SetDisplayNameAsync("Bebbles");

			var otherUser = await ScoreFrost.User.GetAsync("0000-0000");
			var otherUser2 = await ScoreFrost.User.GetAsync("0000-0001");

			var submitResponse = await ScoreFrost.Score.SubmitSolutionAsync(
				levelId: "level_001",
				levelVersion: 1,
				solution: Convert.ToBase64String(Encoding.UTF8.GetBytes("reproduction steps")),
				scores: new Dictionary<Enum, int> {
					{ ScoreType.TimeMs, 15420 },
					{ ScoreType.Stars, 3 }
				}
			);

			return;

			// Get best scores for a level
			var bestScores = await ScoreFrost.Score.GetBestScoresForLevelAsync("level_001");

			// Get leaderboard
			var leaderboard = await ScoreFrost.Score.GetLeaderboardAsync("level_001");

		} catch (System.Exception e) {
			Debug.LogError($"[ScoreFrost] Error: {e.Message}");
		}
	}
}