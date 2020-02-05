using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
        int leftUnits, rightUnits, upUnits, downUnits;
    [SerializeField]
        GameObject enemy;
    private float distanceToPlayer;
    //private GameObject enemySpawn;
    

    void Start()
    {
        GameObject spawnedEnemy = Instantiate(enemy, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(GameObject.Find("Player").transform.position, transform.position);
        /*switch(enemy)
        {
            case 0: enemySpawn = GameObject.Find("GelatinousCube");
                break;
            case 1: enemySpawn = GameObject.Find("Flumph");
                break;
            case 2: enemySpawn = GameObject.Find("DragonTurtle");
                break;
            case 3: enemySpawn = GameObject.Find("RockGolem");
                break;
        }*/

    }
}
