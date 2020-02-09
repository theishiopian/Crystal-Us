using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : AI, ICharacterComponent, ICharacterController
{
    public float patrolDistance = 8f;   // The distance the simple AI will patrol from the patrolArea
    public GameObject player;           // The player GameObject

    //component references
    private CharacterMoverComponent controller;
    private new SpriteRenderer renderer;

    private Vector3 patrolPoint;         // The position the simple AI will patrol around (start position)
    private float moveNext = 3f;             // While patroling, how long until the next movement
    private Vector2 moveDirection;      // Move direction

    //private Animator animator;          //animator component


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterMoverComponent>();
        renderer = GetComponent<SpriteRenderer>();
        patrolPoint = transform.position;
        moveDirection = RandomDirection();
        //animator = this.gameObject.GetComponent<Animator>();

        if (player == null)
        {
            Object cache;
            if (GlobalVariables.globalObjects.TryGetValue("player", out cache))
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
        moveNext -= Time.deltaTime;

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


        // Move
        /*float npcX = moveDirection.x;
        float npcY = moveDirection.y;

        if (Mathf.Abs(npcX) > 0f && Mathf.Abs(npcX) <= 1f)
        {
            npcX = 1f * Mathf.Sign(npcX);
            animator.SetFloat("MovementHorizontal", npcX);
        }
        else animator.SetFloat("MovementHorizontal", 0f);
        if (Mathf.Abs(npcY) > 0f && Mathf.Abs(npcY) <= 1f)
        {
            npcY = 1f * Mathf.Sign(npcY);
            animator.SetFloat("MovementVertical", npcY);
        }
        else animator.SetFloat("MovementVertical", 0f);*/

        controller.Move(moveDirection);
    }
}
