using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class CustomPokemon
    {

        private readonly Dictionary<Move, int> moves;

        public string Name;
        public string PokemonName;
        [XmlIgnore]
        public Nature Nature;
        public Gender Gender;
        [JsonIgnore]
        public List<string> MoveNames;
        [XmlIgnore]
        public Dictionary<string, int> MovePP;
        public int Level;
        public int CurrentEXP;
        public int CurrentHP;
        public StatList Stats;

        public CustomPokemon() { moves = new Dictionary<Move, int>(); Stats = new StatList(); }
        public CustomPokemon(string pokemonName, Nature nature, Gender gender, Dictionary<string, int> movePP, int level, StatList stats)
        {
            PokemonName = pokemonName;
            Nature = nature;
            Gender = gender;
            MovePP = movePP;
            Level = level;
            Stats = stats;
            CurrentHP = Stats.HP;
            CurrentEXP = CurrentLevelEXP;
            Moves = new Dictionary<Move, int>();
        }

        [XmlIgnore]
        [JsonIgnore]
        public Dictionary<Move, int> Moves
        {
            get
            {
                if (moves.Count == 0)
                {
                    foreach (string name in MovePP.Keys)
                        moves.Add(MoveManager.Instance.GetMove(name), MovePP[name]);
                }
                return moves;
            }
            private set { }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Pokemon Pokemon
        {
            get { return PokemonManager.Instance.GetPokemon(PokemonName); }

            private set { }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int EXPTowardsLevelUp
        {
            get { return Level == 100 ? 0 : CurrentEXP - CurrentLevelEXP; }

            private set { }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int EXPNeededToLevelUp
        {
            get { return Level == 100 ? 0 : NextLevelEXP - CurrentLevelEXP; }

            private set {  }
        }

        [XmlIgnore]
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

        [XmlIgnore]
        [JsonIgnore]
        public int NextLevelEXP
        {
            get
            {
                return Level == 100 ? 0 : PokemonManager.Instance.GetPokemon(PokemonName).EXPGroup switch
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


        public void Create()
        {
            Name = PokemonName.ToUpper();
            Random random = new();

            Nature = (Nature)random.Next(25);

            MovePP = new Dictionary<string, int>
            {
                { "Water Gun", MoveManager.Instance.GetMove("Water Gun").PP }
            };

            Stats = PokemonManager.Instance.GenerateStatList(this);

            CurrentHP = Stats.HP;

        }

    }
}
