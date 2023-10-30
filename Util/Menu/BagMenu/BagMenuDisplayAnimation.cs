using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class BagMenuDisplayAnimation : IAnimation
	{
        private readonly Image transitionBox;

        public BagMenuDisplayAnimation()
		{
            transitionBox = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, 576, 380)
            };
            Color[] data = new Color[576 * 380];
            for (int i = 0; i < data.Length; ++i) data[i] = new Color(214, 190, 110);
            transitionBox.Texture.SetData(data);

            transitionBox.LoadContent();
            transitionBox.Position = new(512, 72);
        }

        public void UnloadContent()
        {
            transitionBox.UnloadContent();
        }

        public bool Animate(GameTime gameTime)
        {
            float displaySpeed = (int)gameTime.ElapsedGameTime.TotalMilliseconds * 1.85f;

            if (transitionBox.SourceRect.Height - displaySpeed > 0)
            {
                transitionBox.SourceRect.Height -= (int) displaySpeed;
                return false;
            }

            transitionBox.SourceRect.Height = 0;
            return true;
        }

        public void Reset()
        {
            transitionBox.SourceRect.Height = 380;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            transitionBox.Draw(spriteBatch);
        }
    }
}

