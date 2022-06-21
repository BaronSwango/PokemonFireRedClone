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
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, BattleAssets.Background.SourceRect.Width, BattleAssets.Background.SourceRect.Height)
            };
            //whiteBackground.LoadContent();
            Color[] data = new Color[BattleAssets.Background.SourceRect.Width * BattleAssets.Background.SourceRect.Height];
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

                    if (BattleAssets.PlayerPokemon.Tint != Color.Red)
                    {
                        BattleAssets.PlayerPokemon.Tint = new Color(BattleAssets.PlayerPokemon.Tint.R, BattleAssets.PlayerPokemon.Tint.G - 20, BattleAssets.PlayerPokemon.B - 20, 255);
                        whiteBackground.Alpha += 0.0784f;
                        return false;
                    }
                    BattleAssets.PlayerPokemon.Tint = Color.Red;

                    if (whiteBackground.Alpha < 1 && !whiteBackgroundTransitioned)
                    {
                        whiteBackground.Alpha += 0.0784f;
                        return false;
                    }
                    whiteBackgroundTransitioned = true;

                    if (BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                    {
                        whiteBackground.Alpha -= 0.05f;

                        if (BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                        {
                            BattleAssets.PlayerPokemon.Scale = new Vector2(BattleAssets.PlayerPokemon.Scale.X - 0.05f, BattleAssets.PlayerPokemon.Scale.Y - 0.05f);
                            BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - (int)(BattleAssets.PlayerPokemon.SourceRect.Height * BattleAssets.PlayerPokemon.Scale.Y));
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



                    if (BattleAssets.PlayerPokemon.Scale != Vector2.Zero)
                    {
                        BattleLogic.Battle.UpdatePlayerPokemon();

                        BattleAssets.PlayerPokemon.Scale = Vector2.Zero;
                        BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2, BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - BattleAssets.PlayerPokemon.SourceRect.Height);
                        BattleAssets.PokeOriginalY = BattleAssets.PlayerPokemon.Position.Y;
                        BattleAssets.PlayerPokemon.UnloadContent();

                        BattleAssets.PlayerPokemon = BattleLogic.Battle.PlayerPokemon.Pokemon.Pokemon.Back;
                        BattleAssets.PlayerPokemon.Scale = Vector2.Zero;
                        BattleAssets.PlayerPokemon.LoadContent();
                        BattleAssets.PlayerPokemon.Tint = Color.Red;

                        BattleAssets.PlayerPokemonAssets.UnloadContent();
                        BattleAssets.PlayerPokemonAssets = new PokemonAssets(BattleLogic.Battle.PlayerPokemon.Pokemon, true);
                        BattleAssets.PlayerPokemonAssets.ScaleEXPBar(BattleAssets.EXPBar);
                        BattleAssets.PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
                        BattleAssets.SetDefaultBattleImagePositions(BattleScreen.TextBox);
                        BattleAssets.PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, BattleScreen.TextBox.Border.Position.Y - BattleAssets.PlayerHPBarBackground.SourceRect.Height - 4);
                        BattleAssets.SetAssetPositions();
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
                    BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                    BattleAssets.Animation = new PokemonSendOut();
                    BattleAssets.Pokeball.LoadContent();
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
