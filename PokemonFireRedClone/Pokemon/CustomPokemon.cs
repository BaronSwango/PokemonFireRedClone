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
        public int CurrentEXP;
        public int CurrentHP;
        public StatList Stats;

        public CustomPokemon(string pokemonName, Nature nature, Gender gender, List<string> moveNames, int level, StatList stats)
        {
            PokemonName = pokemonName;
            Nature = nature;
            Gender = gender;
            MoveNames = moveNames;
            Level = level;
            Stats = stats;
            CurrentHP = Stats.HP;
            CurrentEXP = CurrentLevelEXP;
        }

        [JsonIgnore]
        public List<Move> Moves {

            get
            {
                Moves = new List<Move>();
                foreach (string name in MoveNames)
                    Moves.Add(MoveManager.Instance.GetMove(name));

                return Moves;
            }
            private set { }
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
                switch (PokemonManager.Instance.GetPokemon(PokemonName).EXPGroup)
                {
                    case "F":
                        return (int)(4.0f * Math.Pow(Level, 3) / 5.0f);
                    case "MF":
                        return (int)Math.Pow(Level, 3);
                    case "MS":
                        return (int)((6.0f / 5.0f * Math.Pow(Level, 3)) - 15 * Math.Pow(Level, 2) + 100 * Level - 140);
                    case "S":
                        return (int)(5.0f * Math.Pow(Level, 3) / 4.0f);
                }
                return 0;
            }
            private set {  }
        }


        [JsonIgnore]
        public int NextLevelEXP
        {
            get
            {
                switch (PokemonManager.Instance.GetPokemon(PokemonName).EXPGroup)
                {
                    case "F":
                        return (int) (4.0f * Math.Pow(Level + 1, 3) / 5.0f);
                    case "MF":
                        return (int)Math.Pow(Level + 1, 3);
                    case "MS":
                        return (int)((6.0f / 5.0f * Math.Pow(Level + 1, 3)) - 15 * Math.Pow(Level + 1, 2) + 100 * (Level + 1) - 140);
                    case "S":
                        return (int)(5.0f * Math.Pow(Level + 1, 3) / 4.0f);
                }
                return 0;
            }

            private set { } 
        }


    }
}
