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

        // TODO: Share all ScreenStates in one class
        private enum ScreenState { 
            NORMAL, 
            FADE_IN,
            FADE_OUT 
        }

        Game1 _main;

        GameObject _background;
        Texture2D _blackTex;
        int _blackTexAlpha;

        SpriteFont _title, _enter;
        SoundEffect _gameBgm;

        float _totalElapsed;
        float _enterTextFrequency; //text flicker




        private ScreenState currentState;

        public MainMenuScreen (Game1 main) : base (main) 
        {
            _background = new GameObject (
                content.Load<Texture2D> ( "Sprites/mainmenu" ),
                Vector2.Zero,
                1.0f,
                0.0f,
                false,
                SpriteEffects.FlipHorizontally );
            _main = main;
            _blackTexAlpha = 255;
            currentState = ScreenState.FADE_IN;
            _enterTextFrequency = 2;
        }

        public override void Initialize () {
           
        }

        public override void LoadContent () {
            _title = content.Load<SpriteFont> ( "Fonts/aspace" );
            _enter = content.Load<SpriteFont> ( "Fonts/radioSpaceFont" );
            _blackTex = content.Load<Texture2D> ( "Sprites/black" );
            _gameBgm = content.Load<SoundEffect> ( "Audio/Music/cloakanddagger" );
        }

        public override void Update ( GameTime gameTime ) {

        }

        public override void Update ( GameTime gameTime, KeyboardState keyState ) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            _totalElapsed += elapsed;

            switch ( currentState ) {
                case ScreenState.NORMAL:
                    UpdateInput ( keyState );
                    break;
                case ScreenState.FADE_IN:
                    _blackTexAlpha -= 2;
                    if ( _blackTexAlpha <= 0 )
                        currentState = ScreenState.NORMAL;
                    break;
                case ScreenState.FADE_OUT:
                    _blackTexAlpha += 2;
                    if ( _blackTexAlpha >= 255 )
                        _main.setScreen ( new GameScreen ( _main ) );
                    break;
                default:
                    break;
            }
        }

        public override void UpdateInput ( KeyboardState keyState ) {
            if ( keyState.IsKeyDown ( Keys.Enter ) ) {
                _enterTextFrequency = 10;
                _main.stopBGM ();
                _main.setBGM ( _gameBgm );
                currentState = ScreenState.FADE_OUT;
            }
        }

        public override void Draw () {
            _background.Draw ( spriteBatch );
            drawGUIText ();
            if (currentState != ScreenState.NORMAL)
                spriteBatch.Draw ( _blackTex,
                    new Rectangle ( 0, 0, graphics.Viewport.Width, graphics.Viewport.Height ),
                    new Color ( 0, 0, 0, _blackTexAlpha ) );


        }

        private void drawGUIText () {
            // TODO: Clean this up a bit
            string output = "Space Wars";
            Vector2 stringSize = _title.MeasureString ( output );
            Vector2 tmpVect = new Vector2 ( graphics.Viewport.Width / 2 - stringSize.X / 2,
                                            graphics.Viewport.Height / 4 + stringSize.Y / 2 );
            int color = (int)( 189 + 63 * Math.Sin ( _totalElapsed ) );
            spriteBatch.DrawString ( _title, output, tmpVect, new Color ( color, 0, 0 ) );

            output = "- Press  [ENTER]  to start a match -";
            stringSize = _enter.MeasureString ( output );
            tmpVect = new Vector2 ( graphics.Viewport.Width / 2 - stringSize.X / 2,
                                            graphics.Viewport.Height / 2 + stringSize.Y / 2 );
            color = (int)( 189 + 63 * Math.Sin ( 2 * Math.PI * _enterTextFrequency * _totalElapsed ) );
            spriteBatch.DrawString ( _enter, output, tmpVect, new Color ( color, color, color ) );
        }

    }


}
