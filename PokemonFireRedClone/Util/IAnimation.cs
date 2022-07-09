using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public interface IAnimation
    {
        bool Animate(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
