using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class ZonedMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        private float counter;
        private int counterLimit;
        private bool updateCounter;
        private KeyValuePair<KeyValuePair<int, int>, KeyValuePair<int, int>> zoneBounds;
        private Vector2 currentDestination;
        private bool OutOfBounds
        {
            get   
            {
                return currentDestination.X < zoneBounds.Key.Key
                    || currentDestination.Y < zoneBounds.Key.Value
                    || currentDestination.X > zoneBounds.Value.Key
                    || currentDestination.Y > zoneBounds.Value.Value;
            }
        }
        private bool WillCollide(Map map)
        {
            Tile currentTile = TileManager.GetTile(map, new(npc.NPCSprite.Top.Position.X, npc.NPCSprite.Top.Position.Y + 84));

            if (currentTile != null)
            {
                return
                    (npc.Direction == Entity.EntityDirection.Left && TileManager.LeftTile(map, currentTile) != null && TileManager.LeftTile(map, currentTile).State == "Solid") ||
                    (npc.Direction == Entity.EntityDirection.Right && TileManager.RightTile(map, currentTile) != null && TileManager.RightTile(map, currentTile).State == "Solid") ||
                    (npc.Direction == Entity.EntityDirection.Up && TileManager.UpTile(map, currentTile) != null && TileManager.UpTile(map, currentTile).State == "Solid") ||
                    (npc.Direction == Entity.EntityDirection.Down && TileManager.DownTile(map, currentTile) != null && TileManager.DownTile(map, currentTile).State == "Solid");
            }
            else return false;
        }

        public ZonedMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            counterLimit = randomGenerator.Next(3960) + 250;
        }

        public override void LoadContent()
        {
            zoneBounds = new KeyValuePair<KeyValuePair<int, int>, KeyValuePair<int, int>>(
                new KeyValuePair<int, int>(int.Parse(npc.Zone[0].Split(',')[0]), int.Parse(npc.Zone[0].Split(',')[1])),
                new KeyValuePair<int, int>(int.Parse(npc.Zone[1].Split(',')[0]), int.Parse(npc.Zone[1].Split(',')[1]))
            );
        }

        public override void Update(GameTime gameTime, Map map)
        {
            float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (updateCounter)
            {
                if (!npc.IsMoving && (WillCollide(map) || OutOfBounds || Player.Sprite.Position == currentDestination || Player.Destination == currentDestination || Player.PreviousTile == currentDestination))
                {
                    counterLimit = randomGenerator.Next(3960) + 250;
                    counter = 0;
                    updateCounter = false;
                    return;
                }

                if (!npc.IsMoving)
                {
                    npc.NPCSprite.SetFrame(npc.NPCSprite.Top.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1);
                    npc.IsMoving = true;
                }

                npc.Destination = currentDestination;


                if (Move(gameTime))
                {
                    counterLimit = randomGenerator.Next(3960) + 250;
                    counter = 0;
                    updateCounter = false;
                    npc.IsMoving = false;
                }
            }
            else
            {
                counter += counterSpeed;
                if (counter >= counterLimit)
                {
                    Entity.EntityDirection newDirection = (Entity.EntityDirection) randomGenerator.Next(4);

                    if (npc.Direction != newDirection)
                        npc.Direction = newDirection;

                    npc.NPCSprite.SetDirection((int)newDirection);
                    currentDestination = CalculateDestination();

                    updateCounter = true;
                }
            }

        }

    }
}
