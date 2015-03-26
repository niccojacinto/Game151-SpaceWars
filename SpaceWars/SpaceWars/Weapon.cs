using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class Weapon : GameObject{

        private Vector2 _velocity;

        public Weapon (ContentManager content, GraphicsDevice Device, Vector2 position, float rotation)
            :base(content.Load<Texture2D>("Sprites/missile"), position, 0.02f, rotation, true, SpriteEffects.None)
        {
            _position = position;
            _rotation = rotation + (float)( 90 * ( Math.PI / 180 ) ); // Adjustment of the sprite image because the image is oriented on the wrong direction

        }

        public void TurnLeft () {
            _rotation -= 0.04f;
        }

        public void TurnRight () {
            _rotation += 0.04f;
        }

        public void Update ( GameTime gameTime ) {
            float vX = (float)Math.Cos ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            float vY = (float)Math.Sin ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            _velocity = new Vector2 ( vX, vY ) * 2.0f;
            _position += _velocity;
        }




    }
}
