using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars
{
    public class AhhSteroidPORT : Asteroid
    {
        //AhhSteroid takes no arguments! jk... sort of
        public AhhSteroidPORT(Texture2D texture, Vector2 position)
            : base(texture, position) { }

        public override void Update(GameTime gameTime, GraphicsDevice Device)
        {
            base.Update(gameTime, Device);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);

            // texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
            if (!isAlive)
                return;
            spriteBatch.Draw(_texture,
                    _position,
                    null,                  // Rectangle <nullable>
                    Color.CornflowerBlue,
                    _rotation,
                    _origin,
                    Scale,
                    _spriteEffect,
                    0);                   // single (0 or 1)

        }

        public override void resolveCollision(Missile collider)
        {
            if (!isAlive)
                return;
            base.resolveCollision(collider);
            GameScreen.gameSFXs["powerup"].Play ();
            collider.Player.GivePowerUp(CommandCenter.WeaponsList.PORT_MISSILE);
            GameScreen.powerUpText.Add ( new PowerUpText ( this.Position, "PORT Missile" ) );
            GameScreen.currentNumPowerUps--;
        }
    }
}