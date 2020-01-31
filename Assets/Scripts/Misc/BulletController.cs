using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public int damage;
    public float knockback;

    public string layerToHit;
    private LayerMask mask;
    private GameObject owner;
    private Vector2 direction;
    void Start()
    {
        mask = LayerMask.GetMask(layerToHit);
        Destroy(this.gameObject, 1);
    }

    public void SetData(Vector2 direction)//pass data to bullet, may replace with velocity vector
    {
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & mask) != 0)//bitmask shit, do not touch
        {
            //Debug.Log(true);

            CharacterHealthComponent health = collision.gameObject.GetComponent<CharacterHealthComponent>();

            health.Damage(damage);

            if(knockback>0)
            {
                Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();

                if (otherBody)
                {
                    otherBody.AddForce(direction * knockback);
                }
            }
        }

        Destroy(this.gameObject);
    }
}
