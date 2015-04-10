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

        public Weapon (Texture2D texture, Vector2 position, float rotation)
            :base(texture, position, 0.02f, rotation, true, SpriteEffects.None)
        {
            _position = position;
            _rotation = rotation + (float)( 90 * ( Math.PI / 180 ) ); 
                    // Adjustment of the sprite image because the image is oriented in the wrong direction
            _speedMultiplier = 2.0f;

        }

        public virtual void TurnLeft () {
            _rotation -= 0.04f;
        }

        public virtual void TurnRight () {
            _rotation += 0.04f;
        }

        public virtual void Update ( GameTime gameTime ) {
            float vX = (float)Math.Cos ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            float vY = (float)Math.Sin ( _rotation + (float)( 270 * ( Math.PI / 180 ) ) );
            _velocity = new Vector2 ( vX, vY ) * _speedMultiplier;
            _position += _velocity;
        }

        public virtual void ActivateSpecial () {
            _speedMultiplier = 7.0f;
        }




    }
}
