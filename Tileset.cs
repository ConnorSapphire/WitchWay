﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Tileset
    {
        public string name;
        public int firstgid;
        public int tileWidth;
        public int tileHeight;
        public int tileCount;
        public int Columns;
        public Texture2D Texture;
        public Tileset(string name, int firstgid, int tileWidth, int tileHeight, int tileCount, int columns, Texture2D texture) 
        {
            this.name = name;
            this.firstgid = firstgid;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileCount = tileCount;
            this.Columns = columns;
            this.Texture = texture;
        }
    }
}
