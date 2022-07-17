using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class MenuSwitchAnimation : IAnimation
    {
        private int originalSwitchIndex;
        private int newSwitchIndex;

        public bool Switched;

        public MenuSwitchAnimation(int originalSwitchIndex, int newSwitchIndex)
        {
            this.originalSwitchIndex = originalSwitchIndex;
            this.newSwitchIndex = newSwitchIndex;
        }

        public bool Animate(GameTime gameTime)
        {
            return false;
        }

        public void Draw(SpriteBatch spriteBatch) { }

    }
}
