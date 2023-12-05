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
        public Texture2D Texture;
        public Rectangle Rect;
        public Rectangle Source;
        public Vector2 Position;
        public Vector2 Size;
        public int Height;
        public int Width;
        public int Z;

        public Sprite()
        {
        }

        public Sprite(Texture2D texture, Rectangle rect, int z = 0)
        {
            this.Texture = texture;
            this.Rect = rect;
            this.Position = new Vector2(rect.Left, rect.Top);
            this.Size = new Vector2(rect.Width, rect.Height);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.Source = new Rectangle(0, 0, Width, Height);
            this.Z = z;
        }

        public Sprite(Texture2D texture, Rectangle rect, Rectangle source, int z = 0)
        {
            this.Texture = texture;
            this.Rect = rect;
            this.Position = new Vector2(rect.Left, rect.Top);
            this.Size = new Vector2(rect.Width, rect.Height);
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.Source = source;
            this.Z = z;
        }

        public Sprite(Texture2D texture, Vector2 pos, int z = 0)
        {
            this.Texture = texture;
            this.Position = pos;
            this.Size = new Vector2(texture.Width, texture.Height);
            this.Rect = new Rectangle((int) pos.X, (int) pos.Y, (int) Size.X, (int) Size.Y);
            this.Width = Rect.Width;
            this.Height = Rect.Height;
            this.Source = new Rectangle(0, 0, Width, Height);
            this.Z = z;
        }

        public Sprite(Texture2D texture, Vector2 pos, Vector2 size, int z = 0)
        {
            this.Texture = texture;
            this.Position = pos;
            this.Size = size;
            this.Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            this.Width = Rect.Width;
            this.Height = Rect.Height;
            this.Source = new Rectangle(0, 0, Width, Height);
            this.Z = z;
        }

        public bool collides(Rectangle rect)
        {
            return this.Rect.Intersects(rect);
        }

        public bool collides(Vector2 point)
        {
            if (point.X > Rect.X && point.X < Rect.X + Rect.Width)
            {
                if (point.Y > Rect.Y && point.Y <= Rect.Y + Rect.Height)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
