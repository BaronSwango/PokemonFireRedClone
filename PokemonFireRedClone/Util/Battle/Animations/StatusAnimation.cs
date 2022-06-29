using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class StatusAnimation : BattleAnimation
    {

        private bool increase;
        private int reveal;
        private float spinCounter;

        public override bool Animate(GameTime gameTime)
        {
            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (ScreenManager.Instance.BattleScreen.TextBox.Page == 18 || ScreenManager.Instance.BattleScreen.TextBox.Page == 20)
                {
                    if (Counter < 1000.0f)
                    {
                        Counter += CounterSpeed;
                        return false;
                    }

                    if (ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveExecuted)
                        ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = true;

                    if (ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveExecuted)
                        ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved = true;

                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position = new Vector2(-ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Width, 0);
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position = new Vector2(-ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Width, 0);
                    ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;

                    if (ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved && ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved)
                        EndFightSequence();

                    ScreenManager.Instance.BattleScreen.BattleAssets.Animation = null;

                    return true;
                }

                if (!ScreenManager.Instance.BattleScreen.BattleLogic.MoveLanded)
                {
                    if (Counter < 1000.0f)
                    {
                        Counter += CounterSpeed;
                        return false;
                    }

                    Counter = 0;
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 20;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    return false;
                }

                if (ScreenManager.Instance.BattleScreen.BattleLogic.StageMaxed)
                {
                    if (Counter < 1000.0f)
                    {
                        Counter += CounterSpeed;
                        return false;
                    }

                    Counter = 0;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    return false;
                }

                Vector2 animationPos = ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position.X, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position.Y)
                    : new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position.X, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position.Y);
                Rectangle pokeSourceRect = ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect
                    : ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect;
                if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position.X != animationPos.X)
                {

                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position = animationPos;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height)
                        : animationPos;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Width = pokeSourceRect.Width;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? 0 : pokeSourceRect.Height;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Width = pokeSourceRect.Width;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha = 0.00001f;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha = 0.00001f;
                    reveal = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    spinCounter = 0;
                    increase = false;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Tint = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                    ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Tint = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                }
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.3f;
                float alphaSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.0005f;


                if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha > 0)
                {

                    if (spinCounter < 1000)
                    {
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha + alphaSpeed < 0.5f)
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha += alphaSpeed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Alpha += alphaSpeed;
                        }
                        else
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha = 0.5f;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Alpha = 0.5f;
                        }
                    }

                    if (increase)
                    {

                        if (ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease)
                        {
                            reveal -= (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height += (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position.Y -= (int)speed;
                        }
                        else

                        {
                            reveal += (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position.Y += (int)speed;
                        }
                    }
                    else
                    {

                        if (ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease)
                        {
                            reveal -= (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height += (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position.Y -= (int)speed;
                        }
                        else

                        {
                            reveal += (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height -= (int)speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position.Y += (int)speed;
                        }
                    }



                    if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Height <= 0)
                    {
                        increase = true;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ?
                            new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position.Y = animationPos.Y;
                        reveal = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    }
                    else if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Height <= 0)
                    {
                        increase = false;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Position.Y = animationPos.Y;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Position = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ?
                            new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                        ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                        reveal = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                    }

                    spinCounter += CounterSpeed;

                    if (spinCounter > 1000)
                    {
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha - alphaSpeed > 0)
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha -= alphaSpeed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Alpha -= alphaSpeed;
                        }
                        else
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage1.Alpha = 0;
                            ScreenManager.Instance.BattleScreen.BattleAssets.StatChangeAnimationImage2.Alpha = 0;
                        }
                    }

                    return false;
                }

                spinCounter = 0;

                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;

                return true;
            }
            else return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        { }
    }
}
