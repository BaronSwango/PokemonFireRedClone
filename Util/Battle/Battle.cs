using System;
using System.Collections.Generic;

namespace PokemonFireRedClone
{
    public class Battle
    {

        public Trainer Trainer;
        public BattlePokemon EnemyPokemon;
        public BattlePokemon PlayerPokemon;
        public List<CustomPokemon> BattlePokemonInBag;
        public List<CustomPokemon> OpponentPokemon;

        public bool IsWild { get; private set; }
        public bool InBattle;

        public bool IsEnded
        {
            get
            {
                foreach (CustomPokemon pokemon in OpponentPokemon)
                {
                    if (pokemon.CurrentHP > 0)
                        return false;
                }

                return true;
            }

            private set { }
        }

        public Battle(Trainer trainer, params CustomPokemon[] enemyPokemon)
        {
            Trainer = trainer;

            IsWild = trainer == null;

            foreach (CustomPokemon pokemon in enemyPokemon)
                Console.WriteLine(pokemon.PokemonName.ToUpper() + ": " + pokemon.CurrentHP);
            InBattle = true;
            EnemyPokemon = new BattlePokemon(enemyPokemon[0]);
            EnemyPokemon.Pokemon.CurrentHP = EnemyPokemon.Pokemon.Stats.HP;
            OpponentPokemon = new List<CustomPokemon>(enemyPokemon);

            BattlePokemonInBag = new List<CustomPokemon>(Player.PlayerJsonObject.PokemonInBag);
            PlayerPokemon = new BattlePokemon(BattlePokemonInBag[0]);
            EnemyPokemon.Pokemon.MovePP.Add("Growl", 40);

            if (!Player.PlayerJsonObject.Pokedex.ContainsKey(EnemyPokemon.Pokemon.Name))
            {
                Player.PlayerJsonObject.Pokedex.Add(EnemyPokemon.Pokemon.Name, false);
            }
            //EnemyPokemon.Pokemon.MovePP.Add("Double Team", 15);
            //EnemyPokemon.Pokemon.MovePP.Add("Water Gun", 20);
        }

        public void SwapPokemon(int index)
        {
            CustomPokemon temp = BattlePokemonInBag[0];
            BattlePokemonInBag[0] = BattlePokemonInBag[index];
            BattlePokemonInBag[index] = temp;
        }

        public void ChooseNewOpponentPokemon()
        {
            List<int> pokemonAliveIndices = new();

            foreach (CustomPokemon pokemon in OpponentPokemon)
            {
                if (pokemon.CurrentHP > 0)
                    pokemonAliveIndices.Add(OpponentPokemon.IndexOf(pokemon));
            }

            Random random = new();

            int index = random.Next(pokemonAliveIndices.Count);
            EnemyPokemon = new BattlePokemon(OpponentPokemon[pokemonAliveIndices[index]]);

            if (!Player.PlayerJsonObject.Pokedex.ContainsKey(EnemyPokemon.Pokemon.Name))
            {
                Player.PlayerJsonObject.Pokedex.Add(EnemyPokemon.Pokemon.Name, false);
            }
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
