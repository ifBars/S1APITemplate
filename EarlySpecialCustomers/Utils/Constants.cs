namespace RovingSpecialCustomers.Utils;

public static class Constants
{
    public const string ModName = "Roving Special Customers";
    public const string ModVersion = "0.2.0-alpha";
    public const string ModAuthor = "Bars";
    public const string PreferencesCategory = "RovingSpecialCustomers";

    public static class Game
    {
        public const string Studio = "TVGS";
        public const string Name = "Schedule I";
    }

    public static class Timing
    {
        public const int ArrivalTime = 900;
        public const int FirstVisitMinDays = 1;
        public const int FirstVisitMaxDays = 4;
        public const int RepeatVisitMinDays = 5;
        public const int RepeatVisitMaxDays = 10;
        public const int NoticeMinDays = 2;
        public const int NoticeMaxDays = 4;
    }

    public static class World
    {
        public const float HiddenY = -200f;
    }
}
