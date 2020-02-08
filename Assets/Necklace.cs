using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necklace : MonoBehaviour
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
        Debug.Log(other);
        PlayerHUDComponent HUD = other.GetComponent<PlayerHUDComponent>();
        if (HUD == null)
            return; 
        Debug.Log("hello");
        HUD.getNecklace();
        Destroy(gameObject);
    }
}
