using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class SummaryMenuManager
    {

        private IntroBounceAnimation animation;
        private bool isAnimating;

        public SummaryPage CurrentPage;

        public void LoadContent(CustomPokemon pokemon)
        {
            CurrentPage = new PokemonInfo(pokemon);
            CurrentPage.LoadContent();
            animation = new IntroBounceAnimation(CurrentPage.PokeImage.Position.Y);
            isAnimating = true;
        }

        public void UnloadContent()
        {
            CurrentPage.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (isAnimating)
            {
                isAnimating = animation.Animate(gameTime);
                return;
            }
            //handle input to change pages
            if (InputManager.Instance.KeyPressed(Keys.Q))
                ScreenManager.Instance.ChangeScreens("PokemonScreen");
            else if (InputManager.Instance.KeyPressed(Keys.D) && !(CurrentPage is KnownMoves))
            {
                CurrentPage.UnloadContent();
                CurrentPage = CurrentPage is PokemonInfo ? new PokemonSkills(CurrentPage.Pokemon) : new KnownMoves(CurrentPage.Pokemon);
                CurrentPage.LoadContent();
            } else if (InputManager.Instance.KeyPressed(Keys.A) && !(CurrentPage is PokemonInfo))
            {
                CurrentPage.UnloadContent();
                CurrentPage = CurrentPage is PokemonSkills ? new PokemonInfo(CurrentPage.Pokemon) : new PokemonSkills(CurrentPage.Pokemon);
                CurrentPage.LoadContent();
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentPage.Draw(spriteBatch);
        }
    }
}
