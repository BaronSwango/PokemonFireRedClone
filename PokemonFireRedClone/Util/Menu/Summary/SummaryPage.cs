using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class SummaryPage
    {
        public CustomPokemon Pokemon;
        protected Image background;
        protected Image pokeImage;
        protected PokemonAssets pokemonAssets;
        

        readonly Image outline;
        readonly Image pokeball;

        public SummaryPage(CustomPokemon pokemon)
        {
            Pokemon = pokemon;
            outline = new Image
            {
                Path = "Menus/SummaryMenu/Outline"
            };
            pokeImage = pokemon.Pokemon.Front;
            pokeImage.Flip = true;
            background = new Image();
            pokemonAssets = new PokemonAssets(Pokemon, true);
            pokeball = new Image
            {
                Path = "BattleScreen/Pokeball"
            };
        }

        public virtual void LoadContent()
        {
            outline.LoadContent();
            background.LoadContent();
            pokeImage.LoadContent();
            pokeImage.Position = new Vector2(240 - (pokeImage.SourceRect.Width / 2), 256-(pokeImage.SourceRect.Height/2));
            pokemonAssets.LoadContent("Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113));
            pokemonAssets.Name.SetPosition(new Vector2(164, 68));
            pokemonAssets.Gender.SetPosition(new Vector2(pokemonAssets.Name.Position.X + pokemonAssets.Name.SourceRect.Width + 24, 68));
            pokemonAssets.Level.SetPosition(new Vector2(20, pokemonAssets.Name.Position.Y));
            pokeball.LoadContent();
            pokeball.Position = new Vector2(400, 328);
        }

        public virtual void UnloadContent()
        {
            outline.UnloadContent();
            background.UnloadContent();
            pokeImage.UnloadContent();
            pokemonAssets.UnloadContent();
            pokeball.UnloadContent();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            outline.Draw(spriteBatch);
            background.Draw(spriteBatch);
            pokeImage.Draw(spriteBatch);
            pokemonAssets.Name.Draw(spriteBatch);
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.Draw(spriteBatch);
            pokemonAssets.Level.Draw(spriteBatch);
            pokeball.Draw(spriteBatch);
        }
        
    }
}
