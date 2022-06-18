using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public abstract class BattleAnimation
    {

        protected BattleScreen BattleScreen
        {
            get { return (BattleScreen)ScreenManager.Instance.CurrentScreen; }
            set { }
        }

        protected BattleAnimations BattleAnimations
        {
            get { return BattleScreen.BattleAnimations; }
            set { }
        }

        protected float counter;
        protected float counterSpeed;

        //public abstract void LoadContent();

        //public abstract void UnloadContent();

        public abstract bool Animate(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
