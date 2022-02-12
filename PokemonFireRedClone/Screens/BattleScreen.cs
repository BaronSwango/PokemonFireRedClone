using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleScreen : GameScreen
    {

        public BattleTextBox TextBox;
        Image Background;
        Image EnemyPlatform;
        Image PlayerPlatform;
        Image EnemySprite;
        Image PlayerSprite;
        Image Pokeball;
        MenuManager menuManager;
        public bool IsTransitioning;

        public static bool Wild;

        /*
         * Battle screen:
         * - Pokemon sprites
         * - Battle menu
         *      - Fight menu
         *      - Access to pokemon menu
         *      - Access to item menu
         *      - Run away
         * - If wild, Battle starts with "Wild POKEMONNAME appeared!"
         * - Special textbox at bottom
         * - Exp bar
         * - Health bar
         * - Icon whether opposite pokemon has been caught
         * - Pokemon Name
         * - Pokemon Level
         * 
         * 
         * 
         */

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning && !ScreenManager.Instance.IsTransitioning)
            {
                float enemySpeed = (float)(0.596 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float playerSpeed = (float)(0.807 * gameTime.ElapsedGameTime.TotalMilliseconds);
                float enemyPlatformDestinationX = ScreenManager.Instance.Dimensions.X - EnemyPlatform.SourceRect.Width;
                float playerPlatformDestinationX = 16;

                if (!(PlayerPlatform.Position.X-playerSpeed < playerPlatformDestinationX) && !(EnemyPlatform.Position.X+enemySpeed > enemyPlatformDestinationX))
                {
                    PlayerPlatform.Position.X -= playerSpeed;
                    PlayerSprite.Position.X -= playerSpeed;
                    EnemyPlatform.Position.X += enemySpeed;
                    return;
                }

                PlayerPlatform.Position.X = playerPlatformDestinationX;
                PlayerSprite.Position.X = PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2;
                EnemyPlatform.Position.X = enemyPlatformDestinationX;
                IsTransitioning = false;
            }
        }


        public BattleScreen()
        {
            Wild = true;
            menuManager = new MenuManager("BattleMenu");
        }


        public override void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in
            Background = new Image();
            EnemyPlatform = new Image();
            PlayerPlatform = new Image();
            PlayerSprite = new Image();

            Background.Path = "BattleScreen/BattleBackground1";
            EnemyPlatform.Path = "BattleScreen/BattleBackground1EnemyPlatform";
            PlayerPlatform.Path = "BattleScreen/BattleBackground1PlayerPlatform";
            PlayerSprite.Path = Player.PlayerJsonObject.Gender == Gender.MALE ? "BattleScreen/RedSpriteFront" : "BattleScreen/BattleBackground1";

            Background.LoadContent();

            EnemyPlatform.LoadContent();
            PlayerPlatform.LoadContent();
            PlayerSprite.LoadContent();
            TextBox.LoadContent();

            EnemyPlatform.Position = new Vector2(-EnemyPlatform.SourceRect.Width, 192);
            PlayerPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X + PlayerPlatform.SourceRect.Width, TextBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            PlayerSprite.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerSprite.SourceRect.Height);
            IsTransitioning = true;
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            EnemyPlatform.UnloadContent();
            PlayerPlatform.UnloadContent();
            PlayerSprite.UnloadContent();
            TextBox.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Transition(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.K) && !IsTransitioning)
                ScreenManager.Instance.ChangeScreens("GameplayScreen");

            if (TextBox.Page == 4 && !menuManager.IsLoaded)
                menuManager.LoadContent("Load/Menus/BattleMenu.xml");
            else if (TextBox.Page != 4 && menuManager.IsLoaded)
                menuManager.UnloadContent();

            if (menuManager.IsLoaded)
                menuManager.Update(gameTime);
            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if (!IsTransitioning && !ScreenManager.Instance.IsTransitioning)
                TextBox.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TextBox.Draw(spriteBatch);
            Background.Draw(spriteBatch);
            EnemyPlatform.Draw(spriteBatch);
            PlayerPlatform.Draw(spriteBatch);
            PlayerSprite.Draw(spriteBatch);
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
