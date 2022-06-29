using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class MenuManager
    {

        public Menu menu;
        public string menuName;
        public bool IsLoaded;
        public bool wasLoaded;

        public MenuManager(string menuName)
        {
            this.menuName = menuName;
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
        }


        void menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnloadContent();
            xmlMenuManager.Type = System.Type.GetType("PokemonFireRedClone." + menuName);
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

            menu.Update(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.E) && menu.Items.Count > 0)
            {

                switch (menu.Items[menu.ItemNumber].LinkType)
                {
                    case "Screen":
                        ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                        break;
                    case "Menu":
                        menuName = menu.Items[menu.ItemNumber].MenuName;
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                        menu.Transition(1.0f);
                        break;
                    case "Yes":
                        menu.Yes();
                        break;
                    case "Exit":
                        if (!string.IsNullOrEmpty(menu.PrevMenuName))
                        {
                            menuName = menu.PrevMenuName;
                            menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                        } else 
                            UnloadContent();
                        
                        break;
                    case "ExitToScreen":
                        ScreenManager.Instance.ChangeScreens(menu.PrevScreen);
                        break;
                    case "Move":
                        if (BattleLogic.Battle.PlayerPokemon.Pokemon.MoveNames[MoveManager.Instance.GetMove(menu.Items[menu.ItemNumber].Image.Text).Name] == 0)
                        {
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.NextPage = 21;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.IsTransitioning = true;
                        }
                        else
                        {
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveOption = MoveManager.Instance.GetMove(menu.Items[menu.ItemNumber].Image.Text);

                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleAssets.Reset();
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveUsed = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.StartSequence = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.NextPage = 5;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.IsTransitioning = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.Update(gameTime);
                        }

                        UnloadContent();
                        break;
                    default:
                        break;
                }

                wasLoaded = true;
            }

            if (InputManager.Instance.KeyPressed(Keys.Q) && menu.BaseMenu)
            {
                if (menu.PrevMenuName != null)
                {
                    menuName = menu.PrevMenuName;
                    menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                }
                else if (menu.ScreenMenu)
                    ScreenManager.Instance.ChangeScreens(menu.PrevScreen);
                
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }


    }
}
