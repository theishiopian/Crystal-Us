using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputListner : MonoBehaviour
{
    private CharacterController2D controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController2D>();
    }

    //will move to reference class if neccesary
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis(Horizontal);
        float y = Input.GetAxis(Vertical);

        controller.Move(new Vector2(x,y));
    }
}
