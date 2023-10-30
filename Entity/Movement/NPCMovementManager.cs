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
                Entity.EntityDirection.Left => new(npc.NPCSprite.Top.Position.X - 64, npc.NPCSprite.Top.Position.Y),
                Entity.EntityDirection.Right => new(npc.NPCSprite.Top.Position.X + 64, npc.NPCSprite.Top.Position.Y),
                Entity.EntityDirection.Down => new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + 64),
                Entity.EntityDirection.Up => new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - 64),
                _ => new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - 64),
            };
        }

        protected bool Move(GameTime gameTime)
        {
            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * npc.MoveSpeed * 1.1);

            switch (npc.Direction)
            {
                case Entity.EntityDirection.Left:

                    if (npc.NPCSprite.Top.Position.X - speed < (int)npc.Destination.X)
                    {
                        npc.NPCSprite.SetPosition(npc.Destination);
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X - speed, npc.NPCSprite.Top.Position.Y));

                        if (Math.Abs(npc.NPCSprite.Top.Position.X - npc.Destination.X) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }

                case Entity.EntityDirection.Right:

                    if (npc.NPCSprite.Top.Position.X + speed > (int)npc.Destination.X)
                    {
                        npc.NPCSprite.SetPosition(npc.Destination);
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X + speed, npc.NPCSprite.Top.Position.Y));

                        if (Math.Abs(npc.NPCSprite.Top.Position.X - npc.Destination.X) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }

                case Entity.EntityDirection.Up:

                    if (npc.NPCSprite.Top.Position.Y - speed < (int)npc.Destination.Y)
                    {
                        npc.NPCSprite.SetPosition(npc.Destination);
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - speed));

                        if (Math.Abs(npc.NPCSprite.Top.Position.Y - npc.Destination.Y) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }

                case Entity.EntityDirection.Down:

                    if (npc.NPCSprite.Top.Position.Y + speed > (int)npc.Destination.Y)
                    {
                        npc.NPCSprite.SetPosition(npc.Destination);
                        npc.PreviousTile = npc.Destination;
                        return true;
                    }
                    else
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + speed));

                        if (Math.Abs(npc.NPCSprite.Top.Position.Y - npc.Destination.Y) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }

            }

            return false;
        }

    }
}
