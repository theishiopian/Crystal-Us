using System.Collections;
using UnityEngine;

public class EnemyController : AI, ICharacterComponent, ICharacterController
{
    public float patrolDistance = 8f;   // The distance the simple AI will patrol from the patrolArea
    public GameObject player;           // The player GameObject
    public float detectionRange;   // The range at which the simple AI will begin following the player
    public float followDistance;  // The distance the simple AI will follow the player before going back to the patrolArea
    public float startDelay;
    public float endDelay;
    public float rangedPower;

    public Material[] effects;// Temporary hurt effect TODO: replace with animation

    //component references
    private CharacterMoverComponent controller;
    private CharacterMeleeComponent melee; // Will use melee if ranged is null
    private CharacterRangedComponent ranged; // Will use ranged while not null
    private CharacterLevelComponent level;
    private new SpriteRenderer renderer;

    private Vector3 patrolPoint;         // The position the simple AI will patrol around (start position)
    private float moveNext = 3f;             // While patroling, how long until the next movement
    private Vector2 moveDirection;      // Move direction
    private bool following = false;             // If the simple AI is following the player

    private Animator animator;          //animator component


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterMoverComponent>();
        melee = GetComponent<CharacterMeleeComponent>();
        ranged = GetComponent<CharacterRangedComponent>();
        level = GetComponent<CharacterLevelComponent>();
        renderer = GetComponent<SpriteRenderer>();
        patrolPoint = transform.position;
        following = false;
        moveDirection = RandomDirection();
        animator = this.gameObject.GetComponent<Animator>();

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

    bool isAttacking = false;
    bool attackStarted = false;

    // Update is called once per frame
    void Update()
    {
        if(!isAttacking)
        {
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDamaged", false);
            if (following)
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
            float enemyX = moveDirection.x;
            float enemyY = moveDirection.y;

            if(Mathf.Abs(enemyX) > 0f && Mathf.Abs(enemyX) <= 1f)
            {
                enemyX = 1f * Mathf.Sign(enemyX);
                animator.SetFloat("MovementHorizontal", enemyX);
            }
            else animator.SetFloat("MovementHorizontal", 0f);
            if (Mathf.Abs(enemyY) > 0f && Mathf.Abs(enemyY) <= 1f)
            {
                enemyY = 1f * Mathf.Sign(enemyY);
                animator.SetFloat("MovementVertical", enemyY);
            }
            else animator.SetFloat("MovementVertical", 0f);

            controller.Move(moveDirection);
            

            //detect attack

            if(Vector2.Distance(this.transform.position, player.transform.position) < 1.5f && ranged == null)
            {
                isAttacking = true;

            }
            else if (Vector2.Distance(this.transform.position, player.transform.position) < 4.5f && ranged != null) // If ranged isn't null, use this range.
            {
                isAttacking = true;
            }
        }
        else//meleecode
        {
            if(!attackStarted)
            {
                attackStarted = true;
                StartCoroutine("AttackSequence");
            }
        }
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(startDelay);
        Vector2 direction = (player.transform.position - this.transform.position).normalized;
        animator.SetBool("IsAttacking", true);
        if (ranged == null) // If the ranged component is null then use melee, otherwise use ranged.
        {
            melee.Attack(direction);
        }
        else
        {
            ranged.Attack(direction, rangedPower);
        }
        //renderer.material = effects[1];
        yield return new WaitForSeconds(endDelay);
        isAttacking = false;
        attackStarted = false;
        //renderer.material = effects[0];
        yield return null;
    }

    // Check if the simple AI should follow the player or not
    bool FollowCheck()
    {
        var distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (!following)
        {
            // While not following, check if the player is within detection range
            return distanceToPlayer < detectionRange;
        }
        else
        {
            // While following, check if the simple AI has exceeded the follow distance
            return distanceToPlayer > followDistance;
        }
    }
}
