﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class EXPAnimation : BattleAnimation
    {

        // change constant speeds to delta time
        public override bool Animate(GameTime gameTime)
        {
            float scaleSpeed = (float) (1.15 * gameTime.ElapsedGameTime.TotalSeconds);

            int goalLevel = BattleLogic.Battle.PlayerPokemon.Pokemon.Level;
            float goalEXPScale = (float)BattleLogic.Battle.PlayerPokemon.Pokemon.EXPTowardsLevelUp / BattleLogic.Battle.PlayerPokemon.Pokemon.EXPNeededToLevelUp;
            //CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // if (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + 0.01f < goalEXPScale || (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + 0.01f < 1 && int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Image.Text[2..]) < goalLevel))
            if (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + scaleSpeed < goalEXPScale || (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + scaleSpeed < 1 && int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Image.Text[2..]) < goalLevel))
            {
                // ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X += 0.01f;
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X += scaleSpeed;
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
                return false;
            }

            // if (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + 0.01f >= 1)
            if (ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X + scaleSpeed >= 1)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X = 1;
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);

                if (int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Image.Text[2..]) < goalLevel)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.LEVEL_UP_ANIMATION;
                    ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new LevelUpAnimation();
                    return false;
                }
            }
            else
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X = goalEXPScale;
                ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
            }

            Counter ??= BattleLogic.Battle.IsWild ? new Counter(1000) : new Counter(500);

            if (BattleLogic.Battle.IsWild)
            {
                //if (Counter < 1000)
                if (!Counter.Finished)
                {
                    //Counter += CounterSpeed;
                    Counter.Update(gameTime);
                    return false;
                }

                ScreenManager.Instance.ChangeScreens("GameplayScreen");
                BattleLogic.EndBattle();
            } else
            {
                //if (Counter < 500)
                if (!Counter.Finished)
                {
                    //Counter += CounterSpeed;
                    Counter.Update(gameTime);
                    return false;
                }

                if (BattleLogic.Battle.IsEnded)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 22;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    BattleAssets.IsTransitioning = false;
                }
                else
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.OPPONENT_SEND_POKEMON;
                    ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new TrainerBallBarAnimation();
                }
                return true;

            }
            ScreenManager.Instance.BattleScreen.BattleLogic.LevelUp = false;
            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
