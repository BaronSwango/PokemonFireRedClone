using System;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Item
    {
        public string Name;
        public Image Icon;
        public ItemType ItemType;
        public string Description;

        public virtual void Use(params Entity[] entities) { }
        public virtual void Give(CustomPokemon pokemon) { }
        public virtual void Toss() { }// defualt functionality
    }
}