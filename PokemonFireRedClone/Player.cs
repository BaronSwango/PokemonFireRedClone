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

        private int waitToMove;
        private bool wasMoving;
        private bool changeDirection;
        private bool isSpawned;
        private Vector2 destination;
        private static bool isLoaded;
        private static JsonManager<PlayerJsonObject> playerLoader;

        public Image Image;

        [XmlIgnore]
        public static PlayerJsonObject PlayerJsonObject;
        public static double ElapsedTime;

        public float MoveSpeed;
        public PlayerDirection Direction;
        public PlayerState State;
        public bool Running;
        public bool Colliding;
        public bool CanUpdate;
        [XmlIgnore]
        public List<CustomPokemon> Pokemon;

        public enum PlayerState { Idle, MoveRight, MoveLeft, MoveUp, MoveDown }
        public enum PlayerDirection { Left, Right, Down, Up }

        public Player()
        {
            destination = Vector2.Zero;
            State = PlayerState.Idle;
            changeDirection = false;
            Running = false;
            waitToMove = 0;
            Colliding = false;
            CanUpdate = true;
            Direction = PlayerDirection.Up;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.SpriteSheetEffect.Player = true;
            if (!isLoaded)
            {
                Load();
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
            Direction = Image.SpriteSheetEffect.CurrentFrame.Y > 3 ? (PlayerDirection)Image.SpriteSheetEffect.CurrentFrame.Y - 4 : (PlayerDirection)Image.SpriteSheetEffect.CurrentFrame.Y;

            // COLLISION DETECTION START
            if (State == PlayerState.Idle
                && Direction == PlayerDirection.Up && !InputManager.Instance.KeyDown(Keys.A, Keys.S, Keys.D)
                || (Direction == PlayerDirection.Down && !InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D))
                || (Direction == PlayerDirection.Left && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.D))
                || (Direction == PlayerDirection.Right && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.A)))
            {
                Tile currentTile = TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 2, Image.SourceRect.Height);

                if (currentTile != null)
                {
                    if ((TileManager.Instance.UpTile(map, currentTile) != null && TileManager.Instance.UpTile(map, currentTile).State == "Solid" && Direction == PlayerDirection.Up)
                        || (TileManager.Instance.DownTile(map, currentTile) != null && TileManager.Instance.DownTile(map, currentTile).State == "Solid" && Direction == PlayerDirection.Down)
                        || (TileManager.Instance.LeftTile(map, currentTile) != null && TileManager.Instance.LeftTile(map, currentTile).State == "Solid" && Direction == PlayerDirection.Left)
                        || (TileManager.Instance.RightTile(map, currentTile) != null && TileManager.Instance.RightTile(map, currentTile).State == "Solid" && Direction == PlayerDirection.Right))
                    {
                        if (!Colliding)
                        {
                            if (changeDirection)
                                changeDirection = false;
                            Colliding = true;

                        } 
                        if (((TileManager.Instance.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.Instance.UpTile(map, currentTile)) && Direction == PlayerDirection.Up)
                            || (TileManager.Instance.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.Instance.DownTile(map, currentTile)) && Direction == PlayerDirection.Down))
                            && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.Closed)
                        {
                            if (Direction == PlayerDirection.Up && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.IsDisplayed)
                                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.Instance.UpTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player);
                            else if (Direction == PlayerDirection.Down && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.IsDisplayed)
                                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.Instance.DownTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player);

                        }
                    }

                }
            }
            // COLLISION DETECTION END

           if (!Colliding && ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.Closed)
                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.Closed = false;

            // CHANGES ANIMATION SPEED
            if (Colliding)
            {
                Image.SpriteSheetEffect.SwitchFrame = 250;
                if (Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                    Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
            }
            else if (Running)
                Image.SpriteSheetEffect.SwitchFrame = 60;
            else
                Image.SpriteSheetEffect.SwitchFrame = 130;

                
            if (changeDirection && Colliding)
                Colliding = false;


            int speed = Running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2.2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (CanUpdate)
            {
                Image.IsActive = true;
                // TILE BASED MOVEMENT START
                switch (State)
                {
                    case PlayerState.Idle:
                        destination = Image.Position;
                        Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        // causes a change in Direction but no movement unless key is held down more than 4 iterations of the Update method

                        if (changeDirection && !wasMoving)
                        {
                            if (waitToMove < 4)
                            {
                                if (!InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.S, Keys.D))
                                    State = PlayerState.Idle;
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
                                    Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                                    changeDirection = true;
                                    break;
                                }


                                destination.Y -= 64;
                                Image.IsActive = true;
                                State = PlayerState.MoveUp;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.S))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 2 && Image.SpriteSheetEffect.CurrentFrame.Y != 6)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;
                                    changeDirection = true;
                                    break;
                                }
                                destination.Y += 64;
                                Image.IsActive = true;
                                State = PlayerState.MoveDown;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.A))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 0 && Image.SpriteSheetEffect.CurrentFrame.Y != 4)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;
                                    changeDirection = true;
                                    break;
                                }
                                destination.X -= 64;
                                Image.IsActive = true;
                                State = PlayerState.MoveLeft;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.D))
                        {
                            if (Image.IsActive)
                            {
                                if (Image.SpriteSheetEffect.CurrentFrame.Y != 1 && Image.SpriteSheetEffect.CurrentFrame.Y != 5)
                                {
                                    Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;
                                    changeDirection = true;
                                    break;
                                }
                                destination.X += 64;
                                Image.IsActive = true;
                                State = PlayerState.MoveRight;
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
                    case PlayerState.MoveUp:

                        Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                        if (Image.Position.Y - speed < (int)destination.Y)
                        {
                            Image.Position.Y = (int)destination.Y;
                            destination.Y -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.W))
                            {
                                State = PlayerState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.Y -= speed;
                        }

                        break;
                    case PlayerState.MoveDown:

                        Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;

                        if (Image.Position.Y + speed > (int)destination.Y)
                        {
                            Image.Position.Y = (int)destination.Y;
                            destination.Y += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.S))
                            {
                                State = PlayerState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.Y += speed;
                        }

                        break;
                    case PlayerState.MoveLeft:

                        Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;

                        if (Image.Position.X - speed < destination.X)
                        {
                            Image.Position.X = (int)destination.X;
                            destination.X -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.A))
                            {
                                State = PlayerState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Image.Position.X -= speed;
                        }
                        break;
                    case PlayerState.MoveRight:

                        Image.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;

                        if (Image.Position.X + speed > destination.X)
                        {
                            Image.Position.X = (int)destination.X;
                            destination.X += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.D))
                            {
                                State = PlayerState.Idle;
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

            }

            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        private void Load()
        {

            playerLoader = new JsonManager<PlayerJsonObject>();

            if (!File.Exists("Load/Gameplay/Player.json")) return;


            PlayerJsonObject = playerLoader.Load("Load/Gameplay/Player.json");

            foreach (CustomPokemon pokemon in PlayerJsonObject.PokemonInBag)
            {
                pokemon.Stats = PokemonManager.Instance.GenerateStatList(pokemon);
                pokemon.CurrentHP = pokemon.Stats.HP;
            }

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

        public void Spawn(Map map)
        {
            if (!isSpawned)
            {
                if (TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8) != null)
                {
                    Vector2 centerTile = new(TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8).Position.X,
                        TileManager.Instance.GetCurrentTile(map, Image, Image.SourceRect.Width / 8, Image.SourceRect.Height / 8).Position.Y - 84);
                    Image.Position = centerTile;
                }
                isSpawned = true;
            }
        }

        public void ResetPosition()
        {
            Image.Position = PlayerJsonObject.Position;
            Image.SpriteSheetEffect.CurrentFrame.Y = PlayerJsonObject.Direction;
        }

    }
}
