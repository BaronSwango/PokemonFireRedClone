using System;
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
            MenuManager = new MenuManager("MainMenu");
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
            TextBoxManager.LoadXML();
            Player.Spawn(ref Map);

            foreach (NPC npc in Map.NPCs)
                npc.Spawn(ref Map);
            
            Camera = new Camera();
            AreaManager.LoadContent(Map.Areas, Player);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Player.UnloadContent();
            Map.UnloadContent();
            menuWasLoaded = MenuManager.IsLoaded;
            if (MenuManager.IsLoaded)
                MenuManager.UnloadContent();
            AreaManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            //float speed = (float)(30 * gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);

            // print player's current tile to console
            if (InputManager.Instance.KeyPressed(Keys.M))
            {
                Tile currentTile = TileManager.GetCurrentTile(Map, Player.Sprite, Player.Sprite.SourceRect.Width / 2, Player.Sprite.SourceRect.Height, 1);
                if (currentTile != null)
                    Console.WriteLine($"({currentTile.Position.X}, {currentTile.Position.Y})");
                
            }

            if (MenuManager.WasLoaded)
                MenuManager.WasLoaded = false;
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
                            TextBoxManager.UnloadContent(ref Player);
                        Player.Sprite.IsActive = false;
                        MenuManager.MenuName = "MainMenu";
                        MenuManager.LoadContent("Load/Menus/MainMenu.xml");
                        AreaManager.Reset();
                    }
                    else
                        MenuManager.UnloadContent();
                    
                }
            }

            if ((MenuManager.IsLoaded || TextBoxManager.IsDisplayed || DoorManager.IsTransitioning || ScreenManager.Instance.IsTransitioning) && Player.CanUpdate)
                Player.CanUpdate = false;
            else if (!MenuManager.IsLoaded && !TextBoxManager.IsDisplayed && !DoorManager.IsTransitioning && !ScreenManager.Instance.IsTransitioning && !Player.CanUpdate)
                Player.CanUpdate = true;

            DoorManager.Update(gameTime, this, mapLoader);
            Player.Update(gameTime, ref Map);
            Map.Update(gameTime, ref Player);
            Camera.Follow(Player);

            if (TextBoxManager.IsDisplayed && AreaManager.IsTransitioning)
                AreaManager.Reset();

            if (!Map.Inside && !DoorManager.IsTransitioning)
                AreaManager.Update(gameTime, Map.Areas, Player);

            // COUNTS AND ADDS TIME TO Player'S TOTAL GAME TIME
            if (MenuManager.Menu is not SaveMenu)
                Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if (MenuManager.IsLoaded)
            {
                if (TextBoxManager.IsDisplayed)
                {
                    TextBoxManager.UnloadContent(ref Player);
                    TextBoxManager.Closed = true;
                }
                MenuManager.Update(gameTime);
            }
            if (!MenuManager.WasLoaded && !MenuManager.IsLoaded)
                TextBoxManager.Update(gameTime, ref Map, ref Player);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch, "Underlay");
            foreach (NPC npc in Map.NPCs)
                npc.NPCSprite.Bottom.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            foreach (NPC npc in Map.NPCs)
                npc.NPCSprite.Top.Draw(spriteBatch);
            Map.Draw(spriteBatch, "Overlay");
            if (MenuManager.IsLoaded)
                MenuManager.Draw(spriteBatch);
            TextBoxManager.Draw(spriteBatch);
            if (!Map.Inside && !DoorManager.IsTransitioning)
                AreaManager.Draw(spriteBatch);
            DoorManager.Draw(spriteBatch);
        }

    }
}