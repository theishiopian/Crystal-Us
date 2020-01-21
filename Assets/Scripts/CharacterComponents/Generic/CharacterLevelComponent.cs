using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterLevelComponent : MonoBehaviour, ICharacterComponent
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
                Object cache;
                GlobalVariables.globalObjects.TryGetValue("last_enemy", out cache);
                
                if(cache is GameObject && cache != null)
                {
                    GameObject c = (GameObject)cache;
                    
                    if(c.GetComponent<CharacterHealthComponent>())
                    {
                        level += c.GetComponent<CharacterHealthComponent>().health/2;
                        XP = 0;
                        levelUp *= 2;
                    }
                }
            }
        }
        else
        {
            //monsters dont need to level up
        }
    }
}
