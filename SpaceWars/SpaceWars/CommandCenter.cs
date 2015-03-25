using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class CommandCenter : GameObject {

        Texture2D line;
        private float _launchAngle;

        public CommandCenter (ContentManager content, GraphicsDevice Device, Vector2 position)
            :base(content.Load<Texture2D>("Sprites/command_center"), position, 0.1f, 0.0f, true, SpriteEffects.None)
        {
            _position = position;

            _launchAngle = 0.0f;

            line = new Texture2D ( Device, 1, 1 );
            line.SetData<Color> (
                new Color[] { Color.White } );// fill the texture with Red
            
        }

        public void Update ( GameTime gameTime ) {

            _rotation += 0.01f;
            Console.WriteLine ( _launchAngle );
        }

        void DrawLine ( SpriteBatch sb, Vector2 start ) {
            


            sb.Draw ( line,
                new Rectangle (// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    30, //sb will strech the texture to fill this rectangle
                    3 ), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                _launchAngle,    
                new Vector2 ( 0, 0 ), // point in line about which to rotate
                SpriteEffects.None,
                0 );

        }

        public void Draw (SpriteBatch spriteBatch) {
            base.Draw (spriteBatch);
            DrawLine ( spriteBatch,  _position );
        }


        public void AimLeft () {
            _launchAngle-=0.1f;
        }

        public void AimRight () {
            _launchAngle+=0.1f;
        }

    }
}
