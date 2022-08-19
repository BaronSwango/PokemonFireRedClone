using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonSwitchAnimation : BattleAnimation
    {

        private bool draw;

        public PokemonSwitchAnimation()
        {
            draw = true;
            CreateWhiteEffect();
        }

        public override bool Animate(GameTime gameTime)
        {
            CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                if (ScreenManager.Instance.BattleScreen.TextBox.Page != 3)
                {
                    if (!WhiteEffect.IsLoaded)
                        WhiteEffect.LoadContent();

                    if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint != Color.Red)
                    {
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint = new Color(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint.R,
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint.G - 20,
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.B - 20, 255);
                        WhiteEffect.Alpha += 0.0784f;
                        return false;
                    }
                    ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint = Color.Red;

                    if (WhiteEffect.Alpha < 1 && !WhiteEffectTransitioned)
                    {
                        WhiteEffect.Alpha += 0.0784f;
                        return false;
                    }
                    WhiteEffectTransitioned = true;

                    if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                    {
                        WhiteEffect.Alpha -= 0.05f;

                        if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                        {
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.X - 0.05f,
                                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y - 0.05f);
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Width / 2,
                                ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Height - (int)(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height * ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale.Y));
                        }
                        return false;
                    }

                    if (WhiteEffectTransitioned && WhiteEffect.Alpha - 0.05f > 0)
                    {
                        WhiteEffect.Alpha -= 0.05f;
                        return false;
                    }
                    if (WhiteEffect.Alpha != 0)
                    {
                        WhiteEffect.Alpha = 0;
                        draw = false;
                    }



                    if (ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale != Vector2.Zero)
                    {
                        BattleLogic.Battle.UpdatePlayerPokemon();

                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale = Vector2.Zero;
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position = new Vector2(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.X + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Width / 2,
                            ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.Position.Y + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPlatform.SourceRect.Height - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.SourceRect.Height);
                        ScreenManager.Instance.BattleScreen.BattleAssets.PokeOriginalY = ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Position.Y;
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.UnloadContent();

                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon = BattleLogic.Battle.PlayerPokemon.Pokemon.Pokemon.Back;
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Scale = Vector2.Zero;
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.LoadContent();
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemon.Tint = Color.Red;

                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.UnloadContent();
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets = new PokemonAssets(BattleLogic.Battle.PlayerPokemon.Pokemon, true);
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.ScaleEXPBar(ScreenManager.Instance.BattleScreen.BattleAssets.EXPBar);
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
                        ScreenManager.Instance.BattleScreen.BattleAssets.SetDefaultBattleImagePositions(ScreenManager.Instance.BattleScreen.TextBox);
                        ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X,
                            ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y - ScreenManager.Instance.BattleScreen.BattleAssets.PlayerHPBarBackground.SourceRect.Height - 4);
                        ScreenManager.Instance.BattleScreen.BattleAssets.SetAssetPositions();
                    }

                    if (Counter < 100.0f)
                    {
                        Counter += CounterSpeed;
                        return false;
                    }

                    Counter = 0;

                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 3;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    WhiteEffectTransitioned = false;
                }
                else
                {
                    ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                    ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonSendOut();
                    ResetPokeball();
                }
                return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (draw)
                WhiteEffect.Draw(spriteBatch);
        }
    }
}
