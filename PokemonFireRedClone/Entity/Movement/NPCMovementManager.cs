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
        private Vector2 destination;

        public NPCMovementManager(NPC npc)
        {
            this.npc = npc;
            if (npc.MoveType != NPC.MovementType.STILL)
            {
                randomGenerator = new Random();
                counterLimit = randomGenerator.Next(7500) + 500;
            }
            destination = Vector2.Zero;
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
                                if (destination == Vector2.Zero)
                                    destination = new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - 64);

                                if (!Move(gameTime)) return;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Right)
                            {
                                if (destination == Vector2.Zero)
                                    destination = new Vector2(npc.NPCSprite.Top.Position.X + 64, npc.NPCSprite.Top.Position.Y);

                                if (!Move(gameTime)) return;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Down)
                            {
                                if (destination == Vector2.Zero)
                                    destination = new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + 64);

                                if (!Move(gameTime)) return;
                            } else
                            {
                                if (destination == Vector2.Zero)
                                    destination = new Vector2(npc.NPCSprite.Top.Position.X - 64, npc.NPCSprite.Top.Position.Y);

                                if (!Move(gameTime)) return;
                            }
                            break;
                    }

                    counterLimit = randomGenerator.Next(7500) + 500;
                    counter = 0;
                    updateCounter = false;
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
                        }
                    }
                }
            }
        }


        public bool Move(GameTime gameTime)
        {

            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * npc.MoveSpeed);

            /*
            int frame = npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 0 && npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X != 1
                ? 3 : 1;

            npc.NPCSprite.SetFrame(frame);
            */
            npc.NPCSprite.Top.IsActive = true;
            npc.NPCSprite.Bottom.IsActive = true;

            switch (npc.Direction)
            {
                case Entity.EntityDirection.Left:
                    if (npc.NPCSprite.Top.Position.X - speed > destination.X)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X - speed, npc.NPCSprite.Top.Position.Y));
                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(destination);
                    break;
                case Entity.EntityDirection.Right:
                    if (npc.NPCSprite.Top.Position.X + speed < destination.X)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X + speed, npc.NPCSprite.Top.Position.Y));
                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(destination);
                    break;
                case Entity.EntityDirection.Up:
                    if (npc.NPCSprite.Top.Position.Y - speed > destination.Y)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - speed));
                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(destination);
                    break;
                case Entity.EntityDirection.Down:
                    if (npc.NPCSprite.Top.Position.Y + speed < destination.Y)
                    {
                        npc.NPCSprite.SetPosition(new Vector2(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + speed));
                        return false;
                    }
                    else
                        npc.NPCSprite.SetPosition(destination);
                    break;
            }

            if (npc.NPCSprite.Top.Position == destination)
            {
                //frame = frame == 1 ? 2 : 0;

                //npc.NPCSprite.SetFrame(frame);
                npc.NPCSprite.Top.SpriteSheetEffect.FrameCounter = 0;
                npc.NPCSprite.Bottom.SpriteSheetEffect.FrameCounter = 0;
                npc.NPCSprite.Top.IsActive = false;
                npc.NPCSprite.Bottom.IsActive = false;
                destination = Vector2.Zero;
                return true;
            }

            return false;
        }

    }
}
