using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static Dictionary<string, Object> globalObjects = new Dictionary<string, Object>();//player can be stored here
    public static Dictionary<string, int> globalIntegers = new Dictionary<string, int>();//score etc
}
