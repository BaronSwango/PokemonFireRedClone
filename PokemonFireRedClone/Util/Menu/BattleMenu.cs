using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleMenu : Menu
    {

        public Image Arrow;
        public Image Background;

        protected override void AlignMenuItems()
        {
            Background.Position = new Vector2(ScreenManager.Instance.Dimensions.X - Background.SourceRect.Width,
                ScreenManager.Instance.Dimensions.Y - Background.SourceRect.Height);
        }

        public override void LoadContent()
        {
            Background.LoadContent();
            //Arrow.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            //Arrow.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            AlignMenuItems();
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            //Arrow.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
