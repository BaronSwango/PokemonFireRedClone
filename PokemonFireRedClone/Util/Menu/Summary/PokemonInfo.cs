using System;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonInfo : SummaryPage
    {

        public PokemonInfo(CustomPokemon pokemon)
            : base(pokemon)
        {
            background.Path = "Menus/SummaryMenu/PokemonInfoBackground";
        }

    }
}
