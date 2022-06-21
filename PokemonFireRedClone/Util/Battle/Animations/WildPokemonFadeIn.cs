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

            if (BattleAssets.EnemyPokemon.Tint != Color.White || BattleAssets.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
            {
                if (BattleAssets.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                {
                    BattleAssets.EnemyHPBarBackground.Position.X += enemySpeed;
                    BattleAssets.EnemyPokemonAssets.Name.OffsetX(enemySpeed);
                    if (BattleAssets.EnemyPokemonAssets.Gender != null)
                        BattleAssets.EnemyPokemonAssets.Gender.OffsetX(enemySpeed);
                    BattleAssets.EnemyPokemonAssets.Level.OffsetX(enemySpeed);
                    BattleAssets.EnemyPokemonAssets.HPBar.Position.X += enemySpeed;
                }
                if (BattleAssets.EnemyPokemon.Tint != Color.White)
                    BattleAssets.EnemyPokemon.Tint = new Color(BattleAssets.EnemyPokemon.Tint.R + 3, BattleAssets.EnemyPokemon.Tint.G + 3, BattleAssets.EnemyPokemon.Tint.B + 3, 255);
                return false;
            }
            BattleAssets.EnemyHPBarBackground.Position.X = enemyHPDestinationX;
            BattleAssets.EnemyPokemonAssets.Name.SetPosition(new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 24, BattleAssets.EnemyHPBarBackground.Position.Y + 19));
            if (BattleAssets.EnemyPokemonAssets.Gender != null)
                BattleAssets.EnemyPokemonAssets.Gender.SetPosition(new Vector2(BattleAssets.EnemyPokemonAssets.Name.Position.X + BattleAssets.EnemyPokemonAssets.Name.SourceRect.Width, BattleAssets.EnemyPokemonAssets.Name.Position.Y));
            BattleAssets.EnemyPokemonAssets.Level.SetPosition(new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + BattleAssets.EnemyHPBarBackground.SourceRect.Width - 56 - BattleAssets.EnemyPokemonAssets.Level.SourceRect.Width, BattleAssets.EnemyPokemonAssets.Name.Position.Y));
            BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAssets.EnemyHPBarBackground.Position.Y + 68);

            if (BattleScreen.TextBox.Page == 3 && !BattleScreen.TextBox.IsTransitioning)
            {
                BattleAssets.State = BattleAssets.BattleState.PLAYER_SEND_POKEMON;
                BattleAssets.Animation = new PlayerSendPokemon();
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
