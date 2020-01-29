using UnityEngine;

[RequireComponent(typeof(CharacterLevelComponent))]
public class CharacterMeleeComponent : MonoBehaviour, ICharacterComponent
{
    public string layerToHit;
    public float distance;
    public float knockback;
    private LayerMask mask;
    private CharacterLevelComponent level;
    void Start()
    {
        mask = LayerMask.GetMask(layerToHit);
        level = GetComponent<CharacterLevelComponent>();
    }

    private bool Attack(Vector2 direction, float attackPower, float distance, float knockback)
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

        Vector2 position = this.gameObject.transform.position;
        RaycastHit2D hit = Physics2D.CircleCast(position,1,direction,distance,mask);
        Debug.DrawRay(position, direction, Color.blue,1);
        CharacterHealthComponent health = null;
        Rigidbody2D body = null;
        try
        {
            health = hit.collider.gameObject.GetComponent<CharacterHealthComponent>();
        }
        catch
        {
            //add debug code here if needed
        }
        if(hit && health != null)
        {
            health.Damage(Mathf.CeilToInt(attackPower));
            body = health.gameObject.GetComponent<Rigidbody2D>();
            if(body != null && knockback>0)
            {
                body.AddForce(direction * knockback, ForceMode2D.Impulse);
            }
            return true;
        }

        return false;
    }

    public bool Attack(Vector2 direction, float attackPower)//player attack method
    {
        return Attack(direction, attackPower * level.level, 2, 20);
    }

    public bool Attack(Vector2 direction)//enemy attack method
    {
        return Attack(direction, 2 * level.level, distance, knockback);
    }
}
