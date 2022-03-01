using System;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class Move
    {

        public string Name;
        public string Description;
        public string TypeName;
        public int Power;
        public int Accuracy;
        public int PP;
        public bool Special;

        [XmlIgnore]
        public Type Type
        {
            get { return TypeProperties.TypeFromName(TypeName); }
        }

    }
}
