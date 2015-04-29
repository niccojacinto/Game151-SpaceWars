using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
 public abstract class Missile : GameObject {

        public CommandCenter Player;
        protected Vector2 velocity;
        protected float speedMultiplier;
        protected float specialTimerDelay;

        public Missile (CommandCenter player, Texture2D texture, Vector2 position, float scale,  float rotation, SpriteEffects spriteEffects)
            :base(texture, position, scale, rotation, true, spriteEffects)
        {
            Player = player;         
        }

        abstract public void TurnLeft();
        abstract public void TurnRight();
        abstract public void ActivateSpecial();
        abstract public void resolveCollision( CommandCenter collider );
        //abstract public void resolveCollision ( Missile collider );
        abstract public void resolveCollision( Asteroid collider );

        abstract public void Update(GameTime gameTime);

    }// public abstract class Screen {
}//namespace SpaceWars {

