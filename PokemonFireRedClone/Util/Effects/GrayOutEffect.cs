using System;
using Microsoft.Xna.Framework;


namespace PokemonFireRedClone
{
    public class GrayOutEffect : ImageEffect
    {

        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (image.IsActive)
            {
                image.Tint = Color.White;
            }
            else
                image.Tint = Color.Gray;
        }

    }
}
