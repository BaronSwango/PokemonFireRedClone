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
        bool wasMoving;
        bool running;

        public enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }
        public enum Direction { Left, Right, Down, Up, RunLeft, RunRight, RunDown, RunUp }

        public Player()
        {
            Velocity = Vector2.Zero;
            destination = Vector2.Zero;
            state = State.Idle;
            changeDirection = false;
            running = false;
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
            running = InputManager.Instance.KeyDown(Keys.LeftShift);
            int speed = running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            Image.SpriteSheetEffect.SwitchFrame = running ? 66 : 125;

            switch (state)
            {
                case State.Idle:
                    destination = Image.Position;

                    // causes a change in direction but no movement unless key is held down more than 4 iterations of the Update method
                    if (changeDirection && !wasMoving)
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
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 3 && Image.SpriteSheetEffect.CurrentFrame.Y != 7)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = running ? 7 : 3;
                            changeDirection = true;
                            break;
                        }
                        destination.Y -= 64;
                        state = State.MoveUp;

                    }
                    else if (InputManager.Instance.KeyDown(Keys.S))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 2 && Image.SpriteSheetEffect.CurrentFrame.Y != 6)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = running ? 6 : 2;
                            changeDirection = true;
                            break;
                        }
                        destination.Y += 64;
                        state = State.MoveDown;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.A))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 0 && Image.SpriteSheetEffect.CurrentFrame.Y != 4)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = running ? 4 : 0;
                            changeDirection = true;
                            break;
                        }
                        destination.X -= 64;
                        state = State.MoveLeft;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.D))
                    {
                        if (Image.SpriteSheetEffect.CurrentFrame.Y != 1 && Image.SpriteSheetEffect.CurrentFrame.Y != 5)
                        {
                            Image.SpriteSheetEffect.CurrentFrame.Y = running ? 5 : 1;
                            changeDirection = true;
                            break;
                        }
                        destination.X += 64;
                        state = State.MoveRight;
                    }
                    else
                    {
                        if (running && Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                            Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
                        Image.IsActive = false;
                    }

                    wasMoving = false;
                    break;
                case State.MoveUp:

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 7 : 3;

                    if (Image.Position.Y - speed < (int) destination.Y)
                    {
                        
                        Image.Position.Y = (int)destination.Y;
                        destination.Y -= 64;

                        if (!InputManager.Instance.KeyDown(Keys.W))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.Y -= speed;
                    break;
                case State.MoveDown:

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 6 : 2;

                    if (Image.Position.Y + speed > (int) destination.Y)
                    {
                        Image.Position.Y = (int) destination.Y;
                        destination.Y += 64;

                        if (!InputManager.Instance.KeyDown(Keys.S))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.Y += speed;
                    break;
                case State.MoveLeft:

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 4 : 0;

                    if (Image.Position.X - speed < destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X -= 64;
                        if (!InputManager.Instance.KeyDown(Keys.A))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.X -= speed;
                    break;
                case State.MoveRight:

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 5 : 1;

                    if (Image.Position.X + speed > destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X += 64;
                        if (!InputManager.Instance.KeyDown(Keys.D))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.X += speed;
                       
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
