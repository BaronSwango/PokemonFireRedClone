﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class GameplayScreen : GameScreen
    {
        private static bool menuWasLoaded;
        private readonly XmlManager<Map> mapLoader;

        public Map Map;
        public Player Player;
        public readonly AreaManager AreaManager;
        public MenuManager MenuManager;
        public TextBoxManager TextBoxManager;
        public DoorManager DoorManager;
        public Camera Camera { get; private set; }

        public GameplayScreen()
        {
            MenuManager = new("MainMenu");
            TextBoxManager = new();
            XmlManager<Player> playerLoader = new();
            XmlManager<DoorManager> doorLoader = new();
            mapLoader = new();
            Player = playerLoader.Load("Load/Gameplay/Player.xml");
            DoorManager = doorLoader.Load("Load/Gameplay/DoorManager.xml");
            AreaManager = new();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Player.LoadContent();
            Map = mapLoader.Load($"Load/Gameplay/Map/{Player.PlayerJsonObject.MapName}.xml");
            Map.LoadContent();
            NPCStateManager.Instance.LoadNPCStates(Map.NPCs);
            TextBoxManager.LoadXML();
            Player.Spawn(Map);

            foreach (NPC npc in Map.NPCs)
            {
                npc.Spawn(Map);
            }
            
            Camera = new Camera();

            if (Map.Inside)
            {
                Player.PlayerJsonObject.CurrentArea = Map.Areas[0];
                Player.PlayerJsonObject.AreaName = Map.Areas[0].Name;
            }
            else
            {
                AreaManager.LoadContent(Map.Areas, Player);
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Player.UnloadContent();
            NPCStateManager.Instance.SaveNPCStates(Map.NPCs);
            Map.UnloadContent();

            menuWasLoaded = MenuManager.IsLoaded;

            if (MenuManager.IsLoaded)
            {
                MenuManager.UnloadContent();
            }

            AreaManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            //float speed = (float)(30 * gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);

            // print player's current tile to console
            if (InputManager.Instance.KeyPressed(Keys.M))
            {
                Tile currentTile = TileManager.GetCurrentTile(Map, Player.Sprite, Player.Sprite.SourceRect.Width / 2, Player.Sprite.SourceRect.Height, 0);

                if (currentTile != null)
                {
                    Console.WriteLine($"({currentTile.Position.X}, {currentTile.Position.Y})");
                }
            }

            if (MenuManager.WasLoaded)
            {
                MenuManager.WasLoaded = false;
            }

            if (menuWasLoaded)
            {
                MenuManager.MenuName = "MainMenu";
                MenuManager.LoadContent("Load/Menus/MainMenu.xml");
                Player.ResetPosition();
                menuWasLoaded = false;
            }
            else
            {
                if (InputManager.Instance.KeyPressed(Keys.F)
                    && Player.State == Entity.MoveState.Idle
                    && (Player.Sprite.SpriteSheetEffect.CurrentFrame.X == 0 || Player.Sprite.SpriteSheetEffect.CurrentFrame.X == 2)
                    && Player.Sprite.SpriteSheetEffect.CurrentFrame.Y < 4
                    && !DoorManager.IsTransitioning)
                {
                    if (!MenuManager.IsLoaded)
                    {
                        if (TextBoxManager.IsDisplayed)
                        {
                            TextBoxManager.UnloadContent(Player);
                        }

                        Player.Sprite.IsActive = false;
                        MenuManager.MenuName = "MainMenu";
                        MenuManager.LoadContent("Load/Menus/MainMenu.xml");
                        AreaManager.Reset();
                    }
                    else
                    {
                        MenuManager.UnloadContent();
                    }   
                }
            }

            if ((MenuManager.IsLoaded || TextBoxManager.IsDisplayed || DoorManager.IsTransitioning || ScreenManager.Instance.IsTransitioning) && Player.CanUpdate)
            {
                Player.CanUpdate = false;
            }
            else if (!MenuManager.IsLoaded && !TextBoxManager.IsDisplayed && !DoorManager.IsTransitioning && !ScreenManager.Instance.IsTransitioning && !Player.CanUpdate)
            {
                Player.CanUpdate = true;
            }

            DoorManager.Update(gameTime, this, mapLoader);
            EntityAnimationManager.Instance.Update(gameTime);
            Player.Update(gameTime, Map);
            Map.Update(gameTime, Player);
            Camera.Follow(Player);

            if (TextBoxManager.IsDisplayed && AreaManager.IsTransitioning)
            {
                AreaManager.Reset();
            }

            if (!Map.Inside && !DoorManager.IsTransitioning)
            {
                AreaManager.Update(gameTime, Map.Areas, Player);
            }

            // COUNTS AND ADDS TIME TO Player'S TOTAL GAME TIME
            if (MenuManager.Menu is not SaveMenu)
            {
                Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            }

            if (MenuManager.IsLoaded)
            {
                if (TextBoxManager.IsDisplayed)
                {
                    TextBoxManager.UnloadContent(Player);
                    TextBoxManager.Closed = true;
                }

                MenuManager.Update(gameTime);
            }

            if (!MenuManager.WasLoaded && !MenuManager.IsLoaded)
            {
                TextBoxManager.Update(gameTime, Map, Player);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch, "Underlay");

            List<Entity> drawOrder = new(){ Player };
            drawOrder.AddRange(Map.NPCs);

            drawOrder.Sort((a, b) => a.Sprite.Position.Y.CompareTo(b.Sprite.Position.Y));

            foreach (Entity entity in drawOrder)
            {
                EntityAnimationManager.Instance.Draw((IAnimatable) entity, spriteBatch);
                TileAnimationManager.Instance.Draw(entity, spriteBatch);
                entity.Draw(spriteBatch);
                EntityAnimationManager.Instance.PostDraw((IAnimatable) entity, spriteBatch);
                TileAnimationManager.Instance.PostDraw(entity, spriteBatch);
            }

            Map.Draw(spriteBatch, "Overlay");

            if (MenuManager.IsLoaded)
            {
                MenuManager.Draw(spriteBatch);
            }

            TextBoxManager.Draw(spriteBatch);

            if (!Map.Inside && !DoorManager.IsTransitioning)
            {
                AreaManager.Draw(spriteBatch);
            }

            DoorManager.Draw(spriteBatch);
        }
    }
}