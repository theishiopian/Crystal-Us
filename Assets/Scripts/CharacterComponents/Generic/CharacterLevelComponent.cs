using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelComponent : MonoBehaviour
{
    public bool isPlayer;
    public int level = 1;
    public int XP = 0;
    private int levelUp = 30;

    private void Start()
    {
        EventManager.StartListening("enemy_death", EnemyDeath);
    }

    void EnemyDeath()
    {
        if(isPlayer)
        {
            XP += 1;

            if (XP >= levelUp)
            {
                level += 1;
                XP = 0;
                levelUp *= 2;
            }
        }
        else
        {
            //monsters dont need to level up
        }
    }
}
