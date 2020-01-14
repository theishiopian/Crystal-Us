using UnityEngine;

public class CharacterAttack : MonoBehaviour, ICharacterComponent
{
    void Start()
    {

    }

    public bool Attack(Vector2 direction, float attackPower)
    {
        Vector2 position = this.gameObject.transform.position;
        RaycastHit2D hit = Physics2D.CircleCast(position,1,direction);

        return true;
    }
}
