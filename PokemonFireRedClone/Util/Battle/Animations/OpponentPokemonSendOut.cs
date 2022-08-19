using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class OpponentPokemonSendOut : BattleAnimation
    {
        public override bool Animate(GameTime gameTime)
        {
            CounterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Counter < 1000)
            {
                Counter += CounterSpeed;
                return false;
            }



            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
