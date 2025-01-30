using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone {
    public class EntityAnimationManager
    {
        private readonly Dictionary<IAnimatable, List<IEntityAnimation>> activeAnimations;
        private static EntityAnimationManager instance;

        private EntityAnimationManager()
        {
            activeAnimations = new Dictionary<IAnimatable, List<IEntityAnimation>>();
        }

        public static EntityAnimationManager Instance
        {
            get
            {
                instance ??= new EntityAnimationManager();

                return instance;
            }
        }

        public void StartAnimation(IAnimatable entity, IEntityAnimation animation)
        {
            if (!activeAnimations.ContainsKey(entity))
            {
                activeAnimations[entity] = new List<IEntityAnimation>();
            }

            animation.LoadContent();
            activeAnimations[entity].Add(animation);
            entity.OnAnimationStart(animation);
        }

        public void Update(GameTime gameTime)
        {
            var completedAnimations = new List<(IAnimatable entity, IEntityAnimation animation)>();
            var entityAnimationPairs = activeAnimations.ToList();

            foreach (var kvp in entityAnimationPairs)
            {
                var entity = kvp.Key;
                var animations = kvp.Value.ToList();

                foreach (var animation in animations)
                {
                    if (animation.Animate(gameTime))
                    {
                        completedAnimations.Add((entity, animation));
                    }
                }
            }

            // Clean up completed animations
            foreach (var (entity, animation) in completedAnimations)
            {
                animation.UnloadContent();
                activeAnimations[entity].Remove(animation);
                entity.OnAnimationComplete(animation);

                if (!activeAnimations[entity].Any())
                {
                    activeAnimations.Remove(entity);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var animations in activeAnimations.Values)
            {
                foreach (var animation in animations)
                {
                    animation.Draw(spriteBatch);
                }
            }
        }

        public void PostDraw(SpriteBatch spriteBatch)
        {
            foreach (var animations in activeAnimations.Values)
            {
                foreach (var animation in animations)
                {
                    animation.PostDraw(spriteBatch);
                }
            }
        }

        public bool IsEntityAnimating(IAnimatable entity)
        {
            return activeAnimations.ContainsKey(entity) && activeAnimations[entity].Any();
        }

        public bool CanEntityMove(IAnimatable entity)
        {
            if (!activeAnimations.ContainsKey(entity))
            {
                return true;
            }

            return !activeAnimations[entity].Any(animation => !animation.CanMove());
        }
    }
}