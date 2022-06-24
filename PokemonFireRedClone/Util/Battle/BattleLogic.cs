﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class BattleLogic
    {

        public enum FightState
        {
            NONE,
            PLAYER_DEFEND,
            ENEMY_DEFEND,
            PLAYER_STATUS,
            ENEMY_STATUS,
            PLAYER_FAINT,
            ENEMY_FAINT
        }

        static Battle battle;

        public static bool PlayerShift;
        public static int ShiftNumber;

        public FightState State;
        public Move PlayerMoveOption;
        public Move EnemyMoveOption;
        public int GainedEXP;
        public string Stat;
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
        public bool MoveHit;
        public bool SharplyStat;
        public bool StatStageIncrease;
        public bool StageMaxed;
        public bool PlayerMoveExecuted;
        public bool EnemyMoveExecuted;
        bool playerFirst;

        BattleScreen battleScreen
        {
            get { return (BattleScreen)ScreenManager.Instance.CurrentScreen; }
            set { }
        }

        public static Battle Battle
        {
            get { return battle; }
            private set { }
        }

        public BattleLogic()
        {
            if (!Battle.InBattle)
                battle = new Battle(PokemonManager.Instance.CreatePokemon(PokemonManager.Instance.GetPokemon("Mew"), 20));

            PlayerMoveUsed = false;
            PlayerHasMoved = false;
            EnemyHasMoved = false;
            Crit = false;
            SuperEffective = false;
            NotVeryEffective = false;
            NoEffect = false;
            MoveHit = true;
            StartSequence = false;
            playerFirst = false;
            PokemonFainted = false;
            EXPGainApplied = false;
            State = FightState.NONE;
        }

        public void Update(GameTime gameTime)
        {
            if (PlayerShift)
            {
                playerFirst = true;
                PlayerMoveExecuted = true;
                PlayerMoveUsed = true;
                PlayerShift = false;
                BattleMenu.SavedItemNumber = 0;
                MoveMenu.SavedItemNumber = 0;
                Battle.SwapPokemonInBattle(ShiftNumber);
                battleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_SWITCH;
                battleScreen.BattleAssets.Animation = new PokemonSwitchAnimation();
                battleScreen.BattleAssets.IsTransitioning = true;
                battleScreen.TextBox.NextPage = 19;
                battleScreen.TextBox.IsTransitioning = true;
            }
            else if (StartSequence)
            {
                playerFirst = playerFirstMover(Battle.PlayerPokemon, Battle.EnemyPokemon);
                StartSequence = false;
            }

            // TODO: ADD MOVE ACCURACY TO STATUS MOVES
            if (!PokemonFainted && PlayerMoveUsed)
            {
                if (playerFirst)
                {
                    if (PlayerHasMoved)
                    {
                        if (!EnemyMoveExecuted)
                        {
                            MoveHit = enemyUseMove(Battle.EnemyPokemon, Battle.PlayerPokemon);
                            battleScreen.BattleAssets.State = EnemyMoveOption.Category == "Status" ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            battleScreen.BattleAssets.Animation = EnemyMoveOption.Category == "Status" ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.PLAYER_DEFEND;
                            EnemyMoveExecuted = true;
                            battleScreen.BattleAssets.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!PlayerMoveExecuted)
                        {
                            MoveHit = useMove(Battle.PlayerPokemon, Battle.EnemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAssets.State = PlayerMoveOption.Category == "Status" ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            battleScreen.BattleAssets.Animation = PlayerMoveOption.Category == "Status" ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.ENEMY_DEFEND;
                            PlayerMoveExecuted = true;
                            battleScreen.BattleAssets.IsTransitioning = true;
                        }
                    }
                }
                else
                {
                    if (EnemyHasMoved)
                    {
                        if (!PlayerMoveExecuted)
                        {
                            MoveHit = useMove(Battle.PlayerPokemon, Battle.EnemyPokemon, PlayerMoveOption);
                            battleScreen.BattleAssets.State = PlayerMoveOption.Category == "Status" ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            battleScreen.BattleAssets.Animation = PlayerMoveOption.Category == "Status" ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.ENEMY_DEFEND;
                            PlayerMoveExecuted = true;
                            battleScreen.BattleAssets.IsTransitioning = true;
                            battleScreen.TextBox.NextPage = 5;
                            battleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!EnemyMoveExecuted)
                        {
                            MoveHit = enemyUseMove(Battle.EnemyPokemon, Battle.PlayerPokemon);
                            battleScreen.BattleAssets.State = EnemyMoveOption.Category == "Status" ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            battleScreen.BattleAssets.Animation = EnemyMoveOption.Category == "Status" ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.PLAYER_DEFEND;
                            EnemyMoveExecuted = true;
                            battleScreen.BattleAssets.IsTransitioning = true;
                        }
                    }
                }
            } else if (PokemonFainted && State == FightState.ENEMY_FAINT && Battle.PlayerPokemon.Pokemon.Level < 100)
            {
                if (!EXPGainApplied)
                {
                    
                    GainedEXP = calcualteEXP(Battle.EnemyPokemon.Pokemon, Battle.IsWild, false, 1) * 100;
                    Battle.PlayerPokemon.Pokemon.CurrentEXP += GainedEXP;
                    int oldMaxHP = Battle.PlayerPokemon.Pokemon.Stats.HP;
                    while (Battle.PlayerPokemon.Pokemon.CurrentEXP >= Battle.PlayerPokemon.Pokemon.NextLevelEXP)
                    {
                        Battle.PlayerPokemon.Pokemon.Level++;
                        LevelUp = true;
                        if (Battle.PlayerPokemon.Pokemon.Level == 100)
                        {
                            GainedEXP -= Battle.PlayerPokemon.Pokemon.CurrentEXP - Battle.PlayerPokemon.Pokemon.CurrentLevelEXP;
                            Battle.PlayerPokemon.Pokemon.CurrentEXP = Battle.PlayerPokemon.Pokemon.CurrentLevelEXP;
                            break;
                        }
                    }

                    Battle.PlayerPokemon.Pokemon.Stats = PokemonManager.Instance.GenerateStatList(Battle.PlayerPokemon.Pokemon);
                    Battle.PlayerPokemon.Pokemon.CurrentHP += Battle.PlayerPokemon.Pokemon.Stats.HP - oldMaxHP;
                    EXPGainApplied = true;
                }
            }


        }

        bool playerFirstMover(BattlePokemon playerPokemon, BattlePokemon enemyPokemon)
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

        bool useMove(BattlePokemon user, BattlePokemon defender, Move move)
        {
            user.Pokemon.MoveNames[move.Name] -= 1;

            if (!move.Self && !moveHit(move.Accuracy, user, defender)) return false;

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
                int StageChange;
                int OldStat;
                if (move.Self)
                {
                    // get stat stage being changed and check if it's already 6 or 5 in order to calculate the new stage change and see if it can still be increased
                    OldStat = user.GetStat(move.Stat);
                    Stat = user.AdjustTempStat(move.Stat, move.StageChange);
                    StageChange = user.GetStat(move.Stat) - OldStat;
                } else
                {
                    OldStat = defender.GetStat(move.Stat);
                    Stat = defender.AdjustTempStat(move.Stat, move.StageChange);
                    StageChange = defender.GetStat(move.Stat) - OldStat;
                    Console.WriteLine("Attack: " + defender.TempAttack);
                }

                SharplyStat = Math.Abs(StageChange) > 1 ? true : false;
                StatStageIncrease = move.StageChange > 0 ? true : false;
                StageMaxed = StageChange == 0 ? true : false;
            }


            defender.Pokemon.CurrentHP = defender.Pokemon.CurrentHP - damage > 0 ? defender.Pokemon.CurrentHP - damage : 0;
            Console.WriteLine("Defender HP: " + defender.Pokemon.CurrentHP);
            return true;
        }

        bool enemyUseMove(BattlePokemon enemyPokemon, BattlePokemon playerPokemon)
        {
            Random random = new Random();
            Move move = MoveManager.Instance.GetMove(enemyPokemon.Pokemon.MoveNames.Keys.ElementAt(random.Next(enemyPokemon.Pokemon.MoveNames.Count)));
            EnemyMoveOption = move;
            return useMove(enemyPokemon, playerPokemon, move);
        }

        int calculateDamage(int level, int power, int attack, int defense, List<Type> userTypes, Type moveType, List<Type> defenderTypes)
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

        bool moveHit(int moveAccuracy, BattlePokemon attacker, BattlePokemon defender)
        {
            int accuracyStage = attacker.AccuracyStage - defender.EvasionStage;

            Random random = new Random();
             
            int moveHit = random.Next(100) + 1;

            float accuracyModified = moveAccuracy * attacker.StatStageMultiplier(accuracyStage);
            
            return moveHit <= accuracyModified;
        }

        // add exp share calculation
        int calcualteEXP(CustomPokemon faintedPokemon, bool wild, bool luckyEgg, int numOfPokemonNotFained)
        {
            float trainerMult = !wild ? 1.5f : 1f;
            float eggMult = luckyEgg ? 1.5f : 1f;

            return (int)(trainerMult * faintedPokemon.Pokemon.EXPYield * faintedPokemon.Level * eggMult / (numOfPokemonNotFained * 7));
        }

        public static void EndBattle()
        {
            Battle.InBattle = false;
            BattleAssets.FromMenu = false;
            BattleMenu.SavedItemNumber = 0;
            MoveMenu.SavedItemNumber = 0;
        }

    }
}
