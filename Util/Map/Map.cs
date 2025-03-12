using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace PokemonFireRedClone
{
    public class Map
    {
        private int signID;
        private int doorID;
        public string Name;
        public bool Inside;
        [XmlElement("Layer")]
        public List<Layer> Layers;
        [XmlIgnore]
        public List<Tile> Tiles;
        [XmlIgnore]
        public List<Tile> SolidTiles;
        [XmlIgnore]
        public List<Tile> NPCTiles;
        [XmlIgnore]
        public List<Tile> GrassTiles;
        [XmlElement("NPC")]
        public List<NPC> NPCs;
        [XmlElement("Trainer")]
        public List<Trainer> Trainers;
        [XmlElement("Area")]
        public List<Area> Areas;
        public Vector2 TileDimensions;

        public void LoadContent()
        {
            Tiles = new List<Tile>();
            SolidTiles = new List<Tile>();
            NPCTiles = new List<Tile>();
            GrassTiles = new List<Tile>();

            foreach (Layer l in Layers)
            {
                l.LoadContent(TileDimensions);
                foreach (Tile tile in l.Tiles)
                {
                    if (tile.State == "Solid")
                    {
                        SolidTiles.Add(tile);
                        if (l.SignTiles != null && l.SignTiles.Contains(tile.ID))
                        {
                            tile.ID = tile.ID.Replace(']', ':') + signID + ":" + Name + "]";
                            signID++;
                            Console.WriteLine(tile.ID);
                        }
                        if (l.DoorTiles != null && l.DoorTiles.Contains(tile.ID))
                        {
                            tile.ID = tile.ID.Replace(']', ':') + doorID + ":" + Name + "]";
                            doorID++;
                            Console.WriteLine(tile.ID);
                        }
                    }

                    if (tile.ID == "[1:1]") {
                        GrassTiles.Add(tile);
                    }
                    Tiles.Add(tile);
                }
            }

            foreach (Area area in Areas)
            {
                area.LoadContent();
            }

            foreach (Trainer trainer in Trainers)
            {
                NPCs.Add(trainer);
            }

            foreach (NPC npc in NPCs)
            {
                npc.LoadContent();
            }
        }

        public void UnloadContent()
        {
            foreach (Layer l in Layers)
            {
                l.UnloadContent();
            }

            foreach (NPC npc in NPCs)
            {
                npc.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            TileAnimationManager.Instance.Update(gameTime);

            foreach (Layer l in Layers)
            {
                l.Update(player, NPCs, NPCTiles, gameTime, TileDimensions);
            }

            foreach (NPC npc in NPCs)
            {
                npc.Update(gameTime, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer l in Layers)
            {
                l.Draw(spriteBatch, drawType);
            }
        }

    }
}