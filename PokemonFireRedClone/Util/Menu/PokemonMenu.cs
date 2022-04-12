using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class PokemonMenu : Menu
    {

        public Image Background;

        protected override void AlignMenuItems()
        {
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player.Image.Position;

            Background.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);
        }

        public override void LoadContent()
        {
            Background.LoadContent();
            AlignMenuItems();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
        }
    }
}
