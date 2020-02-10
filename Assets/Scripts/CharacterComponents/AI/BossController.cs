﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    SPINNING,
    LAUNCHING,
    MOVING
}

public class BossController : AI, ICharacterComponent, ICharacterController
{
    public GameObject[] tentacles;

    public GameObject player;

    [SerializeField] private int touchDamage;
    [SerializeField] private float touchKnockback;
    private bool playerInvulnerability = false;

    private BossState currentState = BossState.MOVING;
    private Rigidbody2D body;


    private void Start()
    {
        body = this.GetComponent<Rigidbody2D>();

        interval = Random.Range(3, 8);
        Object cache;
        if (player == null && GlobalVariables.globalObjects.TryGetValue("player", out cache)) player = (GameObject)cache;
    }

    float t = 0;
    float interval;

    private void Update()
    {
        switch(currentState)
        {
            case BossState.MOVING: Move();
                break;
            case BossState.LAUNCHING: Launch();
                break;
            case BossState.SPINNING: Spin();
                break;
        }

        //increment and reset
        t += Time.deltaTime;
        if(t >= interval)
        {
            
            t = 0;
            ChangeState();
        }
    }

    //Gabe's damage on collision code
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject == player && !playerInvulnerability)
        {
            Debug.Log("Collide Distance: " + (transform.position - player.transform.position).magnitude);
            player.GetComponent<CharacterHealthComponent>().Damage(touchDamage);
            if (player.GetComponent<Rigidbody2D>() != null && touchKnockback > 0)
            {
                player.GetComponent<Rigidbody2D>().AddForce((transform.position - player.transform.position).normalized * touchKnockback, ForceMode2D.Impulse);
            }
            StartCoroutine(PlayerInvulnerable());
        }
    }

        private void Move()
    {
        body.position += TargetDirection(player.transform.position) * 1.8f * Time.deltaTime;  //fixed so the movement isn't tied to the script's update speed
    }

    bool spinning = false;

    private void Spin()
    {
        if(!spinning)
        {
            body.drag = 0f;

            Vector2 dir = Quaternion.Euler(0,0, 45 + Random.Range(0,4) * 90) * Vector2.up * 10; //45 degree angles

            body.AddForce(dir, ForceMode2D.Impulse);

            spinning = true;
        }
    }

    private void ChangeState()
    {
        body.velocity = Vector2.zero;
        if (currentState != BossState.MOVING)
        {
            currentState = BossState.MOVING;
        }
        else
        {
            int state = Random.Range(0, 2);
            switch (state)
            {
                case 0:
                    currentState = BossState.LAUNCHING;
                    interval = 6;
                    break;
                case 1:
                    currentState = BossState.SPINNING;
                    interval = Random.Range(3, 6);
                    break;
                    //any other states go here
            }
        }

        spinning = false;
        body.drag = 20f;
        launching = false;
    }

    bool launching = false;

    private void Launch()
    {
        if (!launching)
        {
            StartCoroutine(TentacleBarrage());
            launching = true;
        }
    }
    
    IEnumerator TentacleBarrage()
    {
        for(int i = 0; i != 4; i++)
        {
            //launch them tentacles my guy ;D
            tentacles[i].SetActive(true);
            tentacles[i].GetComponent<BossTentacle>().SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.6f);
        }

        yield return null;
    }

    IEnumerator PlayerInvulnerable()
    {
        playerInvulnerability = true;
        yield return new WaitForSeconds(0.5f);
        playerInvulnerability = false;
        yield return null;
    }
}
