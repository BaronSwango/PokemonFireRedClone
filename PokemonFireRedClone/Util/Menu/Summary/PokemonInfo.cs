using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonInfo : SummaryPage
    {

        private readonly PokemonText pokedexNum;
        private readonly PokemonText pokemonName;
        private readonly List<PokemonText> types;
        private readonly PokemonText playerName;
        private readonly PokemonText trainerID;
        private readonly PokemonText heldItem;
        private readonly PokemonText nature;


        public PokemonInfo(CustomPokemon pokemon)
            : base(pokemon)
        {
            Background.Path = "Menus/SummaryMenu/PokemonInfoBackground";

            pokedexNum = new PokemonText();
            pokemonName = new PokemonText();
            types = new List<PokemonText>();
            playerName = new PokemonText();
            trainerID = new PokemonText();
            heldItem = new PokemonText();
            nature = new PokemonText();

        }

    }
}
