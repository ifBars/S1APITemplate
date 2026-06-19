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

        public override void OnInitializeMelon()
        {
            Instance = this;
            HarmonyPatches.Initialize(this);
            GameLifecycle.OnLoadComplete += OnLoadComplete;

            LoggerInstance.Msg($"{Constants.ModName} initialized.");
        }

        public override void OnApplicationQuit()
        {
            GameLifecycle.OnLoadComplete -= OnLoadComplete;
            Instance = null;
        }

        private void OnLoadComplete()
        {
            LoggerInstance.Msg("Save load completed.");
        }
    }
}
