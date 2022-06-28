using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class SummaryScreen : GameScreen
    {

        readonly SummaryMenuManager menuManager;

        public SummaryScreen()
        {
            menuManager = new SummaryMenuManager();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent(Player.PlayerJsonObject.PokemonInBag[PokemonMenu.SelectedIndex]);

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
