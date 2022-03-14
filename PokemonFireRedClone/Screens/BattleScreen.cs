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
        public BattleLogic BattleLogic;
        [XmlIgnore]
        public MenuManager menuManager;
        [XmlIgnore]
        public CustomPokemon enemyPokemon;
        CustomPokemon playerPokemon;

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
            menuManager = new MenuManager("BattleMenu");
            playerPokemon = Player.PlayerJsonObject.Pokemon;
            enemyPokemon = PokemonManager.createPokemon(PokemonManager.Instance.GetPokemon("Pidgeot"), 50);
            enemyPokemon.CurrentHP = enemyPokemon.Stats.HP;
            enemyPokemon.MoveNames.Add("Water Gun", 25);
            //enemyPokemon.MoveNames.Add("Aeroblast", 5);
            BattleLogic = new BattleLogic();
        }


        public override void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in

            TextBox.LoadContent(enemyPokemon);
            BattleAnimations.LoadContent(this);

            Console.WriteLine("Player Poke: ");
            Console.WriteLine("Nature: " + playerPokemon.Nature.ToString());
            Console.WriteLine("HP: " + playerPokemon.Stats.HP);
            Console.WriteLine("Attack: " + playerPokemon.Stats.Attack);
            Console.WriteLine("Defense: " + playerPokemon.Stats.Defense);
            Console.WriteLine("Special Attack: " + playerPokemon.Stats.SpecialAttack);
            Console.WriteLine("Special Defense: " + playerPokemon.Stats.SpecialDefense);
            Console.WriteLine("Speed: " + playerPokemon.Stats.Speed);
            Console.WriteLine("EXP TO NEXT: " + playerPokemon.CurrentLevelEXP);
            Console.WriteLine("--------");
            Console.WriteLine("Enemy Poke: ");
            Console.WriteLine("Nature: " + enemyPokemon.Nature.ToString());
            Console.WriteLine("HP: " + enemyPokemon.Stats.HP);
            Console.WriteLine("Attack: " + enemyPokemon.Stats.Attack);
            Console.WriteLine("Defense: " + enemyPokemon.Stats.Defense);
            Console.WriteLine("Special Attack: " + enemyPokemon.Stats.SpecialAttack);
            Console.WriteLine("Special Defense: " + enemyPokemon.Stats.SpecialDefense);
            Console.WriteLine("Speed: " + enemyPokemon.Stats.Speed);
            Console.WriteLine("EXP TO NEXT: " + enemyPokemon.CurrentLevelEXP);
            Console.WriteLine("---------------");
            Console.WriteLine();
            foreach (Move move in playerPokemon.Pokemon.MoveLearnset.Keys)
            {
                Console.WriteLine(move.Name);
            }
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            BattleAnimations.UnloadContent();
            TextBox.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            BattleAnimations.Update(gameTime, this);
            BattleLogic.Update(gameTime, this);
            
            if (InputManager.Instance.KeyPressed(Keys.K) && !BattleAnimations.IsTransitioning)
                ScreenManager.Instance.ChangeScreens("GameplayScreen");

            if (TextBox.Page == 4 && BattleAnimations.state != BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION && !menuManager.IsLoaded)
            {
                menuManager.LoadContent("Load/Menus/BattleMenu.xml");
                BattleAnimations.state = BattleAnimations.BattleState.BATTLE_MENU;
            }
            else if (TextBox.Page != 4 && menuManager.IsLoaded)
                menuManager.UnloadContent();

            if (menuManager.IsLoaded)
                menuManager.Update(gameTime);
            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if ((!BattleAnimations.IsTransitioning && !ScreenManager.Instance.IsTransitioning) || BattleAnimations.state == BattleAnimations.BattleState.WILD_POKEMON_FADE_IN || BattleAnimations.state == BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION || BattleAnimations.state == BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION)
                TextBox.Update(gameTime, this);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BattleAnimations.Draw(spriteBatch, TextBox);

            TextBox.Draw(spriteBatch);
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
