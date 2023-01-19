using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class OpponentSendPokemon : BattleAnimation
    {

        private int trainerBallIndex;
        private float counter;
        private float counterSpeed;

        public OpponentSendPokemon()
        {
            CreateWhiteEffect();
        }

        public override bool Animate(GameTime gameTime)
        {
            float opponentSpriteDestinationX = ScreenManager.Instance.Dimensions.X + 8;
            float opponentSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float barSpeed = BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON
                ? (float)(0.9 * gameTime.ElapsedGameTime.TotalMilliseconds) : (float)(0.4 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float ballSpeed = (float)(7 * gameTime.ElapsedGameTime.TotalMilliseconds);
            Vector2 opponentPokemonDestination = new(BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.EnemyPokemon.SourceRect.Width / 2,
                BattleAssets.EnemyPlatform.Position.Y + BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - BattleAssets.EnemyPokemon.SourceRect.Height);
            float enemyHPSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);

            const float EnemyHPDestinationX = 52;

            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (counter < 1000 && BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON)
            {
                counter += counterSpeed;
                return false;
            }
            else if (counter < 1000 && BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON)
                counter = 1000;

            if (counter < 2000)
            {

                if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON)
                    BattleAssets.EnemySprite.Position.X = BattleAssets.EnemySprite.Position.X + opponentSpeed < opponentSpriteDestinationX
                        ? BattleAssets.EnemySprite.Position.X + opponentSpeed : opponentSpriteDestinationX;

                if (BattleAssets.TrainerBallBar.Alpha > 0)
                {
                    BattleAssets.TrainerBallBar.Alpha -= 0.03f;

                    foreach (Image image in BattleAssets.TrainerBarBalls)
                        image.Alpha -= 0.03f;

                    if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON)
                    {
                        BattleAssets.TrainerBallBar.Position.X += barSpeed;

                        if (BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X < ScreenManager.Instance.Dimensions.X)
                            BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X += ballSpeed;

                        if (trainerBallIndex > 0)
                        {
                            if (BattleAssets.TrainerBarBalls[trainerBallIndex-1].Position.X < ScreenManager.Instance.Dimensions.X)
                                BattleAssets.TrainerBarBalls[trainerBallIndex-1].Position.X += ballSpeed;
                        }

                        if (BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X > ScreenManager.Instance.Dimensions.X - 250)
                        {
                            if (trainerBallIndex + 1 < BattleAssets.TrainerBarBalls.Count)
                                trainerBallIndex++;
                        }
                    } 
                }
                else
                {
                    if (BattleAssets.EnemyHPBarBackground.Position.X + enemyHPSpeed < EnemyHPDestinationX)
                    {
                        BattleAssets.EnemyHPBarBackground.Position.X += enemyHPSpeed;
                        BattleAssets.EnemyPokemonAssets.Name.OffsetX(enemyHPSpeed);
                        if (BattleAssets.EnemyPokemonAssets.Gender != null)
                            BattleAssets.EnemyPokemonAssets.Gender.OffsetX(enemyHPSpeed);
                        BattleAssets.EnemyPokemonAssets.Level.OffsetX(enemyHPSpeed);
                        BattleAssets.EnemyPokemonAssets.HPBar.Position.X += enemyHPSpeed;
                    } else
                    {
                        BattleAssets.EnemyHPBarBackground.Position.X = EnemyHPDestinationX;
                        BattleAssets.EnemyPokemonAssets.Name.SetPosition(new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 24, BattleAssets.EnemyHPBarBackground.Position.Y + 19));
                        if (BattleAssets.EnemyPokemonAssets.Gender != null)
                            BattleAssets.EnemyPokemonAssets.Gender.SetPosition(new Vector2(BattleAssets.EnemyPokemonAssets.Name.Position.X + BattleAssets.EnemyPokemonAssets.Name.SourceRect.Width, BattleAssets.EnemyPokemonAssets.Name.Position.Y));
                        BattleAssets.EnemyPokemonAssets.Level.SetPosition(new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + BattleAssets.EnemyHPBarBackground.SourceRect.Width - 56 - BattleAssets.EnemyPokemonAssets.Level.SourceRect.Width, BattleAssets.EnemyPokemonAssets.Name.Position.Y));
                        BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAssets.EnemyHPBarBackground.Position.Y + 68);

                        counter += counterSpeed;
                        if (counter < 2000)
                            return false;

                        if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON)
                        {
                            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 3;
                            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                }


                if (counter < 1017)
                {
                    counter += counterSpeed;
                    return false;
                }

                if (counter < 1450)
                    BattleAssets.Pokeball.Alpha = 1;

                if (counter < 1300)
                {
                    counter += counterSpeed;
                    return false;
                }

                if (BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X == 0)
                    BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 1;

                if (counter < 1350)
                {
                    counter += counterSpeed;
                    return false;
                }

                if ((BattleAssets.EnemyPokemon.Scale.X + 0.07f < 1) || !WhiteEffectTransitioned)
                {
                    WhiteEffect.Alpha += 0.07f;

                    if (WhiteEffect.Alpha >= 1)
                        WhiteEffectTransitioned = true;

                    if (BattleAssets.EnemyPokemon.Scale.X + 0.07f < 1)
                    {
                        if (BattleAssets.EnemyPokemon.Alpha == 0)
                            BattleAssets.EnemyPokemon.Alpha = 1;
                        BattleAssets.EnemyPokemon.Scale = new Vector2(BattleAssets.EnemyPokemon.Scale.X + 0.07f, BattleAssets.EnemyPokemon.Scale.Y + 0.07f);
                        BattleAssets.EnemyPokemon.Position = new(BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.EnemyPokemon.SourceRect.Width / 2,
                            BattleAssets.EnemyPlatform.Position.Y + BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - (BattleAssets.EnemyPokemon.SourceRect.Height / 2) - (BattleAssets.EnemyPokemon.SourceRect.Height * BattleAssets.EnemyPokemon.Scale.Y / 2));
                    } else
                    {
                        BattleAssets.EnemyPokemon.Scale = Vector2.One;
                        BattleAssets.EnemyPokemon.Position = opponentPokemonDestination;
                    }
                } else
                {
                    if (counter < 1525)
                    {
                        counter += counterSpeed;
                        return false;
                    }

                    if (BattleAssets.EnemyPokemon.Tint != Color.White || WhiteEffect.Alpha > 0)
                    {

                        if (WhiteEffect.Alpha > 0)
                            WhiteEffect.Alpha -= 0.06f;
                        BattleAssets.EnemyPokemon.Tint = new Color(BattleAssets.EnemyPokemon.Tint.R + 20, BattleAssets.EnemyPokemon.Tint.G + 20, BattleAssets.EnemyPokemon.Tint.B + 20, 255);
                    }
                }

                if (counter < 1400)
                {
                    counter += counterSpeed;
                    return false;
                }

                if (BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X == 1)
                    BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 2;

                if (counter < 1450)
                {
                    counter += counterSpeed;
                    return false;
                }

                BattleAssets.Pokeball.Alpha = 0;
                BattleAssets.Pokeball.SpriteSheetEffect.CurrentFrame.X = 0;
                
                return false;
            }

            if (ScreenManager.Instance.BattleScreen.TextBox.Page == 3 && !ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                BattleAssets.State = BattleAssets.BattleState.PLAYER_SEND_POKEMON;
                BattleAssets.Animation = new PlayerSendPokemon();
            }

            BattleAssets.TrainerBallBar.Position.X = -BattleAssets.TrainerBallBar.SourceRect.Width;
            BattleAssets.TrainerBallBar.Alpha = 1;

            if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON)
            {
                EndFightSequence();
                ScreenManager.Instance.BattleScreen.BattleLogic.PokemonFainted = false;
                BattleAssets.IsTransitioning = false;
                
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WhiteEffect.Draw(spriteBatch);
        }
    }
}
