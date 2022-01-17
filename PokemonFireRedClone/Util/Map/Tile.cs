using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Tile
    {

        Vector2 position;
        Rectangle sourceRect;
        public string State;
        public string ID;

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

        public void LoadContent(Vector2 position, Rectangle sourceRect, string State)
        {
            this.position = position;
            this.sourceRect = sourceRect;
            this.State = State;
        }

        public void UnloadContent()
        {

        }

        //TODO: Play sound when colliding
        //TODO: 
        public void Update(GameTime gameTime, ref Player player)
        {
            if (State == "Solid")
            {
                Rectangle tileRect = new Rectangle((int)Position.X, (int)Position.Y,
                    sourceRect.Width, sourceRect.Height - 20);
                Rectangle playerRect = new Rectangle((int)player.Image.Position.X, (int)player.Image.Position.Y,
                    player.Image.SourceRect.Width, player.Image.SourceRect.Height);

                if (playerRect.Intersects(tileRect))
                {
                    player.Colliding = true;
                    player.Image.SpriteSheetEffect.SwitchFrame = 250;
                    if (player.Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                        player.Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
                    if (player.state == Player.State.MoveLeft)
                        player.Image.Position.X = tileRect.Right;
                    else if (player.state == Player.State.MoveRight)
                        player.Image.Position.X = tileRect.Left - player.Image.SourceRect.Width;
                    else if (player.state == Player.State.MoveUp)
                    {
                        player.Image.Position.Y = tileRect.Bottom;
                        player.Image.SpriteSheetEffect.CurrentFrame.X = 0;
                        ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(ID, ref player);
                    }
                    else
                    {
                        player.Image.Position.Y = tileRect.Top - player.Image.SourceRect.Height;
                        player.Image.SpriteSheetEffect.CurrentFrame.X = 0;
                        ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(ID, ref player);
                    }
                    player.state = Player.State.Idle;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

    }

}
