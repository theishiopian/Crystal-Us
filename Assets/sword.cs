using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHUDComponent HUD = other.GetComponent<PlayerHUDComponent>();
        if (HUD == null)
            return;
        HUD.getSword();
        Destroy(gameObject);
    }
}
