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

        // TODO: Share all ScreenStates in one class
        private enum ScreenState { NORMAL, FADE_IN, COUNTDOWN, GAMEOVER }
        private ScreenState currentState;

        public static SpriteFont fontUI;
        private SpriteFont fontCountdown;

        private Texture2D blackTex, iconMissileGemini, iconMissilePORT, iconMissileCrusader;
        private int blackTexAlpha;
        private float totalElapsed;
        private string currentCount;
        private string winner;


        private float countDownScale;
        private float timer;
        private float timerDelay = 1;
        private float spawnTimer;
        private float spawnTimerPowerUp;
        private SoundEffect sfxCountdown, sfxReady;
        private Random random;
        private KeyboardState prevState;

        private Dictionary<string, SoundEffect> gameSFXs;


        // Data (assets needed for certain tasks)
        Texture2D texCommandCenter, texGeminiMissile, texAsteroid, texCrusaderShield;

        // Entities (anything on the screen)
        GameObject background;
        public static CommandCenter player1, player2;
        public static List<Asteroid> asteroids;
        public static List<PowerUpText> powerUpText;
        public static Queue<Asteroid> deadAsteroids;

        // Settings
        private const uint NUM_ASTEROIDS = 10;
        private const uint NUM_POWERUPS = 10;
        public static int currentNumAsteroids;
        public static int currentNumPowerUps;

        public static BasicParticleSystem particleSystem;
        public static TimeSpan totalParticleTime;

        public GameScreen() : base ()
        {
            background = new GameObject(
                content.Load<Texture2D>("Sprites/space"),
                Vector2.Zero,
                2.5f,
                0.0f,
                false,
                SpriteEffects.None);

            currentState = ScreenState.FADE_IN;
            blackTexAlpha = 255;
            currentCount = "3";
            countDownScale = 10.0f;
            timer = 3;
            prevState = Keyboard.GetState ();
  
        }//public Screen ()

        public override void Initialize(){
            asteroids = new List<Asteroid>();
            powerUpText = new List<PowerUpText>();
            deadAsteroids = new Queue<Asteroid> ();
            player1 = new CommandCenter(this, texCommandCenter, texCrusaderShield, texGeminiMissile, new Vector2(100, 500));
            player2 = new CommandCenter(this, texCommandCenter, texCrusaderShield, texGeminiMissile, new Vector2(1000, 200));

            currentNumAsteroids = 0;
            currentNumPowerUps = 0;
            spawnTimer = 0.0f;
            spawnTimerPowerUp = 5.0f;
            winner = " Wins!";

            random = new Random();

            for (int i = 0; i < NUM_ASTEROIDS; i++)
            {
                Asteroid tmpAsteroid = new Asteroid(texAsteroid, Vector2.Zero);
                asteroids.Add(tmpAsteroid);
                deadAsteroids.Enqueue ( tmpAsteroid );
            }

            for (int i = 0; i < NUM_POWERUPS; i++)
            {
                int randPowerUp = random.Next(0, 2);
                Asteroid tmpPowerUp;
                switch (randPowerUp)
                {
                    case 0 :
                        tmpPowerUp = new AhhSteroidCrusader(texAsteroid, Vector2.Zero);
                        asteroids.Add(tmpPowerUp);
                        deadAsteroids.Enqueue(tmpPowerUp);
                        break;
                    case 1 :
                        tmpPowerUp = new AhhSteroidPORT(texAsteroid, Vector2.Zero);
                        asteroids.Add(tmpPowerUp);
                        deadAsteroids.Enqueue(tmpPowerUp);
                        break;
                    default :
                        break;
                }
                
            }

        }//public override void Initialize(){

        public override void LoadContent(){
            // Textures
            texCommandCenter = content.Load<Texture2D>("Sprites/command_center");
            texGeminiMissile = content.Load<Texture2D>("Sprites/missile");
            texAsteroid = content.Load<Texture2D>("Sprites/asteroid");
            texCrusaderShield = content.Load<Texture2D> ( "Sprites/crusadershield" );
            blackTex = content.Load<Texture2D> ( "Sprites/black" );

            // Fonts
            fontCountdown = content.Load<SpriteFont> ( "Fonts/Times" );
            fontUI = content.Load<SpriteFont> ( "Fonts/agencyFBUI" );
            // Sound Effects
            sfxCountdown = content.Load<SoundEffect>("Audio/countdownvoice");
            //sfxReady = content.Load<SoundEffect> ( "Audio/areyouready" );

            // Initialize gameSFXs dictionary
            gameSFXs = new Dictionary<string, SoundEffect> ();
            gameSFXs.Add ( "launch", content.Load<SoundEffect> ( "Audio/launch" ) );
            gameSFXs.Add ( "explode", content.Load<SoundEffect> ( "Audio/explosion" ) );

            // Initialize UI Textures
            iconMissileGemini = content.Load<Texture2D> ( "Sprites/UI/GeminiMissileIcon" );
            iconMissilePORT = content.Load<Texture2D> ( "Sprites/UI/PORTMissileIcon" );
            iconMissileCrusader = content.Load<Texture2D> ( "Sprites/UI/CrusaderMissileIcon" );

            particleSystem = new BasicParticleSystem ( content.Load<Texture2D> ( "Sprites/fireball" ) );
        }//public override void LoadContent()

        public override void Update(GameTime gameTime)
        {}

        public override void Update(GameTime gameTime, KeyboardState keyState) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            totalElapsed += elapsed;

            switch ( currentState ) {
                case ScreenState.NORMAL:
                    // TODO: Make a list for all GameObjects
                    //       and do all collision checks there

                    // Player Updates


                    player1.Update ( gameTime );
                    player2.Update ( gameTime );

                    //Update Particle System
                    totalParticleTime += gameTime.ElapsedGameTime;
                    particleSystem.Update ( totalParticleTime, gameTime.ElapsedGameTime );

                    // Asteroid Updates

                    foreach ( Asteroid asteroid in asteroids ) {
                        asteroid.Update ( gameTime, graphics );
                    }

                    for (int i = powerUpText.Count - 1; i >= 0; --i) 
                    {
                        if ( powerUpText[i].isAlive ) {
                            powerUpText[i].Update ( elapsed );
                        }
                        else {
                            powerUpText.RemoveAt ( i );
                        }
                    }


                    SpawnAsteroids ( elapsed );
                    UpdateInput ( keyState );

                    // Game Over
                    if (player1.hp <= 0 || player2.hp <= 0) {
                        currentState = GameScreen.ScreenState.GAMEOVER;
                        if (player1.hp <= 0) {
                            winner = "Player2" + winner;
                        } else if (player2.hp <= 0) {
                            winner = "Player1" + winner;
                        }
                    }
                    
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
                        //sfxReady.Play ();
                    }
                    break;
                case ScreenState.GAMEOVER:
                    UpdateInput ( keyState );
                    break;
                default:
                    break;
            }


        }//public override void Update(GameTime gameTime, KeyboardState keyState) {

        public override void UpdateInput(KeyboardState keyState)
        {
            KeyboardState newState = Keyboard.GetState ();
            handlePlayerInput ( player1, keyState, Keys.A, Keys.D, Keys.W, Keys.Q, Keys.E );
            handlePlayerInput( player2, keyState, Keys.NumPad4, Keys.NumPad6, Keys.NumPad8, Keys.NumPad7, Keys.NumPad9);
            prevState = newState;
        }//public override void UpdateInput(KeyboardState keyState)

        private void handlePlayerInput (CommandCenter player, KeyboardState keyState
                , Keys left, Keys right, Keys primary, Keys cycleLeft, Keys cycleRight)
        {
            bool readyToCycleLeft = !prevState.IsKeyDown (cycleLeft );
            bool readyToCycleRight = !prevState.IsKeyDown (cycleRight);
            bool readyToFire = !prevState.IsKeyDown ( primary );

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

                if (keyState.IsKeyDown(primary) && readyToFire) {
                    player._currentActive.ActivateSpecial();
                }
            }//else

            if ( keyState.IsKeyDown (cycleLeft) && readyToCycleLeft ) {
                    player.cycleWeaponsLeft ();
            }
            else if ( keyState.IsKeyDown(cycleRight) && readyToCycleRight) {
                    player.cycleWeaponsRight ();
            }

            switch (currentState) {
                case ScreenState.GAMEOVER:
                    if ( keyState.IsKeyDown ( Keys.Enter ) )
                        Program.game.setScreen ( new MainMenuScreen ( ) );
                    break;
                default:
                    break;
            }

           

        }// private void function handlePlayerInput (CommandCenter player, KeyState keyState

        public void SpawnAsteroids(float elapsed)
        {
            spawnTimer -= elapsed;
            spawnTimerPowerUp -= elapsed;

            if (currentNumAsteroids < NUM_ASTEROIDS)
            {
                if (spawnTimer <= 0)
                {
                    Vector2 spawnPoint = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height + 50);
                    float speed = random.Next(100, 100);
                    float rot = -random.Next(150, 210);
                    float mass = random.Next(1, 5);

                    Asteroid tmpAsteroid = deadAsteroids.Dequeue ();
                    if (tmpAsteroid.GetType() == typeof(AhhSteroidCrusader))
                    {       
                        if (spawnTimerPowerUp <= 0)
                        {
                            spawnTimerPowerUp = 5.0f;
                            currentNumPowerUps++;
                           /* do
                            {
                                deadAsteroids.Enqueue(tmpAsteroid);
                                tmpAsteroid = deadAsteroids.Dequeue();
                                flag = NUM_ASTEROIDS > currentNumAsteroids - currentNumPowerUps;
                            } while ((tmpAsteroid.GetType() == typeof(AhhSteroidCrusader)) && !flag); */
                        }
                        else
                        {
                            deadAsteroids.Enqueue(tmpAsteroid);
                            return;
                        }
                    }



                    tmpAsteroid.setProperty(spawnPoint, rot, speed);
                    tmpAsteroid.Mass = mass;
                    currentNumAsteroids++;
                    spawnTimer = 1f;
                }
            }
        }

        public void playSFX ( string sfxName ) {
            gameSFXs[sfxName].Play ();
        }

        public void drawPlayerUI (SpriteBatch spriteBatch) {
            int counter = 0;
            
            foreach ( CommandCenter.WeaponsList weaponType in  Enum.GetValues(typeof(CommandCenter.WeaponsList) ) ) {
                int x = 25 + 35 * ( counter % 7 );
                int y = 25 + 10 * ( counter / 7 );
                float scale = 0.3f;
                Vector2 tmpPos = new Vector2 ( x, y );
                Color color = new Color ( 255, 255, 255, 200 );
                if ( player1.currentWeapon == weaponType )
                    color = new Color ( 30, 220, 30, 100 );
                switch ( weaponType ) {
                    case CommandCenter.WeaponsList.GEMINI_MISSILE:
                        spriteBatch.Draw ( iconMissileGemini, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0 );
                        break;
                    case CommandCenter.WeaponsList.PORT_MISSILE:
                        spriteBatch.Draw ( iconMissilePORT, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0 );
                        break;
                    case CommandCenter.WeaponsList.CRUSADER_MISSILE:
                        spriteBatch.Draw ( iconMissileCrusader, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0 );
                        break;
                    default:
                        break;
                }
                counter++;
            }

            foreach (CommandCenter.WeaponsList weaponType in Enum.GetValues(typeof(CommandCenter.WeaponsList)))
            {
                int x = graphics.Viewport.Width + 25 + 35 * (counter % 7) - 275;
                int y = graphics.Viewport.Height + 25 + 10 * (counter / 7) - 120;
                float scale = 0.3f;
                Vector2 tmpPos = new Vector2(x, y);
                Color color = new Color(255, 255, 255, 200);
                if (player2.currentWeapon == weaponType)
                    color = new Color(30, 220, 30, 100);
                switch (weaponType)
                {
                    case CommandCenter.WeaponsList.GEMINI_MISSILE:
                        spriteBatch.Draw(iconMissileGemini, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                        break;
                    case CommandCenter.WeaponsList.PORT_MISSILE:
                        spriteBatch.Draw(iconMissilePORT, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                        break;
                    case CommandCenter.WeaponsList.CRUSADER_MISSILE:
                        spriteBatch.Draw(iconMissileCrusader, tmpPos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
                counter++;
            }
        }

        public override void Draw ()
        {
            background.Draw(spriteBatch);
            for (int i = 0; i < NUM_ASTEROIDS + NUM_POWERUPS; i++)
            {
                asteroids[i].Draw(spriteBatch);
            }

            foreach ( PowerUpText txt in powerUpText ) {
                txt.Draw(spriteBatch);
            }

            player1.Draw ( spriteBatch );
            player2.Draw ( spriteBatch );
            drawPlayerUI ( spriteBatch );
            if ( currentState == ScreenState.FADE_IN )
                spriteBatch.Draw ( blackTex,
                    new Rectangle ( 0, 0, graphics.Viewport.Width, graphics.Viewport.Height),
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

            // GAMEOVER state
            stringSize = fontUI.MeasureString ( winner ) * 8;
            tmpVect = new Vector2 ( (graphics.Viewport.Width/2 - stringSize.X/2),
                                        (graphics.Viewport.Height - stringSize.Y) / 2 );

            if (currentState == ScreenState.GAMEOVER)
            {
                spriteBatch.DrawString(fontUI,
                    winner,
                    tmpVect,
                    Color.Red,
                    0.0f,
                    Vector2.Zero,
                    8.0f,
                    SpriteEffects.None,
                    0);
            }

            // Weapon Label player 1
            string output = "Gemini Missile: ";
            switch (player1.currentWeapon) {
                case CommandCenter.WeaponsList.GEMINI_MISSILE:
                    output = "Gemini Missile: ";
                    break;
                case CommandCenter.WeaponsList.PORT_MISSILE:
                    output = "PORT Missile: ";
                    break;
                case CommandCenter.WeaponsList.CRUSADER_MISSILE:
                    output = "Crusader Missile: ";
                    break;
                default:
                    break;
            };

            stringSize = fontCountdown.MeasureString ( output );
            tmpVect = new Vector2 ( 25, 60 );
            output += player1.Weapons[player1.currentWeapon];

            spriteBatch.DrawString ( fontUI,
                output,
                tmpVect,
                Color.LimeGreen,
                0.0f,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0 );

            // Weapon Label player 2
            string output2 = "Gemini Missile: ";
            switch (player2.currentWeapon)
            {
                case CommandCenter.WeaponsList.GEMINI_MISSILE:
                    output2 = "Gemini Missile: ";
                    break;
                case CommandCenter.WeaponsList.PORT_MISSILE:
                    output2 = "PORT Missile: ";
                    break;
                case CommandCenter.WeaponsList.CRUSADER_MISSILE:
                    output2 = "Crusader Missile: ";
                    break;
                default:
                    break;
            };

            tmpVect = new Vector2(graphics.Viewport.Width - 145, graphics.Viewport.Height - 60);
            output2 += player2.Weapons[player2.currentWeapon];

            spriteBatch.DrawString(fontUI,
                output2,
                tmpVect,
                Color.LimeGreen,
                0.0f,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0);

            // Stasis Timer
            tmpVect = new Vector2 ( 25, 80 );
            if ( player1.stasisDelay > 0 ) {
                spriteBatch.DrawString ( fontUI,
                    "In Stasis: " + player1.stasisDelay,
                    tmpVect,
                    Color.Red,
                    0.0f,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0 );
            }

            // Draw Particles
            Console.WriteLine ( "Drawing Particle" );
            particleSystem.Draw ( spriteBatch );
        }//public override void Draw()


    }//public class GameScreen : Screen
}//namespace SpaceWars
