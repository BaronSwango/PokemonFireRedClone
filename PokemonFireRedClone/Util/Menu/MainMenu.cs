using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class MainMenu : Menu
    {

        private static int itemNumber;

        public Image Arrow;
        public Image Background;

        protected override void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player.Image.Position;

            Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2)+32,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2)+44);
            Position = new Vector2(Background.Position.X, Background.Position.Y - 2);

            foreach (MenuItem item in Items)
            {
                item.Image.Position = new Vector2(Background.Position.X + 1052, Background.Position.Y + dimensions.Y + 24);
                item.Description[0].Position = new Vector2(Background.Position.X + 8, Background.Position.Y + 580);

                dimensions += new Vector2(item.Image.SourceRect.Width + PaddingX,
                    item.Image.SourceRect.Height + PaddingY);
            }
        }

        public override void LoadContent()
        {
            Background.LoadContent();
            Arrow.LoadContent();
            foreach (MenuItem item in Items)
            {
                if (item.Image.Text == "PlayerName")
                    item.Image.Text = Player.PlayerJsonObject.Name;

                item.Description[0].LoadContent();
            }
            base.LoadContent();
            AlignMenuItems();
            ItemNumber = itemNumber;
        }

        public override void UnloadContent()
        {
            itemNumber = ItemNumber;
            Background.UnloadContent();
            Arrow.UnloadContent();
            foreach (MenuItem item in Items)
                item.Description[0].UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.S))
                ItemNumber++;
            else if (InputManager.Instance.KeyPressed(Keys.W))
                ItemNumber--;
            

            if (ItemNumber < 0)
                ItemNumber = Items.Count - 1;
            else if (ItemNumber > Items.Count - 1)
                ItemNumber = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[i].Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width,
                        Items[i].Image.Position.Y + (Items[i].Image.SourceRect.Height / 4)-2);
                    Items[i].Description[0].IsActive = true;
                        
                }
                else
                {
                    Items[i].Image.IsActive = false;
                    Items[i].Description[0].IsActive = false;
                }

                Items[i].Description[0].Update(gameTime); 
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);
            foreach (MenuItem item in Items)
            {
                if (item.Description[0].IsActive)
                    item.Description[0].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

    }
}
