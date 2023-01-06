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

            foreach (Layer l in Layers)
            {
                l.LoadContent(TileDimensions);
                foreach (Tile tile in l.Tiles)
                {
                    if (tile.State == "Solid")
                    {
                        SolidTiles.Add(tile);
                        if ((tile.ID == "[4:0]" || tile.ID == "[4:1]" || tile.ID == "[3:2]" || tile.ID == "[0:13]") && l.Image.Path.Contains("Ground"))
                        {
                            tile.ID = tile.ID.Replace(']', ':') + signID + ":" + Name + "]";
                            signID++;
                        }
                        else if (((tile.ID == "[1:4]") && l.Image.Path.Contains("Buildings")) || (tile.ID == "[3:26]") && l.Image.Path.Contains("Interior"))
                        {
                            tile.ID = tile.ID.Replace(']', ':') + doorID + ":" + Name + "]";
                            doorID++;
                        }
                    }
                    Tiles.Add(tile);
                }

            }

            foreach (Area area in Areas)
                area.LoadContent();

            foreach (Trainer trainer in Trainers)
            {
                NPCs.Add(trainer);
            }

            foreach (NPC npc in NPCs)
                npc.LoadContent();
        }

        public void UnloadContent()
        {
            foreach (Layer l in Layers)
                l.UnloadContent();

            foreach (NPC npc in NPCs)
                npc.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (Layer l in Layers)
                l.Update(ref player);

            foreach (NPC npc in NPCs)
            {
                npc.Update(gameTime, this);

                Tile currentTile = TileManager.GetCurrentTile(this, npc.NPCSprite.Bottom, npc.NPCSprite.Bottom.SourceRect.Width / 2,
                    npc.NPCSprite.Bottom.SourceRect.Height / (int)npc.NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.Y);

                if (currentTile != null)
                {
                    currentTile.Entity = npc;
                    if (!NPCTiles.Contains(currentTile))
                        NPCTiles.Add(currentTile);

                }
            }

            foreach (Tile tile in NPCTiles)
            {
                if (tile.Entity != null && tile.Entity is NPC npc)
                {
                    Tile currentTile = TileManager.GetCurrentTile(this, npc.NPCSprite.Bottom, npc.NPCSprite.Bottom.SourceRect.Width / 2,
                        npc.NPCSprite.Bottom.SourceRect.Height / (int)npc.NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.Y);

                    if (currentTile != null && currentTile != tile)
                        tile.Entity = null;

                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer l in Layers)
                l.Draw(spriteBatch, drawType);
        }

    }
}
