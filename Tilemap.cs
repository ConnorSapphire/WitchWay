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
        public int TileWidth;
        public int TileHeight;
        public Dictionary<int, Tileset> TileSets;
        public Dictionary<string, Layer> Layers;
        public Dictionary<string, ObjectGroup> ObjectGroups;
        public Tilemap(int Width, int Height, int TileWidth, int TileHeight, Dictionary<int, Tileset> TileSets, Dictionary<string, Layer> Layers, Dictionary<string, ObjectGroup> ObjectGroups) {
            this.Width = Width;
            this.Height = Height;
            this.TileWidth = TileWidth;
            this.TileHeight = TileHeight;
            this.TileSets = TileSets;
            this.Layers = Layers;
            this.ObjectGroups = ObjectGroups;
        }

        public List<Sprite> GetLayer(string Name)
        {
            Layer Layer = Layers[Name];

            Dictionary<int, Tileset> Set = new Dictionary<int, Tileset>();
            List<Sprite> Sprites = new List<Sprite>();
            for (int i = 0; i < Layer.Height; i++)
            {
                for (int j = 0; j < Layer.Width; j++)
                {
                    int gid = Layer.Data[i, j];
                    if (gid == 0)
                    {
                        continue;
                    }
                    if (Set.ContainsKey(gid))
                    {
                        int col = (gid - Set[gid].firstgid) % Set[gid].Columns;
                        int row = (gid - Set[gid].firstgid) / Set[gid].Columns;
                        Sprites.Add(new Sprite(Set[gid].Texture, new Rectangle(j * TileWidth, i * TileHeight, TileWidth, TileHeight), new Rectangle(col * TileWidth, row * TileHeight, TileWidth, TileHeight)));
                    } 
                    else
                    {
                        int key = 0;
                        foreach (int firstgid in TileSets.Keys)
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
                        Set.Add(gid, TileSets[key]);
                        int col = (gid - Set[gid].firstgid) % Set[gid].Columns;
                        int row = (gid - Set[gid].firstgid) / Set[gid].Columns;
                        Sprites.Add(new Sprite(Set[gid].Texture, new Rectangle(j * TileWidth, i * TileHeight, TileWidth, TileHeight), new Rectangle(col* TileWidth, row * TileHeight, TileWidth, TileHeight)));
                    }
                }
            }

            return Sprites;           
        }

        public ObjectGroup GetObjectGroup(string Name)
        {
            return ObjectGroups[Name];
        }

        public List<Sprite> GetSpritesFromObjectGroup(string Name)
        {
            ObjectGroup ObjectGroup = ObjectGroups[Name];
            List<TilemapObject> Images = new List<TilemapObject>();
            foreach (TilemapObject Obj in ObjectGroup.objects)
            {
                if (Obj.Type == "image")
                {
                    Images.Add(Obj);
                }
            }
            List<Sprite> Sprites = new List<Sprite>();
            Dictionary<int, Tileset> Set = new Dictionary<int, Tileset>();
            foreach (TilemapObject Obj in Images)
            {
                if (Set.ContainsKey(Obj.gid))
                {
                    int col = (Obj.gid - Set[Obj.gid].firstgid) % Set[Obj.gid].Columns;
                    int row = (Obj.gid - Set[Obj.gid].firstgid) / Set[Obj.gid].Columns;
                    Sprites.Add(new Sprite(Set[Obj.gid].Texture, new Rectangle(Obj.X, Obj.Y - Obj.Height, Obj.Width, Obj.Height), new Rectangle(col * Obj.Width, row * Obj.Height, Obj.Width, Obj.Height)));
                }
                else
                {
                    int key = 1;
                    foreach (int firstgid in TileSets.Keys)
                    {
                        if (firstgid <= Obj.gid)
                        {
                            key = firstgid;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Set.Add(Obj.gid, TileSets[key]);
                    int col = (Obj.gid - Set[Obj.gid].firstgid) % Set[Obj.gid].Columns;
                    int row = (Obj.gid - Set[Obj.gid].firstgid) / Set[Obj.gid].Columns;
                    Sprites.Add(new Sprite(Set[Obj.gid].Texture, new Rectangle(Obj.X, Obj.Y - Obj.Height, Obj.Width, Obj.Height), new Rectangle(col * Obj.Width, row * Obj.Height, Obj.Width, Obj.Height)));
                }
            }
            return Sprites;
        }
    }
}
