using UnityEngine;

namespace ScoreFrostSDK {
	public class Settings : ScriptableObject {
		[Header("Server Connection")]
		[SerializeField] private string _serverUrl = "https://api.scorefrost.com";
		[SerializeField, Range(0, 65535)] private int _port = 443;
		[SerializeField] private bool _useSSL = true;
		[SerializeField] private string _apiVersion = "v1";
		public string ApiVersion => _apiVersion;

		[Header("Game")]
		[SerializeField] private string _gameId = "";
		public string GameId => _gameId;
		[Tooltip("Secret salt used for solution integrity hashing")]
		[SerializeField] private string _secretSalt = "";
		public string SecretSalt => _secretSalt;

		[Header("Connection Settings")]
		// [SerializeField, Min(1)] private int _connectionTimeoutSeconds = 30;
		// public int ConnectionTimeoutSeconds => _connectionTimeoutSeconds;
		[SerializeField, Min(1)] private int _requestTimeoutSeconds = 10;
		public int RequestTimeoutSeconds => _requestTimeoutSeconds;
		[SerializeField] private int _maxRetryAttempts = 3;
		public int MaxRetryAttempts => _maxRetryAttempts;

		[Header("Debug")]
		[SerializeField] private bool _logInfo = true;
		public bool EnableLogging => _logInfo;
		[SerializeField] private bool _logWarnings = true;
		public bool EnableWarnings => _logWarnings;
		[SerializeField] private bool _logErrors = true;
		public bool EnableErrors => _logErrors;

		/// <summary>
		/// Gets the full API base URL including protocol, server, port, and API version
		/// </summary>
		public string GetApiBaseUrl() {
			string protocol = _useSSL ? "https" : "http";
			string portStr = (_useSSL && _port == 443) || (!_useSSL && _port == 80) ? "" : $":{_port}";
			return $"{protocol}://{_serverUrl.Replace("https://", "").Replace("http://", "")}{portStr}/api/{_apiVersion}";
		}

		/// <summary>
		/// Validates the current settings
		/// </summary>
		public bool ValidateSettings() {
			if (string.IsNullOrEmpty(_serverUrl)) {
				Debug.LogError("ScoreFrost Settings: Server URL is required");
				return false;
			}

			if (string.IsNullOrEmpty(_secretSalt)) {
				Debug.LogWarning("ScoreFrost Settings: Secret salt is not set");
			}

			if (string.IsNullOrEmpty(_gameId)) {
				Debug.LogWarning("ScoreFrost Settings: Game ID is not set");
			}

			return true;
		}
	}
}
