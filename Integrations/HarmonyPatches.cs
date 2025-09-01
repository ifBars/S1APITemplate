#if MONO
using ScheduleOne;
#else
using Il2CppScheduleOne;
#endif
using HarmonyLib;

namespace $safeprojectname$.Integrations
{
    [HarmonyPatch]
    public static class HarmonyPatches
    {
        private static Core? _modInstance;

        /// <summary>
        /// Set the mod instance for patch callbacks
        /// </summary>
        public static void SetModInstance(Core modInstance)
        {
            _modInstance = modInstance;
        }
    }
}
