using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class StatusAnimation : BattleAnimation
    {
        bool increase;
        int reveal;
        float spinCounter;

        public override bool Animate(GameTime gameTime)
        {
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (BattleScreen.TextBox.Page == 18)
            {
                if (counter < 1000.0f)
                {
                    counter += counterSpeed;
                    return false;
                }

                if (BattleScreen.BattleLogic.PlayerMoveExecuted)
                    BattleScreen.BattleLogic.PlayerHasMoved = true;

                if (BattleScreen.BattleLogic.EnemyMoveExecuted)
                    BattleScreen.BattleLogic.EnemyHasMoved = true;

                BattleAnimations.StatChangeAnimationImage1.Position = new Vector2(-BattleAnimations.StatChangeAnimationImage1.SourceRect.Width, 0);
                BattleAnimations.StatChangeAnimationImage2.Position = new Vector2(-BattleAnimations.StatChangeAnimationImage2.SourceRect.Width, 0);
                BattleAnimations.IsTransitioning = false;

                if (BattleScreen.BattleLogic.EnemyHasMoved && BattleScreen.BattleLogic.PlayerHasMoved)
                    endFightSequence();

                BattleAnimations.Animation = null;

                return true;
            }

            if (BattleScreen.BattleLogic.StageMaxed)
            {
                if (counter < 1000.0f)
                {
                    counter += counterSpeed;
                    return false;
                }

                counter = 0;
                BattleScreen.TextBox.IsTransitioning = true;
                return false;
            }

            Vector2 animationPos = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? new Vector2(BattleAnimations.PlayerPokemon.Position.X, BattleAnimations.PlayerPokemon.Position.Y)
                : new Vector2(BattleAnimations.EnemyPokemon.Position.X, BattleAnimations.EnemyPokemon.Position.Y);
            Rectangle pokeSourceRect = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? BattleAnimations.PlayerPokemon.SourceRect
                : BattleAnimations.EnemyPokemon.SourceRect;
            if (BattleAnimations.StatChangeAnimationImage1.Position.X != animationPos.X)
            {

                BattleAnimations.StatChangeAnimationImage1.Position = animationPos;
                BattleAnimations.StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ? new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height)
                    : animationPos;
                BattleAnimations.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                BattleAnimations.StatChangeAnimationImage1.SourceRect.Width = pokeSourceRect.Width;
                BattleAnimations.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                BattleAnimations.StatChangeAnimationImage2.SourceRect.Height = BattleScreen.BattleLogic.StatStageIncrease ? 0 : pokeSourceRect.Height;
                BattleAnimations.StatChangeAnimationImage2.SourceRect.Width = pokeSourceRect.Width;
                BattleAnimations.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                BattleAnimations.StatChangeAnimationImage1.Alpha = 0.00001f;
                BattleAnimations.StatChangeAnimationImage1.Alpha = 0.00001f;
                reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                spinCounter = 0;
                increase = false;
                BattleAnimations.StatChangeAnimationImage1.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                BattleAnimations.StatChangeAnimationImage2.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
            }
            float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.3f;
            float alphaSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.0005f;


            if (BattleAnimations.StatChangeAnimationImage1.Alpha > 0)
            {

                if (spinCounter < 1000)
                {
                    if (BattleAnimations.StatChangeAnimationImage1.Alpha + alphaSpeed < 0.5f)
                    {
                        BattleAnimations.StatChangeAnimationImage1.Alpha += alphaSpeed;
                        BattleAnimations.StatChangeAnimationImage2.Alpha += alphaSpeed;
                    }
                    else
                    {
                        BattleAnimations.StatChangeAnimationImage1.Alpha = 0.5f;
                        BattleAnimations.StatChangeAnimationImage2.Alpha = 0.5f;
                    }
                }

                if (increase)
                {

                    if (BattleScreen.BattleLogic.StatStageIncrease)
                    {
                        reveal -= (int)speed;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height += (int)speed;
                        BattleAnimations.StatChangeAnimationImage1.Position.Y -= (int)speed;
                    }
                    else

                    {
                        reveal += (int)speed;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height -= (int)speed;
                        BattleAnimations.StatChangeAnimationImage2.Position.Y += (int)speed;
                    }
                }
                else
                {

                    if (BattleScreen.BattleLogic.StatStageIncrease)
                    {
                        reveal -= (int)speed;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height += (int)speed;
                        BattleAnimations.StatChangeAnimationImage2.Position.Y -= (int)speed;
                    }
                    else

                    {
                        reveal += (int)speed;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                        BattleAnimations.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                        BattleAnimations.StatChangeAnimationImage1.SourceRect.Height -= (int)speed;
                        BattleAnimations.StatChangeAnimationImage1.Position.Y += (int)speed;
                    }
                }



                if (BattleAnimations.StatChangeAnimationImage1.SourceRect.Height <= 0)
                {
                    increase = true;
                    BattleAnimations.StatChangeAnimationImage1.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                        new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                    BattleAnimations.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                    BattleAnimations.StatChangeAnimationImage2.Position.Y = animationPos.Y;
                    reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                }
                else if (BattleAnimations.StatChangeAnimationImage2.SourceRect.Height <= 0)
                {
                    increase = false;
                    BattleAnimations.StatChangeAnimationImage1.Position.Y = animationPos.Y;
                    BattleAnimations.StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                        new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                    BattleAnimations.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                    reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                }

                spinCounter += counterSpeed;

                if (spinCounter > 1000)
                {
                    if (BattleAnimations.StatChangeAnimationImage1.Alpha - alphaSpeed > 0)
                    {
                        BattleAnimations.StatChangeAnimationImage1.Alpha -= alphaSpeed;
                        BattleAnimations.StatChangeAnimationImage2.Alpha -= alphaSpeed;
                    }
                    else
                    {
                        BattleAnimations.StatChangeAnimationImage1.Alpha = 0;
                        BattleAnimations.StatChangeAnimationImage2.Alpha = 0;
                    }
                }

                return false;
            }

            spinCounter = 0;

            BattleScreen.TextBox.IsTransitioning = true;

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
