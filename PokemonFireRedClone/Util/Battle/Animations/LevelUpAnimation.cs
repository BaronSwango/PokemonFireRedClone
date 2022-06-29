using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class LevelUpAnimation : BattleAnimation
    {
        private bool levelUpTransitioned;

        public override bool Animate(GameTime gameTime)
        {
            if (!levelUpTransitioned)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarLevelUp.Alpha += 0.1f;

                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarLevelUp.Alpha >= 1)
                    levelUpTransitioned = true;

                return false;
            }
            else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarLevelUp.Alpha > 0)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarLevelUp.Alpha -= 0.1f;
                return false;
            }

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarLevelUp.Alpha = 0;

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.UpdateText("Lv" + (int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]) + 1).ToString());

            int level = int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]);
            int newCurrentHP = int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.Text.Text) + (PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP - PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level - 1).HP);
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(newCurrentHP.ToString());
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.UpdateText(PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP.ToString());

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 92));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));

            float healthScale = (float)newCurrentHP / PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP;
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X = healthScale;
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(healthScale);
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 68);

            ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X = 0;
            levelUpTransitioned = false;

            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;
            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 17;
            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
