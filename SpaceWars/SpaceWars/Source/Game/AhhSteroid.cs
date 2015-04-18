using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars.Source.Game {
    public class AhhSteroid : Asteroid {
        //AhhSteroid takes no arguments! jk... sort of
        public AhhSteroid( Texture2D texture, Vector2 position )
            : base( texture, position ) { }

        public void Update(GameTime gameTime, GraphicsDevice Device) {
            base.Update(gameTime, Device);
        }

        public void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            
        }
    }
}
