using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class RotatingMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        private readonly Counter counter;

        public RotatingMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            counter = new Counter(randomGenerator.Next(3960) + 250);
        }

        public override void Update(GameTime gameTime, Map map)
        {
            counter.Update(gameTime);

            if (counter.Finished)
            {
                var direction = npc.MoveType switch
                {
                    NPC.MovementType.HORIZONTAL_ROTATE => randomGenerator.Next(2),
                    NPC.MovementType.VERTICAL_ROTATE => randomGenerator.Next(2) + 2,
                    _ => randomGenerator.Next(4),
                };

                npc.Sprite.SpriteSheetEffect.CurrentFrame.Y = direction;

                counter.Reset(randomGenerator.Next(3960) + 250);
            }
        }
    }
}