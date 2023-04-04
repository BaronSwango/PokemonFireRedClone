using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace PokemonFireRedClone
{
	public class PokedexMenu : Menu
	{
		public Image PokedexMenuBackground;
        public Image PokedexBackground;
        public Image TransitionBox;
        public Image Arrow;
        public PokemonText Seen;
        public PokemonText Owned;

        // TEXT ON PAGE
        public PokemonText PokemonList;
        public PokemonText SeenText;
        public PokemonText OwnedText;

        protected override void AlignMenuItems()
        {
            Items[0].PokemonText.SetPosition(new(PokedexMenuBackground.Position.X + 176, PokedexMenuBackground.Position.Y + 124));
            Items[0].Image.Position = new(980, 400);
        }

        public override void LoadContent()
        {
            // TEXT ON PAGE
            PokemonList.LoadContent();
            SeenText.LoadContent();
            OwnedText.LoadContent();

            // POSITIONS
            PokemonList.SetPosition(new(128, 68));
            SeenText.SetPosition(new(992, 96));
            OwnedText.SetPosition(new(992, 212));

            PokedexMenuBackground.LoadContent();
            PokedexBackground.LoadContent();
            Arrow.LoadContent();

            PokedexBackground.Position.Y = 64;

            Seen.Image.Text = Player.PlayerJsonObject.Pokedex.Count.ToString();


            Owned.Image.Text = Player.PlayerJsonObject.Pokedex.Count(kv => kv.Value == true).ToString();

            Seen.LoadContent();
            Owned.LoadContent();

            base.LoadContent();

            Arrow.Position = new Vector2(Items[0].PokemonText.Position.X - Arrow.SourceRect.Width,
                                    Items[0].PokemonText.Position.Y + (Items[0].PokemonText.SourceRect.Height / 4) - 2);
            Seen.SetPosition(new(PokedexMenuBackground.SourceRect.Width - Seen.SourceRect.Width - 96, 144));
            Owned.SetPosition(new(PokedexMenuBackground.SourceRect.Width - Owned.SourceRect.Width - 96, Seen.Position.Y + Owned.SourceRect.Height + 60));
        }

        public override void UnloadContent()
        {
            PokemonList.UnloadContent();
            SeenText.UnloadContent();
            OwnedText.UnloadContent();

            PokedexMenuBackground.UnloadContent();
            Arrow.UnloadContent();
            PokedexBackground.UnloadContent();

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
            PokedexBackground.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);

            PokemonList.Draw(spriteBatch);
            SeenText.Draw(spriteBatch);
            OwnedText.Draw(spriteBatch);

            Seen.Draw(spriteBatch);
            Owned.Draw(spriteBatch);

            Items[ItemNumber].Image.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}

