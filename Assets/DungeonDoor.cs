﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    PlayerHUDComponent HUD;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HUD = other.collider.GetComponent<PlayerHUDComponent>();
        if (HUD.hasKey)
        {
            Destroy(gameObject);
        }
    }
}
