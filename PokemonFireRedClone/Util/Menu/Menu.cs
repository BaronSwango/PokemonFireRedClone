using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Menu
    {

        private string id;

        public event EventHandler OnMenuChange;

        [XmlIgnore]
        public System.Type Type;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;
        public int PaddingX;
        public int PaddingY;
        public int FromSide; // indent from left side of screen
        public int FromTop; // indent from top of screen
        public int ItemNumber;
        public string PrevMenuName;
        public bool ScreenMenu;
        public string PrevScreen;
        public Vector2 Position;
        public bool BaseMenu;

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
        public virtual void Transition(float alpha) { }

        protected virtual void AlignMenuItems() { }

        public virtual void Yes() { }

        public Menu()
        {
            Type = GetType();
            id = string.Empty;
            ItemNumber = 0;
            Effects = string.Empty;
            Items = new List<MenuItem>();
            BaseMenu = true;
            Position = Vector2.Zero;
        }

        public virtual void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach (MenuItem item in Items)
            {
                if (item.Image != null)
                {
                    item.Image.LoadContent();
                    foreach (string s in split)
                        item.Image.ActivateEffect(s);
                }
                else if (item.PokemonText != null)
                {
                    item.PokemonText.LoadContent();
                    foreach (string s in split)
                        item.PokemonText.ActivateEffect(s);
                }
            }
            AlignMenuItems();
        }

        public virtual void UnloadContent()
        {
            foreach (MenuItem item in Items)
            {
                if (item.Image != null)
                    item.Image.UnloadContent();
                else if (item.PokemonText != null)
                    item.PokemonText.UnloadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (MenuItem item in Items)
            {
                if (item.Image != null)
                    item.Image.Update(gameTime);
                else if (item.PokemonText != null)
                    item.PokemonText.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
            {
                if (item.Image != null)
                    item.Image.Draw(spriteBatch);
                else if (item.PokemonText != null)
                    item.PokemonText.Draw(spriteBatch);
            }
        }

    }
}

