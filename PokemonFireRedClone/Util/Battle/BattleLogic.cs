using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class BattleLogic
    {

        public Move PlayerMoveOption;
        public Move EnemyMoveOption;
        public int GainedEXP;
        public bool PlayerMoveUsed;
        public bool PlayerHasMoved;
        public bool EnemyHasMoved;
        public bool SuperEffective;
        public bool NotVeryEffective;
        public bool NoEffect;
        public bool Crit;
        public bool PokemonFainted;
        public bool StartSequence;
        public bool EXPGainApplied;
        public bool LevelUp;
        bool playerFirst;

        public BattleLogic()
        {
            PlayerMoveUsed = false;
            PlayerHasMoved = false;
            EnemyHasMoved = false;
            Crit = false;
            SuperEffective = false;
            NotVeryEffective = false;
            NoEffect = false;
            StartSequence = false;
            playerFirst = false;
            PokemonFainted = false;
            EXPGainApplied = false;
        }

        public void Update(GameTime gameTime, BattleScreen battleScreen)
        {

            if (StartSequence)
            {
                playerFirst = playerFirstMover(Player.PlayerJsonObject.Pokemon, battleScreen.enemyPokemon);
                StartSequence = false;
            }

            if (!PokemonFainted && PlayerMoveUsed)
            {
                if (playerFirst)
                {
                    if (PlayerHasMoved)
                    {
                        if (battleScreen.BattleAnimations.state != BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION)
                        {
                            enemyUseMove(ref battleScreen.enemyPokemon, ref Player.PlayerJsonObject.Pokemon);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (battleScreen.BattleAnimations.state != BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION)
                        {
                            useMove(ref Player.PlayerJsonObject.Pokemon, ref battleScreen.enemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                        }
                    }
                }
                else
                {
                    if (EnemyHasMoved)
                    {
                        if (battleScreen.BattleAnimations.state != BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION)
                        {
                            useMove(ref Player.PlayerJsonObject.Pokemon, ref battleScreen.enemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (battleScreen.BattleAnimations.state != BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION)
                        {
                            enemyUseMove(ref battleScreen.enemyPokemon, ref Player.PlayerJsonObject.Pokemon);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                        }
                    }
                }
            } else if (PokemonFainted && battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_POKEMON_FAINT && Player.PlayerJsonObject.Pokemon.Level < 100)
            {
                if (!EXPGainApplied)
                {
                    
                    GainedEXP = calcualteEXP(battleScreen.enemyPokemon, BattleScreen.Wild, false, 1);
                    Player.PlayerJsonObject.Pokemon.CurrentEXP += GainedEXP;
                    if (Player.PlayerJsonObject.Pokemon.CurrentEXP >= Player.PlayerJsonObject.Pokemon.NextLevelEXP)
                    {
                        Player.PlayerJsonObject.Pokemon.Level++;
                        LevelUp = true;
                    }

                    Player.PlayerJsonObject.Pokemon.Stats = PokemonManager.generateStatList(Player.PlayerJsonObject.Pokemon);
                    EXPGainApplied = true;
                }
            }


        }

        private bool playerFirstMover(CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
        {
            if (playerPokemon.Stats.Speed == enemyPokemon.Stats.Speed)
            {
                Random random = new Random();
                int first = random.Next(2);
                if (first == 0)
                    return true;
            }
            return playerPokemon.Stats.Speed > enemyPokemon.Stats.Speed;
        }

        private void useMove(ref CustomPokemon user, ref CustomPokemon defender, Move move)
        {
            user.MoveNames[move.Name] -= 1;

            int damage = 0;

            if (move.Category == "Special")
                damage = calculateDamage(
                    user.Level,
                    move.Power,
                    user.Stats.SpecialAttack,
                    defender.Stats.SpecialDefense,
                    user.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Types);
            else if (move.Category == "Physical")
                damage = calculateDamage(
                    user.Level,
                    move.Power,
                    user.Stats.Attack,
                    defender.Stats.Defense,
                    user.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Types);
            else
            {
                // Status moves
            }


            Console.WriteLine("User used " + move.Name + "!");
            defender.CurrentHP = defender.CurrentHP - damage > 0 ? defender.CurrentHP - damage : 0;
            Console.WriteLine("Defender HP: " + defender.CurrentHP);
        }

        private void enemyUseMove(ref CustomPokemon enemyPokemon, ref CustomPokemon playerPokemon)
        {
            Random random = new Random();
            Move move = MoveManager.Instance.GetMove(enemyPokemon.MoveNames.Keys.ElementAt(random.Next(enemyPokemon.MoveNames.Count)));
            EnemyMoveOption = move;
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

            float crit = critRand < 0.0625f ? 2.0f : 1.0f;
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
                Crit = true;

            if (typeMult > 1)
                SuperEffective = true;
            else if (typeMult < 1 && typeMult > 0)
                NotVeryEffective = true;
            else if (typeMult == 0)
                NoEffect = true;

            int damage = (int)((2.0f * level / 5 + 2) * power * ((float)attack / defense) / 50 * STAB * random * crit * typeMult);

            if (damage < 1)
                damage = 1;

            Console.WriteLine("Damage: " + damage);

            return damage;
        }

        // add exp share calculation
        private int calcualteEXP(CustomPokemon faintedPokemon, bool wild, bool luckyEgg, int numOfPokemonNotFained)
        {
            float trainerMult = !wild ? 1.5f : 1f;
            float eggMult = luckyEgg ? 1.5f : 1f;

            return (int)(trainerMult * faintedPokemon.Pokemon.EXPYield * faintedPokemon.Level * eggMult / (numOfPokemonNotFained * 7));
        }

    }
}
