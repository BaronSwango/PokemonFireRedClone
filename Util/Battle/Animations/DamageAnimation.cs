using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class DamageAnimation : BattleAnimation
    {

        // change constant speeds to delta time
        private int blinkCounter;

        public DamageAnimation() {
            blinkCounter = 0;
        }

        public override bool Animate(GameTime gameTime)
        {
            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                bool player = ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND;
                //CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //FIX MOVE HIT
                if (ScreenManager.Instance.BattleScreen.BattleLogic.MoveLanded && ScreenManager.Instance.BattleScreen.TextBox.Page != 20)
                {
                    if (blinkCounter < 4)
                    {
                        Counter ??= new Counter(60);

                        //if (Counter > 60)
                        if (Counter.Finished)
                        {
                            if (player)
                            {
                                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Alpha == 1)
                                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Alpha = 0;
                                else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Alpha == 0)
                                {
                                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Alpha = 1;
                                    blinkCounter++;
                                }
                            }
                            else
                            {
                                if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha == 1)
                                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha = 0;
                                else if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha == 0)
                                {
                                    ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemon.Alpha = 1;
                                    blinkCounter++;
                                }
                            }
                            //Counter = 0;
                            Counter.Reset();
                            if (blinkCounter == 4)
                            {
                                Counter.Reset(1000);
                                return false;
                            }
                        }
                        //Counter += CounterSpeed;
                        Counter.Update(gameTime);
                        return false;
                    }


                    float goalScale = player ? (float)BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP / BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP : (float)BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP / BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP;
                    int goalHP = 0;
                    if (player)
                        goalHP = BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP;
                    float speed = (float) (1.25 * gameTime.ElapsedGameTime.TotalSeconds); // 0.01

                    if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP < 50) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP < 50)
                        speed = (float) (2.5 * gameTime.ElapsedGameTime.TotalSeconds); // 0.04
                    else if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP >= 100) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP >= 100)
                        speed = (float) (0.625 * gameTime.ElapsedGameTime.TotalSeconds);


                    if (player)
                    {
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X - speed > goalScale)
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X -= speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X);
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 68);
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.UnloadContent();
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(((int)(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X * BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP)).ToString());
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
                            return false;
                        }

                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.UnloadContent();
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.UpdateText(goalHP.ToString());
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X = goalScale;
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CalculateHealthBarColor(goalScale);
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 68);

                    }
                    else
                    {
                        if (ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X - speed > goalScale)
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X -= speed;
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.CalculateHealthBarColor(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X);
                            ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 68);
                            return false;
                        }

                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X = goalScale;
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.CalculateHealthBarColor(goalScale);
                        ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.X + 156 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.EnemyHPBarBackground.Position.Y + 68);
                    }

                }


                //if (Counter < 1000.0f)
                if (!Counter.Finished)
                {
                    //Counter += CounterSpeed;
                    Counter.Update(gameTime);
                    return false;
                }

                if (!ScreenManager.Instance.BattleScreen.BattleLogic.MoveLanded)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 20;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    ScreenManager.Instance.BattleScreen.BattleLogic.MoveLanded = true;
                    //Counter = 0;
                    Counter.Reset();
                    return false;
                }

                if (ScreenManager.Instance.BattleScreen.BattleLogic.Crit)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 15;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    ScreenManager.Instance.BattleScreen.BattleLogic.Crit = false;
                    //Counter = 0;
                    Counter.Reset();
                    return false;
                }

                if (ScreenManager.Instance.BattleScreen.BattleLogic.SuperEffective)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 7;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    ScreenManager.Instance.BattleScreen.BattleLogic.SuperEffective = false;
                    //Counter = 0;
                    Counter.Reset();
                    return false;
                }
                else if (ScreenManager.Instance.BattleScreen.BattleLogic.NotVeryEffective)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 8;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    ScreenManager.Instance.BattleScreen.BattleLogic.NotVeryEffective = false;
                    //Counter = 0;
                    Counter.Reset();
                    return false;
                }

                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = null;

                if (player)
                    ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved = true;
                else
                    ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = true;

                if (ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved && ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved)
                    EndFightSequence();


                if (player)
                {
                    if (BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP == 0)
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_FAINT;
                        ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.PLAYER_FAINT;
                        ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonFaint();
                        ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                        ScreenManager.Instance.BattleScreen.BattleLogic.PokemonFainted = true;
                    }
                    else
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;
                        ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                    }
                }
                else
                {
                    if (BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP == 0)
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_FAINT;
                        ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.ENEMY_FAINT;
                        ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonFaint();
                        ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                        ScreenManager.Instance.BattleScreen.BattleLogic.PokemonFainted = true;
                    }
                    else
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;
                        ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
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
