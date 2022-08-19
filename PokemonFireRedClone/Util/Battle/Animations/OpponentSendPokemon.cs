using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class OpponentSendPokemon : BattleAnimation
    {

        int trainerBallIndex;

        public OpponentSendPokemon()
        {
            CreateWhiteEffect();
        }

        public override bool Animate(GameTime gameTime)
        {
            float opponentSpriteDestinationX = ScreenManager.Instance.Dimensions.X + 8;
            float opponentSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float barSpeed = (float)(0.4 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float ballSpeed = (float)(7 * gameTime.ElapsedGameTime.TotalMilliseconds);
            Vector2 opponentPokemonDestination = new(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Width / 2 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Width / 2,
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height);
            float enemyHPDestinationX = 52;
            float enemyHPSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Counter < 1000)
            {
                Counter += CounterSpeed;
                return false;
            }

            if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemySprite.Position.X + opponentSpeed < opponentSpriteDestinationX
                || ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint != Color.White
                || ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X < enemyHPDestinationX
                || Counter < 2000)
            {

                ScreenManager.Instance.BattleScreen.BattleAssets.EnemySprite.Position.X = ScreenManager.Instance.BattleScreen.BattleAssets.EnemySprite.Position.X + opponentSpeed < opponentSpriteDestinationX
                    ? ScreenManager.Instance.BattleScreen.BattleAssets.EnemySprite.Position.X + opponentSpeed : opponentSpriteDestinationX;

                if (ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBallBar.Alpha > 0)
                {
                    if (ScreenManager.Instance.BattleScreen.BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON)
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBallBar.Position.X += barSpeed;
                        ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBallBar.Alpha -= 0.03f;

                        if (ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X < ScreenManager.Instance.Dimensions.X)
                            ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X += ballSpeed;

                        if (trainerBallIndex > 0)
                        {
                            if (ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls[trainerBallIndex-1].Position.X < ScreenManager.Instance.Dimensions.X)
                                ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls[trainerBallIndex-1].Position.X += ballSpeed;
                        }

                        if (ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X > ScreenManager.Instance.Dimensions.X - 250)
                        {
                            if (trainerBallIndex + 1 < ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls.Count)
                                trainerBallIndex++;
                        }

                        foreach (Image image in ScreenManager.Instance.BattleScreen.BattleAssets.TrainerBarBalls)
                            image.Alpha -= 0.03f;
                    } else
                    {

                    }
                }
                else
                {
                    if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + enemyHPSpeed < enemyHPDestinationX)
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X += enemyHPSpeed;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.OffsetX(enemyHPSpeed);
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender != null)
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender.OffsetX(enemyHPSpeed);
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.OffsetX(enemyHPSpeed);
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position.X += enemyHPSpeed;
                    } else
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X = enemyHPDestinationX;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 24, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 19));
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender != null)
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.Y));
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.SourceRect.Width - 56 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.Y));
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 68);

                        Counter += CounterSpeed;
                        if (Counter < 2000)
                            return false;

                        ScreenManager.Instance.BattleScreen.TextBox.NextPage = 3;
                        ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    }
                }


                if (Counter < 1017)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                if (Counter < 1450)
                    ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Alpha = 1;

                if (Counter < 1300)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X == 0)
                    ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 1;

                if (Counter < 1350)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                if ((ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale.X + 0.07f < 1) || !WhiteEffectTransitioned)
                {
                    WhiteEffect.Alpha += 0.07f;

                    if (WhiteEffect.Alpha >= 1)
                        WhiteEffectTransitioned = true;

                    if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale.X + 0.07f < 1)
                    {
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha == 0)
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha = 1;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale.X + 0.07f, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale.Y + 0.07f);
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position = new(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Width / 2 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Width / 2,
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height / 2) - (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height * ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale.Y / 2));
                    } else
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Scale = Vector2.One;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position = opponentPokemonDestination;
                    }
                } else
                {
                    if (Counter < 1525)
                    {
                        Counter += CounterSpeed;
                        return false;
                    }

                    if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint != Color.White || WhiteEffect.Alpha > 0)
                    {

                        if (WhiteEffect.Alpha > 0)
                            WhiteEffect.Alpha -= 0.06f;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint = new Color(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.R + 20, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.G + 20, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.B + 20, 255);
                    }
                }

                if (Counter < 1400)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X == 1)
                    ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 2;

                if (Counter < 1450)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Alpha = 0;
                ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 0;
                
                return false;
            }

            if (ScreenManager.Instance.BattleScreen.TextBox.Page == 3 && !ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.PLAYER_SEND_POKEMON;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PlayerSendPokemon();
            }

            return true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WhiteEffect.Draw(spriteBatch);
        }
    }
}
