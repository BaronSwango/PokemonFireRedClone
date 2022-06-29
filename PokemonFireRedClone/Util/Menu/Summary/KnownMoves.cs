using System;
namespace PokemonFireRedClone
{
    public class KnownMoves : SummaryPage
    {
        public KnownMoves(CustomPokemon pokemon)
            : base(pokemon)
        {
            Background.Path = "Menus/SummaryMenu/KnownMovesBackground";
        }
    }
}
