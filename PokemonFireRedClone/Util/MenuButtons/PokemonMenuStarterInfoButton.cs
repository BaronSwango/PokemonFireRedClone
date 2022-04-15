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
            PokemonName.Position = new Vector2(BackgroundUnselected.Position.X + 120, BackgroundUnselected.Position.Y + 76);
            PokemonLevel.Position = new Vector2(PokemonName.Position.X + 32, PokemonName.Position.Y + PokemonName.SourceRect.Height + 8);
        }

        protected override void LoadBackground()
        {
            base.LoadBackground();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuMainButton";
        }
    }
}
