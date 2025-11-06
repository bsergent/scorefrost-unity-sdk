using UnityEngine;

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

		private UserAPI _user = new();
		public static UserAPI User => _instance._user;

		private ScoreAPI _score = new();
		public static ScoreAPI Score => _instance._score;

		private static Settings _settings;
		/// <summary>
		/// Current SDK settings configuration
		/// </summary>
		public static Settings Settings => _settings;

		private void Awake() {
			if (_instance != null) {
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);

			// Try to load settings from Resources
			_settings = Resources.Load<Settings>("ScoreFrost Settings");
			if (_settings == null) {
				Debug.LogWarning("[ScoreFrost] No settings found in Resources/ScoreFrost Settings.");
				// TODO Generate default settings file?
			} else if (_settings != null && _settings.ValidateSettings()) {
				Debug.Log("[ScoreFrost] Initialized successfully.");
			} else {
				Debug.LogError("[ScoreFrost] Initialization failed - invalid settings.");
			}
		}

		/// <summary>
		/// Initializes the ScoreFrost SDK. Call this before using any static APIs.
		/// </summary>
		public static void Initialize() {
			if (_instance == null) {
				GameObject go = new("ScoreFrost SDK");
				go.AddComponent<ScoreFrost>();
			}
		}
	}
}
