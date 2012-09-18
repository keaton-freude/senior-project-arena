using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.UI
{
    public class GameIcon
    {
        private Texture2D _texture_no_border;
        //private Texture2D _texture_with_border;
        private Texture2D _texture_border;
        public int GameID { get; private set; }
        private bool _border_enabled;
        private bool _alpha_increase = false;
        private const float ALPHA_SPEED = 2.0f;
        public bool BorderEnabled
        {
            get
            {
                return _border_enabled;
            }

            set
            {
                _border_enabled = value;
                if (value == true)
                {
                    //This icon is selected, reset alpha to full
                    _border_alpha = 1.0f;
                }
            }
        }
        private Rectangle _position;
        private float _border_alpha;
        

        public GameIcon()
        {
        }

        public GameIcon(Texture2D tex_no_border, Texture2D tex_border, int game_id, Rectangle pos)
        {
            this._texture_no_border = tex_no_border;
            this._texture_border = tex_border;
            this.GameID = game_id;
            this._position = pos;
            this.BorderEnabled = false;
            _border_alpha = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            if (_alpha_increase)
                _border_alpha += ALPHA_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                _border_alpha -= ALPHA_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_border_alpha < 0.0f)
                _alpha_increase = true;
            else if (_border_alpha > 1.0f)
                _alpha_increase = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BorderEnabled)
            {
                spriteBatch.Draw(_texture_no_border, _position, Color.White);
                spriteBatch.Draw(_texture_border, _position, Color.White * _border_alpha);
            }
            else
            {
                //Draw unselected icons at 30% alpha, this gives the illusion of our selected icon as being
                //much brigher (more obviously selected)
                spriteBatch.Draw(_texture_no_border, _position, Color.White * .3f);
            }
        }
    }
}
