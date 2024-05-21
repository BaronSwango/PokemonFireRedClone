using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TrainerVictoryAnimation : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            float trainerDestinationX = BattleAssets.EnemyPlatform.Position.X + (7 * BattleAssets.EnemyPlatform.SourceRect.Width / 10) - BattleAssets.EnemySprite.SourceRect.Width / 2;
            float opponentSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (BattleAssets.EnemySprite.Position.X - opponentSpeed > trainerDestinationX)
            {
                BattleAssets.EnemySprite.Position.X -= opponentSpeed;
                return false;
            }

            BattleAssets.EnemySprite.Position.X = trainerDestinationX;

            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 1;
            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            BattleAssets.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
