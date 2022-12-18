using System;
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

        public Vector2 PreviousTile;
        public Vector2 Destination;
        public Vector2 SpawnLocation;
        public EntityDirection Direction;
        public MoveState State;
        public Vector2 SpriteFrames;
        public float MoveSpeed;
        public bool IsMoving;
        public bool Colliding;

        public Entity()
        {
            Destination = Vector2.Zero;
            State = MoveState.Idle;
            MoveSpeed = 0.26f;
        }

        public virtual void LoadContent()
        {
            Sprite.LoadContent();
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
            Direction = Sprite.SpriteSheetEffect.CurrentFrame.Y > 3
                ? (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y - 4 : (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y;

            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public virtual void Spawn(ref Map map)
        {
            Tile currentTile =
                TileManager.GetCurrentTile(map, Sprite, Sprite.SourceRect.Width / 8, Sprite.SourceRect.Height / (int)Sprite.SpriteSheetEffect.AmountOfFrames.Y);
            if (currentTile != null)
            {
                Vector2 centerTile = new(currentTile.Position.X,
                currentTile.Position.Y - 84);
                Sprite.Position = centerTile;
            }
        }

        public void Collide(Rectangle rect)
        {
            if (State == MoveState.Left)
                Sprite.Position.X = rect.Right;
            else if (State == MoveState.Right)
                Sprite.Position.X = rect.Left - Sprite.SourceRect.Width;
            else if (State == MoveState.Up)
                Sprite.Position.Y = rect.Bottom;
            else
                Sprite.Position.Y = rect.Top - Sprite.SourceRect.Height;

            State = MoveState.Idle;
            Colliding = true;
        }

    }
}
