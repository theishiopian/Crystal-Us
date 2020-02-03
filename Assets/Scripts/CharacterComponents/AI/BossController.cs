using System.Collections;
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

    private BossState currentState = BossState.MOVING;
    private Rigidbody2D body;


    private void Start()
    {
        body = this.GetComponent<Rigidbody2D>();

        interval = Random.Range(3, 8);
        Object cache;
        if (player == null && GlobalVariables.globalObjects.TryGetValue("player", out cache)) player = (GameObject)cache;

        Debug.Log("started " + Time.time);
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
            interval = Random.Range(3, 8);
            t = 0;
            ChangeState();
        }
    }

    private void Move()
    {
        Debug.Log("moving");

        body.position += TargetDirection(player.transform.position) * 0.03f;
    }

    bool spinning = false;

    private void Spin()
    {
        Debug.Log("spinning");
        if(!spinning)
        {
            Vector2 dir = Random.insideUnitCircle.normalized * 10;

            body.AddForce(dir, ForceMode2D.Impulse);

            spinning = true;
        }
    }

    private void Launch()
    {
        Debug.Log("launching");
    }

    private void ChangeState()
    {
        body.velocity = Vector2.zero;
        if(currentState != BossState.MOVING)
        {
            currentState = BossState.MOVING;
        }
        else
        {
            int state = Random.Range(0,2);
            switch (state)
            {
                case 0: currentState = BossState.LAUNCHING;
                    break;
                case 1: currentState = BossState.SPINNING;
                    break;
                    //any other states go here
            }
        }

        spinning = false;
    }
}
