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
        public BattleAnimations BattleAnimations;
        [XmlIgnore]
        public BattleLogic BattleLogic;
        [XmlIgnore]
        public MenuManager menuManager;

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
            menuManager = new MenuManager("BattleMenu");
        }


        public override void LoadContent()
        {
            
            // TODO: Load Background based on what environment the battle is in
            BattleLogic = new BattleLogic();
            TextBox.LoadContent(BattleLogic.Battle.EnemyPokemon.Pokemon);
            BattleAnimations.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            
            BattleAnimations.UnloadContent();
            TextBox.UnloadContent();
            if (menuManager.IsLoaded)
                menuManager.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (menuManager.menuName != "PokemonMenu")
            {
                BattleAnimations.Update(gameTime);
                BattleLogic.Update(gameTime);

                if (InputManager.Instance.KeyPressed(Keys.K) && !BattleAnimations.IsTransitioning)
                    ScreenManager.Instance.ChangeScreens("GameplayScreen");

                if (TextBox.Page == 4 && !menuManager.IsLoaded)
                {
                    menuManager.LoadContent("Load/Menus/BattleMenu.xml");
                    BattleAnimations.State = BattleAnimations.BattleState.BATTLE_MENU;
                }
                else if (TextBox.Page != 4 && menuManager.IsLoaded)
                    menuManager.UnloadContent();
            }

            if (menuManager.IsLoaded)
                menuManager.Update(gameTime);

            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;

            if (menuManager.menuName != "PokemonMenu")
            {
                if (InputManager.Instance.KeyPressed(Keys.E) && TextBox.BattleLevelUp.IsActive)
                    TextBox.BattleLevelUp.NextPage();

                if ((!BattleAnimations.IsTransitioning && !TextBox.BattleLevelUp.IsActive && (!ScreenManager.Instance.IsTransitioning || TextBox.NextPage == 4))
                    || BattleAnimations.State == BattleAnimations.BattleState.WILD_POKEMON_FADE_IN
                    || BattleAnimations.State == BattleAnimations.BattleState.DAMAGE_ANIMATION
                    || BattleAnimations.State == BattleAnimations.BattleState.STATUS_ANIMATION
                    || (BattleAnimations.State == BattleAnimations.BattleState.POKEMON_SWITCH && !ScreenManager.Instance.IsTransitioning))
                    TextBox.Update(gameTime);


            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if (menuManager.menuName != "PokemonMenu")
            {
                BattleAnimations.Draw(spriteBatch);

                TextBox.Draw(spriteBatch);

            }
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);
            
        }
    }
}
