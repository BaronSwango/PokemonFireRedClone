using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    //TODO: Display player stats (Name [string], time [int -> string], amount of pokemon in pokedex [int], gym badges[int])
    public class TitleMenu : Menu
    {

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
            base.AlignMenuItems();
            Console.WriteLine("test");
            Vector2 dimensions = Vector2.Zero;

            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X -
                dimensions.X) / 2, FromTop);

            foreach (MenuItem item in Items)
            {
                item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X -
                        item.Image.SourceRect.Width) / 2, dimensions.Y);
                dimensions += new Vector2(item.Image.SourceRect.Width + PaddingX,
                    item.Image.SourceRect.Height + PaddingY);
            }
        }


        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
                if (InputManager.Instance.KeyPressed(Keys.S))
                    itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    itemNumber--;

                if (itemNumber < 0)
                    itemNumber = 0;
                else if (itemNumber > Items.Count - 1)
                    itemNumber = Items.Count - 1;

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i == itemNumber)
                        Items[i].Image.IsActive = true;
                    else
                        Items[i].Image.IsActive = false;
                }
                base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
