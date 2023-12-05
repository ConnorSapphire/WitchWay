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
    internal class SpriteGroup
    {
        private List<Sprite> Sprites;

        public SpriteGroup()
        {
            this.Sprites = new List<Sprite>();
        }

        public SpriteGroup(Sprite Sprite)
        {
            Sprites.Add(Sprite);
        }

        public void Add(Sprite Sprite)
        {
            Sprites.Add(Sprite);
        }

        public void Remove(Sprite Sprite)
        {
            Sprites.Remove(Sprite);
        }

        public void Clear()
        {
            Sprites.Clear();
        }

        public bool Contains(Sprite Sprite)
        {
            return Sprites.Contains(Sprite);
        }

        public List<Sprite> Collision(Vector2 Point)
        {
            List<Sprite> Collisions = new List<Sprite>();
            foreach (Sprite s in Sprites)
            {
                if (s.collides(Point))
                {
                    Collisions.Add(s);
                }
            }
            return Collisions;
        }

        public List<Sprite> Collision(Sprite Sprite)
        {
            List<Sprite> Collisions = new List<Sprite>();
            foreach (Sprite s in Sprites)
            {
                if (s.collides(Sprite.Rect))
                {
                    Collisions.Add(s);
                }
            }
            return Collisions;
        }

        public void Draw(SpriteBatch _spriteBatch, Player Player)
        {
            int OffsetX = (Player.Sprite.Rect.Center.X * Settings.Scale) - (Settings.WIDTH / 2);
            int OffsetY = (Player.Sprite.Rect.Center.Y * Settings.Scale) - (Settings.HEIGHT / 2);

            for (int Layer = 0; Layer < Settings.ZLayers; Layer++)
            {
                foreach (Sprite Sprite in Sprites.OrderBy(x => -(x.Rect.Bottom)))
                {
                    if (Sprite.Z == Layer && Sprite.Texture != null)
                    {
                        Rectangle OffsetRect = Sprite.Rect;
                        OffsetRect.X *= Settings.Scale;
                        OffsetRect.Y *= Settings.Scale;
                        OffsetRect.X -= OffsetX;
                        OffsetRect.Y -= OffsetY;
                        OffsetRect.Width = Sprite.Rect.Width * Settings.Scale;
                        OffsetRect.Height = Sprite.Rect.Height * Settings.Scale;
                        _spriteBatch.Draw(Sprite.Texture, OffsetRect, Sprite.Source, Color.White);   
                    }
                }
            }
        }
    }
}
