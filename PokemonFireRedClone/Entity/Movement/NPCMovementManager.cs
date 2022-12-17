using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPCMovementManager
    {

        private readonly NPC npc;

        private float counter;
        private int counterLimit;
        private bool updateCounter;
        private readonly Random randomGenerator;

        public bool IsMoving;

        public NPCMovementManager(NPC npc)
        {
            this.npc = npc;
            if (npc.MoveType != NPC.MovementType.STILL)
            {
                randomGenerator = new Random();
                counterLimit = randomGenerator.Next(1500) + 500;
            }
            npc.Destination = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {

            if (npc.MoveType != NPC.MovementType.STILL)
            {
                float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (updateCounter)
                {

                    switch (npc.MoveType)
                    {
                        case NPC.MovementType.ROTATE:
                            npc.NPCSprite.SetDirection(randomGenerator.Next(4));
                            break;
                        case NPC.MovementType.SET_PATH:
                            if (npc.Direction == Entity.EntityDirection.Up)
                            { 
                                if (npc.Destination == Vector2.Zero)
                                    npc.Destination = new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - 64);

                                if (!Move(gameTime)) return;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Right)
                            {
                                if (npc.Destination == Vector2.Zero)
                                    npc.Destination = new Vector2(npc.NPCSprite.Top.Position.X + 64, npc.NPCSprite.Top.Position.Y);

                                if (!Move(gameTime)) return;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Down)
                            {
                                if (npc.Destination == Vector2.Zero)
                                    npc.Destination = new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + 64);

                                if (!Move(gameTime)) return;
                            } else
                            {
                                if (npc.Destination == Vector2.Zero)
                                    npc.Destination = new Vector2(npc.NPCSprite.Top.Position.X - 64, npc.NPCSprite.Top.Position.Y);

                                if (!Move(gameTime)) return;
                            }
                            break;
                    }

                    counterLimit = randomGenerator.Next(1500) + 500;
                    counter = 0;
                    updateCounter = false;
                    IsMoving = false;
                }
                else
                {
                    counter += counterSpeed;
                    if (counter >= counterLimit)
                    {
                        updateCounter = true;
                        if (npc.MoveType == NPC.MovementType.SET_PATH)
                        {
                            switch (npc.Direction)
                            {
                                case Entity.EntityDirection.Left:
                                    npc.NPCSprite.SetDirection(3);
                                    npc.Direction = Entity.EntityDirection.Up;
                                    break;
                                case Entity.EntityDirection.Right:
                                    npc.NPCSprite.SetDirection(2);
                                    npc.Direction = Entity.EntityDirection.Down;
                                    break;
                                case Entity.EntityDirection.Up:
                                    npc.NPCSprite.SetDirection(1);
                                    npc.Direction = Entity.EntityDirection.Right;
                                    break;
                                case Entity.EntityDirection.Down:
                                    npc.NPCSprite.SetDirection(0);
                                    npc.Direction = Entity.EntityDirection.Left;
                                    break;
                            }


                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                            IsMoving = true;
                        }
                    }
                }
            }
        }


        public bool Move(GameTime gameTime)
        {

            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * npc.MoveSpeed * 1.1);

            /*
            int frame = npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 0 && npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X != 1
                ? 3 : 1;

            npc.NPCSprite.SetFrame(frame);
            */
            //npc.NPCSprite.Top.IsActive = true;
            //npc.NPCSprite.Bottom.IsActive = true;

            switch (npc.Direction)
            {
                case Entity.EntityDirection.Left:
                    if (npc.NPCSprite.Top.Position.X - speed > npc.Destination.X)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X - speed, npc.NPCSprite.Top.Position.Y));

                        if (Math.Abs(npc.NPCSprite.Top.Position.X - npc.Destination.X) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);
                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(npc.Destination);
                    break;
                case Entity.EntityDirection.Right:
                    if (npc.NPCSprite.Top.Position.X + speed < npc.Destination.X)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X + speed, npc.NPCSprite.Top.Position.Y));

                        if (Math.Abs(npc.NPCSprite.Top.Position.X - npc.Destination.X) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(npc.Destination);
                    break;
                case Entity.EntityDirection.Up:
                    if (npc.NPCSprite.Top.Position.Y - speed > npc.Destination.Y)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - speed));

                        if (Math.Abs(npc.NPCSprite.Top.Position.Y - npc.Destination.Y) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(npc.Destination);
                    break;
                case Entity.EntityDirection.Down:
                    if (npc.NPCSprite.Top.Position.Y + speed < npc.Destination.Y)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + speed));

                        if (Math.Abs(npc.NPCSprite.Top.Position.Y - npc.Destination.Y) < 32 && (npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 1
                            || npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X == 3))
                            npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2);

                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(npc.Destination);
                    break;
            }

            if (npc.NPCSprite.Top.Position == npc.Destination)
            {
                int frame = npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 2 : 0;

                npc.NPCSprite.SetFrame(frame);
                npc.NPCSprite.Top.SpriteSheetEffect.FrameCounter = 0;
                npc.NPCSprite.Bottom.SpriteSheetEffect.FrameCounter = 0;
                npc.NPCSprite.Top.IsActive = false;
                npc.NPCSprite.Bottom.IsActive = false;
                npc.Destination = Vector2.Zero;
                return true;
            }

            return false;
        }

    }
}
