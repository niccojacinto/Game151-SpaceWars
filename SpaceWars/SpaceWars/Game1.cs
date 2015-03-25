#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SpaceWars {

    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Screen screenGame, screenVictory, screenMainMenu;
        Screen activeScreen;

        public Game1 ()
            : base () {
            graphics = new GraphicsDeviceManager ( this );
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1080;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 640;   // set this value to the desired height of your window
            graphics.ApplyChanges ();
        }


        protected override void Initialize () {
            // TODO: Add your initialization logic here

            base.Initialize ();
        }


        protected override void LoadContent () {

            spriteBatch = new SpriteBatch ( GraphicsDevice );

            // TODO: use this.Content to load your game content here
            screenGame = new Screen ( Content, GraphicsDevice );
            //screenVictory = new Screen ( );
            //screenMainMenu = new Screen ( );
            activeScreen = screenGame;
           
            
        }

        protected override void UnloadContent () {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update ( GameTime gameTime ) {
            if ( GamePad.GetState ( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown ( Keys.Escape ) )
                Exit ();

            // TODO: Add your update logic here
            activeScreen.Update (gameTime, GraphicsDevice);
            activeScreen.UpdateInput ( Keyboard.GetState() );
            base.Update ( gameTime );
        }



        protected override void Draw ( GameTime gameTime ) {
            GraphicsDevice.Clear ( Color.CornflowerBlue );

            // TODO: Add your drawing code here
            spriteBatch.Begin ();
            activeScreen.Draw (spriteBatch);
            spriteBatch.End ();

            base.Draw ( gameTime );
        }
    }
}
