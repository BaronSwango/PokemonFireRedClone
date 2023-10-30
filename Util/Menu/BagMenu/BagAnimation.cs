using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class BagAnimation : IAnimation
	{
        private readonly Image bag;
        private readonly int originalY;
        private bool falling;

		public BagAnimation(Image bag)
		{
            this.bag = bag;
            originalY = (int) bag.Position.Y;
        }

        public bool Animate(GameTime gameTime)
        {
            if (!falling)
            {
                bag.SpriteSheetEffect.CurrentFrame.X = 0;
                bag.Position.Y -= 16;
                falling = true;
            }

            float fallSpeed = (float) gameTime.ElapsedGameTime.TotalMilliseconds / 5.5f;

            if (bag.Position.Y + fallSpeed < originalY)
            {
                bag.Position.Y += fallSpeed;
                return false;
            }

            bag.Position.Y = originalY;
            return true;
        }

        public void Reset()
        {
            falling = false;
        }

        public void Draw(SpriteBatch spriteBatch) {}
    }
}

