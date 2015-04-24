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
        CommandCenter _player;
        private int hp;
        public float radius;

        public CrusaderShield (GameScreen screen, CommandCenter player, Texture2D texture, Vector2 position)
            :base(texture, position, 0.2f, 0.0f, true, SpriteEffects.None)
        {
            this._screen = screen;
            _position = position;
            radius = ( _texture.Width * Scale ) / 2;
            hp = 100;
            _player = player;

        }

        public void Update ( GameTime gameTime ) {
            if ( !isAlive ) 
                _player.shields.Remove ( this );
            
            //float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            boxCollider = new Rectangle (
              (int)_position.X - (int)((_texture.Width * Scale) / 2),
              (int)_position.Y - (int)((_texture.Height * Scale) / 2),
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            _rotation += 0.01f;

            foreach ( Asteroid collider in GameScreen.asteroids ) {
                resolveCollision ( collider );
                collider.resolveCollision ( this );
            }
            resolveCollision (GameScreen.player1._currentActive);
            resolveCollision(GameScreen.player2._currentActive);

        }

        public override void Draw (SpriteBatch spriteBatch) {
            //base.Draw (spriteBatch);
            if ( !isAlive )
                return;

            spriteBatch.Draw ( _texture,
                _position,
                null,                  // Rectangle <nullable>
                new Color(200, 200, 255, 255),
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

        public void resolveCollision( Missile collider ) {
            if (collider == null)
                return;

            if (boxCollider.Intersects(collider.boxCollider) && _player != collider.Player) {
                collider.isAlive = false;
                collider.Player._currentActive = null;
                Hit();
            }
        }

        public void resolveCollision ( Asteroid collider ) {
            if ( !collider.isAlive )
                return;

            float distance = ( _position - collider.Position ).Length ();

            if ( !( distance < radius + collider.radius ) )
                return;

            Hit ();
        }

        public void Hit () {
            hp -= 10;
            _screen.playSFX ( "explode" );
            if ( hp <= 0 ) {
                isAlive = false;
            }
        }

    }
}
