namespace S1APITemplate.Utils
{
    public static class Constants
    {
        public const string ModName = "S1APITemplate";
        public const string ModVersion = "1.0.0";
        public const string ModAuthor = "YourName";
        public const string ModDescription = "Mod description.";

        public const string PreferencesCategory = ModName;

        public static class Defaults
        {
            public const bool DebugLogsEnabled = false;
        }

        public static class Constraints
        {
            public const float MinConstraint = 0f;
            public const float MaxConstraint = 100f;
        }

        public static class Game
        {
            public const string Studio = "TVGS";
            public const string Name = "Schedule I";
        }
    }
}
