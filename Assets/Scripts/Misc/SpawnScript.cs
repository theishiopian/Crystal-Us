using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    public GameObject enemy;
    private float distanceToPlayer;
    Vector2 spawnLoc;


    void Start()
    {
        spawnLoc = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        GameObject spawnedEnemy = Instantiate(enemy, spawnLoc, Quaternion.identity, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(GameObject.Find("Player").transform.position, transform.position);
        
        if(transform.childCount == 0 && distanceToPlayer > 30)
        {
           GameObject spawnedEnemy = Instantiate(enemy, spawnLoc, Quaternion.identity, gameObject.transform);
        }
    }
}
