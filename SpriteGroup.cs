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
        private List<Sprite> sprites;

        public SpriteGroup()
        {
            this.sprites = new List<Sprite>();
        }

        public SpriteGroup(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public void Add(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public void Remove(Sprite sprite)
        {
            sprites.Remove(sprite);
        }

        public void Clear()
        {
            sprites.Clear();
        }

        public bool Contains(Sprite sprite)
        {
            return sprites.Contains(sprite);
        }

        public List<Sprite> Collision(Sprite sprite)
        {
            List<Sprite> collisions = new List<Sprite>();
            foreach (Sprite s in sprites)
            {
                if (s.collides(sprite.rect))
                {
                    collisions.Add(s);
                }
            }
            return collisions;
        }

        public void Draw(SpriteBatch _spriteBatch, Player player)
        {
            int offsetX = player.sprite.rect.Center.X * Settings.scale - Settings.WIDTH / 2;
            int offsetY = player.sprite.rect.Center.Y * Settings.scale - Settings.HEIGHT / 2;

            for (int layer = 0; layer < Settings.zLayers; layer++)
            {
                foreach (Sprite sprite in sprites.OrderBy(x => -(x.rect.Bottom)))
                {
                    if (sprite.z == layer && sprite.texture != null)
                    {
                        Rectangle offsetRect = sprite.rect;
                        offsetRect.X *= Settings.scale;
                        offsetRect.Y *= Settings.scale;
                        offsetRect.X -= offsetX;
                        offsetRect.Y -= offsetY;
                        offsetRect.Width = sprite.rect.Width * Settings.scale;
                        offsetRect.Height = sprite.rect.Height * Settings.scale;
                        _spriteBatch.Draw(sprite.texture, offsetRect, sprite.source, Color.White);   
                    }
                }
            }
        }
    }
}
