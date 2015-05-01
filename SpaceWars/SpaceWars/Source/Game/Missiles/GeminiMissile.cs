using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class GeminiMissile : Missile{

        //private Vector2 _velocity;
       // private float _speedMultiplier;
        //float specialTimerDelay;

        // public CommandCenter Player { get; set; }

        public GeminiMissile (CommandCenter player, Texture2D texture, Vector2 position, float scale, float rotation, SpriteEffects spriteEffects)
            :base(player, texture, position, scale, rotation, SpriteEffects.None)
        {
            specialTimerDelay = 0.2f;
            _position = position;
            _rotation = rotation + (float)( 90 * ( Math.PI / 180 ) ); 
                    // Adjustment of the sprite image because the image is oriented in the wrong direction
            speedMultiplier = 2.0f;
            Player = player;
        }

        public override void TurnLeft () {
            _rotation -= 0.04f;
        }

        public override void TurnRight () {
            _rotation += 0.04f;
        }

        public override void Update ( GameTime gameTime ) {
            boxCollider = new Rectangle (
              (int)_position.X - (int)((_texture.Width * Scale * 0.75) / 2),
              (int)_position.Y - (int)((_texture.Height * Scale * 0.75) / 2),
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );

            float elapsed = ((float)gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
            specialTimerDelay -= elapsed;

            float vX = (float)Math.Cos ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            float vY = (float)Math.Sin ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            velocity = new Vector2 ( vX, vY ) * speedMultiplier;
            _position += velocity;

            foreach ( Asteroid collider in GameScreen.asteroids ) {
                resolveCollision ( collider );
            }
            // CommandCenter collision check
            resolveCollision ( GameScreen.player1 );
            resolveCollision ( GameScreen.player2 );
            // Missile to missile collision check
            resolveCollision(GameScreen.player1._currentActive);
            resolveCollision(GameScreen.player2._currentActive);

            // if missile leaves the game screen, it is no longer the active missile, and is no longer drawn
            if (!Game1.viewportRect.Contains(new Point(
                        (int)_position.X,
                        (int)_position.Y)))
            {
                isAlive = false;
                Player._currentActive = null;
            }
        }

        public override void ActivateSpecial () {
            if (specialTimerDelay < 0) {
                speedMultiplier = 5.5f;
            }
        }

        public void resolveCollision(CrusaderShield collider) {
            if (boxCollider.Intersects(collider.boxCollider) && collider.isAlive) {
                collider.Hit();
                Player._currentActive = null;
                isAlive = false;
            }
        }

        public override void resolveCollision (Asteroid collider) {
            if ( boxCollider.Intersects ( collider.boxCollider ) && collider.isAlive) {
                Player._currentActive = null;
                isAlive = false;
                collider.resolveCollision ( this );
                GameScreen.particleSystem.AddExplosion ( _position );
            }
        }

        public void resolveCollision(Missile collider)
        {
            if (collider == this || collider == null)
                return;

            if (boxCollider.Intersects(collider.boxCollider))
            {

                Player._currentActive = null;
                collider.Player._currentActive = null;
                isAlive = false;
                collider.isAlive = false;
                GameScreen.gameSFXs["explode02"].Play ();
                GameScreen.particleSystem.AddExplosion ( _position );
            }
        }

        public override void resolveCollision ( CommandCenter collider ) {
            if ( boxCollider.Intersects ( collider.boxCollider ) ) {
                if ( Player != collider ) {
                    collider.Hit (7);
                    Player._currentActive = null;
                    isAlive = false;
                    GameScreen.particleSystem.AddExplosion ( _position );
                }
            }
        }




    }
}
