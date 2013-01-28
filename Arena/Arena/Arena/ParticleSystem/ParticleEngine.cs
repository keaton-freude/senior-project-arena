using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace ArenaParticleEngine
{
    public class ParticleEngine
    {
        public Dictionary<int, ParticleSystem> systems;
        private static ParticleEngine _instance;

        private static int _system_count;

        /* This was a dirty hack to do some fun things with WinForms */
        //public MainForm main_form_reference;

        public int LoadFromFile(string path, ContentManager content)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            ParticleSystem new_system = new ParticleSystem();
            //new_system.name = doc.SelectSingleNode("ParticleSystem").Attributes["Name"].Value;
            ParticleEffect effect_to_add = null;
            ParticleEmitter emitter_to_add = null;
            //Particle particle_to_add = null;

            string effect_type = "None";
            string texture_polling = "None";
            string BlendState = "None";

            /* Particle Attributes */
            Color StartColor = Color.White;
            Color EndColor = Color.White;
            float StartAlpha = 0.0f;
            float EndAlpha = 0.0f;
            float Rotation = 0.0f;
            float RotationSpeed = 0.0f;
            float ScaleStart = 0.0f;
            float ScaleEnd = 0.0f;
            Vector2 Origin = Vector2.Zero;
            string TextureName = "None";
            float LifeMin = 0.0f;
            float LifeMax = 0.0f;
            //string ParticleName = "None";
            /* End Particle Attributes */

            /*Emitter Attributes */
            Vector2 MaxParticleSpeed = Vector2.Zero;
            Vector2 MinParticleSpeed = Vector2.Zero;
            Int32 MaxParticles = 0;
            Vector2 MinVelocity = Vector2.Zero;
            Vector2 MaxVelocity = Vector2.Zero;
            Int32 ParticlesPerUpdate = 0;
            Vector2 OffsetMin = Vector2.Zero;
            Vector2 OffsetMax = Vector2.Zero;
            Vector2 MinAccel = Vector2.Zero;
            Vector2 MaxAccel = Vector2.Zero;

            int r = 0;
            int g = 0;
            int b = 0;

            List<String> texture_names = new List<String>();
            List<Particle> particles_to_add = new List<Particle>();
            /* System is set up, lets parse out effects */
            foreach (XmlNode node in doc.SelectNodes("ParticleSystem/ParticleEffect"))
            {
                effect_type = node.Attributes["Type"].Value;
                texture_polling = node.Attributes["TexturePolling"].Value;
                BlendState = node.Attributes["BlendState"].Value;

                /* Now lets grab all information so we can create an emitter */
                XmlNode emitter_node = node.SelectSingleNode("ParticleEmitter");
                //MaxParticleSpeed = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[0].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[0].Attributes["Y"].Value));
                //MinParticleSpeed = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[1].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[1].Attributes["Y"].Value));
                MaxParticles = Convert.ToInt32(emitter_node.ChildNodes[0].InnerText);
                MinVelocity = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[1].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[1].Attributes["Y"].Value));
                MaxVelocity = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[2].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[2].Attributes["Y"].Value));
                ParticlesPerUpdate = Convert.ToInt32(emitter_node.ChildNodes[3].InnerText);
                OffsetMin = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[4].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[4].Attributes["Y"].Value));
                OffsetMax = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[5].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[5].Attributes["Y"].Value));
                MinAccel = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[6].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[6].Attributes["Y"].Value));
                MaxAccel = new Vector2((float)Convert.ToDouble(emitter_node.ChildNodes[7].Attributes["X"].Value), (float)Convert.ToDouble(emitter_node.ChildNodes[7].Attributes["Y"].Value));

                /* This is enough info to create an Emitter */
                /* Now let's loop through each particle and at the end, build the particle and add it master particle list */
                foreach (XmlNode particle_node in node.SelectNodes("Particle"))
                {
                    XmlNode StartColorNode = particle_node.ChildNodes[0];
                    r = Convert.ToInt32(StartColorNode.Attributes["R"].Value);
                    g = Convert.ToInt32(StartColorNode.Attributes["G"].Value);
                    b = Convert.ToInt32(StartColorNode.Attributes["B"].Value);
                    StartColor = new Color(r, g, b);
                    XmlNode EndColorNode = particle_node.ChildNodes[1];
                    r = Convert.ToInt32(EndColorNode.Attributes["R"].Value);
                    g = Convert.ToInt32(EndColorNode.Attributes["G"].Value);
                    b = Convert.ToInt32(EndColorNode.Attributes["B"].Value);
                    EndColor = new Color(r, g, b);

                    StartAlpha = (float)Convert.ToDouble(particle_node.ChildNodes[2].InnerText);
                    EndAlpha = (float)Convert.ToDouble(particle_node.ChildNodes[3].InnerText);
                    Rotation = (float)Convert.ToDouble(particle_node.ChildNodes[4].InnerText);
                    RotationSpeed = (float)Convert.ToDouble(particle_node.ChildNodes[5].InnerText);
                    ScaleStart = (float)Convert.ToDouble(particle_node.ChildNodes[6].InnerText);
                    ScaleEnd = (float)Convert.ToDouble(particle_node.ChildNodes[7].InnerText);
                    Origin = new Vector2((float)Convert.ToDouble(particle_node.ChildNodes[8].Attributes["X"].Value), (float)Convert.ToDouble(particle_node.ChildNodes[8].Attributes["Y"].Value));
                    TextureName = particle_node.ChildNodes[9].InnerText;
                    texture_names.Add(TextureName);
                    LifeMin = (float)Convert.ToDouble(particle_node.ChildNodes[10].Attributes["Min"].Value);
                    LifeMax = (float)Convert.ToDouble(particle_node.ChildNodes[10].Attributes["Max"].Value);

                    Particle temp = new Particle();
                    temp.StartColor = StartColor;
                    temp.EndColor = EndColor;
                    temp.StartAlpha = StartAlpha;
                    temp.EndAlpha = EndAlpha;
                    temp.Rotation = Rotation;
                    temp.RotationSpeed = RotationSpeed;
                    temp.ScaleStart = ScaleStart;
                    temp.ScaleEnd = ScaleEnd;
                    temp.Origin = Origin;
                    temp.TextureName = TextureName;
                    temp.MinLife = LifeMin;
                    temp.MaxLife = LifeMax;
                    temp.Name = particle_node.Attributes["Name"].Value;

                    particles_to_add.Add(temp);
                }

                /* We have a complete list of texture names, lets load the textures and store in list */
                List<Texture2D> textures = new List<Texture2D>();
                /* WIN FORMS ONLY (had to use bastard-version of content load */
                //foreach (String name in texture_names)
                //{
                //    textures.Add(main_form_reference.LoadTexture(name));
                //}
                foreach (String name in texture_names)
                {
                    /* Our texture names in the WinForms version require the extension
                     * In XNA, the assets are prebuilt and so the extension is unneeded
                     * So we'll just split the string into two, and take the first part*/
                    textures.Add(content.Load<Texture2D>(name.Split('.')[0]));
                }

                /* We also have enough info to build our emitter */
                emitter_to_add = new ParticleEmitter();
                emitter_to_add.MaxParticles = MaxParticles;
                emitter_to_add.MinVelocity = MinVelocity;
                emitter_to_add.MaxVelocity = MaxVelocity;
                emitter_to_add.ParticlesPerUpdate = ParticlesPerUpdate;
                emitter_to_add.OffsetMin = OffsetMin;
                emitter_to_add.OffsetMax = OffsetMax;
                emitter_to_add.MinimumAcceleration = MinAccel;
                emitter_to_add.MaximumAcceleration = MaxAccel;

                /* We have an emitter and every particle information stored */
                
                
                /* Lets create our final effect to be added to our system */
                if (effect_type == "Continuous")
                {
                    effect_to_add = new ContinuousParticleEffect(textures, emitter_to_add);
                    effect_to_add.MasterParticles = particles_to_add;
                    effect_to_add.BlendingState = BlendState;
                    effect_to_add.TexturePolling = texture_polling;
                    effect_to_add.Emitter = emitter_to_add;
                    new_system.effects.Add(effect_to_add);
                }
                else
                {
                    effect_to_add = new OneShotParticleEffect(textures, emitter_to_add);
                    effect_to_add.MasterParticles = particles_to_add;
                    effect_to_add.BlendingState = BlendState;
                    effect_to_add.TexturePolling = texture_polling;
                    effect_to_add.Emitter = emitter_to_add;
                    new_system.effects.Add(effect_to_add);

                }
                
                systems.Add(_system_count, new_system);
                _system_count++;
                return _system_count - 1;
            }
            return -1;
        }

        //public Texture2D LoadTexture(string fileName)
        //{
        //    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        //    string real_path = Path.Combine(assemblyLocation, "..\\..\\..\\..\\Content\\" + fileName);
        //    string contentPath = Path.GetFullPath(real_path);

        //    // Unload any existing model.
        //    //contentManager.Unload();

        //    // Tell the ContentBuilder what to build.
        //    //contentBuilder.Clear();
        //    contentBuilder.Add(contentPath, fileName.Split('.')[0], null, "TextureProcessor");

        //    // Build this new model data.
        //    string buildError = contentBuilder.Build();

        //    if (string.IsNullOrEmpty(buildError))
        //    {
        //        // If the build succeeded, use the ContentManager to
        //        // load the temporary .xnb file that we just created.
        //        return contentManager.Load<Texture2D>(fileName.Split('.')[0]);


        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        public static ParticleEngine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ParticleEngine();
                return _instance;
            }
        }

        private ParticleEngine()
        {
            systems = new Dictionary<int, ParticleSystem>();
        }

        public void Update(float dt)
        {
            foreach (ParticleSystem system in systems.Values)
            {
                system.Update(dt);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleSystem system in systems.Values)
            {
                system.Draw(spriteBatch);
            }
        }
    }
}
