using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public interface ITileAnimation
    {
        void LoadContent();
        void UnloadContent();
        bool Animate(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}