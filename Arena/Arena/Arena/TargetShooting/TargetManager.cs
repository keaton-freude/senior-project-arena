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

        public TargetManager(ContentManager con)
        {
            r = new Random();
            content = con;
            _current_time = 0.0f;
            _time_until_next_spawn = (float)r.NextDouble() * 1.0f;
            Targets = new List<TargetShootingTarget>();
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
                x = (int)MathHelper.Clamp((int)x, 0, 1280 - 128);
                _current_time = 0.0f;
                _time_until_next_spawn = (float)r.NextDouble() * 5.25f;
                Targets.Add(new TargetShootingTarget(content.Load<Texture2D>(@"Target"),
                    null, new Vector2(x, 720), 1.0f, -Vector2.UnitY, new Vector2(0, 1), 12));
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
