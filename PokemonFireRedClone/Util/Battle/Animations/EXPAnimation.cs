using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class EXPAnimation : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            int goalLevel = BattleLogic.Battle.PlayerPokemon.Pokemon.Level;
            float goalEXPScale = (float)BattleLogic.Battle.PlayerPokemon.Pokemon.EXPTowardsLevelUp / BattleLogic.Battle.PlayerPokemon.Pokemon.EXPNeededToLevelUp;
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (BattleAnimations.EXPBar.Scale.X + 0.01f < goalEXPScale || (BattleAnimations.EXPBar.Scale.X + 0.01f < 1 && int.Parse(BattleAnimations.PlayerPokemonAssets.Level.Text.Text[2..]) < goalLevel))
            {
                BattleAnimations.EXPBar.Scale.X += 0.01f;
                BattleAnimations.EXPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAnimations.EXPBar.Scale.X) / 2 * BattleAnimations.EXPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + BattleAnimations.PlayerHPBarBackground.SourceRect.Height - 16);
                return false;
            }

            if (BattleAnimations.EXPBar.Scale.X + 0.01f >= 1)
            {
                BattleAnimations.EXPBar.Scale.X = 1;
                BattleAnimations.EXPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAnimations.EXPBar.Scale.X) / 2 * BattleAnimations.EXPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + BattleAnimations.PlayerHPBarBackground.SourceRect.Height - 16);
                BattleAnimations.State = BattleAnimations.BattleState.LEVEL_UP_ANIMATION;
                return false;
            }
            else
            {
                BattleAnimations.EXPBar.Scale.X = goalEXPScale;
                BattleAnimations.EXPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAnimations.EXPBar.Scale.X) / 2 * BattleAnimations.EXPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + BattleAnimations.PlayerHPBarBackground.SourceRect.Height - 16);
            }

            if (counter < 1000.0f)
            {
                counter += counterSpeed;
                return false;
            }

            if (BattleLogic.Battle.IsWild)
            {
                ScreenManager.Instance.ChangeScreens("GameplayScreen");
                BattleLogic.EndBattle();
            }
            BattleScreen.BattleLogic.LevelUp = false;
            BattleAnimations.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
