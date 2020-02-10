using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int Health { get; set; } = 30;

    public static int Maxhealth { get; set; } = 30;

    public static int Xp { get; set; } = 0;

    public static int Level { get; set; } = 1;

    public static int Levelup { get; set; } = 30;

    public static bool HasKey { get; set; } = false;

    public static bool HasNecklace { get; set; } = false;

    public static bool HasSword { get; set; } = false;
}
