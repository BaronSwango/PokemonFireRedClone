using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class Trainer : NPC
    {
        
        public string Name;
        public Image BattleSprite;
        public int Reward;
        [XmlElement("BattleText")]
        public List<TextBoxText> BattleText;
        [XmlElement("Pokemon")]
        public List<CustomPokemon> Pokemon;

        [XmlIgnore]
        public int TotalBattleTextPages
        {
            get { return BattleText[^1].Page; }
            private set { }
        }

        public Trainer() { }

    }
}
