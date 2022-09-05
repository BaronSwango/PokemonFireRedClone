using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Sprite
    {
        public Image Top;
        public Image Bottom;

        public Sprite(Image image)
        {
            Top = new Image
            {
                Path = image.Path,
                Effects = image.Effects
            };

            Bottom = new Image
            {
                Path = image.Path,
                Effects = image.Effects
            };
        }

        public void LoadContent(Vector2 spriteFrames, Entity.EntityDirection direction)
        {
            Top.LoadContent();
            Top.SpriteSheetEffect.IsActive = true;
            Top.SpriteSheetEffect.SwitchManual = true;
            Top.SpriteSheetEffect.Entity = true;
            Top.SpriteSheetEffect.AmountOfFrames = spriteFrames;
            Top.SpriteSheetEffect.CurrentFrame.Y = (int) direction;
            Top.SpriteSheetEffect.SpriteType = "NPCTop";
            Top.SpriteSheetEffect.SetupSourceRects();

            Bottom.LoadContent();
            Bottom.SpriteSheetEffect.IsActive = true;
            Bottom.SpriteSheetEffect.SwitchManual = true;
            Bottom.SpriteSheetEffect.Entity = true;
            Bottom.SpriteSheetEffect.AmountOfFrames = spriteFrames;
            Bottom.SpriteSheetEffect.CurrentFrame.Y = (int) direction;
            Bottom.SpriteSheetEffect.SpriteType = "NPCBottom";
            Bottom.SpriteSheetEffect.SetupSourceRects();
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

        public void SetFrame(int frame)
        {
            Top.SpriteSheetEffect.CurrentFrame.X = frame;
            Bottom.SpriteSheetEffect.CurrentFrame.X = frame;
        }

        public void SetPosition(Vector2 position)
        {
            Top.Position = position;
            Bottom.Position = new Vector2(Top.Position.X, Top.Position.Y + Top.SourceRect.Height);
        }
    

    }
}
