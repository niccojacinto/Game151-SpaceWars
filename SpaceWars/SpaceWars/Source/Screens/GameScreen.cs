using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars
{
    public class GameScreen : Screen
    {


        private enum ScreenState {NORMAL, FADE_IN, COUNTDOWN, GAMEOVER}
        //Dictionary<SoundEffect, string> gameSounds;

        Game1 _main;
        // State (the internal status of the screen)
        bool _active = true;

        // Data (assets needed for certain tasks)
        Texture2D texCommandCenter, texGeminiMissile, texAsteroid;

        // Entities (anything on the screen)
        GameObject background;
        CommandCenter player1, player2;
        public static List<Asteroid> asteroids;

        // Settings
        private const uint NUM_ASTEROIDS = 20;

        public GameScreen(Game1 main) : base (main)
        {
            background = new GameObject(
                content.Load<Texture2D>("Sprites/space"),
                Vector2.Zero,
                2.5f,
                0.0f,
                false,
                SpriteEffects.None);
            _main = main;
        }//public Screen ()

        public override void Initialize(){
            asteroids = new List<Asteroid>();
            player1 = new CommandCenter(graphics, texCommandCenter, texGeminiMissile, new Vector2(100, 100));
            player2 = new CommandCenter(graphics, texCommandCenter, texGeminiMissile, new Vector2(1000, 200));

            Random random = new Random();
            for (int i = 0; i < NUM_ASTEROIDS; i++)
            {
                float x = random.Next(50, 1000);
                float y = random.Next(50, 600);
                float speed = random.Next(0, 100);
                float rot = random.Next ( 0, 360 );
                Asteroid tmpAsteroid = new Asteroid(texAsteroid, new Vector2(x, y), rot, speed);
                asteroids.Add(tmpAsteroid);
            }
        }//public override void Initialize(){

        public override void LoadContent(){
            texCommandCenter = content.Load<Texture2D>("Sprites/command_center");
            texGeminiMissile = content.Load<Texture2D>("Sprites/missile");
            texAsteroid = content.Load<Texture2D>("Sprites/asteroid");
            //sfxLaunch = content.Load<SoundEffect> ( "Audio/launch" );

        }//public override void LoadContent()

        public override void Update(GameTime gameTime)
        {}

        public override void Update(GameTime gameTime, KeyboardState keyState) {
            // Player Updates
            player1.Update(gameTime);
            player2.Update(gameTime);
            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.Update(gameTime, graphics);
            }

            UpdateInput(keyState);
        }//public override void Update(GameTime gameTime, KeyboardState keyState) {

        public override void UpdateInput(KeyboardState keyState)
        {
            handlePlayerInput(player1, keyState, Keys.A, Keys.D, Keys.W);
            handlePlayerInput(player2, keyState, Keys.NumPad4, Keys.NumPad6, Keys.NumPad8);
        }//public override void UpdateInput(KeyboardState keyState)

        private void handlePlayerInput (CommandCenter player, KeyboardState keyState
                , Keys left, Keys right, Keys primary)
        {
            if (player._currentActive == null)
            {
                if (keyState.IsKeyDown(left)) {
                    player.AimLeft();}
                else if (keyState.IsKeyDown(right)) {
                    player.AimRight();}

                if (keyState.IsKeyDown(primary)) {
                    player.Launch();}
            } else {
                if (keyState.IsKeyDown(left)) {
                    player._currentActive.TurnLeft();}
                else if (keyState.IsKeyDown(right)) {
                    player._currentActive.TurnRight();}

                if (keyState.IsKeyDown(primary)) {
                    player._currentActive.ActivateSpecial();}
            }//else
        }// private void function handlePlayerInput (CommandCenter player, KeyState keyState


        public override void Draw()
        {
            if (_active){
                background.Draw(spriteBatch);
                player1.Draw(spriteBatch);
                player2.Draw(spriteBatch);
                for (int i = 0; i < NUM_ASTEROIDS; i++)
                {
                    asteroids[i].Draw(spriteBatch);
                }
            }// if (_active){
        }//public override void Draw()
    }//public class GameScreen : Screen
}//namespace SpaceWars
