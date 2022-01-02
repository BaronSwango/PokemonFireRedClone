using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    class MenuManager
    {

        public Menu menu;
        string menuName;
        bool isTransitioning;
        public bool IsLoaded;

        void Transition(GameTime gameTime)
        {
            if (isTransitioning)
            {
                int oldMenuCount = menu.Items.Count;
                for (int i = 0; i < oldMenuCount; i++)
                {
                    menu.Items[i].Image.Update(gameTime);
                    float first = menu.Items[0].Image.Alpha;
                    float last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if (first == 0.0f && last == 0.0f)
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                    else if (first == 1.0f && last == 1.0f)
                    {
                        isTransitioning = false;
                        foreach (MenuItem item in menu.Items)
                            item.Image.RestoreEffects();
                    }
                }
            }

        }

        public MenuManager(string menuName)
        {
            //menu = (Menu) Activator.CreateInstance(Type.GetType("PokemonFireRedClone." + menuName));
            this.menuName = menuName;
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
        }


        void menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnloadContent();
            xmlMenuManager.Type = Type.GetType("PokemonFireRedClone." + menuName);
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            menu.OnMenuChange += menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach (MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }


        }
        public void LoadContent(string menuPath)
        {
            if (menuPath != string.Empty)
                menu.ID = menuPath;
            IsLoaded = true;
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
            IsLoaded = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!ScreenManager.Instance.IsTransitioning)
            {
                foreach (MenuItem item in menu.Items)
                    item.Image.RestoreEffects();
            }
  

            if (!isTransitioning)
                menu.Update(gameTime);
            if (InputManager.Instance.KeyPressed(Keys.E) && !isTransitioning)
            {
                if (menu.Items[menu.ItemNumber].LinkType == "Screen")
                    ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                else if (menu.Items[menu.ItemNumber].LinkType == "Menu")
                {
                    isTransitioning = true;
                    menu.Transition(1.0f);
                    foreach (MenuItem item in menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                } else if (menu.Items[menu.ItemNumber].LinkType == "Exit")
                {

                }
            }

            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }


    }
}
