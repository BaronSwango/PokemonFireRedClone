using System;
using Microsoft.Xna.Framework;


namespace PokemonFireRedClone
{
    public class GrayOutEffect : ImageEffect
    {

        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Image.IsActive)
            {
                Image.Tint = Color.White;
            }
            else
                Image.Tint = Color.Gray;
        }

    }
}
