using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    public class RunNJumpNinja: AnimatedSprite
    {
        public bool Jumping
        {
            get;
            set;
        }

        private const int _JUMP_HEIGHT = 128;

        private const float _JUMP_HANG_TIME = 0.5f;

        private const float _JUMP_SIN_WAVE_SPEED = ((float)Math.PI / 2.0f) / _JUMP_HANG_TIME;

        private float _jump_sin_wave_pos = 0.0f;

        public RunNJumpNinja(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale, int start_frame, int end_frame, float time_between_frames, Point frame_size) :
            base(tex, src_rectangle, position, scale, start_frame, end_frame, time_between_frames, frame_size)
        {
            Jumping = false;
        }

        public void Update(GameTime gameTime)
        {
            //This lucky guy can jump! So he has a jumping state. If jumping state is true, then we'll move along
            //a sine wave curve. If we complete more than half of the unit circle (Math.PI), then we've completed the jump
            //we also need to ensure we don't go below the ground
            //base.Update(gameTime);




            if (Jumping)
            {
                float last_height = this._jump_sin_wave_pos;
                this._jump_sin_wave_pos += _JUMP_SIN_WAVE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this._jump_sin_wave_pos >= Math.PI)
                    this.Position += (new Vector2(0, Position.Y) * _JUMP_HEIGHT) / (_JUMP_HANG_TIME * 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                else
                    this.Position -= (new Vector2(0, 1) * ((float)Math.Sin(_jump_sin_wave_pos) - (float)Math.Sin(last_height)) * _JUMP_HEIGHT);

                _curr_frame = 8;
            }

            if (Position.Y > 274)
            {
                Jumping = false;
                Position = new Vector2(Position.X, 272);
                this._jump_sin_wave_pos = 0.0f;
               // _curr_frame = 0;
            }


        }
    }
}
