﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : MonoBehaviour, ICharacterComponent
{
    public int maxHealth;
    public int health;
    public int armor;

    Animator anim;
    Animator enemyAnim;

    private bool isPlayer;

    void Start()
    {
        health = maxHealth;
        isPlayer = (this.gameObject.tag.Equals("Player"));
        anim = this.gameObject.GetComponent<Animator>();

    }

    void Awake()
    {

    }

    public void Damage(int amount)
    {
        health -= (int)Mathf.Clamp(amount-armor, 1,Mathf.Infinity);
        
        Object cache;
        GlobalVariables.globalObjects.TryGetValue("last_enemy", out cache);
        if (cache is GameObject && cache != null)
        {
            GameObject c = (GameObject)cache;
            enemyAnim = c.GetComponent<Animator>();
            enemyAnim.SetBool("IsDamaged", true);
        }
        if (health > 0 && isPlayer)
        {
            anim.SetBool("IsDamaged", true);
            //PlayerStats.Health = health;
        }
        else if(health <= 0)
        {
            if(isPlayer)
            {
                EventManager.TriggerEvent("death");
            }
            else
            {
                GlobalVariables.globalObjects["last_enemy"] = this.gameObject;
                EventManager.TriggerEvent("enemy_death");
                Die();
            }
        }
        if (anim != null)
        {
            anim.SetBool("IsDamaged", false);
        }
    }

    void Die()
    {
        Destroy(this.gameObject, 0.1f);//todo: death effects? object pool?
    }
}
