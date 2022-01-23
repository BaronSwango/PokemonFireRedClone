using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{

    /* Creating TextBox for specific tiles
     * - Add unique tile id for each solid tile (signs, mailboxes)
     */ 

    public class Layer
    {
        public class TileMap
        {
            [XmlElement("Row")]
            public List<string> Row;

            public TileMap()
            {
                Row = new List<string>();
            }
        }

        [XmlElement("TileMap")]
        public TileMap Tile;
        List<Tile> underlayTiles, overlayTiles;
        public List<Tile> Tiles;
        public string SolidTiles, OverlayTiles;
        public Image Image;
        string state;


        public Layer()
        {
            Image = new Image();
            underlayTiles = new List<Tile>();
            overlayTiles = new List<Tile>();
            Tiles = new List<Tile>();
            SolidTiles = OverlayTiles = string.Empty;
        }

        public void LoadContent(Vector2 tileDimensions)
        {
            Image.LoadContent();
            Vector2 position = -tileDimensions;
            foreach (string row in Tile.Row)
            {
                string[] split = row.Split(']');
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;
                foreach(string s in split)
                { 
                    if (s != string.Empty)
                    {
                        position.X += tileDimensions.X;
                        if (!s.Contains("x"))
                        {
                            state = "Passive";
                            Tile tile = new Tile();
                            tile.ID = s + "]";

                            string str = s.Replace("[", string.Empty);
                            int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

                            if (SolidTiles.Contains(tile.ID))
                                state = "Solid";
                           

                            tile.LoadContent(position, new Rectangle(
                                value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), state);

                            if (OverlayTiles.Contains(s + "]"))
                                overlayTiles.Add(tile);
                            else
                                underlayTiles.Add(tile);
                            
                            Tiles.Add(tile);
                        }
                    }
                }
            }
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach(Tile tile in underlayTiles)
                tile.Update(gameTime, ref player);

            foreach (Tile tile in overlayTiles)
                tile.Update(gameTime, ref player);
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            List<Tile> tiles;
            if (drawType == "Underlay")
                tiles = underlayTiles;
            else
                tiles = overlayTiles;

            foreach (Tile tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }
    }
}
