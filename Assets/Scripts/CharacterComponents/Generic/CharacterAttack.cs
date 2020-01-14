using UnityEngine;

public class CharacterAttack : MonoBehaviour, ICharacterComponent
{
    public string layerToHit;

    private LayerMask mask;
    void Start()
    {
        mask = LayerMask.GetMask(layerToHit);
    }

    public bool Attack(Vector2 direction, float attackPower)
    {
        Vector2 position = this.gameObject.transform.position;
        RaycastHit2D hit = Physics2D.CircleCast(position,1,direction,2,mask);
        Debug.DrawRay(position, direction, Color.blue,1);
        CharacterHealth health = hit.collider.gameObject.GetComponent<CharacterHealth>();
        if(hit && health != null)
        {
            Debug.Log("attacked: " + hit.collider.gameObject);
            health.Damage(2 * Mathf.CeilToInt(attackPower));
            return true;
        }

        return false;
    }
}
