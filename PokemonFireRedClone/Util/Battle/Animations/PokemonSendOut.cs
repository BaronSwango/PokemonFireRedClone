using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonSendOut : BattleAnimation
    {
        Image whiteBackground;
        bool whiteBackgroundTransitioned;
        float pokeballSpeedY;
        bool maxHeight;

        public PokemonSendOut()
        {
            pokeballSpeedY = 4;
            whiteBackground = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, BattleAssets.Background.SourceRect.Width, BattleAssets.Background.SourceRect.Height)
            };
            Color[] data = new Color[BattleAssets.Background.SourceRect.Width * BattleAssets.Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            whiteBackground.LoadContent();
        }

        public override bool Animate(GameTime gameTime)
        {
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (BattleAssets.Pokeball.Alpha == 0)
                BattleAssets.Pokeball.Alpha = 1;

            float ballMaxHeight = 296;
            float ballSpeedX = (float)(0.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float playerSpeed;

            if (!maxHeight)
            {
                BattleAssets.Pokeball.Position.Y -= pokeballSpeedY;
                if (BattleAssets.Pokeball.Position.Y <= ballMaxHeight)
                {
                    maxHeight = true;
                    pokeballSpeedY = 1;
                }
            }
            else
            {
                if (BattleAssets.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                {
                    ballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    BattleAssets.Pokeball.Angle += 0.7f;
                    BattleAssets.Pokeball.Position.Y += pokeballSpeedY;
                    pokeballSpeedY *= 1.2f;
                }
            }

            BattleAssets.Pokeball.Position.X += ballSpeedX;

            if (BattleAssets.PlayerSprite!= null)
            {
                playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float playerSpriteDestinationX = -BattleAssets.PlayerSprite.SourceRect.Width - 8;
                if (!(BattleAssets.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || BattleAssets.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                {
                    BattleAssets.PlayerSprite.Position.X -= (int)playerSpeed;
                    return false; ;
                }

                BattleAssets.PlayerSprite.UnloadContent();
            }

            if (BattleAssets.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y) return false;

            if ((BattleAssets.PlayerPokemon.Scale.X + 0.05f < 1 && BattleAssets.PlayerPokemon.Scale.Y + 0.05f < 1) || !whiteBackgroundTransitioned)
            {
                whiteBackground.Alpha += 0.05f;

                if (whiteBackground.Alpha >= 1)
                    whiteBackgroundTransitioned = true;

                if (BattleAssets.PlayerPokemon.Scale.X + 0.05f < 1 && BattleAssets.PlayerPokemon.Scale.Y + 0.05f < 1)
                {
                    BattleAssets.PlayerPokemon.Scale = new Vector2(BattleAssets.PlayerPokemon.Scale.X + 0.05f, BattleAssets.PlayerPokemon.Scale.Y + 0.05f);
                    BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - (int)(BattleAssets.PlayerPokemon.SourceRect.Height * BattleAssets.PlayerPokemon.Scale.Y));
                }
                return false;
            }

            BattleAssets.PlayerPokemon.Scale = Vector2.One;
            BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - BattleAssets.PlayerPokemon.SourceRect.Height);
            BattleAssets.PokeOriginalY = BattleAssets.PlayerPokemon.Position.Y;

            float playerHPDestX = ScreenManager.Instance.Dimensions.X - BattleAssets.PlayerHPBarBackground.SourceRect.Width - 40;
            playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            if (BattleAssets.PlayerPokemon.Tint != Color.White || whiteBackground.Alpha > 0 || BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
            {
                if (BattleAssets.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                {
                    BattleAssets.PlayerHPBarBackground.Position.X -= playerSpeed;
                    BattleAssets.PlayerPokemonAssets.Name.OffsetX(-playerSpeed);
                    if (BattleAssets.PlayerPokemonAssets.Gender != null)
                        BattleAssets.PlayerPokemonAssets.Gender.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.Level.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.MaxHP.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.CurrentHP.OffsetX(-playerSpeed);
                    BattleAssets.PlayerPokemonAssets.HPBar.Position.X -= playerSpeed;
                    BattleAssets.EXPBar.Position.X -= playerSpeed;
                }

                if (whiteBackground.Alpha > 0)
                    whiteBackground.Alpha -= 0.05f;
                BattleAssets.PlayerPokemon.Tint = new Color(BattleAssets.PlayerPokemon.Tint.R + 20, BattleAssets.PlayerPokemon.Tint.G + 20, BattleAssets.PlayerPokemon.Tint.B + 20, 255);
                return false;
            }

            BattleAssets.PlayerHPBarBackground.Position.X = playerHPDestX;
            BattleAssets.PlayerPokemonAssets.Name.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 64, BattleAssets.PlayerHPBarBackground.Position.Y + 19));
            if (BattleAssets.PlayerPokemonAssets.Gender != null)
                BattleAssets.PlayerPokemonAssets.Gender.SetPosition(new Vector2(BattleAssets.PlayerPokemonAssets.Name.Position.X + BattleAssets.PlayerPokemonAssets.Name.SourceRect.Width, BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            BattleAssets.PlayerPokemonAssets.Level.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.Level.SourceRect.Width, BattleAssets.PlayerPokemonAssets.Name.Position.Y));
            BattleAssets.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAssets.PlayerPokemonAssets.MaxHP.SourceRect.Width, BattleAssets.PlayerHPBarBackground.Position.Y + 92));
            BattleAssets.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + BattleAssets.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAssets.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAssets.PlayerPokemonAssets.MaxHP.Position.Y));
            BattleAssets.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAssets.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAssets.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + 68);
            BattleAssets.EXPBar.Position = new Vector2(BattleAssets.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAssets.EXPBar.Scale.X) / 2 * BattleAssets.EXPBar.SourceRect.Width), BattleAssets.PlayerHPBarBackground.Position.Y + BattleAssets.PlayerHPBarBackground.SourceRect.Height - 16);

            if (BattleAssets.PlayerSprite== null)
            {
                if (counter < 500)
                {
                    counter += counterSpeed;
                    return false;
                }

                counter = 0;

            }

            whiteBackground.Alpha = 0;
            whiteBackground.UnloadContent();

            resetPokeball();
            BattleAssets.Pokeball.UnloadContent();
            BattleAssets.IsTransitioning = false;

            if (BattleAssets.PlayerSprite != null)
            {
                BattleScreen.TextBox.NextPage = 4;
                BattleScreen.TextBox.IsTransitioning = true;
            }
            else
                BattleScreen.BattleLogic.PlayerHasMoved = true;

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            whiteBackground.Draw(spriteBatch);
        }
    }
}
