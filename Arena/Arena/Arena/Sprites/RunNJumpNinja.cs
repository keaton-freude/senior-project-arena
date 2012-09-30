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

        private bool _sliding = false;
        private ParticleEngine.ParticleEngine particleEngine = null;
        List<Texture2D> dirt_textures = new List<Texture2D>();
        public float JumpCooldown
        {
            get;
            private set;
        }

        public bool Sliding
        {
            get
            {
                return _sliding;
            }
            set
            {
                if (value == true && !Sliding && !Jumping)
                {
                    //we need to set the sliding position
                    Position = new Vector2(Position.X, Position.Y + 30);
                    _sliding = true;

                    //if no particle engine instance is ready for him yet, make it ready
                    if (particleEngine == null)
                        particleEngine = new ParticleEngine.ParticleEngine(dirt_textures, new Vector2(Position.X + 64, Position.Y + 34));
                    particleEngine.Generating = true;
                }
                else if (value == false && Sliding)
                {
                    //We need to reset back to normal position
                    Position = new Vector2(Position.X, Position.Y - 30);
                    _sliding = false;

                    particleEngine.Generating = false;
                }
            }
        }

        /* Parameters for the jump ability */
        private const int _JUMP_HEIGHT = 90;
        private const float _JUMP_HANG_TIME = 0.2f;
        private const float _JUMP_SIN_WAVE_SPEED = ((float)Math.PI / 2.0f) / _JUMP_HANG_TIME;
        private float _jump_sin_wave_pos = 0.0f;
        /* ------------------------------ */

        public RunNJumpNinja(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale, int start_frame, int end_frame, float time_between_frames, Point frame_size, Texture2D dirt_texture) :
            base(tex, src_rectangle, position, scale, start_frame, end_frame, time_between_frames, frame_size)
        {
            dirt_textures.Add(dirt_texture);
            Jumping = false;
        }

        public new void Update(GameTime gameTime)
        {
            //This lucky guy can jump! So he has a jumping state. If jumping state is true, then we'll move along
            //a sine wave curve. If we complete more than half of the unit circle (Math.PI), then we've completed the jump
            //we also need to ensure we don't go below the ground

            JumpCooldown = MathHelper.Clamp(JumpCooldown - ((float)gameTime.ElapsedGameTime.TotalSeconds), 0.0f, 1.0f);

            base.Update(gameTime);

            if (Jumping)
            {
                float last_height = this._jump_sin_wave_pos;
                this._jump_sin_wave_pos += _JUMP_SIN_WAVE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this._jump_sin_wave_pos >= Math.PI)
                    this.Position += (new Vector2(0, Position.Y) * _JUMP_HEIGHT) / (_JUMP_HANG_TIME * 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                else
                    this.Position -= (new Vector2(0, 1) * 
                        ((float)Math.Sin(_jump_sin_wave_pos) - (float)Math.Sin(last_height)) * _JUMP_HEIGHT);

                _curr_frame = 8;
                BuildAnimationRectangle();
            }

            if (particleEngine != null)
                particleEngine.EmitterLocation = new Vector2(Position.X + 64, Position.Y + 34);

            if (particleEngine != null)
                particleEngine.Update();
            if (Sliding)
            {
                _curr_frame = 7;
                _rotation = (float)MathHelper.ToRadians(305.0f);
                //need to translate our ninja down so he's on the ground again

                BuildAnimationRectangle();
            }
            else
            {
                _rotation = 0.0f;
            }

            if (Position.Y > 336 && Jumping == true)
            {
                Jumping = false;
                JumpCooldown = .2f;
                Position = new Vector2(Position.X, 336);
                this._jump_sin_wave_pos = 0.0f;
               // _curr_frame = 0;
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {



            base.Draw(spriteBatch);

            if (particleEngine != null)
                particleEngine.Draw(spriteBatch);
        }
    }
}
