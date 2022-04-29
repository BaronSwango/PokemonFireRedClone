using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleMenu : Menu
    {

        public Image Arrow;
        public Image Background;

        // STORE ITEM NUMBER SO FADE ANIMATION WORKS
        static int itemNumber;

        protected override void AlignMenuItems()
        {
            Background.Position = new Vector2(ScreenManager.Instance.Dimensions.X - Background.SourceRect.Width,
                ScreenManager.Instance.Dimensions.Y - Background.SourceRect.Height);

            Items[0].Image.Position = new Vector2(Background.Position.X + 64, Background.Position.Y + 36);
            Items[1].Image.Position = new Vector2(Items[0].Image.Position.X, Items[0].Image.Position.Y + 64);
            Items[2].Image.Position = new Vector2(Items[0].Image.Position.X + 224, Items[0].Image.Position.Y);
            Items[3].Image.Position = new Vector2(Items[2].Image.Position.X, Items[1].Image.Position.Y);

        }

        public override void LoadContent()
        {
            Background.LoadContent();
            Arrow.LoadContent();
            Arrow.Position = new Vector2(-Arrow.SourceRect.Width, 0);
            base.LoadContent();
            AlignMenuItems();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            Arrow.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Background.Update(gameTime);

            if (Transitioned)
            {
                if (InputManager.Instance.KeyPressed(Keys.W))
                {
                    if (ItemNumber == 1 || ItemNumber == 3)
                        ItemNumber--;
                }
                else if (InputManager.Instance.KeyPressed(Keys.S))
                {
                    if (ItemNumber == 0 || ItemNumber == 2)
                        ItemNumber++;
                }

                else if (InputManager.Instance.KeyPressed(Keys.A))
                {
                    if (ItemNumber == 2 || ItemNumber == 3)
                        ItemNumber -= 2;
                }
                else if (InputManager.Instance.KeyPressed(Keys.D))
                {
                    if (ItemNumber == 0 || ItemNumber == 1)
                        ItemNumber += 2;
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[i].Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width - 4,
                        Items[i].Image.Position.Y + (Items[i].Image.SourceRect.Height / 4) - 2);

                }
                else
                    Items[i].Image.IsActive = false;
  
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
