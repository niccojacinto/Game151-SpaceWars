using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars {
    public class GameObject {

        protected Texture2D _texture;
        protected Vector2 _position;
        protected float _scale;
        protected float _rotation;
        protected SpriteEffects _spriteEffect;
        protected Vector2 _origin;
        protected bool UseOrigin { get; set; }

        // Base Constructor : Actors & Image Elements
        public GameObject ( Texture2D texture, Vector2 position, float scale, float rotation, bool useOrigin, SpriteEffects spriteEffects ) {
            _position = position;
            _scale = scale;
            _rotation = rotation;
            _spriteEffect = spriteEffects;
            _texture = texture;
            UseOrigin = useOrigin;
            if ( UseOrigin ) {
                _origin = new Vector2 ( _texture.Width / 2, _texture.Height / 2 );
            }
        }

        public virtual void Draw (SpriteBatch spriteBatch) {
            // texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
            spriteBatch.Draw ( _texture,        
                    _position,                 
                    null,                  // Rectangle <nullable>
                    Color.White,                 
                    _rotation,             
                    _origin,               
                    _scale,              
                    _spriteEffect,        
                    0 );                   // single (0 or 1)
           
        }


    }
}
