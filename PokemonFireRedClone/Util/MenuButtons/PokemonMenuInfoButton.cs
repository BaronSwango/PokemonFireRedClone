using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonMenuInfoButton
    {
        public Image BackgroundSelected;
        public Image BackgroundUnselected;
        public Image BackgroundSwitchSelected;
        public Image BackgroundSwitchUnselected;

        protected PokemonAssets pokemonAssets;

        public PokemonMenuInfoButton(CustomPokemon pokemon)
        {
            pokemonAssets = new PokemonAssets(pokemon, true);
        }


        public void LoadContent()
        {
            LoadBackground();
            pokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", Color.White);
            
        }

        public void UnloadContent()
        {
            BackgroundUnselected.UnloadContent();
            pokemonAssets.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pokemonAssets.Draw(spriteBatch);
        }

        public virtual void UpdateInfoPositions()
        {
            pokemonAssets.Name.Position = new Vector2(BackgroundUnselected.Position.X + 116, BackgroundUnselected.Position.Y + 20);
            pokemonAssets.Level.Position = new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8);
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.Position = new Vector2(BackgroundUnselected.Position.X + (BackgroundUnselected.SourceRect.Width / 2), pokemonAssets.Level.Position.Y);
            pokemonAssets.MaxHP.Position = new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 21 - pokemonAssets.MaxHP.SourceRect.Width, pokemonAssets.Level.Position.Y);
            pokemonAssets.CurrentHP.Position = new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 101 - pokemonAssets.CurrentHP.SourceRect.Width, pokemonAssets.Level.Position.Y);
            pokemonAssets.HPBar.Position = new Vector2(BackgroundUnselected.Position.X + 384 - ((1 - pokemonAssets.HPBar.Scale.X) / 2 * pokemonAssets.HPBar.SourceRect.Width), BackgroundUnselected.Position.Y + 32);
        }

        protected virtual void LoadBackground()
        {
            BackgroundSelected = new Image();
            BackgroundUnselected = new Image();
            BackgroundSwitchSelected = new Image();
            BackgroundSwitchUnselected = new Image();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuButton";

        }
 
    }
}
