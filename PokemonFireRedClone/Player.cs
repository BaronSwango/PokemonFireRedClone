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
        Vector2 destination;
        public State state;

        public enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }

        public Player()
        {
            Velocity = Vector2.Zero;
            destination = Vector2.Zero;
            state = State.Idle;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            destination = Image.Position;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            switch (state)
            {
                case State.Idle:
                    destination = Image.Position;
                    if (InputManager.Instance.KeyDown(Keys.W) || InputManager.Instance.KeyReleased(Keys.W))
                    {
                        if (InputManager.Instance.KeyDown(Keys.W)) { 
                            destination.Y -= 64;
                            state = State.MoveUp;
                        }
                        Image.SpriteSheetEffect.CurrentFrame.Y = 3;

                    }
                    else if (InputManager.Instance.KeyDown(Keys.S) || InputManager.Instance.KeyReleased(Keys.S))
                    {
                        if (InputManager.Instance.KeyDown(Keys.S) && !InputManager.Instance.KeyReleased(Keys.S))
                        {
                            destination.Y += 64;
                            state = State.MoveDown;
                        }
                        Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.A) || InputManager.Instance.KeyReleased(Keys.A))
                    {
                            if (InputManager.Instance.KeyDown(Keys.A))
                            {
                                destination.X -= 64;
                                state = State.MoveLeft;
                            }
                        Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.D) || InputManager.Instance.KeyReleased(Keys.D))
                    {
                        if (InputManager.Instance.KeyDown(Keys.D))
                        {
                            destination.X += 64;
                            state = State.MoveRight;
                        }
                        Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                    } else
                    {
                        Image.IsActive = false;
                        destination = Image.Position;
                    }


                    break;
                case State.MoveUp:
                    if (Image.Position.Y - MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds < destination.Y)
                    {
                        Image.Position.Y = (int) destination.Y;
                        destination.Y -= 64;

                        if (!InputManager.Instance.KeyDown(Keys.W))
                        {
                            state = State.Idle;
                        }
                    }
                    else
                        Image.Position.Y -= MoveSpeed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                    break;
                case State.MoveDown:
                    if (Image.Position.Y + MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds > destination.Y)
                    {
                        Image.Position.Y = (int) destination.Y;
                        destination.Y += 64;

                        if (!InputManager.Instance.KeyDown(Keys.S))
                        {
                            state = State.Idle;
                        }
                    }
                    else
                        Image.Position.Y += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    break;
                case State.MoveLeft:
                    if (Image.Position.X - MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds < destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X -= 64;
                        if (!InputManager.Instance.KeyDown(Keys.A))
                        {
                            state = State.Idle;
                        }
                    }
                    else
                        Image.Position.X -= MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    break;
                case State.MoveRight:

                    if (Image.Position.X + MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds > destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X += 64;
                        if (!InputManager.Instance.KeyDown(Keys.D))
                        {
                            state = State.Idle;
                        }
                    }
                    else
                        Image.Position.X += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    break;
                default:
                    break;
            }
            Image.Update(gameTime);

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
