using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PlayerSmokeAnimation : IPlayerAnimation
    {
        private readonly Player player;
        private readonly float originY;
        private readonly Image jumpSmoke;
        private readonly Counter counter;
        private bool isPostDraw;

        public PlayerSmokeAnimation(Player player) {
            this.player = player;
            originY = player.Sprite.Position.Y;
            isPostDraw = true;
            jumpSmoke = new Image();
            counter = new(100);
        }

        public void LoadContent()
        {
            jumpSmoke.Path = "Gameplay/AnimationEffects/JumpSmoke";
            jumpSmoke.Effects = "SpriteSheetEffect";
            jumpSmoke.LoadContent();
            jumpSmoke.IsActive = true;
            jumpSmoke.SpriteSheetEffect.AmountOfFrames = new(3, 1);
            jumpSmoke.SpriteSheetEffect.CurrentFrame.X = 0;
            jumpSmoke.SpriteSheetEffect.SwitchManual = true;
            jumpSmoke.Position = new(player.TrackPos.X - jumpSmoke.SourceRect.X / 2, player.TrackPos.Y + 24);
            jumpSmoke.SpriteSheetEffect.SetupSourceRects();
        }

        public void UnloadContent()
        {
            jumpSmoke.UnloadContent();
        }
        public bool Animate(GameTime gameTime)
        {
            if (counter.Finished)
            {
                jumpSmoke.SpriteSheetEffect.CurrentFrame.X += 1;
                counter.Reset(250);
            }

            if (player.Sprite.Position.Y > originY + 8)
            {
                isPostDraw = false;
            }
            else if (!isPostDraw)
            {
                isPostDraw = true;
            }

            counter.Update(gameTime);

            jumpSmoke.Update(gameTime);
            return jumpSmoke.SpriteSheetEffect.CurrentFrame.X == 2 && counter.Finished;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isPostDraw) {
                jumpSmoke.Draw(spriteBatch);
            }
        }

        public bool CanMove() { return true; }

        public void PostDraw(SpriteBatch spriteBatch) {
            if (isPostDraw) 
            {
                jumpSmoke.Draw(spriteBatch);
            }
        }
    }
}