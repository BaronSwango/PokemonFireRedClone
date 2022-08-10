using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class NPC : Entity
    {

        [XmlElement("TextBoxes")]
        public List<TextBox> TextBoxes;


    }
}
