using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PlayerSendPokemon : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            float playerSpriteDestinationX = -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width - 8;

            float playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > 0)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 1;
            else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X <= 0 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 6)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 2;
            else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X <= -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 6 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 3)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 3;
            else
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 4;
                ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.LoadContent();

                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonSendOut();
                ResetPokeball();

                return true;

            }

            if (!(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
