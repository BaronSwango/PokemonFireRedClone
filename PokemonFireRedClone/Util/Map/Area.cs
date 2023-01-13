using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Area
    {
        public class PokemonRange
        {
            public string PokemonName;
            [XmlElement("Levels")]
            public List<int> Levels;
            public float EncounterRate;
        }

        private KeyValuePair<KeyValuePair<int, int>, KeyValuePair<int, int>> zoneBounds;
        private Rectangle zoneRect;

        public string Name;
        [XmlElement("Zone")]
        public List<string> Zone;
        [XmlElement("PokemonRange")]
        public List<PokemonRange> Ranges;
        public int WildRate;

        public void LoadContent()
        {
            zoneBounds = new KeyValuePair<KeyValuePair<int, int>, KeyValuePair<int, int>>(
                new KeyValuePair<int, int>(int.Parse(Zone[0].Split(',')[0]), int.Parse(Zone[0].Split(',')[1])),
                new KeyValuePair<int, int>(int.Parse(Zone[1].Split(',')[0]), int.Parse(Zone[1].Split(',')[1]))
            );
            zoneRect = new(zoneBounds.Key.Key, zoneBounds.Key.Value, zoneBounds.Value.Key - zoneBounds.Key.Key, zoneBounds.Value.Value - zoneBounds.Key.Value);
        }

        public bool PlayerEntered(Player player)
        {
            return !zoneRect.Contains(player.PreviousPos.X, player.PreviousPos.Y - 12) && zoneRect.Contains(player.Sprite.Position.X, player.Sprite.Position.Y - 12);
        }

        public bool Contains(Vector2 pos)
        {
            return zoneRect.Contains(pos);
        }

    }
}
