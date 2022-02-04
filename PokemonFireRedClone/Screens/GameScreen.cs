using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PokemonFireRedClone
{
    public class GameScreen
    {
        protected ContentManager content;
        [XmlIgnore]
        public System.Type Type;

        public string XmlPath;

        public GameScreen()
        {
            Type = GetType();
            XmlPath = "Load/" + Type.ToString().Replace("PokemonFireRedClone.", "")+ ".xml";
        }
        public virtual void LoadContent()
        {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
