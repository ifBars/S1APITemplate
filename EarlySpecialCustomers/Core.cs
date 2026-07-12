using MelonLoader;
using RovingSpecialCustomers.Items;
using RovingSpecialCustomers.NPCs;
using RovingSpecialCustomers.Services;
using RovingSpecialCustomers.Utils;
using S1API.Lifecycle;
using UnityEngine;

[assembly: MelonInfo(typeof(RovingSpecialCustomers.Core), Constants.ModName, Constants.ModVersion, Constants.ModAuthor)]
[assembly: MelonGame(Constants.Game.Studio, Constants.Game.Name)]

namespace RovingSpecialCustomers;

public sealed class Core : MelonMod
{
    private static MelonPreferences_Entry<bool>? _debugLogs;

    public static bool GameLoaded { get; private set; }
    public static bool DebugLogsEnabled => _debugLogs?.Value ?? false;

    public override void OnInitializeMelon()
    {
        var category = MelonPreferences.CreateCategory(Constants.PreferencesCategory);
        _debugLogs = category.CreateEntry("DebugLogs", false, "Enable debug logs");
        category.SaveToFile(false);

        GameLifecycle.OnPreLoad += HandlePreLoad;
        GameLifecycle.OnLoadComplete += HandleLoadComplete;
        GameLifecycle.OnPreSceneChange += HandlePreSceneChange;
        LoggerInstance.Msg($"{Constants.ModName} {Constants.ModVersion} initialized.");
    }

    public override void OnUpdate()
    {
        if (GameLoaded)
        {
            SpecialCustomerDispatcher.Instance?.Tick(Time.unscaledDeltaTime);
        }
    }

    public override void OnApplicationQuit()
    {
        GameLifecycle.OnPreLoad -= HandlePreLoad;
        GameLifecycle.OnLoadComplete -= HandleLoadComplete;
        GameLifecycle.OnPreSceneChange -= HandlePreSceneChange;
        HandlePreSceneChange();
    }

    private static void HandlePreLoad()
    {
        ExclusiveItemRegistry.Initialize();
    }

    private static void HandleLoadComplete()
    {
        GameLoaded = true;
        if (DebugLogsEnabled)
        {
            MelonLogger.Msg("[RSC] Game load complete; visit coordinator enabled.");
        }
    }

    private static void HandlePreSceneChange()
    {
        GameLoaded = false;
        RovingCrewMember.HideAll();
        SpecialCustomerRuntime.DestroyCrewVehicle();
    }
}
