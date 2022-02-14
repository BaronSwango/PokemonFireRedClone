using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class PokemonManager
    {
        private static PokemonManager instance;
        [XmlElement("Pokemon")]
        public List<Pokemon> Pokemon;

        public static PokemonManager Instance
        {
            get
            {
                XmlManager<PokemonManager> xml = new XmlManager<PokemonManager>();
                instance = xml.Load("Load/Pokemon/PokemonManager.xml");

                return instance;
            }
        }

        public Pokemon GetPokemon(string name)
        {
            foreach (Pokemon pokemon in Pokemon)
            {
                if (pokemon.Name == name)
                    return pokemon;
            }
            return null;
        }
    }
}
