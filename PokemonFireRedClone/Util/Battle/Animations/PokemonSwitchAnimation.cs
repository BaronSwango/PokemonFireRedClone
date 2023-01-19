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
            //CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning)
            {
                if (ScreenManager.Instance.BattleScreen.TextBox.Page != 3)
                {
                    if (!WhiteEffect.IsLoaded)
                        WhiteEffect.LoadContent();

                    if (BattleAssets.PlayerPokemon.Tint != Color.Red)
                    {
                        BattleAssets.PlayerPokemon.Tint = new Color(BattleAssets.PlayerPokemon.Tint.R,
                            BattleAssets.PlayerPokemon.Tint.G - 20,
                            BattleAssets.PlayerPokemon.B - 20, 255);
                        WhiteEffect.Alpha += 0.0784f;
                        return false;
                    }
                    BattleAssets.PlayerPokemon.Tint = Color.Red;

                    if (WhiteEffect.Alpha < 1 && !WhiteEffectTransitioned)
                    {
                        WhiteEffect.Alpha += 0.0784f;
                        return false;
                    }
                    WhiteEffectTransitioned = true;

                    if (BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                    {
                        WhiteEffect.Alpha -= 0.05f;

                        if (BattleAssets.PlayerPokemon.Scale.X - 0.05f > 0 && BattleAssets.PlayerPokemon.Scale.Y - 0.05f > 0)
                        {
                            BattleAssets.PlayerPokemon.Scale = new Vector2(BattleAssets.PlayerPokemon.Scale.X - 0.05f,
                                BattleAssets.PlayerPokemon.Scale.Y - 0.05f);
                            BattleAssets.PlayerPokemon.Position = new Vector2(BattleAssets.PlayerPlatform.Position.X + BattleAssets.PlayerPlatform.SourceRect.Width * 0.55f - BattleAssets.PlayerPokemon.SourceRect.Width / 2,
                                BattleAssets.PlayerPlatform.Position.Y + BattleAssets.PlayerPlatform.SourceRect.Height - (int)(BattleAssets.PlayerPokemon.SourceRect.Height * BattleAssets.PlayerPokemon.Scale.Y));
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

                    if (BattleAssets.PlayerPokemon.Scale != Vector2.Zero)
                        BattleAssets.UpdatePlayerPokemon();

                    if (Counter == null)
                        Counter = new Counter(100);

                    //if (Counter < 100.0f)
                    if (!Counter.Finished)
                    {
                        //Counter += CounterSpeed;
                        Counter.Update(gameTime);
                        return false;
                    }

                    //Counter = 0;
                    Counter.Reset();

                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 3;
                    ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                    WhiteEffectTransitioned = false;
                }
                else
                {
                    BattleAssets.State = BattleAssets.BattleState.POKEMON_SEND_OUT;
                    BattleAssets.Animation = new PokemonSendOut();
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
