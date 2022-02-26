using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class CustomPokemon
    {
        public string Name;
        public string PokemonName;
        public Nature Nature;
        public Gender Gender;
        public List<string> MoveNames;
        public int Level;
        public int currentHP;
        public StatList Stats;

        public CustomPokemon(string pokemonName, Nature nature, Gender gender, List<string> moveNames, int level, StatList stats)
        {
            PokemonName = pokemonName;
            Nature = nature;
            Gender = gender;
            MoveNames = moveNames;
            Level = level;
            Stats = stats;
        }

        [JsonIgnore]
        public List<Move> Moves {

            get
            {
                Moves = new List<Move>();
                foreach (string name in MoveNames)
                {
                    Moves.Add(MoveManager.Instance.GetMove(name));
                }
                return Moves;
            }
            private set { }
        }

        [JsonIgnore]
        public Pokemon Pokemon
        {
            get
            {
                return PokemonManager.Instance.GetPokemon(PokemonName);
            }

            private set { }
        }


    }
}
