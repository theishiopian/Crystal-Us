using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterComponent, ICharacterController
{
    //vfx for the charging animation
    public ParticleSystem chargingEffect;
    public GameObject chargeLevel1;
    public GameObject chargeLevel2;

    public List<GameObject> forceArrows;
    public List<GameObject> forceLevels;

    private CharacterMoverComponent controller;
    private CharacterAnimationController animator;
    private CharacterAttackComponent attack;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.globalObjects.Add("player", this.gameObject);
        controller = this.gameObject.GetComponent<CharacterMoverComponent>();
        animator = this.gameObject.GetComponent<CharacterAnimationController>();
        attack = this.gameObject.GetComponent<CharacterAttackComponent>();
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
        float x = Input.GetAxis(Horizontal);
        float y = Input.GetAxis(Vertical);

        int i = animator.index;
        int ti = 0;
        //TODO replace all this grox shit with a proper animator system
        if (x > 0)
        {
            ti = 3;
        }
        else if (x < 0)
        {
            ti = 1;
        }
        else if (y > 0)
        {
            ti = 2;
        }
        else if (y < 0)
        {
            ti = 0;
        }

        Vector2 movement = new Vector2(x, y);
        controller.Move(movement);

        if (ti != i)
        {
            i = ti;
            animator.SetSprite(i);
        }
    }

    float attackPower = 0;
    int attackLevel = 0;

    bool attacked = true;

    int oldPower = 0;
    int arrowLevel = 0;
    bool hasCharged = false;

    private void ClearArrows()
    {
        foreach (GameObject arrow in forceArrows)
        {
            arrow.SetActive(false);
        }
    }

    private void ClearLevels()
    {
        foreach (GameObject level in forceLevels)
        {
            level.SetActive(false);
        }
    }
    void Attack()
    {
        Debug.Log(arrowLevel);
        if(Input.GetMouseButton(0))//TODO replace with axis
        {
            if(attackPower < 2)
            {
                arrowLevel += Mathf.FloorToInt(attackPower*10/2);
                arrowLevel *= 2;
                if (arrowLevel > oldPower && oldPower < 30 && attackLevel < 3)
                {
                    forceArrows[oldPower/3].SetActive(true);
                    oldPower++;
                }
                else if (oldPower >= 15 && attackLevel > 0 && attackLevel < 2 && !hasCharged)
                {
                    oldPower = 0;
                    attackLevel++;
                    arrowLevel = 0;
                    ClearArrows();
                    Debug.Log("called");
                    hasCharged = true;
                }
            }
            if (!chargingEffect.isPlaying && attackPower ==0) chargingEffect.Play();
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
            oldPower = 0;
            arrowLevel = 0;
            hasCharged = false;
            ClearArrows();
            chargingEffect.Stop();
        }

        if (attackPower >= 2)
        {

            chargeLevel2.SetActive(true);
            chargeLevel1.SetActive(false);
            attackLevel = 2;
            forceLevels[1].SetActive(true);
            if (!chargingEffect.isPlaying && attackPower<2) chargingEffect.Play();
        }
        else if (attackPower >= 1)
        {

            chargeLevel1.SetActive(true);
            chargeLevel2.SetActive(false);
            attackLevel = 1;
            forceLevels[0].SetActive(true);
            if (!chargingEffect.isPlaying) chargingEffect.Play();
        }
        else
        {
            chargeLevel1.SetActive(false);
            chargeLevel2.SetActive(false);
            attackLevel = 0;
            ClearLevels();
        }
    }
}
