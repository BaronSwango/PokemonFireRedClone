using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class DamageAnimation : BattleAnimation
    {

        int blinkCounter;

        public DamageAnimation() {
            blinkCounter = 0;
        }

        public override bool Animate(GameTime gameTime)
        {
            bool player = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND;
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (blinkCounter < 4)
            {
                if (counter > 60)
                {
                    if (player)
                    {
                        if (BattleAnimations.PlayerPokemon.Alpha == 1)
                            BattleAnimations.PlayerPokemon.Alpha = 0;
                        else if (BattleAnimations.PlayerPokemon.Alpha == 0)
                        {
                            BattleAnimations.PlayerPokemon.Alpha = 1;
                            blinkCounter++;
                        }
                    }
                    else
                    {
                        if (BattleAnimations.EnemyPokemon.Alpha == 1)
                            BattleAnimations.EnemyPokemon.Alpha = 0;
                        else if (BattleAnimations.EnemyPokemon.Alpha == 0)
                        {
                            BattleAnimations.EnemyPokemon.Alpha = 1;
                            blinkCounter++;
                        }
                    }
                    counter = 0;
                    if (blinkCounter == 4) return false;
                }
                counter += counterSpeed;
                return false;
            }


            float goalScale = player ? (float)BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP / BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP : (float)BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP / BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP;
            int goalHP = 0;
            if (player)
                goalHP = BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP;
            float speed = 0.01f;

            if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP < 50) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP < 50)
                speed = 0.04f;
            else if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP >= 100) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP >= 100)
                speed = 0.005f;


            if (player)
            {
                if (BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X - speed > goalScale)
                {
                    BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X -= speed;
                    BattleAnimations.PlayerPokemonAssets.CalculateHealthBarColor(BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X);
                    BattleAnimations.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + 68);
                    BattleAnimations.PlayerPokemonAssets.CurrentHP.UnloadContent();
                    BattleAnimations.PlayerPokemonAssets.CurrentHP.UpdateText(((int)(BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X * BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP)).ToString());
                    BattleAnimations.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAnimations.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.MaxHP.Position.Y));
                    return false;
                }

                BattleAnimations.PlayerPokemonAssets.CurrentHP.UnloadContent();
                BattleAnimations.PlayerPokemonAssets.CurrentHP.UpdateText(goalHP.ToString());
                BattleAnimations.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAnimations.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.MaxHP.Position.Y));
                BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X = goalScale;
                BattleAnimations.PlayerPokemonAssets.CalculateHealthBarColor(goalScale);
                BattleAnimations.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + 68);

            }
            else
            {
                if (BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X - speed > goalScale)
                {
                    BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X -= speed;
                    BattleAnimations.EnemyPokemonAssets.CalculateHealthBarColor(BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X);
                    BattleAnimations.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.EnemyHPBarBackground.Position.Y + 68);
                    return false;
                }

                BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X = goalScale;
                BattleAnimations.EnemyPokemonAssets.CalculateHealthBarColor(goalScale);
                BattleAnimations.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAnimations.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.EnemyHPBarBackground.Position.Y + 68);
            }



            if (counter < 1000.0f)
            {
                counter += counterSpeed;
                return false;
            }

            if (BattleScreen.BattleLogic.Crit)
            {
                BattleScreen.TextBox.NextPage = 15;
                BattleScreen.TextBox.IsTransitioning = true;
                BattleScreen.BattleLogic.Crit = false;
                counter = 0;
                return false;
            }

            if (BattleScreen.BattleLogic.SuperEffective)
            {
                BattleScreen.TextBox.NextPage = 7;
                BattleScreen.TextBox.IsTransitioning = true;
                BattleScreen.BattleLogic.SuperEffective = false;
                counter = 0;
                return false;
            }
            else if (BattleScreen.BattleLogic.NotVeryEffective)
            {
                BattleScreen.TextBox.NextPage = 8;
                BattleScreen.TextBox.IsTransitioning = true;
                BattleScreen.BattleLogic.NotVeryEffective = false;
                counter = 0;
                return false;
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
