using UnityEngine;

[RequireComponent(typeof(CharacterLevelComponent))]
public class CharacterAttackComponent : MonoBehaviour, ICharacterComponent
{
    public string layerToHit;

    private LayerMask mask;
    private CharacterLevelComponent level;
    void Start()
    {
        mask = LayerMask.GetMask(layerToHit);
        level = GetComponent<CharacterLevelComponent>();
    }

    public bool Attack(Vector2 direction, float attackPower, float distance)
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
            health.Damage(1+level.level * Mathf.CeilToInt(attackPower));
            return true;
        }

        return false;
    }

    public bool Attack(Vector2 direction, float attackPower)
    {
        return Attack(direction, attackPower, 2);//player attack method
    }
}
