using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class GrassTileAnimation : ITileAnimation
    {
        private readonly Entity entity;
        private readonly Tile grassTile;
        private readonly Counter counter;
        private const int ANIMATION_DURATION = 170; // Animation lasts for half a second

        public GrassTileAnimation(Entity entity, Tile grassTile)
        {
            this.entity = entity;
            this.grassTile = grassTile;
            counter = new Counter(ANIMATION_DURATION);
        }

        public void LoadContent()
        {
            // Change the tile frame to the "rustling" grass frame
            string str = grassTile.ID.Replace("[", string.Empty).Replace("]", string.Empty);
            int value1 = int.Parse(str[..str.IndexOf(':')]) + 1; // Use the next tile in the spritesheet
            int value2 = int.Parse(str[(str.IndexOf(':') + 1)..]);
            grassTile.ID = "[" + value1 + ":" + value2 + "]";
            
            // Update the tile's source rectangle to show the rustling frame
            grassTile.LoadContent(
                grassTile.Position, 
                new Rectangle(
                    value1 * grassTile.SourceRect.Width, 
                    value2 * grassTile.SourceRect.Height,
                    grassTile.SourceRect.Width, 
                    grassTile.SourceRect.Height
                ), 
                grassTile.State);
            
            // isAnimating = true;
        }

        public void UnloadContent()
        {
            // No resources to unload
        }

        public bool Animate(GameTime gameTime)
        {
            counter.Update(gameTime);
            
            // Once animation is complete, change back to original grass tile
            if (counter.Finished)
            {
                // Reset the tile to its original state
                string str = grassTile.ID.Replace("[", string.Empty).Replace("]", string.Empty);
                int value1 = int.Parse(str[..str.IndexOf(':')]) - 1; 
                int value2 = int.Parse(str[(str.IndexOf(':') + 1)..]);
                grassTile.ID = "[" + value1 + ":" + value2 + "]";
                
                grassTile.LoadContent(
                    grassTile.Position, 
                    new Rectangle(
                        value1 * grassTile.SourceRect.Width, 
                        value2 * grassTile.SourceRect.Height,
                        grassTile.SourceRect.Width, 
                        grassTile.SourceRect.Height
                    ),
                    grassTile.State
                );
                
                return true; // Remove this animation from the manager
            }
            
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // No additional drawing needed as the tile itself will be drawn by the map rendering system
        }
    }
}