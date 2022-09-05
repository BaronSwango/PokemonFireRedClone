using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class SpriteSheetEffect : ImageEffect
    {
        private bool wasActive;

        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 AmountOfFrames;
        public string SpriteType;
        public bool Entity;
        public bool SwitchManual;

        public int FrameWidth
        {
            get
            {
                if (Image.Texture != null)
                    return Image.Texture.Width / (int) AmountOfFrames.X;
                return 0;
            }
        }

        public int FrameHeight
        {
            get
            {
                if (Image.Texture != null)
                    return Image.Texture.Height / (int)AmountOfFrames.Y;
                return 0;
            }
        }


        public SpriteSheetEffect()
        {
            AmountOfFrames = new Vector2(4, 8);
            CurrentFrame = new Vector2(0, 0);
            SwitchFrame = 130;
            FrameCounter = 0;
        }

        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Image.IsActive)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    CurrentFrame.X++;

                    if (CurrentFrame.X * FrameWidth >= Image.Texture.Width)
                        CurrentFrame.X = 0;

                    if (!wasActive)
                        wasActive = true;
                }
            }
            else if (!SwitchManual)
            {
                if (Entity)
                {
                    if (CurrentFrame.Y > 3)
                        CurrentFrame.Y -= 4;
                    CurrentFrame.X = CurrentFrame.X == 1 || CurrentFrame.X == 2 ? 2 : 0;

                    if (wasActive)
                    {
                        FrameCounter = 0;
                        wasActive = false;
                    }
                }
            }

            SetupSourceRects();
           
        }

        public void SetupSourceRects()
        {
            if (SpriteType == "NPCTop")
                Image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                    (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight / 2);
            else if (SpriteType == "NPCBottom")
            {
                Image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                    (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight / 2);
                Image.SourceRect.Y += Image.SourceRect.Height;
            }
            else
                Image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                        (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

    }
}
