using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Camera
    {

        public Matrix Transform { get; private set; }

        public void Follow(Player target)
        {
            var position = Matrix.CreateTranslation(
                (int) (-target.Sprite.Position.X - (target.Sprite.SourceRect.Width / 2)),
                (int) (-target.Sprite.Position.Y - (target.Sprite.SourceRect.Height / 2)),
                0); 
            var offset = Matrix.CreateTranslation(
                    (int) ScreenManager.Instance.Dimensions.X / 2,
                    (int) ScreenManager.Instance.Dimensions.Y / 2,
                    0);

            Transform = position * offset;


        }

    }
}
