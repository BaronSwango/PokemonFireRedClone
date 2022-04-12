using System;
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
            ENEMY_STATUS
        }

        public FightState State;
        public BattlePokemon playerPokemon;
        public BattlePokemon enemyPokemon;
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
        public bool MoveMiss;
        public bool SharplyStat;
        public bool StatStageIncrease;
        public bool StageMaxed;
        public bool PlayerMoveExecuted;
        public bool EnemyMoveExecuted;
        bool playerFirst;

        public BattleLogic(BattleScreen battleScreen)
        {
            playerPokemon = new BattlePokemon(Player.PlayerJsonObject.PokemonInBag[0]);
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
            State = FightState.NONE;
        }

        public void Update(GameTime gameTime, BattleScreen battleScreen)
        {

            if (StartSequence)
            {
                playerFirst = playerFirstMover(playerPokemon, enemyPokemon);
                StartSequence = false;
            }

            // CHECK IF MOVE IS STATUS AND SET BATTLE ANIMATIONS STATE TO STATUS ANIMATION
            if (!PokemonFainted && PlayerMoveUsed)
            {
                if (playerFirst)
                {
                    if (PlayerHasMoved)
                    {
                        if (!EnemyMoveExecuted)
                        {
                            enemyUseMove(enemyPokemon, playerPokemon);
                            battleScreen.BattleAnimations.state = EnemyMoveOption.Category == "Status" ? BattleAnimations.BattleState.STATUS_ANIMATION : BattleAnimations.BattleState.DAMAGE_ANIMATION;
                            State = FightState.PLAYER_DEFEND;
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
                            battleScreen.BattleAnimations.state = PlayerMoveOption.Category == "Status" ? BattleAnimations.BattleState.STATUS_ANIMATION : BattleAnimations.BattleState.DAMAGE_ANIMATION;
                            State = FightState.ENEMY_DEFEND;
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
                            battleScreen.BattleAnimations.state = PlayerMoveOption.Category == "Status" ? BattleAnimations.BattleState.STATUS_ANIMATION : BattleAnimations.BattleState.DAMAGE_ANIMATION;
                            State = FightState.ENEMY_DEFEND;
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
                            battleScreen.BattleAnimations.state = EnemyMoveOption.Category == "Status" ? BattleAnimations.BattleState.STATUS_ANIMATION : BattleAnimations.BattleState.DAMAGE_ANIMATION;
                            State = FightState.PLAYER_DEFEND;
                            EnemyMoveExecuted = true;
                            battleScreen.BattleAnimations.IsTransitioning = true;
                        }
                    }
                }
            } else if (PokemonFainted && battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_POKEMON_FAINT && Player.PlayerJsonObject.PokemonInBag[0].Level < 100)
            {
                if (!EXPGainApplied)
                {
                    
                    GainedEXP = calcualteEXP(battleScreen.enemyPokemon, BattleScreen.Wild, false, 1) * 100;
                    Player.PlayerJsonObject.PokemonInBag[0].CurrentEXP += GainedEXP;
                    int oldMaxHP = Player.PlayerJsonObject.PokemonInBag[0].Stats.HP;
                    while (Player.PlayerJsonObject.PokemonInBag[0].CurrentEXP >= Player.PlayerJsonObject.PokemonInBag[0].NextLevelEXP)
                    {
                        Player.PlayerJsonObject.PokemonInBag[0].Level++;
                        LevelUp = true;
                        if (Player.PlayerJsonObject.PokemonInBag[0].Level == 100)
                        {
                            GainedEXP -= Player.PlayerJsonObject.PokemonInBag[0].CurrentEXP - Player.PlayerJsonObject.PokemonInBag[0].CurrentLevelEXP;
                            Player.PlayerJsonObject.PokemonInBag[0].CurrentEXP = Player.PlayerJsonObject.PokemonInBag[0].CurrentLevelEXP;
                            break;
                        }
                    }

                    Player.PlayerJsonObject.PokemonInBag[0].Stats = PokemonManager.generateStatList(Player.PlayerJsonObject.PokemonInBag[0]);
                    Player.PlayerJsonObject.PokemonInBag[0].CurrentHP += Player.PlayerJsonObject.PokemonInBag[0].Stats.HP - oldMaxHP;
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
