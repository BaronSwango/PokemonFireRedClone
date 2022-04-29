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
        public Image TransitionRect;

        bool isTransitioning;
        bool prevMenuTransition;
        int menuItemNumber;

        void Transition(GameTime gameTime)
        {
            if (menuItemNumber >= -1 && menu.ItemNumber < menu.Items.Count && (menu.Items[menu.ItemNumber].HasTransition || menu.HasTransition) && isTransitioning)
            {
                TransitionRect.Update(gameTime);
                if (TransitionRect.Alpha == 1.0f)
                {
                    menu.Transitioned = false;
                    if (prevMenuTransition)
                    {
                        menuName = menu.PrevMenuName;
                        menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                        if (menu is BattleMenu)
                            menu.ItemNumber = menuItemNumber;
                    }
                    else
                    {
                        menuName = menu.Items[menu.ItemNumber].MenuName;
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                    }
                    TransitionRect.Position = menu.Position;
                    menu.Transitioned = false;
                }
                else if (TransitionRect.Alpha == 0.0f)
                {
                    TransitionRect.IsActive = false;
                    isTransitioning = false;
                    prevMenuTransition = false;
                    menu.Transitioned = true;
                }

            }
            else if (isTransitioning)
            {

                menuName = menu.Items[menu.ItemNumber].MenuName;
                menu.ID = menu.Items[menu.ItemNumber].LinkID;
                isTransitioning = false;
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
            if (menu is BattleMenu)
                menuItemNumber = menu.ItemNumber;
            menu.UnloadContent();
            xmlMenuManager.Type = System.Type.GetType("PokemonFireRedClone." + menuName);
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            if (menu is MoveMenu)
                menu.ItemNumber = menuItemNumber;
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
            createTransitionRect();
            if (menuPath != string.Empty)
                menu.ID = menuPath;
            IsLoaded = true;
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
            TransitionRect.UnloadContent();
            IsLoaded = false;
            if (menu is MoveMenu)
                menuItemNumber = menu.ItemNumber;
        }

        public void Update(GameTime gameTime)
        {
            Transition(gameTime);
            if (!ScreenManager.Instance.IsTransitioning)
            {
                foreach (MenuItem item in menu.Items)
                    item.Image.RestoreEffects();
            }

            menu.Update(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.E) && !isTransitioning && menu.Items.Count > 0)
            {

                switch (menu.Items[menu.ItemNumber].LinkType)
                {
                    case "Screen":
                        ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                        break;
                    case "Menu":
                        isTransitioning = true;
                        menu.Transitioned = false;
                        TransitionRect.Position = menu.Position;
                        TransitionRect.IsActive = true;
                        menu.Transition(1.0f);
                        break;
                    case "Yes":
                        menu.Yes();
                        break;
                    case "Exit":
                        if (!string.IsNullOrEmpty(menu.PrevMenuName))
                        {
                            if (menu.HasTransition)
                            {
                                isTransitioning = true;
                                menu.Transitioned = false;
                                prevMenuTransition = true;
                                TransitionRect.IsActive = true;
                            }
                            else
                            {
                                menuName = menu.PrevMenuName;
                                menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                            }
                        } else { 
                            UnloadContent();
                            ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.CanUpdate = true;
                        }
                        break;
                    case "Move":
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveOption = MoveManager.Instance.GetMove(menu.Items[menu.ItemNumber].Image.Text);
                        UnloadContent();

                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleAnimations.Reset();
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.PlayerMoveUsed = true;
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.StartSequence = true;
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.NextPage = 5;
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).TextBox.IsTransitioning = true;
                        ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleLogic.Update(gameTime, (BattleScreen)ScreenManager.Instance.CurrentScreen);
                        break;
                    default:
                        break;
                }

                wasLoaded = true;
            }

            if (InputManager.Instance.KeyPressed(Keys.Q) && !isTransitioning && menu.PrevMenuName != null && menu.BaseMenu)
            {
                if (menu.HasTransition)
                {
                    isTransitioning = true;
                    menu.Transitioned = false;
                    prevMenuTransition = true;
                    TransitionRect.IsActive = true;
                }
                else
                {
                    menuName = menu.PrevMenuName;
                    menu.ID = "Load/Menus/" + menu.PrevMenuName + ".xml";
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
            if (isTransitioning)
                TransitionRect.Draw(spriteBatch);
        }

        void createTransitionRect()
        {
            TransitionRect = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y)
            };
            TransitionRect.Effects = "FadeEffect";
            TransitionRect.LoadContent();
            Color[] data = new Color[(int)ScreenManager.Instance.Dimensions.X * (int)ScreenManager.Instance.Dimensions.Y];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            TransitionRect.Texture.SetData(data);
            TransitionRect.Alpha = 0;
            TransitionRect.IsActive = true;
            TransitionRect.FadeEffect.Increase = true;
            TransitionRect.FadeEffect.FadeSpeed = 2;
        }

        /*

            MENU TRANSITIONS

             WHEN LOADING MENU WITH TRANSITION:
            - *First check if menu to be loaded has a transition*
            - Fade to black image not closing out previous menu and unloading prev content
            - Load next menu (from screen or other menu) when fade image is completely black
            - Unfade black image when next menu is loaded

             WHEN UNLOADING MENU WITH TRANSITION:
            - Fade to black image not closing out the menu and unloading prev content
            - Unload menu and load new menu/screen when image is completely black
            - Unfade black image when menu is unloaded and new menu/screen is loaded

        */


    }
}
