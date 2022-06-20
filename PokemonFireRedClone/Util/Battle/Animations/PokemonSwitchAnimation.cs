using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonSwitchAnimation : BattleAnimation
    {
        Image whiteBackground;
        bool whiteBackgroundTransitioned;
        bool draw;

        public PokemonSwitchAnimation()
        {
            whiteBackground = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, BattleAnimations.Background.SourceRect.Width, BattleAnimations.Background.SourceRect.Height)
            };
            //whiteBackground.LoadContent();
            Color[] data = new Color[BattleAnimations.Background.SourceRect.Width * BattleAnimations.Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            whiteBackground.LoadContent();
            draw = true;
        }

        public override bool Animate(GameTime gameTime)
        {
            counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!BattleScreen.TextBox.IsTransitioning)
            {
                if (BattleScreen.TextBox.Page != 3)
                {
                    if (!whiteBackground.IsLoaded)
                        whiteBackground.LoadContent();

                    if (BattleAnimations.PlayerPokemon.Tint != Color.Red)
                    {
                        BattleAnimations.PlayerPokemon.Tint = new Color(BattleAnimations.PlayerPokemon.Tint.R, BattleAnimations.PlayerPokemon.Tint.G - 20, BattleAnimations.PlayerPokemon.B - 20, 255);
                        whiteBackground.Alpha += 0.0784f;
                        return false;
                    }
                    BattleAnimations.PlayerPokemon.Tint = Color.Red;

                    if (whiteBackground.Alpha < 1 && !whiteBackgroundTransitioned)
                    {
                        whiteBackground.Alpha += 0.0784f;
                        return false;
                    }
                    whiteBackgroundTransitioned = true;

                    if (BattleAnimations.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAnimations.PlayerPokemon.Scale.Y - 0.05f > 0)
                    {
                        whiteBackground.Alpha -= 0.05f;

                        if (BattleAnimations.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAnimations.PlayerPokemon.Scale.Y - 0.05f > 0)
                        {
                            BattleAnimations.PlayerPokemon.Scale = new Vector2(BattleAnimations.PlayerPokemon.Scale.X - 0.05f, BattleAnimations.PlayerPokemon.Scale.Y - 0.05f);
                            BattleAnimations.PlayerPokemon.Position = new Vector2(BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width * 0.55f - BattleAnimations.PlayerPokemon.SourceRect.Width / 2, BattleAnimations.PlayerPlatform.Position.Y + BattleAnimations.PlayerPlatform.SourceRect.Height - (int)(BattleAnimations.PlayerPokemon.SourceRect.Height * BattleAnimations.PlayerPokemon.Scale.Y));
                        }
                        return false;
                    }

                    if (whiteBackgroundTransitioned && whiteBackground.Alpha - 0.05f > 0)
                    {
                        whiteBackground.Alpha -= 0.05f;
                        return false;
                    }
                    if (whiteBackground.Alpha != 0)
                        whiteBackground.Alpha = 0;



                    if (BattleAnimations.PlayerPokemon.Scale != Vector2.Zero)
                    {
                        BattleLogic.Battle.UpdatePlayerPokemon();

                        BattleAnimations.PlayerPokemon.Scale = Vector2.Zero;
                        BattleAnimations.PlayerPokemon.Position = new Vector2(BattleAnimations.PlayerPlatform.Position.X + BattleAnimations.PlayerPlatform.SourceRect.Width * 0.55f - BattleAnimations.PlayerPokemon.SourceRect.Width / 2, BattleAnimations.PlayerPlatform.Position.Y + BattleAnimations.PlayerPlatform.SourceRect.Height - BattleAnimations.PlayerPokemon.SourceRect.Height);
                        BattleAnimations.PokeOriginalY = BattleAnimations.PlayerPokemon.Position.Y;
                        BattleAnimations.PlayerPokemon.UnloadContent();

                        BattleAnimations.PlayerPokemon = BattleLogic.Battle.PlayerPokemon.Pokemon.Pokemon.Back;
                        BattleAnimations.PlayerPokemon.Scale = Vector2.Zero;
                        BattleAnimations.PlayerPokemon.LoadContent();
                        BattleAnimations.PlayerPokemon.Tint = Color.Red;

                        BattleAnimations.PlayerPokemonAssets.UnloadContent();
                        BattleAnimations.PlayerPokemonAssets = new PokemonAssets(BattleLogic.Battle.PlayerPokemon.Pokemon, true);
                        BattleAnimations.PlayerPokemonAssets.ScaleEXPBar(BattleAnimations.EXPBar);
                        BattleAnimations.PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
                        BattleAnimations.SetDefaultBattleImagePositions(BattleScreen.TextBox);
                        BattleAnimations.PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, BattleScreen.TextBox.Border.Position.Y - BattleAnimations.PlayerHPBarBackground.SourceRect.Height - 4);
                        BattleAnimations.SetAssetPositions();
                    }

                    if (counter < 100.0f)
                    {
                        counter += counterSpeed;
                        return false;
                    }

                    counter = 0;

                    BattleScreen.TextBox.NextPage = 3;
                    BattleScreen.TextBox.IsTransitioning = true;
                    whiteBackgroundTransitioned = false;
                }
                else
                {
                    BattleAnimations.State = BattleAnimations.BattleState.POKEMON_SEND_OUT;
                    BattleAnimations.Pokeball.LoadContent();
                    resetPokeball();
                }
                return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (draw)
                whiteBackground.Draw(spriteBatch);
        }
    }
}
