using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public abstract class BattleAnimation
    {

        protected BattleAnimations BattleAnimations
        {
            get { return ((BattleScreen)ScreenManager.Instance.CurrentScreen).BattleAnimations; }
            set { }
        }

        protected int counter;
        protected float counterSpeed;

        //public abstract void LoadContent();

        //public abstract void UnloadContent();

        public abstract bool Animate(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
