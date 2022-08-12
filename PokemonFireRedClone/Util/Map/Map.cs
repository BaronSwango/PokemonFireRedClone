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
        public Vector2 TileDimensions;

        public Map()
        {
            Layers = new List<Layer>();
            Tiles = new List<Tile>();
            SolidTiles = new List<Tile>();
            TileDimensions = Vector2.Zero;
        }

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
<<<<<<< HEAD

            foreach (NPC npc in NPCs)
                npc.LoadContent();
=======
>>>>>>> parent of eee7c30 (Started work on NPCs)
        }

        public void UnloadContent()
        {
            foreach (Layer l in Layers)
                l.UnloadContent();
        }

        public void Update(ref Player player)
        {
            foreach (Layer l in Layers)
                l.Update(ref player);
<<<<<<< HEAD

            foreach (NPC npc in NPCs)
            {
                npc.Update(gameTime);
                //Tile currentTile = TileManager.GetCurrentTile(this, npc.Sprite, npc.Sprite.SourceRect.Width / 8,
                //npc.Sprite.SourceRect.Height / (int)npc.Sprite.SpriteSheetEffect.AmountOfFrames.Y);
                Tile currentTile = TileManager.GetCurrentTile(this, npc.NPCSprite.Bottom, npc.NPCSprite.Bottom.SourceRect.Width / 8,
                    npc.NPCSprite.Bottom.SourceRect.Height / (int)npc.NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.Y);
                currentTile.ContainsEntity = true;
            }
=======
>>>>>>> parent of eee7c30 (Started work on NPCs)
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer l in Layers)
                l.Draw(spriteBatch, drawType);
        }

    }
}
