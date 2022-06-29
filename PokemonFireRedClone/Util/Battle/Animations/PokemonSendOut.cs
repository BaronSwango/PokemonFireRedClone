using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonSendOut : BattleAnimation
    {

        private float pokeballSpeedY;
        private bool maxHeight;

        public PokemonSendOut()
        {
            pokeballSpeedY = 4;
            CreateWhiteEffect();
        }

        public override bool Animate(GameTime gameTime)
        {
            CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Alpha == 0)
                ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Alpha = 1;

            float ballmaxHeight = 296;
            float ballSpeedX = (float)(0.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float playerSpeed;

            if (!maxHeight)
            {
                ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y -= pokeballSpeedY;
                if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y <= ballmaxHeight)
                {
                    maxHeight = true;
                    pokeballSpeedY = 1;
                }
            }
            else
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y)
                {
                    ballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Angle += 0.7f;
                    ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y += pokeballSpeedY;
                    pokeballSpeedY *= 1.2f;
                }
            }

            ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.X += ballSpeedX;

            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite!= null)
            {
                playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float playerSpriteDestinationX = -ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.SourceRect.Width - 8;
                if (!(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;
                    return false; ;
                }

                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite.UnloadContent();
            }

            if (ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.Position.Y < ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y) return false;

            if ((ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X + 0.05f < 1 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y + 0.05f < 1) || !WhiteEffectTransitioned)
            {
                WhiteEffect.Alpha += 0.05f;

                if (WhiteEffect.Alpha >= 1)
                    WhiteEffectTransitioned = true;

                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X + 0.05f < 1 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y + 0.05f < 1)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X + 0.05f, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y + 0.05f);
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Width / 2, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Height - (int)(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y));
                }
                return false;
            }

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale = Vector2.One;
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Width / 2, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Height - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height);
            ScreenManager.Instance.BattleScreen.BattleAssets.PokeOriginalY = ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position.Y;

            float playerHPDestX = ScreenManager.Instance.Dimensions.X - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 40;
            playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint != Color.White || WhiteEffect.Alpha > 0 || ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X -= playerSpeed;
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.OffsetX(-playerSpeed);
                    if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Gender != null)
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Gender.OffsetX(-playerSpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.OffsetX(-playerSpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.OffsetX(-playerSpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.OffsetX(-playerSpeed);
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Position.X -= playerSpeed;
                    ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Position.X -= playerSpeed;
                }

                if (WhiteEffect.Alpha > 0)
                    WhiteEffect.Alpha -= 0.05f;
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint = new Color(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint.R + 20, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint.G + 20, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint.B + 20, 255);
                return false;
            }

            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X = playerHPDestX;
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 64, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 19));
            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Gender != null)
                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Gender.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 92));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + 68);
            ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.Scale.X) / 2 * ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar.SourceRect.Width), ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);

            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite== null)
            {
                if (Counter < 500)
                {
                    Counter += CounterSpeed;
                    return false;
                }

                Counter = 0;

            }

            WhiteEffect.Alpha = 0;
            WhiteEffect.UnloadContent();

            ResetPokeball();
            ScreenManager.Instance.BattleScreen.BattleAssets.Pokeball.UnloadContent();
            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = false;

            if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerSprite != null)
            {
                ScreenManager.Instance.BattleScreen.TextBox.NextPage = 4;
                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            }
            else
                ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = true;

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WhiteEffect.Draw(spriteBatch);
        }
    }
}
