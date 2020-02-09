using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyNPC : MonoBehaviour
{
    PlayerHUDComponent HUD;
    private GameObject player;
    
    void Start()
    {
        Object cache;

        GlobalVariables.globalObjects.TryGetValue("player", out cache);

        if (cache != null)
        {
            player = (GameObject)cache;
        }
        HUD = player.GetComponent<PlayerHUDComponent>();
    }

    
    void Update()
    {
        if (HUD.hasNecklace || HUD.hasKey)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
