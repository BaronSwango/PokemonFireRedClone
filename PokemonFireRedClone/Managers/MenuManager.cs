﻿using System;

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

        private bool isTransitioning;
        private Counter counter;
        private Image PokedexTransitionBox;

        public MenuManager(string MenuName)
        {
            this.MenuName = MenuName;
            Menu = new Menu();
            Menu.OnMenuChange += Menu_OnMenuChange;
        }

        private void Transition(GameTime gameTime)
        {
            if (isTransitioning)
            {
                PokedexTransitionBox.Update(gameTime);
                if (PokedexTransitionBox.Alpha == 1.0f)
                {
                    if (!counter.Finished)
                    {
                        //counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                        counter.Update(gameTime);
                        PokedexTransitionBox.IsActive = false;
                        return;
                    }

                    if (!PokedexTransitionBox.IsActive)
                    {
                        PokedexTransitionBox.IsActive = true;
                    }

                    Menu.ID = "Load/Menus/" + MenuName + ".xml";
                    counter.Reset();
                }
                else if (PokedexTransitionBox.Alpha == 0.0f)
                {
                    PokedexTransitionBox.IsActive = false;
                    PokedexTransitionBox.UnloadContent();
                    isTransitioning = false;
                }
            }    
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
                if (item.Image != null && item.Image.IsLoaded)
                {
                    item.Image.StoreEffects();
                }
                else if (item.PokemonText != null && item.PokemonText.IsLoaded)
                {
                    item.PokemonText.StoreEffects();
                }
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
                    if (item.Image != null && item.Image.IsLoaded)
                        item.Image.RestoreEffects();
                    else if (item.PokemonText != null && item.PokemonText.IsLoaded)
                        item.PokemonText.RestoreEffects();
                }
            }

            Menu.Update(gameTime);
            Transition(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.E) && Menu.Items.Count > 0 && !isTransitioning)
            {
                switch (Menu.Items[Menu.ItemNumber].LinkType)
                {
                    case "Screen":
                        ScreenManager.Instance.ChangeScreens(Menu.Items[Menu.ItemNumber].LinkID);
                        break;
                    case "Menu":
                        MenuName = Menu.Items[Menu.ItemNumber].MenuName;

                        if (Menu.Items[Menu.ItemNumber].MenuName == "PokemonListMenu"
                            || Menu.Items[Menu.ItemNumber].MenuName == "PokemonDetailsMenu")
                        {
                            StartPokedexTransition();
                        }
                        else
                        {
                            Menu.ID = Menu.Items[Menu.ItemNumber].LinkID;
                        }

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
                    if (Menu.PrevMenuName == "PokedexMenu"
                            || Menu.PrevMenuName == "PokemonListMenu")
                    {
                        StartPokedexTransition();
                    }
                    else
                    {
                        Menu.ID = "Load/Menus/" + Menu.PrevMenuName + ".xml";
                    }
                }
                else if (Menu.PrevScreen != null)
                {
                    ScreenManager.Instance.ChangeScreens(Menu.PrevScreen);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Menu.Draw(spriteBatch);

            if (isTransitioning)
            {
                PokedexTransitionBox.Draw(spriteBatch);
            }
        }

        private void LoadPokedexTransitionImage()
        {
            PokedexTransitionBox = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, ((PokedexMenu) Menu).PokedexBackground.SourceRect.Width, ((PokedexMenu)Menu).PokedexBackground.SourceRect.Height)
            };
            Color[] data = new Color[PokedexTransitionBox.Texture.Width * PokedexTransitionBox.Texture.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            PokedexTransitionBox.Texture.SetData(data);
            PokedexTransitionBox.Effects = "FadeEffect";
            PokedexTransitionBox.LoadContent();
            counter = new Counter(400);
            PokedexTransitionBox.Position.Y = 64;
        }

        private void StartPokedexTransition()
        {
            isTransitioning = true;

            if (PokedexTransitionBox == null)
            {
                LoadPokedexTransitionImage();
            }
            else
            {
                PokedexTransitionBox.ReloadTexture();
            }

            PokedexTransitionBox.IsActive = true;
            PokedexTransitionBox.FadeEffect.Increase = true;
            PokedexTransitionBox.Alpha = 0.0f;
            PokedexTransitionBox.FadeEffect.FadeSpeed = 3.5f;
        }

    }
}
