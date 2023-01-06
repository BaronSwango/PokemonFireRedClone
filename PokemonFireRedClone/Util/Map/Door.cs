using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Door
    {
        public string ID;
        public string MapName;
        public string SpawnCoords;

        [XmlIgnore]
        public Vector2 Coords
        {
            get { return new(int.Parse(SpawnCoords.Split(',')[0]), int.Parse(SpawnCoords.Split(',')[1])); }
            private set { }
        }

    }
}
