using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Players
{
    public class TargetShootingPlayer: Player
    {
        public TargetShootingCrosshair Crosshair
        {
            get;
            set;
        }

        public Int32 Score
        {
            get;
            set;
        }

        public string Target_Explode_Name = "";

        public TargetShootingPlayer(PlayerIndex player_index, ContentManager content) :
            base(player_index)
        {
            Color color = Color.White;
            switch (player_index)
            {
                case PlayerIndex.One:
                    color = Color.Red;
                    break;
                case PlayerIndex.Two:
                    color = Color.Green;
                    break;
                case PlayerIndex.Three:
                    color = Color.Blue;
                    break;
                case PlayerIndex.Four:
                    color = Color.Orange;
                    break;
            }
            Crosshair = new TargetShootingCrosshair(content.Load<Texture2D>(@"Crosshairs/circle-5"),
                null, new Vector2(600, 300), .4f, 70, color);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime, List<TargetShootingTarget> targets)
        {
            Crosshair.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            //Crosshair.Position += new Vector2(GamePad.GetState(Player_Index).ThumbSticks.Left.X * 25.0f, GamePad.GetState(Player_Index).ThumbSticks.Left.Y * -25.0f);

            //check player input (A firing action)

            if (PrevMouseState.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released ||
                GamePad.GetState(Player_Index).Triggers.Right > .35f && PrevGamepadState.Triggers.Right <= .35f)
            {
                //Shot fired!

                //Create particle effect
                if (ParticleEngine.ParticleEngine.GetInstance().effects.ContainsKey("ShotsFiredEffect"))
                {
                    //Ask the effect to generate new particles
                    ((ParticleEngine.TargetShootingParticleEffects.ShotFiredEffect)ParticleEngine.ParticleEngine.GetInstance().effects["ShotsFiredEffect"]).GenerateShotFired(Crosshair.Center, Crosshair.Radius * Crosshair.Scale);
                }

                foreach (TargetShootingTarget target in targets)
                {
                //Check collision
                    if (Collision.CollisionDetection.CircleCircleCollision(Crosshair.Center, Crosshair.Radius * Crosshair.Scale,
                        target.Center, target.Radius * target.Scale))
                    {
                        target.Destroyed = true;

                        //Throw up some text talking about it
                        FloatingText.FloatingTextEngine.GetInstance().AddText(String.Format("{0}!", target.TargetValue), new Vector2(target.Position.X, target.Position.Y - 10),
                            -Vector2.UnitY, new Vector2(0, 3), 1.0f, Crosshair.CrosshairColor);
                        Score += target.TargetValue;

                        /* and EXPLODE IT! */

                            //((ParticleEngine.TargetShootingParticleEffects.TargetDestroyedEffect)ParticleEngine.ParticleEngine.GetInstance().effects["TargetDestroyedEffect"]).DestroyTarget(target.Center, target.Radius * target.Scale, target.TargetColor, target.ParticleScale);
                        ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[0].effects[0]).Emitter.Location = new Vector2(target.Center.X, target.Center.Y);
                        ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[0].effects[0]).Emitter.LastLocation = new Vector2(target.Center.X, target.Center.Y);

                        foreach (ArenaParticleEngine.Particle p in ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[0].effects[0]).MasterParticles)
                        {
                            p.StartColor = target.TargetColor;
                            p.EndColor = target.TargetColor;
                            p.ScaleStart = target.ParticleScale;
                            p.ScaleEnd = target.ParticleScale;
                        }
                        
                        ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[0].effects[0]).Fire();
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Crosshair.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
