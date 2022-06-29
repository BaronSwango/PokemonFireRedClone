using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class IntroAnimation : BattleAnimation
    {

        public override bool Animate(GameTime gameTime)
        {
            float enemySpeed = (float)(0.596 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float playerSpeed = (float)(0.807 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float enemyPlatformDestinationX = ScreenManager.Instance.Dimensions.X - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Width;
            float playerPlatformDestinationX = 16;

            if (!(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X - playerSpeed < playerPlatformDestinationX) && !(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X + enemySpeed > enemyPlatformDestinationX))
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X -= playerSpeed;
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X -= playerSpeed;
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X += enemySpeed;
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position.X += enemySpeed;
                return false;
            }

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X = playerPlatformDestinationX;
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X = ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width / 2 - 48;
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X = enemyPlatformDestinationX;
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position.X = ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Width / 2 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Width / 2;


            if (BattleLogic.Battle.IsWild)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.WILD_POKEMON_FADE_IN;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new WildPokemonFadeIn();
            }
            else
                ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
