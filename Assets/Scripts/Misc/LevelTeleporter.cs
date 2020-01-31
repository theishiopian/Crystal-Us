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
        GlobalVariables.spawnPos = destinationCoordinates;

        SceneManager.LoadScene(sceneTarget, LoadSceneMode.Single);
    }
}
