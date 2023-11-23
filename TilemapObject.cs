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
        public string type;
        public int gid;
        public int x;
        public int y;
        public int width;
        public int height;

        public TilemapObject(int id, string name, string type, int x, int y)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.x = x;
            this.y = y;
        }

        public TilemapObject(int id, string name, string type, int gid, int x, int y, int width, int height)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.gid = gid;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
