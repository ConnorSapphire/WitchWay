using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Sprite
    {
        public Texture2D texture;
        public Rectangle rect;
        public Rectangle source;
        public Vector2 pos;
        public Vector2 size;
        public int Height;
        public int Width;
        public int z;

        public Sprite()
        {
        }

        public Sprite(Texture2D texture, Rectangle rect, int z = 0)
        {
            this.texture = texture;
            this.rect = rect;
            this.pos = new Vector2(rect.Left, rect.Top);
            this.size = new Vector2(rect.Width, rect.Height);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.source = new Rectangle(0, 0, Width, Height);
            this.z = z;
        }

        public Sprite(Texture2D texture, Rectangle rect, Rectangle source, int z = 0)
        {
            this.texture = texture;
            this.rect = rect;
            this.pos = new Vector2(rect.Left, rect.Top);
            this.size = new Vector2(rect.Width, rect.Height);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.source = source;
            this.z = z;
        }

        public Sprite(Texture2D texture, Vector2 pos, int z = 0)
        {
            this.texture = texture;
            this.pos = pos;
            this.size = new Vector2(texture.Width, texture.Height);
            this.rect = new Rectangle((int) pos.X, (int) pos.Y, (int) size.X, (int) size.Y);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.source = new Rectangle(0, 0, Width, Height);
            this.z = z;
        }

        public Sprite(Texture2D texture, Vector2 pos, Vector2 size, int z = 0)
        {
            this.texture = texture;
            this.pos = pos;
            this.size = size;
            this.rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.source = new Rectangle(0, 0, Width, Height);
            this.z = z;
        }

        public bool collides(Rectangle rect)
        {
            return this.rect.Intersects(rect);
        }

        public bool collides(Vector2 point)
        {
            return this.rect.Intersects(new Rectangle((int) point.X, (int) point.Y, 1, 1));
        }
    }
}
