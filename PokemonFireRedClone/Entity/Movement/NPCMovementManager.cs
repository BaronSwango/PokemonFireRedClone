using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPCMovementManager
    {

        private Player Player
        {
            get
            {
                return ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player;
            }
        }

        private readonly NPC npc;

        private float counter;
        private int counterLimit;
        private bool updateCounter;
        private readonly Random randomGenerator;
        private Entity.EntityDirection nextDirection;

        public NPCMovementManager(NPC npc)
        {
            this.npc = npc;
            if (npc.MoveType != NPC.MovementType.STILL)
            {
                randomGenerator = new Random();
                counterLimit = randomGenerator.Next(1500) + 500;
            }
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
                                Vector2 destination = new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y - 64);

                                if (Player.Sprite.Position == destination || Player.Destination == destination || Player.PreviousTile == destination)
                                    break;

                                if (!npc.IsMoving)
                                {
                                    npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                                    npc.IsMoving = true;
                                }

                                if (npc.Destination == npc.NPCSprite.Top.Position)
                                    npc.Destination = destination;

                                if (!Move(gameTime)) return;

                                nextDirection = Entity.EntityDirection.Right;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Right)
                            {
                                Vector2 destination = new(npc.NPCSprite.Top.Position.X + 64, npc.NPCSprite.Top.Position.Y);

                                if (Player.Sprite.Position == destination || Player.Destination == destination || Player.PreviousTile == destination)
                                    break;

                                if (!npc.IsMoving)
                                {
                                    npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                                    npc.IsMoving = true;
                                }

                                if (npc.Destination == npc.NPCSprite.Top.Position)
                                    npc.Destination = destination;
                                

                                if (!Move(gameTime)) return;

                                nextDirection = Entity.EntityDirection.Down;
                            }
                            else if (npc.Direction == Entity.EntityDirection.Down)
                            {
                                Vector2 destination = new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + 64);

                                if (Player.Sprite.Position == destination || Player.Destination == destination || Player.PreviousTile == destination)
                                    break;

                                if (!npc.IsMoving)
                                {
                                    npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                                    npc.IsMoving = true;
                                }

                                if (npc.Destination == npc.NPCSprite.Top.Position)
                                    npc.Destination = destination;
                                

                                if (!Move(gameTime)) return;

                                nextDirection = Entity.EntityDirection.Left;
                            }
                            else
                            {
                                Vector2 destination = new(npc.NPCSprite.Top.Position.X - 64, npc.NPCSprite.Top.Position.Y);

                                if (Player.Sprite.Position == destination || Player.Destination == destination || Player.PreviousTile == destination)
                                    break;

                                if (!npc.IsMoving)
                                {
                                    npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                                    npc.IsMoving = true;
                                }

                                if (npc.Destination == npc.NPCSprite.Top.Position)
                                    npc.Destination = destination;
                                

                                if (!Move(gameTime)) return;

                                nextDirection = Entity.EntityDirection.Up;
                            }
                            break;
                    }

                    counterLimit = randomGenerator.Next(1500) + 500;
                    counter = 0;
                    updateCounter = false;
                    npc.IsMoving = false;
                }
                else
                {
                    counter += counterSpeed;
                    if (counter >= counterLimit)
                    {

                        if (npc.MoveType == NPC.MovementType.SET_PATH)
                        {
                            switch (nextDirection)
                            {
                                case Entity.EntityDirection.Left:
                                    if (npc.Direction != Entity.EntityDirection.Left)
                                        npc.Direction = Entity.EntityDirection.Left;
                                    npc.NPCSprite.SetDirection(0);
                                    break;
                                case Entity.EntityDirection.Right:
                                    if (npc.Direction != Entity.EntityDirection.Right)
                                        npc.Direction = Entity.EntityDirection.Right;
                                    npc.NPCSprite.SetDirection(1);
                                    break;
                                case Entity.EntityDirection.Up:
                                    if (npc.Direction != Entity.EntityDirection.Up)
                                        npc.Direction = Entity.EntityDirection.Up;
                                    npc.NPCSprite.SetDirection(3);
                                    break;
                                case Entity.EntityDirection.Down:
                                    if (npc.Direction != Entity.EntityDirection.Down)
                                        npc.Direction = Entity.EntityDirection.Down;
                                    npc.NPCSprite.SetDirection(2);
                                    break;
                            }

                        }

                        updateCounter = true;
                    }
                }
            }
        }


        public bool Move(GameTime gameTime)
        {

            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * npc.MoveSpeed * 1.1);

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
                    {
                        npc.NPCSprite.SetPosition(npc.Destination);
                        npc.PreviousTile = npc.Destination;
                    }
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
                return true;
            }

            return false;
        }

    }
}
