﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class SetPathMovementManager : NPCMovementManager
    {

        private List<KeyValuePair<int, int>> pathCoords;
        private int currentDestinationIndex;
        private bool ReachedDestination
        {
            get
            {
                return npc.NPCSprite.Top.Position.X == pathCoords[currentDestinationIndex].Key
                    && npc.NPCSprite.Top.Position.Y == pathCoords[currentDestinationIndex].Value;
            }
        }

        public SetPathMovementManager(NPC npc) : base(npc)
        {
            currentDestinationIndex = 1;
        }

        public override void LoadContent()
        {
            pathCoords = new List<KeyValuePair<int, int>>();
            foreach (string coordPair in npc.PathCoordsXML)
                pathCoords.Add(new KeyValuePair<int, int>(int.Parse(coordPair.Split(',')[0]),
                    int.Parse(coordPair.Split(',')[1])));

            npc.Direction = CalculateDirection();
            npc.NPCSprite.SetDirection((int)npc.Direction);
            npc.Destination = CalculateDestination();
            npc.IsMoving = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!npc.IsMoving && (Player.Sprite.Position == npc.Destination || Player.Destination == npc.Destination || Player.PreviousTile == npc.Destination))
                return;

            if (!npc.IsMoving)
            {
                npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                npc.Direction = CalculateDirection();
                npc.NPCSprite.SetDirection((int)npc.Direction);
            }

            npc.IsMoving = true;

            if (Move(gameTime))
            {
                if (ReachedDestination)
                {
                    currentDestinationIndex++;
                    if (currentDestinationIndex == pathCoords.Count)
                        currentDestinationIndex = 0;
                }

                npc.Direction = CalculateDirection();
                npc.NPCSprite.SetDirection((int)npc.Direction);
                npc.Destination = CalculateDestination();
                npc.IsMoving = false;
            }

        }

        private Vector2 CalculateDestination()
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


        private Entity.EntityDirection CalculateDirection()
        {
            int xOffset = pathCoords[currentDestinationIndex].Key - (int)npc.NPCSprite.Top.Position.X;
            int yOffset = pathCoords[currentDestinationIndex].Value - (int)npc.NPCSprite.Top.Position.Y;

            if (xOffset > 0)
                return Entity.EntityDirection.Right;

            if (xOffset < 0)
                return Entity.EntityDirection.Left;

            if (yOffset > 0)
                return Entity.EntityDirection.Down;

            return Entity.EntityDirection.Up;
        }
    }
}