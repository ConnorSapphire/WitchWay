using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Tilemap
    {
        public int Width;
        public int Height;
        public int tileWidth;
        public int tileHeight;
        public Dictionary<int, Tileset> tilesets;
        public Dictionary<string, Layer> layers;
        public Dictionary<string, ObjectGroup> objectsGroups;
        public Tilemap(int Width, int Height, int tileWidth, int tileHeight, Dictionary<int, Tileset> tilesets, Dictionary<string, Layer> layers, Dictionary<string, ObjectGroup> objectGroups) {
            this.Width = Width;
            this.Height = Height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tilesets = tilesets;
            this.layers = layers;
            this.objectsGroups = objectGroups;
        }

        public List<Sprite> GetLayer(string name)
        {
            Layer layer = layers[name];

            Dictionary<int, Tileset> set = new Dictionary<int, Tileset>();
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < layer.height; i++)
            {
                for (int j = 0; j < layer.width; j++)
                {
                    int gid = layer.data[i, j];
                    if (gid == 0)
                    {
                        continue;
                    }
                    if (set.ContainsKey(gid))
                    {
                        int col = (gid - set[gid].firstgid) % set[gid].columns;
                        int row = (gid - set[gid].firstgid) / set[gid].columns;
                        sprites.Add(new Sprite(set[gid].texture, new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight), new Rectangle(col * tileWidth, row * tileHeight, tileWidth, tileHeight)));
                    } 
                    else
                    {
                        int key = 0;
                        foreach (int firstgid in tilesets.Keys)
                        {
                            if (firstgid <= gid)
                            {
                                key = firstgid;
                            }
                            else
                            {
                                break;
                            }
                        }
                        set.Add(gid, tilesets[key]);
                        int col = (gid - set[gid].firstgid) % set[gid].columns;
                        int row = (gid - set[gid].firstgid) / set[gid].columns;
                        sprites.Add(new Sprite(set[gid].texture, new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight), new Rectangle(col* tileWidth, row * tileHeight, tileWidth, tileHeight)));
                    }
                }
            }

            return sprites;           
        }

        public ObjectGroup GetObjectGroup(string name)
        {
            return objectsGroups[name];
        }

        public List<Sprite> GetSpritesFromObjectGroup(string name)
        {
            ObjectGroup objectGroup = objectsGroups[name];
            List<TilemapObject> images = new List<TilemapObject>();
            foreach (TilemapObject obj in objectGroup.objects)
            {
                if (obj.type == "image")
                {
                    images.Add(obj);
                }
            }
            List<Sprite> sprites = new List<Sprite>();
            Dictionary<int, Tileset> set = new Dictionary<int, Tileset>();
            foreach (TilemapObject obj in images)
            {
                if (set.ContainsKey(obj.gid))
                {
                    int col = (obj.gid - set[obj.gid].firstgid) % set[obj.gid].columns;
                    int row = (obj.gid - set[obj.gid].firstgid) / set[obj.gid].columns;
                    sprites.Add(new Sprite(set[obj.gid].texture, new Rectangle(obj.x, obj.y - obj.height, obj.width, obj.height), new Rectangle(col * obj.width, row * obj.height, obj.width, obj.height)));
                }
                else
                {
                    int key = 1;
                    foreach (int firstgid in tilesets.Keys)
                    {
                        if (firstgid <= obj.gid)
                        {
                            key = firstgid;
                        }
                        else
                        {
                            break;
                        }
                    }
                    set.Add(obj.gid, tilesets[key]);
                    int col = (obj.gid - set[obj.gid].firstgid) % set[obj.gid].columns;
                    int row = (obj.gid - set[obj.gid].firstgid) / set[obj.gid].columns;
                    sprites.Add(new Sprite(set[obj.gid].texture, new Rectangle(obj.x, obj.y - obj.height, obj.width, obj.height), new Rectangle(col * obj.width, row * obj.height, obj.width, obj.height)));
                }
            }
            return sprites;
        }
    }
}
