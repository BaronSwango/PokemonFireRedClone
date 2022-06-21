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

            if (BattleAssets.EXPBar.Scale.X + 0.01f < goalEXPScale || (BattleAssets.EXPBar.Scale.X + 0.01f < 1 && int.Parse(BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]) < goalLevel))
            {
                BattleAssets.EXPBar.Scale.X += 0.01f;
                BattleAssets.EXPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAssets.EXPBar.Scale.X) / 2 * BattleAssets.EXPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
                return false;
            }

            if (BattleAssets.EXPBar.Scale.X + 0.01f >= 1)
            {
                BattleAssets.EXPBar.Scale.X = 1;
                BattleAssets.EXPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAssets.EXPBar.Scale.X) / 2 * BattleAssets.EXPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
                BattleAssets.State = BattleAssets.BattleState.LEVEL_UP_ANIMATION;
                BattleAssets.Animation = new LevelUpAnimation();
                return false;
            }
            else
            {
                BattleAssets.EXPBar.Scale.X = goalEXPScale;
                BattleAssets.EXPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAssets.EXPBar.Scale.X) / 2 * BattleAssets.EXPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
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
            BattleAssets.IsTransitioning = false;
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
