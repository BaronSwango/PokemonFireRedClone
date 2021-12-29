using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    /*
 * Image with menu interface
 * Move arrow to each item of menu
 * each item of menu denoted by enum value
 * 
 * Allow for description image with text anywhere on screen
 * 
 * 
 */

    public class Menu
    {
        public event EventHandler OnMenuChange;

        public string Type;
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
        int itemNumber;
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

        public void Transition(float alpha)
        {
            foreach(MenuItem item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                    item.Image.FadeEffect.Increase = true;
                else
                    item.Image.FadeEffect.Increase = false;
            }
        }

        //TODO: Figure out a way to center & also allow padding for Y axis
        //TODO: Possibly add padding for X axis
        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;

            if (Type == "TitleMenu")
                dimensions = new Vector2((ScreenManager.Instance.Dimensions.X -
                    dimensions.X) / 2, FromTop);


            if (Type == "MainMenu")
            {
                Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.Position;
                Rectangle playerRect = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.SourceRect;
                Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + playerRect.Width / 2,
                    playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + playerRect.Height / 2);
            }
            foreach (MenuItem item in Items)
            {
                if (Type == "MainMenu") {
                    item.Image.Position = new Vector2(Background.Position.X + 1052, Background.Position.Y + dimensions.Y + 24);
                    if (item.Description != null)
                        item.Description.Position = new Vector2(Background.Position.X + 8, Background.Position.Y + 580);
                }
                else if (Type == "TitleMenu")
                    item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X -
                        item.Image.SourceRect.Width) / 2, dimensions.Y);

                dimensions += new Vector2(item.Image.SourceRect.Width+PaddingX,
                    item.Image.SourceRect.Height+PaddingY);
            }
        }

        public Menu()
        {
            id = string.Empty;
            itemNumber = 0;
            Effects = string.Empty;
            Type = "TitleMenu";
            Items = new List<MenuItem>();
        }
        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            if (Background != null)
                Background.LoadContent();
            if (Arrow != null)
                Arrow.LoadContent();
            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach (string s in split)
                    item.Image.ActivateEffect(s);
                if (item.Description != null)
                    item.Description.LoadContent();
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            if (Background != null)
                Background.UnloadContent();
            if (Arrow != null)
                Arrow.UnloadContent();
            foreach (MenuItem item in Items)
            {
                item.Image.UnloadContent();
                if (item.Description != null)
                    item.Description.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Background != null)
                Background.Update(gameTime);
            if (Type == "TitleMenu" || Type == "MainMenu")
            {
                if (Type == "MainMenu")
                    AlignMenuItems();
                if (InputManager.Instance.KeyPressed(Keys.S))
                    itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    itemNumber--;

                if (itemNumber < 0)
                    itemNumber = Type == "MainMenu" ? Items.Count - 1 : 0;
                else if (itemNumber > Items.Count - 1)
                    itemNumber = Type == "MainMenu" ? 0 : Items.Count - 1;

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i == itemNumber)
                    {
                        Items[i].Image.IsActive = true;
                        if (Type == "MainMenu")
                        {
                            Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width, Items[i].Image.Position.Y + (Items[i].Image.SourceRect.Height / 4));
                            if (Items[i].Description != null)
                                Items[i].Description.IsActive = true;
                        }
                    }
                    else
                    {
                        Items[i].Image.IsActive = false;
                        if (Items[i].Description != null)
                            Items[i].Description.IsActive = false;
                    }
                    
                    Items[i].Image.Update(gameTime);
                    if (Items[i].Description != null)
                        Items[i].Description.Update(gameTime);
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (Background != null)
                Background.Draw(spriteBatch);
            if (Arrow != null)
                Arrow.Draw(spriteBatch);
            foreach (MenuItem item in Items)
            {
                item.Image.Draw(spriteBatch);
                if (item.Description != null)
                {
                    if (item.Description.IsActive)
                        item.Description.Draw(spriteBatch);
                }
            }
        }

    }
}
