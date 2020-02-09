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
            
            t = 0;
            ChangeState();
        }
    }

    private void Move()
    {
        body.position += TargetDirection(player.transform.position) * 1.8f * Time.deltaTime;  //fixed so the movement isn't tied to the script's refresh speed
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
        //launch tentacles until move
        if (!launching)
        {
            StartCoroutine(TentacleBarrage());
            launching = true;
        }
        if (t <= 4.05f)                     //The latest time for all the tentacles to return
        {
            body.velocity = Vector2.zero;   //Stop motion in order to not mess up tentacle animations
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
}
