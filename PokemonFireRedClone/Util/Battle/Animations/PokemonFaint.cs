using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonFaint : BattleAnimation
    {

        //TODO: REPLACE WITH SOUND ENDING TO TRIGGER INSTEAD OF COUNTER
        // TODO: TEXTBOX FAINT MESSAGE WITH ARROW (CHECK WILD VS TRAINER FOR SPECIFIC MESSAGE)
        // - AFTER CLICKING PAST ARROW, GO TO GAMEPLAY SCREEN

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
                if (BattleAssets.PlayerPokemon.SourceRect.Height - 16 > 0)
                {
                    BattleAssets.PlayerPokemon.SourceRect.Height -= 16;
                    BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - BattleAssets.PlayerPokemon.SourceRect.Height);
                    return false;
                }
                BattleAssets.PlayerPokemon.SourceRect.Height = 0;
            }
            else
            {
                if (BattleAssets.EnemyPokemon.SourceRect.Height - 12 > 0)
                {
                    BattleAssets.EnemyPokemon.SourceRect.Height -= 12;
                    BattleAssets.EnemyPokemon.Position = new Vector2(BattleAssets.EnemyPlatform.Position.X + BattleAssets.EnemyPlatform.SourceRect.Width / 2 - BattleAssets.EnemyPokemon.SourceRect.Width / 2, BattleAssets.EnemyPlatform.Position.Y + BattleAssets.EnemyPlatform.SourceRect.Height * 0.75f - BattleAssets.EnemyPokemon.SourceRect.Height);
                    return false;
                }
                BattleAssets.EnemyPokemon.SourceRect.Height = 0;
            }


            BattleAssets.IsTransitioning = false;

            BattleScreen.TextBox.NextPage = 9;
            BattleScreen.TextBox.IsTransitioning = true;

            return true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
