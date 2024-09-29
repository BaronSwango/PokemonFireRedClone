using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone 
{
    public class PlayerJumpAnimation : IPlayerAnimation
    {

        private readonly Player player;
        private readonly float originY;
        private readonly Image jumpShadow;
        private int spriteSpeed;

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
            jumpShadow.Position = new(player.TrackPos.X - jumpShadow.SourceRect.X / 2, player.TrackPos.Y + 24);
        }

        public void UnloadContent()
        {
            jumpShadow.UnloadContent();
        }

        public bool Animate(GameTime gameTime)
        {
            int speed = (int) (player.MoveSpeed * 1.1 * (float) gameTime.ElapsedGameTime.TotalMilliseconds); 

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
                spriteSpeed = (int) (player.MoveSpeed * 1.1 * (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                player.Sprite.Update(gameTime);
            }
            else if (player.TrackPos.Y >= originY + 64 && player.TrackPos.Y < originY + 96 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 1 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 3)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 0 ? 1 : 3;
                spriteSpeed = (int) (player.MoveSpeed * 2 * (float) gameTime.ElapsedGameTime.TotalMilliseconds); 
                player.Sprite.Update(gameTime);
            }
            else if (player.TrackPos.Y >= originY + 96 && player.TrackPos.Y < originY + 128 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 0 && player.Sprite.SpriteSheetEffect.CurrentFrame.X != 2)
            {
                player.Sprite.SpriteSheetEffect.CurrentFrame.X = player.Sprite.SpriteSheetEffect.CurrentFrame.X == 1 ? 2 : 0;
                spriteSpeed = (int) (player.MoveSpeed * 2 * (float) gameTime.ElapsedGameTime.TotalMilliseconds); 
                player.Sprite.Update(gameTime);
            }

            if (spriteSpeed != speed && player.Sprite.Position.Y + spriteSpeed > player.TrackPos.Y)
            {
                player.Sprite.Position.Y = player.TrackPos.Y;
                spriteSpeed = speed;
            }

            if (player.TrackPos.Y + speed < originY + 128)
            {
                player.TrackPos.Y += speed;
                player.Sprite.Position.Y += spriteSpeed;    
                jumpShadow.Position.Y += speed;
                return false;
            }

            player.TrackPos.Y = originY + 128;
            player.Sprite.Position.Y = originY + 128;
            player.Destination.Y = originY + 128;
            PlayerAnimationManager.Instance.Start(new PlayerSmokeAnimation(player));
            UnloadContent();
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            jumpShadow.Draw(spriteBatch);
        }
    }
}