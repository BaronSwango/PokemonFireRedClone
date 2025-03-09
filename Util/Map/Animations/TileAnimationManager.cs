using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TileAnimationManager
    {
        private static TileAnimationManager instance;
        public static TileAnimationManager Instance
        {
            get
            {
                instance ??= new TileAnimationManager();
                return instance;
            }
        }

        private readonly List<ITileAnimation> tileAnimations;
        private readonly List<ITileAnimation> animationsToRemove;

        public TileAnimationManager()
        {
            tileAnimations = new List<ITileAnimation>();
            animationsToRemove = new List<ITileAnimation>();
        }

        public void AddAnimation(ITileAnimation animation)
        {
            animation.LoadContent();
            tileAnimations.Add(animation);
        }

        public void Update(GameTime gameTime)
        {
            foreach (ITileAnimation animation in tileAnimations)
            {
                if (animation.Animate(gameTime))
                {
                    animationsToRemove.Add(animation);
                }
            }

            foreach (ITileAnimation animation in animationsToRemove)
            {
                tileAnimations.Remove(animation);
                animation.UnloadContent();
            }

            animationsToRemove.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ITileAnimation animation in tileAnimations)
            {
                animation.Draw(spriteBatch);
            }
        }

        public void Clear()
        {
            foreach (ITileAnimation animation in tileAnimations)
            {
                animation.UnloadContent();
            }
            tileAnimations.Clear();
            animationsToRemove.Clear();
        }
    }
}