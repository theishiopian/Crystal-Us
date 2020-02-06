using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
        float leftUnits = 0, rightUnits = 0, upUnits = 0, downUnits = 0;
    [SerializeField]
        GameObject enemy;
    private float distanceToPlayer;

    

    void Start()
    {
        GameObject spawnedEnemy = Instantiate(enemy, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(GameObject.Find("Player").transform.position, transform.position);
        
        if(transform.childCount == 0 && distanceToPlayer > 30)
        {
            Vector2 spawnLoc = new Vector2(gameObject.transform.position.x + Random.Range(leftUnits * -1, rightUnits), gameObject.transform.position.y + Random.Range(downUnits * -1, upUnits));
            GameObject spawnedEnemy = Instantiate(enemy, spawnLoc, Quaternion.identity, gameObject.transform);
        }
    }
}
