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

            if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint != Color.White || ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X += enemySpeed;
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.OffsetX(enemySpeed);
                    if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender != null)
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender.OffsetX(enemySpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.OffsetX(enemySpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position.X += enemySpeed;
                }

                if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint != Color.White)
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint = new Color(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.R + 3, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.G + 3, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Tint.B + 3, 255);
                return false;
            }
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X = enemyHPDestinationX;
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 24, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 19));
            if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender != null)
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Gender.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.SourceRect.Width - 56 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Level.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 68);

            if (ScreenManager.Instance.BattleScreen.TextBox.Page == 3 && !ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.PLAYER_SEND_POKEMON;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PlayerSendPokemon();
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        { }
    }
}
