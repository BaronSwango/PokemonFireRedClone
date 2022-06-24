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
            if (!BattleScreen.TextBox.IsTransitioning)
            {
                bool player = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND;
                counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //FIX MOVE HIT
                if (BattleScreen.BattleLogic.MoveHit && BattleScreen.TextBox.Page != 20)
                {
                    if (blinkCounter < 4)
                    {
                        if (counter > 60)
                        {
                            if (player)
                            {
                                if (BattleAssets.PlayerPokemon.Alpha == 1)
                                    BattleAssets.PlayerPokemon.Alpha = 0;
                                else if (BattleAssets.PlayerPokemon.Alpha == 0)
                                {
                                    BattleAssets.PlayerPokemon.Alpha = 1;
                                    blinkCounter++;
                                }
                            }
                            else
                            {
                                if (BattleAssets.EnemyPokemon.Alpha == 1)
                                    BattleAssets.EnemyPokemon.Alpha = 0;
                                else if (BattleAssets.EnemyPokemon.Alpha == 0)
                                {
                                    BattleAssets.EnemyPokemon.Alpha = 1;
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
                        if (BattleAssets.PlayerPokemonAssets.HPBar.Scale.X - speed > goalScale)
                        {
                            BattleAssets.PlayerPokemonAssets.HPBar.Scale.X -= speed;
                            BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(BattleAssets.PlayerPokemonAssets.HPBar.Scale.X);
                            BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + 68);
                            BattleAssets.PlayerPokemonAssets.CurrentHP.UnloadContent();
                            BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(((int)(BattleAssets.PlayerPokemonAssets.HPBar.Scale.X * BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP)).ToString());
                            BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
                            return false;
                        }

                        BattleAssets.PlayerPokemonAssets.CurrentHP.UnloadContent();
                        BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(goalHP.ToString());
                        BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
                        BattleAssets.PlayerPokemonAssets.HPBar.Scale.X = goalScale;
                        BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(goalScale);
                        BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + 68);

                    }
                    else
                    {
                        if (BattleAssets.EnemyPokemonAssets.HPBar.Scale.X - speed > goalScale)
                        {
                            BattleAssets.EnemyPokemonAssets.HPBar.Scale.X -= speed;
                            BattleAssets.EnemyPokemonAssets.CalculateHealthBarColor(BattleAssets.EnemyPokemonAssets.HPBar.Scale.X);
                            BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAssets.EnemyHPBarBackground.Position.Y + 68);
                            return false;
                        }

                        BattleAssets.EnemyPokemonAssets.HPBar.Scale.X = goalScale;
                        BattleAssets.EnemyPokemonAssets.CalculateHealthBarColor(goalScale);
                        BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), BattleAssets.EnemyHPBarBackground.Position.Y + 68);
                    }
                }


                if (counter < 1000.0f)
                {
                    counter += counterSpeed;
                    return false;
                }

                if (!BattleScreen.BattleLogic.MoveHit)
                {
                    BattleScreen.TextBox.NextPage = 20;
                    BattleScreen.TextBox.IsTransitioning = true;
                    BattleScreen.BattleLogic.MoveHit = true;
                    counter = 0;
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

                BattleAssets.Animation = null;

                if (player)
                    BattleScreen.BattleLogic.EnemyHasMoved = true;
                else
                    BattleScreen.BattleLogic.PlayerHasMoved = true;

                if (BattleScreen.BattleLogic.EnemyHasMoved && BattleScreen.BattleLogic.PlayerHasMoved)
                    endFightSequence();


                if (player)
                {
                    if (BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP == 0)
                    {
                        BattleAssets.State = BattleAssets.BattleState.POKEMON_FAINT;
                        BattleScreen.BattleLogic.State = BattleLogic.FightState.PLAYER_FAINT;
                        BattleAssets.Animation = new PokemonFaint();
                        BattleAssets.IsTransitioning = true;
                        BattleScreen.BattleLogic.PokemonFainted = true;
                    }
                    else
                    {
                        BattleAssets.IsTransitioning = false;
                        BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                    }
                }
                else
                {
                    if (BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP == 0)
                    {
                        BattleAssets.State = BattleAssets.BattleState.POKEMON_FAINT;
                        BattleScreen.BattleLogic.State = BattleLogic.FightState.ENEMY_FAINT;
                        BattleAssets.Animation = new PokemonFaint();
                        BattleAssets.IsTransitioning = true;
                        BattleScreen.BattleLogic.PokemonFainted = true;
                    }
                    else
                    {
                        BattleAssets.IsTransitioning = false;
                        BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                    }
                }

                return true;
            }
            else return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
