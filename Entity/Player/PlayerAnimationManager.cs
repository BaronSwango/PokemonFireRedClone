using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone 
{
    public class PlayerAnimationManager
    {
        private IPlayerAnimation playerAnimation;
        public bool IsAnimating;
        public bool CanMove;
        private static PlayerAnimationManager instance;

        public static PlayerAnimationManager Instance
        {
            get
            {
                instance ??= new PlayerAnimationManager();

                return instance;
            }
        }

        public void Start(IPlayerAnimation animation)
        {
            playerAnimation = animation;
            IsAnimating = true;
            CanMove = playerAnimation.CanMove();
            playerAnimation.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (IsAnimating && playerAnimation.Animate(gameTime))
            {
                IsAnimating = false;
                playerAnimation.UnloadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAnimating)
            {
                playerAnimation.Draw(spriteBatch);
            }
        }

        public void PostDraw(SpriteBatch spriteBatch) 
        {
            if (IsAnimating) {
                playerAnimation.PostDraw(spriteBatch);
            }
        }
    }
}