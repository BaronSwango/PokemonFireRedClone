using System;
namespace PokemonFireRedClone
{
    public static class TypeProperties
    {
        public static Type TypeFromName(string typeName)
        {
            switch(typeName)
            {
                case "Fire":
                    return Type.FIRE;
                case "Ice":
                    return Type.ICE;
                case "Electric":
                    return Type.ELECTRIC;
                case "Water":
                    return Type.WATER;
                case "Grass":
                    return Type.GRASS;
                case "Psychic":
                    return Type.PSYCHIC;
                case "Dark":
                    return Type.DARK;
                case "Ghost":
                    return Type.GHOST;
                case "Normal":
                    return Type.NORMAL;
                case "Bug":
                    return Type.BUG;
                case "Poison":
                    return Type.POISON;
                case "Ground":
                    return Type.GROUND;
                case "Rock":
                    return Type.ROCK;
                case "Flying":
                    return Type.FLYING;
                case "Dragon":
                    return Type.DRAGON;
                case "Steel":
                    return Type.STEEL;
                case "Fighting":
                    return Type.FIGHTING;
                default:
                    return 0;
            }
        }

        public static string Name(Type type)
        {
            switch (type)
            {
                case Type.FIRE:
                    return "Fire";
                case Type.ICE:
                    return "Ice";
                case Type.ELECTRIC:
                    return "Electric";
                case Type.WATER:
                    return "Water";
                case Type.GRASS:
                    return "Grass";
                case Type.PSYCHIC:
                    return "Psychic";
                case Type.DARK:
                    return "Dark";
                case Type.GHOST:
                    return "Ghost";
                case Type.NORMAL:
                    return "Normal";
                case Type.BUG:
                    return "Bug";
                case Type.POISON:
                    return "Poison";
                case Type.GROUND:
                    return "Ground";
                case Type.ROCK:
                    return "Rock";
                case Type.FLYING:
                    return "Flying";
                case Type.DRAGON:
                    return "Dragon";
                case Type.STEEL:
                    return "Steel";
                case Type.FIGHTING:
                    return "Fighting";
                default:
                    return "";
            }
        }

