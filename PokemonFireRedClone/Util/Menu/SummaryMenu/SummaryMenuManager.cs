using System;
using System.Collections.Generic;
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
            animation = new IntroBounceAnimation(CurrentPage.PokeImage);
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

            if (InputManager.Instance.KeyPressed(Keys.W) || InputManager.Instance.KeyPressed(Keys.S))
            {
                List<CustomPokemon> pokemon = BattleLogic.Battle != null && BattleLogic.Battle.InBattle ? BattleLogic.Battle.BattlePokemonInBag : Player.PlayerJsonObject.PokemonInBag;
                bool inBounds = InputManager.Instance.KeyPressed(Keys.W) ? pokemon.IndexOf(CurrentPage.Pokemon) > 0 : pokemon.IndexOf(CurrentPage.Pokemon) < pokemon.Count - 1;
                if (inBounds)
                {
                    int index = InputManager.Instance.KeyPressed(Keys.W) ? pokemon.IndexOf(CurrentPage.Pokemon) - 1 : pokemon.IndexOf(CurrentPage.Pokemon) + 1;
                    CurrentPage.UnloadContent();
                    CurrentPage = (SummaryPage)Activator.CreateInstance(CurrentPage.GetType(), pokemon[index]);
                    CurrentPage.LoadContent();
                    PokemonMenu.SelectedIndex = index;
                    animation = new IntroBounceAnimation(CurrentPage.PokeImage);
                    isAnimating = true;
                }
            }

            //handle input to change pages

            if (InputManager.Instance.KeyPressed(Keys.Q))
            {
                ScreenManager.Instance.ChangeScreens("PokemonScreen");
            }
            else if (InputManager.Instance.KeyPressed(Keys.D) && CurrentPage is not KnownMoves)
            {
                CurrentPage.UnloadContent();
                CurrentPage = CurrentPage is PokemonInfo ? new PokemonSkills(CurrentPage.Pokemon) : new KnownMoves(CurrentPage.Pokemon);
                CurrentPage.LoadContent();
            }
            else if (InputManager.Instance.KeyPressed(Keys.A) && CurrentPage is not PokemonInfo)
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
