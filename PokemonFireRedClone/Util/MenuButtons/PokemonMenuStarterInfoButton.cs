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
            pokemonAssets.Name.Position = new Vector2(BackgroundUnselected.Position.X + 120, BackgroundUnselected.Position.Y + 76);
            pokemonAssets.Level.Position = new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8);
        }

        protected override void LoadBackground()
        {
            base.LoadBackground();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuMainButton";
        }
    }
}
