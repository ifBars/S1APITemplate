using MelonLoader;
using S1API;
using $safeprojectname$.Integrations;
using $safeprojectname$.Utils;

[assembly: MelonInfo(typeof($safeprojectname$.Core), Constants.MOD_NAME, Constants.MOD_VERSION, Constants.MOD_AUTHOR)]
[assembly: MelonGame(Constants.Game.GAME_STUDIO, Constants.Game.GAME_NAME)]

namespace $safeprojectname$
{
    public class Core : MelonMod
    {
        public static Core? Instance { get; private set; }

        public override void OnInitializeMelon()
        {
            Instance = this;
            HarmonyPatches.SetModInstance(this);
        }

        public override void OnApplicationQuit()
        {
            Instance = null;
        }
    }
}