using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class RotatingMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        //private float counter;
        //private int counterLimit;
        private readonly Counter counter;

        public RotatingMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            //counterLimit = randomGenerator.Next(3960) + 250;
            counter = new Counter(randomGenerator.Next(3960) + 250);
        }

        public override void Update(GameTime gameTime, Map map)
        {
            //float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //counter += counterSpeed;
            counter.Update(gameTime);

            //if (counter >= counterLimit)
            if (counter.Finished)
            {
                var direction = npc.MoveType switch
                {
                    NPC.MovementType.HORIZONTAL_ROTATE => randomGenerator.Next(2),
                    NPC.MovementType.VERTICAL_ROTATE => randomGenerator.Next(2) + 2,
                    _ => randomGenerator.Next(4),
                };

                npc.NPCSprite.SetDirection(direction);

                //counterLimit = randomGenerator.Next(3960) + 250;
                //counter = 0;
                counter.Reset(randomGenerator.Next(3960) + 250);
            }

        }
    }
}
