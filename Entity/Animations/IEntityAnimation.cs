using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public interface IEntityAnimation
    {
        void LoadContent();
        void UnloadContent();
        bool Animate(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        bool CanMove() { return false; }
        void PostDraw(SpriteBatch spriteBatch) {}
    }
}