using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class BattleLogic
    {

        public BattlePokemon playerPokemon;
        public BattlePokemon enemyPokemon;
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
        public bool MoveMiss;
        public bool SharplyStat;
        public bool StatStageIncrease;
        public string Stat;
        public bool PlayerMoveExecuted;
        public bool EnemyMoveExecuted;
        bool playerFirst;

        public BattleLogic(BattleScreen battleScreen)
        {
            playerPokemon = new BattlePokemon(Player.PlayerJsonObject.Pokemon);
            enemyPokemon = new BattlePokemon(battleScreen.enemyPokemon);
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
                playerFirst = playerFirstMover(playerPokemon, enemyPokemon);
                StartSequence = false;
            }

            if (!PokemonFainted && PlayerMoveUsed)
            {
                if (playerFirst)
                {
                    if (PlayerHasMoved)
                    {
                        if (!EnemyMoveExecuted)
                        {
                            enemyUseMove(enemyPokemon, playerPokemon);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION;
                            EnemyMoveExecuted = true;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!PlayerMoveExecuted)
                        {
                            useMove(playerPokemon, enemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION;
                            PlayerMoveExecuted = true;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                        }
                    }
                }
                else
                {
                    if (EnemyHasMoved)
                    {
                        if (!PlayerMoveExecuted)
                        {
                            useMove(playerPokemon, enemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION;
                            PlayerMoveExecuted = true;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!EnemyMoveExecuted)
                        {
                            enemyUseMove(enemyPokemon, playerPokemon);
                            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION;
                            EnemyMoveExecuted = true;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                        }
                    }
                }
            } else if (PokemonFainted && battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_POKEMON_FAINT && Player.PlayerJsonObject.Pokemon.Level < 100)
            {
                if (!EXPGainApplied)
                {
                    
                    GainedEXP = calcualteEXP(battleScreen.enemyPokemon, BattleScreen.Wild, false, 1) * 100;
                    Player.PlayerJsonObject.Pokemon.CurrentEXP += GainedEXP;
                    int oldMaxHP = Player.PlayerJsonObject.Pokemon.Stats.HP;
                    while (Player.PlayerJsonObject.Pokemon.CurrentEXP >= Player.PlayerJsonObject.Pokemon.NextLevelEXP)
                    {
                        Player.PlayerJsonObject.Pokemon.Level++;
                        LevelUp = true;
                        if (Player.PlayerJsonObject.Pokemon.Level == 100)
                        {
                            GainedEXP -= Player.PlayerJsonObject.Pokemon.CurrentEXP - Player.PlayerJsonObject.Pokemon.CurrentLevelEXP;
                            Player.PlayerJsonObject.Pokemon.CurrentEXP = Player.PlayerJsonObject.Pokemon.CurrentLevelEXP;
                            break;
                        }
                    }

                    Player.PlayerJsonObject.Pokemon.Stats = PokemonManager.generateStatList(Player.PlayerJsonObject.Pokemon);
                    Player.PlayerJsonObject.Pokemon.CurrentHP += Player.PlayerJsonObject.Pokemon.Stats.HP - oldMaxHP;
                    EXPGainApplied = true;
                }
            }


        }

        private bool playerFirstMover(BattlePokemon playerPokemon, BattlePokemon enemyPokemon)
        {
            if (playerPokemon.TempSpeed == enemyPokemon.TempSpeed)
            {
                Random random = new Random();
                int first = random.Next(2);
                if (first == 0)
                    return true;
            }
            return playerPokemon.TempSpeed > enemyPokemon.TempSpeed;
        }

        private void useMove(BattlePokemon user, BattlePokemon defender, Move move)
        {
            user.Pokemon.MoveNames[move.Name] -= 1;

            int damage = 0;

            if (move.Category == "Special")
                damage = calculateDamage(
                    user.Pokemon.Level,
                    move.Power,
                    user.TempSpecialAttack,
                    defender.TempSpecialDefense,
                    user.Pokemon.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Pokemon.Types);
            else if (move.Category == "Physical")
                damage = calculateDamage(
                    user.Pokemon.Level,
                    move.Power,
                    user.TempAttack,
                    defender.TempDefense,
                    user.Pokemon.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Pokemon.Types);
            else
            {
                if (move.Self)
                {
                    Stat = user.AdjustTempStat(move.Stat, move.StageChange);
                } else
                {
                    Stat = defender.AdjustTempStat(move.Stat, move.StageChange);
                    Console.WriteLine("Attack: " + defender.TempAttack);
                }

                if (Math.Abs(move.StageChange) > 1)
                    SharplyStat = true;

                StatStageIncrease = move.StageChange > 0 ? true : false;

            }


            defender.Pokemon.CurrentHP = defender.Pokemon.CurrentHP - damage > 0 ? defender.Pokemon.CurrentHP - damage : 0;
            Console.WriteLine("Defender HP: " + defender.Pokemon.CurrentHP);
        }

        private void enemyUseMove(BattlePokemon enemyPokemon, BattlePokemon playerPokemon)
        {
            Random random = new Random();
            Move move = MoveManager.Instance.GetMove(enemyPokemon.Pokemon.MoveNames.Keys.ElementAt(random.Next(enemyPokemon.Pokemon.MoveNames.Count)));
            EnemyMoveOption = move;
            useMove(enemyPokemon, playerPokemon, move);
        }

        private int calculateDamage(int level, int power, int attack, int defense, List<Type> userTypes, Type moveType, List<Type> defenderTypes)
        {

            float STAB = 1;
            foreach (Type type in userTypes)
            {
                if (moveType == type)
                {
                    STAB = 1.5f;
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

        private bool moveMissed(int moveAccuracy, BattlePokemon attacker, BattlePokemon defender)
        {
            int accuracyStage = 0;
            accuracyStage += attacker.AccuracyStage -= defender.EvasionStage;


            Random random = new Random();
             
            int moveHit = random.Next(100) + 1;
            
            return false;
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
