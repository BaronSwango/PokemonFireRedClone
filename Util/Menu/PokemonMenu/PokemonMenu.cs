using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class PokemonMenu : Menu
    {

        private bool positioned;
        private List<PokemonMenuInfoButton> buttons;
        private List<Image> emptyButtons;
        private int prevItemNumber;

        private bool switchState;
        private int originalSwitchIndex;
        private bool isSwitching;
        private bool switchedButtons;

        public Image Background;
        [XmlElement("Text")]
        public List<Image> Text;
        public Image CancelSelected;
        public Image CancelUnselected;
        public ButtonOverlayMenu ButtonMenu;

        // index of pokemon being switched to
        public static int SelectedIndex;


        void AlignMenuItems(GameTime gameTime)
        {

            Items[0].Image.Position = new Vector2(Background.Position.X + 100, Background.Position.Y + 116);

            float originalX = Items[0].Image.Position.X + Items[0].Image.SourceRect.Width + 148;
            float dimensionY = Background.Position.Y + 52;

            for (int i = 1; i < Items.Count - 1; i++)
            {
                Items[i].Image.Position = new Vector2(originalX, dimensionY);
                dimensionY += Items[i].Image.SourceRect.Height + 16;
            }

            foreach (Image i in emptyButtons)
            {
                i.Position = new Vector2(originalX, dimensionY);
                dimensionY += i.SourceRect.Height + 16;
            }

            foreach (var button in buttons)
            {
                button.UpdateInfoPositions(gameTime);
            }

            CancelUnselected.Position = new Vector2(Background.Position.X + Background.SourceRect.Width - CancelUnselected.SourceRect.Width - 96,
                Background.Position.Y + Background.SourceRect.Height - CancelUnselected.SourceRect.Height - 28);
            CancelSelected.Position = new Vector2(CancelUnselected.Position.X, CancelUnselected.Position.Y - 8);

            foreach (Image text in Text)
            {
                text.Position = new Vector2(Background.Position.X + 92, CancelUnselected.Position.Y + CancelUnselected.SourceRect.Height / 2 - Text[0].SourceRect.Height / 2);
            }

            if (ScreenManager.Instance.PreviousScreen is SummaryScreen && !positioned)
            {
                OpenButtonBenu();
            }

            positioned = true;
        }

        public override void LoadContent()
        {
            PrevScreen = BattleLogic.Battle != null && BattleLogic.Battle.InBattle ? "BattleScreen" : "GameplayScreen";

            buttons = new List<PokemonMenuInfoButton>();
            emptyButtons = new List<Image>();
            Background.LoadContent();

            List<CustomPokemon> menuList = BattleLogic.Battle != null && BattleLogic.Battle.InBattle ? BattleLogic.Battle.BattlePokemonInBag : Player.PlayerJsonObject.PokemonInBag;

            buttons.Add(new PokemonMenuStarterInfoButton(menuList[0]));

            for (int i = 1; i < Player.PlayerJsonObject.PokemonInBag.Count; i++)
            {
                buttons.Add(new PokemonMenuInfoButton(menuList[i]));
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].LoadContent();
                Items.Add(new MenuItem());
                Items[i].Image = buttons[i].BackgroundInUse;
            }

            for (int i = buttons.Count; i < 6; i++)
            {
                emptyButtons.Add(new Image { Path = "Menus/PokemonMenu/EmptyButton" });
                emptyButtons[i - buttons.Count].LoadContent();
            }

            Items.Add(new MenuItem());
            Items[^1].LinkType = "ExitToScreen";
            Items[^1].Image = CancelUnselected;

            foreach (Image i in Text)
            {
                i.LoadContent();
            }

            base.LoadContent();

            if (ScreenManager.Instance.PreviousScreen is SummaryScreen)
                ItemNumber = SelectedIndex;
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            base.UnloadContent();

            foreach (Image i in Text)
            {
                i.UnloadContent();
            }

            if (ButtonMenu.IsOpen)
            {
                ButtonMenu.UnloadContent();
            }

            foreach (Image i in emptyButtons)
            {
                i.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in buttons)
            {
                button.Update(gameTime);
            }

            if (!ButtonMenu.IsOpen)
            {
                if (isSwitching)
                {
                    AnimateSwitchingButtons(gameTime);
                    return;
                }

                if (!BaseMenu && !switchState)
                {

                    if ((BattleLogic.Battle == null || !BattleLogic.Battle.InBattle) && ButtonMenu.Switch)
                    {
                        switchState = true;
                        originalSwitchIndex = SelectedIndex;
                        ButtonMenu.Switch = false;
                    }
                    else
                    {
                        BaseMenu = true;
                    }
                }

                if (InputManager.Instance.KeyPressed(Keys.A))
                {
                    if (ItemNumber > 0 && ItemNumber < buttons.Count)
                    {
                        prevItemNumber = ItemNumber;
                        ItemNumber = 0;
                    }
                }
                else if (InputManager.Instance.KeyPressed(Keys.D))
                {
                    if (prevItemNumber > 0 && ItemNumber == 0)
                    {
                        ItemNumber = prevItemNumber;
                    }
                    else if (ItemNumber == 0 && buttons.Count > 1)
                    {
                        ItemNumber = 1;
                    }
                }
                else if (InputManager.Instance.KeyPressed(Keys.S))
                {
                    ItemNumber++;
                }
                else if (InputManager.Instance.KeyPressed(Keys.W))
                {
                    ItemNumber--;
                }
                else if (InputManager.Instance.KeyPressed(Keys.E))
                {

                    if (switchState)
                    {
                        if (ItemNumber != originalSwitchIndex && ItemNumber != Items.Count - 1)
                        {
                            CustomPokemon temp = Player.PlayerJsonObject.PokemonInBag[originalSwitchIndex];
                            Player.PlayerJsonObject.PokemonInBag[originalSwitchIndex] = Player.PlayerJsonObject.PokemonInBag[ItemNumber];
                            Player.PlayerJsonObject.PokemonInBag[ItemNumber] = temp;
                            isSwitching = true;
                        }

                        switchState = false;
                    }
                    else if (ItemNumber < Items.Count - 1)
                    {
                        OpenButtonBenu();
                    }
                }
                else if (InputManager.Instance.KeyPressed(Keys.Q) && switchState)
                {
                    switchState = false;
                }

                if (ItemNumber < 0)
                {
                    ItemNumber = Items.Count - 1;
                }
                else if (ItemNumber > Items.Count - 1)
                {
                    ItemNumber = 0;
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i < buttons.Count)
                    {
                        if (switchState || isSwitching)
                        {
                            if (ItemNumber == i)
                            {
                                buttons[i].State = isSwitching ? PokemonMenuInfoButton.ButtonState.SWITCH_ORIGINAL : PokemonMenuInfoButton.ButtonState.SWITCH_SELECTED;
                            }
                            else
                            {
                                buttons[i].State = i == originalSwitchIndex ? PokemonMenuInfoButton.ButtonState.SWITCH_ORIGINAL : PokemonMenuInfoButton.ButtonState.UNSELECTED;
                            }
                        }
                        else
                        {
                            buttons[i].State = ItemNumber == i ? PokemonMenuInfoButton.ButtonState.SELECTED : PokemonMenuInfoButton.ButtonState.UNSELECTED;
                        }
                        Items[i].Image = buttons[i].BackgroundInUse;
                    }
                    else
                    {
                        Items[i].Image = ItemNumber == i ? CancelSelected : CancelUnselected;
                    }

                    if (!Items[i].Image.IsLoaded)
                    {
                        Items[i].Image.LoadContent();
                    }
                }
            }
            else
            {
                ButtonMenu.Update();
            }
            
            AlignMenuItems(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (positioned)
            {
                Background.Draw(spriteBatch);
                base.Draw(spriteBatch);

                foreach (var button in buttons)
                {
                    button.Draw(spriteBatch);
                }

                foreach (Image i in emptyButtons)
                {
                    i.Draw(spriteBatch);
                }

                if (ButtonMenu.IsOpen)
                {
                    Text[1].Draw(spriteBatch);
                    ButtonMenu.Draw(spriteBatch);
                }
                else if (ButtonMenu.Switch || switchState || isSwitching)
                {
                    Text[2].Draw(spriteBatch);
                }
                else
                {
                    Text[0].Draw(spriteBatch);
                }
            }
        }

        private void OpenButtonBenu()
        {
            ButtonMenu.SelectedIndex = ItemNumber;
            ButtonMenu.LoadContent();
            ButtonMenu.AlignMenuItems(new Vector2(Background.Position.X + Background.SourceRect.Width - ButtonMenu.Background.SourceRect.Width - 12,
                Text[1].Position.Y + Text[1].SourceRect.Height - ButtonMenu.Background.SourceRect.Height));
            BaseMenu = false;
        }

        private void ResetButtons(GameTime gameTime)
        {
            List<CustomPokemon> menuList = Player.PlayerJsonObject.PokemonInBag; // never gets called in a battle
            float goalX = buttons[originalSwitchIndex] is PokemonMenuStarterInfoButton ?
                584 + buttons[ItemNumber].BackgroundInUse.SourceRect.Width + 300 : 584 + buttons[originalSwitchIndex].BackgroundInUse.SourceRect.Width + 300;
            float originalButtonY = buttons[originalSwitchIndex].BackgroundInUse.Position.Y;
            float newButtonY = buttons[ItemNumber].BackgroundInUse.Position.Y;

            float originalButtonX = buttons[originalSwitchIndex].BackgroundInUse.Position.X;
            float newButtonX = buttons[ItemNumber].BackgroundInUse.Position.X;

            buttons[originalSwitchIndex].UnloadContent();
            buttons[ItemNumber].UnloadContent();

            buttons[originalSwitchIndex] = buttons[originalSwitchIndex] is PokemonMenuStarterInfoButton ?
                new PokemonMenuStarterInfoButton(menuList[originalSwitchIndex]) : new PokemonMenuInfoButton(menuList[originalSwitchIndex]);
            buttons[ItemNumber] = buttons[ItemNumber] is PokemonMenuStarterInfoButton ?
                new PokemonMenuStarterInfoButton(menuList[ItemNumber]) : new PokemonMenuInfoButton(menuList[ItemNumber]);

            buttons[originalSwitchIndex].LoadContent();
            buttons[ItemNumber].LoadContent();

            buttons[originalSwitchIndex].State = PokemonMenuInfoButton.ButtonState.SWITCH_ORIGINAL;
            buttons[ItemNumber].State = PokemonMenuInfoButton.ButtonState.SWITCH_ORIGINAL;
            Items[originalSwitchIndex].Image = buttons[originalSwitchIndex].BackgroundInUse;
            Items[ItemNumber].Image = buttons[ItemNumber].BackgroundInUse;
            Items[originalSwitchIndex].Image.LoadContent();
            Items[ItemNumber].Image.LoadContent();

            buttons[originalSwitchIndex].BackgroundInUse.Position.X = buttons[originalSwitchIndex] is PokemonMenuStarterInfoButton ? originalButtonX : goalX;
            buttons[originalSwitchIndex].BackgroundInUse.Position.Y = originalButtonY;
            buttons[ItemNumber].BackgroundInUse.Position.X = buttons[ItemNumber] is PokemonMenuStarterInfoButton ? newButtonX : goalX;
            buttons[ItemNumber].BackgroundInUse.Position.Y = newButtonY;

            buttons[originalSwitchIndex].UpdateInfoPositions(gameTime);
            buttons[ItemNumber].UpdateInfoPositions(gameTime);

            switchedButtons = true;
        }

        private void AnimateSwitchingButtons(GameTime gameTime)
        {
            const float originalX = 584;
            int index = buttons[originalSwitchIndex] is PokemonMenuStarterInfoButton ? ItemNumber : originalSwitchIndex;
            float goalX = originalX + buttons[index].BackgroundInUse.SourceRect.Width + 300;
            int speed = (int)(2.0f * gameTime.ElapsedGameTime.TotalMilliseconds);
            int originalButtonSpeed = buttons[originalSwitchIndex] is PokemonMenuStarterInfoButton ? -speed : speed;
            int newButtonSpeed = buttons[ItemNumber] is PokemonMenuStarterInfoButton ? -speed : speed;

            if (buttons[index].BackgroundInUse.Position.X < goalX && !switchedButtons)
            {
                buttons[originalSwitchIndex].OffsetX(originalButtonSpeed, gameTime);
                buttons[ItemNumber].OffsetX(newButtonSpeed, gameTime);

                if (buttons[index].BackgroundInUse.Position.X >= goalX)
                {
                    ResetButtons(gameTime);
                }

            }
            else if (buttons[index].BackgroundInUse.Position.X > originalX && switchedButtons)
            {
                buttons[originalSwitchIndex].OffsetX(-originalButtonSpeed, gameTime);
                buttons[ItemNumber].OffsetX(-newButtonSpeed, gameTime);

                if (buttons[index].BackgroundInUse.Position.X - speed <= originalX)
                {
                    buttons[index].BackgroundInUse.Position.X = originalX;
                    isSwitching = false;
                    switchedButtons = false;
                }
            }
        }

    }
}
