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
        public static GameScreen _gameScreen;
        private static GraphicsDevice _Device;
        private float _launchAngle;
        public float radius;

        public int hp;
        public float stasisDelay;

        public Missile _currentActive; // Missile currently launched
        public WeaponsList currentWeapon; // Weapon currently selected
        public Texture2D texGeminiMissile, texCrusaderShield;

        public List<CrusaderShield> shields;
        private Dictionary<WeaponsList, int> weapons;
        public Dictionary<WeaponsList, int> Weapons {
            get { return weapons; }
        }
        


        public CommandCenter (GameScreen gameScreen, Texture2D texture, Texture2D shield, Texture2D weapon, Vector2 position)
            :base(texture, position, 0.1f, 0.0f, true, SpriteEffects.None)
        {
            stasisDelay = 0;
            _gameScreen = gameScreen;
            _position = position;
            _launchAngle = 0.0f;
            _Device = Screen.graphics;
            // TODO: Make a list for all textures passed to this class
            texGeminiMissile = weapon;
            texCrusaderShield = shield;
            currentWeapon = WeaponsList.GEMINI_MISSILE;
            hp = 100;
            radius = (_texture.Width * Scale) / 2;

            line = new Texture2D ( _Device, 1, 1 );
            line.SetData<Color> (
                new Color[] { Color.White } );// fill the texture with White
            shields = new List<CrusaderShield> ();
            weapons = new Dictionary<WeaponsList, int> ();
            weapons.Add ( WeaponsList.GEMINI_MISSILE, 999  );
            weapons.Add ( WeaponsList.PORT_MISSILE, 3);
            weapons.Add ( WeaponsList.CRUSADER_MISSILE, 3);

        }

        public bool isWithinRadius ( Vector2 colPosition, float colRadius ) {

            float distance = ( _position - colPosition ).Length ();

            if ( !( distance < radius + colRadius + 100) )
                return true;

            return false;
        }

        public void Update ( GameTime gameTime ) {
            float elapsed = ( (float)gameTime.ElapsedGameTime.Milliseconds ) / 1000.0f;
            if ( stasisDelay > 0 )
                stasisDelay -= elapsed;
            boxCollider = new Rectangle (
              (int)_position.X - (int)((_texture.Width * Scale * 0.75) / 2),
              (int)_position.Y - (int)((_texture.Height * Scale * 0.75) / 2),
              (int)( _texture.Width * Scale ),
              (int)( _texture.Height * Scale ) );
            _rotation += 0.01f;
            if (_currentActive != null) {
                _currentActive.Update ( gameTime );
            }

            foreach ( CrusaderShield shield in shields ) {
                if (shield.isAlive)
                    shield.Update ( gameTime );
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

        public void GivePowerUp ( WeaponsList weapon ) {
            /*switch (weapon) {
                case WeaponsList.GEMINI_MISSILE :
                    weapons[weapon] += 1;
                    break;

                case WeaponsList.CRUSADER_MISSILE :
                    break;

                case WeaponsList.PORT_MISSILE :
                    break;

                default :
                    break;
            } */
            weapons[weapon] += 1;
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

            foreach ( CrusaderShield shield in shields ) {
                shield.Draw ( spriteBatch );
            }

        }

        public void cycleWeaponsLeft () {
            GameScreen.gameSFXs["cycle"].Play ();
            if ( (int)currentWeapon == 0 ) {
                return;
            }
            int intEnum = (int)( currentWeapon );
            intEnum--;
            currentWeapon = (WeaponsList)intEnum;

        }


        public void cycleWeaponsRight () {
            GameScreen.gameSFXs["cycle"].Play ();
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
             if ( weapons[currentWeapon] > 0 && stasisDelay <= 0 ) {
                 stasisDelay = 2.0f;
                weapons[currentWeapon]--;

                switch ( currentWeapon ) {
                    case WeaponsList.GEMINI_MISSILE:
                        GameScreen.gameSFXs["launch"].Play ();
                        _currentActive = new GeminiMissile ( this, texGeminiMissile, _position, 0.02f, _launchAngle, SpriteEffects.None );
                        break;
                    case WeaponsList.PORT_MISSILE:
                        GameScreen.gameSFXs["nuke"].Play ();
                        _currentActive = new PORTMissile ( this, texGeminiMissile, _position, 0.02f, _launchAngle, SpriteEffects.None );
                        break;
                    case WeaponsList.CRUSADER_MISSILE:
                        GameScreen.gameSFXs["nuke"].Play ();
                        _currentActive = new CrusaderMissile ( this, texGeminiMissile, _position, 0.02f, _launchAngle, SpriteEffects.None );
                        break;
                    default:
                        break;
                }
            }

        }

        public void Hit () {
            hp -= 7;
            GameScreen.gameSFXs["explode"].Play ();
        }

    }
}
