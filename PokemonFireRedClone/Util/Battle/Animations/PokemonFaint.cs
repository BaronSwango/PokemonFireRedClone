using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonFaint : BattleAnimation
    {

        //TODO: REPLACE WITH SOUND ENDING TO TRIGGER INSTEAD OF COUNTER
        // TODO: TEXTBOX FAINT MESSAGE WITH ARROW (CHECK WILD VS TRAINER FOR SPECIFIC MESSAGE)
        // - AFTER CLICKING PAST ARROW, GO TO GAMEPLAY SCREEN

        public override bool Animate(GameTime gameTime)
        {
            bool player = ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT;
            CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Counter < 1000.0f)
            {
                Counter += CounterSpeed;
                return false;
            }

            if (player)
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height - 16 > 0)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height -= 16;
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Width / 2, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Height - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height);
                    return false;
                }
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height = 0;
            }
            else
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height - 12 > 0)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height -= 12;
                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Width / 2 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Width / 2, ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height);
                    return false;
                }
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.SourceRect.Height = 0;

                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Alpha = 0;
                ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.SetAlpha(0);
                if (!BattleLogic.Battle.IsWild)
                    ScreenManager.Instance.BattleScreen.BattleAssets.RefreshTrainerBalls();
            }


            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;

            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 9;
            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;

            return true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
