using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class BattleLogic
    {

        public static Move playerMoveOption;

        public void Update(GameTime gameTime, BattleScreen battleScreen)
        {
            switch(battleScreen.BattleAnimations.state)
            {
                case BattleAnimations.BattleState.PLAYER_MOVE:
                    useMove(ref Player.PlayerJsonObject.Pokemon, ref battleScreen.enemyPokemon, playerMoveOption);
                    enemyUseMove(ref battleScreen.enemyPokemon, ref Player.PlayerJsonObject.Pokemon);
                    battleScreen.BattleAnimations.state = BattleAnimations.BattleState.ENEMY_MOVE;
                    break;
                case BattleAnimations.BattleState.ENEMY_MOVE:
                    enemyUseMove(ref battleScreen.enemyPokemon, ref Player.PlayerJsonObject.Pokemon);
                    battleScreen.BattleAnimations.state = BattleAnimations.BattleState.BATTLE_MENU;
                    break;
                default:
                    break;
            }

        }

        private bool playerFirstMover(CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
        {

            return false;
        }

        private void useMove(ref CustomPokemon user, ref CustomPokemon defender, Move move)
        {
            user.MoveNames[move.Name] -= 1;

            
            int damage = move.Special ?
                calculateDamage(
                    user.Level,
                    move.Power,
                    user.Stats.SpecialAttack,
                    defender.Stats.SpecialDefense,
                    user.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Types) :
                calculateDamage(
                    user.Level,
                    move.Power,
                    user.Stats.Attack,
                    defender.Stats.Defense,
                    user.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Types);

            Console.WriteLine("User used " + move.Name + "!");
            defender.CurrentHP = defender.CurrentHP - damage > 0 ? defender.CurrentHP - damage : 0;
            Console.WriteLine("Defender HP: " + defender.CurrentHP);
        }

        private void enemyUseMove(ref CustomPokemon enemyPokemon, ref CustomPokemon playerPokemon)
        {
            Random random = new Random();
            Move move = MoveManager.Instance.GetMove(enemyPokemon.MoveNames.Keys.ElementAt(random.Next(enemyPokemon.MoveNames.Count)));

            useMove(ref enemyPokemon, ref playerPokemon, move);
        }

        private int calculateDamage(int level, int power, int attack, int defense, List<Type> userTypes, Type moveType, List<Type> defenderTypes)
        {
            float STAB = 1;
            foreach (Type type in userTypes)
            {
                if (moveType == type)
                {
                    STAB *= 1.5f;
                    break;
                }
            }

            Random rand = new Random();
            float random = rand.Next(85, 101) / 100.0f;

            float critRand = (float) Math.Round(rand.NextDouble(), 4);

            float crit = critRand <= 0.0625f ? 1.5f : 1.0f;
            float typeMult = 1.0f;

            foreach (Type type in defenderTypes)
            {
                switch (TypeProperties.DamageMultiplier(moveType, type))
                {
                    case 0:
                        typeMult *= 0;
                        break;
                    case 0.5f:
                        typeMult *= 0.5f;
                        break;
                    case 2.0f:
                        typeMult *= 2.0f;
                        break;
                    default:
                        break;
                }
            }

            if (crit > 1)
                Console.WriteLine("Critical hit!");

            if (typeMult > 1)
                Console.WriteLine("It's super effective!");
            else if (typeMult < 1)
                Console.WriteLine("It's not very effective...");
            Console.WriteLine((int)((2.0f * level / 5 + 2) * power * ((float)attack / defense) / 50 * STAB * random * crit * typeMult));
            return (int) ((2.0f*level / 5 + 2) * power * ((float) attack / defense) / 50 * STAB * random * crit * typeMult);
        }

    }
}
