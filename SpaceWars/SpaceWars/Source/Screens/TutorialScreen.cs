using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWars{
    public class TutorialScreen : Screen {
        GameObject bg;
        
        public TutorialScreen () : base () {
        }

        public override void Initialize () {
          
        }//public override void Initialize(){


        public override void Update ( GameTime gameTime ) { }

        public override void Update ( GameTime gameTime, KeyboardState keyState ) {
            if ( keyState.IsKeyDown ( Keys.Enter ) ) {
                Program.game.setScreen ( new GameScreen ( ) );
            }
        }//public override void Update(GameTime gameTime, KeyboardState keyState) {

        public override void UpdateInput ( KeyboardState keyState ) {
        }//public override void UpdateInput(KeyboardState keyState)

        public override void LoadContent () {
            bg = new GameObject (
                content.Load<Texture2D> ( "Sprites/tutorial" ),
                Vector2.Zero,
                1.25f,
                0.0f,
                false,
                SpriteEffects.None );
        }//public override void LoadContent()

        public override void Draw () {
            bg.Draw ( spriteBatch );
        }//public override void Draw()

    }
}
