using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Player
    {
        private Dictionary<string, List<Texture2D>> SpriteSheet;

        public Sprite Sprite;
        private Texture2D Texture;
        private double CurrentFrame;
        private int TotalFrames;

        private double VelocityX;
        private double VelocityY;
        private bool OnGround = true;
        private bool jumpUnreleased = false;
        private double jumped = 0;

        private int MaxSpeed;
        private Vector2 Position;
        private string State;

        private SpriteGroup Collidables;
        public Player(Vector2 Position, Dictionary<string, List<Texture2D>> SpriteSheet, Sprite Sprite, SpriteGroup Collidables)
        {
            this.SpriteSheet = SpriteSheet;
            this.CurrentFrame = 0;

            this.MaxSpeed = Settings.Speed;
            this.VelocityX = 0;
            this.VelocityY = 0;
            this.Position = Position;

            this.State = "right_idle";
            this.Texture = SpriteSheet[State][(int)CurrentFrame];
            this.Sprite = Sprite;
            UpdateSprite();

            this.Position.Y -= this.Sprite.Height;
            UpdateSprite();

            this.Collidables = Collidables;
        }

        public void UpdateSprite()
        {
            Sprite.Texture = Texture;
            Sprite.Position = Position;
            Sprite.Size = new Vector2(Texture.Width, Texture.Height);
            Sprite.Width = Texture.Width;
            Sprite.Height = Texture.Height;
            Sprite.Rect = new Rectangle((int)Sprite.Position.X, (int)Sprite.Position.Y, Sprite.Width, Sprite.Height);
            Sprite.Source = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            Sprite.Z = 1;
        }

        private void Input(GameTime GameTime, KeyboardState Keys)
        {
            double DeltaTime = GameTime.ElapsedGameTime.TotalSeconds;

            if (Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                if (OnGround)
                {
                    State = "left_run";
                }
                else
                {
                    State = "left_jump";
                }
                VelocityX = -MaxSpeed;
            }
            else if (Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                if (OnGround)
                {
                    State = "right_run";
                }
                else
                {
                    State = "right_jump";
                }
                VelocityX = MaxSpeed;
            }
            else
            {
                State = State.Split("_")[0] + "_idle";
                VelocityX = 0;
            }

            if (Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z) && (OnGround | jumpUnreleased))
            {
                State = State.Split("_")[0] + "_jump";
                OnGround = false;
                VelocityY = Settings.Jump;
                jumpUnreleased = true;
                jumped += VelocityY * DeltaTime;
            }
            else
            {
                jumpUnreleased = false;
                jumped = 0;
                VelocityY =  (-Settings.Jump);
            }

            if (jumpUnreleased && jumped < -Settings.JumpHeight)
            {
                VelocityY =  (-Settings.Jump);
                jumpUnreleased = false;
                jumped = 0;
            }

            if (OnGround)
                VelocityY = 0;

            if (Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                VelocityY = MaxSpeed;
            }
            else if (Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                VelocityY = -MaxSpeed;
            }

        }

        private void Move(GameTime GameTime)
        {
            double DeltaTime = GameTime.ElapsedGameTime.TotalSeconds;

            double XDist = VelocityX * DeltaTime;
            double YDist = VelocityY * DeltaTime;

            double Distance = Math.Sqrt((Math.Pow(XDist, 2) + Math.Pow(YDist, 2.0)));
            double Theta = Math.Atan2(YDist, XDist);

            CollisionDetection(Distance, Theta);
        }

        private void CollisionDetection(double Distance, double Direction)
        {
            (bool, Vector2, Sprite, double, double, double) CornerCollision = Support.RaycastRect(Sprite.Rect, Distance, Direction, Collidables, Sprite.Rect.Width / 2);

            if (!CornerCollision.Item1)
            {
                Position.X = CornerCollision.Item2.X;
                Position.Y = CornerCollision.Item2.Y;
                return;
            }

            CalculateCollisionOffset(Direction, CornerCollision.Item2, CornerCollision.Item3, CornerCollision.Item5, CornerCollision.Item6);
        }

        private void CalculateCollisionOffset(double Direction, Vector2 CollisionPosition, Sprite Tile, double OffsetX, double OffsetY)
        {
            double R, X, Y, DeltaX, DeltaY;

            float TileTop = Tile.Position.Y;
            float TileBottom = TileTop + Tile.Height;

            float TileLeft = Tile.Position.X;
            float TileRight = TileLeft + Tile.Width;
            
            bool Left = Math.Abs(Direction) <= Math.PI / 2;
            bool Down = Direction >= 0;

            if (Left && Down)
            {
                if (Math.Cos(Direction) != 0)
                {
                    R = (TileLeft - CollisionPosition.X) / Math.Cos(Direction);
                    DeltaY = R * Math.Sin(Direction);
                    Y = CollisionPosition.Y + DeltaY;
                    if (Y >= TileTop && Y <= TileBottom)
                    {
                        Position.X = (float)(TileLeft - OffsetX);
                        Position.Y = (float)(Y - OffsetY);
                        return;
                    }
                }

                if (Math.Sin(Direction) != 0)
                {
                    R = (TileTop - CollisionPosition.Y) / Math.Sin(Direction);
                    DeltaX = R * Math.Cos(Direction);
                    X = CollisionPosition.X + DeltaX;
                    if (X >= TileLeft && X <= TileRight)
                    {
                        Position.X = (float)(X - OffsetX);
                        Position.Y = (float)(TileTop - OffsetY);
                        return;
                    }
                }
            }
            else if (!Left && Down)
            {
                if (Math.Cos(Direction) != 0)
                {
                    R = (TileRight - CollisionPosition.X) / Math.Cos(Direction);
                    DeltaY = R * Math.Sin(Direction);
                    Y = CollisionPosition.Y + DeltaY;
                    if (Y >= TileTop && Y <= TileBottom)
                    {
                        Position.X = (float)(TileRight - OffsetX);
                        Position.Y = (float)(Y - OffsetY);
                        return;
                    }
                }

                if (Math.Sin(Direction) != 0)
                {
                    R = (TileTop - CollisionPosition.Y) / Math.Sin(Direction);
                    DeltaX = R * Math.Cos(Direction);
                    X = CollisionPosition.X - DeltaX;
                    if (X >= TileLeft && X <= TileRight)
                    {
                        Position.X = (float)(X - OffsetX);
                        Position.Y = (float)(TileTop - OffsetY);
                        return;
                    }
                }
            }
            else if (Left && !Down)
            {
                if (Math.Cos(Direction) != 0)
                {
                    R = (TileLeft - CollisionPosition.X) / Math.Cos(Direction);
                    DeltaY = R * Math.Sin(Direction);
                    Y = CollisionPosition.Y + DeltaY;
                    if (Y >= TileTop && Y <= TileBottom)
                    {
                        Position.X = (float)(TileLeft - OffsetX);
                        Position.Y = (float)(Y - OffsetY);
                        return;
                    }
                }

                if (Math.Sin(Direction) != 0)
                {
                    R = (TileBottom - CollisionPosition.Y) / Math.Sin(Direction);
                    DeltaX = R * Math.Cos(Direction);
                    X = CollisionPosition.X + DeltaX;
                    if (X >= TileLeft && X <= TileRight)
                    {
                        Position.X = (float)(X - OffsetX);
                        Position.Y = (float)(TileBottom + 1 - OffsetY);
                        return;
                    }
                }
            }
            else if (!Left && !Down)
            {
                if (Math.Cos(Direction) != 0)
                {
                    R = (TileRight - CollisionPosition.X) / Math.Cos(Direction);
                    DeltaY = R * Math.Sin(Direction);
                    Y = CollisionPosition.Y + DeltaY;
                    if (Y >= TileTop && Y <= TileBottom)
                    {
                        Position.X = (float)(TileRight - OffsetX);
                        Position.Y = (float)(Y - OffsetY);
                        return;
                    }
                }

                if (Math.Sin(Direction) != 0)
                {
                    R = (TileBottom - CollisionPosition.Y) / Math.Sin(Direction);
                    DeltaX = R * Math.Cos(Direction);
                    X = CollisionPosition.X - DeltaX;
                    if (X >= TileLeft && X <= TileRight)
                    {
                        Position.X = (float)(X - OffsetX);
                        Position.Y = (float)(TileBottom + 1 - OffsetY);
                        return;
                    }
                }
            }
        }  

        private bool IsOnGround()
        {
            OnGround = true;
            return true;
        }

        private bool IsOnWall()
        {
            return false;
        }

        public void Update(GameTime GameTime, KeyboardState Keys)
        {
            // update player state
            IsOnGround();
            IsOnWall();
            Input(GameTime, Keys);
            Move(GameTime);
            UpdateSprite();

            // get current frame
            TotalFrames = SpriteSheet[State].Count;
            CurrentFrame += 12 * GameTime.ElapsedGameTime.TotalSeconds;
            if (State.Split("_")[1] == "jump" && CurrentFrame == TotalFrames)
            {
                CurrentFrame -= 1;
            }
            CurrentFrame %= TotalFrames;

            // get current spritesheet
            Texture = SpriteSheet[State][(int)CurrentFrame];
            UpdateSprite();

        }
    }
}
