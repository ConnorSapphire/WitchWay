using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WitchWay
{
    internal static class Support
    {
        public static Dictionary<String, List<Texture2D>> GetSpritesFromChildFolders(ContentManager Content, string parent_folder)
        {
            Dictionary<String, List<Texture2D>> sprites = new Dictionary<String, List<Texture2D>>();
            string[] folders = Directory.GetDirectories(parent_folder);
            foreach (string folder in folders)
            {
                string[] files = Directory.GetFiles(folder);
                List<Texture2D> list = new List<Texture2D>();
                foreach (string file in files)
                {
                    string filename = file.Split(".xnb")[0].Split("Content\\").Last();
                    list.Add(Content.Load<Texture2D>(filename));
                }
                sprites.Add(folder.Split("\\").Last(), list);
            }
            return sprites;
        }

        public static Tileset GetTileset(ContentManager Content, string file, int firstgid)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode set = doc.GetElementsByTagName("tileset").Item(0);

            string name = set.Attributes["name"].Value;
            int tileWidth = int.Parse(set.Attributes["tilewidth"].Value);
            int tileHeight = int.Parse(set.Attributes["tileheight"].Value);
            int tileCount = int.Parse(set.Attributes["tilecount"].Value);
            int columns = int.Parse(set.Attributes["columns"].Value);

            XmlNode image = doc.GetElementsByTagName("image").Item(0);
            string imageSource = "data\\" + image.Attributes["source"].Value.Split(".")[0];
            Texture2D texture = Content.Load<Texture2D>(imageSource);

            Tileset tileset = new Tileset(name, firstgid, tileWidth, tileHeight, tileCount, columns, texture);
            return tileset;
        }

        public static Tilemap GetTilemap(ContentManager Content, string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode map = doc.GetElementsByTagName("map").Item(0);

            int width = int.Parse(map.Attributes["width"].Value);
            int height = int.Parse(map.Attributes["height"].Value);
            int tileWidth = int.Parse(map.Attributes["tilewidth"].Value);
            int tileHeight = int.Parse(map.Attributes["tileheight"].Value);

            XmlNodeList tilesetnodes = doc.GetElementsByTagName("tileset");
            Dictionary<int, Tileset> tilesets = new Dictionary<int, Tileset>();
            foreach (XmlNode tilesetnode in tilesetnodes)
            {
                int firstgid = int.Parse(tilesetnode.Attributes["firstgid"].Value);
                string source = tilesetnode.Attributes["source"].Value;
                tilesets.Add(firstgid, GetTileset(Content, Directory.GetCurrentDirectory() + "\\Content\\data\\" + source, firstgid));
            }

            XmlNodeList layernodes = doc.GetElementsByTagName("layer");
            Dictionary<string, Layer> layers = new Dictionary<string, Layer>();
            foreach (XmlNode layernode in layernodes)
            {
                int id = int.Parse(layernode.Attributes["id"].Value);
                string name = layernode.Attributes["name"].Value;
                int layerWidth = int.Parse(layernode.Attributes["width"].Value);
                int layerHeight = int.Parse(layernode.Attributes["height"].Value);
                int[,] data = new int[layerHeight, layerWidth];
                string[] rows = layernode.ChildNodes[0].InnerText.Trim().Split("\n");
                int i = 0;
                foreach (string row in rows)
                {
                    int j = 0;
                    foreach (string col in row.Trim().TrimEnd(',').Split(","))
                    {
                        data[i, j] = int.Parse(col.Trim());
                        j++;
                    }
                    i++;
                }
                layers.Add(name, new Layer(data, id, name, layerWidth, layerHeight));
            }

            XmlNodeList objectGroupNodes = doc.GetElementsByTagName("objectgroup");
            Dictionary<string, ObjectGroup> objectGroups = new Dictionary<string, ObjectGroup>();
            foreach (XmlNode objectGroupNode in objectGroupNodes)
            {
                ObjectGroup currentGroup = new ObjectGroup(int.Parse(objectGroupNode.Attributes["id"].Value), objectGroupNode.Attributes["name"].Value);
                objectGroups.Add(objectGroupNode.Attributes["name"].Value, currentGroup);
                XmlNodeList objectNodes = objectGroupNode.ChildNodes;
                foreach (XmlNode objectNode in objectNodes)
                {
                    int id = int.Parse(objectNode.Attributes["id"].Value);
                    int x = (int) float.Parse(objectNode.Attributes["x"].Value);
                    int y = (int) float.Parse(objectNode.Attributes["y"].Value);
                    string name = "";
                    if (objectNode.Attributes["name"] != null)
                    {
                        name = objectNode.Attributes["name"].Value;
                    }
                    if (objectNode.LastChild != null && objectNode.LastChild.Name == "point")
                    {
                        currentGroup.objects.Add(new TilemapObject(id, name, "point", x, y));
                    }
                    else
                    {
                        int gid = int.Parse(objectNode.Attributes["gid"].Value);
                        int objectWidth = int.Parse(objectNode.Attributes["width"].Value);
                        int objectHeight = int.Parse(objectNode.Attributes["height"].Value);
                        currentGroup.objects.Add(new TilemapObject(id, name, "image", gid, x, y, objectWidth, objectHeight));
                    }
                }
            }

            Tilemap tilemap = new Tilemap(width, height, tileWidth, tileHeight, tilesets, layers, objectGroups);

            return tilemap;
        }

        public static (bool, Vector2, Sprite, double, double, double) RaycastRect(Rectangle Rect, double Distance, double Direction, SpriteGroup Collidables, int RaycastGap)
        {
            (Vector2, Sprite, double) TopLeft = Support.Raycast(Rect.X, Rect.Y, Distance, Direction, Collidables, RaycastGap);
            (Vector2, Sprite, double) TopRight = Support.Raycast(Rect.X + Rect.Width, Rect.Y, Distance, Direction, Collidables, RaycastGap);
            (Vector2, Sprite, double) BottomLeft = Support.Raycast(Rect.X, Rect.Y + Rect.Height, Distance, Direction, Collidables, RaycastGap);
            (Vector2, Sprite, double) BottomRight = Support.Raycast(Rect.X + Rect.Width, Rect.Y + Rect.Height, Distance, Direction, Collidables, RaycastGap);

            if (TopLeft.Item2 != null && TopLeft.Item3 <= TopRight.Item3 && TopLeft.Item3 <= BottomRight.Item3 && TopLeft.Item3 <= BottomLeft.Item3)
                return (true, TopLeft.Item1, TopLeft.Item2, TopLeft.Item3, 0, 0);
            else if (TopRight.Item2 != null && TopRight.Item3 <= TopLeft.Item3 && TopRight.Item3 <= BottomRight.Item3 && TopRight.Item3 <= BottomLeft.Item3)
                return (true, TopRight.Item1, TopRight.Item2, TopRight.Item3, Rect.Width, 0);
            else if (BottomLeft.Item2 != null && BottomLeft.Item3 <= TopLeft.Item3 && BottomLeft.Item3 <= TopRight.Item3 && BottomLeft.Item3 <= BottomRight.Item3)
                return (true, BottomLeft.Item1, BottomLeft.Item2, BottomLeft.Item3, 0, Rect.Height);
            else if (BottomRight.Item2 != null && BottomRight.Item3 <= TopLeft.Item3 && BottomRight.Item3 <= TopRight.Item3 && BottomRight.Item3 <= BottomLeft.Item3)
                return (true, BottomRight.Item1, BottomRight.Item2, BottomRight.Item3, Rect.Width, Rect.Height);

            return (false, TopLeft.Item1, null, 0, 0, 0);
        }

        public static (Vector2, Sprite, double) Raycast(double X, double Y, double Distance, double Direction, SpriteGroup Collidables, int RaycastGap)
        {
            Vector2 point = new Vector2();
            Sprite collision = null;

            for (int i = 0; i < Distance; i += RaycastGap)
            {
                (point, collision) = CheckCollision(X, Y, i, Direction, Collidables);
                if (collision != null)
                {
                    return (point, collision, i);
                }
            }
            
            (point, collision) = CheckCollision(X, Y, Distance, Direction, Collidables);
            return (point, collision, Distance);
        }

        private static (Vector2, Sprite) CheckCollision(double x, double y, double distance, double direction, SpriteGroup collidables)
        {
            x +=  (distance * Math.Cos(direction));
            y +=  (distance * Math.Sin(direction));
            Vector2 point = new Vector2((float) x, (float) y);
            List<Sprite> collisions = collidables.Collision(point);
            if (collisions.Any())
            {
                return (point, collisions.FirstOrDefault()); ;
            }
            return (point, null);
        }
    }
}
