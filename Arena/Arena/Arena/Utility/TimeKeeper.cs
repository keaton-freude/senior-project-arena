using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arena.Utility
{
    /* A syncronous cooldown manager, can be thought to replace Timer */
    public class TimeKeeper
    {
        public Dictionary<string, TimeObject> time_objects;
        private static TimeKeeper _instance;
        public static TimeKeeper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TimeKeeper();
                return _instance;
            }
        }

        private TimeKeeper()
        {
            time_objects = new Dictionary<string, TimeObject>();
        }

        public void Update(float dt)
        {
            foreach (TimeObject to in time_objects.Values)
            {
                to.Update(dt);
            }
        }
    }
}
