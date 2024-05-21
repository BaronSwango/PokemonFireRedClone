using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TrainerBallBarAnimation : BattleAnimation
    {

        float playerPadX;
        float trainerPadX;
        int playerBallIndex;
        int trainerBallIndex;

        public override bool Animate(GameTime gameTime)
        {
            float trainerBallGoalX = BattleAssets.TrainerBallBar.SourceRect.Width - BattleAssets.TrainerBarBalls[0].SourceRect.Width + trainerPadX - 372;
            float barSpeed = BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON
                ? (float)(0.9 * gameTime.ElapsedGameTime.TotalMilliseconds) : (float)(1.8 * gameTime.ElapsedGameTime.TotalMilliseconds);

            const float BarDestination = -292;
            //CounterSpeed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            Counter ??= new Counter(1000);

            if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON
                //&& Counter < 1000)
                && !Counter.Finished)
            {

                if (BattleAssets.TrainerBallBar.Position.X + barSpeed < BarDestination)
                {

                    BattleAssets.TrainerBallBar.Position.X += barSpeed;

                    foreach (Image image in BattleAssets.TrainerBarBalls)
                        image.Position.X += barSpeed;

                    return false;
                }

                BattleAssets.TrainerBallBar.Position.X = BarDestination;

                float padX = 0;
                foreach (Image image in BattleAssets.TrainerBarBalls)
                {
                    image.Position.X = BattleAssets.TrainerBallBar.SourceRect.Width - image.SourceRect.Width + padX - 372;
                    padX -= image.SourceRect.Width + 12;
                }

                BattleAssets.Pokeball.Position = new Vector2(BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.Pokeball.SourceRect.Width / 6,
                    BattleAssets.EnemyPlatform.Position.Y + BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - BattleAssets.Pokeball.SourceRect.Height);

                //Counter += CounterSpeed;
                Counter.Update(gameTime);
                //if (Counter < 1000)
                if (!Counter.Finished)
                    return false;

                ScreenManager.Instance.BattleScreen.TextBox.NextPage = 2;
                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            }
            else if (BattleAssets.State == BattleAssets.BattleState.TRAINER_BALL_BAR)
            {
                float playerBarGoalX = ScreenManager.Instance.Dimensions.X - BattleAssets.PlayerBallBar.SourceRect.Width + 292;
                float playerBallGoalX = playerBarGoalX + playerPadX + 80;


                if (BattleAssets.TrainerBallBar.Position.X + barSpeed < -292
                    && BattleAssets.PlayerBallBar.Position.X - barSpeed > playerBarGoalX)
                {
                    BattleAssets.TrainerBallBar.Position.X += barSpeed;
                    BattleAssets.PlayerBallBar.Position.X -= barSpeed;

                    return false;
                }

                BattleAssets.TrainerBallBar.Position.X = -292;
                BattleAssets.PlayerBallBar.Position.X = playerBarGoalX;

                if (BattleAssets.PlayerBarBalls[playerBallIndex].Position.X - (2 * barSpeed) > playerBallGoalX
                    || BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X + (2 * barSpeed) < trainerBallGoalX)
                {
                    BattleAssets.PlayerBarBalls[playerBallIndex].Position.X -= 2 * barSpeed;
                    BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X += 2 * barSpeed;
                    return false;
                }

                BattleAssets.PlayerBarBalls[playerBallIndex].Position.X = playerBallGoalX;
                BattleAssets.TrainerBarBalls[trainerBallIndex].Position.X = trainerBallGoalX;

                if (playerBallIndex < BattleAssets.PlayerBarBalls.Count - 1)
                {
                    playerPadX += BattleAssets.PlayerBarBalls[playerBallIndex].SourceRect.Width + 12;
                    playerBallIndex++;

                    trainerPadX -= BattleAssets.TrainerBarBalls[trainerBallIndex].SourceRect.Width + 12;
                    trainerBallIndex++;
                    return false;
                }
            }

            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning && ScreenManager.Instance.BattleScreen.TextBox.Page == 2)
            {
                if (BattleAssets.State == BattleAssets.BattleState.TRAINER_BALL_BAR)
                    BattleAssets.State = BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON;
                BattleAssets.Animation = new OpponentSendPokemon();
                BattleAssets.Pokeball.Update(gameTime);
                BattleAssets.Pokeball.Position = new Vector2(BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.Pokeball.SourceRect.Width / 2,
                    BattleAssets.EnemyPlatform.Position.Y + BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - BattleAssets.Pokeball.SourceRect.Height);
                Console.WriteLine("Pokeball Pos: " + BattleAssets.Pokeball.Position.X + ", " + BattleAssets.Pokeball.Position.Y);
                Console.WriteLine("Pokeball Rect: " + BattleAssets.Pokeball.SourceRect.Width + ", " + BattleAssets.Pokeball.SourceRect.Height);
                BattleAssets.Pokeball.Angle = 0;
                BattleAssets.Pokeball.Alpha = 0;
                if (BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON)
                    BattleAssets.UpdateOpponentPokemon();
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
