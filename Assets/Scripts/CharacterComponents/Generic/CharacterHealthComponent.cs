using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : MonoBehaviour, ICharacterComponent
{
    public int health;
    public int armor;
    public bool isPlayer;
    bool[] b = { };

    public void Damage(int amount)
    {
        health -= (int)Mathf.Clamp(amount-armor, 1,Mathf.Infinity);
        if(health <= 0)
        {
            if(isPlayer)
            {
                EventManager.TriggerEvent("death");
            }
            else
            {
                GlobalVariables.globalObjects.Add("last_enemy", this.gameObject);
                EventManager.TriggerEvent("enemy_death");
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("dead");
        Destroy(this.gameObject, 0.1f);//todo: death effects? object pool?
    }
}
