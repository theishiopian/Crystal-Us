using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private static int health=30, maxhealth=30, xp=0, level=1, levelUp=30;
    private static bool hasKey=false, hasNecklace=false, hasSword=false;

    public static int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public static int Maxhealth
    {
        get
        {
            return maxhealth;
        }
        set
        {
            maxhealth = value;
        }
    }

    public static int Xp
    {
        get
        {
            return xp;
        }
        set
        {
            xp = value;
        }
    }

    public static int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public static int Levelup
    {
        get
        {
            return levelUp;
        }
        set
        {
            levelUp = value;
        }
    }

    public static bool HasKey
    {
        get
        {
            return hasKey;
        }
        set
        {
            hasKey = value;
        }
    }

    public static bool HasNecklace
    {
        get
        {
            return hasNecklace;
        }
        set
        {
            hasNecklace = value;
        }
    }

    public static bool HasSword
    {
        get
        {
            return hasSword;
        }
        set
        {
            hasSword = value;
        }
    }

}
