using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMoverComponent controller;

    private Vector3 patrolPoint;         // The position the simple AI will patrol around (start position)
    public float patrolDistance = 8f;   // The distance the simple AI will patrol from the patrolArea
    public GameObject player;           // The player GameObject
    public float detectionRange;   // The range at which the simple AI will begin following the player
    public float followDistance;  // The distance the simple AI will follow the player before going back to the patrolArea
    private float moveNext = 3f;             // While patroling, how long until the next movement
    private Vector2 moveDirection;      // Move direction

    private bool following = false;             // If the simple AI is following the player

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterMoverComponent>();
        patrolPoint = transform.position;
        following = false;
        moveDirection = RandomDirection();

        if(player == null)
        {
            Object cache;
            if(GlobalVariables.globalObjects.TryGetValue("player", out cache))
            {
                player = (GameObject)cache;
            }
            else
            {
                Debug.Log("player null");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(following)
        {
            Debug.DrawLine(this.transform.position, player.transform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(this.transform.position, patrolPoint, Color.red);
        }

        // Perform follow check
        following = FollowCheck();
        
        moveNext -= Time.deltaTime;

        if (!following)
        {
            // While not following player, patrol area in random directions
            if (moveNext <= 0f)
            {
                // Reset timer for next direction change
                moveNext = 3f;
                // Check if within patrol distance
                if ((patrolPoint - this.transform.position).magnitude < patrolDistance)
                {
                    moveDirection = RandomDirection();              // Move randomly
                }
                else
                {
                    Vector2 dir = TargetDirection(patrolPoint);    // Move towards patrol area
                    moveDirection = dir;
                } 
            }
        }
        else
        {
            moveDirection = TargetDirection(player.transform.position); // Move towards player
        }

        // Move

        controller.Move(moveDirection);
    }

    // Check if the simple AI should follow the player or not
    bool FollowCheck()
    {
        var distance2Player = Vector3.Distance(player.transform.position, this.transform.position);

        if (!following)
        {
            // While not following, check if the player is within detection range
            return distance2Player < detectionRange;
        }
        else
        {
            // While following, check if the simple AI has exceeded the follow distance
            return distance2Player > followDistance;
        }
    }

    // Returns a random Vector2 in one of the four cardinal directions
    Vector2 RandomDirection()
    {
        //Debug.Log("called");
        Vector2 direction = Random.insideUnitCircle.normalized;
        float angle = Mathf.Round(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) / 90) * 90; // change the 90s to 45s if you want 8 cardinal directions
        //float sin = Mathf.Sin(angle);
        //float cos = Mathf.Cos(angle);
        //Debug.Log(angle);
        switch (angle)//hack to get the direction right based on angle TODO: generic solution?
        {
            case 0: return new Vector2(1.0f, 0.0f);
            case 90: return new Vector2(0.0f, 1.0f);
            case 180: return new Vector2(-1.0f, 0.0f);
            case -180: return new Vector2(1.0f, 0.0f);
            case -90: return new Vector2(0.0f, -1.0f);
            default: return new Vector2(0.0f, 0.0f);
        }

        //return new Vector2(cos, sin);
    }

    // Returns a Vector2 in the rough direction of the input target (world space) rounded to one of the four cardinal directions
    Vector2 TargetDirection(Vector3 target)
    {
        Vector2 direction = (target - this.transform.position).normalized;
        float angle = Mathf.Round((Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x)))/90)*90; // change the 90s to 45s if you want 8 cardinal directions
        //float sin = Mathf.Sin(angle);
        //float cos = Mathf.Cos(angle);
        //var d = new Vector2(cos,sin);


        Debug.DrawRay(this.transform.position, direction, Color.blue);
        //Debug.DrawRay(this.transform.position, d, Color.green);
        //Debug.Log(angle);

        switch (angle)//hack to get the direction right based on angle TODO: generic solution?
        {
            case 0: return new Vector2(1.0f, 0.0f);
            case 90: return new Vector2(0.0f, 1.0f);
            case 180: return new Vector2(-1.0f, 0.0f);
            case -180: return new Vector2(-1.0f, 0.0f);//not sure why these two need to be  different betwqeen methods, needs further investigation
            case -90: return new Vector2(0.0f, -1.0f);
            default: return new Vector2(0.0f,0.0f);
        }

        //return new Vector2(-1.0f,0.0f);
    }
}
