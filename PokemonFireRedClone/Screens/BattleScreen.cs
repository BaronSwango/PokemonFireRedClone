using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleScreen : GameScreen
    {

        public BattleTextBox TextBox;
        [XmlIgnore]
        public Trainer Trainer;
        public BattleAssets BattleAssets;
        [XmlIgnore]
        public BattleLogic BattleLogic;
        [XmlIgnore]
        public MenuManager MenuManager;

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
            MenuManager = new MenuManager("BattleMenu");
        }


        public override void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in
            BattleLogic = new BattleLogic(Trainer);
            TextBox.LoadContent(BattleLogic.Battle.EnemyPokemon.Pokemon);
            BattleAssets.LoadContent();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            
            BattleAssets.UnloadContent();
            TextBox.UnloadContent();
            if (MenuManager.IsLoaded)
                MenuManager.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            if (MenuManager.MenuName != "PokemonMenu")
            {
                BattleAssets.Update(gameTime);
                if (!BattleAssets.IsTransitioning || BattleAssets.State == BattleAssets.BattleState.INTRO || BattleAssets.State == BattleAssets.BattleState.POKEMON_FAINT || BattleAssets.State == BattleAssets.BattleState.BATTLE_MENU)
                    BattleLogic.Update();

                if (InputManager.Instance.KeyPressed(Keys.K) && !BattleAssets.IsTransitioning)
                    ScreenManager.Instance.ChangeScreens("GameplayScreen");

                if (TextBox.Page == 4 && !MenuManager.IsLoaded)
                {
                    MenuManager.LoadContent("Load/Menus/BattleMenu.xml");
                    BattleAssets.State = BattleAssets.BattleState.BATTLE_MENU;
                }
                else if (TextBox.Page != 4 && MenuManager.IsLoaded)
                    MenuManager.UnloadContent();
            }

            if (MenuManager.IsLoaded)
                MenuManager.Update(gameTime);

            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;

            if (MenuManager.MenuName != "PokemonMenu")
            {
                if (InputManager.Instance.KeyPressed(Keys.E) && TextBox.BattleLevelUp.IsActive)
                    TextBox.BattleLevelUp.NextPage();

                if ((!BattleAssets.IsTransitioning && !TextBox.BattleLevelUp.IsActive && (!ScreenManager.Instance.IsTransitioning || TextBox.NextPage == 4))
                    || BattleAssets.State == BattleAssets.BattleState.TRAINER_BALL_BAR
                    || BattleAssets.State == BattleAssets.BattleState.WILD_POKEMON_FADE_IN
                    || BattleAssets.State == BattleAssets.BattleState.OPPONENT_INTRO_SEND_POKEMON
                    || BattleAssets.State == BattleAssets.BattleState.OPPONENT_SEND_POKEMON
                    || BattleAssets.State == BattleAssets.BattleState.DAMAGE_ANIMATION
                    || BattleAssets.State == BattleAssets.BattleState.STATUS_ANIMATION
                    || (BattleAssets.State == BattleAssets.BattleState.POKEMON_SWITCH && !ScreenManager.Instance.IsTransitioning))
                    TextBox.Update(gameTime);


            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if (MenuManager.MenuName != "PokemonMenu")
            {
                BattleAssets.Draw(spriteBatch);

                TextBox.Draw(spriteBatch);

            }
            if (MenuManager.IsLoaded)
                MenuManager.Draw(spriteBatch);
            
        }
    }
}
