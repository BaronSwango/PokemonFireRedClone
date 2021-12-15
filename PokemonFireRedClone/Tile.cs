using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Tile
    {

        Vector2 position;
        Rectangle sourceRect;

        public Rectangle SourceRect
        {
            get { return sourceRect; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Tile()
        {
        }

        public void LoadContent(Vector2 position, Rectangle sourceRect)
        {
            this.position = position;
            this.sourceRect = sourceRect;
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }

}
