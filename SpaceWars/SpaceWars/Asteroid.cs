using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class Asteroid : GameObject {

        public Vector2 _velocity;

        public Asteroid ( ContentManager content, GraphicsDevice Device, Vector2 position )
            : base ( content.Load<Texture2D> ( "Sprites/asteroid" ), position, 0.1f, 0.0f, true, SpriteEffects.None ) {
            _position = position;

        }

        public void Update ( GameTime gameTime, GraphicsDevice Device ) {

            _rotation += 0.005f;
            _position += _velocity;
            if ( _position.X > Device.Viewport.Width ) {
                _velocity = new Vector2 ( _velocity.X * -1, _velocity.Y);
            }
            else if ( _position.X < 0 ) {
                _velocity = new Vector2 ( _velocity.X * -1, _velocity.Y );
            }

            if ( _position.Y > Device.Viewport.Height ) {
                _velocity = new Vector2 ( _velocity.X, _velocity.Y * -1 );
            }
            else if ( _position.Y < 0 ) {
                _velocity = new Vector2 ( _velocity.X, _velocity.Y * -1 );
            }
        }
    }
}
