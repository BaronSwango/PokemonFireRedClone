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
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, BattleAnimations.Background.SourceRect.Width, BattleAnimations.Background.SourceRect.Height)
            };
            Color[] data = new Color[BattleAnimations.Background.SourceRect.Width * BattleAnimations.Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            whiteBackground.LoadContent();
        }

        public override bool Animate(GameTime gameTime)
        {
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (BattleAnimations.Pokeball.Alpha == 0)
                BattleAnimations.Pokeball.Alpha = 1;

            float ballMaxHeight = 296;
            float ballSpeedX = (float)(0.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            float playerSpeed;

            if (!maxHeight)
            {
                BattleAnimations.Pokeball.Position.Y -= pokeballSpeedY;
                if (BattleAnimations.Pokeball.Position.Y <= ballMaxHeight)
                {
                    maxHeight = true;
                    pokeballSpeedY = 1;
                }
            }
            else
            {
                if (BattleAnimations.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                {
                    ballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    BattleAnimations.Pokeball.Angle += 0.7f;
                    BattleAnimations.Pokeball.Position.Y += pokeballSpeedY;
                    pokeballSpeedY *= 1.2f;
                }
            }

            BattleAnimations.Pokeball.Position.X += ballSpeedX;

            if (BattleAnimations.PlayerSprite!= null)
            {
                playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float playerSpriteDestinationX = -BattleAnimations.PlayerSprite.SourceRect.Width - 8;
                if (!(BattleAnimations.PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || BattleAnimations.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                {
                    BattleAnimations.PlayerSprite.Position.X -= (int)playerSpeed;
                    return false; ;
                }

                BattleAnimations.PlayerSprite.UnloadContent();
            }

            if (BattleAnimations.Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y) return false;

            if ((BattleAnimations.PlayerPokemon.Scale.X + 0.05f < 1 && BattleAnimations.PlayerPokemon.Scale.Y + 0.05f < 1) || !whiteBackgroundTransitioned)
            {
                whiteBackground.Alpha += 0.05f;

                if (whiteBackground.Alpha >= 1)
                    whiteBackgroundTransitioned = true;

                if (BattleAnimations.PlayerPokemon.Scale.X + 0.05f < 1 && BattleAnimations.PlayerPokemon.Scale.Y + 0.05f < 1)
                {
                    BattleAnimations.PlayerPokemon.Scale = new Vector2(BattleAnimations.PlayerPokemon.Scale.X + 0.05f, BattleAnimations.PlayerPokemon.Scale.Y + 0.05f);
                    BattleAnimations.PlayerPokemon.Position = new Vector2(BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width * 0.55f - BattleAnimations.PlayerPokemon.SourceRect.Width / 2, BattleAnimations.PlayerPlatform.Position.Y + BattleAnimations.PlayerPlatform.SourceRect.Height - (int)(BattleAnimations.PlayerPokemon.SourceRect.Height * BattleAnimations.PlayerPokemon.Scale.Y));
                }
                return false;
            }

            BattleAnimations.PlayerPokemon.Scale = Vector2.One;
            BattleAnimations.PlayerPokemon.Position = new Vector2(BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width * 0.55f - BattleAnimations.PlayerPokemon.SourceRect.Width / 2, BattleAnimations.PlayerPlatform.Position.Y + BattleAnimations.PlayerPlatform.SourceRect.Height - BattleAnimations.PlayerPokemon.SourceRect.Height);
            BattleAnimations.PokeOriginalY = BattleAnimations.PlayerPokemon.Position.Y;

            float playerHPDestX = ScreenManager.Instance.Dimensions.X - BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 40;
            playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
            if (BattleAnimations.PlayerPokemon.Tint != Color.White || whiteBackground.Alpha > 0 || BattleAnimations.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
            {
                if (BattleAnimations.PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                {
                    BattleAnimations.PlayerHPBarBackground.Position.X -= playerSpeed;
                    BattleAnimations.PlayerPokemonAssets.Name.OffsetX(-playerSpeed);
                    if (BattleAnimations.PlayerPokemonAssets.Gender != null)
                        BattleAnimations.PlayerPokemonAssets.Gender.OffsetX(-playerSpeed);
                    BattleAnimations.PlayerPokemonAssets.Level.OffsetX(-playerSpeed);
                    BattleAnimations.PlayerPokemonAssets.MaxHP.OffsetX(-playerSpeed);
                    BattleAnimations.PlayerPokemonAssets.CurrentHP.OffsetX(-playerSpeed);
                    BattleAnimations.PlayerPokemonAssets.HPBar.Position.X -= playerSpeed;
                    BattleAnimations.EXPBar.Position.X -= playerSpeed;
                }

                if (whiteBackground.Alpha > 0)
                    whiteBackground.Alpha -= 0.05f;
                BattleAnimations.PlayerPokemon.Tint = new Color(BattleAnimations.PlayerPokemon.Tint.R + 20, BattleAnimations.PlayerPokemon.Tint.G + 20, BattleAnimations.PlayerPokemon.Tint.B + 20, 255);
                return false;
            }

            BattleAnimations.PlayerHPBarBackground.Position.X = playerHPDestX;
            BattleAnimations.PlayerPokemonAssets.Name.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 64, BattleAnimations.PlayerHPBarBackground.Position.Y + 19));
            if (BattleAnimations.PlayerPokemonAssets.Gender != null)
                BattleAnimations.PlayerPokemonAssets.Gender.SetPosition(new Vector2(BattleAnimations.PlayerPokemonAssets.Name.Position.X + BattleAnimations.PlayerPokemonAssets.Name.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.Name.Position.Y));
            BattleAnimations.PlayerPokemonAssets.Level.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAnimations.PlayerPokemonAssets.Level.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.Name.Position.Y));
            BattleAnimations.PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 36 - BattleAnimations.PlayerPokemonAssets.MaxHP.SourceRect.Width, BattleAnimations.PlayerHPBarBackground.Position.Y + 92));
            BattleAnimations.PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + BattleAnimations.PlayerHPBarBackground.SourceRect.Width - 116 - BattleAnimations.PlayerPokemonAssets.CurrentHP.SourceRect.Width, BattleAnimations.PlayerPokemonAssets.MaxHP.Position.Y));
            BattleAnimations.PlayerPokemonAssets.HPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 192 - ((1 - BattleAnimations.PlayerPokemonAssets.HPBar.Scale.X) / 2 * BattleAnimations.PlayerPokemonAssets.HPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + 68);
            BattleAnimations.EXPBar.Position = new Vector2(BattleAnimations.PlayerHPBarBackground.Position.X + 128 - ((1 - BattleAnimations.EXPBar.Scale.X) / 2 * BattleAnimations.EXPBar.SourceRect.Width), BattleAnimations.PlayerHPBarBackground.Position.Y + BattleAnimations.PlayerHPBarBackground.SourceRect.Height - 16);

            if (BattleAnimations.PlayerSprite== null)
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
            BattleAnimations.Pokeball.UnloadContent();
            BattleAnimations.IsTransitioning = false;

            if (BattleAnimations.PlayerSprite != null)
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
