using MelonLoader;
using S1API.Lifecycle;
using EarlySpecialCustomers.Utils;

[assembly: MelonInfo(typeof(EarlySpecialCustomers.Core), Constants.ModName, Constants.ModVersion, Constants.ModAuthor)]
[assembly: MelonGame(Constants.Game.Studio, Constants.Game.Name)]

namespace EarlySpecialCustomers;

public sealed class Core : MelonMod
{
    private static MelonPreferences_Category? _preferencesCategory;
    private static MelonPreferences_Entry<bool>? _introMessagesEntry;
    private static MelonPreferences_Entry<bool>? _milestoneMessagesEntry;
    private static MelonPreferences_Entry<bool>? _debugLogsEntry;

    public static bool IntroMessagesEnabled => _introMessagesEntry?.Value ?? Constants.Defaults.IntroMessagesEnabled;
    public static bool MilestoneMessagesEnabled => _milestoneMessagesEntry?.Value ?? Constants.Defaults.MilestoneMessagesEnabled;
    public static bool DebugLogsEnabled => _debugLogsEntry?.Value ?? Constants.Defaults.DebugLogsEnabled;

    public override void OnInitializeMelon()
    {
        InitializePreferences();
        GameLifecycle.OnPreLoad += OnPreLoad;
        LoggerInstance.Msg($"{Constants.ModName} {Constants.ModVersion} initialized.");
    }

    public override void OnApplicationQuit()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
    }

    private void OnPreLoad()
    {
        // S1API discovers concrete NPC subclasses during its NPC load flow.
        // Do not manually instantiate custom NPCs.
        if (DebugLogsEnabled)
        {
            LoggerInstance.Msg("Preparing early special-customer NPC definitions.");
        }
    }

    private static void InitializePreferences()
    {
        _preferencesCategory = MelonPreferences.CreateCategory(Constants.PreferencesCategory);
        _introMessagesEntry = _preferencesCategory.CreateEntry(
            "IntroMessages",
            Constants.Defaults.IntroMessagesEnabled,
            "Enable Intro Messages",
            "Allow each special customer to introduce themselves once per save.");
        _milestoneMessagesEntry = _preferencesCategory.CreateEntry(
            "MilestoneMessages",
            Constants.Defaults.MilestoneMessagesEnabled,
            "Enable Milestone Messages",
            "Allow occasional messages after completed deals.");
        _debugLogsEntry = _preferencesCategory.CreateEntry(
            "DebugLogs",
            Constants.Defaults.DebugLogsEnabled,
            "Enable Debug Logs",
            "Show detailed Early Special Customers messages in the MelonLoader console.");
        _preferencesCategory.SaveToFile(false);
    }
}
