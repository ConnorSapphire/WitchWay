using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace WitchWay
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteGroup allSprites;
        private SpriteGroup collidable;

        private Player player;
        private Dictionary<string, List<Texture2D>> playerSpriteSheet;
        private Tilemap tilemap;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Settings.WIDTH = _graphics.PreferredBackBufferWidth;
            Settings.HEIGHT = _graphics.PreferredBackBufferHeight;
            allSprites = new SpriteGroup();
            collidable = new SpriteGroup();

            base.Initialize();
            TilemapObject playerObject = tilemap.GetObjectGroup(Settings.layers["positions"]).GetNamedObjects(Settings.objects["player"])[0];
            Vector2 playerPosition = new Vector2(playerObject.x, playerObject.y);
            player = new Player(playerPosition, playerSpriteSheet, new Sprite(), collidable);

            allSprites.Add(player.sprite);
            foreach (Sprite tile in tilemap.GetLayer(Settings.layers["ground"]))
            {
                allSprites.Add(tile);
                collidable.Add(tile);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSpriteSheet = Support.GetSpritesFromChildFolders(Content, "Content\\images\\player");

            tilemap = Support.GetTilemap(Content, Directory.GetCurrentDirectory() + "\\Content\\data\\level.tmx");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }

            Settings.WIDTH = _graphics.PreferredBackBufferWidth;
            Settings.HEIGHT = _graphics.PreferredBackBufferHeight;
            player.Update(gameTime, Keyboard.GetState());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            allSprites.Draw(_spriteBatch, player);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
