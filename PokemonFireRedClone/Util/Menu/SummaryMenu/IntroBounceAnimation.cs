using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class IntroBounceAnimation : IAnimation
    {
        private double speedMultiplier;
        private double counter;
        private readonly float spriteHeight;
        private int bounces;
        private bool bounceUp;
        private int offset;

        public IntroBounceAnimation(float spriteHeight)
        {
            this.spriteHeight = spriteHeight;
            bounceUp = true;
            speedMultiplier = 2;
        }

        public bool Animate(GameTime gameTime)
        {
            ref Image sprite = ref ((SummaryScreen)ScreenManager.Instance.CurrentScreen).MenuManager.CurrentPage.PokeImage;
            double bounceSpeed = speedMultiplier*gameTime.ElapsedGameTime.TotalMilliseconds;

            counter += bounceSpeed;

            if (bounceUp)
            {
                if (counter > 10)
                {
                    offset -= 4;
                    counter = 0;
                    if (offset <= -24)
                        speedMultiplier *= 0.4;
                }
            }
            else
            {
                if (counter > 10)
                {
                    offset += 4;
                    counter = 0;
                    if (offset <= -20)
                        speedMultiplier /= 0.4;
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
                    sprite.Position.Y = spriteHeight;
                    return false;
                }
            }

            sprite.Position.Y = spriteHeight + offset;
            return true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
