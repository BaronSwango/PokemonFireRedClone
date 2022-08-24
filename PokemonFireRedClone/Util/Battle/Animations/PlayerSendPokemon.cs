using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PlayerSendPokemon : BattleAnimation
    {
        int playerBallIndex;

        public override bool Animate(GameTime gameTime)
        {
            float playerSpriteDestinationX = -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width - 8;
            float playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float barSpeed = (float)(0.4 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float ballSpeed = (float)(7 * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > 0)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 1;
            else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X <= 0 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 6)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 2;
            else if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X <= -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 6 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X > -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width / 3)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 3;
            else
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 4;

                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonSendOut();
                ResetPokeball();

                BattleAssets.PlayerBallBar.Alpha = 0;
                foreach (Image image in BattleAssets.PlayerBarBalls)
                    image.Alpha = 0;

                return true;

            }

            if (!(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;

            if (!BattleLogic.Battle.IsWild && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBallBar.Alpha > 0)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBallBar.Position.X -= barSpeed;
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBallBar.Alpha -= 0.03f;

                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls[playerBallIndex].Position.X > 0)
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls[playerBallIndex].Position.X -= ballSpeed;

                if (playerBallIndex > 0)
                {
                    if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls[playerBallIndex - 1].Position.X > 0)
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls[playerBallIndex - 1].Position.X -= ballSpeed;
                }

                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls[playerBallIndex].Position.X < 250)
                {
                    if (playerBallIndex + 1 < ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls.Count)
                        playerBallIndex++;
                }

                foreach (Image image in ScreenManager.Instance.BattleScreen.BattleAssets.PlayerBarBalls)
                    image.Alpha -= 0.03f;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
