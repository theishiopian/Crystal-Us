using System.Collections.Generic;
using UnityEngine;

//global variable storage
//use for things that everyone needs access to
public static class GlobalVariables
{
    public static Dictionary<string, Object> globalObjects = new Dictionary<string, Object>();//player can be stored here

    public static Vector3 spawnPos = new Vector3();
}
