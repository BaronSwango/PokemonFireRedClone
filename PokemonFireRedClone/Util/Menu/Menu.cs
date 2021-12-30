using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone.Util.Menu
{
    public class Menu
    {

        public event EventHandler OnMenuChange;

        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;
        public Image Arrow;
        public Image Background;
        public int PaddingX;
        public int PaddingY;
        public bool CenterX;
        public bool CenterY;
        public int FromSide; // indent from left side of screen
        public int FromTop; // indent from top of screen

        public Menu()
        {
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
