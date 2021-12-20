using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PokemonFireRedClone
{
    public class GameplayScreen : GameScreen
    {
        Player player;
        Map map;
        public Camera Camera
        {
            get; private set;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            XmlManager<Player> playerLoader = new XmlManager<Player>();
            XmlManager<Map> mapLoader = new XmlManager<Map>();
            player = playerLoader.Load("Load/Gameplay/Player.xml");
            map = mapLoader.Load("Load/Gameplay/Map/PalletTown.xml");
            player.LoadContent(map);
            map.LoadContent();

            if (TileManager.Instance.GetCurrentTile(map, player.Image, player.Image.SourceRect.Height/4) != null)
            {
                Vector2 centerTile = new Vector2(TileManager.Instance.GetCurrentTile(map, player.Image, player.Image.SourceRect.Height/4).Position.X-4,
                    TileManager.Instance.GetCurrentTile(map, player.Image, player.Image.SourceRect.Height/4).Position.Y-84);
                player.Image.Position = centerTile;
            }

            Camera = new Camera();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update(gameTime, map);
            map.Update(gameTime, ref player);
            Camera.Follow(player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch, "Underlay");
            player.Draw(spriteBatch);
            map.Draw(spriteBatch, "Overlay");
        }
    }
}
