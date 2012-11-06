using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arena.Sprites;
using Microsoft.Xna.Framework.Content;

namespace Arena.TargetShooting
{
    public class TargetManager
    {
        private float _current_time;
        private float _time_until_next_spawn;
        private ContentManager content;
        Random r;

        private struct TargetInfo
        {
            //radius of the target
            public float scale;
            //speed of the target
            public Vector2 speed;
            //Direction of the target
            public Vector2 direction;
            //Point value of the target
            public int point_value;
            //Color of the target
            public Color target_color;
            public float particle_scale;

            public TargetInfo(float r, Vector2 s, Vector2 d, int pv, Color tc, float particle_s)
            {
                scale = r;
                speed = s;
                direction = d;
                point_value = pv;
                target_color = tc;
                particle_scale = particle_s;
            }
         }

        private TargetInfo[] TargetInformation;

        //We need to build a dictionary of target types
        //and assign a randomness percent to each

        private void InstantiateTargetInfo()
        {
            TargetInformation = new TargetInfo[5];

            /* Easy targets show up 35% of the time
             * Easy-Medium show up 25% of the time
             * Medium show up 20% of the time
             * Medium-Hard show up 15% of the time
             * Hard show up 5% of the time
             */

            TargetInformation[0] = new TargetInfo(1.0f, new Vector2(0, 1), -Vector2.UnitY, 10, Color.White, .5f);
            TargetInformation[1] = new TargetInfo(0.85f, new Vector2(0, 5), -Vector2.UnitY, 20, Color.Green, .4f);
            TargetInformation[2] = new TargetInfo(.60f, new Vector2(0, 6), -Vector2.UnitY, 30, Color.Blue, .3f);
            TargetInformation[3] = new TargetInfo(.45f, new Vector2(0, 7), -Vector2.UnitY, 40, Color.Red, .2f);
            TargetInformation[4] = new TargetInfo(.2f, new Vector2(0, 8), -Vector2.UnitY, 50, Color.Gold, .1f);
        }


        public TargetManager(ContentManager con)
        {
            r = new Random();
            content = con;
            _current_time = 0.0f;
            _time_until_next_spawn = (float)r.NextDouble() * 1.0f;
            Targets = new List<TargetShootingTarget>();
            InstantiateTargetInfo();
        }

        public List<TargetShootingTarget> Targets
        {
            get;
            set;
        }

        public void Update(GameTime gameTime)
        {
            _current_time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_current_time > _time_until_next_spawn)
            {
                int x = r.Next(0, 1280);

                TargetInfo TI;

                double roll = r.NextDouble();

                if (roll <= .35)
                    TI = TargetInformation[0];
                else if (roll <= .60)
                    TI = TargetInformation[1];
                else if (roll <= .80)
                    TI = TargetInformation[2];
                else if (roll <= .95)
                    TI = TargetInformation[3];
                else
                    TI = TargetInformation[4];


                x = (int)MathHelper.Clamp((int)x, 0, 1280 - (128 * TI.scale));
                _current_time = 0.0f;
                _time_until_next_spawn = (float)r.NextDouble() * 1.0f;
                Targets.Add(new TargetShootingTarget(content.Load<Texture2D>(@"Target"),
                    null, new Vector2(x, 720), TI.scale, TI.direction, TI.speed, 64 * TI.scale, TI.target_color, TI.point_value, TI.particle_scale));
            }
            List<TargetShootingTarget> _targets_to_remove = new List<TargetShootingTarget>();
            foreach (TargetShootingTarget target in Targets)
            {
                target.Update(gameTime);
                if (target.Destroyed)
                    _targets_to_remove.Add(target);
            }

            foreach (TargetShootingTarget target in _targets_to_remove)
            {
                Targets.Remove(target);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TargetShootingTarget target in Targets)
            {
                target.Draw(spriteBatch);
            }
        }
    }
}
