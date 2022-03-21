using System;
namespace PokemonFireRedClone
{
    public class BattlePokemon
    {
        public CustomPokemon Pokemon;
        public int AttackStage;
        public int DefenseStage;
        public int SpecialAttackStage;
        public int SpecialDefenseStage;
        public int SpeedStage;

        public int AccuracyStage;
        public int EvasionStage;

        public BattlePokemon(CustomPokemon pokemon)
        {
            Pokemon = pokemon;
        }

        public int TempAttack
        {
            get { return (int) (Pokemon.Stats.Attack * StatStageMultiplier(AttackStage)); }
            private set { }
        }

        public int TempDefense
        {
            get { return (int) (Pokemon.Stats.Defense * StatStageMultiplier(DefenseStage)); }
            private set { }
        }

        public int TempSpecialAttack
        {
            get { return (int) (Pokemon.Stats.SpecialAttack * StatStageMultiplier(SpecialAttackStage)); }
            private set { }
        }

        public int TempSpecialDefense
        {
            get { return (int) (Pokemon.Stats.SpecialDefense * StatStageMultiplier(SpecialDefenseStage)); }
            private set { }
        }

        public int TempSpeed
        {
            get { return (int) (Pokemon.Stats.Speed * StatStageMultiplier(SpeedStage)); }
            private set { }
        }

        private float StatStageMultiplier(int stage)
        {
            switch(stage)
            {
                case -6:
                    return 2f / 8f;
                case -5:
                    return 2f / 7f;
                case -4:
                    return 2f / 6f;
                case -3:
                    return 2f / 5f;
                case -2:
                    return 2f / 4f;
                case -1:
                    return 2f / 3f;
                case 1:
                    return 3f / 2f;
                case 2:
                    return 4f / 2f;
                case 3:
                    return 5f / 2f;
                case 4:
                    return 6f / 2f;
                case 5:
                    return 7f / 2f;
                case 6:
                    return 8f / 2f;
                default:
                    return 1;
            }
        }

        public string AdjustTempStat(string stat, int adjustment)
        {
            switch(stat)
            {
                case "A":
                    AttackStage += adjustment;
                    return "ATTACK";
                case "D":
                    DefenseStage += adjustment;
                    return "DEFENSE";
                case "SA":
                    SpecialAttackStage += adjustment;
                    return "SP. ATTACK";
                case "SD":
                    SpecialDefenseStage += adjustment;
                    return "SP. DEFENSE";
                case "S":
                    SpeedStage += adjustment;
                    return "SPEED";
                case "ACC":
                    AccuracyStage += adjustment;
                    return "accuracy";
                case "EVAS":
                    EvasionStage += adjustment;
                    return "evasiveness";
            }
            return "";
        }

    }
}
