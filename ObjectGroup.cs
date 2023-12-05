using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class ObjectGroup
    {
        public int id;
        public string name;
        public List<TilemapObject> objects;

        public ObjectGroup(int id, string name) 
        { 
            this.id = id;
            this.name = name;
            objects = new List<TilemapObject>();
        }

        public List<Vector2> GetPositions()
        {
            List<Vector2> positions = new List<Vector2>();
            foreach (TilemapObject obj in objects)
            {
                positions.Add(new Vector2(obj.X, obj.Y));
            }
            return positions;
        }

        public List<TilemapObject> GetNamedObjects(string name) 
        {
            List<TilemapObject> ls = new List<TilemapObject>();
            foreach (TilemapObject obj in objects)
            {
                if (obj.name == name)
                {
                    ls.Add(obj);
                }
            }
            return ls;
        }
    }
}
