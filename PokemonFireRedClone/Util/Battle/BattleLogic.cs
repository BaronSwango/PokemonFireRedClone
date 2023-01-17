using System;
using System.Collections.Generic;
using System.Linq;

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

        private bool playerFirst;
        private int escapeAttempts;

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
        public bool EXPGainApplied;
        public bool Crit;
        public bool PokemonFainted;
        public bool StartSequence;
        public bool LevelUp;
        public bool MoveLanded;
        public bool SharplyStat;
        public bool StatStageIncrease;
        public bool StageMaxed;
        public bool PlayerMoveExecuted;
        public bool EnemyMoveExecuted;
        public bool EscapeWildBattle;

        // detects whether player is switching pokemon
        public static bool PlayerShift;
        public static Battle Battle { get; private set; }

        public BattleLogic(Trainer trainer)
        {
            if (Battle == null || !Battle.InBattle)
                Battle = trainer != null ? new Battle(trainer, trainer.Pokemon.ToArray())
                    : new Battle(trainer, GenerateWildPokemon());
            
            
            PlayerMoveUsed = false;
            PlayerHasMoved = false;
            EnemyHasMoved = false;
            Crit = false;
            SuperEffective = false;
            NotVeryEffective = false;
            NoEffect = false;
            MoveLanded = true;
            StartSequence = false;
            playerFirst = false;
            PokemonFainted = false;
            EXPGainApplied = false;
            State = FightState.NONE;
        }

        public void Update()
        {
            if (EscapeWildBattle)
            {
                if (Battle.IsWild)
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = Escape(Battle.PlayerPokemon, Battle.EnemyPokemon) ? 25 : 24;
                }
                else
                {
                    ScreenManager.Instance.BattleScreen.TextBox.NextPage = 26;
                }

                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.WILD_BATTLE_ESCAPE;
                ScreenManager.Instance.BattleScreen.BattleAssets.Reset();
                EscapeWildBattle = false;
            }

            if (PlayerShift)
            {
                playerFirst = true;
                PlayerMoveExecuted = true;
                PlayerMoveUsed = true;
                PlayerShift = false;
                BattleMenu.SavedItemNumber = 0;
                MoveMenu.SavedItemNumber = 0;
                Battle.SwapPokemon(PokemonMenu.SelectedIndex);
                ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.POKEMON_SWITCH;
                ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new PokemonSwitchAnimation();
                ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                ScreenManager.Instance.BattleScreen.TextBox.NextPage = 19;
                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            }
            else if (StartSequence)
            {
                playerFirst = PlayerFirstMover(Battle.PlayerPokemon, Battle.EnemyPokemon);
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
                            MoveLanded = EnemyUseMove(Battle.EnemyPokemon, Battle.PlayerPokemon);
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = EnemyMoveOption.Category == "Status"
                                ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = EnemyMoveOption.Category == "Status"
                                ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.PLAYER_DEFEND;
                            EnemyMoveExecuted = true;
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 5;
                            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!PlayerMoveExecuted)
                        {
                            MoveLanded = UseMove(Battle.PlayerPokemon, Battle.EnemyPokemon, PlayerMoveOption);
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = PlayerMoveOption.Category == "Status"
                                ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = PlayerMoveOption.Category == "Status"
                                ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.ENEMY_DEFEND;
                            PlayerMoveExecuted = true;
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                        }
                    }
                }
                else
                {
                    if (EnemyHasMoved)
                    {
                        if (!PlayerMoveExecuted)
                        {
                            MoveLanded = UseMove(Battle.PlayerPokemon, Battle.EnemyPokemon, PlayerMoveOption);
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = PlayerMoveOption.Category == "Status"
                                ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = PlayerMoveOption.Category == "Status"
                                ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.ENEMY_DEFEND;
                            PlayerMoveExecuted = true;
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 5;
                            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                        }
                    }
                    else
                    {
                        if (!EnemyMoveExecuted)
                        {
                            MoveLanded = EnemyUseMove(Battle.EnemyPokemon, Battle.PlayerPokemon);
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = EnemyMoveOption.Category == "Status"
                                ? BattleAssets.BattleState.STATUS_ANIMATION : BattleAssets.BattleState.DAMAGE_ANIMATION;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = EnemyMoveOption.Category == "Status"
                                ? new StatusAnimation() : new DamageAnimation();
                            State = FightState.PLAYER_DEFEND;
                            EnemyMoveExecuted = true;
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                        }
                    }
                }
            } else if (PokemonFainted && State == FightState.ENEMY_FAINT)
            {

                if (Battle.PlayerPokemon.Pokemon.Level < 100 && !EXPGainApplied)
                {
                    
                    GainedEXP = CalcualteEXP(Battle.EnemyPokemon.Pokemon, Battle.IsWild, false, 1);
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

                if (!Battle.IsEnded)
                    Battle.ChooseNewOpponentPokemon();

            }

        }

        private CustomPokemon GenerateWildPokemon()
        {
            Random random = new();

            int pokemonIndex = 0;
            int num = random.Next(100);
            float curr = 0;

            foreach (Area.PokemonRange range in Player.PlayerJsonObject.CurrentArea.Ranges)
            {
                if (num < (float) (range.EncounterRate + curr))
                {
                    pokemonIndex = Player.PlayerJsonObject.CurrentArea.Ranges.IndexOf(range);
                    break;
                }

                curr += range.EncounterRate;
            }

            return PokemonManager.Instance.CreatePokemon(PokemonManager.Instance.GetPokemon(Player.PlayerJsonObject.CurrentArea.Ranges[pokemonIndex].PokemonName),
                Player.PlayerJsonObject.CurrentArea.Ranges[pokemonIndex].Levels[random.Next(Player.PlayerJsonObject.CurrentArea.Ranges[pokemonIndex].Levels.Count)]);
        }

        private bool Escape(BattlePokemon playerPokemon, BattlePokemon enemyPokemon)
        {
            Random random = new();

            int escapeVal = (playerPokemon.Pokemon.Stats.Speed * 128 / enemyPokemon.Pokemon.Stats.Speed) + (30 * ++escapeAttempts);

            if (escapeVal > 255)
                return true;

            int oddsEscape = escapeVal % 256;
            int randomNum = random.Next(256);

            return randomNum < oddsEscape;
        }

        private bool PlayerFirstMover(BattlePokemon playerPokemon, BattlePokemon enemyPokemon)
        {
            if (playerPokemon.TempSpeed == enemyPokemon.TempSpeed)
            {
                Random random = new();
                int first = random.Next(2);
                if (first == 0)
                    return true;
            }
            return playerPokemon.TempSpeed > enemyPokemon.TempSpeed;
        }

        private bool UseMove(BattlePokemon user, BattlePokemon defender, Move move)
        {
            user.Pokemon.MovePP[move.Name] -= 1;

            if (!move.Self && !MoveHit(move.Accuracy, user, defender)) return false;

            int damage = 0;

            if (move.Category == "Special")
                damage = CalculateDamage(
                    user.Pokemon.Level,
                    move.Power,
                    user.TempSpecialAttack,
                    defender.TempSpecialDefense,
                    user.Pokemon.Pokemon.Types,
                    move.Type,
                    defender.Pokemon.Pokemon.Types);
            else if (move.Category == "Physical")
                damage = CalculateDamage(
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

                SharplyStat = Math.Abs(StageChange) > 1;
                StatStageIncrease = move.StageChange > 0;
                StageMaxed = StageChange == 0;
            }


            defender.Pokemon.CurrentHP = defender.Pokemon.CurrentHP - damage > 0 ? defender.Pokemon.CurrentHP - damage : 0;
            Console.WriteLine("Defender HP: " + defender.Pokemon.CurrentHP);
            return true;
        }

        private bool EnemyUseMove(BattlePokemon enemyPokemon, BattlePokemon playerPokemon)
        {
            Random random = new();
            Move move = MoveManager.Instance.GetMove(enemyPokemon.Pokemon.MovePP.Keys.ElementAt(random.Next(enemyPokemon.Pokemon.MovePP.Count)));
            EnemyMoveOption = move;
            return UseMove(enemyPokemon, playerPokemon, move);
        }

        private int CalculateDamage(int level, int power, int attack, int defense, List<Type> userTypes, Type moveType, List<Type> defenderTypes)
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

            Random rand = new();
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

        private bool MoveHit(int moveAccuracy, BattlePokemon attacker, BattlePokemon defender)
        {
            int accuracyStage = attacker.AccuracyStage - defender.EvasionStage;

            Random random = new();
             
            int moveHit = random.Next(100) + 1;

            float accuracyModified = moveAccuracy * attacker.StatStageMultiplier(accuracyStage);
            
            return moveHit <= accuracyModified;
        }

        // add exp share calculation
        private int CalcualteEXP(CustomPokemon faintedPokemon, bool wild, bool luckyEgg, int numOfPokemonNotFained)
        {
            float trainerMult = !wild ? 1.5f : 1f;
            float eggMult = luckyEgg ? 1.5f : 1f;

            return (int)(trainerMult * faintedPokemon.Pokemon.EXPYield * faintedPokemon.Level * eggMult / (numOfPokemonNotFained * 7));
        }

        public static void EndBattle()
        {
            Battle.InBattle = false;
            BattleMenu.SavedItemNumber = 0;
            MoveMenu.SavedItemNumber = 0;
            if (!Battle.IsWild)
            {
                Player.PlayerJsonObject.Money += Battle.Trainer.Reward;
                Player.PlayerJsonObject.TrainersDefeated.Add(Battle.Trainer.ID);
            }
        }

    }
}
