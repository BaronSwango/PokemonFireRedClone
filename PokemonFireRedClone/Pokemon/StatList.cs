using System;
namespace PokemonFireRedClone
{
    public class StatList
    {

        public int HP;
        public int HPIV;
        public int HPEV;
        public int Attack;
        public int AttackIV;
        public int AttackEV;
        public int Defense;
        public int DefenseIV;
        public int DefenseEV;
        public int SpecialAttack;
        public int SpecialAttackIV;
        public int SpecialAttackEV;
        public int SpecialDefense;
        public int SpecialDefenseIV;
        public int SpecialDefenseEV;
        public int Speed;
        public int SpeedIV;
        public int SpeedEV;

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
