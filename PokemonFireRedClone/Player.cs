using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class Player
    {
        public Image Image;
        public Vector2 Velocity;

        public float MoveSpeed;
        Vector2 position;
        Vector2 destination;
        State state;

        enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }

        public Player()
        {
            Velocity = Vector2.Zero;
            position = new Vector2(64,64);
            destination = position;
            state = State.Idle;
        }

        public void LoadContent()
        {
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;
            if (Velocity.X == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Down))
                {
                    Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                }
                else if (InputManager.Instance.KeyDown(Keys.Up))
                {
                    Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 3;
                }
                else
                    Velocity.Y = 0;
            }

            if (Velocity.Y == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Right))
                {
                    Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                }
                else if (InputManager.Instance.KeyDown(Keys.Left))
                {
                    Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                }
                else
                    Velocity.X = 0;
            }

            if (Velocity.X == 0 && Velocity.Y == 0)
                Image.IsActive = false;

            Image.Update(gameTime);
            //Rounding vector to prevent sprite sheet bug
            Image.Position += new Vector2((int)Velocity.X, (int)Velocity.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        public void Spawn(Map map)
        {
            if (TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4) != null)
            {
                Vector2 centerTile = new Vector2(TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4).Position.X - 4,
                    TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4).Position.Y - 84);
                Image.Position = centerTile;
            }
        }

    }
}
