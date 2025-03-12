using System.Collections.Generic;
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
                return npc.Sprite.Position.X == pathCoords[currentDestinationIndex].Key
                    && npc.Sprite.Position.Y == pathCoords[currentDestinationIndex].Value;
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
            npc.Sprite.SpriteSheetEffect.CurrentFrame.Y = (int) npc.Direction;
            npc.Destination = CalculateDestination();
            npc.IsMoving = true;
        }

        public override void Update(GameTime gameTime, Map map)
        {
            if (!npc.IsMoving && (Player.Sprite.Position == npc.Destination || Player.Destination == npc.Destination || Player.PreviousTile == npc.Destination))
                return;

            if (!npc.IsMoving)
            {
                npc.Sprite.SpriteSheetEffect.CurrentFrame.X = npc.Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                npc.Direction = CalculateDirection();
                npc.Sprite.SpriteSheetEffect.CurrentFrame.Y = (int) npc.Direction;
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
                npc.Sprite.SpriteSheetEffect.CurrentFrame.Y = (int) npc.Direction;
                npc.Destination = CalculateDestination();
                npc.IsMoving = false;
            }

        }

        private Entity.EntityDirection CalculateDirection()
        {
            int xOffset = pathCoords[currentDestinationIndex].Key - (int)npc.Sprite.Position.X;
            int yOffset = pathCoords[currentDestinationIndex].Value - (int)npc.Sprite.Position.Y;

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