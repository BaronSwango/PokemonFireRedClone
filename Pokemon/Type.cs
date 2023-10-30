namespace PokemonFireRedClone
{
    public enum Type
    {
        FIRE, ICE, ELECTRIC, WATER, GRASS, PSYCHIC, DARK, GHOST, NORMAL, BUG, POISON, GROUND, ROCK, FLYING, DRAGON, STEEL, FIGHTING
    }

    public static class TypeProperties
    {
        public static Type TypeFromName(string typeName)
        {
            return typeName switch
            {
                "Fire" => Type.FIRE,
                "Ice" => Type.ICE,
                "Electric" => Type.ELECTRIC,
                "Water" => Type.WATER,
                "Grass" => Type.GRASS,
                "Psychic" => Type.PSYCHIC,
                "Dark" => Type.DARK,
                "Ghost" => Type.GHOST,
                "Normal" => Type.NORMAL,
                "Bug" => Type.BUG,
                "Poison" => Type.POISON,
                "Ground" => Type.GROUND,
                "Rock" => Type.ROCK,
                "Flying" => Type.FLYING,
                "Dragon" => Type.DRAGON,
                "Steel" => Type.STEEL,
                "Fighting" => Type.FIGHTING,
                _ => 0,
            };
        }

        public static string Name(Type type)
        {
            return type switch
            {
                Type.FIRE => "Fire",
                Type.ICE => "Ice",
                Type.ELECTRIC => "Electric",
                Type.WATER => "Water",
                Type.GRASS => "Grass",
                Type.PSYCHIC => "Psychic",
                Type.DARK => "Dark",
                Type.GHOST => "Ghost",
                Type.NORMAL => "Normal",
                Type.BUG => "Bug",
                Type.POISON => "Poison",
                Type.GROUND => "Ground",
                Type.ROCK => "Rock",
                Type.FLYING => "Flying",
                Type.DRAGON => "Dragon",
                Type.STEEL => "Steel",
                Type.FIGHTING => "Fighting",
                _ => "",
            };
        }

        public static Image ImageOf(Type type)
        {
            Image image = new()
            {
                Path = "Types/" + Name(type)
            };
            return image;
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
}
