using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public abstract class Screen {
        protected ContentManager content;
        protected GraphicsDevice graphics;
        protected SpriteBatch spriteBatch;

        public Screen (Game1 main) {
            content = Program.game.Content;
            graphics = Program.game.GraphicsDevice;
            spriteBatch = Program.game.spriteBatch;

            LoadContent();
            Initialize();
        }//public Screen ()

        abstract public void Initialize();
        abstract public void LoadContent();
        abstract public void Update(GameTime gameTime);
        abstract public void Update(GameTime gameTime, KeyboardState keyState);
        abstract public void UpdateInput(KeyboardState keyState);
        abstract public void Draw();
    }// public abstract class Screen {
}//namespace SpaceWars {
