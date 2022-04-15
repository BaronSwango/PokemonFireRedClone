using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class PokemonMenu : Menu
    {
        public Image Background;
        List<PokemonMenuInfoButton> buttons;

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

        protected override void AlignMenuItems()
        {
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.Position;

            Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);

            Items[0].Image.Position = new Vector2(Background.Position.X + 100, Background.Position.Y + 116);

            float dimensionY = Background.Position.Y + 72;

            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Image.Position = new Vector2(Items[0].Image.Position.X + Items[0].Image.SourceRect.Width + 148, dimensionY);
                dimensionY += Items[i].Image.SourceRect.Height + 24;
            }

            foreach (var button in buttons)
                button.UpdateInfoPositions();

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
                Items[i].Image = buttons[i].BackgroundUnselected;
            }

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            /*
            if (InputManager.Instance.KeyPressed(Keys.H))
            {
                Items[0].Image.ReloadTexture("Menus/PokemonMenu/");
            }
            */

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            base.Draw(spriteBatch);
            foreach (var button in buttons)
                button.Draw(spriteBatch);
        }
    }
}
