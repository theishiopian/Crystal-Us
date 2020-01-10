using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputListner : MonoBehaviour, ICharacterComponent, ICharacterController
{
    private CharacterMover controller;
    private CharacterAnimationController animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterMover>();
        animator = this.gameObject.GetComponent<CharacterAnimationController>();
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis(Horizontal);
        float y = Input.GetAxis(Vertical);

        int i = animator.index;
        int ti = 0;
        
        if(x > 0)
        {
            ti = 3;
        }
        else if(x < 0)
        {
            ti = 1;
        }
        else if(y > 0)
        {
            ti = 2;
        }
        else if(y < 0)
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
}
