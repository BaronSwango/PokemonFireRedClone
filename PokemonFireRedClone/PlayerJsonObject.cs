using System;
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
        public int Time;
        public int Badges;
        public int Pokedex;

    }
}
