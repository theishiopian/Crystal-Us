using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMoverComponent controller;
    private CharacterAnimationController animator;
    private CharacterAttackComponent attack;
    private PlayerHUDComponent hud;
    private PlayerVFXComponent vfx;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.globalObjects.Add("player", this.gameObject);
        controller = this.gameObject.GetComponent<CharacterMoverComponent>();
        animator = this.gameObject.GetComponent<CharacterAnimationController>();
        attack = this.gameObject.GetComponent<CharacterAttackComponent>();
        hud = this.gameObject.GetComponent<PlayerHUDComponent>();
        vfx = this.gameObject.GetComponent<PlayerVFXComponent>();
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        
        Move();
        Attack();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(x, y);

        anim.SetFloat("MovementHorizontal", movement.x);
        anim.SetFloat("MovementVertical", movement.y);

        if(movement.x != 0 || movement.y != 0)
        {
            anim.SetFloat("Speed", 5.0f);
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
        }
                
        controller.Move(movement);
   
    }

    private float attackPower = 0;
    private bool attacked = true;

    void Attack()
    {
        //Debug.Log(arrowLevel);
        if(Input.GetMouseButton(0))//TODO replace with axis
        {
            hud.InitAttack(attackPower);
            vfx.InitAttack(attackPower);
            
            attacked = false;
            attackPower += Time.deltaTime;
            
            if(attackPower > 2f)//if you can get clamp to work here, go for it!
            {
                attackPower = 2f;//TODO: implement sword projectile
            }

            //Debug.Log("attack power: "+attackPower);
        }
        else if(!attacked)
        {
            //get direction and normalize
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-this.transform.position;

            float x = direction.x;
            float y = direction.y;

            if(Mathf.Abs(x)>Mathf.Abs(y))
            {
                direction.x = Mathf.Sign(x);
                direction.y = 0;
            }
            else
            {
                direction.y = Mathf.Sign(y);
                direction.x = 0;
            }

            attacked = true;
            attack.Attack(direction, attackPower);
            attackPower = 0;
            hud.ResetAttack();
            vfx.ResetAttack();
        }

        hud.SetAttackLevel(attackPower);
        vfx.SetAttackLevel(attackPower);
    }
}
