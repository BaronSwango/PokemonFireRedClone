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
                BattleAnimations.PlayerHPBarLevelUp.Alpha += 0.1f;

                if (BattleAnimations.PlayerHPBarLevelUp.Alpha >= 1)
                    levelUpTransitioned = true;

                return false;
            }
            else if (BattleAnimations.PlayerHPBarLevelUp.Alpha > 0)
            {
                BattleAnimations.PlayerHPBarLevelUp.Alpha -= 0.1f;
                return false;
            }

            BattleAnimations.PlayerHPBarLevelUp.Alpha = 0;

            BattleAnimations.PlayerPokemonAssets.Level.UpdateText("Lv" + (int.Parse(BattleAnimations.PlayerPokemonAssets.Level.Text.Text[2..]) + 1).ToString());

            int level = int.Parse(BattleAnimations.PlayerPokemonAssets.Level.Text.Text[2..]);
            int newCurrentHP = int.Parse(BattleAnimations.PlayerPokemonAssets.CurrentHP.Text.Text) + (PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP - PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level - 1).HP);
            BattleAnimations.PlayerPokemonAssets.CurrentHP.UpdateText(newCurrentHP.ToString());
            BattleAnimations.PlayerPokemonAssets.MaxHP.UpdateText(PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP.ToString());

            BattleAnimations.PlayerPokemonAssets.Level.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAnimations.PlayerPokemonAssets.Level.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.Name.Position.Y));
            BattleAnimations.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAnimations.PlayerPokemonAssets.MaxHP.SourceRect.Width, BattleAnimations.PlayerHPBarBackground.Position.Y + 92));
            BattleAnimations.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAnimations.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.MaxHP.Position.Y));

            float healthScale = (float)newCurrentHP / PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP;
            BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X = healthScale;
            BattleAnimations.PlayerPokemonAssets.CalculateHealthBarColor(healthScale);
            BattleAnimations.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + 68);

            BattleAnimations.EXPBar.Scale.X = 0;
            levelUpTransitioned = false;

            BattleAnimations.IsTransitioning = false;
            BattleScreen.TextBox.NextPage = 17;
            BattleScreen.TextBox.IsTransitioning = true;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
