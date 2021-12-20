using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Map
    {
        [XmlElement("Layer")]
        public List<Layer> Layers;
        public List<Tile> Tiles;
        public Vector2 TileDimensions;

        public Map()
        {
            Layers = new List<Layer>();
            Tiles = new List<Tile>();
            TileDimensions = Vector2.Zero;
        }

        public void LoadContent()
        {
            foreach (Layer l in Layers)
            {
                l.LoadContent(TileDimensions);
                foreach (Tile tile in l.Tiles)
                    Tiles.Add(tile);
            }
        }

        public void UnloadContent()
        {
            foreach (Layer l in Layers)
                l.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (Layer l in Layers)
                l.Update(gameTime, ref player);
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer l in Layers)
                l.Draw(spriteBatch, drawType);
        }

    }
}
