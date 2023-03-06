using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class AnimatedTile : Tile
    {
        private int totalFrames;
        private readonly Counter counter;

        public AnimatedTile()
        {
            counter = new Counter(250);
        }

        public void LoadContent(int totalFrames)
        {
            this.totalFrames = totalFrames;
        }

        public void Animate(GameTime gameTime, Vector2 tileDimensions)
        {
            if (!counter.Finished)
            {
                counter.Update(gameTime);
                return;
            }

            counter.Reset();

            string str = ID.Replace("[", string.Empty).Replace("]", string.Empty);
            int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
            int value2 = int.Parse(str[(str.IndexOf(':') + 1)..]);

            value1++;

            if (value1 == totalFrames)
                value1 = 0;

            ID = "[" + value1 + ":" + value2 + "]";
            LoadContent(Position, new Rectangle(
                                value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), State);

        }

    }
}
