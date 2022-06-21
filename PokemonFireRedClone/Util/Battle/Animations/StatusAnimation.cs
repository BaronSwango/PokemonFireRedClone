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
            if (!BattleScreen.TextBox.IsTransitioning)
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

                    BattleAssets.StatChangeAnimationImage1.Position = new Vector2(-BattleAssets.StatChangeAnimationImage1.SourceRect.Width, 0);
                    BattleAssets.StatChangeAnimationImage2.Position = new Vector2(-BattleAssets.StatChangeAnimationImage2.SourceRect.Width, 0);
                    BattleAssets.IsTransitioning = false;

                    if (BattleScreen.BattleLogic.EnemyHasMoved && BattleScreen.BattleLogic.PlayerHasMoved)
                        endFightSequence();

                    BattleAssets.Animation = null;

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

                Vector2 animationPos = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? new Vector2(BattleAssets.PlayerPokemon.Position.X, BattleAssets.PlayerPokemon.Position.Y)
                    : new Vector2(BattleAssets.EnemyPokemon.Position.X, BattleAssets.EnemyPokemon.Position.Y);
                Rectangle pokeSourceRect = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? BattleAssets.PlayerPokemon.SourceRect
                    : BattleAssets.EnemyPokemon.SourceRect;
                if (BattleAssets.StatChangeAnimationImage1.Position.X != animationPos.X)
                {

                    BattleAssets.StatChangeAnimationImage1.Position = animationPos;
                    BattleAssets.StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ? new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height)
                        : animationPos;
                    BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                    BattleAssets.StatChangeAnimationImage1.SourceRect.Width = pokeSourceRect.Width;
                    BattleAssets.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                    BattleAssets.StatChangeAnimationImage2.SourceRect.Height = BattleScreen.BattleLogic.StatStageIncrease ? 0 : pokeSourceRect.Height;
                    BattleAssets.StatChangeAnimationImage2.SourceRect.Width = pokeSourceRect.Width;
                    BattleAssets.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                    BattleAssets.StatChangeAnimationImage1.Alpha = 0.00001f;
                    BattleAssets.StatChangeAnimationImage1.Alpha = 0.00001f;
                    reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    spinCounter = 0;
                    increase = false;
                    BattleAssets.StatChangeAnimationImage1.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                    BattleAssets.StatChangeAnimationImage2.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                }
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.3f;
                float alphaSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.0005f;


                if (BattleAssets.StatChangeAnimationImage1.Alpha > 0)
                {

                    if (spinCounter < 1000)
                    {
                        if (BattleAssets.StatChangeAnimationImage1.Alpha + alphaSpeed < 0.5f)
                        {
                            BattleAssets.StatChangeAnimationImage1.Alpha += alphaSpeed;
                            BattleAssets.StatChangeAnimationImage2.Alpha += alphaSpeed;
                        }
                        else
                        {
                            BattleAssets.StatChangeAnimationImage1.Alpha = 0.5f;
                            BattleAssets.StatChangeAnimationImage2.Alpha = 0.5f;
                        }
                    }

                    if (increase)
                    {

                        if (BattleScreen.BattleLogic.StatStageIncrease)
                        {
                            reveal -= (int)speed;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height += (int)speed;
                            BattleAssets.StatChangeAnimationImage1.Position.Y -= (int)speed;
                        }
                        else

                        {
                            reveal += (int)speed;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= (int)speed;
                            BattleAssets.StatChangeAnimationImage2.Position.Y += (int)speed;
                        }
                    }
                    else
                    {

                        if (BattleScreen.BattleLogic.StatStageIncrease)
                        {
                            reveal -= (int)speed;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height += (int)speed;
                            BattleAssets.StatChangeAnimationImage2.Position.Y -= (int)speed;
                        }
                        else

                        {
                            reveal += (int)speed;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= (int)speed;
                            BattleAssets.StatChangeAnimationImage1.Position.Y += (int)speed;
                        }
                    }



                    if (BattleAssets.StatChangeAnimationImage1.SourceRect.Height <= 0)
                    {
                        increase = true;
                        BattleAssets.StatChangeAnimationImage1.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                            new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                        BattleAssets.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                        BattleAssets.StatChangeAnimationImage2.Position.Y = animationPos.Y;
                        reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    }
                    else if (BattleAssets.StatChangeAnimationImage2.SourceRect.Height <= 0)
                    {
                        increase = false;
                        BattleAssets.StatChangeAnimationImage1.Position.Y = animationPos.Y;
                        BattleAssets.StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                            new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                        BattleAssets.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                        reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    }

                    spinCounter += counterSpeed;

                    if (spinCounter > 1000)
                    {
                        if (BattleAssets.StatChangeAnimationImage1.Alpha - alphaSpeed > 0)
                        {
                            BattleAssets.StatChangeAnimationImage1.Alpha -= alphaSpeed;
                            BattleAssets.StatChangeAnimationImage2.Alpha -= alphaSpeed;
                        }
                        else
                        {
                            BattleAssets.StatChangeAnimationImage1.Alpha = 0;
                            BattleAssets.StatChangeAnimationImage2.Alpha = 0;
                        }
                    }

                    return false;
                }

                spinCounter = 0;

                BattleScreen.TextBox.IsTransitioning = true;

                return true;
            }
            else return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
