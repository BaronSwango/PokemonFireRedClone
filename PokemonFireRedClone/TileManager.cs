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
                if (Instance == null)
                {
                    instance = new TileManager();
                }

                return instance;
            }
        }

        public Tile GetCurrentTile(Map map, Vector2 position)
        {
            foreach (Tile tile in map.Tiles)
            {
                if (tile.SourceRect.Contains(position.ToPoint()))
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
       
