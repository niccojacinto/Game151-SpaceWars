using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class CrusaderShield : GameObject {

        //private static GraphicsDevice _Device;
        GameScreen _screen;
        private int hp;
        public float radius;

        public CrusaderShield (GameScreen screen, CommandCenter player, Texture2D texture, Vector2 position)
            :base(texture, position, 0.5f, 0.0f, true, SpriteEffects.None)
        {
            this._screen = screen;
            _position = position;
            radius = ( _texture.Width * Scale ) / 2;
            hp = 100;

        }

        public void Update ( GameTime gameTime ) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            boxCollider = new Rectangle (
              (int)_position.X,
              (int)_position.Y,
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            _rotation += 0.01f;

        }

        public override void Draw (SpriteBatch spriteBatch) {
            //base.Draw (spriteBatch);

            spriteBatch.Draw ( _texture,
                _position,
                null,                  // Rectangle <nullable>
                new Color(75, 75, 230, 200),
                _rotation,
                _origin,
                Scale,
                _spriteEffect,
                0 );   

            Color color = Color.GreenYellow;
            if ( hp < 20 ) 
                color = Color.Red;
            else if (hp < 60) 
                color = Color.Gold;

            string sHP = hp + "%";
            Vector2 stringSize = GameScreen.fontUI.MeasureString(sHP);
            spriteBatch.DrawString ( GameScreen.fontUI, sHP,
                new Vector2 ( ( _position.X - stringSize.X / 3 ), ( _position.Y - stringSize.Y - 20) ),
                color);

        }

        public void Hit () {
            hp -= 2;
            //_screen.playSFX ( "explode" );
        }

    }
}
