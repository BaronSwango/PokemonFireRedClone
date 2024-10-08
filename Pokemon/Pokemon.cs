﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    // TODO: Move evolution and learnset to XML file
    public class Pokemon
    {
        private List<Type> types;
        private Dictionary<Move, int> moveLearnset;
        private KeyValuePair<int, Pokemon> evolution;

        public string Name;
        public int Index;
        [XmlElement("Type")]
        public List<string> TypesName;
        public Image Back;
        public Image Front;
        public Image MenuSprite;
        public Image Footprint;
        public string Classification;
        public string Height;
        public string Weight;
        public string EXPGroup;
        public int EXPYield;
        public string Description;
        public string Ability;
        public float PercentMale;
        public int BaseHP;
        public int BaseAttack;
        public int BaseDefense;
        public int BaseSpecialAttack;
        public int BaseSpecialDefense;
        public int BaseSpeed;

        [XmlIgnore]
        public List<Type> Types
        {
            get { return types; }

            private set
            {
                types = value;
                foreach (string name in TypesName)
                    types.Add(TypeProperties.TypeFromName(name));
            }
        }

        [XmlIgnore]
        public Dictionary<Move, int> MoveLearnset
        {
            get { return moveLearnset; }

            private set
            {
                moveLearnset = value;
                switch (Name)
                {
                    case "Charmander":
                        moveLearnset.Add(MoveManager.Instance.GetMove("Scratch"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Growl"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Ember"), 7);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Flamethrower"), 31);
                        break;
                    case "Charmeleon":
                        moveLearnset.Add(MoveManager.Instance.GetMove("Scratch"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Growl"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Ember"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Flamethrower"), 34);
                        break;
                    case "Charizard":
                        moveLearnset.Add(MoveManager.Instance.GetMove("Scratch"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Growl"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Ember"), 1);
                        moveLearnset.Add(MoveManager.Instance.GetMove("Flamethrower"), 34);
                        break;
                    default:
                        break;
                }
            }
        }

        [XmlIgnore]
        public KeyValuePair<int, Pokemon> Evolution
        {
            get
            {
                switch(Name)
                {
                    case "Bulbasaur":
                        evolution = new KeyValuePair<int, Pokemon>(16, PokemonManager.Instance.GetPokemon("Ivysaur"));
                        break;
                    case "Ivysaur":
                        evolution = new KeyValuePair<int, Pokemon>(32, PokemonManager.Instance.GetPokemon("Venusaur"));
                        break;
                    case "Charmander":
                        evolution = new KeyValuePair<int, Pokemon>(16, PokemonManager.Instance.GetPokemon("Charmeleon"));
                        break;
                    case "Charmeleon":
                        evolution = new KeyValuePair<int, Pokemon>(36, PokemonManager.Instance.GetPokemon("Charizard"));
                        break;
                    case "Squirtle":
                        evolution = new KeyValuePair<int, Pokemon>(16, PokemonManager.Instance.GetPokemon("Wartortle"));
                        break;
                    case "Wartortle":
                        evolution = new KeyValuePair<int, Pokemon>(36, PokemonManager.Instance.GetPokemon("Blastoise"));
                        break;
                    default:
                        break;
                }
                return evolution;
            }
            private set { }
        }

        public void LoadInfo()
        {
            moveLearnset = new Dictionary<Move, int>();
            Types = new List<Type>();
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
         *   evolution 
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
