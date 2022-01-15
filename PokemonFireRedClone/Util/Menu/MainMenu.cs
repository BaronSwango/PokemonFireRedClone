using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class MainMenu : Menu
    {
        public Image Arrow;
        public Image Background;

        protected override void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.Position;

            Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2)+32,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2)+44);

            foreach (MenuItem item in Items)
            {
                item.Image.Position = new Vector2(Background.Position.X + 1052, Background.Position.Y + dimensions.Y + 24);
                item.Description.Position = new Vector2(Background.Position.X + 8, Background.Position.Y + 580);

                dimensions += new Vector2(item.Image.SourceRect.Width + PaddingX,
                    item.Image.SourceRect.Height + PaddingY);
            }
        }

        public override void LoadContent()
        {
            Background.LoadContent();
            Arrow.LoadContent();
            foreach (MenuItem item in Items)
                item.Description.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            Arrow.UnloadContent();
            foreach (MenuItem item in Items)
                item.Description.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            AlignMenuItems();
            if (InputManager.Instance.KeyPressed(Keys.S))
                itemNumber++;
            else if (InputManager.Instance.KeyPressed(Keys.W))
                itemNumber--;

            if (itemNumber < 0)
                itemNumber = Items.Count - 1;
            else if (itemNumber > Items.Count - 1)
                itemNumber = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == itemNumber)
                {
                    Items[i].Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width,
                        Items[i].Image.Position.Y + (Items[i].Image.SourceRect.Height / 4));
                    Items[i].Description.IsActive = true;
                        
                }
                else
                {
                    Items[i].Image.IsActive = false;
                    Items[i].Description.IsActive = false;
                }

                Items[i].Description.Update(gameTime); 
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);
            foreach (MenuItem item in Items)
            {
                if (item.Description.IsActive)
                    item.Description.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

    }
}
