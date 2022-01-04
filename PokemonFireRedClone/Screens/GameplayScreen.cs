using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class GameplayScreen : GameScreen
    {
        public Player player;
        Map map;
        MenuManager menuManager;

        public Camera Camera
        {
            get; private set;
        }


        public GameplayScreen()
        {
            menuManager = new MenuManager("MainMenu");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            XmlManager<Player> playerLoader = new XmlManager<Player>();
            XmlManager<Map> mapLoader = new XmlManager<Map>();
            player = playerLoader.Load("Load/Gameplay/Player.xml");
            map = mapLoader.Load("Load/Gameplay/Map/PalletTown.xml");
            player.LoadContent();
            map.LoadContent();

            player.Spawn(map);

            Camera = new Camera();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            XmlManager<Player> playerSaver = new XmlManager<Player>();
            playerSaver.Save("Load/Gameplay/Baron.xml", player.Image.Position.X);
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.F) && player.state == Player.State.Idle)
            {               
                if (!menuManager.IsLoaded)
                    menuManager.LoadContent("Load/Menus/MainMenu.xml");
                else
                    menuManager.UnloadContent();
            }

            if (!menuManager.IsLoaded)
                player.Update(gameTime);
            map.Update(gameTime, ref player);
            Camera.Follow(player);
            if (menuManager.IsLoaded)
                menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch, "Underlay");
            player.Draw(spriteBatch);
            map.Draw(spriteBatch, "Overlay");
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);
        }


        
    }
}
