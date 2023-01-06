using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace PokemonFireRedClone
{

/*TODO: List of things to save in json object
 * 
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
        public string MapName;
        public string Name;
        public string TrainerID;
        public Gender Gender;
        public string RivalName;
        public double Time;
        public int Badges;
        public int Pokedex;
        public int Money;
        public List<CustomPokemon> PokemonInBag;
        public List<string> TrainersDefeated;
        [JsonIgnore]
        public Area CurrentArea;
        //public Dictionary<Pokemon, bool> PokemonInPokedex;
        //public Dictionary<CustomPokemon,KeyValuePair<int,KeyValuePair<int,int>>> PokemonInPC;

    }
}
