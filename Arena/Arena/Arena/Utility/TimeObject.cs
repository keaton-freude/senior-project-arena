using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arena.Utility
{
    public class TimeObject
    {
        public float time_threshold;
        public float current_time;
        public bool available = false;
        public bool reset_time_on_consume;

        public bool ConsumeAvailability
        {
            get
            {
                if (available)
                {
                    available = false;
                    if (reset_time_on_consume)
                    {
                        current_time = 0.0f;
                    }
                    return true;
                }
                else
                    return false;
            }
        }

        public TimeObject(float tt, float st)
        {
            time_threshold = tt;
            current_time = st;
            reset_time_on_consume = true;
        }

        public void Update(float dt)
        {
            current_time += dt;

            if (current_time >= time_threshold)
            {
                current_time -= time_threshold;
                available = true;
            }
        }
    }
}
