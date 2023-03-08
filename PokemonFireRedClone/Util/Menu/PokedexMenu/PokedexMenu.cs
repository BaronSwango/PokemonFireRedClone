﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
	public class PokedexMenu : Menu
	{
		public Image PokedexMenuBackground;
        public Image Arrow;
        public PokemonText Seen;
        public PokemonText Owned;

        protected override void AlignMenuItems()
        {
            Items[0].PokemonText.SetPosition(new(PokedexMenuBackground.Position.X + 176, PokedexMenuBackground.Position.Y + 124));
        }

        public override void LoadContent()
        {
            PokedexMenuBackground.LoadContent();
            Arrow.LoadContent();

            Seen.Image.Text = Player.PlayerJsonObject.PokemonSeen.Count.ToString();
            Owned.Image.Text = Player.PlayerJsonObject.PokemonOwned.Count.ToString();

            Seen.LoadContent();
            Owned.LoadContent();

            base.LoadContent();

            Seen.SetPosition(new(PokedexMenuBackground.SourceRect.Width - Seen.SourceRect.Width - 100, 144));
            Owned.SetPosition(new(Seen.Position.X, Seen.Position.Y + Owned.SourceRect.Height + 60));
        }

        public override void UnloadContent()
        {
            PokedexMenuBackground.UnloadContent();
            Arrow.UnloadContent();

            Seen.UnloadContent();
            Owned.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
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
                    Items[i].PokemonText.Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].PokemonText.Position.X - Arrow.SourceRect.Width,
                        Items[i].PokemonText.Position.Y + (Items[i].PokemonText.SourceRect.Height / 4) - 2);
                }
                else
                {
                    Items[i].PokemonText.Image.IsActive = false;
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokedexMenuBackground.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);

            Seen.Draw(spriteBatch);
            Owned.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
