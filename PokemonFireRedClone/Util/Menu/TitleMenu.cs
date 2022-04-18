using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    //TODO: Display player stats (Name [string], time [int -> string], amount of pokemon in pokedex [int], gym badges[int])
    public class TitleMenu : Menu
    {
        PlayerJsonObject playerJsonObject;
        

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
            Vector2 dimensions = Vector2.Zero;

            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X -
                dimensions.X) / 2, FromTop);

            for (int i = 0; i < 2; i++)
            {
                Items[i].Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X -
                    Items[i].Image.SourceRect.Width) / 2, dimensions.Y);
                dimensions += new Vector2(Items[i].Image.SourceRect.Width + PaddingX,
                    Items[i].Image.SourceRect.Height + PaddingY);
            }

            dimensions = Vector2.Zero;

            for (int i = 2; i < Items.Count; i++)
            {
                Items[i].Image.Position = new Vector2(ScreenManager.Instance.Dimensions.X / 2 - 136 , dimensions.Y+100);
                dimensions += new Vector2(Items[i].Image.SourceRect.Width,
                    Items[i].Image.SourceRect.Height + 8);
            }

        }

        public override void LoadContent()
        {
            playerJsonObject = new PlayerJsonObject();
            var playerLoader = new JsonManager<PlayerJsonObject>();

            playerJsonObject = playerLoader.Load("Load/Gameplay/Player.json");

            // format time to include days to hours
            var tsTime = TimeSpan.FromHours(playerJsonObject.Time);

            Items[2].Image.Text = playerJsonObject.Name;
            Items[3].Image.Text = $"{tsTime.Hours + (tsTime.Days * 24):0}:{tsTime.Minutes:00}";
            Items[4].Image.Text = playerJsonObject.Pokedex.ToString();
            Items[5].Image.Text = playerJsonObject.Badges.ToString();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
                if (InputManager.Instance.KeyPressed(Keys.S))
                    ItemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    ItemNumber--;

                if (ItemNumber < 0)
                    ItemNumber = 0;
                else if (ItemNumber > 1)
                    ItemNumber = 1;

                for (int i = 0; i < 2; i++)
                {
                    if (i == ItemNumber)
                    {
                        Items[i].Image.IsActive = true;
                        for (int j = 2; j < Items.Count; j++)
                            Items[j].Image.IsActive = i == 0;
                    }
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
