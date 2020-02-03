using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    SPINNING,
    LAUNCHING,
    MOVING
}


public class BossController : MonoBehaviour
{
    public BossState currentState = BossState.MOVING;

    public GameObject player;

    float interval;

    private void Start()
    {
        interval = Random.Range(3, 8);
        Object cache;
        if (player == null && GlobalVariables.globalObjects.TryGetValue("player", out cache)) player = (GameObject)cache;

        Debug.Log("started " + Time.time);
    }

    float t = 0;
    

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
    }

    private void Spin()
    {
        Debug.Log("spinning");
    }

    private void Launch()
    {
        Debug.Log("launching");
    }

    private void ChangeState()
    {
        if(currentState != BossState.MOVING)
        {
            currentState = BossState.MOVING;
        }
        else
        {
            int state = Random.Range(0,2);
            currentState = state == 0 ? BossState.LAUNCHING : BossState.SPINNING;
        }
    }
}
