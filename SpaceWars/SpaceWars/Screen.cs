using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class Screen {

        protected bool _active = true;
        protected GameObject background;

        public CommandCenter player1, player2;
        public List<Asteroid> asteroids;

        // Settings
        int numAsteroids = 50;

        public Screen (ContentManager content, GraphicsDevice Device) {
            background = new GameObject (
                content.Load<Texture2D>("Sprites/space"),
                Vector2.Zero,
                2.5f,
                0.0f,
                false, 
                SpriteEffects.None
                );

            Initialize ( content, Device );
        }

        public void Initialize (ContentManager content, GraphicsDevice Device) {
            asteroids = new List<Asteroid> ();
            player1 = new CommandCenter ( content, Device, new Vector2(100, 100));
            player2 = new CommandCenter ( content, Device, new Vector2 ( 450, 200 ) );
            Random random = new Random();
            for ( int i = 0; i < numAsteroids; i++ ) {
                float x = random.Next ( 0, 1080 );
                float y = random.Next ( 0, 640 );
                asteroids.Add( new Asteroid ( content, Device, new Vector2 ( x, y ) ));
            }
        }

        public virtual void Update ( GameTime gameTime ) {
            player1.Update ( gameTime );
            player2.Update ( gameTime );
            for ( int i = 0; i < numAsteroids; i++ ) {
                asteroids[i].Update (gameTime);
            }
        }

        public virtual void Draw ( SpriteBatch spriteBatch ) {
            if ( _active ) {
                background.Draw ( spriteBatch );
                player1.Draw ( spriteBatch );
                player2.Draw ( spriteBatch );
                for ( int i = 0; i < numAsteroids; i++ ) {
                    asteroids[i].Draw( spriteBatch );
                }
            }
        }


    }
}
