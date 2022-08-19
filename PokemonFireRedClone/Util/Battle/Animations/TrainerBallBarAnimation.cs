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
            BattleAssets battleAssets = ScreenManager.Instance.BattleScreen.BattleAssets;
            float playerBarGoalX = ScreenManager.Instance.Dimensions.X - battleAssets.PlayerBallBar.SourceRect.Width + 292;
            float playerBallGoalX = playerBarGoalX + playerPadX + 80;
            float trainerBallGoalX = battleAssets.TrainerBallBar.SourceRect.Width - 292 - battleAssets.TrainerBarBalls[0].SourceRect.Width + trainerPadX - 80;
            float barSpeed = (float) (1.8*gameTime.ElapsedGameTime.TotalMilliseconds);

            if (battleAssets.TrainerBallBar.Position.X + barSpeed < -292
                && battleAssets.PlayerBallBar.Position.X - barSpeed > playerBarGoalX)
            {
                battleAssets.TrainerBallBar.Position.X += barSpeed;
                battleAssets.PlayerBallBar.Position.X -= barSpeed;

                return false;
            }

            battleAssets.TrainerBallBar.Position.X = -292;
            battleAssets.PlayerBallBar.Position.X = playerBarGoalX;

            if (battleAssets.PlayerBarBalls[playerBallIndex].Position.X - (2*barSpeed) > playerBallGoalX
                || battleAssets.TrainerBarBalls[trainerBallIndex].Position.X + (2*barSpeed) < trainerBallGoalX)
            {
                battleAssets.PlayerBarBalls[playerBallIndex].Position.X -= 2 * barSpeed;
                battleAssets.TrainerBarBalls[trainerBallIndex].Position.X += 2 * barSpeed;
                return false;
            }

            battleAssets.PlayerBarBalls[playerBallIndex].Position.X = playerBallGoalX;
            battleAssets.TrainerBarBalls[trainerBallIndex].Position.X = trainerBallGoalX;

            if (playerBallIndex < battleAssets.PlayerBarBalls.Count - 1)
            {
                playerPadX += battleAssets.PlayerBarBalls[playerBallIndex].SourceRect.Width + 12;
                playerBallIndex++;

                trainerPadX -= battleAssets.TrainerBarBalls[trainerBallIndex].SourceRect.Width + 12;
                trainerBallIndex++;
                return false;
            }

            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning && ScreenManager.Instance.BattleScreen.TextBox.Page == 2)
            {
                battleAssets.State = BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON;
                battleAssets.Animation = new OpponentSendPokemon();
                battleAssets.Pokeball.Position = new Vector2(battleAssets.EnemySprite.Position.X + battleAssets.EnemySprite.SourceRect.Width / 2 - battleAssets.Pokeball.SourceRect.Width / 6,
                    battleAssets.EnemySprite.Position.Y + battleAssets.EnemySprite.SourceRect.Height - battleAssets.Pokeball.SourceRect.Height);
                battleAssets.Pokeball.Alpha = 0;
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
