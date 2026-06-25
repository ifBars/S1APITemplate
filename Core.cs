using MelonLoader;
using S1API.Lifecycle;
using S1APITemplate.Integrations;
using S1APITemplate.Utils;

[assembly: MelonInfo(typeof(S1APITemplate.Core), Constants.ModName, Constants.ModVersion, Constants.ModAuthor)]
[assembly: MelonGame(Constants.Game.Studio, Constants.Game.Name)]

namespace S1APITemplate
{
    public sealed class Core : MelonMod
    {
        public static Core? Instance { get; private set; }

        private static MelonPreferences_Category? _preferencesCategory;
        private static MelonPreferences_Entry<bool>? _debugLogsEntry;

        public static bool DebugLogsEnabled => _debugLogsEntry?.Value ?? Constants.Defaults.DebugLogsEnabled;

        public override void OnInitializeMelon()
        {
            Instance = this;
            InitializePreferences();
            HarmonyPatches.Initialize(this);
            GameLifecycle.OnPreLoad += OnPreLoad;

            LoggerInstance.Msg($"{Constants.ModName} initialized.");
        }

        public override void OnApplicationQuit()
        {
            GameLifecycle.OnPreLoad -= OnPreLoad;
            Instance = null;
        }

        private void OnPreLoad()
        {
            LoggerInstance.Msg("Game is preparing to load. Register S1API items, NPCs, quests, or saveables here.");
        }

        private static void InitializePreferences()
        {
            _preferencesCategory = MelonPreferences.CreateCategory(Constants.PreferencesCategory);
            _debugLogsEntry = _preferencesCategory.CreateEntry(
                "DebugLogs",
                Constants.Defaults.DebugLogsEnabled,
                "Enable Debug Logs",
                "Show detailed debug messages in the MelonLoader console.");
            _preferencesCategory.SaveToFile(false);
        }
    }
}
