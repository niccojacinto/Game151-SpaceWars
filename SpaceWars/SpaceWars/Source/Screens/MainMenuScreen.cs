using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class MainMenuScreen : Screen {

        GameObject background;
        Texture2D blackTex;
        int blackTexAlpha;
        Game1 _main;
        SpriteFont title, enter;
        float totalElapsed;
        float frequency; //text flicker
        SoundEffect start, gameBgm;


        private enum ScreenState {NORMAL, FADE_IN, FADE_OUT}

        ScreenState currentState;

        public MainMenuScreen (Game1 main) : base (main) 
        {
            background = new GameObject (
                content.Load<Texture2D> ( "Sprites/mainmenu" ),
                Vector2.Zero,
                1.0f,
                0.0f,
                false,
                SpriteEffects.FlipHorizontally );
            _main = main;
            blackTexAlpha = 255;
            currentState = ScreenState.FADE_OUT;
            frequency = 2;
        }

        public override void Initialize () {
           
        }

        public override void LoadContent () {
            title = content.Load<SpriteFont> ( "Fonts/aspace" );
            enter = content.Load<SpriteFont> ( "Fonts/radioSpaceFont" );
            start = content.Load<SoundEffect> ( "Audio/start" );
            blackTex = content.Load<Texture2D> ( "Sprites/black" );
            gameBgm = content.Load<SoundEffect> ( "Audio/Music/cloakanddagger" );
        }

        public override void Update ( GameTime gameTime ) {

        }

        public override void Update ( GameTime gameTime, KeyboardState keyState ) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            totalElapsed += elapsed;

            switch ( currentState ) {
                case ScreenState.NORMAL:
                    UpdateInput ( keyState );
                    break;
                case ScreenState.FADE_IN:
                    blackTexAlpha -= 2;
                    if ( blackTexAlpha <= 0 )
                        currentState = ScreenState.NORMAL;
                    break;
                case ScreenState.FADE_OUT:
                    blackTexAlpha += 2;
                    if ( blackTexAlpha >= 255 )
                        _main.setScreen ( new GameScreen ( _main ) );
                    break;
                default:
                    break;

            }

            

        }

        public override void UpdateInput ( KeyboardState keyState ) {
            if ( keyState.IsKeyDown ( Keys.Enter ) ) {
                frequency = 10;
                _main.stopBGM ();
                //start.Play ();
                _main.setBGM ( gameBgm );
                currentState = ScreenState.FADE_OUT;
            }
        }

        public override void Draw () {
            background.Draw ( spriteBatch );
            drawGUIText ();
            if (currentState != ScreenState.NORMAL)
                spriteBatch.Draw ( blackTex,
                    new Rectangle ( 0, 0, graphics.Viewport.Width, graphics.Viewport.Height ),
                    new Color ( 0, 0, 0, blackTexAlpha ) );


        }

        private void drawGUIText () {
            // TODO: Clean this up a bit
            string output = "Space Wars";
            Vector2 stringSize = title.MeasureString ( output );
            Vector2 tmpVect = new Vector2 ( graphics.Viewport.Width / 2 - stringSize.X / 2,
                                            graphics.Viewport.Height / 4 + stringSize.Y / 2 );
            int color = (int)( 189 + 63 * Math.Sin ( totalElapsed ) );
            spriteBatch.DrawString ( title, output, tmpVect, new Color ( color, 0, 0 ) );

            output = "- Press  [ENTER]  to start a match -";
            stringSize = enter.MeasureString ( output );
            tmpVect = new Vector2 ( graphics.Viewport.Width / 2 - stringSize.X / 2,
                                            graphics.Viewport.Height / 2 + stringSize.Y / 2 );
            color = (int)( 189 + 63 * Math.Sin ( 2 * Math.PI * frequency * totalElapsed ) );
            spriteBatch.DrawString ( enter, output, tmpVect, new Color ( color, color, color ) );
        }

    }


}
