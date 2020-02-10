using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterLevelComponent : MonoBehaviour, ICharacterComponent
{ 
    public int level = 1;
    public int XP = 0;

    public int levelUp = 30;
    private bool isPlayer = false;



    void Start()
    {
        EventManager.StartListening("enemy_death", EnemyDeath);
        isPlayer = this.gameObject.CompareTag("Player");
        /*PlayerStats.Xp = XP;
        PlayerStats.Level = level;
        PlayerStats.Levelup = levelUp;*/
    }

    void Awake()
    {
        /*XP = PlayerStats.Xp;
        level = PlayerStats.Level;
        levelUp = PlayerStats.Levelup;*/
    }

    void EnemyDeath()
    {
        if(isPlayer)
        {
            Object cache;
            GlobalVariables.globalObjects.TryGetValue("last_enemy", out cache);
            if (cache is GameObject && cache != null)
            {
                GameObject c = (GameObject)cache;

                if (c.GetComponent<CharacterHealthComponent>())
                {
                    XP += c.GetComponent<CharacterHealthComponent>().maxHealth / 2;
                }
            }
            
            if (XP >= levelUp)
            {
                
                level += 1;
                XP = 0;
                levelUp *= 2;
                
            }

            //store values whenever XP is gained
            /*PlayerStats.Xp = XP;
            PlayerStats.Level = level;
            PlayerStats.Levelup = levelUp;*/

        }
        else
        {
            //monsters dont need to level up
        }
    }

    public int GetNextLevelXP()
    {
        return levelUp;
    }
}
