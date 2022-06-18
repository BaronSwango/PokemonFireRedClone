using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PlayerSendPokemon : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            float playerSpriteDestinationX = -BattleAnimations.PlayerSprite.SourceRect.Width - 8;

            float playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (BattleAnimations.PlayerSprite.Position.X > 0)
                BattleAnimations.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 1;
            else if (BattleAnimations.PlayerSprite.Position.X <= 0 && BattleAnimations.PlayerSprite.Position.X > -BattleAnimations.PlayerSprite.SourceRect.Width / 6)
                BattleAnimations.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 2;
            else if (BattleAnimations.PlayerSprite.Position.X <= -BattleAnimations.PlayerSprite.SourceRect.Width / 6 && BattleAnimations.PlayerSprite.Position.X > -BattleAnimations.PlayerSprite.SourceRect.Width / 3)
                BattleAnimations.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 3;
            else
            {
                BattleAnimations.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 4;
                BattleAnimations.Pokeball.LoadContent();
                return true;

            }

            if (!(BattleAnimations.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || BattleAnimations.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                BattleAnimations.PlayerSprite.Position.X -= (int)playerSpeed;
            

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
