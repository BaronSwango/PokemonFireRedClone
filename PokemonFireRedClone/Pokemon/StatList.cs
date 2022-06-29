namespace PokemonFireRedClone
{
    public class StatList
    {

        public int HP, HPIV, HPEV;
        public int Attack, AttackIV, AttackEV;
        public int Defense, DefenseIV, DefenseEV;
        public int SpecialAttack, SpecialAttackIV, SpecialAttackEV;
        public int SpecialDefense, SpecialDefenseIV, SpecialDefenseEV;
        public int Speed, SpeedIV, SpeedEV;

        public int GetStat(string stat)
        {
            return stat switch
            {
                "A" => Attack,
                "D" => Defense,
                "SA" => SpecialAttack,
                "SD" => SpecialDefense,
                "S" => Speed,
                _ => 0,
            };
        }

    }
}
