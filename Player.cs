using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Player
    {
        private Dictionary<string, List<Texture2D>> spriteSheet;

        public Sprite sprite;
        private Texture2D texture;
        private double currentFrame;
        private int rows;
        private int columns;
        private int totalFrames;

        private int speed;
        private int gravity;
        private bool onGround = true;
        private int direction;
        private Vector2 position;
        private string state;

        private SpriteGroup collidable;
        public Player(Vector2 position, Dictionary<string, List<Texture2D>> spriteSheet, Sprite sprite, SpriteGroup collidable) 
        {
            this.spriteSheet = spriteSheet;
            this.currentFrame = 0;

            this.speed = Settings.speed;
            this.gravity = 0;
            this.direction = 1;
            this.position = position;
            this.state = "right_idle";
            this.texture = spriteSheet[state][(int) currentFrame];
            this.sprite = sprite;
            UpdateSprite();
            this.position.Y -= this.sprite.Height;

            this.collidable = collidable;
        }

        public void UpdateSprite()
        {
            sprite.texture = texture;
            sprite.pos = position;
            sprite.size = new Vector2(texture.Width, texture.Height);
            sprite.Width = texture.Width;
            sprite.Height = texture.Height;
            sprite.rect = new Rectangle((int) sprite.pos.X, (int) sprite.pos.Y, sprite.Width, sprite.Height);
            sprite.source = new Rectangle(0,0, sprite.Width, sprite.Height);
            sprite.z = 1;
        }

        public void Input(GameTime gameTime, KeyboardState keys)
        {
            if (keys.IsKeyDown(Keys.Left))
            {
                if (onGround)
                {
                    state = "left_run";
                }
                else
                {
                    state = "left_jump";
                }
                direction = -1;
            }
            else if (keys.IsKeyDown(Keys.Right))
            {
                if (onGround)
                {
                    state = "right_run";
                } else
                {
                    state = "right_jump";
                }
                direction = 1;
            }
            else
            {
                state = state.Split("_")[0] + "_idle";
                direction = 0;
            }

            if (keys.IsKeyDown(Keys.Up) && onGround)
            {
                state = state.Split("_")[0] + "_jump";
                onGround = false;
                gravity = Settings.jump;
            }

            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            position.X += direction * speed * deltaTime;
            position.Y += gravity * deltaTime;
            UpdateSprite();
        }

        public void Collision()
        {
            List<Sprite> collisions = collidable.Collision(sprite);
            foreach (Sprite collision in collisions)
            {
                if (position.X > collision.pos.X + (collision.Width / 2) && position.X < collision.pos.X + collision.Width)
                {
                    position.X = collision.pos.X + collision.Width;
                }
                else if (position.X + sprite.Width <= collision.pos.X + (collision.Width / 2) && position.X + sprite.Width > collision.pos.X)
                {
                    position.X = collision.pos.X - sprite.Width;
                }
            }
            UpdateSprite();

            collisions = collidable.Collision(sprite);
            foreach (Sprite collision in collisions)
            { 
                if (position.Y > collision.pos.Y + (collision.Height / 2) && position.Y <  collision.pos.Y + collision.Height)
                {
                    position.Y = collision.pos.Y + collision.Height;
                }
                else if (position.Y + sprite.Height <= collision.pos.Y + (collision.Height / 2) && position.Y + sprite.Height > collision.pos.Y)
                {
                    position.Y = collision.pos.Y - sprite.Height;
                    onGround = true;
                    gravity = 0;
                }
            }
            UpdateSprite();
        }

        public void Update(GameTime gameTime, KeyboardState keys)
        {
            // update player state
            Input(gameTime, keys);
            Collision();
            if (!onGround && gravity > 0)
            {
                gravity -= 50;
            } 
            else if (!onGround && gravity <= 0)
            {
                gravity += 150;
            } 

            // get current frame
            totalFrames = spriteSheet[state].Count;
            currentFrame += 12 * gameTime.ElapsedGameTime.TotalSeconds;
            if (state.Split("_")[1] == "jump" && currentFrame == totalFrames)
            {
                currentFrame -= 1;
            }
            currentFrame %= totalFrames;

            // get current spritesheet
            texture = spriteSheet[state][(int) currentFrame];
            UpdateSprite();

        }
    }
}
