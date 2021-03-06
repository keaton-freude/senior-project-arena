﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arena.Sprites
{
    public class AnimatedSprite : Sprite
    {
        private float _curr_time = 0.0f;
        private float _time_between_frames;
        private int _start_frame;
        private int _end_frame;
        protected int _curr_frame;
        protected Point _frame_size;

        public AnimatedSprite()
        {
            throw new NotImplementedException();
        }

        public AnimatedSprite(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale, int start_frame, int end_frame, float time_between_frames, Point frame_size) : 
            base(tex, src_rectangle, position, scale)
        {
            _curr_time = 0.0f;
            _time_between_frames = time_between_frames;
            _start_frame = start_frame;
            _end_frame = end_frame;
            _curr_frame = start_frame;
            _frame_size = frame_size;
            BuildAnimationRectangle();
            _texture.GetData(0, _src_rectangle, ColorData, 0, _frame_size.X * _frame_size.Y);
        }

        /* Sets _src_rectangle (The rectangle that is drawn to the screen from the source (texture))
         * to the correct frame, based on curr_frame, _frame_size */
        protected void BuildAnimationRectangle()
        {
            this._src_rectangle = new Rectangle(_curr_frame * _frame_size.X, 0, _frame_size.X, _frame_size.Y);
            _texture.GetData(0, _src_rectangle, ColorData, 0, _frame_size.X * _frame_size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            _curr_time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_curr_time >= _time_between_frames)
            {
                _curr_frame++;
                if (_curr_frame > _end_frame)
                    _curr_frame = _start_frame;
                _curr_time -= _time_between_frames;
                BuildAnimationRectangle();
            }
            
            base.Update(gameTime);
        }
    }
}
