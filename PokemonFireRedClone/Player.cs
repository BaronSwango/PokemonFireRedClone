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
        int waitToMove;
        bool changeDirection;
        public State state;
        State prevState;

        public enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }

        public Player()
        {
            Velocity = Vector2.Zero;
            destination = Vector2.Zero;
            state = State.Idle;
            changeDirection = false;
            waitToMove = 0;
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

            switch (state)
            {
                case State.Idle:
                    destination = Image.Position;

                    // causes a change in direction but no movement unless key is held down more than 4 iterations of the Update method
                    if (changeDirection && prevState == State.Idle)
                    {
                        if (waitToMove < 4)
                        {
                            if (!InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.S, Keys.D))
                                state = State.Idle;
                            waitToMove++;
                            break;
                        }
                        changeDirection = false;
                        waitToMove = 0;
                    }

                    if (InputManager.Instance.KeyDown(Keys.W))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 3)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = 3;
                            changeDirection = true;
                            break;
                        }
                        destination.Y -= 64;
                        state = State.MoveUp;

                    }
                    else if (InputManager.Instance.KeyDown(Keys.S))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 2)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                            changeDirection = true;
                            break;
                        }
                        destination.Y += 64;
                        state = State.MoveDown;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.A))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 0)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                            changeDirection = true;
                            break;
                        }
                        destination.X -= 64;
                        state = State.MoveLeft;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.D))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 1)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                            changeDirection = true;
                            break;
                        }
                        destination.X += 64;
                        state = State.MoveRight;
                    } else
                        Image.IsActive = false;

                    prevState = State.Idle;
                    break;
                case State.MoveUp:

                    // causes a change in direction but no movement unless key is held down more than 4 iterations of the Update method


                    if (Image.Position.Y - (int) (MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds) < (int) destination.Y)
                    {
                        
                        Image.Position.Y = (int)destination.Y;
                        destination.Y -= 64;

                        if (!InputManager.Instance.KeyDown(Keys.W))
                        {
                            state = State.Idle;
                            prevState = State.MoveUp;
                        }
                    }
                    else
                        Image.Position.Y -= (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case State.MoveDown:

                    if (Image.Position.Y + (int) (MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds) > (int) destination.Y)
                    {
                        Image.Position.Y = (int) destination.Y;
                        destination.Y += 64;

                        if (!InputManager.Instance.KeyDown(Keys.S))
                        {
                            state = State.Idle;
                            prevState = State.MoveDown;
                        }
                    }
                    else
                        Image.Position.Y += (int) (MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case State.MoveLeft:

                    if (Image.Position.X - MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds < destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X -= 64;
                        if (!InputManager.Instance.KeyDown(Keys.A))
                        {
                            state = State.Idle;
                            prevState = State.MoveLeft;
                        }
                    }
                    else
                        Image.Position.X -= (int) (MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case State.MoveRight:

                    if (Image.Position.X + MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds > destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X += 64;
                        if (!InputManager.Instance.KeyDown(Keys.D))
                        {
                            state = State.Idle;
                            prevState = State.MoveRight;
                        }
                    }
                    else
                        Image.Position.X += (int) (MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
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

        // spawns in player on a tile
        public void Spawn(Map map)
        {
            if (TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4) != null)
            {
                Vector2 centerTile = new Vector2(TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4).Position.X,
                    TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 4).Position.Y - 84);
                Image.Position = centerTile;
            }
        }

    }
}
