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

    [SerializeField] private int touchDamage;
    [SerializeField] private float touchKnockback;
    private bool playerInvulnerability = false;

    private BossState currentState = BossState.MOVING;
    private Rigidbody2D body;
    private Animator animator;
    public bool facingLeft = true;
    private float spinDuration = .05f;


    private void Start()
    {
        body = this.GetComponent<Rigidbody2D>();

        interval = Random.Range(3, 8);
        Object cache;
        if (player == null && GlobalVariables.globalObjects.TryGetValue("player", out cache)) player = (GameObject)cache;
        animator = this.gameObject.GetComponent<Animator>();
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
            player.GetComponent<CharacterHealthComponent>().Damage(touchDamage);
            if (player.GetComponent<Rigidbody2D>() != null && touchKnockback > 0)
            {
                player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * touchKnockback, ForceMode2D.Impulse);
            }
            StartCoroutine(PlayerInvulnerable());
        }
    }

    private void Move()
    {
        animator.SetBool("IsSpinning", false);
        body.position += TargetDirection(player.transform.position) * 1.8f * Time.deltaTime;  //fixed so the movement isn't tied to the script's update speed

        float enemyX = TargetDirection(player.transform.position).x;
        float enemyY = TargetDirection(player.transform.position).y;

        if (Mathf.Abs(enemyX) > 0f && Mathf.Abs(enemyX) <= 1f)
        {
            enemyX = 1f * Mathf.Sign(enemyX);
            animator.SetFloat("MovementHorizontal", enemyX);
            if (enemyX < 0 && !facingLeft)
                Flip();
            else if (enemyX > 0 && facingLeft)
                Flip();

        }
        else animator.SetFloat("MovementHorizontal", 0f);
        if (Mathf.Abs(enemyY) > 0f && Mathf.Abs(enemyY) <= 1f)
        {
            enemyY = 1f * Mathf.Sign(enemyY);
            animator.SetFloat("MovementVertical", enemyY);
        }
        else animator.SetFloat("MovementVertical", 0f);
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
            animator.SetBool("IsSpinning", true);
        }
        spinDuration -= Time.deltaTime;
        if (spinDuration < 0f)
        {
            Flip();
            spinDuration = .05f;
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
        animator.SetBool("IsFiring", false);
    }

    bool launching = false;

    private void Launch()
    {
        if (!launching)
        {
            StartCoroutine(TentacleBarrage());
            animator.SetBool("IsFiring", true);
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

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
