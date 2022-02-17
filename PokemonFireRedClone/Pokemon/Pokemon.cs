using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class Pokemon
    {

        public string Name;
        public int Index;
        [XmlElement("Type")]
        public List<string> TypesName;
        public Image Back;
        public Image Front;
        public Image Footprint;
        public string Classification;
        public string Height;
        public string Weight;
        public string EXPGroup;
        public int EXPYield;
        public string Description;
        public string Ability;
        public int BaseHP;
        public int BaseAttack;
        public int BaseDefense;
        public int BaseSpecialAttack;
        public int BaseSpecialDefense;
        public int BaseSpeed;

        [XmlIgnore]
        public List<Type> Types
        {
            get
            {
                if (Types.Count == 0)
                {
                    foreach (string name in TypesName)
                        Types.Add(TypeProperties.TypeFromName(name));
                }

                return Types;
            }
        }

        [XmlIgnore]
        public Dictionary<int, Move> MoveLearnset
        {
            get
            {
                if (MoveLearnset.Count == 0)
                {
                    switch(Name)
                    {
                        case "Charmander":
                            MoveLearnset.Add(1, MoveManager.Instance.GetMove("Scratch"));
                            MoveLearnset.Add(1, MoveManager.Instance.GetMove("Growl"));
                            MoveLearnset.Add(7, MoveManager.Instance.GetMove("Ember"));
                            MoveLearnset.Add(13, MoveManager.Instance.GetMove("Metal Claw"));
                            break;
                        default:
                            break;
                    }
                }
                return MoveLearnset;
            }
            private set { }
        }

        [XmlIgnore]
        public KeyValuePair<int, Pokemon> Evolution
        {
            get
            {
                switch(Name)
                {
                    case "Bulbasaur":
                        Evolution = new KeyValuePair<int, Pokemon>(16, PokemonManager.Instance.GetPokemon("Ivysaur"));
                        break;
                    case "Ivysaur":
                        Evolution = new KeyValuePair<int, Pokemon>(32, PokemonManager.Instance.GetPokemon("Venusaur"));
                        break;
                    case "Charmander":
                        Evolution = new KeyValuePair<int, Pokemon>(16, PokemonManager.Instance.GetPokemon("Charmeleon"));
                        break;
                    case "Charmeleon":
                        Evolution = new KeyValuePair<int, Pokemon>(36, PokemonManager.Instance.GetPokemon("Charizard"));
                        break;
                    default:
                        break;
                }
                return Evolution;
            }
            private set { }
        }



        public Pokemon()
        {
            MoveLearnset = new Dictionary<int, Move>();
        }


        /*
         * 
         * POKEMON:
         *  Name
         *  Pokedex #
         *  List of Types (could have at least 2)
         *   Move learnset
         *   Back image (when pokemon is the player's)
         *   Front image (when the pokemon is opposing the player)
         *   Footprint image
         *   Classification (string)
         *   Height (string)
         *   Weight (string)
         *   Experience group (fast, medium fast, medium slow, slow)
         *   Experience yield (int)
         *   Evolution 
         *    - Level
         *    - Which pokemon it evolves to
         *   Natures (for choosing specific stats)
         *   Base stats (some sort of stat calculator per level)
         *   Abilities
         *   Description
         *   
         *    
         *   Trainer and player specific:
         *    List of Moves:
         *    - Name
         *    - Type
         *    - Power (0 if negligible)
         *    - Accuracy (0 if negligible)
         *    - Description
         *    - BasePP
         *    - MaxPP
         *    - Effects 
         *     Nature
         *     Gender
         *      - 
         *     Stats
         *      - 
         *  
         */


    }
}
