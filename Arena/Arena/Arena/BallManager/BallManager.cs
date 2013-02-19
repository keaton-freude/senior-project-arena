using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.Sprites;
using Microsoft.Xna.Framework.Input;
using Arena.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.BallManager
{
    public class BallManager
    {
        float time_between_spawns;
        List<TBBall> balls;
        MouseState prevMouse;
        public int explosion_id = 0;
        public BallManager(float time_between_spawns)
        {
            balls = new List<TBBall>();
            this.time_between_spawns = time_between_spawns;
            Utility.TimeKeeper.Instance.time_objects.Add("BallSpawn", 
                new Utility.TimeObject(time_between_spawns, 0.0f));
            explosion_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("explosion blue", Arena.Screens.TrailBlazer.content);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            /* Update all the balls */
            List<TBBall> to_remove = new List<TBBall>();
            foreach (TBBall ball in balls)
            {
                ball.Update(gameTime);

                /* check for triangle collision */

                Microsoft.Xna.Framework.Vector2 origin = new Microsoft.Xna.Framework.Vector2(1280 / 2, 720 / 2);

                Microsoft.Xna.Framework.Vector2 distance = ball.Position - origin;

                if (distance.Length() > 668.0f)
                {
                    Utility.MathFunctions.TriangleRectangleReflection(ref ball.direction, 0);
                    ball.Update(gameTime);
                }

                /* Check collsion of ball against all paddles */

                foreach (TrailerBlazerPlayer player in Arena.Screens.TrailBlazer._players)
                {
                    if (player.paddle.BoundingRectangle.Intersects(ball.GetCollisionRect()))
                    {
                        //Utility.MathFunctions.CircleRectangleReflection(ref ball.direction, 0);
                        if (player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.One || player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.Two)
                            Utility.MathFunctions.RectRectPaddleReflection(ref ball.direction, ball.GetCollisionRect(), player.paddle.BoundingRectangle, 0);
                        else
                            Utility.MathFunctions.RectRectPaddleReflection(ref ball.direction, ball.GetCollisionRect(), player.paddle.BoundingRectangle, 1);

                        if (player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.One)
                            ball.owner = TBBall.OWNER.PLAYERONE;
                        else if (player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.Two)
                            ball.owner = TBBall.OWNER.PLAYERTWO;
                        else if (player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.Three)
                            ball.owner = TBBall.OWNER.PLAYERTHREE;
                        else if (player.Player_Index == Microsoft.Xna.Framework.PlayerIndex.Four)
                            ball.owner = TBBall.OWNER.PLAYERFOUR;



                        ball.Update(gameTime);
                    }
                }

                if (ball.Position.X > 1250 || ball.Position.X < 32)
                {

                    /* Make sure we don't reflect if it hits a goal (left->right) */
                    if (ball.Position.Y < 292 || ball.Position.Y > 432)
                    {
                        Utility.MathFunctions.CircleRectangleReflection(ref ball.direction, 1);
                    }
                    else
                    {
                        /* Its a goal!, either left or right, lets check */
                        if (ball.Position.X < 32 && Arena.Screens.TrailBlazer._players.Count >= 3) // we would need 3 or more playing for this to be valid
                        {
                            /* left */
                            //ball.owner = TBBall.OWNER.PLAYERONE;
                            /* Regardless of what happens, the player to the left gets no points! */
                            SetParticleColor(ball.owner);
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = ball.Position;
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.LastLocation = ball.Position;
                            ((ArenaParticleEngine.OneShotParticleEffect)(ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0])).Fire();
                            
                            ball.Destroy();
                            to_remove.Add(ball);
                            int ball_owner = -1;
                            if (ball.owner == TBBall.OWNER.PLAYERONE)
                                ball_owner = 1;
                            else if (ball.owner == TBBall.OWNER.PLAYERTWO)
                                ball_owner = 2;
                            else if (ball.owner == TBBall.OWNER.PLAYERTHREE)
                                ball_owner = 3;
                            else if (ball.owner == TBBall.OWNER.PLAYERFOUR)
                                ball_owner = 4;
                            HandlePoints(3, ball_owner);
                        }
                        else if (ball.Position.X > 1250 && Arena.Screens.TrailBlazer._players.Count == 4) // We would need exactly 4 for this to be legit
                        {
                            /* right */
                            //ball.owner = TBBall.OWNER.PLAYERONE;
                            SetParticleColor(ball.owner);
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = ball.Position;
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.LastLocation = ball.Position;
                            ((ArenaParticleEngine.OneShotParticleEffect)(ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0])).Fire();

                            ball.Destroy();
                            to_remove.Add(ball);
                            int ball_owner = -1;
                            if (ball.owner == TBBall.OWNER.PLAYERONE)
                                ball_owner = 1;
                            else if (ball.owner == TBBall.OWNER.PLAYERTWO)
                                ball_owner = 2;
                            else if (ball.owner == TBBall.OWNER.PLAYERTHREE)
                                ball_owner = 3;
                            else if (ball.owner == TBBall.OWNER.PLAYERFOUR)
                                ball_owner = 4;
                            HandlePoints(4, ball_owner);
                        }
                        else
                        {
                            //Reflect (Dealing with 2-player and 3-player
                            Utility.MathFunctions.CircleRectangleReflection(ref ball.direction, 1);
                        }
                    }
                }
                else if (ball.Position.Y < 30 || ball.Position.Y > 690)
                {
                    /* Make sure we dont reflect if it hits a goal */
                    if (ball.Position.X < 572 || ball.Position.X > 708)
                    {
                        Utility.MathFunctions.CircleRectangleReflection(ref ball.direction, 0);
                    }
                    else
                    {
                        /* Its a goal!, either top or bottom, lets check... */
                        if (ball.Position.Y < 30)
                        {
                            /* top goal */
                            //ball.owner = TBBall.OWNER.PLAYERTWO;
                            SetParticleColor(ball.owner);
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = ball.Position;
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.LastLocation = ball.Position;
                            ((ArenaParticleEngine.OneShotParticleEffect)(ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0])).Fire();

                            ball.Destroy();
                            to_remove.Add(ball);
                            int ball_owner = -1;
                            if (ball.owner == TBBall.OWNER.PLAYERONE)
                                ball_owner = 1;
                            else if (ball.owner == TBBall.OWNER.PLAYERTWO)
                                ball_owner = 2;
                            else if (ball.owner == TBBall.OWNER.PLAYERTHREE)
                                ball_owner = 3;
                            else if (ball.owner == TBBall.OWNER.PLAYERFOUR)
                                ball_owner = 4;
                            HandlePoints(2, ball_owner);
                            
                        }
                        else
                        {
                            /* bottom goal */
                            ///ball.owner = TBBall.OWNER.PLAYERTWO;
                            SetParticleColor(ball.owner);
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = ball.Position;
                            ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.LastLocation = ball.Position;
                            ((ArenaParticleEngine.OneShotParticleEffect)(ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0])).Fire();
                            SetParticleColor(ball.owner);
                            ball.Destroy();
                            to_remove.Add(ball);
                            int ball_owner = -1;
                            if (ball.owner == TBBall.OWNER.PLAYERONE)
                                ball_owner = 1;
                            else if (ball.owner == TBBall.OWNER.PLAYERTWO)
                                ball_owner = 2;
                            else if (ball.owner == TBBall.OWNER.PLAYERTHREE)
                                ball_owner = 3;
                            else if (ball.owner == TBBall.OWNER.PLAYERFOUR)
                                ball_owner = 4;
                            HandlePoints(1, ball_owner);
                        }
                    }
                }


            }
            foreach (TBBall toremove in to_remove)
            {
                balls.Remove(toremove);
            }
            if (Utility.TimeKeeper.Instance.time_objects["BallSpawn"].ConsumeAvailability)
            {
                /* spawn ball */
                balls.Add(new TBBall(0.0f, new Microsoft.Xna.Framework.Vector2(1280 / 2, 720 / 2)));
            }

            if (prevMouse.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Microsoft.Xna.Framework.Vector2 direction;
                direction = new Microsoft.Xna.Framework.Vector2(Mouse.GetState().X, Mouse.GetState().Y) - new Microsoft.Xna.Framework.Vector2(1280 / 2, 720 / 2);
                direction.Normalize();
                //balls.Clear();
                balls.Add(new TBBall(direction, new Microsoft.Xna.Framework.Vector2(1280 / 2, 720 / 2)));
            }

            /* Check collisions here and update directions of balls based on that collision */

            /* Next collision for balls being in scoring zones and award points as needed */
            prevMouse = Mouse.GetState();
        }

        public void SetParticleColor(TBBall.OWNER owner)
        {
            if (owner == TBBall.OWNER.PLAYERONE)
            {
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Red;
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Red;
            }
            else if (owner == TBBall.OWNER.PLAYERTWO)
            {
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Green;
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Green;
            }
            else if (owner == TBBall.OWNER.PLAYERTHREE)
            {
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Yellow;
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Yellow;
            }
            else if (owner == TBBall.OWNER.PLAYERFOUR)
            {
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Blue;
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.Blue;
            }
            else
            {
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.White;
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].MasterParticles[0].StartColor = Color.White;             
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TBBall ball in balls)
            {
                ball.Draw(spriteBatch);
            }
        }

        public void HandlePoints(int goal_owner, int ball_owner)
        {
            /* A goal has been made, subtract points from the goal owner */
            Arena.Screens.TrailBlazer._players[goal_owner - 1].score--;

            /* next, if the ball is colored, add a point to the owner */
            if (ball_owner != -1)
                Arena.Screens.TrailBlazer._players[ball_owner - 1].score++;
        }
    }
}
