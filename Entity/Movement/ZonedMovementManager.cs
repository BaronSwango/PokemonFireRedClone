using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class ZonedMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        private readonly Counter counter;
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
                Tile directionTile = TileManager.GetTile(map, currentTile, npc.Direction);
                return directionTile != null && directionTile.State == "Solid";
            }
            else return false;
        }

        public ZonedMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            counter = new Counter(randomGenerator.Next(3960) + 250);
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
            if (updateCounter)
            {
                if (!npc.IsMoving && (WillCollide(map) || OutOfBounds || Player.Sprite.Position == currentDestination || Player.Destination == currentDestination || Player.PreviousTile == currentDestination))
                {
                    counter.Reset(randomGenerator.Next(3960) + 250);
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
                    counter.Reset(randomGenerator.Next(3960) + 250);
                    updateCounter = false;
                    npc.IsMoving = false;
                }
            }
            else
            {
                counter.Update(gameTime);
                if (counter.Finished)
                {
                    Entity.EntityDirection newDirection;
                    if (zoneBounds.Key.Value == zoneBounds.Value.Value)
                    {
                        newDirection = (Entity.EntityDirection)randomGenerator.Next(2);
                    }
                    else if (zoneBounds.Key.Key == zoneBounds.Value.Key)
                    {
                        newDirection = (Entity.EntityDirection)randomGenerator.Next(2) + 2;
                    }
                    else
                        newDirection = (Entity.EntityDirection) randomGenerator.Next(4);

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