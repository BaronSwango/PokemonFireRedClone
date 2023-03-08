using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
	public class PokemonListMenu : Menu
	{
		public Image PokemonListBackground;

        public override void LoadContent()
        {
            PokemonListBackground.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            PokemonListBackground.UnloadContent();
            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokemonListBackground.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

    }
}

