using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class Screen {

        protected bool _active = true;
        protected GameObject background;
        Random random = new Random ();
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
            player2 = new CommandCenter ( content, Device, new Vector2 ( 1000, 200 ) );
           
            for ( int i = 0; i < numAsteroids; i++ ) {
                float x = random.Next ( 50, 1000 );
                float y = random.Next ( 50, 600 );
                float vX = random.Next ( 0, 3 ) - 1;
                float vY = random.Next ( 0, 3 ) - 1;
                Asteroid tmpAsteroid = new Asteroid ( content, Device, new Vector2 ( x, y ) );
                tmpAsteroid._velocity = new Vector2 ( vX * 0.1f, vY * 0.1f);
                asteroids.Add(tmpAsteroid);
            }
        }

        public virtual void Update ( GameTime gameTime ) {

        }

        public virtual void Update ( GameTime gameTime, GraphicsDevice Device ) {
            player1.Update ( gameTime );
            player2.Update ( gameTime );
            foreach (Asteroid asteroid in asteroids) {
                asteroid.Update (gameTime, Device);
            }

        }

        public void UpdateInput (KeyboardState keyState) {
            if ( player1._currentActive == null ) {
                if ( keyState.IsKeyDown ( Keys.A ) ) {
                    player1.AimLeft ();
                }
                else if ( keyState.IsKeyDown ( Keys.D ) ) {
                    player1.AimRight ();
                }

                if ( keyState.IsKeyDown ( Keys.W ) ) {
                    player1.Launch ();
                }
            }
            else {
                if ( keyState.IsKeyDown ( Keys.A ) ) {
                    player1._currentActive.TurnLeft ();
                }
                else if ( keyState.IsKeyDown ( Keys.D ) ) {
                    player1._currentActive.TurnRight ();
                }
            }

            if ( player2._currentActive == null ) {
                if ( keyState.IsKeyDown ( Keys.NumPad4 ) ) {
                    player2.AimLeft ();
                }
                else if ( keyState.IsKeyDown ( Keys.NumPad6 ) ) {
                    player2.AimRight ();
                }
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
