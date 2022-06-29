using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class SummaryPage
    {
        private readonly Image outline;
        private readonly Image pokeball;

        protected Image Background;
        protected Image PokeImage;
        protected PokemonAssets PokemonAssets;
        
        public CustomPokemon Pokemon;

        public SummaryPage(CustomPokemon pokemon)
        {
            Pokemon = pokemon;

            outline = new Image
            {
                Path = "Menus/SummaryMenu/Outline"
            };
            pokeball = new Image
            {
                Path = "BattleScreen/Pokeball"
            };

            PokeImage = pokemon.Pokemon.Front;
            PokeImage.Flip = true;
            Background = new Image();
            PokemonAssets = new PokemonAssets(Pokemon, true);
        }

        public virtual void LoadContent()
        {
            outline.LoadContent();
            pokeball.LoadContent();
            pokeball.Position = new Vector2(400, 328);

            Background.LoadContent();
            PokeImage.LoadContent();
            PokeImage.Position = new Vector2(240 - (PokeImage.SourceRect.Width / 2), 256-(PokeImage.SourceRect.Height/2));
            PokemonAssets.LoadContent("Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113));
            PokemonAssets.Name.SetPosition(new Vector2(160, 68));
            PokemonAssets.Gender.SetPosition(new Vector2(PokemonAssets.Name.Position.X + PokemonAssets.Name.SourceRect.Width + 20, 68));
            PokemonAssets.Level.SetPosition(new Vector2(20, PokemonAssets.Name.Position.Y));
        }

        public virtual void UnloadContent()
        {
            outline.UnloadContent();
            pokeball.UnloadContent();

            Background.UnloadContent();
            PokeImage.UnloadContent();
            PokemonAssets.UnloadContent();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            outline.Draw(spriteBatch);
            Background.Draw(spriteBatch);

            PokeImage.Draw(spriteBatch);
            PokemonAssets.Name.Draw(spriteBatch);
            if (PokemonAssets.Gender != null)
                PokemonAssets.Gender.Draw(spriteBatch);
            PokemonAssets.Level.Draw(spriteBatch);
            pokeball.Draw(spriteBatch);
        }
        
    }
}
