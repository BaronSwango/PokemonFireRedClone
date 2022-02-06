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
        Image PlayerPokemon;
        Image EnemyPokemon;
        Image PlayerSprite;
        Image Pokeball;

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

        public BattleScreen()
        {
            Wild = true;
        }


        public override void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in
            Background = new Image();
            EnemyPlatform = new Image();

            Background.Path = "BattleScreen/BattleBackground1";
            
            Background.LoadContent();
            TextBox.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.K))
                ScreenManager.Instance.ChangeScreens("GameplayScreen");


            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if (!ScreenManager.Instance.IsTransitioning)
                TextBox.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TextBox.Draw(spriteBatch);
            Background.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
