using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class GameplayScreen : GameScreen
    {

        private Map map;
        private static bool menuWasLoaded;

        public Player Player;
        public MenuManager MenuManager;
        public TextBoxManager TextBoxManager;
        public Camera Camera { get; private set; }

        public GameplayScreen()
        {
            MenuManager = new MenuManager("MainMenu");
            TextBoxManager = new TextBoxManager();
            XmlManager<Player> PlayerLoader = new();
            XmlManager<Map> mapLoader = new();
            Player = PlayerLoader.Load("Load/Gameplay/Player.xml");
            map = mapLoader.Load("Load/Gameplay/Map/PalletTown.xml");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Player.LoadContent();
            map.LoadContent();
            TextBoxManager.LoadXML();
            Player.Spawn(map);

            foreach (NPC npc in map.NPCs)
                npc.Spawn(map);

            Camera = new Camera();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Player.UnloadContent();
            map.UnloadContent();
            menuWasLoaded = MenuManager.IsLoaded;
            if (MenuManager.IsLoaded)
                MenuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.K))
                ScreenManager.Instance.ChangeScreens("BattleScreen");

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
                    && Player.Sprite.SpriteSheetEffect.CurrentFrame.Y < 4)
                {
                    if (!MenuManager.IsLoaded)
                    {
                        if (TextBoxManager.IsDisplayed)
                            TextBoxManager.UnloadContent(ref Player);
                        Player.Sprite.IsActive = false;
                        MenuManager.MenuName = "MainMenu";
                        MenuManager.LoadContent("Load/Menus/MainMenu.xml");
                    }
                    else
                        MenuManager.UnloadContent();
                    
                }
            }

            if ((MenuManager.IsLoaded || TextBoxManager.IsDisplayed) && Player.CanUpdate)
                Player.CanUpdate = false;
            else if (!MenuManager.IsLoaded && !TextBoxManager.IsDisplayed && !Player.CanUpdate)
                Player.CanUpdate = true;

            Player.Update(gameTime, ref map);
            map.Update(gameTime, ref Player);
            Camera.Follow(Player);


            // COUNTS AND ADDS TIME TO Player'S TOTAL GAME TIME
            if (!(MenuManager.Menu is SaveMenu))
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
                TextBoxManager.Update(gameTime, ref map, ref Player);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch, "Underlay");
            foreach (NPC npc in map.NPCs)
                npc.NPCSprite.Bottom.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            foreach (NPC npc in map.NPCs)
                npc.NPCSprite.Top.Draw(spriteBatch);
            map.Draw(spriteBatch, "Overlay");
            if (MenuManager.IsLoaded)
                MenuManager.Draw(spriteBatch);
            TextBoxManager.Draw(spriteBatch);
        }


        
    }
}
