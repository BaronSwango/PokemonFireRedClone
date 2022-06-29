using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class SummaryMenuManager
    {

        private SummaryPage currentPage;
        private bool isTransitioning;

        public void LoadContent(CustomPokemon pokemon)
        {
            currentPage = new PokemonInfo(pokemon);
            currentPage.LoadContent();
        }

        public void UnloadContent()
        {
            currentPage.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            //handle input to change pages
            if (InputManager.Instance.KeyPressed(Keys.Q))
                ScreenManager.Instance.ChangeScreens("PokemonScreen");
            else if (InputManager.Instance.KeyPressed(Keys.D) && !(currentPage is KnownMoves))
            {
                currentPage.UnloadContent();
                currentPage = currentPage is PokemonInfo ? new PokemonSkills(currentPage.Pokemon) : new KnownMoves(currentPage.Pokemon);
                currentPage.LoadContent();
            } else if (InputManager.Instance.KeyPressed(Keys.A) && !(currentPage is PokemonInfo))
            {
                currentPage.UnloadContent();
                currentPage = currentPage is PokemonSkills ? new PokemonInfo(currentPage.Pokemon) : new PokemonSkills(currentPage.Pokemon);
                currentPage.LoadContent();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentPage.Draw(spriteBatch);
        }

        private void TransitionForward()
        {

        }

        private void TransitionBackward()
        {

        }
    }
}