        public static float DamageMultiplier(Type attacker, Type defender)
        {
            switch(attacker)
            {
                case Type.NORMAL:
                    switch(defender)
                    {
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.ROCK:
                        case Type.STEEL:
                            return 0.5f;
                        case Type.GHOST:
                            return 0;
                    }
                    break;
                case Type.FIRE:
                    switch(defender)
                    {
                        case Type.GRASS:
                        case Type.ICE:
                        case Type.BUG:
                        case Type.STEEL:
                            return 2;
                        case Type.NORMAL:
                        case Type.ELECTRIC:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.GHOST:
                        case Type.DARK:
                            return 1;
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.ROCK:
                        case Type.DRAGON:
                            return 0.5f;
                    }
                    break;
                case Type.WATER:
                    switch(defender)
                    {
                        case Type.FIRE:
                        case Type.GROUND:
                        case Type.ROCK:
                            return 2;
                        case Type.NORMAL:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.GHOST:
                        case Type.DARK:
                        case Type.STEEL:
                            return 1;
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.DRAGON:
                            return 0.5f;
                    }
                    break;
                case Type.GRASS:
                    switch(defender)
                    {
                        case Type.WATER:
                        case Type.GROUND:
                        case Type.ROCK:
                            return 2;
                        case Type.NORMAL:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.PSYCHIC:
                        case Type.GHOST:
                        case Type.DARK:
                            return 1;
                        case Type.FIRE:
                        case Type.GRASS:
                        case Type.POISON:
                        case Type.FLYING:
                        case Type.BUG:
                        case Type.DRAGON:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.ELECTRIC:
                    switch(defender) {
                        case Type.WATER:
                        case Type.FLYING:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.GHOST:
                        case Type.DARK:
                        case Type.STEEL:
                            return 1;
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.DRAGON:
                            return 0.5f;
                        case Type.GROUND:
                            return 0;
                    }
                    break;
                case Type.ICE:
                    switch(defender)
                    {
                        case Type.GRASS:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.DRAGON:
                            return 2;
                        case Type.NORMAL:
                        case Type.ELECTRIC:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.GHOST:
                        case Type.DARK:
                            return 1;
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.ICE:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.FIGHTING:
                    switch(defender)
                    {
                        case Type.NORMAL:
                        case Type.ICE:
                        case Type.ROCK:
                        case Type.DARK:
                        case Type.STEEL:
                            return 2;
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.FIGHTING:
                        case Type.GROUND:
                        case Type.DRAGON:
                            return 1;
                        case Type.POISON:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                            return 0.5f;
                        case Type.GHOST:
                            return 0;
                    }
                    break;
                case Type.POISON:
                    switch(defender)
                    {
                        case Type.GRASS:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.ROCK:
                        case Type.GHOST:
                            return 0.5f;
                        case Type.STEEL:
                            return 0;
                    }
                    break;
                case Type.GROUND:
                    switch (defender)
                    {
                        case Type.FIRE:
                        case Type.ELECTRIC:
                        case Type.POISON:
                        case Type.ROCK:
                        case Type.STEEL:
                            return 2;
                        case Type.NORMAL:
                        case Type.WATER:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.GROUND:
                        case Type.PSYCHIC:
                        case Type.GHOST:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.GRASS:
                        case Type.BUG:
                            return 0.5f;
                        case Type.FLYING:
                            return 0;
                    }
                    break;
                case Type.FLYING:
                    switch (defender)
                    {
                        case Type.GRASS:
                        case Type.FIGHTING:
                        case Type.BUG:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.ICE:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.GHOST:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.ELECTRIC:
                        case Type.ROCK:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.PSYCHIC:
                    switch (defender)
                    {
                        case Type.FIGHTING:
                        case Type.POISON:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.GHOST:
                        case Type.DRAGON:
                            return 1;
                        case Type.PSYCHIC:
                        case Type.STEEL:
                            return 0.5f;
                        case Type.DARK:
                            return 0;
                    }
                    break;
                case Type.BUG:
                    switch (defender)
                    {
                        case Type.GRASS:
                        case Type.PSYCHIC:
                        case Type.DARK:
                            return 2;
                        case Type.NORMAL:
                        case Type.WATER:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.GROUND:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.DRAGON:
                            return 1;
                        case Type.FIRE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.FLYING:
                        case Type.GHOST:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.ROCK:
                    switch (defender)
                    {
                        case Type.FIRE:
                        case Type.ICE:
                        case Type.FLYING:
                        case Type.BUG:
                            return 2;
                        case Type.NORMAL:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.POISON:
                        case Type.PSYCHIC:
                        case Type.ROCK:
                        case Type.GHOST:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.FIGHTING:
                        case Type.GROUND:
                        case Type.STEEL:
                            return 0.5f;

                    }
                    break;
                case Type.GHOST:
                    switch (defender)
                    {
                        case Type.PSYCHIC:
                        case Type.GHOST:
                            return 2;
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.DRAGON:
                            return 1;
                        case Type.DARK:
                        case Type.STEEL:
                            return 0.5f;
                        case Type.NORMAL:
                            return 0;
                    }
                    break;
                case Type.DRAGON:
                    switch (defender)
                    {
                        case Type.DRAGON:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.GHOST:
                        case Type.DARK:
                            return 1;
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.DARK:
                    switch (defender)
                    {
                        case Type.PSYCHIC:
                        case Type.GHOST:
                            return 2;
                        case Type.NORMAL:
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.ICE:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.BUG:
                        case Type.ROCK:
                        case Type.DRAGON:
                            return 1;
                        case Type.FIGHTING:
                        case Type.DARK:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                case Type.STEEL:
                    switch (defender)
                    {
                        case Type.ICE:
                        case Type.ROCK:
                            return 2;
                        case Type.NORMAL:
                        case Type.GRASS:
                        case Type.ELECTRIC:
                        case Type.FIGHTING:
                        case Type.POISON:
                        case Type.GROUND:
                        case Type.FLYING:
                        case Type.PSYCHIC:
                        case Type.BUG:
                        case Type.GHOST:
                        case Type.DRAGON:
                        case Type.DARK:
                            return 1;
                        case Type.FIRE:
                        case Type.WATER:
                        case Type.STEEL:
                            return 0.5f;
                    }
                    break;
                default:
                    return 1;
            }
            return 1;
        }
    }

    public enum Type
    {
        FIRE, ICE, ELECTRIC, WATER, GRASS, PSYCHIC, DARK, GHOST, NORMAL, BUG, POISON, GROUND, ROCK, FLYING, DRAGON, STEEL, FIGHTING
    }
}
