using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class LevelUpAnimation : BattleAnimation
    {
        bool levelUpTransitioned;

        public override bool Animate(GameTime gameTime)
        {
            if (!levelUpTransitioned)
            {
                BattleAssets.PlayerHPBarLevelUp.Alpha += 0.1f;

                if (BattleAssets.PlayerHPBarLevelUp.Alpha >= 1)
                    levelUpTransitioned = true;

                return false;
            }
            else if (BattleAssets.PlayerHPBarLevelUp.Alpha > 0)
            {
                BattleAssets.PlayerHPBarLevelUp.Alpha -= 0.1f;
                return false;
            }

            BattleAssets.PlayerHPBarLevelUp.Alpha = 0;

            BattleAssets.PlayerPokemonAssets.Level.UpdateText("Lv" + (int.Parse(BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]) + 1).ToString());

            int level = int.Parse(BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]);
            int newCurrentHP = int.Parse(BattleAssets.PlayerPokemonAssets.CurrentHP.Text.Text) + (PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP - PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level - 1).HP);
            BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(newCurrentHP.ToString());
            BattleAssets.PlayerPokemonAssets.MaxHP.UpdateText(PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP.ToString());

            BattleAssets.PlayerPokemonAssets.Level.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.Level.SourceRect.Width, BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            BattleAssets.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.MaxHP.SourceRect.Width, BattleAssets.PlayerHPBarBackground.Position.Y + 92));
            BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));

            float healthScale = (float)newCurrentHP / PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP;
            BattleAssets.PlayerPokemonAssets.HPBar.Scale.X = healthScale;
            BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(healthScale);
            BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + 68);

            BattleAssets.EXPBar.Scale.X = 0;
            levelUpTransitioned = false;

            BattleAssets.IsTransitioning = false;
            BattleScreen.TextBox.NextPage = 17;
            BattleScreen.TextBox.IsTransitioning = true;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
