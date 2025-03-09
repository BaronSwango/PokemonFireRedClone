using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public static class TileManager
    {
        public static Tile GetCurrentTile(Map map, Image image, int offsetX, int offSetY)
        {

            Rectangle playerRect = new((int)image.Position.X, (int)image.Position.Y,
                image.SourceRect.Width, image.SourceRect.Height);

            foreach (Tile tile in map.Tiles)
            {
                Rectangle tileRect = new((int)tile.Position.X + 4, (int)tile.Position.Y,
                    tile.SourceRect.Width - 8, tile.SourceRect.Height - 20);

                if (tileRect.Contains(new Vector2(playerRect.Location.X + offsetX, playerRect.Location.Y + offSetY)))
                    return tile;
            }
            return null;
        }

        // TODO: refactor these
        public static Tile GetCurrentTile(Map map, Image image, int offsetX, int offSetY, int layerIndex)
        {

            Rectangle playerRect = new((int)image.Position.X, (int)image.Position.Y,
                image.SourceRect.Width, image.SourceRect.Height);

            foreach (Tile tile in map.Layers[layerIndex].Tiles)
            {
                Rectangle tileRect = new((int)tile.Position.X + 4, (int)tile.Position.Y,
                    tile.SourceRect.Width - 8, tile.SourceRect.Height - 20);

                if (tileRect.Contains(new Vector2(playerRect.Location.X + offsetX, playerRect.Location.Y + offSetY)))
                    return tile;
            }
            return null;
        }

        // TODO: refactor based on direction rather than checking each individual one
        public static Tile GetTile(Map map, Tile currentTile, Entity.EntityDirection direction) 
        {
            if (currentTile == null) return null;

            int[][] directions = new int[4][]
            {
                new int[] { -64, -64 },
                new int[] { 64, -64 },
                new int[] { 0, 0 },
                new int[] { 0, -128 }
            };

            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + directions[(int) direction][0], currentTile.Position.Y + directions[(int) direction][1]))
                    && tile.ID.Contains(map.Name))
                    return tile;
            }

            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + directions[(int) direction][0], currentTile.Position.Y + directions[(int) direction][1])))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + directions[(int) direction][0], currentTile.Position.Y + directions[(int) direction][1])))
                    return tile;
            }

            foreach (Tile tile in map.GrassTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + directions[(int) direction][0], currentTile.Position.Y + directions[(int) direction][1])))
                    return tile;
            }

            return null;
        }

        public static bool IsTextBoxTile(GameplayScreen gameplayScreen, Tile tile)
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

        public static bool IsDoorTile(GameplayScreen gameplayScreen, Tile tile)
        {
            foreach (Door door in gameplayScreen.DoorManager.Doors)
            {
                if (tile != null)
                {
                    if (door.ID == tile.ID)
                        return true;
                }
            }
            return false;
        }

        public static Tile GetTile(Map map, Vector2 location)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(location))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(location))
                    return tile;
            }

            foreach (Tile tile in map.Layers[0].Tiles)
            {
                if (tile.Position.Equals(location))
                    return tile;
            }

            return null;
        }

        public static Tile GetTile(Map map, Vector2 location, int layerIndex)
        { 
            foreach (Tile tile in map.Layers[layerIndex].Tiles)
            {
                if (tile.Position.Equals(location))
                    return tile;
            }

            return null;
        }

        public static Tile GetTile(Map map, Tile currentTile, int offsetX, int offsetY)
        {

            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + offsetX, currentTile.Position.Y + offsetY)))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + offsetX, currentTile.Position.Y + offsetY)))
                    return tile;
            }

            foreach (Tile tile in map.Layers[0].Tiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + offsetX, currentTile.Position.Y + offsetY)))
                    return tile;
            }

            return null;

        }
    }
}