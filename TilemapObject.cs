using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class TilemapObject
    {
        public int id;
        public string name;
        public string Type;
        public int gid;
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public TilemapObject(int id, string name, string type, int x, int y)
        {
            this.id = id;
            this.name = name;
            this.Type = type;
            this.X = x;
            this.Y = y;
        }

        public TilemapObject(int id, string name, string type, int gid, int x, int y, int width, int height)
        {
            this.id = id;
            this.name = name;
            this.Type = type;
            this.gid = gid;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }
}
