using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour, ICharacterComponent
{
    //TODO: replace with proper animator-based system

    /*
     * down
     * left
     * up
     * right
     */

    public List<Sprite> states;

    [HideInInspector]
    public int index = 0;

    private new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int index)
    {
        renderer.sprite = states[index];
        this.index = index;
    }
}
