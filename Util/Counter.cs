using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Counter
    {

        private float counter;
        private float limit;

        public bool Finished
        {
            get { return counter >= limit; }
            private set { }
        }

        public bool Running
        {
            get { return counter > 0; }
            private set { }
        }

        public Counter(int milliseconds)
        {
            counter = 0;
            limit = milliseconds;
        }

        public void Finish()
        {
            counter = limit;
        }

        public void Update(GameTime gameTime)
        {
            counter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Reset()
        {
            counter = 0;
        }

        public void Reset(float milliseconds)
        {
            limit = milliseconds;
            counter = 0;
        }

    }
}
