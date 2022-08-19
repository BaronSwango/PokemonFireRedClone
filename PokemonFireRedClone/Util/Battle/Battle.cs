using System;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Battle
    {

        public BattlePokemon EnemyPokemon;
        public BattlePokemon PlayerPokemon;
        public List<CustomPokemon> BattlePokemonInBag;

        public bool IsWild;
        public bool InBattle;

        public Battle(params CustomPokemon[] enemyPokemon)
        {

            foreach (CustomPokemon pokemon in enemyPokemon)
                Console.WriteLine(pokemon.PokemonName.ToUpper() + ": " + pokemon.CurrentHP);
            InBattle = true;
            EnemyPokemon = new BattlePokemon(enemyPokemon[0]);
            EnemyPokemon.Pokemon.CurrentHP = EnemyPokemon.Pokemon.Stats.HP;
            BattlePokemonInBag = new List<CustomPokemon>(Player.PlayerJsonObject.PokemonInBag);
            PlayerPokemon = new BattlePokemon(BattlePokemonInBag[0]);
            //EnemyPokemon.Pokemon.MovePP.Add("Growl", 40);
            //EnemyPokemon.Pokemon.MovePP.Add("Double Team", 15);
            //EnemyPokemon.Pokemon.MovePP.Add("Water Gun", 20);
        }

        public void SwapPokemon(int index)
        {
            CustomPokemon temp = BattlePokemonInBag[0];
            BattlePokemonInBag[0] = BattlePokemonInBag[index];
            BattlePokemonInBag[index] = temp;
        }

        public void UpdatePlayerPokemon()
        {
            PlayerPokemon = new BattlePokemon(BattlePokemonInBag[0]);
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
