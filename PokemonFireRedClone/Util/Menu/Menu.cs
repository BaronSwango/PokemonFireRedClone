using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Menu
    {

        public event EventHandler OnMenuChange;

        [XmlIgnore]
        public Type Type;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;
        public int PaddingX;
        public int PaddingY;
        public int FromSide; // indent from left side of screen
        public int FromTop; // indent from top of screen
        protected int itemNumber;
        string id;

        public int ItemNumber
        {
            get { return itemNumber; }
        }

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        // transitioning from menu to screen or menu to other menu through link ID
        public virtual void Transition(float alpha)
        {
            //TODO: Add to TitleMenu
            //foreach (MenuItem item in Items)
            //{
                //item.Image.IsActive = true;
                //item.Image.Alpha = alpha;
                //if (alpha == 0.0f)
                    //item.Image.FadeEffect.Increase = true;
                //else
                    //item.Image.FadeEffect.Increase = false;
            //}
        }

        protected virtual void AlignMenuItems() { }

        public Menu()
        {
            Type = this.GetType();
            id = string.Empty;
            itemNumber = 0;
            Effects = string.Empty;
            Items = new List<MenuItem>();
        }

        public virtual void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach (string s in split)
                    item.Image.ActivateEffect(s);
            }
            AlignMenuItems();
        }

        public virtual void UnloadContent()
        {
            foreach (MenuItem item in Items)
                item.Image.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (MenuItem item in Items)
                item.Image.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
                item.Image.Draw(spriteBatch);
        }

    }
}
