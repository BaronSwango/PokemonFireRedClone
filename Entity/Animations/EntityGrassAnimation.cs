using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone 
{
    public class EntityGrassAnimation : IEntityAnimation
    {
        private readonly Entity entity;
        private readonly Tile grassTile;
        private readonly Map map;
        private readonly Rectangle triggerArea;

        public EntityGrassAnimation(Entity entity, Tile grassTile, Map map)
        {
            this.entity = entity;
            this.grassTile = grassTile;
            this.map = map;
            triggerArea = new Rectangle((int)(grassTile.Position.X + 8), (int)(grassTile.Position.Y + 24), 48, 40);
        }

        public void LoadContent()
        { 
            // change the tile frame 
            string str = grassTile.ID.Replace("[", string.Empty).Replace("]", string.Empty);
            int value1 = int.Parse(str[..str.IndexOf(':')]) + 1;
            int value2 = int.Parse(str[(str.IndexOf(':') + 1)..]);

            grassTile.ID = "[" + value1 + ":" + value2 + "]";
            grassTile.LoadContent(grassTile.Position, new Rectangle(
                                value1 * (int)map.TileDimensions.X, value2 * (int)map.TileDimensions.Y,
                                (int)map.TileDimensions.X, (int)map.TileDimensions.Y), grassTile.State);
        }

        public void UnloadContent() { }

        public bool Animate(GameTime gameTime)
        {
            Console.WriteLine(entity.Sprite.Position.X + ", " + (entity.Sprite.Position.Y + entity.Sprite.SourceRect.Height));
            Console.WriteLine(triggerArea);
            if (triggerArea.Contains(entity.Sprite.Position.X + (entity.Sprite.SourceRect.Width / 2), entity.Sprite.Position.Y + entity.Sprite.SourceRect.Height - 16)) 
            {
                string str = grassTile.ID.Replace("[", string.Empty).Replace("]", string.Empty);
                int value1 = int.Parse(str[..str.IndexOf(':')]) - 1;
                int value2 = int.Parse(str[(str.IndexOf(':') + 1)..]);

                grassTile.ID = "[" + value1 + ":" + value2 + "]";
                grassTile.LoadContent(grassTile.Position, new Rectangle(
                                    value1 * (int)map.TileDimensions.X, value2 * (int)map.TileDimensions.Y,
                                    (int)map.TileDimensions.X, (int)map.TileDimensions.Y), grassTile.State); 
                return true;
            }
            /*
            * if center of player is outside of the bottom area of the grass tile, return false
            * else, change grass tile back to original state and return true
            */ 


            return false;
        }

        public void Draw(SpriteBatch spriteBatch) { }

        public bool CanMove() { return true; }

        public void PostDraw(SpriteBatch spriteBatch)
        {
            // draw animation after grass tile is reset in Animate function
        }
    }
}