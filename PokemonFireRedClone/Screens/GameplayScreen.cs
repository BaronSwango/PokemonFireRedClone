using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class GameplayScreen : GameScreen
    {
        public Player player;
        Map map;
        public MenuManager menuManager;
        public TextBoxManager TextBoxManager;

        static bool menuWasLoaded;

        public Camera Camera
        {
            get; private set;
        }

        public GameplayScreen()
        {
            menuManager = new MenuManager("MainMenu");
            TextBoxManager = new TextBoxManager();
            XmlManager<Player> playerLoader = new();
            XmlManager<Map> mapLoader = new XmlManager<Map>();
            player = playerLoader.Load("Load/Gameplay/Player.xml");
            map = mapLoader.Load("Load/Gameplay/Map/PalletTown.xml");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            player.LoadContent();
            map.LoadContent();
            TextBoxManager.LoadXML();

            player.Spawn(map);
            
            Camera = new Camera();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
            menuWasLoaded = menuManager.IsLoaded;
            if (menuManager.IsLoaded)
                menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.K))
                ScreenManager.Instance.ChangeScreens("BattleScreen");

            if (menuManager.wasLoaded)
                menuManager.wasLoaded = false;

            if (menuWasLoaded)
            {
                menuManager.menuName = "MainMenu";
                menuManager.LoadContent("Load/Menus/MainMenu.xml");
                player.ResetPosition();
                menuWasLoaded = false;
            }
            else
            {
                if (InputManager.Instance.KeyPressed(Keys.F)
                    && player.state == Player.State.Idle
                    && (player.Image.SpriteSheetEffect.CurrentFrame.X == 0 || player.Image.SpriteSheetEffect.CurrentFrame.X == 2)
                    && player.Image.SpriteSheetEffect.CurrentFrame.Y < 4)
                {
                    if (!menuManager.IsLoaded)
                    {
                        if (TextBoxManager.IsDisplayed)
                            TextBoxManager.UnloadContent(ref player);
                        player.Image.IsActive = false;
                        menuManager.menuName = "MainMenu";
                        menuManager.LoadContent("Load/Menus/MainMenu.xml");
                    }
                    else
                        menuManager.UnloadContent();
                    
                }
            }

            if ((menuManager.IsLoaded || TextBoxManager.IsDisplayed) && player.CanUpdate)
                player.CanUpdate = false;
            else if (!menuManager.IsLoaded && !TextBoxManager.IsDisplayed && !player.CanUpdate)
                player.CanUpdate = true;

            player.Update(gameTime, ref map);
            map.Update(gameTime, ref player);
            Camera.Follow(player);


            // COUNTS AND ADDS TIME TO PLAYER'S TOTAL GAME TIME
            if (!(menuManager.menu is SaveMenu))
                Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if (menuManager.IsLoaded)
            {
                if (TextBoxManager.IsDisplayed)
                {
                    TextBoxManager.UnloadContent(ref player);
                    TextBoxManager.Closed = true;
                }
                menuManager.Update(gameTime);
            }
            if (!menuManager.wasLoaded && !menuManager.IsLoaded)
                TextBoxManager.Update(gameTime, ref map, ref player);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch, "Underlay");
            player.Draw(spriteBatch);
            map.Draw(spriteBatch, "Overlay");
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);
            TextBoxManager.Draw(spriteBatch);
        }


        
    }
}
