using System.Collections.Generic;

namespace WitchWay
{
    internal static class Settings
    {
        public static int WIDTH;
        public static int HEIGHT;

        public static int Scale = 5;
        public static int ZLayers = 3;

        public static int SpriteWidth = 10;
        public static int SpriteHeight = 17;

        public static int Speed = 150;
        public static int Jump = -300;
        public static float FallMultiplier = 1.6F;
        public static int JumpHeight = 200;

        public static Dictionary<string, string> Layers = new Dictionary<string, string>
        {
            { "ground", "ground" },
            { "positions", "positions"}
        };

        public static Dictionary<string, string> Objects = new Dictionary<string, string>
        {
            { "player", "Player" }
        };
    }
}
