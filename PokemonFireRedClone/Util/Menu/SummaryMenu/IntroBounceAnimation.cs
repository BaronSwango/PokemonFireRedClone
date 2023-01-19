using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class IntroBounceAnimation : IAnimation
    {
        private double speedMultiplier;
        //private double counter;
        private readonly Counter counter;
        private readonly float spriteHeight;
        private int bounces;
        private bool bounceUp;
        private int offset;
        private readonly Image pokeImage;

        public IntroBounceAnimation(Image pokeImage)
        {
            spriteHeight = pokeImage.Position.Y;
            bounceUp = true;
            speedMultiplier = 5;
            this.pokeImage = pokeImage;
            counter = new Counter(10);
        }

        public bool Animate(GameTime gameTime)
        {
            //double bounceSpeed = speedMultiplier*gameTime.ElapsedGameTime.TotalMilliseconds;

            //counter += bounceSpeed;
            counter.Update(gameTime);

            if (bounceUp)
            {
                //if (counter > 10)
                if (counter.Finished)
                {
                    offset -= 4;
                    //counter = 0;
                    speedMultiplier *= 0.7;
                    if (offset == -24)
                        speedMultiplier *= 0.7;
                    counter.Reset((float) (10 / speedMultiplier));
                }
            }
            else
            {
                //if (counter > 10)
                if (counter.Finished)
                {
                    offset += 4;
                    //counter = 0;
                    speedMultiplier /= 0.7;
                    if (offset == -20)
                    {
                        speedMultiplier /= 0.7;
                    }
                    counter.Reset((float)(10 / speedMultiplier));
                }
            }

            if (offset <= -32 && bounceUp)
                bounceUp = false;
            else if (offset >= 0 && !bounceUp)
            {
                offset = 0;
                bounceUp = true;
                bounces++;
                if (bounces == 2)
                {
                    pokeImage.Position.Y = spriteHeight;
                    return false;
                }
            }

            pokeImage.Position.Y = spriteHeight + offset;
            return true;

        }

        public void Draw(SpriteBatch spriteBatch){}
    }
}
