using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class Weapon : GameObject{

        private Vector2 _velocity;
        private float _speedMultiplier;

        public CommandCenter Player { get; set; }

        public Weapon (Texture2D texture, Vector2 position, float rotation, CommandCenter player)
            :base(texture, position, 0.02f, rotation, true, SpriteEffects.None)
        {
            _position = position;
            _rotation = rotation + (float)( 90 * ( Math.PI / 180 ) ); 
                    // Adjustment of the sprite image because the image is oriented in the wrong direction
            _speedMultiplier = 2.0f;
            Player = player;


        }

        public virtual void TurnLeft () {
            _rotation -= 0.04f;
        }

        public virtual void TurnRight () {
            _rotation += 0.04f;
        }

        public virtual void Update ( GameTime gameTime ) {
            boxCollider = new Rectangle (
              (int)_position.X,
              (int)_position.Y,
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );

            float vX = (float)Math.Cos ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            float vY = (float)Math.Sin ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            _velocity = new Vector2 ( vX, vY ) * _speedMultiplier;
            _position += _velocity;

            foreach ( Asteroid collider in GameScreen.asteroids ) {
                resolveCollision ( collider );
            }

            // if missile leaves the game screen, it is no longer the active missile, and is no longer drawn
            if (!Game1.viewportRect.Contains(new Point(
                        (int)_position.X,
                        (int)_position.Y)))
            {
                isAlive = false;
                Player._currentActive = null;
            }
        }

        public virtual void ActivateSpecial () {
            _speedMultiplier = 7.0f;
        }

        public void resolveCollision (Asteroid collider) {
            if ( boxCollider.Intersects ( collider.boxCollider ) && collider.isAlive) {
                Player._currentActive = null;
                isAlive = false;
                collider.resolveCollision ( this );
            }
        }




    }
}
