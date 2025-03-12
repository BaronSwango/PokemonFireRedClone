using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPCMovementManager
    {

        protected readonly NPC npc;
        protected Player Player
        {
            get
            {
                return ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player;
            }
        }

        public NPCMovementManager(NPC npc)
        {
            this.npc = npc;
        }

        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime, Map map) { }

        protected Vector2 CalculateDestination()
        {
            return npc.Direction switch
            {
                Entity.EntityDirection.Left => new(npc.Sprite.Position.X - 64, npc.Sprite.Position.Y),
                Entity.EntityDirection.Right => new(npc.Sprite.Position.X + 64, npc.Sprite.Position.Y),
                Entity.EntityDirection.Down => new(npc.Sprite.Position.X, npc.Sprite.Position.Y + 64),
                Entity.EntityDirection.Up => new(npc.Sprite.Position.X, npc.Sprite.Position.Y - 64),
                _ => new(npc.Sprite.Position.X, npc.Sprite.Position.Y - 64),
            };
        }

        protected bool Move(GameTime gameTime)
        {
            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * npc.MoveSpeed * 1.1);

            switch (npc.Direction)
            {
                case Entity.EntityDirection.Left:

                    if (npc.Sprite.Position.X - speed < (int)npc.Destination.X)
                    {
                        npc.Sprite.Position = npc.Destination;
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.Sprite.Position = new Vector2(npc.Sprite.Position.X - speed, npc.Sprite.Position.Y);

                        if (Math.Abs(npc.Sprite.Position.X - npc.Destination.X) < 32 && (npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                        {
                            npc.Sprite.SpriteSheetEffect.CurrentFrame.X = npc.Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        return false;
                    }

                case Entity.EntityDirection.Right:

                    if (npc.Sprite.Position.X + speed > (int)npc.Destination.X)
                    {
                        npc.Sprite.Position = npc.Destination;
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.Sprite.Position = new Vector2(npc.Sprite.Position.X + speed, npc.Sprite.Position.Y);

                        if (Math.Abs(npc.Sprite.Position.X - npc.Destination.X) < 32 && (npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                        {
                            npc.Sprite.SpriteSheetEffect.CurrentFrame.X = npc.Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        return false;
                    }

                case Entity.EntityDirection.Up:

                    if (npc.Sprite.Position.Y - speed < (int)npc.Destination.Y)
                    {
                        npc.Sprite.Position = npc.Destination;
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.Sprite.Position = new Vector2(npc.Sprite.Position.X, npc.Sprite.Position.Y - speed);

                        if (Math.Abs(npc.Sprite.Position.Y - npc.Destination.Y) < 32 && (npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                        {
                            npc.Sprite.SpriteSheetEffect.CurrentFrame.X = npc.Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        return false;
                    }

                case Entity.EntityDirection.Down:

                    if (npc.Sprite.Position.Y + speed > (int)npc.Destination.Y)
                    {
                        npc.Sprite.Position = npc.Destination;
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.Sprite.Position = new Vector2(npc.Sprite.Position.X, npc.Sprite.Position.Y + speed);

                        if (Math.Abs(npc.Sprite.Position.Y - npc.Destination.Y) < 32 && (npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                        {
                            npc.Sprite.SpriteSheetEffect.CurrentFrame.X = npc.Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        return false;
                    }

            }

            return false;
        }

    }
}
