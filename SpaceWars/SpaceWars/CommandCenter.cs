using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    class CommandCenter : GameObject {
        public CommandCenter (ContentManager content, GraphicsDevice Device, Vector2 position)
            :base(content.Load<Texture2D>("Sprites/command_center"), position, 0.1f, 0.0f, true, SpriteEffects.None)
        {
            _position = position;
        }

        public void Update ( GameTime gameTime ) {

            _rotation += 0.01f;
        }
    }
}
