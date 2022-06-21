using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PlayerSendPokemon : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            float playerSpriteDestinationX = -BattleAssets.PlayerSprite.SourceRect.Width - 8;

            float playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (BattleAssets.PlayerSprite.Position.X > 0)
                BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 1;
            else if (BattleAssets.PlayerSprite.Position.X <= 0 && BattleAssets.PlayerSprite.Position.X > -BattleAssets.PlayerSprite.SourceRect.Width / 6)
                BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 2;
            else if (BattleAssets.PlayerSprite.Position.X <= -BattleAssets.PlayerSprite.SourceRect.Width / 6 && BattleAssets.PlayerSprite.Position.X > -BattleAssets.PlayerSprite.SourceRect.Width / 3)
                BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 3;
            else
            {
                BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 4;
                BattleAssets.Pokeball.LoadContent();

                BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                BattleAssets.Animation = new PokemonSendOut();
                resetPokeball();

                return true;

            }

            if (!(BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || BattleAssets.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
