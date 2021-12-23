using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

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

        public Tile GetCurrentTile(Map map, Image image, int offSet)
        {

            foreach (Tile tile in map.Tiles)
            {
                Rectangle tileRect = new Rectangle((int)tile.Position.X + 4, (int)tile.Position.Y,
                    tile.SourceRect.Width - 8, tile.SourceRect.Height - 20);
                Rectangle playerRect = new Rectangle((int)image.Position.X, (int)image.Position.Y,
                    image.SourceRect.Width, image.SourceRect.Height);
                if (tileRect.Contains(new Vector2(playerRect.Location.X + (playerRect.Width/8), playerRect.Location.Y + offSet)))
                    return tile;
            }
            return null;
        }
        
        public Tile LeftTile(Map map, Tile currentTile)
        {
            Vector2 position = new Vector2(currentTile.Center.X - 64, currentTile.Center.Y);

            foreach (Tile tile in map.Tiles)
            {
                if (tile.SourceRect.Contains(position.ToPoint()))
                    return tile;
            }

            return null;
        }

        public Tile RightTile(Map map, Tile currentTile)
        {
            Vector2 position = new Vector2(currentTile.Center.X, currentTile.Center.Y);

            foreach (Tile tile in map.Tiles)
            {
                if (tile.SourceRect.Contains(position.ToPoint()))
                    return tile;
            }

            return null;
        }

        public Tile UpTile(Map map, Tile currentTile)
        {
            Vector2 position = new Vector2(currentTile.Center.X, currentTile.Center.Y - 64);

            foreach (Tile tile in map.Tiles)
            {
                if (tile.SourceRect.Contains(position.ToPoint()))
                    return tile;
            }

            return null;
        }

        public Tile DownTile(Map map, Tile currentTile)
        {
            Vector2 position = new Vector2(currentTile.Center.X, currentTile.Center.Y + 64);

            foreach (Tile tile in map.Tiles)
            {
                if (tile.SourceRect.Contains(position.ToPoint()))
                    return tile;
            }

            return null;
        }

    }
}
       
