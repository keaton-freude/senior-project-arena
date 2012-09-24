using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena
{
    public class RunNJumpMap
    {
        //A map a 2d structure with grassy tile on top, dirt blocks below
        //the map moves steadily to the left as the game progresses
        //we do some magic hackery to make it look like its always moving left
        //more info on this hackery below:

        private List<RunNJumpMapCell> _map = new List<RunNJumpMapCell>();
        private Vector2 _cell_move_direction = Vector2.UnitX;
        private float _map_scroll_speed = 400.0f;

        private const int MAP_TOP = 400;
        private const int MAP_LEFT = 0;
        private const int MAP_NUM_CELLS_WIDE = 22;
        private const int MAP_NUM_CELLS_HIGH = 6;

        private float _current_transform = 0.0f;

        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        public int GroundY
        {
            get;
            private set;
        }


        public RunNJumpMap(Texture2D dirt_texture, Texture2D grassy_dirt_texture)
        {
            for (int i = 0; i < MAP_NUM_CELLS_WIDE; ++i)
            {
                _map.Add(new RunNJumpMapCell(grassy_dirt_texture, new Vector2(MAP_LEFT + (i * CELL_WIDTH), MAP_TOP)));
                for (int j = 0; j < MAP_NUM_CELLS_HIGH; ++j)
                {
                    _map.Add(new RunNJumpMapCell(dirt_texture, new Vector2(MAP_LEFT + (i * CELL_WIDTH), MAP_TOP + CELL_HEIGHT + (j * CELL_HEIGHT))));
                }
            }

            GroundY = MAP_TOP;
        }



        public void Update(GameTime gameTime)
        {
            foreach (RunNJumpMapCell cell in _map)
            {
                cell.Position -= _cell_move_direction * _map_scroll_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            _current_transform += _cell_move_direction.X * _map_scroll_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_current_transform > CELL_WIDTH)
            {
                //we need to move all of your blocks back however many units of transform we collected
                foreach (RunNJumpMapCell cell in _map)
                    cell.Position += _cell_move_direction * _current_transform;
                _current_transform = 0.0f;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (RunNJumpMapCell cell in _map)
                cell.Draw(spriteBatch);
        }
    }
}
