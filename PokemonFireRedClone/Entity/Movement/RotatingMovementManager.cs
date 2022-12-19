using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class RotatingMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        private float counter;
        private int counterLimit;
        private bool updateCounter;

        public RotatingMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            counterLimit = randomGenerator.Next(3960) + 250;
        }

        public override void Update(GameTime gameTime)
        {
            float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (updateCounter)
            {
                npc.NPCSprite.SetDirection(randomGenerator.Next(4));

                counterLimit = randomGenerator.Next(3960) + 250;
                counter = 0;
                updateCounter = false;
                npc.IsMoving = false;
            }
            else
            {
                counter += counterSpeed;

                if (counter >= counterLimit)
                    updateCounter = true;
            }

        }


    }
}
