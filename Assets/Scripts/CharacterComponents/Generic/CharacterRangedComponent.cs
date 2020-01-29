using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRangedComponent : MonoBehaviour, ICharacterComponent
{
  
    public GameObject prefab;//projectile prefab

    public void Attack(Vector2 direction, float launchPower)
    {
        float x = direction.x;
        float y = direction.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            direction.x = Mathf.Sign(x);
            direction.y = 0;
        }
        else
        {
            direction.y = Mathf.Sign(y);
            direction.x = 0;
        }

        GameObject g = Instantiate(prefab, (Vector2)this.transform.position + direction * 1.5f, Quaternion.identity);//TODO make sure rotation happens somewhere
        Rigidbody2D body = g.GetComponent<Rigidbody2D>();
        BulletController bullet = g.GetComponent<BulletController>();
        if(bullet && body)
        {       
            body.AddForce(direction * launchPower, ForceMode2D.Impulse);
            bullet.SetData(direction);
        }
        else
        {
            Destroy(g);
            throw new System.InvalidOperationException("bullet prefab incorectly configured");
        }
    }
}
