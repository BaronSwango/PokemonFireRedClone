using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class PokedexScreen : GameScreen
	{
        public readonly MenuManager MenuManager;

        public PokedexScreen()
		{
			MenuManager = new MenuManager("PokedexMenu");
		}

        public override void LoadContent()
		{
            base.LoadContent();
            MenuManager.LoadContent("Load/Menus/PokedexMenu.xml");
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
            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            MenuManager.Draw(spriteBatch);
        }

    }
}

