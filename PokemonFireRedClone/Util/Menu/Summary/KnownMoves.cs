using System;
namespace PokemonFireRedClone
{
    public class KnownMoves : SummaryPage
    {
        public KnownMoves(CustomPokemon pokemon)
            : base(pokemon)
        {
            background.Path = "Menus/SummaryMenu/KnownMovesBackground";
        }
    }
}
