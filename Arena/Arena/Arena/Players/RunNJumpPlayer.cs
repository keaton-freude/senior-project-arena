using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arena.Sprites;

namespace Arena.Players
{
    public class RunNJumpPlayer: Player
    {
        RunNJumpNinja _ninja;
        float speed = 5.0f;

        public bool Collided = false;

        public RunNJumpPlayer(PlayerIndex player_index, Texture2D ninja_texture, int player_starting_y, Texture2D dirt_particle_texture, GraphicsDevice gDevice) :
            base (player_index)
        {
            _ninja = new RunNJumpNinja(ninja_texture, new Rectangle(0, 0, 64, 64), new Vector2(0, player_starting_y), 2.0f, 0, 4, .08f,
                new Point(64, 64), dirt_particle_texture, gDevice);

            if (player_index == PlayerIndex.One)
                _ninja.Position = new Vector2(0, player_starting_y);
            else if (player_index == PlayerIndex.Two)
                _ninja.Position = new Vector2(0, player_starting_y);
            else if (player_index == PlayerIndex.Three)
                _ninja.Position = new Vector2(128, player_starting_y);
            else if (player_index == PlayerIndex.Four)
                _ninja.Position = new Vector2(196, player_starting_y);
        }

        public void Update(GameTime gameTime, RunNJumpMap game_map, string GameState)
        {
            if (GameState == "Playing")
            {
                //Update the player's ninja based on input
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && PrevKeyboardState.IsKeyUp(Keys.Space) ||
                    GamePad.GetState(Player_Index).IsButtonDown(Buttons.A) && PrevGamepadState.IsButtonUp(Buttons.A))
                    _ninja.Jump();
                if (PrevKeyboardState.IsKeyDown(Keys.Space) && Keyboard.GetState().IsKeyUp(Keys.Space) ||
                    GamePad.GetState(Player_Index).IsButtonUp(Buttons.A) && PrevGamepadState.IsButtonDown(Buttons.A))
                    _ninja.ReleaseJump();
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                    _ninja.Sliding = true;
                if (Keyboard.GetState().IsKeyUp(Keys.C))
                    _ninja.Sliding = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && _ninja.Sliding == false)
                    _ninja.Position += new Vector2(-1, 0) * speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && _ninja.Sliding == false)
                    _ninja.Position += new Vector2(1, 0) * speed;

                _ninja.Position += new Vector2(GamePad.GetState(Player_Index).ThumbSticks.Left.X * speed, 0);

                //gamepad checks

                if (GamePad.GetState(Player_Index).IsButtonDown(Buttons.B))
                    _ninja.Sliding = true;
                if (GamePad.GetState(Player_Index).IsButtonUp(Buttons.B) && GamePad.GetState(Player_Index).IsButtonDown(Buttons.B))
                    _ninja.Sliding = false;

                lock (game_map.ObstacleLock)
                {
                    foreach (RunNJumpObstacle obstacle in game_map.Obstacles)
                    {
                        // The per-pixel check is expensive, so check the bounding rectangles
                        // first to prevent testing pixels when collisions are impossible.
                        if (_ninja.Collides(obstacle))
                        {
                            /* Do somemthing with this collision */
                            Collided = true;

                        }
                    }
                }
            }

            _ninja.Update(gameTime, game_map);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _ninja.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
