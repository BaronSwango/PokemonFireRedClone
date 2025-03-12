using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<Entity, List<ITileAnimation>> tileAnimations;

        public TileAnimationManager()
        {
            tileAnimations = new();
        }

        public void AddAnimation(Entity entity, ITileAnimation animation)
        {
            animation.LoadContent();

            if (!tileAnimations.ContainsKey(entity))
            {
                tileAnimations[entity] = new();
            }

            tileAnimations[entity].Add(animation);
        }

        public void Update(GameTime gameTime)
        {
            var animationsToRemove = new List<(Entity entity, ITileAnimation animation)>();
            var entityAnimationPairs = tileAnimations.ToList();

            foreach (var kvp in entityAnimationPairs)
            {
                var entity = kvp.Key;
                var animations = kvp.Value.ToList();

                foreach (var animation in animations)
                {
                    if (animation.Animate(gameTime))
                    {
                        animationsToRemove.Add((entity, animation));
                    }
                }
            }

            // Clean up completed animations
            foreach (var (entity, animation) in animationsToRemove)
            {
                animation.UnloadContent();
                tileAnimations[entity].Remove(animation);

                if (!tileAnimations[entity].Any())
                {
                    tileAnimations.Remove(entity);
                }
            }
        }

        public void Draw(Entity entity, SpriteBatch spriteBatch)
        {
            if (tileAnimations.ContainsKey(entity))
            {
                foreach (var animation in tileAnimations[entity])
                {
                    animation.Draw(spriteBatch);
                }
            }
        }

        public void PostDraw(Entity entity, SpriteBatch spriteBatch)
        {
            if (tileAnimations.ContainsKey(entity))
            {
                foreach (var animation in tileAnimations[entity])
                {
                    animation.PostDraw(spriteBatch);
                }
            }
        }

        public void Clear()
        {
            var entityAnimationPairs = tileAnimations.ToList();

            foreach (var kvp in entityAnimationPairs)
            {
                var entity = kvp.Key;
                var animations = kvp.Value.ToList();

                foreach (var animation in animations)
                {
                    animation.UnloadContent();
                }
            }
            tileAnimations.Clear();
        }
    }
}