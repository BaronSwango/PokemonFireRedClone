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

        public static Tile LeftTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X - 64, currentTile.Position.Y - 64)))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X - 64, currentTile.Position.Y - 64)))
                    return tile;
            }

            return null;
        }

        public static Tile RightTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X+64, currentTile.Position.Y-64)))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X + 64, currentTile.Position.Y - 64)))
                    return tile;
            }

            return null;
        }

        public static Tile UpTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y - 128)))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y - 128)))
                    return tile;
            }

            return null;
        }

        public static Tile DownTile(Map map, Tile currentTile)
        {
            foreach (Tile tile in map.SolidTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y)))
                    return tile;
            }

            foreach (Tile tile in map.NPCTiles)
            {
                if (tile.Position.Equals(new Vector2(currentTile.Position.X, currentTile.Position.Y)))
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

    }
}
       
