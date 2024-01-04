using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class ItemManager
    {

        private static ItemManager instance;

        [XmlElement("Items")]
        public List<Item> Items;

        public static ItemManager Instance
        {
            get
            {
                XmlManager<ItemManager> xml = new();
                instance = xml.Load("Load/Items/ItemManager.xml");

                return instance;
            }
        }

        public Item GetItem(string name)
        {
            foreach (Item item in Items)
            {
                if (item.Name.ToLower() == name.ToLower())
                {
                    return item;
                }
            }
            return null;
        }
    }
}