using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class TextBoxEffect : ImageEffect
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

            } else
            {

            }
        }


    }
}
