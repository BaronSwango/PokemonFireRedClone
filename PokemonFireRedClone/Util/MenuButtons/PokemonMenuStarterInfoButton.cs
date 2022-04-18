using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class PokemonMenuStarterInfoButton : PokemonMenuInfoButton
    {

        public PokemonMenuStarterInfoButton(CustomPokemon pokemon)
            : base(pokemon)
        {}

        public override void UpdateInfoPositions()
        {
            pokemonAssets.Name.SetPosition(new Vector2(BackgroundUnselected.Position.X + 120, BackgroundUnselected.Position.Y + 76));
            pokemonAssets.Level.SetPosition(new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8)); 
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.SetPosition(new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 28 - pokemonAssets.Gender.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 21 - pokemonAssets.MaxHP.SourceRect.Width, BackgroundUnselected.Position.Y + BackgroundUnselected.SourceRect.Height - 16 - pokemonAssets.MaxHP.SourceRect.Height));
            pokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundUnselected.Position.X + BackgroundUnselected.SourceRect.Width - 101 - pokemonAssets.CurrentHP.SourceRect.Width, pokemonAssets.MaxHP.Position.Y));
            pokemonAssets.HPBar.Position = new Vector2(BackgroundUnselected.Position.X + 120 - ((1 - pokemonAssets.HPBar.Scale.X) / 2 * pokemonAssets.HPBar.SourceRect.Width), BackgroundUnselected.Position.Y + 156);
            MenuSprite.Position = new Vector2(BackgroundUnselected.Position.X + 12, BackgroundUnselected.Position.Y + 40);
        }

        protected override void LoadBackground()
        {
            base.LoadBackground();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuMainButton";
        }
    }
}
