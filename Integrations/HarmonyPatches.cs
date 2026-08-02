namespace S1APITemplate.Integrations
{
    public static class HarmonyPatches
    {
        public static void Initialize(Core modInstance)
        {
            modInstance.HarmonyInstance.PatchAll(typeof(HarmonyPatches).Assembly);
        }
    }
}
