using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PokemonFireRedClone
{
    public class MoveMenu : Menu
    {

        public Image Arrow;
        public Image Background;

        protected override void AlignMenuItems()
        {
            Background.Position = new Vector2(0, ScreenManager.Instance.Dimensions.Y - Background.SourceRect.Height);

            Items[0].Image.Position = new Vector2(Background.Position.X + 128, Background.Position.Y + 52);
            Items[1].Image.Position = new Vector2(Items[0].Image.Position.X + 300, Items[0].Image.Position.Y);
            Items[2].Image.Position = new Vector2(Items[0].Image.Position.X, Items[0].Image.Position.Y + 64);
            Items[3].Image.Position = new Vector2(Items[1].Image.Position.X, Items[2].Image.Position.Y);

            foreach (MenuItem item in Items)
            {
                item.Description[0].Position = new Vector2(900, Background.Position.Y + 56);
                item.Description[1].Position = new Vector2(item.Description[0].Position.X + item.Description[0].SourceRect.Width + 160, item.Description[0].Position.Y - 20);
                item.Description[2].Position = new Vector2(900, item.Description[0].Position.Y + item.Description[0].SourceRect.Height + 36);
                item.Description[3].Position = new Vector2(item.Description[2].Position.X + item.Description[2].SourceRect.Width, item.Description[2].Position.Y - 20);
            }


        }

        public override void LoadContent()
        {
            Background.LoadContent();
            Arrow.LoadContent();
            Arrow.Position = new Vector2(-Arrow.SourceRect.Width, 0);

            for (int i = 0; i < Player.PlayerJsonObject.PokemonInBag[0].MoveNames.Count; i++)
            {
                string moveName = Player.PlayerJsonObject.PokemonInBag[0].MoveNames.Keys.ElementAt(i);
                Items[i].Image.Text = moveName.ToUpper();

                Items[i].Description[1].Text = Player.PlayerJsonObject.PokemonInBag[0].MoveNames[moveName] + "/" + MoveManager.Instance.GetMove(moveName).PP;
                Items[i].Description[3].Text = MoveManager.Instance.GetMove(moveName).TypeName.ToUpper();
            }

            foreach (MenuItem item in Items)
            {
                foreach (Image image in item.Description)
                    image.LoadContent();
            }

            base.LoadContent();
            AlignMenuItems();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            Arrow.UnloadContent();

            foreach (MenuItem item in Items)
            {
                foreach (Image image in item.Description)
                    image.UnloadContent();
            }

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.A) && ItemNumber - 1 >= 0 && !Items[ItemNumber - 1].Image.Text.Contains("-"))
            {
                if (ItemNumber == 1 || ItemNumber == 3)
                    ItemNumber--;
            }
            else if (InputManager.Instance.KeyPressed(Keys.D) && ItemNumber + 1 <= Items.Count - 1 && !Items[ItemNumber + 1].Image.Text.Contains("-"))
            {
                if (ItemNumber == 0 || ItemNumber == 2)
                    ItemNumber++;
            }

            else if (InputManager.Instance.KeyPressed(Keys.W) && ItemNumber - 2 >= 0 && !Items[ItemNumber - 2].Image.Text.Contains("-"))
            {
                if (ItemNumber == 2 || ItemNumber == 3)
                    ItemNumber -= 2;
            }
            else if (InputManager.Instance.KeyPressed(Keys.S) && ItemNumber + 2 <= Items.Count - 1 && !Items[ItemNumber + 2].Image.Text.Contains("-"))
            {
                if (ItemNumber == 0 || ItemNumber == 1)
                    ItemNumber += 2;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[i].Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width - 4,
                        Items[i].Image.Position.Y - 4);
                    foreach (Image image in Items[i].Description)
                        image.IsActive = true;
                }
                else
                {
                    Items[i].Image.IsActive = false;
                    foreach (Image image in Items[i].Description)
                        image.IsActive = false;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);
            foreach (MenuItem item in Items)
            {
                foreach (Image image in item.Description)
                {
                if (image.IsActive)
                    image.Draw(spriteBatch);
                }

            }
            base.Draw(spriteBatch);
        }
    }

}

