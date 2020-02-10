using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleporter : MonoBehaviour
{
    public Vector3 destinationCoordinates;

    public string sceneTarget;

    private PlayerHUDComponent hud;
    private CharacterHealthComponent health;
    private CharacterLevelComponent level;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        hud = player.GetComponent<PlayerHUDComponent>();
        health = player.GetComponent<CharacterHealthComponent>();
        level = player.GetComponent<CharacterLevelComponent>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("It's Happening!!!" + other.gameObject.tag);
        if (other.gameObject.tag == "Player") // Make sure the collider belongs to the player.
        {
            GlobalVariables.spawnPos = destinationCoordinates;
            PlayerStats.Health = health.health;
            PlayerStats.Maxhealth = health.maxHealth;
            PlayerStats.HasKey = hud.hasKey;
            PlayerStats.HasNecklace = hud.hasNecklace;
            
            PlayerStats.Xp = level.XP;
            PlayerStats.Level = level.level;
            PlayerStats.Levelup = level.levelUp;

            PlayerStats.HasSword = hud.hasSword;

            SceneManager.LoadScene(sceneTarget, LoadSceneMode.Single);
            //Debug.Log(PlayerStats.Health);
        }
    }
}
