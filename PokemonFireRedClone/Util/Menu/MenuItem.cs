using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class MenuItem
    {
        public string LinkType;
        public string LinkID;
        public string MenuName;
        public Image Image;
        public PokemonText PokemonText;
        [XmlElement("Description")]
        public List<Image> Description;
        public bool HasTransition;
    }
}
