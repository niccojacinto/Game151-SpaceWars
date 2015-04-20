using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWars {
    public class CommandCenter : GameObject {

        public enum WeaponsList {
            GEMINI_MISSILE,
            PORT_MISSILE,
            CRUSADER_MISSILE
        }

        private Texture2D line; // Line that shows the launch angle of the player
        private static GameScreen _gameScreen;
        private static GraphicsDevice _Device;
        private float _launchAngle;
        public Missile _currentActive; // Missile currently launched
        public WeaponsList currentWeapon; // Weapon currently selected
        private int hp;
        Texture2D texGeminiMissile;

        private Dictionary<WeaponsList, int> weapons;
        public Dictionary<WeaponsList, int> Weapons {
            get { return weapons; }
        }


        public CommandCenter (GameScreen gameScreen, Texture2D texture, Texture2D weapon, Vector2 position)
            :base(texture, position, 0.1f, 0.0f, true, SpriteEffects.None)
        {
            _gameScreen = gameScreen;
            _position = position;
            _launchAngle = 0.0f;
            _Device = Screen.graphics;
            texGeminiMissile = weapon;
            currentWeapon = WeaponsList.GEMINI_MISSILE;
            hp = 100;

            line = new Texture2D ( _Device, 1, 1 );
            line.SetData<Color> (
                new Color[] { Color.White } );// fill the texture with White

            weapons = new Dictionary<WeaponsList, int> ();
            weapons.Add ( WeaponsList.GEMINI_MISSILE, 3  );
            weapons.Add ( WeaponsList.PORT_MISSILE, 0);
            weapons.Add ( WeaponsList.CRUSADER_MISSILE, 0);

        }

        public void Update ( GameTime gameTime ) {
            boxCollider = new Rectangle (
              (int)_position.X,
              (int)_position.Y,
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            _rotation += 0.01f;
            if (_currentActive != null) {
                _currentActive.Update ( gameTime );
            }
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


        public override void Draw (SpriteBatch spriteBatch) {
            base.Draw (spriteBatch);
            if (_currentActive != null)
                _currentActive.Draw ( spriteBatch );
            DrawLine ( spriteBatch,  _position );

            Color color = Color.GreenYellow;
            if ( hp < 20 ) 
                color = Color.Red;
            else if (hp < 60) 
                color = Color.Gold;

            string sHP = hp + "%";
            Vector2 stringSize = GameScreen.fontUI.MeasureString(sHP);
            spriteBatch.DrawString ( GameScreen.fontUI, sHP,
                new Vector2 ( ( _position.X - stringSize.X / 3 ), ( _position.Y - stringSize.Y - 20) ),
                color);

        }

        public void cycleWeaponsLeft () {
            if ( (int)currentWeapon == 0 ) {
                return;
            }
            int intEnum = (int)( currentWeapon );
            intEnum--;
            currentWeapon = (WeaponsList)intEnum;
        }


        public void cycleWeaponsRight () {
            //Console.WriteLine ((int)currentWeapon);
            if ( (int)currentWeapon == ( Enum.GetNames ( typeof ( WeaponsList ) ).Length - 1) ) {
                return;
            }
            int intEnum = (int)( currentWeapon );
            intEnum++;
            currentWeapon = (WeaponsList)intEnum;
        }

        public void AimLeft () {
            _launchAngle-=0.1f;
        }

        public void AimRight () {
            _launchAngle+=0.1f;
        }

        public void Launch () {
             if ( weapons[currentWeapon] > 0 ) {
                weapons[currentWeapon]--;
                _gameScreen.playSFX ( "launch" );
                switch ( currentWeapon ) {
                    case WeaponsList.GEMINI_MISSILE:
                            _currentActive = new GeminiMissile ( this, texGeminiMissile, _position, 0.02f, _launchAngle, SpriteEffects.None );
                        break;
                    default:
                        break;
                }
            }

        }

        public void Hit () {
            hp -= 7;
            _gameScreen.playSFX ( "explode" );
        }

    }
}
