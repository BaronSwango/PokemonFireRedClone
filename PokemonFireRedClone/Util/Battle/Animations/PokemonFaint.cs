using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonFaint : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            bool player = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT;
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (counter < 1000.0f)
            {
                counter += counterSpeed;
                return false;
            }

            if (player)
            {
                if (BattleAnimations.PlayerPokemon.SourceRect.Height - 16 > 0)
                {
                    BattleAnimations.PlayerPokemon.SourceRect.Height -= 16;
                    BattleAnimations.PlayerPokemon.Position = new Vector2(BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width * 0.55f - BattleAnimations.PlayerPokemon.SourceRect.Width / 2, BattleAnimations.PlayerPlatform.Position.Y + BattleAnimations.PlayerPlatform.SourceRect.Height - BattleAnimations.PlayerPokemon.SourceRect.Height);
                    return false;
                }
                BattleAnimations.PlayerPokemon.SourceRect.Height = 0;
            }
            else
            {
                if (BattleAnimations.EnemyPokemon.SourceRect.Height - 12 > 0)
                {
                    BattleAnimations.EnemyPokemon.SourceRect.Height -= 12;
                    BattleAnimations.EnemyPokemon.Position = new Vector2(BattleAnimations.EnemyPlatform.Position.X + BattleAnimations.EnemyPlatform.SourceRect.Width / 2 - BattleAnimations.EnemyPokemon.SourceRect.Width / 2, BattleAnimations.EnemyPlatform.Position.Y + BattleAnimations.EnemyPlatform.SourceRect.Height * 0.75f - BattleAnimations.EnemyPokemon.SourceRect.Height);
                    return false;
                }
                BattleAnimations.EnemyPokemon.SourceRect.Height = 0;
            }


            BattleAnimations.IsTransitioning = false;

            BattleScreen.TextBox.NextPage = 9;
            BattleScreen.TextBox.IsTransitioning = true;

            return true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
