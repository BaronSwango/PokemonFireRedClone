using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class PokemonManager
    {
        private static PokemonManager instance;

        [XmlElement("Pokemon")]
        public List<Pokemon> Pokemon;

        public static PokemonManager Instance
        {
            get
            {
                XmlManager<PokemonManager> xml = new();
                instance = xml.Load("Load/Pokemon/PokemonManager.xml");

                return instance;
            }
        }

        private StatList ApplyNature(Nature nature, StatList statList)
        {
            switch (nature)
            {
                case Nature.LONELY:
                    statList.Attack = (int)(statList.Attack * 1.1);
                    statList.Defense = (int)(statList.Defense * 0.9);
                    break;
                case Nature.BRAVE:
                    statList.Attack = (int)(statList.Attack * 1.1);
                    statList.Speed = (int)(statList.Speed * 0.9);
                    break;
                case Nature.ADAMANT:
                    statList.Attack = (int)(statList.Attack * 1.1);
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 0.9);
                    break;
                case Nature.NAUGHTY:
                    statList.Attack = (int)(statList.Attack * 1.1);
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 0.9);
                    break;

                case Nature.BOLD:
                    statList.Defense = (int)(statList.Defense * 1.1);
                    statList.Attack = (int)(statList.Attack * 0.9);
                    break;
                case Nature.RELAXED:
                    statList.Defense = (int)(statList.Defense * 1.1);
                    statList.Speed = (int)(statList.Speed * 0.9);
                    break;
                case Nature.IMPISH:
                    statList.Defense = (int)(statList.Defense * 1.1);
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 0.9);
                    break;
                case Nature.LAX:
                    statList.Defense = (int)(statList.Defense * 1.1);
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 0.9);
                    break;

                case Nature.TIMID:
                    statList.Speed = (int)(statList.Speed * 1.1);
                    statList.Attack = (int)(statList.Attack * 0.9);
                    break;
                case Nature.HASTY:
                    statList.Speed = (int)(statList.Speed * 1.1);
                    statList.Defense = (int)(statList.Defense * 0.9);
                    break;
                case Nature.JOLLY:
                    statList.Speed = (int)(statList.Speed * 1.1);
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 0.9);
                    break;
                case Nature.NAIVE:
                    statList.Speed = (int)(statList.Speed * 1.1);
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 0.9);
                    break;

                case Nature.MODEST:
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 1.1);
                    statList.Attack = (int)(statList.Attack * 0.9);
                    break;
                case Nature.MILD:
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 1.1);
                    statList.Defense = (int)(statList.Defense * 0.9);
                    break;
                case Nature.QUIET:
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 1.1);
                    statList.Speed = (int)(statList.Speed * 0.9);
                    break;
                case Nature.RASH:
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 1.1);
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 0.9);
                    break;

                case Nature.CALM:
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 1.1);
                    statList.Attack = (int)(statList.Attack * 0.9);
                    break;
                case Nature.GENTLE:
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 1.1);
                    statList.Defense = (int)(statList.Defense * 0.9);
                    break;
                case Nature.SASSY:
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 1.1);
                    statList.Speed = (int)(statList.Speed * 0.9);
                    break;
                case Nature.CAREFUL:
                    statList.SpecialDefense = (int)(statList.SpecialDefense * 1.1);
                    statList.SpecialAttack = (int)(statList.SpecialAttack * 0.9);
                    break;

                case Nature.BASHFUL:
                case Nature.DOCILE:
                case Nature.HARDY:
                case Nature.QUIRKY:
                case Nature.SERIOUS:
                default:
                    break;
            }
            return statList;
        }

        private int CalculateStats(int b, int iv, int ev, int level)
        {
            return ((2 * b + iv + (ev / 4)) * level / 100) + 5;
        }

        public Pokemon GetPokemon(string name)
        {
            foreach (Pokemon pokemon in Pokemon)
            {
                if (pokemon.Name == name)
                {
                    pokemon.LoadInfo();
                    return pokemon;
                }
            }
            return null;
        }


        // Level ranges and pokemon from specific area of map
        public CustomPokemon CreatePokemon(Pokemon pokemon, int level)
        {
            Random random = new();

            Gender gender;

            if (pokemon.PercentMale < 0)
                gender = Gender.GENDERLESS;
            else
                gender = random.NextDouble() * 100f < pokemon.PercentMale ? Gender.MALE : Gender.FEMALE;

            CustomPokemon poke = new(pokemon.Name, (Nature)random.Next(25), gender,
                new Dictionary<string, int>(), level,
                new StatList
                {
                    HPIV = random.Next(32),
                    AttackIV = random.Next(32),
                    SpecialAttackIV = random.Next(32),
                    DefenseIV = random.Next(32),
                    SpecialDefenseIV = random.Next(32),
                    SpeedIV = random.Next(32)
                })
            {
                Name = pokemon.Name.ToUpper()
            };
            poke.Stats = GenerateStatList(poke);
            return poke;
        }


        public StatList GenerateStatList(CustomPokemon poke)
        {
            StatList newStatList = poke.Stats;
            newStatList.HP = ((2 * poke.Pokemon.BaseHP + poke.Stats.HPIV + (poke.Stats.HPEV / 4)) * poke.Level / 100) + poke.Level + 10;
            newStatList.Attack = CalculateStats(poke.Pokemon.BaseAttack, poke.Stats.AttackIV, poke.Stats.AttackEV, poke.Level);
            newStatList.SpecialAttack = CalculateStats(poke.Pokemon.BaseSpecialAttack, poke.Stats.SpecialAttackIV, poke.Stats.SpecialAttackEV, poke.Level);
            newStatList.Defense = CalculateStats(poke.Pokemon.BaseDefense, poke.Stats.DefenseIV, poke.Stats.DefenseEV, poke.Level);
            newStatList.SpecialDefense = CalculateStats(poke.Pokemon.BaseSpecialDefense, poke.Stats.SpecialDefenseIV, poke.Stats.SpecialDefenseEV, poke.Level);
            newStatList.Speed = CalculateStats(poke.Pokemon.BaseSpeed, poke.Stats.SpeedIV, poke.Stats.SpeedEV, poke.Level);
            newStatList = ApplyNature(poke.Nature, newStatList);
            return newStatList;
        }

        public StatList StatsOfLevel(CustomPokemon poke, int level)
        {
            StatList newStatList = new()
            {
                HP = ((2 * poke.Pokemon.BaseHP + poke.Stats.HPIV + (poke.Stats.HPEV / 4)) * level / 100) + level + 10,
                Attack = CalculateStats(poke.Pokemon.BaseAttack, poke.Stats.AttackIV, poke.Stats.AttackEV, level),
                SpecialAttack = CalculateStats(poke.Pokemon.BaseSpecialAttack, poke.Stats.SpecialAttackIV, poke.Stats.SpecialAttackEV, level),
                Defense = CalculateStats(poke.Pokemon.BaseDefense, poke.Stats.DefenseIV, poke.Stats.DefenseEV, level),
                SpecialDefense = CalculateStats(poke.Pokemon.BaseSpecialDefense, poke.Stats.SpecialDefenseIV, poke.Stats.SpecialDefenseEV, level),
                Speed = CalculateStats(poke.Pokemon.BaseSpeed, poke.Stats.SpeedIV, poke.Stats.SpeedEV, level)
            };
            newStatList = ApplyNature(poke.Nature, newStatList);
            return newStatList;
        }

    }
}
