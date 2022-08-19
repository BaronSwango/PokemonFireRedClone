using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class Trainer : NPC
    {
        
        public string Name;
        public Image BattleSprite;
        [XmlElement("Pokemon")]
        public List<CustomPokemon> Pokemon;     

    }
}
