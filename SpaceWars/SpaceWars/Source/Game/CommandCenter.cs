using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class CommandCenter : GameObject {

        Texture2D line;
        static GraphicsDevice _Device;
        private float _launchAngle;
        public Weapon _currentActive; // Missile currently launched
        public Weapon _currentWeapon; // Weapon currently selected
        private int hp;
        Texture2D texGeminiMissile;
        SoundEffect launch;

        public CommandCenter (GraphicsDevice Device, Texture2D texture, Texture2D weapon, Vector2 position)
            :base(texture, position, 0.1f, 0.0f, true, SpriteEffects.None)
        {
            _position = position;
            _launchAngle = 0.0f;
            _Device = Device;
            texGeminiMissile = weapon;
            hp = 100;

            line = new Texture2D ( _Device, 1, 1 );
            line.SetData<Color> (
                new Color[] { Color.White } );// fill the texture with White

        }

        public void Update ( GameTime gameTime ) {
            boxCollider = new Rectangle (
              (int)_position.X,
              (int)_position.Y,
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            _rotation += 0.01f;
            if (_currentActive != null) {
                _currentActive.Update ( gameTime );
            }
        }

        void DrawLine ( SpriteBatch sb, Vector2 start ) {
            
            sb.Draw ( line,
                new Rectangle (// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    30, //sb will strech the texture to fill this rectangle
                    3 ), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                _launchAngle,    
                new Vector2 ( 0, 0 ), // point in line about which to rotate
                SpriteEffects.None,
                0 );

        }


        public override void Draw (SpriteBatch spriteBatch) {
            base.Draw (spriteBatch);
            if (_currentActive != null)
                _currentActive.Draw ( spriteBatch );
            DrawLine ( spriteBatch,  _position );

            Color color = Color.GreenYellow;
            if ( hp < 20 ) 
                color = Color.Red;
            else if (hp < 60) 
                color = Color.Gold;

            string sHP = hp + "%";
            Vector2 stringSize = GameScreen.fontUI.MeasureString(sHP);
            spriteBatch.DrawString ( GameScreen.fontUI, 
                sHP,
                new Vector2 ( ( _position.X - stringSize.X / 3 ), ( _position.Y - stringSize.Y - 20) ),
                color);

        }


        public void AimLeft () {
            _launchAngle-=0.1f;
        }

        public void AimRight () {
            _launchAngle+=0.1f;
        }

        public void Launch () {
            _currentActive = new Weapon ( texGeminiMissile, _position, _launchAngle, this );
        }

        public void Hit () {
            hp -= 7;
        }

    }
}
