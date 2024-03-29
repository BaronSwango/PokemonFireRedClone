﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class ButtonOverlayMenu : OverlayMenu
    {
        private bool inBattle
        {
            get { return BattleLogic.Battle != null && BattleLogic.Battle.InBattle; }
        }

        public int SelectedIndex;
        public bool Switch;

        public override void LoadContent()
        {

            if (Player.PlayerJsonObject.PokemonInBag.Count == 1 && !inBattle && !Background.IsLoaded)
            {
                Background.Path = "Menus/PokemonMenu/OnlyStarterButtonMenu";
                Items[0].PokemonText.Image.Text = "SUMMARY";
                Items[1].PokemonText.Image.Text = "ITEM";
                Items[2].PokemonText.Image.Text = "CANCEL";
                Items.RemoveAt(3);
            }
            
            string path = SelectedIndex == 0 && inBattle ? "Menus/PokemonMenu/StarterBattleButtonMenu" : "Menus/PokemonMenu/BattleButtonMenu";
            if (inBattle)
            {
                if (Background.IsLoaded)
                   Background.ReloadTexture(path);
                else
                    Background.Path = path;
                Items[1].PokemonText.Image.Text = Items[0].PokemonText.Image.Text;
                Items[0].PokemonText.Image.Text = "SHIFT";
                for (int i = 2; i < Items.Count - 1; i++)
                    Items.RemoveAt(i);
            }

            base.LoadContent();

            if (SelectedIndex == 0 && inBattle)
                ItemNumber = 1;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            
        }

        public void Update()
        {
            if (IsOpen)
            {
                if (InputManager.Instance.KeyPressed(Keys.Q))
                    UnloadContent();
                else if (InputManager.Instance.KeyPressed(Keys.S))
                    ItemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    ItemNumber--;
                else if (InputManager.Instance.KeyPressed(Keys.E)) {
                    PokemonMenu.SelectedIndex = SelectedIndex;
                    switch (ItemNumber)
                    {
                        case 0:
                            if (inBattle)
                            {
                                BattleLogic.PlayerShift = true;
                                ScreenManager.Instance.ChangeScreens("BattleScreen");
                            } else
                                ScreenManager.Instance.ChangeScreens("SummaryScreen");
                            
                            break;
                        case 1:
                            if (inBattle)
                                ScreenManager.Instance.ChangeScreens("SummaryScreen");
                            else
                            {
                                if (Player.PlayerJsonObject.PokemonInBag.Count == 1)
                                {
                                    // Item menu
                                }
                                else
                                {
                                    UnloadContent();
                                    Switch = true;
                                }
                            }
                            break;
                        case 2:
                            if (inBattle)
                                UnloadContent();
                            else
                            {
                                if (Player.PlayerJsonObject.PokemonInBag.Count == 1)
                                    UnloadContent();
                                else
                                {
                                    // Item menu
                                }
                            }
                            break;
                        case 3:
                            UnloadContent();
                            break;
                        default:
                            break;
                    }
                }

                int smallestIndex = SelectedIndex == 0 && inBattle ? 1 : 0;
                if (ItemNumber < smallestIndex)
                    ItemNumber = Items.Count - 1;
                else if (ItemNumber > Items.Count - 1)
                    ItemNumber = smallestIndex;

                for (int i = smallestIndex; i < Items.Count; i++)
                {
                    if (i == ItemNumber)
                    {
                        Items[i].PokemonText.Image.IsActive = true;
                        Arrow.Position = new Vector2(Items[i].PokemonText.Position.X - Arrow.SourceRect.Width,
                            Items[i].PokemonText.Position.Y + (Items[i].PokemonText.SourceRect.Height / 4) - 2);
                    }
                    else
                        Items[i].PokemonText.Image.IsActive = false;

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SelectedIndex == 0 && inBattle && IsOpen)
            {
                Background.Draw(spriteBatch);
                Arrow.Draw(spriteBatch);
                for (int i = 1; i < Items.Count; i++)
                    Items[i].PokemonText.Draw(spriteBatch);
            }
            else
                base.Draw(spriteBatch);
        }

    }
}
