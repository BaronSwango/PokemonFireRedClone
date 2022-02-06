﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    //TODO: Spawn player in room when creating a new game
    //TODO: When creating a new game, don't load in player's save file or the json object

    //TODO: Make PlayerJsonObject accessible anywhere, even start of game, going to have to
    //TODO: Clean up player animation so animation doesn't glitch (has to do with frame counter and move speed per tile)
    

    public class Player
    {
        public Image Image;

        [XmlIgnore]
        public static PlayerJsonObject PlayerJsonObject;
        static JsonManager<PlayerJsonObject> playerLoader;
        public static double ElapsedTime;

        public float MoveSpeed;
        Vector2 destination;
        public Direction direction;
        int waitToMove;
        bool changeDirection;
        public State state;
        bool wasMoving;
        public bool running;
        public bool Colliding;
        public bool CanUpdate;
        [XmlIgnore]
        public List<Pokemon> Pokemon;
        bool isSpawned;
        static bool isLoaded;

        public enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }
        public enum Direction { Left, Right, Down, Up }

        public Player()
        {
            destination = Vector2.Zero;
            state = State.Idle;
            changeDirection = false;
            running = false;
            waitToMove = 0;
            Colliding = false;
            CanUpdate = true;
            direction = Direction.Up;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            if (!isLoaded)
            {
                load();
                isLoaded = true;
            }
            else
            {
                Image.Position = PlayerJsonObject.Position;
                Image.SpriteSheetEffect.CurrentFrame.Y = PlayerJsonObject.Direction;
            }
        }

        public void UnloadContent()
        {
            PlayerJsonObject.Position = Image.Position;
            PlayerJsonObject.Direction = Image.SpriteSheetEffect.CurrentFrame.Y;
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Map map)
        {
            if (CanUpdate)
            {
                Image.IsActive = true;
                direction = Image.SpriteSheetEffect.CurrentFrame.Y > 3 ? (Direction)Image.SpriteSheetEffect.CurrentFrame.Y - 4 : (Direction)Image.SpriteSheetEffect.CurrentFrame.Y;


                // COLLISION DETECTION START
                if (state == State.Idle
                    && direction == Direction.Up && !InputManager.Instance.KeyDown(Keys.A, Keys.S, Keys.D)
                    || (direction == Direction.Down && !InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D))
                    || (direction == Direction.Left && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.D))
                    || (direction == Direction.Right && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.A)))
                {
                    Tile currentTile = TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 2, Image.SourceRect.Height);

                    if (currentTile != null)
                    {
                        if ((TileManager.Instance.UpTile(map, currentTile) != null && TileManager.Instance.UpTile(map, currentTile).State == "Solid" && direction == Direction.Up)
                            || (TileManager.Instance.DownTile(map, currentTile) != null && TileManager.Instance.DownTile(map, currentTile).State == "Solid" && direction == Direction.Down)
                            || (TileManager.Instance.LeftTile(map, currentTile) != null && TileManager.Instance.LeftTile(map, currentTile).State == "Solid" && direction == Direction.Left)
                            || (TileManager.Instance.RightTile(map, currentTile) != null && TileManager.Instance.RightTile(map, currentTile).State == "Solid" && direction == Direction.Right))
                        {
                            if (!Colliding)
                            {
                                if ((TileManager.Instance.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.Instance.UpTile(map, currentTile)) && direction == Direction.Up)
                                || (TileManager.Instance.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.Instance.DownTile(map, currentTile)) && direction == Direction.Down))
                                {
                                    if (direction == Direction.Up)
                                        ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.Instance.UpTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player);
                                    else
                                        ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.Instance.DownTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).player);

                                }
                                if (changeDirection)
                                    changeDirection = false;
                                Colliding = true;

                            }
                        }

                    }
                }
                // COLLISION DETECTION END

                // CHANGES ANIMATION SPEED
                if (Colliding)
                {
                    Image.SpriteSheetEffect.SwitchFrame = 250;
                    if (Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                        Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
                }
                else if (running)
                    Image.SpriteSheetEffect.SwitchFrame = 60;
                else
                    Image.SpriteSheetEffect.SwitchFrame = 130;

                
                if (changeDirection && Colliding)
                    Colliding = false;


                int speed = running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2.2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


                // TILE BASED MOVEMENT START
                switch (state)
                {
                    case State.Idle:
                        destination = Image.Position;
                        running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

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
                            Image.SpriteSheetEffect.CurrentFrame.X = 0;
                            changeDirection = false;
                            waitToMove = 0;
                        }

                        if (InputManager.Instance.KeyDown(Keys.W))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 3 && Image.SpriteSheetEffect.CurrentFrame.Y != 7)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 7 : 3;
                                    changeDirection = true;
                                    break;
                                }


                                destination.Y -= 64;
                                Image.IsActive = true;
                                state = State.MoveUp;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.S))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 2 && Image.SpriteSheetEffect.CurrentFrame.Y != 6)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 6 : 2;
                                    changeDirection = true;
                                    break;
                                }
                                destination.Y += 64;
                                Image.IsActive = true;
                                state = State.MoveDown;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.A))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 0 && Image.SpriteSheetEffect.CurrentFrame.Y != 4)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 4 : 0;
                                    changeDirection = true;
                                    break;
                                }
                                destination.X -= 64;
                                Image.IsActive = true;
                                state = State.MoveLeft;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.D))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 1 && Image.SpriteSheetEffect.CurrentFrame.Y != 5)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 5 : 1;
                                    changeDirection = true;
                                    break;
                                }
                                destination.X += 64;
                                Image.IsActive = true;
                                state = State.MoveRight;
                            }
                        }
                        else
                        {
                            if (Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                                Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
                            Image.IsActive = false;
                        }

                        wasMoving = false;
                        break;
                    case State.MoveUp:

                        Image.SpriteSheetEffect.CurrentFrame.Y = running ? 7 : 3;
                        if (Image.Position.Y - speed < (int)destination.Y)
                        {
                            Image.Position.Y = (int)destination.Y;
                            destination.Y -= 64;
                            running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.W))
                            {
                                state = State.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.Y -= speed;
                        }

                        break;
                    case State.MoveDown:

                        Image.SpriteSheetEffect.CurrentFrame.Y = running ? 6 : 2;

                        if (Image.Position.Y + speed > (int)destination.Y)
                        {
                            Image.Position.Y = (int)destination.Y;
                            destination.Y += 64;
                            running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.S))
                            {
                                state = State.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.Y += speed;
                        }

                        break;
                    case State.MoveLeft:

                        Image.SpriteSheetEffect.CurrentFrame.Y = running ? 4 : 0;

                        if (Image.Position.X - speed < destination.X)
                        {
                            Image.Position.X = (int)destination.X;
                            destination.X -= 64;
                            running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.A))
                            {
                                state = State.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.X -= speed;
                        }
                        break;
                    case State.MoveRight:

                        Image.SpriteSheetEffect.CurrentFrame.Y = running ? 5 : 1;

                        if (Image.Position.X + speed > destination.X)
                        {
                            Image.Position.X = (int)destination.X;
                            destination.X += 64;
                            running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.D))
                            {
                                state = State.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.X += speed;
                        }
                        break;
                    default:
                        break;
                }
                // TILE BASED MOVEMENT END

                Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        
        public void Spawn(Map map)
        {
            if (!isSpawned)
            {
                if (TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8) != null)
                {
                    Vector2 centerTile = new Vector2(TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8).Position.X,
                        TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8).Position.Y - 84);
                    Image.Position = centerTile;
                }
                isSpawned = true;
            }
        }

        void load()
        {
            playerLoader = new JsonManager<PlayerJsonObject>();

            if (!File.Exists("Load/Gameplay/Player.json")) return;

            PlayerJsonObject = playerLoader.Load("Load/Gameplay/Player.json");
            Image.Position = PlayerJsonObject.Position;
            Image.SpriteSheetEffect.CurrentFrame.Y = PlayerJsonObject.Direction;
        }

        public void Save()
        {
            PlayerJsonObject.Position = Image.Position;
            PlayerJsonObject.Direction = Image.SpriteSheetEffect.CurrentFrame.Y > 3 ? Image.SpriteSheetEffect.CurrentFrame.Y - 4 : Image.SpriteSheetEffect.CurrentFrame.Y;
            PlayerJsonObject.Time += ElapsedTime;
            ElapsedTime = 0;
            playerLoader.Save(PlayerJsonObject, "Load/Gameplay/Player.json");
        }

    }
}
