using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Entity
    { 

        public enum EntityDirection {
            [XmlEnum("Left")]
            Left,
            [XmlEnum("Right")]
            Right,
            [XmlEnum("Down")]
            Down,
            [XmlEnum("Up")]
            Up
        }

        public enum MoveState { Idle, Left, Right, Down, Up }

        public Image Sprite;

        protected Vector2 Destination;
        public Vector2 SpawnLocation;
        public EntityDirection Direction;
        public MoveState State;
        public Vector2 SpriteFrames;
        public float MoveSpeed;

        public Entity()
        {
            Destination = Vector2.Zero;
            State = MoveState.Idle;
            MoveSpeed = 0.26f;
        }

        public virtual void LoadContent()
        {
            Sprite.LoadContent();
            Sprite.IsActive = true;
            Sprite.SpriteSheetEffect.AmountOfFrames = SpriteFrames;
            Sprite.SpriteSheetEffect.Entity = true;
            Sprite.Position = SpawnLocation;
            Sprite.SpriteSheetEffect.CurrentFrame.Y = (int) Direction;
        }

        public virtual void UnloadContent()
        {
            Sprite.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            Direction = Sprite.SpriteSheetEffect.CurrentFrame.Y > 3 ? (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y - 4 : (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y;

            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Spawn(Map map)
        {
            if (TileManager.GetCurrentTile(map, Sprite, Sprite.SourceRect.Width / 8, Sprite.SourceRect.Height / (int)Sprite.SpriteSheetEffect.AmountOfFrames.Y) != null)
            {
                Vector2 centerTile = new(TileManager.GetCurrentTile(map, Sprite, Sprite.SourceRect.Width / 8, Sprite.SourceRect.Height / (int)Sprite.SpriteSheetEffect.AmountOfFrames.Y).Position.X,
                TileManager.GetCurrentTile(map, Sprite, Sprite.SourceRect.Width / 8, Sprite.SourceRect.Height / (int)Sprite.SpriteSheetEffect.AmountOfFrames.Y).Position.Y - 84);
                Sprite.Position = centerTile;
            }
        }

    }
}
