﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float movementSpeed;

    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        if(!this.gameObject.GetComponent<Rigidbody2D>())
        {
            body = this.gameObject.AddComponent< Rigidbody2D>();
            body.gravityScale = 0;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            //add any rigidbody tweaks here
        }
        else
        {
            body = this.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public bool Move(Vector2 movement)
    {
        body.position = body.position + (movement * movementSpeed * Time.deltaTime);
        return true;
    }
}
