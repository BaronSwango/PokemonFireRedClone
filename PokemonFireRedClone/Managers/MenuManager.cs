using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class MenuManager
    {

        public Menu Menu;
        public string MenuName;
        public bool IsLoaded;
        public bool WasLoaded;

        public MenuManager(string MenuName)
        {
            this.MenuName = MenuName;
            Menu = new Menu();
            Menu.OnMenuChange += Menu_OnMenuChange;
        }


        private void Menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new();
            Menu.UnloadContent();
            xmlMenuManager.Type = System.Type.GetType("PokemonFireRedClone." + MenuName);
            Menu = xmlMenuManager.Load(Menu.ID);
            Menu.LoadContent();
            Menu.OnMenuChange += Menu_OnMenuChange;
            
            foreach (MenuItem item in Menu.Items)
            {
                if (item.Image != null)
                    item.Image.StoreEffects();
                else if (item.PokemonText != null)
                    item.PokemonText.StoreEffects();
            }
            

        }

        public void LoadContent(string MenuPath)
        {
            if (MenuPath != string.Empty)
                Menu.ID = MenuPath;
            IsLoaded = true;
        }

        public void UnloadContent()
        {
            Menu.UnloadContent();
            IsLoaded = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!ScreenManager.Instance.IsTransitioning)
            {
                foreach (MenuItem item in Menu.Items)
                {
                    if (item.Image != null)
                        item.Image.RestoreEffects();
                    else if (item.PokemonText != null)
                        item.PokemonText.RestoreEffects();
                }
            }

            Menu.Update(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.E) && Menu.Items.Count > 0)
            {

                switch (Menu.Items[Menu.ItemNumber].LinkType)
                {
                    case "Screen":
                        ScreenManager.Instance.ChangeScreens(Menu.Items[Menu.ItemNumber].LinkID);
                        break;
                    case "Menu":
                        MenuName = Menu.Items[Menu.ItemNumber].MenuName;
                        Menu.ID = Menu.Items[Menu.ItemNumber].LinkID;
                        break;
                    case "Yes":
                        Menu.Yes();
                        break;
                    case "Exit":
                        if (!string.IsNullOrEmpty(Menu.PrevMenuName))
                        {
                            MenuName = Menu.PrevMenuName;
                            Menu.ID = "Load/Menus/" + Menu.PrevMenuName + ".xml";
                        } else 
                            UnloadContent();
                        
                        break;
                    case "ExitToScreen":
                        if (Menu.BaseMenu)
                            ScreenManager.Instance.ChangeScreens(Menu.PrevScreen);
                        break;
                    case "Move":
                        if (BattleLogic.Battle.PlayerPokemon.Pokemon.MovePP[MoveManager.Instance.GetMove(Menu.Items[Menu.ItemNumber].PokemonText.Image.Text).Name] == 0)
                        {
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.NextPage = 21;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.IsTransitioning = true;
                        }
                        else
                        {
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveOption =
                                MoveManager.Instance.GetMove(Menu.Items[Menu.ItemNumber].PokemonText.Image.Text);

                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleAssets.Reset();
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveUsed = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.StartSequence = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.NextPage = 5;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.IsTransitioning = true;
                            ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.Update();
                        }

                        UnloadContent();
                        break;
                    case "Run":
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.EscapeWildBattle = true;
                        UnloadContent();
                        break;
                    default:
                        break;
                }

                WasLoaded = true;
            }

            if (InputManager.Instance.KeyPressed(Keys.Q) && Menu.BaseMenu)
            {
                if (Menu.PrevMenuName != null)
                {
                    MenuName = Menu.PrevMenuName;
                    Menu.ID = "Load/Menus/" + Menu.PrevMenuName + ".xml";
                }
                else if (Menu.PrevScreen.Length > 0)
                    ScreenManager.Instance.ChangeScreens(Menu.PrevScreen);
                
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Menu.Draw(spriteBatch);
        }


    }
}
