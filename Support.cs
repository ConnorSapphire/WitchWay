using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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
    }
}
