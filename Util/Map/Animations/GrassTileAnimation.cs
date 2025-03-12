using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class GrassTileAnimation : ITileAnimation
    {
        private readonly Entity entity;
        private readonly Tile grassTile;
        private readonly Image grassOverlay;
        private readonly Counter counter;
        private readonly Rectangle tileRect;
        private Rectangle entityRect;
        private bool grassBounce;
        private const int ANIMATION_DURATION = 170; // Animation lasts for half a second

        public GrassTileAnimation(Entity entity, Tile grassTile)
        {
            this.entity = entity;
            this.grassTile = grassTile;

            counter = new Counter(ANIMATION_DURATION);
            grassOverlay = new Image{
                Path = "Gameplay/AnimationEffects/GrassOverlay",
                Effects = "SpriteSheetEffect",
                Position = new(grassTile.Position.X, grassTile.Position.Y)
            };
            tileRect = new((int)grassTile.Position.X, (int)grassTile.Position.Y,
                grassTile.SourceRect.Width, grassTile.SourceRect.Height - 20);
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
                grassTile.State
            );

            grassOverlay.LoadContent();
            grassOverlay.SpriteSheetEffect.AmountOfFrames = new(4, 1);
            grassOverlay.SpriteSheetEffect.CurrentFrame.X = 0;
            grassOverlay.SpriteSheetEffect.SwitchFrame = 160;
            grassOverlay.SpriteSheetEffect.SetupSourceRects();
        }

        public void UnloadContent()
        {
            grassOverlay.UnloadContent();
        }

        public bool Animate(GameTime gameTime)
        {
            entityRect = new((int)entity.Sprite.Position.X, (int)entity.Sprite.Position.Y, entity.Sprite.SourceRect.Width, 84); 

            counter.Update(gameTime);
            grassOverlay.Update(gameTime);
            
            // Once animation is complete, change back to original grass tile
            if (counter.Finished)
            {
                if (!grassBounce)
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

                    grassBounce = true;
                    grassOverlay.IsActive = true;
                }

                if (grassOverlay.SpriteSheetEffect.CurrentFrame.X == 3)
                {
                    grassOverlay.IsActive = false;
                }

                if (!entityRect.Intersects(tileRect) && grassOverlay.SpriteSheetEffect.CurrentFrame.X == 3)
                {
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int entityFeetPos = (int) entity.Sprite.Position.Y + 84;
            if (grassBounce && entityFeetPos > grassTile.Position.Y + 64)
            {
                grassOverlay.Draw(spriteBatch);
            }
        }

        public void PostDraw(SpriteBatch spriteBatch)
        {
            int entityFeetPos = (int) entity.Sprite.Position.Y + 84;
            if (grassBounce && entityFeetPos <= grassTile.Position.Y + 64)
            {
                grassOverlay.Draw(spriteBatch);
            }
        }

        // TODO: remove all animations when entities spawn
    }
}