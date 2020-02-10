using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMoverComponent controller;
    private CharacterMeleeComponent meleeAttack;
    private PlayerHUDComponent hud;
    private PlayerVFXComponent vfx;
    private Animator animator;
    private CharacterRangedComponent rangedAttack;
    private CharacterHealthComponent health;
    private CharacterLevelComponent level;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GlobalVariables.globalObjects["player"] = this.gameObject;
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance != 0)
            {
                closest = go;
                distance = curDistance;
                Destroy(go);
                break;
            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = GlobalVariables.spawnPos;
        GlobalVariables.globalObjects["player"] = this.gameObject;
        controller = this.gameObject.GetComponent<CharacterMoverComponent>();
        meleeAttack = this.gameObject.GetComponent<CharacterMeleeComponent>();
        hud = this.gameObject.GetComponent<PlayerHUDComponent>();
        vfx = this.gameObject.GetComponent<PlayerVFXComponent>();
        level = this.gameObject.GetComponent<CharacterLevelComponent>();
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetFloat("AnimSpeed", 1.0f);
        rangedAttack = GetComponent<CharacterRangedComponent>();
        health = GetComponent<CharacterHealthComponent>();
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("IsAttacking") && !animator.GetBool("IsDamaged"))
        {
            Move();
        }
        Attack();
        Dialouge();
        //Debug.Log(attackPower);
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
            Vector2 atkdir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

            float x = atkdir.x;
            float y = atkdir.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                x = Mathf.Sign(x);
                y = 0;
            }
            else
            {
                y = Mathf.Sign(y);
                x = 0;
            }
            animator.SetFloat("AttackHorizontal", x);
            animator.SetFloat("AttackVertical", y);

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
            meleeAttack.Attack(direction, attackPower);
            if(attackPower >= 2)
            {
                rangedAttack.Attack(direction, 20);
            }
            else if(attackPower >= 1)
            {
                rangedAttack.Attack(direction, 20);
            }
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
            if (npc.name == "NecklaceNPC_Found")
            {
                npc.PrintDialouge();
                hud.getKey();
            }
            else if (npc.name == "HealNPC")
            {
                npc.PrintDialouge();
                health.health = health.maxHealth;
            }
            else if (npc != null)
            {
                npc.PrintDialouge();
            }
        }
    }

}
