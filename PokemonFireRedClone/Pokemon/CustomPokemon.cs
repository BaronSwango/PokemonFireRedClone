using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class CustomPokemon
    {

        private Dictionary<Move, int> moves;

        public string Name;
        public string PokemonName;
        public Nature Nature;
        public Gender Gender;
        public Dictionary<string, int> MoveNames;
        public int Level;
        public int CurrentEXP;
        public int CurrentHP;
        public StatList Stats;

        public CustomPokemon(string pokemonName, Nature nature, Gender gender, Dictionary<string, int> moveNames, int level, StatList stats)
        {
            PokemonName = pokemonName;
            Nature = nature;
            Gender = gender;
            MoveNames = moveNames;
            Level = level;
            Stats = stats;
            CurrentHP = Stats.HP;
            CurrentEXP = CurrentLevelEXP;
            Moves = new Dictionary<Move, int>();
        }

        [JsonIgnore]
        public Dictionary<Move, int> Moves
        {

            get
            {
                if (moves.Count == 0)
                {
                    foreach (string name in MoveNames.Keys)
                        moves.Add(MoveManager.Instance.GetMove(name), MoveNames[name]);
                }
                return moves;
            }
            private set
            {
                moves = value;

            }
        }

        [JsonIgnore]
        public Pokemon Pokemon
        {
            get { return PokemonManager.Instance.GetPokemon(PokemonName); }

            private set { }
        }

        [JsonIgnore]
        public int EXPTowardsLevelUp
        {
            get { return CurrentEXP - CurrentLevelEXP; }

            private set { }
        }

        [JsonIgnore]
        public int EXPNeededToLevelUp
        {
            get { return NextLevelEXP - CurrentLevelEXP; }

            private set {  }
        }

        [JsonIgnore]
        public int CurrentLevelEXP
        {
            get
            {
                return PokemonManager.Instance.GetPokemon(PokemonName).EXPGroup switch
                {
                    "F" => (int)(4.0f * Math.Pow(Level, 3) / 5.0f),
                    "MF" => (int)Math.Pow(Level, 3),
                    "MS" => (int)((6.0f / 5.0f * Math.Pow(Level, 3)) - 15 * Math.Pow(Level, 2) + 100 * Level - 140),
                    "S" => (int)(5.0f * Math.Pow(Level, 3) / 4.0f),
                    _ => 0,
                };
            }
            private set {  }
        }


        [JsonIgnore]
        public int NextLevelEXP
        {
            get
            {
                return PokemonManager.Instance.GetPokemon(PokemonName).EXPGroup switch
                {
                    "F" => (int)(4.0f * Math.Pow(Level + 1, 3) / 5.0f),
                    "MF" => (int)Math.Pow(Level + 1, 3),
                    "MS" => (int)((6.0f / 5.0f * Math.Pow(Level + 1, 3)) - 15 * Math.Pow(Level + 1, 2) + 100 * (Level + 1) - 140),
                    "S" => (int)(5.0f * Math.Pow(Level + 1, 3) / 4.0f),
                    _ => 0,
                };
            }

            private set { } 
        }


    }
}
