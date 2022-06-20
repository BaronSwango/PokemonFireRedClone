using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class WildPokemonFadeIn : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            float enemyHPDestinationX = 52;

            float enemySpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (BattleAnimations.EnemyPokemon.Tint != Color.White || BattleAnimations.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
            {
                if (BattleAnimations.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                {
                    BattleAnimations.EnemyHPBarBackground.Position.X += enemySpeed;
                    BattleAnimations.EnemyPokemonAssets.Name.OffsetX(enemySpeed);
                    if (BattleAnimations.EnemyPokemonAssets.Gender != null)
                        BattleAnimations.EnemyPokemonAssets.Gender.OffsetX(enemySpeed);
                    BattleAnimations.EnemyPokemonAssets.Level.OffsetX(enemySpeed);
                    BattleAnimations.EnemyPokemonAssets.HPBar.Position.X += enemySpeed;
                }
                if (BattleAnimations.EnemyPokemon.Tint != Color.White)
                    BattleAnimations.EnemyPokemon.Tint = new Color(BattleAnimations.EnemyPokemon.Tint.R + 3, BattleAnimations.EnemyPokemon.Tint.G + 3, BattleAnimations.EnemyPokemon.Tint.B + 3, 255);
                return false;
            }
            BattleAnimations.EnemyHPBarBackground.Position.X = enemyHPDestinationX;
            BattleAnimations.EnemyPokemonAssets.Name.SetPosition(new Vector2(BattleAnimations.EnemyHPBarBackground.Position.X + 24, BattleAnimations.EnemyHPBarBackground.Position.Y + 19));
            if (BattleAnimations.EnemyPokemonAssets.Gender != null)
                BattleAnimations.EnemyPokemonAssets.Gender.SetPosition(new Vector2(BattleAnimations.EnemyPokemonAssets.Name.Position.X + BattleAnimations.EnemyPokemonAssets.Name.SourceRect.Width, BattleAnimations.EnemyPokemonAssets.Name.Position.Y));
            BattleAnimations.EnemyPokemonAssets.Level.SetPosition(new Vector2(BattleAnimations.EnemyHPBarBackground.Position.X + BattleAnimations.EnemyHPBarBackground.SourceRect.Width - 56 - BattleAnimations.EnemyPokemonAssets.Level.SourceRect.Width, BattleAnimations.EnemyPokemonAssets.Name.Position.Y));
            BattleAnimations.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.EnemyHPBarBackground.Position.Y + 68);

            if (BattleScreen.TextBox.Page == 3 && !BattleScreen.TextBox.IsTransitioning)
            {
                BattleAnimations.State = BattleAnimations.BattleState.PLAYER_SEND_POKEMON;
                BattleAnimations.Animation = new PlayerSendPokemon();
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
