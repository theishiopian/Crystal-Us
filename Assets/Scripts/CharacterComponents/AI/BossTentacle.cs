using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTentacle : MonoBehaviour
{
    // Gabriel Staffen (and some code barrowed from CharacterMelleComponent)
    // This script extends and retracts the tentacle.
    // It also runs a raycast to damage the player, and retracts on hit after dealing damage.

    private Vector2 destination;                    //destination of tentacle
    [SerializeField] private float speed = 5f;      //speed of tentacle movement
    [SerializeField] private float maxDist = 9f;    //maximum distance the tentacle travels
    [SerializeField] private float extraDist = 2f;  //extra distance added to the destination (to stop easy dodging)
    [SerializeField] private int damage = 6;        //damage inflicted on the player when hit by the tentacle
    [SerializeField] private float knockback = 6f;  //knockback the player experiences when hit by the tentacle
    private bool returning;                         //whether the tentacle is on the return trip from attacking the destination
    private GameObject tentacleEnd = null;          //end image of tentacle
    private SpriteRenderer tentacleTile = null;     //tentacle tile sprite renderer
    private Vector2 tentaclePos;                    //tentacle end point position
    
    void Update()
    {
        float returnSpdMod = 1f;
        if (returning)                          //if returning
        {
            returnSpdMod = 0.5f;                //make return speed half of normal speed
            destination = transform.position;   //constantly update the return destination with the object world position
        }
        //move end point of tentacle
        if ((tentaclePos - destination).magnitude <= speed * returnSpdMod * Time.deltaTime)    //check if tentacle is within tolerance of destination
        {
            if (!returning)                                                 //if attacking outward (not returning)
            {
                tentaclePos = destination;                                  //set end point to the destination
                destination = transform.position;                           //set new destination to the start point
                returning = true;                                           //set returning to true (now it is returning)
            }
            else
            {
                tentaclePos = destination;                                  //set end point to the destination
                returning = false;                                          //no longer returning (because it has returned to the start point)
                this.gameObject.SetActive(false);                           //deactivate tentacle
            }
        }
        else
        {
            tentaclePos += (destination - tentaclePos).normalized * speed * returnSpdMod * Time.deltaTime;  //move tentacle towards destination
        }
        //apply changes to tentacle
        tentacleTile.size = new Vector2(1, (tentaclePos - new Vector2(transform.position.x, transform.position.y)).magnitude);

        //THE FOLLOWING CODE WORKS BUT I HAVE NO FUCKING IDEA HOW, A RESULT OF PAINFUL TRIAL AND ERROR
        Quaternion tentacleRot = Quaternion.LookRotation((new Vector3(tentaclePos.x, tentaclePos.y, 0f) - transform.position).normalized, Vector3.forward);
        transform.rotation = Quaternion.Euler(0f, 0f, -90f - tentacleRot.eulerAngles.x);
        //DO NOT TOUCH THE TWO LINES ABOVE, IT IS BLACK MAGIC

        tentacleEnd.transform.position = tentaclePos;                                               //apply changes to end point of the tentacle
        
        //damage raycast
        if (!returning)                                                                             //if the tentacle is attacking outward
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, tentaclePos, 1 << 9);         //linecast between start point and end point (layer masked to player layer)
            if (hit)                                                                                //if the linecast hit something
            {
                //-- barrowing code from CharacterMeleeComponent --
                CharacterHealthComponent health = null;
                Rigidbody2D body = null;
                try
                {
                    health = hit.collider.gameObject.GetComponent<CharacterHealthComponent>();      //the player's health from the collider
                }
                catch { /*nothing*/ }
                if (hit && health != null)
                {
                    health.Damage(damage);
                    body = health.gameObject.GetComponent<Rigidbody2D>();
                    if (body != null && knockback > 0)
                    {
                        body.AddForce((destination - tentaclePos).normalized * knockback, ForceMode2D.Impulse);
                    }
                }
                // --                                             --

                //return tentacle
                destination = transform.position;                                       //set new destination to the start point
                returning = true;                                                       //set returning to true
            }
        }
    }

    //public set destination
    public void SetDestination(Vector2 dest)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);  //use a temporary variable to clean up the code
        destination = dest + (dest - pos).normalized * extraDist;               //set destination plus extra distance (to stop easy dodging)
        if ((destination - pos).magnitude > maxDist)                            //check if the destination is outside the max distance
        {
            destination = pos + (dest - pos).normalized * maxDist;              //set destination to the max distance from the start point in the destination direction
        }

        //this is sorta the start script for the tentacle too, since this script is used after first activation
        returning = false;                      //by default, the tentacle isn't returning
        tentaclePos = transform.position;
        tentacleEnd = transform.Find("Tentacle End").gameObject;
        tentacleTile = GetComponent<SpriteRenderer>();
    }
}
