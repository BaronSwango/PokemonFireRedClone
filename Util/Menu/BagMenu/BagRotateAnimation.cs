using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BagRotateAnimation : IAnimation
    {
        private readonly Image bag;
        public enum Stage {
            NONE,
            HALF_FORWARD,
            FULL_BACKWARD,
            FULL_FORWARD,
            HALF_BACKWARD
        }
        public Stage AnimationStage;

        public BagRotateAnimation(Image bag)
        {
            this.bag = bag;
            AnimationStage = Stage.NONE;
        }

        public bool Animate(GameTime gameTime)
        {
            float rotateSpeed = (float) (6f * gameTime.ElapsedGameTime.TotalSeconds);

            switch (AnimationStage) {
                case Stage.FULL_FORWARD:
                case Stage.HALF_FORWARD:
                    if (bag.Angle + rotateSpeed >= Math.PI / 24) {
                        bag.Angle = (float) Math.PI / 24;
                        AnimationStage = AnimationStage == Stage.HALF_FORWARD ? Stage.FULL_BACKWARD : Stage.HALF_BACKWARD;
                    } else {
                        bag.Angle += rotateSpeed;
                    }
                    break;
                case Stage.FULL_BACKWARD:
                    if (bag.Angle - rotateSpeed <= -Math.PI / 24) {
                        bag.Angle = (float) -Math.PI / 24;
                        AnimationStage = Stage.FULL_FORWARD;
                    } else {
                        bag.Angle -= rotateSpeed;
                    }
                    break;
                case Stage.HALF_BACKWARD:
                    if (bag.Angle - rotateSpeed <= 0) {
                        Reset();
                        return true;
                    } else {
                        bag.Angle -= rotateSpeed;
                    }
                    break;
            }

            return false;
        }

        public void Reset()
        {
            bag.Angle = 0;
            AnimationStage = Stage.NONE;
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}

