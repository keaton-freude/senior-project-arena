using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Arena.Map
{
    public class Map
    {
        public const int MAP_WIDTH = 40;
        public const int MAP_HEIGHT = 22;
        public CollisionMask collision_mask;
        public List<MapCell> map
        {
            get;
            set;
        }

        private MapCell background_cell;

        public Map()
        {
            /* Don't use! */
        }

        public Map(string filename, List<Texture2D> textures, GraphicsDevice gDevice)
        {
            /* Load map from file */
            map = new List<MapCell>();
            LoadMap(filename, textures);
            background_cell = new MapCell(textures[26], 0, 0);

            /* Build collision mask */
            collision_mask = new CollisionMask();

            collision_mask.debug_rectangle_overlays.Add(new RectangleOverlay(new Rectangle(0, 0, 32, 720), Color.Green, gDevice));
            collision_mask.debug_rectangle_overlays.Add(new RectangleOverlay(new Rectangle(0, 0, 1280, 124), Color.Green, gDevice));
            collision_mask.debug_rectangle_overlays.Add(new RectangleOverlay(new Rectangle(1280 - 32, 0, 32, 720), Color.Green, gDevice));
            collision_mask.debug_rectangle_overlays.Add(new RectangleOverlay(new Rectangle(0, 720 - 46, 1280, 46), Color.Green, gDevice));
        }

        private void LoadMap(string filename, List<Texture2D> textures)
        {
            


            StreamReader reader = new StreamReader(filename);

            string[] broken = reader.ReadToEnd().Split(',');

            int count = 0;

            for (int i = 0; i < MAP_WIDTH; ++i)
            {
                for (int j = 0; j < MAP_HEIGHT; ++j)
                {
                    map.Add(new MapCell(textures[Convert.ToInt32(broken[count]) - 1], i * 32, j * 32));
                    count++;
                }
            }

            reader.Close();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MapCell cell in map)
            {
                background_cell.X = cell.X;
                background_cell.Y = cell.Y;
                background_cell.Draw(spriteBatch);
                cell.Draw(spriteBatch);
            }

            for (int i = 0; i < MAP_WIDTH; ++i)
            {
                background_cell.X = i * 32;
                background_cell.Y = MAP_HEIGHT * 32;
                background_cell.Draw(spriteBatch);
            }

            collision_mask.Draw(spriteBatch);
        }
    }
}
