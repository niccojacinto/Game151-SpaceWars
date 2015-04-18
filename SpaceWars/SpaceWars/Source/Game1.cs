#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace SpaceWars {
    public class Game1 : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        Screen activeScreen;
        SoundEffectInstance bgm;
        public static Rectangle viewportRect;

        public Game1 ()
            : base () {
            graphics = new GraphicsDeviceManager ( this );
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1080;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 640;   // set this value to the desired height of your window
            graphics.ApplyChanges ();
        }// public Game1 ()

        protected override void Initialize () {
            // TODO: Add your initialization logic here

            //drawable area of the game screen.
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            base.Initialize ();
            #if DEBUG
                System.Console.WriteLine("Testing debug statement");
            #endif
        } //protected override void Initialize () {

        protected override void LoadContent () {
            
            spriteBatch = new SpriteBatch ( GraphicsDevice );
            activeScreen = new MainMenuScreen(this);
            SoundEffect music = Content.Load<SoundEffect> ( "Audio/Music/lonestode" );
            setBGM ( music );

            // TODO: use this.Content to load your game content here
        }//protected override void LoadContent () {

        protected override void UnloadContent () {
            // TODO: Unload any non ContentManager content here
        }//protected override void UnloadContent () {

        protected override void Update ( GameTime gameTime ) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                { Exit(); }
            activeScreen.Update (gameTime, Keyboard.GetState() );
            base.Update ( gameTime );
        }// protected override void Update ( GameTime gameTime ) {

        protected override void Draw ( GameTime gameTime ) {
            //Clear screen
            GraphicsDevice.Clear ( Color.CornflowerBlue );

            //Draw stuff all over screen
            spriteBatch.Begin ();
            activeScreen.Draw ();
            spriteBatch.End ();
            base.Draw ( gameTime );
        }//protected override void Draw ( GameTime gameTime ) {

        /* Sets the active screen of the game */
        public void setScreen(Screen screen){
            //TODO destroy/garbage collect active screen..?
            activeScreen = screen;
        }//public void setScreen(Screen screen)

        public void setBGM ( SoundEffect music ) {
            bgm = music.CreateInstance ();
            bgm.IsLooped = true;
            bgm.Play ();
        }

        public void stopBGM () {
            bgm.Stop ();
        }
    }
}
