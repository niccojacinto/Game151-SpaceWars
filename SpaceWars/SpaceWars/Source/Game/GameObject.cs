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
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Scale;
        protected float _rotation;
        public float Rotation { get; set; }
        protected SpriteEffects _spriteEffect;
        protected Vector2 _origin;
        public Vector2 Origin {
            get { return _origin; }
        }
        protected bool UseOrigin { get; set; }
        public bool isAlive { get; set; }
        public Rectangle boxCollider;

        // Base Constructor : Actors & Image Elements
        public GameObject (Vector2 position) {
            _position = position;
            isAlive = true;
        }

        public GameObject ( Texture2D texture, Vector2 position, float scale, float rotation, bool useOrigin, SpriteEffects spriteEffects ) {
            _position = position;
            Scale = scale;
            _rotation = rotation;
            _spriteEffect = spriteEffects;
            _texture = texture;
            UseOrigin = useOrigin;
            if ( UseOrigin ) {
                _origin = new Vector2 ( _texture.Width / 2, _texture.Height / 2 );
            }
            isAlive = true;
        }

        public virtual void Draw (SpriteBatch spriteBatch) {
            // texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
            if ( !isAlive )
                return;
            spriteBatch.Draw ( _texture,        
                    _position,                 
                    null,                  // Rectangle <nullable>
                    Color.White,                 
                    _rotation,             
                    _origin,               
                    Scale,              
                    _spriteEffect,        
                    0 );                   // single (0 or 1)
           
        }


    }
}
