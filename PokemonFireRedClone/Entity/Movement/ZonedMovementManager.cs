using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class ZonedMovementManager : NPCMovementManager
    {

        private readonly Random randomGenerator;
        private float counter;
        private int counterLimit;
        private bool updateCounter;
        private Entity.EntityDirection nextDirection;

        public ZonedMovementManager(NPC npc) : base(npc)
        {
            randomGenerator = new Random();
            counterLimit = randomGenerator.Next(3960) + 250;
        }


        public override void Update(GameTime gameTime)
        {
            float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (updateCounter)
            {




                counterLimit = randomGenerator.Next(3960) + 250;
                counter = 0;
                updateCounter = false;
                npc.IsMoving = false;
            }
            else
            {
                counter += counterSpeed;
                if (counter >= counterLimit)
                {


                    updateCounter = true;
                }
            }

        }


    }
}
