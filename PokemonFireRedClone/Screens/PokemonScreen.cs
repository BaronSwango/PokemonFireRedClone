using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonScreen : GameScreen
    {
        private readonly MenuManager menuManager;

        public PokemonScreen()
        {
            menuManager = new MenuManager("PokemonMenu");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent("Load/Menus/PokemonMenu.xml");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            //if (InputManager.Instance.KeyPressed(Keys.Q))
                //ScreenManager.Instance.ChangeScreens(ScreenManager.Instance.PreviousScreen.Type.ToString().Replace("PokemonFireRedClone.", ""));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }

    }
}
