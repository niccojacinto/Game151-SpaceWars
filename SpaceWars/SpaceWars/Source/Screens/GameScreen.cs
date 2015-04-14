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
        ScreenState currentState;
        Texture2D blackTex;
        int blackTexAlpha;
        float totalElapsed;
        string currentCount;
        SpriteFont fontCountdown;
        float countDownScale;
        float timer;
        float timerDelay = 1;
        float spawnTimer;
        SoundEffect sfxCountdown, sfxReady;
        Random random;


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
        private const uint NUM_ASTEROIDS = 5;
        public static uint currentNumAsteroids;

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

            currentState = ScreenState.FADE_IN;
            blackTexAlpha = 255;
            currentCount = "3";
            countDownScale = 10.0f;
            timer = 3;
  
        }//public Screen ()

        public override void Initialize(){
            asteroids = new List<Asteroid>();
            player1 = new CommandCenter(graphics, texCommandCenter, texGeminiMissile, new Vector2(100, 100));
            player2 = new CommandCenter(graphics, texCommandCenter, texGeminiMissile, new Vector2(1000, 200));

            currentNumAsteroids = 0;
            spawnTimer = 0.0f;

            random = new Random();

            for (int i = 0; i < NUM_ASTEROIDS; i++)
            {
                Asteroid tmpAsteroid = new Asteroid(texAsteroid, Vector2.Zero);
                asteroids.Add(tmpAsteroid);
            }
            /*for (int i = 0; i < NUM_ASTEROIDS; i++)
            {
                float x = random.Next(50, 1000);
                float y = random.Next(50, 600);
                float speed = random.Next(0, 100);
                float rot = random.Next(0, 360);
                Asteroid tmpAsteroid = new Asteroid(texAsteroid, new Vector2(x, y), rot, speed);
                asteroids.Add(tmpAsteroid);
            }*/
        }//public override void Initialize(){

        public override void LoadContent(){
            texCommandCenter = content.Load<Texture2D>("Sprites/command_center");
            texGeminiMissile = content.Load<Texture2D>("Sprites/missile");
            texAsteroid = content.Load<Texture2D>("Sprites/asteroid");
            blackTex = content.Load<Texture2D> ( "Sprites/black" );
            fontCountdown = content.Load<SpriteFont> ( "Fonts/Times" );
            sfxCountdown = content.Load<SoundEffect>("Audio/countdownvoice");
            sfxReady = content.Load<SoundEffect> ( "Audio/areyouready" );


            
            //sfxLaunch = content.Load<SoundEffect> ( "Audio/launch" );

        }//public override void LoadContent()

        public override void Update(GameTime gameTime)
        {}

        public override void Update(GameTime gameTime, KeyboardState keyState) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            totalElapsed += elapsed;

            switch ( currentState ) {
                case ScreenState.NORMAL:
                    // Player Updates
                    player1.Update ( gameTime );
                    player2.Update ( gameTime );
                    foreach ( Asteroid asteroid in asteroids ) {
                        asteroid.Update ( gameTime, graphics );
                    }
                    UpdateInput ( keyState );
                    SpawnAsteroids(elapsed);
                    break;
                case ScreenState.COUNTDOWN:
                    timer -= elapsed;
                    timerDelay -= elapsed;

                    if ( timer <= -1  ) {
                        currentState = ScreenState.NORMAL;
                    }
                    else if ( timer <= 0  ) {
                        currentCount = "  Match Start"; // Sweep KING!!!
                    }
                    else if ( timer <= 1 ) {
                        currentCount = "1";
                    }
                    else if ( timer <= 2 ) {
                        currentCount = "2";
                    }
                    else if ( timer >= 3 ) {
                        currentCount = "3";
                    }

                    if ( timerDelay <= 0 ) {
                        countDownScale = 10;
                        timerDelay = 1.0f;
                    }

                    countDownScale = ( 30 * timerDelay );
                    if ( countDownScale < 10 )
                        countDownScale = 10;
                    
                    break;
                case ScreenState.FADE_IN:
                    blackTexAlpha-= 2;
                    if ( blackTexAlpha <= 0 ) {
                        totalElapsed = 0;
                        currentState = ScreenState.COUNTDOWN;
                        sfxCountdown.Play ();
                        sfxReady.Play ();
    
                    }
                    break;
                default:
                    break;
            }


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

        public void SpawnAsteroids(float elapsed)
        {
            spawnTimer -= elapsed;
            if (currentNumAsteroids < NUM_ASTEROIDS)
            {
                if (spawnTimer <= 0)
                {


                    Vector2 spawnPoint = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height * 1.25f);
                    //Vector2 spawnPoint = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height /2);
                    float speed = random.Next(100, 200);
                    float rot = -random.Next(150, 210);

                    for (int i = 0; i < NUM_ASTEROIDS; i++)
                    {
                        if (!asteroids[i].isAlive)
                        {
                            currentNumAsteroids++;
                            spawnTimer = 0.5f;
                            asteroids[i].setProperty(spawnPoint, rot, speed);
                            break;
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            if (_active){
                background.Draw(spriteBatch);
                player1.Draw(spriteBatch);
                player2.Draw(spriteBatch);
                for (int i = 0; i < currentNumAsteroids; i++)
                {
                    asteroids[i].Draw(spriteBatch);
                }
                if ( currentState == ScreenState.FADE_IN )
                    spriteBatch.Draw ( blackTex,
                        new Rectangle ( 0, 0, graphics.Viewport.Width, graphics.Viewport.Height ),
                        new Color ( 0, 0, 0, blackTexAlpha ) );

                // Countdown TODO: Clean Up, separate function maybe
                Vector2 stringSize = fontCountdown.MeasureString ( currentCount );
                Vector2 tmpVect = new Vector2 ( (graphics.Viewport.Width - stringSize.X) / 2,
                                            (graphics.Viewport.Height - stringSize.Y) / 2 );

                if ( currentState == ScreenState.COUNTDOWN ) {
                    spriteBatch.DrawString ( fontCountdown,
                        currentCount,
                        tmpVect,
                        Color.Red,
                        0.0f,
                        new Vector2 (stringSize.X / 2, stringSize.Y / 2),
                        countDownScale,
                        SpriteEffects.None,
                        0 );
                }
            }// if (_active){
        }//public override void Draw()


    }//public class GameScreen : Screen
}//namespace SpaceWars
