using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{

/*TODO: List of things to save in json object
 * 
 * Rival name
 * Area
 * Pokemon
 * Phase of game
 * Pokedex
 * Items
 * Trainers battled
 *
 */

    public class PlayerJsonObject
    {
        public Vector2 Position;
        public float Direction;
        public string Name;
        public string RivalName;
        public double Time;
        public int Badges;
        public int Pokedex;
        public int Money;
        public List<Pokemon> PokemonInBag;
        public List<Pokemon> PokemonInPC;

    }
}
