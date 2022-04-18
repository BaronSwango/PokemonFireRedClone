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
        public Image BackgroundFaintSelected;
        public Image BackgroundFaintUnselected;
        public Image MenuSprite;

        protected PokemonAssets pokemonAssets;
        protected CustomPokemon pokemon;

        public PokemonMenuInfoButton(CustomPokemon pokemon)
        {
            this.pokemon = pokemon;
            pokemonAssets = new PokemonAssets(pokemon, true);
        }


        public void LoadContent()
        {
            LoadBackground();
            pokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", Color.White, Color.Gray);

            MenuSprite = pokemon.Pokemon.MenuSprite;
            MenuSprite.LoadContent();
            MenuSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(2, 1);
            MenuSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;
            MenuSprite.IsActive = true;
        }

        public void UnloadContent()
        {
            BackgroundUnselected.UnloadContent();
            pokemonAssets.UnloadContent();
            MenuSprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            MenuSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pokemonAssets.Draw(spriteBatch);
            MenuSprite.Draw(spriteBatch);
        }

        public virtual void UpdateInfoPositions()
        {
            pokemonAssets.Name.SetPosition(new Vector2(BackgroundUnselected.Position.X + 116, BackgroundUnselected.Position.Y + 20));
            pokemonAssets.Level.SetPosition(new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8));
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.SetPosition(new Vector2(BackgroundUnselected.Position.X + (BackgroundUnselected.SourceRect.Width / 2), pokemonAssets.Level.Position.Y));
            pokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 21 - pokemonAssets.MaxHP.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 101 - pokemonAssets.CurrentHP.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.HPBar.Position = new Vector2(BackgroundUnselected.Position.X + 384 - ((1 - pokemonAssets.HPBar.Scale.X) / 2 * pokemonAssets.HPBar.SourceRect.Width), BackgroundUnselected.Position.Y + 32);
            MenuSprite.Position = new Vector2(BackgroundUnselected.Position.X + 4, BackgroundUnselected.Position.Y + 4);
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
