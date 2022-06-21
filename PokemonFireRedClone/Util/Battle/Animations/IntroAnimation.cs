using System;
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
            float enemyPlatformDestinationX = ScreenManager.Instance.Dimensions.X - BattleAssets.EnemyPlatform.SourceRect.Width;
            float playerPlatformDestinationX = 16;

            if (!(BattleAssets.PlayerPlatform.Position.X - playerSpeed < playerPlatformDestinationX) && !(BattleAssets.EnemyPlatform.Position.X + enemySpeed > enemyPlatformDestinationX))
            {
                BattleAssets.PlayerPlatform.Position.X -= playerSpeed;
                BattleAssets.PlayerSprite.Position.X -= playerSpeed;
                BattleAssets.EnemyPlatform.Position.X += enemySpeed;
                BattleAssets.EnemyPokemon.Position.X += enemySpeed;
                return false;
            }

            BattleAssets.PlayerPlatform.Position.X = playerPlatformDestinationX;
            BattleAssets.PlayerSprite.Position.X = BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width / 2 - 48;
            BattleAssets.EnemyPlatform.Position.X = enemyPlatformDestinationX;
            BattleAssets.EnemyPokemon.Position.X = BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.EnemyPokemon.SourceRect.Width / 2;


            if (BattleLogic.Battle.IsWild)
            {
                BattleAssets.State = BattleAssets.BattleState.WILD_POKEMON_FADE_IN;
                BattleAssets.Animation = new WildPokemonFadeIn();
            }
            else
                BattleAssets.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
