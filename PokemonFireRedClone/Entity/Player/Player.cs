using System;
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

        private static GameplayScreen CurrentScreen
        {
            get { return (GameplayScreen)ScreenManager.Instance.CurrentScreen; }
        }

        public Player()
        {
            changeDirection = false;
            Running = false;
            waitToMove = 0;
            Colliding = false;
            CanUpdate = true;
            PreviousTile = Vector2.Zero;
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
            Sprite.SpriteSheetEffect.SwitchManual = true;
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
                    if ((TileManager.UpTile(map, currentTile) != null && (TileManager.UpTile(map, currentTile).State == "Solid" || TileManager.UpTile(map, currentTile).Entity != null) && Direction == EntityDirection.Up)
                        || (TileManager.DownTile(map, currentTile) != null && (TileManager.DownTile(map, currentTile).State == "Solid" || TileManager.DownTile(map, currentTile).Entity != null) && Direction == EntityDirection.Down)
                        || (TileManager.LeftTile(map, currentTile) != null && (TileManager.LeftTile(map, currentTile).State == "Solid" || TileManager.LeftTile(map, currentTile).Entity != null) && Direction == EntityDirection.Left)
                        || (TileManager.RightTile(map, currentTile) != null && (TileManager.RightTile(map, currentTile).State == "Solid" || TileManager.RightTile(map, currentTile).Entity != null) && Direction == EntityDirection.Right))
                    {
                        
                        if (!Colliding && !CurrentScreen.TextBoxManager.IsDisplayed)
                        {
                            if (changeDirection)
                                changeDirection = false;
                            Colliding = true;

                        }

                        if (((TileManager.IsTextBoxTile(CurrentScreen, TileManager.UpTile(map, currentTile)) && Direction == EntityDirection.Up)
                            || (TileManager.IsTextBoxTile(CurrentScreen, TileManager.DownTile(map, currentTile)) && Direction == EntityDirection.Down))
                            && !CurrentScreen.TextBoxManager.Closed)
                        {
                            
                            if (Direction == EntityDirection.Up && !CurrentScreen.TextBoxManager.IsDisplayed)
                                CurrentScreen.TextBoxManager.LoadContent(TileManager.UpTile(map, currentTile).ID, ref map, ref CurrentScreen.Player);
                            else if (Direction == EntityDirection.Down && !CurrentScreen.TextBoxManager.IsDisplayed)
                                CurrentScreen.TextBoxManager.LoadContent(TileManager.DownTile(map, currentTile).ID, ref map, ref CurrentScreen.Player);

                        }
                    } else
                    {
                        Colliding = false;
                        Sprite.SpriteSheetEffect.SwitchManual = true;
                        Sprite.IsActive = false;
                    }

                }
            }

            
            foreach (NPC npc in map.NPCs)
            {
                if (npc.IsMoving && Destination == npc.Destination)
                {
                    Colliding = true;
                    Sprite.SpriteSheetEffect.SwitchManual = false;
                    Sprite.IsActive = true;
                    return;
                }
            }
            

            // COLLISION DETECTION END


            if (!Colliding && CurrentScreen.TextBoxManager.Closed)
                CurrentScreen.TextBoxManager.Closed = false;
            
           //TODO: handle collision with improved player movement

            // CHANGES ANIMATION SPEED
            
            if (Colliding)
            {
                if (Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                    Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
                Sprite.SpriteSheetEffect.SwitchManual = false;
            }

            
            if (changeDirection && Colliding)
            {
                Colliding = false;
                Sprite.SpriteSheetEffect.SwitchManual = true;
                Sprite.IsActive = false;
            }


            int speed = Running ? (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 2.2) : (int)(MoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 1.1);

            if (CanUpdate)
            {
                // TILE BASED MOVEMENT START

                if ((State == MoveState.Right || State == MoveState.Left || State == MoveState.Up || State == MoveState.Down) && Colliding && changeDirection)
                {
                    Colliding = false;
                    Sprite.SpriteSheetEffect.SwitchManual = true;
                    Sprite.IsActive = false;
                }

                switch (State)
                {
                    case MoveState.Idle:
                        Destination = Sprite.Position;
                        PreviousTile = Destination;
                        IsMoving = false;
                        Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                        // causes a change in Direction but no movement unless key is held down more than 4 iterations of the Update method

                        if (changeDirection && !wasMoving)
                        {
                            if (waitToMove < 6)
                            {
                                if (!InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.S, Keys.D))
                                    State = MoveState.Idle;
                                waitToMove++;
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                                break;
                            }
                            Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 0 ? 2 : 0;
                            changeDirection = false;
                            waitToMove = 0;
                        }

                        if (InputManager.Instance.KeyDown(Keys.W))
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 3 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 7)
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                                changeDirection = true;
                                break;
                            }

                            Destination.Y -= 64;
                            
                            if (Colliding)
                                Sprite.IsActive = true;
                            else
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;                             
                                State = MoveState.Up;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.S))
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 2 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 6)
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;
                                changeDirection = true;
                                break;
                            }

                            Destination.Y += 64;
                            
                            if (Colliding)
                                Sprite.IsActive = true;
                            else
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                                State = MoveState.Down;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.A))
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 0 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 4)
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;
                                changeDirection = true;
                                break;
                            }
                            Destination.X -= 64;
                            
                            if (Colliding)
                                Sprite.IsActive = true;
                            else
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                                State = MoveState.Left;
                            }
                        }
                        else if (InputManager.Instance.KeyDown(Keys.D))
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y != 1 && Sprite.SpriteSheetEffect.CurrentFrame.Y != 5)
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;
                                changeDirection = true;
                                break;
                            }
                            Destination.X += 64;
                            
                            if (Colliding)
                                Sprite.IsActive = true;
                            else
                            {
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                                State = MoveState.Right;
                            }
                        }
                        else
                        {
                            if (Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                                Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
                            Sprite.IsActive = false;
                        }

                        if (State != MoveState.Idle)
                            IsMoving = true;

                        wasMoving = false;
                        break;
                    case MoveState.Up:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 7 : 3;
                        if (Sprite.Position.Y - speed < (int)Destination.Y)
                        {
                            Sprite.Position.Y = (int)Destination.Y;
                            PreviousTile = Destination;
                            Destination.Y -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.W))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                                changeDirection = false;
                            }
                            else
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                        }
                        else
                        {
                            Sprite.Position.Y -= speed;

                            if (Math.Abs(Sprite.Position.Y - Destination.Y) < 32 && (Sprite.SpriteSheetEffect.CurrentFrame.X == 1 || Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        break;
                    case MoveState.Down:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 6 : 2;

                        if (Sprite.Position.Y + speed > (int)Destination.Y)
                        {
                            Sprite.Position.Y = (int)Destination.Y;
                            PreviousTile = Destination;
                            Destination.Y += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.S))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                                changeDirection = false;
                            } else
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;
                        }
                        else
                        {
                            Sprite.Position.Y += speed;

                            if (Math.Abs(Sprite.Position.Y - Destination.Y) < 32 && (Sprite.SpriteSheetEffect.CurrentFrame.X == 1 || Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }
                        
                        break;
                    case MoveState.Left:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 4 : 0;

                        if (Sprite.Position.X - speed < Destination.X)
                        {
                            Sprite.Position.X = (int)Destination.X;
                            PreviousTile = Destination;
                            Destination.X -= 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.A))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                                changeDirection = false;
                            } else
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;

                        }
                        else
                        {
                            Sprite.Position.X -= speed;

                            if (Math.Abs(Sprite.Position.X - Destination.X) < 32 && (Sprite.SpriteSheetEffect.CurrentFrame.X == 1 || Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
                        }

                        break;
                    case MoveState.Right:

                        Sprite.SpriteSheetEffect.CurrentFrame.Y = Running ? 5 : 1;

                        if (Sprite.Position.X + speed > Destination.X)
                        {
                            Sprite.Position.X = (int)Destination.X;
                            PreviousTile = Destination;
                            Destination.X += 64;
                            Running = InputManager.Instance.KeyDown(Keys.LeftShift) && !Colliding;

                            if (!InputManager.Instance.KeyDown(Keys.D))
                            {
                                State = MoveState.Idle;
                                wasMoving = true;
                                changeDirection = false;
                            } else
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 1 ? 3 : 1;

                        }
                        else
                        {
                            Sprite.Position.X += speed;

                            if (Math.Abs(Sprite.Position.X - Destination.X) < 32 && (Sprite.SpriteSheetEffect.CurrentFrame.X == 1 || Sprite.SpriteSheetEffect.CurrentFrame.X == 3))
                                Sprite.SpriteSheetEffect.CurrentFrame.X = Sprite.SpriteSheetEffect.CurrentFrame.X > 2 ? 0 : 2;
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
