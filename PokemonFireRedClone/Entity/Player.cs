using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{

    //TODO: Spawn player in room when creating a new game
    //TODO: When creating a new game, don't load in player's save file or the json object

    //TODO: Make PlayerJsonObject accessible anywhere, even start of game, going to have to
    //TODO: Clean up player animation so animation doesn't glitch (has to do with frame counter and move speed per tile)
    

    public class Player : Entity
    {

        private int waitToMove;
        private bool wasMoving;
        private bool changeDirection;
        private static bool isLoaded;
        private static JsonManager<PlayerJsonObject> playerLoader;

        [XmlIgnore]
        public static PlayerJsonObject PlayerJsonObject;
        public static double ElapsedTime;
        public bool Running;
        public bool CanUpdate;
        [XmlIgnore]
        public List<CustomPokemon> Pokemon;

        public Player()
        {
            changeDirection = false;
            Running = false;
            waitToMove = 0;
            Colliding = false;
            CanUpdate = true;
        }

        public override void LoadContent()
        {
            if (!isLoaded)
            {
                Load();
                isLoaded = true;
            } else
            {
                SpawnLocation = PlayerJsonObject.Position;
                Direction = (EntityDirection)PlayerJsonObject.Direction;
            }
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            PlayerJsonObject.Position = Sprite.Position;
            PlayerJsonObject.Direction = Sprite.SpriteSheetEffect.CurrentFrame.Y;
            Sprite.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Map map)
        {
            Direction = Sprite.SpriteSheetEffect.CurrentFrame.Y > 3 ? (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y - 4 : (EntityDirection)Sprite.SpriteSheetEffect.CurrentFrame.Y;

            // COLLISION DETECTION START
            if (State == MoveState.Idle
                && Direction == EntityDirection.Up && !InputManager.Instance.KeyDown(Keys.A, Keys.S, Keys.D)
                || (Direction == EntityDirection.Down && !InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D))
                || (Direction == EntityDirection.Left && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.D))
                || (Direction == EntityDirection.Right && !InputManager.Instance.KeyDown(Keys.W, Keys.S, Keys.A)))
            {
                Tile currentTile = TileManager.GetCurrentTile(map, Sprite, Sprite.SourceRect.Width / 2, Sprite.SourceRect.Height);

                if (currentTile != null)
                {
                    if ((TileManager.UpTile(map, currentTile) != null && TileManager.UpTile(map, currentTile).State == "Solid" && Direction == EntityDirection.Up)
                        || (TileManager.DownTile(map, currentTile) != null && TileManager.DownTile(map, currentTile).State == "Solid" && Direction == EntityDirection.Down)
                        || (TileManager.LeftTile(map, currentTile) != null && TileManager.LeftTile(map, currentTile).State == "Solid" && Direction == EntityDirection.Left)
                        || (TileManager.RightTile(map, currentTile) != null && TileManager.RightTile(map, currentTile).State == "Solid" && Direction == EntityDirection.Right))
                    {
                        if (!Colliding)
                        {
                            if (changeDirection)
                                changeDirection = false;
                            Colliding = true;

                        } 
                        if (((TileManager.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.UpTile(map, currentTile)) && Direction == EntityDirection.Up)
                            || (TileManager.IsTextBoxTile((GameplayScreen)ScreenManager.Instance.CurrentScreen, TileManager.DownTile(map, currentTile)) && Direction == EntityDirection.Down))
                            && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.Closed)
                        {
                            if (Direction == EntityDirection.Up && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.IsDisplayed)
                                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.UpTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player);
                            else if (Direction == EntityDirection.Down && !((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.IsDisplayed)
                                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).TextBoxManager.LoadContent(TileManager.DownTile(map, currentTile).ID, ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player);

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
                Sprite.SpriteSheetEffect.SwitchFrame = 250;
                if (Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                    Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
            }
            else if (Running)
                Sprite.SpriteSheetEffect.SwitchFrame = 60;
            else
                Sprite.SpriteSheetEffect.SwitchFrame = 130;

                
            if (changeDirection && Colliding)
                Colliding = false;


            int speed = Running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2.2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (CanUpdate)
            {
                Sprite.IsActive = true;
                // TILE BASED MOVEMENT START
                switch (State)
                {
                    case MoveState.Idle:
                        Destination = Sprite.Position;
                        Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        // causes a change in Direction but no movement unless key is held down more than 4 iterations of the Update method

                        if (changeDirection && !wasMoving)
                        {
                            if (waitToMove < 4)
                            {
                                if (!InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.S, Keys.D))
                                    State = MoveState.Idle;
                                waitToMove++;
                                break;
                            }
                            Sprite.SpriteSheetEffect.CurrentFrame.X = 0;
                            changeDirection = false;
                            waitToMove = 0;
                        }

                        if (InputManager.Instance.KeyDown(Keys.W))
                        {
                            if (Sprite.IsActive)
                            {
                                if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 3 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 7)
                                {
                                    Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                                    changeDirection = true;
                                    break;
                                }


                                Destination.Y -= 64;
                                Sprite.IsActive = true;
                                State = MoveState.Up;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.S))
                        {
                            if (Sprite.IsActive)
                            {
                                if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 2 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 6)
                                {
                                    Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;
                                    changeDirection = true;
                                    break;
                                }
                                Destination.Y += 64;
                                Sprite.IsActive = true;
                                State = MoveState.Down;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.A))
                        {
                            if (Sprite.IsActive)
                            {
                                if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 0 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 4)
                                {
                                    Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;
                                    changeDirection = true;
                                    break;
                                }
                                Destination.X -= 64;
                                Sprite.IsActive = true;
                                State = MoveState.Left;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.D))
                        {
                            if (Sprite.IsActive)
                            {
                                if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 1 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 5)
                                {
                                    Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;
                                    changeDirection = true;
                                    break;
                                }
                                Destination.X += 64;
                                Sprite.IsActive = true;
                                State = MoveState.Right;
                            }
                        }
                        else
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                                Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
                            Sprite.IsActive = false;
                        }

                        wasMoving = false;
                        break;
                    case MoveState.Up:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                        if (Sprite.Position.Y - speed < (int)Destination.Y)
                        {
                            Sprite.Position.Y = (int)Destination.Y;
                            Destination.Y -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.W))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                            Sprite.Position.Y -= speed;

                        break;
                    case MoveState.Down:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;

                        if (Sprite.Position.Y + speed > (int)Destination.Y)
                        {
                            Sprite.Position.Y = (int)Destination.Y;
                            Destination.Y += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.S))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Sprite.Position.Y += speed;
                        }

                        break;
                    case MoveState.Left:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;

                        if (Sprite.Position.X - speed < Destination.X)
                        {
                            Sprite.Position.X = (int)Destination.X;
                            Destination.X -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.A))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Sprite.Position.X -= speed;
                        }
                        break;
                    case MoveState.Right:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;

                        if (Sprite.Position.X + speed > Destination.X)
                        {
                            Sprite.Position.X = (int)Destination.X;
                            Destination.X += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.D))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                            }
                        }
                        else
                        {
                            Sprite.Position.X += speed;
                        }
                        break;
                    default:
                        break;
                }
                // TILE BASED MOVEMENT END

            }

            Sprite.Update(gameTime);
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

            SpawnLocation = PlayerJsonObject.Position;
            Direction = (EntityDirection) PlayerJsonObject.Direction;
        }

        public void Save()
        {
            PlayerJsonObject.Position = Sprite.Position;
            PlayerJsonObject.Direction = Sprite.SpriteSheetEffect.CurrentFrame.Y > 3 ? Sprite.SpriteSheetEffect.CurrentFrame.Y - 4 : Sprite.SpriteSheetEffect.CurrentFrame.Y;
            PlayerJsonObject.Time += ElapsedTime;
            ElapsedTime = 0;
            playerLoader.Save(PlayerJsonObject, "Load/Gameplay/Player.json");
        }

        public void ResetPosition()
        {
            Sprite.Position = PlayerJsonObject.Position;
            Sprite.SpriteSheetEffect.CurrentFrame.Y = PlayerJsonObject.Direction;
        }

    }
}
