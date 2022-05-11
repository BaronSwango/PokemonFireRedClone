using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class PokemonMenu : Menu
    {
        public Image Background;
        [XmlElement("Text")]
        public List<Image> Text;
        public Image CancelSelected;
        public Image CancelUnselected;
        public ButtonOverlayMenu ButtonMenu;

        bool positioned;

        List<PokemonMenuInfoButton> buttons;
        int prevItemNumber;

        bool inBattle
        {
            get { return ScreenManager.Instance.PreviousScreen is BattleScreen; }
            set { }
        }

        protected override void AlignMenuItems()
        {
            if (ScreenManager.Instance.CurrentScreen is GameplayScreen screen)
            {
                Vector2 playerPos = screen.player.Image.Position;

                Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                    playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);
                Position = new Vector2(Background.Position.X, Background.Position.Y + 2);
            }
        }

        void AlignMenuItems(GameTime gameTime)
        {

            Items[0].Image.Position = new Vector2(Background.Position.X + 100, Background.Position.Y + 116);

            float dimensionY = Background.Position.Y + 52;

            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Image.Position = new Vector2(Items[0].Image.Position.X + Items[0].Image.SourceRect.Width + 148, dimensionY);
                dimensionY += Items[i].Image.SourceRect.Height + 16;
            }

            foreach (var button in buttons)
                button.UpdateInfoPositions(gameTime);

            CancelUnselected.Position = new Vector2(Background.Position.X + Background.SourceRect.Width - CancelUnselected.SourceRect.Width - 96,
                Background.Position.Y + Background.SourceRect.Height - CancelUnselected.SourceRect.Height - 28);
            CancelSelected.Position = new Vector2(CancelUnselected.Position.X, CancelUnselected.Position.Y - 8);
            foreach (Image text in Text)
                text.Position = new Vector2(Background.Position.X + 92, CancelUnselected.Position.Y + CancelUnselected.SourceRect.Height / 2 - Text[0].SourceRect.Height / 2);
            positioned = true;
        }

        public override void LoadContent()
        {
            ScreenMenu = true;

            buttons = new List<PokemonMenuInfoButton>();
            Background.LoadContent();

            List<CustomPokemon> menuList = inBattle ? BattleLogic.Battle.BattlePokemonMenu : Player.PlayerJsonObject.PokemonInBag;

            buttons.Add(new PokemonMenuStarterInfoButton(menuList[0]));
            for (int i = 1; i < Player.PlayerJsonObject.PokemonInBag.Count; i++)
                buttons.Add(new PokemonMenuInfoButton(menuList[i]));

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].LoadContent();
                Items.Add(new MenuItem());
                Items[i].Image = buttons[i].BackgroundInUse;
            }

            Items.Add(new MenuItem());
            Items[^1].LinkType = "ExitToScreen";
            Items[^1].Image = CancelUnselected;

            foreach (Image i in Text)
                i.LoadContent();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            base.UnloadContent();
            foreach (Image i in Text)
                i.UnloadContent();
            if (ButtonMenu.IsOpen)
                ButtonMenu.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!ButtonMenu.IsOpen)
            {
                if (!BaseMenu)
                    BaseMenu = true;

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
                        ItemNumber = prevItemNumber;
                    else if (ItemNumber == 0)
                        ItemNumber = 1;
                }
                else if (InputManager.Instance.KeyPressed(Keys.S))
                    ItemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    ItemNumber--;
                else if (InputManager.Instance.KeyPressed(Keys.E))
                {
                    if (ItemNumber < Items.Count - 1)
                    {
                        ButtonMenu.ShiftNumber = ItemNumber;
                        ButtonMenu.LoadContent();
                        ButtonMenu.AlignMenuItems(new Vector2(Background.Position.X + Background.SourceRect.Width - ButtonMenu.Background.SourceRect.Width - 12,
                            Text[1].Position.Y + Text[1].SourceRect.Height - ButtonMenu.Background.SourceRect.Height));
                        BaseMenu = false;
                    }
                }

                if (ItemNumber < 0)
                    ItemNumber = Items.Count - 1;
                else if (ItemNumber > Items.Count - 1)
                    ItemNumber = 0;

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i < buttons.Count)
                    {
                        buttons[i].State = ItemNumber == i ? PokemonMenuInfoButton.ButtonState.SELECTED : PokemonMenuInfoButton.ButtonState.UNSELECTED;
                        Items[i].Image = buttons[i].BackgroundInUse;
                    }
                    else
                        Items[i].Image = ItemNumber == i ? CancelSelected : CancelUnselected;

                    if (!Items[i].Image.IsLoaded)
                        Items[i].Image.LoadContent();
                }

            }
            else
                ButtonMenu.Update();

            foreach (var button in buttons)
                button.Update(gameTime);
            
            AlignMenuItems(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (positioned)
            {
                Background.Draw(spriteBatch);
                base.Draw(spriteBatch);
                foreach (var button in buttons)
                    button.Draw(spriteBatch);
                if (ButtonMenu.IsOpen)
                {
                    Text[1].Draw(spriteBatch);
                    ButtonMenu.Draw(spriteBatch);
                }
                else
                    Text[0].Draw(spriteBatch);
            }
        }
    }
}
