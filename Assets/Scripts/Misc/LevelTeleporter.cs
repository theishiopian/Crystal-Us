using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleporter : MonoBehaviour
{
    public Vector3 destinationCoordinates;

    public string sceneTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("It's Happening!!!" + other.gameObject.tag);
        if (other.gameObject.tag == "Player") // Make sure the collider belongs to the player.
        {
            GlobalVariables.spawnPos = destinationCoordinates;

            SceneManager.LoadScene(sceneTarget, LoadSceneMode.Single);
        }
    }
}
