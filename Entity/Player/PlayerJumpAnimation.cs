using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone 
{
    public class PlayerJumpAnimation : IPlayerAnimation
    {

        private Player player;
        private float originY;
        private Image jumpShadow;
        private float spriteSpeed;

        public PlayerJumpAnimation(Player player) 
        {
            this.player = player;
            originY = player.TrackPos.Y;
            jumpShadow = new Image();
            player.Sprite.SpriteSheetEffect.SwitchManual = true;
            spriteSpeed = 0;
        }

        public void LoadContent() 
        {
            jumpShadow.Path = "Gameplay/AnimationEffects/JumpShadow";
            jumpShadow.LoadContent();
            jumpShadow.Position = new(player.TrackPos.X - jumpShadow.SourceRect.X / 2, player.TrackPos.Y + 20);
        }

        public void UnloadContent()
        {
            jumpShadow.UnloadContent();
        }

        public bool Animate(GameTime gameTime)
        {
            float speed = (float) (player.MoveSpeed * gameTime.ElapsedGameTime.TotalMilliseconds); 

            if (player.TrackPos.Y < originY + 32 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 1 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 3)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 0 ? 1 : 3;

                if (player.Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                {
                    player.Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
                }

                player.Sprite.Position.Y -= 12;
                player.Sprite.Update(gameTime);
            }
            else if (player.TrackPos.Y >= originY + 32 && player.TrackPos.Y < originY + 64 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 0 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 2)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 1 ? 2 : 0;
                spriteSpeed = (float) (player.MoveSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);
                player.Sprite.Update(gameTime);
            }
            else if (player.TrackPos.Y >= originY + 64 && player.TrackPos.Y < originY + 96 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 1 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 3)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 0 ? 1 : 3;
                spriteSpeed = (float) (player.MoveSpeed * 1.55 * gameTime.ElapsedGameTime.TotalMilliseconds); 
                player.Sprite.Update(gameTime);
            }
            else if (player.TrackPos.Y >= originY + 96 && player.TrackPos.Y < originY + 128 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 0 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 2)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 1 ? 2 : 0;
                spriteSpeed = (float) (player.MoveSpeed * 1.9 * gameTime.ElapsedGameTime.TotalMilliseconds); 
                player.Sprite.Update(gameTime);
            }

            if (player.TrackPos.Y + speed < originY + 128)
            {
                player.Sprite.Position.Y += spriteSpeed;
                player.TrackPos.Y += speed;
                jumpShadow.Position.Y += speed;
                return false;
            }

            player.TrackPos.Y = originY + 128;
            player.Sprite.Position.Y = originY + 128;
            player.Destination.Y = originY + 128;
            player.Sprite.SpriteSheetEffect.SwitchManual = false;
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            jumpShadow.Draw(spriteBatch);
        }
    }
}