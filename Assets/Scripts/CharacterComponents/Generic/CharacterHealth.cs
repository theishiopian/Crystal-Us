using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, ICharacterComponent
{
    public int health;
    public bool isPlayer;
    bool[] b = { };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            if(isPlayer)
            {
                EventManager.TriggerEvent("death");
            }
            else
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("dead");
        Destroy(this.gameObject);//todo: death effects? object pool?
    }
}
