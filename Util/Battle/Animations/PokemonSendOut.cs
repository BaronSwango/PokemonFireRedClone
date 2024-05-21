using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonSendOut : BattleAnimation
    {
        // change constants to delta time speed
        /*
            pokeball angle = 0.7
            pokeball accel = 1.1
            whiteeffect alpha decrease = 0.05
            pokemon scale = 0.05
            tint
        */

        private float pokeballSpeedY;
        private bool maxHeight;
        private Counter pauseCounter1;
        private Counter pauseCounter2;

        public PokemonSendOut()
        {
            pokeballSpeedY = 4;
            CreateWhiteEffect();
        }

        public override bool Animate(GameTime gameTime)
        {
            //CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (BattleAssets.Pokeball.Alpha == 0)
                BattleAssets.Pokeball.Alpha = 1;

            float ballmaxHeight = 296;
            float ballSpeedX = (float)(0.3 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float pokeballAccel = (float) (0.247 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float pokeballRotateSpeed = (float) (0.06 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float whiteEffectAlphaIncreaseSpeed = (float) (8.4 * gameTime.ElapsedGameTime.TotalSeconds);
            float whiteEffectAlphaDecreaseSpeed = (float) (4 * gameTime.ElapsedGameTime.TotalSeconds);
            float scaleSpeed = (float) (8.4 * gameTime.ElapsedGameTime.TotalSeconds);
            int tintSpeed = (int) (1.5 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float playerSpeed;

            if (!maxHeight)
            {
                BattleAssets.Pokeball.Position.Y -= pokeballSpeedY;
                if (BattleAssets.Pokeball.Position.Y <= ballmaxHeight)
                {
                    maxHeight = true;
                    pokeballSpeedY = (float) (0.055 * gameTime.ElapsedGameTime.TotalMilliseconds);
                }
            }
            else
            {
                if (BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y + 4)
                {
                    ballSpeedX = (float)(0.15 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    // BattleAssets.Pokeball.Angle += 0.7f;
                    BattleAssets.Pokeball.Angle -= pokeballRotateSpeed;
                    BattleAssets.Pokeball.Position.Y += pokeballSpeedY;
                    // pokeballSpeedY *= 1.1f;
                    pokeballSpeedY *= pokeballAccel;
                }
            }

            BattleAssets.Pokeball.Position.X += ballSpeedX;

            if (BattleAssets.PlayerSprite != null)
            {
                playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float playerSpriteDestinationX = -BattleAssets.PlayerSprite.SourceRect.Width - 8;
                if (!(BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y + 4)
                {
                    BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;
                    return false; ;
                }

                BattleAssets.PlayerSprite.UnloadContent();
            }

            if (BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y + 4) return false;

            if ((BattleAssets.PlayerPokemon.Scale.X + scaleSpeed < 1 && BattleAssets.PlayerPokemon.Scale.Y + scaleSpeed < 1) || !WhiteEffectTransitioned)
            {
                // WhiteEffect.Alpha += 0.05f;
                WhiteEffect.Alpha += whiteEffectAlphaIncreaseSpeed;

                if (WhiteEffect.Alpha >= 1)
                    WhiteEffectTransitioned = true;

                if (BattleAssets.PlayerPokemon.Scale.X + scaleSpeed < 1 && BattleAssets.PlayerPokemon.Scale.Y + scaleSpeed < 1)
                {
                    BattleAssets.PlayerPokemon.Scale = new Vector2(BattleAssets.PlayerPokemon.Scale.X + scaleSpeed, BattleAssets.PlayerPokemon.Scale.Y + scaleSpeed);
                    BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2,
                        BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - (int)(BattleAssets.PlayerPokemon.SourceRect.Height * BattleAssets.PlayerPokemon.Scale.Y));
                }
                return false;
            }

            BattleAssets.PlayerPokemon.Scale = Vector2.One;
            BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - BattleAssets.PlayerPokemon.SourceRect.Height);
            BattleAssets.PokeOriginalY = BattleAssets.PlayerPokemon.Position.Y;

            pauseCounter1 ??= new Counter(75);

            if (!pauseCounter1.Finished) 
            {
                pauseCounter1.Update(gameTime);
                return false;
            }

            float playerHPDestX = ScreenManager.Instance.Dimensions.X - BattleAssets.PlayerHPBarBackground.SourceRect.Width - 40;
            playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            if (BattleAssets.PlayerPokemon.Tint != Color.White || WhiteEffect.Alpha > 0 || BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
            {
                if (BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                {
                    BattleAssets.PlayerHPBarBackground.Position.X -= playerSpeed;
                    BattleAssets.PlayerPokemonAssets.Name.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.Gender?.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.Level.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.MaxHP.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.CurrentHP.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.HPBar.Position.X -= playerSpeed;
                    BattleAssets.EXPBar.Position.X -= playerSpeed;
                }

                if (WhiteEffect.Alpha - whiteEffectAlphaDecreaseSpeed > 0)
                {
                    // WhiteEffect.Alpha -= 0.05f;
                    WhiteEffect.Alpha -= whiteEffectAlphaDecreaseSpeed;
                }
                else
                {
                    WhiteEffect.Alpha = 0;
                }

                BattleAssets.PlayerPokemon.Tint = new Color(BattleAssets.PlayerPokemon.Tint.R + tintSpeed, BattleAssets.PlayerPokemon.Tint.G + tintSpeed, BattleAssets.PlayerPokemon.Tint.B + tintSpeed, 255);
                return false;
            }

            BattleAssets.PlayerHPBarBackground.Position.X = playerHPDestX;
            BattleAssets.PlayerPokemonAssets.Name.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 64, BattleAssets.PlayerHPBarBackground.Position.Y + 19));
            BattleAssets.PlayerPokemonAssets.Gender?.SetPosition(new Vector2(BattleAssets.PlayerPokemonAssets.Name.Position.X + BattleAssets.PlayerPokemonAssets.Name.SourceRect.Width, BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            BattleAssets.PlayerPokemonAssets.Level.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.Level.SourceRect.Width, BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            BattleAssets.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.MaxHP.SourceRect.Width, BattleAssets.PlayerHPBarBackground.Position.Y + 92));
            BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
            BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + 68);
            BattleAssets.EXPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAssets.EXPBar.Scale.X) / 2 * BattleAssets.EXPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);
            
            pauseCounter2 ??= new Counter(200);

            if (!pauseCounter2.Finished)
            {
                pauseCounter2.Update(gameTime);
                return false;
            } 

            WhiteEffect.UnloadContent();

            ResetPokeball();

            BattleAssets.IsTransitioning = false;

            if (BattleAssets.PlayerSprite != null)
            {
                ScreenManager.Instance.BattleScreen.TextBox.NextPage = 4;
                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            }
            else
            {
                ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = true;
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WhiteEffect.Draw(spriteBatch);
        }
    }
}
