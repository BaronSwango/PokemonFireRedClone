using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class SummaryScreen : GameScreen
    {

        public SummaryMenuManager MenuManager;

        public SummaryScreen()
        {
            MenuManager = new SummaryMenuManager();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            CustomPokemon pokemon = BattleLogic.Battle != null && BattleLogic.Battle.InBattle ? BattleLogic.Battle.BattlePokemonMenu[PokemonMenu.SelectedIndex]
                : Player.PlayerJsonObject.PokemonInBag[PokemonMenu.SelectedIndex];
            MenuManager.LoadContent(pokemon);
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
