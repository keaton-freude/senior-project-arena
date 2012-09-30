using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

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
        private float _map_scroll_speed = 1000.0f;
        

        private const int MAP_TOP = 400;
        private const int MAP_LEFT = 0;
        private const int MAP_NUM_CELLS_WIDE = 30;
        private const int MAP_NUM_CELLS_HIGH = 6;

        Random random;

        private Texture2D _obstacle_texture;

        private object obstacle_lock = new object();

        private float _current_transform = 0.0f;

        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        public Timer ObstacleTimer
        {
            get;
            set;
        }
        

        public List<RunNJumpObstacle> Obstacles
        {
            get;
            set;
        }

        public int GroundY
        {
            get;
            private set;
        }


        public RunNJumpMap(Texture2D dirt_texture, Texture2D grassy_dirt_texture, Texture2D obstacle_texture)
        {
            for (int i = 0; i < MAP_NUM_CELLS_WIDE; ++i)
            {
                _map.Add(new RunNJumpMapCell(grassy_dirt_texture, new Vector2(MAP_LEFT + (i * CELL_WIDTH), MAP_TOP)));
                for (int j = 0; j < MAP_NUM_CELLS_HIGH; ++j)
                {
                    _map.Add(new RunNJumpMapCell(dirt_texture, new Vector2(MAP_LEFT + (i * CELL_WIDTH), MAP_TOP + CELL_HEIGHT + (j * CELL_HEIGHT))));
                }
            }
            random = new Random();
            _obstacle_texture = obstacle_texture;
            GroundY = MAP_TOP;
            Obstacles = new List<RunNJumpObstacle>();
            TimerCallback tcb = this.SpawnObstacle;
            ObstacleTimer = new Timer(tcb, null, 1000, 1000);
        }

        public void SpawnObstacle(Object stateInfo)
        {
            int y = 0;
            int choice = random.Next(2);
            if (choice == 0)
                y = 280;
            else if (choice == 1)
                y = 380;

            RunNJumpObstacle obstacle = new RunNJumpObstacle(_obstacle_texture, null, new Vector2(1280, y), 2.0f);

            lock (obstacle_lock)
            {
                Obstacles.Add(obstacle);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (RunNJumpMapCell cell in _map)
            {
                cell.Position -= _cell_move_direction * _map_scroll_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            _current_transform += _cell_move_direction.X * _map_scroll_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_current_transform >= CELL_WIDTH)
            {
                //we need to move all of your blocks back however many units of transform we collected
                foreach (RunNJumpMapCell cell in _map)
                    cell.Position += _cell_move_direction * _current_transform;
                _current_transform = 0.0f;
            }

            foreach (RunNJumpObstacle obstacle in Obstacles)
            {
                obstacle.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (RunNJumpObstacle obstacle in Obstacles)
                obstacle.Draw(spriteBatch);
            foreach (RunNJumpMapCell cell in _map)
                cell.Draw(spriteBatch);
        }
    }
}
