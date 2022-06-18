using System;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Battle
    {

        public BattlePokemon EnemyPokemon;
        public BattlePokemon PlayerPokemon;
        public List<CustomPokemon> BattlePokemonMenu;

        public bool IsWild;
        public static bool InBattle;

        public Battle(CustomPokemon enemyPokemon)
        {
            IsWild = true;
            InBattle = true;
            EnemyPokemon = new BattlePokemon(enemyPokemon);
            EnemyPokemon.Pokemon.CurrentHP = EnemyPokemon.Pokemon.Stats.HP;
            BattlePokemonMenu = new List<CustomPokemon>(Player.PlayerJsonObject.PokemonInBag);
            PlayerPokemon = new BattlePokemon(BattlePokemonMenu[0]);
            EnemyPokemon.Pokemon.MoveNames.Add("Growl", 40);
            EnemyPokemon.Pokemon.MoveNames.Add("Vine Whip", 35);
        }

        public void SwapPokemonInBattle(int index)
        {
            CustomPokemon temp = BattlePokemonMenu[0];
            BattlePokemonMenu[0] = BattlePokemonMenu[index];
            BattlePokemonMenu[index] = temp;
        }

        public void UpdatePlayerPokemon()
        {
            PlayerPokemon = new BattlePokemon(BattlePokemonMenu[0]);
        }

        /*
        public void DisplayStats()
        {
            Console.WriteLine("Player Poke: ");
            Console.WriteLine("Nature: " + PlayerPokemon.Nature.ToString());
            Console.WriteLine("HP: " + PlayerPokemon.Stats.HP);
            Console.WriteLine("Attack: " + PlayerPokemon.Stats.Attack);
            Console.WriteLine("Defense: " + PlayerPokemon.Stats.Defense);
            Console.WriteLine("Special Attack: " + PlayerPokemon.Stats.SpecialAttack);
            Console.WriteLine("Special Defense: " + PlayerPokemon.Stats.SpecialDefense);
            Console.WriteLine("Speed: " + PlayerPokemon.Stats.Speed);
            Console.WriteLine("EXP TO NEXT: " + PlayerPokemon.CurrentLevelEXP);
            Console.WriteLine("--------");
            Console.WriteLine("Enemy Poke: ");
            Console.WriteLine("Nature: " + EnemyPokemon.Nature.ToString());
            Console.WriteLine("HP: " + EnemyPokemon.Stats.HP);
            Console.WriteLine("Attack: " + EnemyPokemon.Stats.Attack);
            Console.WriteLine("Defense: " + EnemyPokemon.Stats.Defense);
            Console.WriteLine("Special Attack: " + EnemyPokemon.Stats.SpecialAttack);
            Console.WriteLine("Special Defense: " + EnemyPokemon.Stats.SpecialDefense);
            Console.WriteLine("Speed: " + EnemyPokemon.Stats.Speed);
            Console.WriteLine("EXP TO NEXT: " + EnemyPokemon.CurrentLevelEXP);
            Console.WriteLine("---------------");
            Console.WriteLine();
            foreach (Move move in PlayerPokemon.Pokemon.MoveLearnset.Keys)
            {
                Console.WriteLine(move.Name);
            }
        }
        */





    }
}
