using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Map
    {

        private int solidID;

        [XmlElement("Layer")]
        public List<Layer> Layers;
        public List<Tile> Tiles;
        public List<Tile> SolidTiles;
        public List<Tile> NPCTiles;
        [XmlElement("NPC")]
        public List<NPC> NPCs;
        [XmlElement("Trainer")]
        public List<Trainer> Trainers;
        public Vector2 TileDimensions;

        public void LoadContent()
        {
            foreach (Layer l in Layers)
            {
                l.LoadContent(TileDimensions);
                foreach (Tile tile in l.Tiles)
                {
                    if (tile.State == "Solid")
                    {
                        SolidTiles.Add(tile);
                        if ((tile.ID == "[4:0]" || tile.ID == "[4:1]" || tile.ID == "[3:2]") && l.Image.Path.Contains("Ground"))
                        {
                            tile.ID = tile.ID.Replace(']', ':') + solidID + "]";
                            solidID++;
                        }
                    }
                    Tiles.Add(tile);
                }

            }

            foreach (Trainer trainer in Trainers)
                NPCs.Add(trainer);

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
                npc.Update(gameTime);
                Tile currentTile = TileManager.GetCurrentTile(this, npc.NPCSprite.Bottom, npc.NPCSprite.Bottom.SourceRect.Width / ((int) npc.NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.X * 2),
                    npc.NPCSprite.Bottom.SourceRect.Height / (int)npc.NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.Y);
                currentTile.Entity = npc;
                if (!NPCTiles.Contains(currentTile))
                    NPCTiles.Add(currentTile);
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer l in Layers)
                l.Draw(spriteBatch, drawType);
        }

    }
}
