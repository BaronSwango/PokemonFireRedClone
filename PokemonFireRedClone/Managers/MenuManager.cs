using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class MenuManager
    {

        public Menu menu;
        public string menuName;
        bool isTransitioning;
        public bool IsLoaded;
        public bool wasLoaded;
        int menuItemNumber;

        void Transition(GameTime gameTime, string linkID)
        {
            if (isTransitioning)
            {
                int oldMenuCount = menu.Items.Count;
                for (int i = 0; i < oldMenuCount; i++)
                {
                    //menu.Items[i].Image.Update(gameTime);
                    //float first = menu.Items[0].Image.Alpha;
                    //float last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if (linkID == menu.Items[menu.ItemNumber].LinkID)
                    {
                        menuName = menu.Items[menu.ItemNumber].MenuName;
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                        isTransitioning = false;
                        break;
                    }
                    //else if (first == 1.0f && last == 1.0f)
                    //{
                        //foreach (MenuItem item in menu.Items)
                            //item.Image.RestoreEffects();
                    //}
                }
            }

        }

        public MenuManager(string menuName)
        {
            this.menuName = menuName;
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
            menuItemNumber = 0;
        }


        void menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            int itemNumber = 0;
            menu.UnloadContent();

            if (menu.GetType().ToString() == "PokemonFireRedClone.SaveMenu")
                itemNumber = 4;

            xmlMenuManager.Type = System.Type.GetType("PokemonFireRedClone." + menuName);
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            menu.ItemNumber = itemNumber;
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
            menu.ItemNumber = menuItemNumber;
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
            IsLoaded = false;
            menuItemNumber = menu.ItemNumber;
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
            if (InputManager.Instance.KeyPressed(Keys.E) && !isTransitioning && menu.Items.Count > 0)
            {
                if (menu.Items[menu.ItemNumber].LinkType == "Screen")
                    ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                else if (menu.Items[menu.ItemNumber].LinkType == "Menu")
                {
                    isTransitioning = true;
                    Transition(gameTime, menu.Items[menu.ItemNumber].LinkID);
                    menu.Transition(1.0f);
                }
                else if (menu.Items[menu.ItemNumber].LinkType == "Yes")
                    menu.Yes();
                else if (menu.Items[menu.ItemNumber].LinkType == "Exit")
                {
                    UnloadContent();
                    ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.CanUpdate = true;
                } else if (menu.Items[menu.ItemNumber].LinkType == "Move")
                {
                    BattleLogic.playerMoveOption = MoveManager.Instance.GetMove(menu.Items[menu.ItemNumber].Image.Text);
                    UnloadContent();
                    ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleAnimations.state = BattleAnimations.BattleState.PLAYER_MOVE;
                    menuName = menu.PrevMenuName;
                    menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                }
                wasLoaded = true;
            }

            if (InputManager.Instance.KeyPressed(Keys.Q) && !isTransitioning && menu.PrevMenuName != null)
            {
                menuName = menu.PrevMenuName;
                menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }


    }
}
