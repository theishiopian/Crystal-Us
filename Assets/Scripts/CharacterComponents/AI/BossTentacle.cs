using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTentacle : MonoBehaviour
{
    // Gabriel Staffen (and some code barrowed from CharacterMelleComponent)
    // This script extends and retracts the tentacle line renderer.
    // It also runs a raycast to damage the player, and retracts on hit after dealing damage.

    private Vector2 destination;                    //destination of tentacle
    [SerializeField] private float speed = 5f;      //speed of tentacle movement
    [SerializeField] private float maxDist = 9f;    //maximum distance the tentacle travels
    [SerializeField] private float extraDist = 2f;  //extra distance added to the destination (to stop easy dodging)
    [SerializeField] private int damage = 6;        //damage inflicted on the player when hit by the tentacle
    [SerializeField] private float knockback = 6f;  //knockback the player experiences when hit by the tentacle
    private bool returning;                         //whether the tentacle is on the return trip from attacking the destination
    private LineRenderer line;                      //line renderer of the object
    private Vector2 linePos;                        //the end point of the line renderer (in world space)
    
    void Update()
    {
        //move line renderer
        float returnSpdMod = 1f;
        if (returning)                          //if returning
        {
            returnSpdMod = 0.5f;                //make return speed half of normal speed
            destination = transform.position;   //constantly update the return destination with the object world position
        }
        //move end point of tentacle
        if ((linePos - destination).magnitude <= speed * returnSpdMod * Time.deltaTime)    //check if tentacle is within tolerance of destination
        {
            if (!returning)                                                 //if attacking outward (not returning)
            {
                linePos = destination;                                      //set end point to the destination
                destination = transform.position;                           //set new destination to the start point
                returning = true;                                           //set returning to true (now it is returning)
            }
            else
            {
                linePos = destination;                                      //set end point to the destination
                returning = false;                                          //no longer returning (because it has returned to the start point)
                this.gameObject.SetActive(false);                           //deactivate tentacle
            }
        }
        else
        {
            linePos += (destination - linePos).normalized * speed * returnSpdMod * Time.deltaTime;  //move tentacle towards destination
        }
        //apply changes to line renderer
        line.SetPosition(1, linePos - new Vector2(transform.position.x, transform.position.y));     //apply changes to end point in the line renderer (but in local space)
        
        //damage raycast
        if (!returning)                                                                             //if the tentacle is attacking outward
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, linePos, 1 << 9);             //linecast between start point and end point (layer masked to player layer)
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
                        body.AddForce((destination - linePos).normalized * knockback, ForceMode2D.Impulse);
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
        line = GetComponent<LineRenderer>();    //set line renderer
        line.SetPosition(0, Vector2.zero);      //set start point to zeros (local space)
        line.SetPosition(1, Vector2.zero);      //set end point to zeros (local space)
        returning = false;                      //by default, the tentacle isn't returning

        //set end point of line renderer to the starting position (in world space)
        linePos = transform.position;   // (starting position is (0, 0) so it is just the object's world position)
    }
}
