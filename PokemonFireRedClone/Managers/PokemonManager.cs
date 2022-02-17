﻿using System;
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
                XmlManager<PokemonManager> xml = new XmlManager<PokemonManager>();
                instance = xml.Load("Load/Pokemon/PokemonManager.xml");

                return instance;
            }
        }

        public Pokemon GetPokemon(string name)
        {
            foreach (Pokemon pokemon in Pokemon)
            {
                if (pokemon.Name == name)
                    return pokemon;
            }
            return null;
        }

        private static int calculateStats(int b, int iv, int level)
        {
            return ((2*b+iv)*level/100)+5;
        }

        // Level ranges and pokemon from specific area of map
        public static CustomPokemon createRandomPokemon(Pokemon pokemon)
        {
            Random random = new Random();

            //Pokemon pokemon = pokemons[pokemonIndex];

            CustomPokemon poke = new CustomPokemon(pokemon.Name, (Nature)random.Next(25), (Gender)random.Next(2),
                new List<string>(), 6,
                new StatList
                {
                    HPIV = random.Next(32),
                    AttackIV = random.Next(32),
                    SpecialAttackIV = random.Next(32),
                    DefenseIV = random.Next(32),
                    SpecialDefenseIV = random.Next(32),
                    SpeedIV = random.Next(32),
                });

            poke.Stats.HP = ((2 * pokemon.BaseHP + poke.Stats.HPIV) * poke.Level / 100) + poke.Level + 10;
            poke.Stats.Attack = calculateStats(pokemon.BaseAttack, poke.Stats.AttackIV, poke.Level);
            poke.Stats.SpecialAttack = calculateStats(pokemon.BaseSpecialAttack, poke.Stats.SpecialAttackIV, poke.Level);
            poke.Stats.Defense = calculateStats(pokemon.BaseDefense, poke.Stats.DefenseIV, poke.Level);
            poke.Stats.SpecialDefense = calculateStats(pokemon.BaseSpecialDefense, poke.Stats.SpecialDefenseIV, poke.Level);
            poke.Stats.Speed = calculateStats(pokemon.BaseSpeed, poke.Stats.SpeedIV, poke.Level);
            return poke;
        }
    }
}