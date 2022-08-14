using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Sprite
    {
        public Image Top;
        public Image Bottom;

        public Sprite(Image image)
        {
            Top = new Image();
            Top.Path = image.Path;
            Top.Effects = image.Effects;

            Bottom = new Image();
            Bottom.Path = image.Path;
            Bottom.Effects = image.Effects;
        }

        public void LoadContent()
        {
            Top.LoadContent();
            Top.SpriteSheetEffect.IsActive = true;
            Top.SpriteSheetEffect.Entity = true;
            Top.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 4);
            Top.SpriteSheetEffect.CurrentFrame.Y = 2;
            Top.SpriteSheetEffect.SpriteType = "NPCTop";

            Bottom.LoadContent();
            Bottom.SpriteSheetEffect.IsActive = true;
            Bottom.SpriteSheetEffect.Entity = true;
            Bottom.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 4);
            Bottom.SpriteSheetEffect.CurrentFrame.Y = 2;
            Bottom.SpriteSheetEffect.SpriteType = "NPCBottom";
        }


        public void UnloadContent()
        {
            Bottom.UnloadContent();
            Top.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Top.Update(gameTime);
            
            Bottom.Update(gameTime);
        }

        public void SetDirection(int direction)
        {
            Top.SpriteSheetEffect.CurrentFrame.Y = direction;
            Bottom.SpriteSheetEffect.CurrentFrame.Y = direction;
        }

        public void SetPosition(Vector2 position)
        {
            Top.Position = position;
            Bottom.Position = new Vector2(Top.Position.X, Top.Position.Y + (Top.SourceRect.Height / 8));
        }

    }
}
