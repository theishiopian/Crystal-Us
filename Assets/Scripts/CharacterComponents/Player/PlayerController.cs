﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMoverComponent controller;
    private CharacterAttackComponent attack;
    private PlayerHUDComponent hud;
    private PlayerVFXComponent vfx;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.globalObjects.Add("player", this.gameObject);
        controller = this.gameObject.GetComponent<CharacterMoverComponent>();
        attack = this.gameObject.GetComponent<CharacterAttackComponent>();
        hud = this.gameObject.GetComponent<PlayerHUDComponent>();
        vfx = this.gameObject.GetComponent<PlayerVFXComponent>();
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetFloat("AnimSpeed", 1.0f);
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        Dialouge();
        Debug.Log(attackPower);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(x, y);

        animator.SetFloat("MovementHorizontal", movement.x);
        animator.SetFloat("MovementVertical", movement.y);

        if(movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("Speed", 5.0f);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
                
        controller.Move(movement);
   
    }

    private float attackPower = 0;
    private bool hasAttacked = true;

    void Attack()
    {
        //Debug.Log(arrowLevel);
        if(Input.GetMouseButton(0))//TODO replace with axis
        {
            hud.InitAttack(attackPower);
            vfx.InitAttack(attackPower);
            animator.SetBool("IsAttacking", true);
            animator.SetFloat("AnimSpeed", 0.0f); //hold attack animation
            hasAttacked = false;
            attackPower += Time.deltaTime;
            
            if(attackPower > 2f)//if you can get clamp to work here, go for it!
            {
                attackPower = 2f;//TODO: implement sword projectile
            }

            //Debug.Log("attack power: "+attackPower);
        }
        else if(!hasAttacked)
        {
            //get direction and normalize
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-this.transform.position;
            animator.SetFloat("AnimSpeed", 1.0f);  //resume attack animation
            

            hasAttacked = true;
            attack.Attack(direction, attackPower);
            attackPower = 0;
            hud.ResetAttack();
            vfx.ResetAttack();
            animator.SetBool("IsAttacking", false);
        }

        hud.SetAttackLevel(attackPower);
        vfx.SetAttackLevel(attackPower);
    }

    void Dialouge()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1.5f);

            CharacterDialougeComponent npc = null;
            float distance = Mathf.Infinity;

            foreach(Collider2D c in colliders)
            {
                float distTo = Vector2.Distance(this.transform.position, c.transform.position);
                if (distTo < distance)
                {
                    CharacterDialougeComponent d = c.gameObject.GetComponent<CharacterDialougeComponent>();
                    if(d != null)
                    {
                        distance = distTo;
                        npc = d;
                    }
                }
            }
            if (npc != null)
            {
                npc.PrintDialouge();
            }
        }
    }
}
