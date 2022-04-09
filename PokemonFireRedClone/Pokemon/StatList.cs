using System;
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
            switch(stat)
            {
                case "A":
                    return Attack;
                case "D":
                    return Defense;
                case "SA":
                    return SpecialAttack;
                case "SD":
                    return SpecialDefense;
                case "S":
                    return Speed;
                default:
                    return 0;
            }
        }

    }
}
