using System.Collections.Generic;

namespace WitchWay
{
    internal static class Settings
    {
        public static int WIDTH;
        public static int HEIGHT;
        public static int scale = 3;
        public static int zLayers = 3;

        public static int spriteWidth = 10;
        public static int spriteHeight = 17;
        public static int speed = 300;
        public static int jump = -600;

        public static Dictionary<string, string> layers = new Dictionary<string, string>
        {
            { "ground", "ground" },
            { "positions", "positions"}
        };

        public static Dictionary<string, string> objects = new Dictionary<string, string>
        {
            { "player", "Player" }
        };
    }
}
