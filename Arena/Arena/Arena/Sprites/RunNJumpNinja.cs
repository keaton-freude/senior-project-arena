using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arena.Collision;

namespace Arena.Sprites
{
    public class RunNJumpNinja: AnimatedSprite
    {
        public RectangleOverlay player_overlay;
        private bool _sliding = false;
        //private ParticleEngine.ParticleEngine particleEngine = null;
        //List<Texture2D> dirt_textures = new List<Texture2D>();

        public bool Jumping
        {
            get;
            set;
        }

        public float JumpCooldown
        {
            get;
            private set;
        }

        private string EffectName = "";

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
                    //Position = new Vector2(Position.X, Position.Y + 30);
                    _sliding = true;

                    ArenaParticleEngine.ParticleEngine.Instance.systems[EffectName].effects[0].Generating = true;
                }
                else if (value == false && Sliding)
                {
                    //We need to reset back to normal position
                    //Position = new Vector2(Position.X, Position.Y - 30);
                    _sliding = false;

                    //ParticleEngine.ParticleEngine.GetInstance().effects["NinjaDirtSlide"].Generating = false;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[EffectName].effects[0].Generating = false;
                }
            }
        }

        public bool OnGround
        {
            get;
            set;
        }

        private Vector2 _velocity;
        private Vector2 _gravity = new Vector2(0, 1.4f);
        private int _impulse = -20;

        public bool Collided = false;

        public void Jump()
        {
            if (OnGround)
            {
                _velocity = new Vector2(0, _impulse);
                OnGround = false;
            }
        }

        public void ReleaseJump()
        {
            if (_velocity.Y < -8)
                _velocity.Y = -8;
        }

        public Matrix NinjaTransform
        {
            get
            {
                return //Matrix.CreateTranslation(new Vector3(-test_sprite.Origin, 0.0f)) *
                Matrix.CreateScale(Scale) *  //would go here
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            }
        }

        public Rectangle NinjaRectangle
        {
            get
            {
                return Collision.CollisionDetection.CalculateBoundingRectangle(
                     new Rectangle(0, 0, 64, 64),
                     NinjaTransform);
            }
        }

        public RunNJumpNinja(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale, int start_frame, 
            int end_frame, float time_between_frames, Point frame_size, string effect_name, GraphicsDevice gDevice) :
            base(tex, src_rectangle, position, scale, start_frame, end_frame, time_between_frames, frame_size)
        {
            //dirt_textures.Add(dirt_texture);
            EffectName = effect_name;
            player_overlay = new RectangleOverlay(new Rectangle((int)Position.X, (int)Position.Y, 
                (int)(frame_size.X * scale), (int)(frame_size.Y * scale)), Color.Red, gDevice);

            Jumping = false;
        }

        public void Update(GameTime gameTime, RunNJumpMap game_map)
        {
            /* Subtract time from the JumpCooldown, clamp to 0.0f < JumpCooldown < 1.0f (don't want it to go below 0.0f */
            //JumpCooldown = MathHelper.Clamp(JumpCooldown - ((float)gameTime.ElapsedGameTime.TotalSeconds), 0.0f, 1.0f);

            /* Update Animations, Must do this first, because we may override some of that behavior later on */
            base.Update(gameTime);

            /* If we're displaying player_overlays, then update it's position */
            player_overlay.dummyRectangle = BoundingRectangle;

            _velocity.Y += _gravity.Y;
            Position += _velocity;

            if (BoundingRectangle.Bottom > game_map.GroundY)
            {
                Position = new Vector2(Position.X, game_map.GroundY - (_frame_size.Y * _scale));
                _velocity.Y = 0;
                OnGround = true;
            }

            /* The particle Engine will be instantiated when it's needed based on other variables
             * So if it's instantiated then we'll update the Particle Engine, and update it's emitter location */
            //if (ParticleEngine.ParticleEngine.GetInstance().effects.ContainsKey("NinjaDirtSlide"))
            //{
            //    /* Position.X + 110, Position.Y + 128 is right behind the front sliding foot */
                ArenaParticleEngine.ParticleEngine.Instance.systems[EffectName].effects[0].Emitter.Location = new Vector2(Position.X + 110, Position.Y + 124);
            //}

            if (Sliding)
            {
                /* If we're sliding, ensure that we use the correct frame
                 * and always use the correct Animation Rectangle */
                _curr_frame = 7;
                BuildAnimationRectangle();
            }

            if (!OnGround)
            {
                _curr_frame = 8;
                BuildAnimationRectangle();
            }
        }

        public bool Collides(RunNJumpObstacle obstacle)
        {
            if (NinjaRectangle.Intersects(obstacle.BoundingRectangle))
            {
                // Check collision with person
                if (CollisionDetection.IntersectPixels(obstacle.TranslationC, 32,
                                    32, obstacle.ColorData,
                                    NinjaTransform, 64,
                                    64, ColorData))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
