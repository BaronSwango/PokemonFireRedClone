using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class BagScreen : GameScreen
	{
		public BagMenuManager MenuManager;

		public BagScreen()
		{
			MenuManager = new BagMenuManager();
		}

        public override void LoadContent()
        {
            base.LoadContent();
			MenuManager.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            MenuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MenuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            MenuManager.Draw(spriteBatch);
        }

    }
}

