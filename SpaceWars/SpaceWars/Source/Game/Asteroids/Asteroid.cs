using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class Asteroid : GameObject {

        private Vector2 _velocity;
        private Vector2 _initialVelocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public Vector2 InitialVelocity {
            get { return _initialVelocity; }
            set { _initialVelocity = value; }
        }
        public Vector2 Force { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float _trueRotation; // Where the asteroid is facing, 
                                    // because _rotation is used for sprite rotation already
        public float radius;

        public Asteroid ( Texture2D texture, Vector2 position)
            : base ( texture, position, 0.1f, 0.0f, true, SpriteEffects.None ) {

            Mass = 1.0f;
            radius = (_texture.Width * Scale) / 2;
            isAlive = false;
        }

        public virtual void Update ( GameTime gameTime, GraphicsDevice Device ) {

            if (!isAlive)
                return;


            boxCollider = new Rectangle (
              (int)_position.X - (int)((_texture.Width * Scale) / 2),
              (int)_position.Y - (int)((_texture.Height * Scale) / 2),
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            // Force = 0;
            Acceleration = Force / Mass; // over Mass but mass is currently one

            // vf =  vi + at
            _velocity = _initialVelocity + ( Acceleration * elapsed );

            // s = Vi(t) + 1/2at^2
            _position += _initialVelocity * elapsed +
                0.5f * Acceleration * elapsed * elapsed;

            // Rotate sprite along axis
            _rotation += 0.020f;

            // boundary checks
            if ( Game1.viewportRect.Contains(_position) ) {
                if ( _position.X + Origin.X * Scale > Device.Viewport.Width ) {
                    // Only bounce it back if velocity is not toward screenspace
                    if (_velocity.X >= 0)
                        _velocity = new Vector2 ( _velocity.X * -1, _velocity.Y );
                }
                else if ( _position.X - Origin.X * Scale < 0 ) {
                    if (_velocity.X < 0)
                        _velocity = new Vector2 ( _velocity.X * -1, _velocity.Y );
                }

                if ( _position.Y + Origin.Y * Scale > Device.Viewport.Height ) {
                    if ( _velocity.Y >= 0 )
                        _velocity = new Vector2 ( _velocity.X, _velocity.Y * -1 );
                }
                else if ( _position.Y - Origin.Y * Scale  < 0 ) {
                    if (_velocity.Y < 0)
                        _velocity = new Vector2 ( _velocity.X, _velocity.Y * -1 );
                }
            }

            foreach ( Asteroid collider in GameScreen.asteroids ) {
                resolveCollision ( collider );
            }
            resolveCollision(GameScreen.player1);
            resolveCollision(GameScreen.player2);

            // Set initial velcity for the next timestep, which is current timestep's final velocity
            _initialVelocity = _velocity;

        }

        public void setProperty(Vector2 pos, float rot, float speed)
        {
            _position = pos;
            _trueRotation = rot;
            _trueRotation = rot * ((float)Math.PI / 180);
            _initialVelocity = new Vector2((float)Math.Sin(_trueRotation), (float)Math.Cos(_trueRotation)) * speed;
            isAlive = true;
        }
        public void resolveCollision (Asteroid collider) {

            if ( collider == this || !collider.isAlive)
                return;

            float distance = (_position - collider._position).Length();

            if ( !( distance < radius + collider.radius ) ) 
                return;

            // determine normal
            Vector2 unitNormal = _position - collider._position;
            unitNormal.Normalize(); // normalize normal
            // ensure asteroids do not stick together or orbit eachother
            _position = collider._position + ((radius + collider.radius) * unitNormal);
            // determine the initial velocity in direction of the normal
            Vector2 velocityNormal = Vector2.Dot(_initialVelocity, unitNormal ) * unitNormal;

            _velocity = _initialVelocity - (2 * velocityNormal);

        }

        public void resolveCollision ( CrusaderShield collider ) {
            if ( !collider.isAlive || GameScreen.deadAsteroids.Contains(this))
                return;

            float distance = ( _position - collider.Position ).Length ();

            if ( !( distance < radius + collider.radius ) )
                return;
            /*
            // determine normal
            Vector2 unitNormal = _position - collider.Position;
            unitNormal.Normalize (); // normalize normal
            // ensure asteroids do not stick together or orbit eachother
            _position = collider.Position + ( ( radius + collider.radius ) * unitNormal );
            // determine the initial velocity in direction of the normal
            Vector2 velocityNormal = Vector2.Dot ( _initialVelocity, unitNormal ) * unitNormal;

            _velocity = _initialVelocity - ( 2 * velocityNormal );
             */


            GameScreen.deadAsteroids.Enqueue ( this );
            GameScreen.currentNumAsteroids--;
            isAlive = false;

        }

        public virtual void resolveCollision ( Missile collider ) {
            GameScreen.deadAsteroids.Enqueue ( this );
            isAlive = false;
            GameScreen.gameSFXs["explode"].Play ();
            GameScreen.currentNumAsteroids--;
            GameScreen.particleSystem.AddExplosion2 ( _position );
        }

        public void resolveCollision ( CommandCenter collider ) {
            if (!isAlive)
                return;

            float distance = (_position - collider.Position).Length();

            if (!(distance < radius + collider.radius))
                return;

            collider.Hit();
            GameScreen.deadAsteroids.Enqueue ( this );
            isAlive = false;
            GameScreen.currentNumAsteroids--;
        }
    }
}
