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
            get { return (int)(Pokemon.Stats.Attack * StatStageMultiplier(AttackStage)); }
            private set { }
        }

        public int TempDefense
        {
            get { return (int)(Pokemon.Stats.Defense * StatStageMultiplier(DefenseStage)); }
            private set { }
        }

        public int TempSpecialAttack
        {
            get { return (int)(Pokemon.Stats.SpecialAttack * StatStageMultiplier(SpecialAttackStage)); }
            private set { }
        }

        public int TempSpecialDefense
        {
            get { return (int)(Pokemon.Stats.SpecialDefense * StatStageMultiplier(SpecialDefenseStage)); }
            private set { }
        }

        public int TempSpeed
        {
            get { return (int)(Pokemon.Stats.Speed * StatStageMultiplier(SpeedStage)); }
            private set { }
        }

        public float StatStageMultiplier(int stage)
        {
            return stage switch
            {
                -6 => 2f / 8f,
                -5 => 2f / 7f,
                -4 => 2f / 6f,
                -3 => 2f / 5f,
                -2 => 2f / 4f,
                -1 => 2f / 3f,
                0 => 1,
                1 => 3f / 2f,
                2 => 4f / 2f,
                3 => 5f / 2f,
                4 => 6f / 2f,
                5 => 7f / 2f,
                6 => 8f / 2f,
                _ => 1,
            };
        }

        public int GetStat(string stat)
        {
            return stat switch
            {
                "A" => AttackStage,
                "D" => DefenseStage,
                "SA" => SpecialAttackStage,
                "SD" => SpecialDefenseStage,
                "S" => SpeedStage,
                "ACC" => AccuracyStage,
                "EVAS" => EvasionStage,
                _ => 0,
            };
        }

        public string AdjustTempStat(string stat, int adjustment)
        {
            switch(stat)
            {
                case "A":
                    AttackStage += adjustment;
                    if (AttackStage > 6)
                        AttackStage = 6;
                    else if (AttackStage < -6)
                        AttackStage = -6;
                    return "ATTACK";
                case "D":
                    DefenseStage += adjustment;
                    if (DefenseStage > 6)
                        DefenseStage = 6;
                    else if (DefenseStage < -6)
                        DefenseStage = -6;
                    return "DEFENSE";
                case "SA":
                    SpecialAttackStage += adjustment;
                    if (SpecialAttackStage > 6)
                        SpecialAttackStage = 6;
                    else if (SpecialAttackStage < -6)
                        SpecialAttackStage = -6;
                    return "SP.   ATK";
                case "SD":
                    SpecialDefenseStage += adjustment;
                    if (SpecialDefenseStage > 6)
                        SpecialDefenseStage = 6;
                    else if (SpecialDefenseStage < -6)
                        SpecialDefenseStage = -6;
                    return "SP.   DEF";
                case "S":
                    SpeedStage += adjustment;
                    if (SpeedStage > 6)
                        SpeedStage = 6;
                    else if (SpeedStage < -6)
                        SpeedStage = -6;
                    return "SPEED";
                case "ACC":
                    AccuracyStage += adjustment;
                    if (AccuracyStage > 6)
                        AccuracyStage = 6;
                    else if (AccuracyStage < -6)
                        AccuracyStage = -6;
                    return "accuracy";
                case "EVAS":
                    EvasionStage += adjustment;
                    if (EvasionStage > 6)
                        EvasionStage = 6;
                    else if (EvasionStage < -6)
                        EvasionStage = -6;
                    return "evasiveness";
            }
            return "";
        }

    }
}
