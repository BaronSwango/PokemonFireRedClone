using System;
namespace PokemonFireRedClone
{
    public class PokemonSkills : SummaryPage
    {
        public PokemonSkills(CustomPokemon pokemon)
            :base(pokemon)
        {
            background.Path = "Menus/SummaryMenu/PokemonSkillsBackground";
        }
    }
}
