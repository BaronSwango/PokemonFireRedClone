using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
	public class PokemonDetailsMenu : Menu
	{
		private readonly List<PokemonDetails> pokemonDetails;
        public Image PokemonDetailsBorder;
		public Image PokemonDetailsBackground;

        private Image pokedexTransitionBox;
        private Counter counter;

        private static int SavedIndex
        {
            get { return ((PokedexScreen)ScreenManager.Instance.CurrentScreen).SavedSearchIndex; }
            set { ((PokedexScreen)ScreenManager.Instance.CurrentScreen).SavedSearchIndex = value; }
        }

        public PokemonDetailsMenu()
		{
			pokemonDetails = new();
		}

        public override void LoadContent()
        {
            PokemonDetailsBorder.LoadContent();
			PokemonDetailsBackground.LoadContent();
            PokemonDetailsBackground.Position.Y = 64;

		    InitializePokemonOrder();

			pokemonDetails[ItemNumber].LoadContent();

			InitializePositions();
        }

        public override void UnloadContent()
        {
            PokemonDetailsBorder.UnloadContent();
            PokemonDetailsBackground.UnloadContent();
			pokemonDetails[ItemNumber].UnloadContent();
            PokemonMenu.SelectedIndex = ItemNumber;
        }

        public override void Update(GameTime gameTime)
        {
			if (InputManager.Instance.KeyPressed(Keys.W) && ItemNumber > 0)
			{
                pokemonDetails[ItemNumber--].UnloadContent();

				if (pokemonDetails[ItemNumber].Index.IsLoaded)
				{
                    pokemonDetails[ItemNumber].ReloadContent();
                }
                else
				{
					pokemonDetails[ItemNumber].LoadContent();
				}

                InitializePositions();
            }
            else if (InputManager.Instance.KeyPressed(Keys.S) && ItemNumber < pokemonDetails.Count - 1)
			{
                pokemonDetails[ItemNumber++].UnloadContent();

                if (pokemonDetails[ItemNumber].Index.IsLoaded)
                {
                    pokemonDetails[ItemNumber].ReloadContent();
                }
                else
                {
                    pokemonDetails[ItemNumber].LoadContent();
                }

                InitializePositions();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokemonDetailsBorder.Draw(spriteBatch);
            PokemonDetailsBackground.Draw(spriteBatch);
			pokemonDetails[ItemNumber].Draw(spriteBatch);
        }

        private void InitializePokemonOrder()
		{
			foreach (string s in Player.PlayerJsonObject.PokemonSeen)
			{
				bool owned = Player.PlayerJsonObject.PokemonOwned.Contains(s);
				Pokemon pokemon = PokemonManager.Instance.GetPokemon(s);

				pokemonDetails.Add(new(pokemon, owned));
            }

            pokemonDetails.Sort((o1, o2) => o1.Pokemon.Index.CompareTo(o2.Pokemon.Index));

			for (int i = 0; i < pokemonDetails.Count; i++)
			{
				if (pokemonDetails[i].Pokemon.Index == SavedIndex)
				{
					ItemNumber = i;
					return;
				}
			}

        }

		private void InitializePositions()
		{
            pokemonDetails[ItemNumber].Index.SetPosition(new(108, 176));
			pokemonDetails[ItemNumber].Name.SetPosition(new(pokemonDetails[ItemNumber].Index.Position.X + pokemonDetails[ItemNumber].Index.SourceRect.Width + 16,
				pokemonDetails[ItemNumber].Index.Position.Y - 20));
            pokemonDetails[ItemNumber].Classification.SetPosition(new(72,
				pokemonDetails[ItemNumber].Index.Position.Y + pokemonDetails[ItemNumber].Index.SourceRect.Height + 36));
            pokemonDetails[ItemNumber].Height.SetPosition(new(pokemonDetails[ItemNumber].Classification.Position.X + 272 - pokemonDetails[ItemNumber].Height.SourceRect.Width,
                pokemonDetails[ItemNumber].Classification.Position.Y + pokemonDetails[ItemNumber].Classification.SourceRect.Height + 20));
            pokemonDetails[ItemNumber].Weight.SetPosition(new(pokemonDetails[ItemNumber].Classification.Position.X + 352 - pokemonDetails[ItemNumber].Weight.SourceRect.Width,
                pokemonDetails[ItemNumber].Height.Position.Y + pokemonDetails[ItemNumber].Height.SourceRect.Height + 20));
            pokemonDetails[ItemNumber].Description?.SetPosition(new(20, 420));
            pokemonDetails[ItemNumber].PokemonImage.Position = new(940 - (pokemonDetails[ItemNumber].PokemonImage.SourceRect.Width / 2), 260 - (pokemonDetails[ItemNumber].PokemonImage.SourceRect.Height / 2));
        }

        private void LoadPokedexTransitionImage()
        {
            pokedexTransitionBox = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, PokemonDetailsBackground.SourceRect.Width, PokemonDetailsBackground.SourceRect.Height)
            };
            Color[] data = new Color[pokedexTransitionBox.Texture.Width * pokedexTransitionBox.Texture.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            pokedexTransitionBox.Texture.SetData(data);
            pokedexTransitionBox.Effects = "FadeEffect";
            pokedexTransitionBox.LoadContent();
            counter = new Counter(400);
            pokedexTransitionBox.Position.Y = 64;
        }

    }
}

