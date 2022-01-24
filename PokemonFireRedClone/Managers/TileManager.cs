using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class TileManager
    {

        private static TileManager instance;

        public static TileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TileManager();
                }

                return instance;
            }
        }


        public Tile GetCurrentTile(Map map, Image image, int offsetX, int offSetY)
        {

            Rectangle playerRect = new Rectangle((int)image.Position.X, (int)image.Position.Y,
                image.SourceRect.Width, image.SourceRect.Height);

            foreach (Tile tile in map.Tiles)
            {
                Rectangle tileRect = new Rectangle((int)tile.Position.X + 4, (int)tile.Position.Y,
                    tile.SourceRect.Width - 8, tile.SourceRect.Height - 20);

                if (tileRect.Contains(new Vector2(playerRect.Location.X + offsetX, playerRect.Location.Y + offSetY)))
                    return tile;
            }
            return null;
        }

        public Tile LeftTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X-64, currentTile.Position.Y-64)))
                    return tile;
            }
            return null;
        }

        public Tile RightTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X+64, currentTile.Position.Y-64)))
                    return tile;
            }
            return null;
        }

        public Tile UpTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y - 128)))
                    return tile;
            }
            return null;
        }

        public Tile DownTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y)))
                    return tile;
            }
            return null;
        }

        public bool IsTextBoxTile(GameplayScreen gameplayScreen, Tile tile)
        {
            foreach (TextBox textBox in gameplayScreen.TextBoxManager.TextBoxes)
            {
                if (tile != null)
                {
                    if (textBox.ID == tile.ID)
                        return true;
                }
            }
            return false;
        }

    }
}
       
