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
            float enemyPlatformDestinationX = ScreenManager.Instance.Dimensions.X - BattleAnimations.EnemyPlatform.SourceRect.Width;
            float playerPlatformDestinationX = 16;

            if (!(BattleAnimations.PlayerPlatform.Position.X - playerSpeed < playerPlatformDestinationX) && !(BattleAnimations.EnemyPlatform.Position.X + enemySpeed > enemyPlatformDestinationX))
            {
                BattleAnimations.PlayerPlatform.Position.X -= playerSpeed;
                BattleAnimations.PlayerSprite.Position.X -= playerSpeed;
                BattleAnimations.EnemyPlatform.Position.X += enemySpeed;
                BattleAnimations.EnemyPokemon.Position.X += enemySpeed;
                return false;
            }

            BattleAnimations.PlayerPlatform.Position.X = playerPlatformDestinationX;
            BattleAnimations.PlayerSprite.Position.X = BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width / 2 - 48;
            BattleAnimations.EnemyPlatform.Position.X = enemyPlatformDestinationX;
            BattleAnimations.EnemyPokemon.Position.X = BattleAnimations.EnemyPlatform.Position.X + BattleAnimations.EnemyPlatform.SourceRect.Width / 2 - BattleAnimations.EnemyPokemon.SourceRect.Width / 2;


            if (BattleLogic.Battle.IsWild)
            {
                BattleAnimations.State = BattleAnimations.BattleState.WILD_POKEMON_FADE_IN;
                BattleAnimations.Animation = new WildPokemonFadeIn();
            }
            else
                BattleAnimations.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
