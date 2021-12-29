using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    //TODO: Spawn player in room when creating a new game
    //TODO: When creating a new game, don't load in player's save file or the json object

    //TODO: Make PlayerJsonObject accessible anywhere, even start of game, going to have to
    // make a JsonObjectManager (singleton) in order to load the data to the main menu
    

    public class Player
    {
        public Image Image;

        public PlayerJsonObject PlayerJsonObject;
        JsonManager<PlayerJsonObject> playerLoader;

        public float MoveSpeed;
        Vector2 destination;
        int waitToMove;
        bool changeDirection;
        public State state;
        bool wasMoving;
        public bool running;
        public bool Colliding;

        public enum State { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }
        public enum Direction { Left, Right, Down, Up, RunLeft, RunRight, RunDown, RunUp }

        public Player()
        {
            destination = Vector2.Zero;
            state = State.Idle;
            changeDirection = false;
            running = false;
            waitToMove = 0;
            Colliding = false;
            PlayerJsonObject = new PlayerJsonObject();
        }

        public void LoadContent()
        {
            Image.LoadContent();
            load();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

            if (InputManager.Instance.KeyDown(Keys.Tab))
                save();


            if (changeDirection && Colliding)
                Colliding = false;

            if (Colliding)
            {
                Image.SpriteSheetEffect.SwitchFrame = 250;
                if (Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                    Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
            }
            else if (running)
                Image.SpriteSheetEffect.SwitchFrame = 62;
            else
                Image.SpriteSheetEffect.SwitchFrame = 125;

            int speed = running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2.2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

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
                        if (Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                            Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
                        Image.IsActive = false;
                    }

                    wasMoving = false;
                    break;
                case State.MoveUp:

                    if (Image.Position.Y - speed < (int) destination.Y)
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
                        Image.Position.Y -= speed;

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 7 : 3;
                    break;
                case State.MoveDown:

                    if (Image.Position.Y + speed > (int) destination.Y)
                    {
                        Image.Position.Y = (int) destination.Y;
                        destination.Y += 64;
                        running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        if (!InputManager.Instance.KeyDown(Keys.S))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.Y += speed;

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 6 : 2;
                    break;
                case State.MoveLeft:

                    if (Image.Position.X - speed < destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X -= 64;
                        running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        if (!InputManager.Instance.KeyDown(Keys.A))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.X -= speed;

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 4 : 0;
                    break;
                case State.MoveRight:

                    if (Image.Position.X + speed > destination.X)
                    {
                        Image.Position.X = (int) destination.X;
                        destination.X += 64;
                        running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        if (!InputManager.Instance.KeyDown(Keys.D))
                        {
                            state = State.Idle;
                            wasMoving = true;
                        }
                    }
                    else
                        Image.Position.X += speed;

                    Image.SpriteSheetEffect.CurrentFrame.Y = running ? 5 : 1;
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
            if (TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 8) != null)
            {
                Vector2 centerTile = new Vector2(TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 8).Position.X,
                    TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Height / 8).Position.Y - 84);
                Image.Position = centerTile;
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

        void save()
        {
            PlayerJsonObject.Position = Image.Position;
            PlayerJsonObject.Direction = Image.SpriteSheetEffect.CurrentFrame.Y > 3 ? Image.SpriteSheetEffect.CurrentFrame.Y - 4 : Image.SpriteSheetEffect.CurrentFrame.Y;
            PlayerJsonObject.Name = "BillyBOB";
            playerLoader.Save(PlayerJsonObject, "Load/Gameplay/Player.json");
        }

    }
}
