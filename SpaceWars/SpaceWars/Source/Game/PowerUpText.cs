using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars {
    public class PowerUpText : GameObject{
        private static Vector2 SWEEP_CENTER = new Vector2 ( 50, 0 );
        private float duration;
        private String name;

        public PowerUpText ( Vector2 position, String name ) : base (position) {
            duration = 1;
            this.name = name;
        }
        

        public virtual void Update (float elapsed) {
            _position.Y -= 1;
            duration -= elapsed;
            if ( duration <= 0 ) {
                isAlive = false;
            }

        }

        public virtual void Draw ( SpriteBatch spriteBatch ) {
            Color color = new Color ( Color.LimeGreen, duration);//test
            spriteBatch.DrawString ( GameScreen.fontUI, "Obtained " + name, Position - SWEEP_CENTER/*Sweep!*/, color );

        }                                                                               
    }
}
