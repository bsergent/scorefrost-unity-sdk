using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScoreFrostSDK.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace ScoreFrostSDK {
	/// <summary>
	/// Main ScoreFrost SDK class for initialization and configuration
	/// </summary>
	public sealed class ScoreFrost : MonoBehaviour {
		private static ScoreFrost _instance;
		/// <summary>
		/// Whether the SDK has been initialized
		/// </summary>
		public static bool Initialized => _instance != null && _settings != null;

		[SerializeReference] private UserAPI _user = new();
		public static UserAPI User => _instance._user;

		[SerializeReference] private ScoreAPI _score = new();
		public static ScoreAPI Score => _instance._score;

		private static Settings _settings;
		/// <summary>
		/// Current SDK settings configuration
		/// </summary>
		public static Settings Settings => _settings;

		[SerializeField] private string _gameVersion = "unknown";
		public static string GameVersion => _instance._gameVersion;

		private void Awake() {
			if (_instance != null) {
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// Initializes the ScoreFrost SDK. Call this before using any static APIs.
		/// </summary>
		public static async Task InitializeAsync(string gameVersion) {
			if (Initialized) return;

			GameObject go = new("ScoreFrost SDK");
			go.AddComponent<ScoreFrost>();

			_instance._gameVersion = gameVersion;

			// Try to load settings from Resources
			_settings = Resources.Load<Settings>("ScoreFrost Settings");
			if (_settings == null) {
				Log(LogType.Warning, "No settings found in Resources/ScoreFrost Settings.");
				// TODO Generate default settings file?
			} else if (_settings != null && _settings.ValidateSettings()) {
				Log(LogType.Log, "Initialized successfully.");
			} else {
				Log(LogType.Error, "Initialization failed - invalid settings.");
				return;
			}

			// Initialize user
			await User.LoginAsync();
		}

		[HideInCallstack]
		internal static void Log(LogType type, string message) {
			switch (type) {
				case LogType.Error when Settings.EnableErrors:
					Debug.LogError($"[ScoreFrost] {message}");
					break;
				case LogType.Warning when Settings.EnableWarnings:
					Debug.LogWarning($"[ScoreFrost] {message}");
					break;
				default:
					if (Settings.EnableLogging)
						Debug.Log($"[ScoreFrost] {message}");
					break;
			}
		}

		#region Internal HTTP Methods
		/// <summary>
		/// Sends a GET request to the specified URL. Includes authorization header.
		/// </summary>
		internal static async Task<T> Get<T>(string url) where T : ApiResponse {
			return await SendRequestWithRetry<T>(() => UnityWebRequest.Get(ProcessUrl(url)));
		}

		/// <summary>
		/// Sends a POST request to the specified URL. Includes authorization header.
		/// If a request body is provided, it is serialized as JSON.
		/// </summary>
		internal static async Task<T> Post<T>(string url, ApiRequest request = null) where T : ApiResponse {
			return await SendRequestWithRetry<T>(() => {
				var jsonBody = JsonConvert.SerializeObject(request);
				Log(LogType.Log, $"POST Body: {jsonBody}");
				byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
				var www = new UnityWebRequest(ProcessUrl(url), "POST") {
					uploadHandler = new UploadHandlerRaw(bodyRaw),
					downloadHandler = new DownloadHandlerBuffer()
				};
				www.SetRequestHeader("Content-Type", "application/json");
				return www;
			});
		}

		/// <summary>
		/// Sends a PUT request to the specified URL. Includes authorization header.
		/// If a request body is provided, it is serialized as JSON.
		/// </summary>
		internal static async Task<T> Put<T>(string url, ApiRequest request = null) where T : ApiResponse {
			return await SendRequestWithRetry<T>(() => {
				var jsonBody = JsonConvert.SerializeObject(request);
				Log(LogType.Log, $"PUT Body: {jsonBody}");
				byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
				var www = new UnityWebRequest(ProcessUrl(url), "PUT") {
					uploadHandler = new UploadHandlerRaw(bodyRaw),
					downloadHandler = new DownloadHandlerBuffer()
				};
				www.SetRequestHeader("Content-Type", "application/json");
				return www;
			});
		}

		/// <summary>
		/// Prepend the base API URL if the provided URL is relative.
		/// </summary>
		private static string ProcessUrl(string url) {
			if (!url.StartsWith("http://") && !url.StartsWith("https://"))
				url = $"{Settings.GetApiBaseUrl()}/{url}";
			return url;
		}

		/// <summary>
		/// Sends a UnityWebRequest with retry logic for transient failures.
		/// </summary>
		private static async Task<T> SendRequestWithRetry<T>(System.Func<UnityWebRequest> createRequest) where T : ApiResponse {
			int attempts = 0;
			while (attempts++ < Settings.MaxRetryAttempts) {
				using var www = createRequest();
				www.timeout = Settings.RequestTimeoutSeconds;

				Log(LogType.Log, $"Request {www.method} {www.url} (attempt {attempts}/{Settings.MaxRetryAttempts})");

				// Include api key on all requests, if available
				if (!string.IsNullOrWhiteSpace(User.ApiKey))
					www.SetRequestHeader("Authorization", $"Bearer {User.ApiKey}");

				// Wait for request to complete
				var operation = www.SendWebRequest();
				while (!operation.isDone)
					await Task.Yield();

				// Retry on connection issues
				if (www.result == UnityWebRequest.Result.ConnectionError || IsServerError(www)) {
					Log(LogType.Warning, $"Request failed (attempt {attempts}/{Settings.MaxRetryAttempts}): {www.error}");
					continue;
				}

				var jsonResponse = www.downloadHandler.text;
				Log(LogType.Log, $"Response {jsonResponse}");

				// Error with request
				if (IsRequestError(www))
					throw new HttpRequestException($"{www.responseCode} {jsonResponse}");

				// Success
				var response = JsonConvert.DeserializeObject<T>(jsonResponse);
				response.StatusCode = (int)www.responseCode;
				return response;
			}

			throw new HttpRequestException($"Max retry attempts ({Settings.MaxRetryAttempts}) reached for request.");
		}

		private static bool IsRequestError(UnityWebRequest www) {
			return www.result == UnityWebRequest.Result.ProtocolError && www.responseCode >= 400 && www.responseCode < 500;
		}

		private static bool IsServerError(UnityWebRequest www) {
			return www.result == UnityWebRequest.Result.ProtocolError && www.responseCode >= 500 && www.responseCode < 600;
		}
		#endregion
	}
}
