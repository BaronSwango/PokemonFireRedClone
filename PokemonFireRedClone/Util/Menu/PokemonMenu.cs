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

        List<PokemonMenuInfoButton> buttons;
        int prevItemNumber;

        public override void Transition(float alpha)
        {
            base.Transition(alpha);
            foreach (MenuItem item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                    item.Image.FadeEffect.Increase = true;
                else
                    item.Image.FadeEffect.Increase = false;
            }
        }

        void AlignMenuItems(GameTime gameTime)
        {
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.Position;

            Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);

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
            Text[0].Position = new Vector2(Background.Position.X + 92, CancelUnselected.Position.Y + CancelUnselected.SourceRect.Height / 2 - Text[0].SourceRect.Height / 2);
        }

        public override void LoadContent()
        {
            buttons = new List<PokemonMenuInfoButton>();
            Background.LoadContent();

            buttons.Add(new PokemonMenuStarterInfoButton(Player.PlayerJsonObject.PokemonInBag[0]));
            for (int i = 1; i < Player.PlayerJsonObject.PokemonInBag.Count; i++)
                buttons.Add(new PokemonMenuInfoButton(Player.PlayerJsonObject.PokemonInBag[i]));

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].LoadContent();
                Items.Add(new MenuItem());
                Items[i].Image = buttons[i].BackgroundInUse;
            }

            Items.Add(new MenuItem());
            Items[^1].Image = CancelUnselected;

            Text[0].LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            base.UnloadContent();
            Text[0].UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
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
            } else if (InputManager.Instance.KeyPressed(Keys.S))
                ItemNumber++;
            else if (InputManager.Instance.KeyPressed(Keys.W))
                ItemNumber--;

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
                } else
                    Items[i].Image = ItemNumber == i ? CancelSelected : CancelUnselected;
                    
                if (!Items[i].Image.IsLoaded)
                    Items[i].Image.LoadContent();
            }

            foreach (var button in buttons)
            {
                button.Update(gameTime);
            }
            AlignMenuItems(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            base.Draw(spriteBatch);
            foreach (var button in buttons)
                button.Draw(spriteBatch);
            Text[0].Draw(spriteBatch);
        }

    }
}
