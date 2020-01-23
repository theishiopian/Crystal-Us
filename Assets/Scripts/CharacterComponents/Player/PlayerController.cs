﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMoverComponent controller;
    private CharacterAnimationController animator;
    private CharacterAttackComponent attack;
    private PlayerHUDComponent hud;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.globalObjects.Add("player", this.gameObject);
        controller = this.gameObject.GetComponent<CharacterMoverComponent>();
        animator = this.gameObject.GetComponent<CharacterAnimationController>();
        attack = this.gameObject.GetComponent<CharacterAttackComponent>();
        hud = this.gameObject.GetComponent<PlayerHUDComponent>();
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        float x = Input.GetAxis(Horizontal);
        float y = Input.GetAxis(Vertical);

        int i = animator.index;
        int ti = 0;
        //TODO replace all this grox shit with a proper animator system
        if (x > 0)
        {
            ti = 3;
        }
        else if (x < 0)
        {
            ti = 1;
        }
        else if (y > 0)
        {
            ti = 2;
        }
        else if (y < 0)
        {
            ti = 0;
        }

        Vector2 movement = new Vector2(x, y);
        controller.Move(movement);

        if (ti != i)
        {
            i = ti;
            animator.SetSprite(i);
        }
    }

    float attackPower = 0;
    bool attacked = true;
    

    
    void Attack()
    {
        //Debug.Log(arrowLevel);
        if(Input.GetMouseButton(0))//TODO replace with axis
        {
            hud.AttackHUDInit(attackPower);
            
            attacked = false;
            attackPower += Time.deltaTime;
            
            if(attackPower > 2f)//if you can get clamp to work here, go for it!
            {
                attackPower = 2f;//TODO: implement sword projectile
            }

            //Debug.Log("attack power: "+attackPower);
        }
        else if(!attacked)
        {
            //get direction and normalize
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-this.transform.position;

            float x = direction.x;
            float y = direction.y;

            if(Mathf.Abs(x)>Mathf.Abs(y))
            {
                direction.x = Mathf.Sign(x);
                direction.y = 0;
            }
            else
            {
                direction.y = Mathf.Sign(y);
                direction.x = 0;
            }

            attacked = true;
            attack.Attack(direction, attackPower);
            attackPower = 0;
            hud.AttackHUDReset();
        }

        hud.AttackHUDLevel(attackPower);
    }
}
