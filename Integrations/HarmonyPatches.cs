using HarmonyLib;

namespace S1APITemplate.Integrations
{
    [HarmonyPatch]
    public static class HarmonyPatches
    {
        private static Core? _modInstance;

        public static void Initialize(Core modInstance)
        {
            _modInstance = modInstance;
        }
    }
}
