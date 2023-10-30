using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class PokemonDetails
	{
		public Pokemon Pokemon;
		public PokemonText Index;
		public PokemonText Name;
		public PokemonText Classification;
		public PokemonText Height;
		public PokemonText Weight;
		public PokemonText Description;
		public Image PokemonImage;
		public Image Footprint;

		public PokemonDetails(Pokemon pokemon, bool owned)
		{
			Pokemon = pokemon;

			Index = new(pokemon.Index.ToString().PadLeft(3, '0'), "Fonts/PokemonFireRedSmall",
					new Color(0, 0, 0), new Color(224, 216, 192));

            Name = new(pokemon.Name.ToUpper(), "Fonts/PokemonFireRedDialogue",
                    new Color(0, 0, 0), new Color(224, 216, 192));

			string classification = owned ? pokemon.Classification.ToUpper() : "???????????";

            Classification = new(classification + "  POKeMON", "Fonts/PokemonFireRedSmall",
                    new Color(0, 0, 0), new Color(224, 216, 192));

            string height = owned ? pokemon.Height : "??`??\"";

            Height = new(height, "Fonts/PokemonFireRedSmall",
                    new Color(0, 0, 0), new Color(224, 216, 192));

			string weight = owned ? pokemon.Weight : "????.?";

            Weight = new(weight + "  lbs.", "Fonts/PokemonFireRedSmall",
                    new Color(0, 0, 0), new Color(224, 216, 192));

			if (owned)
			{
				Description = new(pokemon.Description, "Fonts/PokemonFireRedDialogue",
                    new Color(0, 0, 0), new Color(201, 189, 155));
			}

			PokemonImage = pokemon.Front;
        }

		public void LoadContent()
		{
			Index.LoadContent();
			Name.LoadContent();
			Classification.LoadContent();
			Height.LoadContent();
			Weight.LoadContent();
			Description?.LoadContent();
			PokemonImage.LoadContent();
		}

		public void UnloadContent()
		{
            Index.UnloadContent();
            Name.UnloadContent();
            Classification.UnloadContent();
            Height.UnloadContent();
            Weight.UnloadContent();
            Description?.UnloadContent();
            PokemonImage.UnloadContent();
        }

		public void ReloadContent()
		{
			Index.ReloadText();
			Name.ReloadText();
            Classification.ReloadText();
            Height.ReloadText();
            Weight.ReloadText();
            Description?.ReloadText();
            PokemonImage.ReloadTexture();
        }

		public void Draw(SpriteBatch spriteBatch)
		{
            Index.Draw(spriteBatch);
            Name.Draw(spriteBatch);
            Classification.Draw(spriteBatch);
            Height.Draw(spriteBatch);
            Weight.Draw(spriteBatch);
            Description?.Draw(spriteBatch);
            PokemonImage.Draw(spriteBatch);
        }
	}
}

