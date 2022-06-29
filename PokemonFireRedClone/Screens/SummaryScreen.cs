using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class SummaryScreen : GameScreen
    {

        private readonly SummaryMenuManager menuManager;

        public SummaryScreen()
        {
            menuManager = new SummaryMenuManager();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            CustomPokemon pokemon = BattleLogic.Battle != null && BattleLogic.Battle.InBattle ? BattleLogic.Battle.BattlePokemonMenu[PokemonMenu.SelectedIndex]
                : Player.PlayerJsonObject.PokemonInBag[PokemonMenu.SelectedIndex];
            menuManager.LoadContent(pokemon);
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }

    }
}
